using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using AutoMapper;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.Services;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.WebForms.UserControls;
using Button = System.Web.UI.WebControls.Button;
using classes = Sebrae.Academico.Dominio.Classes;
using Label = System.Web.UI.WebControls.Label;
using ListControl = System.Web.UI.WebControls.ListControl;

namespace Sebrae.Academico.WebForms.Cadastros.Matricula
{
    public partial class MatriculasAluno : PageBase
    {
        private readonly ManterUsuario _manterUsuario = new ManterUsuario();

        // Numero de registros por página de matriculas oferta
        private const int QuantidadeMatriculasPorPagina = 30;

        protected override enumFuncionalidade Funcionalidade
        {
            get { return enumFuncionalidade.GerenciamentoMatricula; }
        }

        public bool InModoDeAvaliacao
        {
            get
            {
                if (ViewState["InModoDeAvaliacao"] != null)
                {
                    return (bool)ViewState["InModoDeAvaliacao"];
                }

                return false;
            }
            set
            {
                ViewState["InModoDeAvaliacao"] = value;
            }
        }

        public int IdMatriculaTurma
        {
            get
            {
                if (ViewState["ViewStateIdMatriculaTurma"] != null)
                {
                    return (int)ViewState["ViewStateIdMatriculaTurma"];
                }

                return 0;
            }
            set { ViewState["ViewStateIdMatriculaTurma"] = value; }

        }

        protected override IList<enumPerfil> PerfisNecessariosParaAcessarAFuncionalidade
        {
            get
            {
                IList<enumPerfil> perfisNecessariosParaAcessarAFuncionalidade = new List<enumPerfil>();
                perfisNecessariosParaAcessarAFuncionalidade.Add(enumPerfil.Administrador);
                return perfisNecessariosParaAcessarAFuncionalidade;
            }
        }
  
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    // Previnindo evento de clique duplo
                    WebFormHelper.PrevinirCliqueDuplo(new List<Button>()
                    {
                        btnFiltrarMatriculas    
                    }, this);

                    WebFormHelper.PreencherComponenteComOpcoesSimNao(rblGerenciarMatriculasAbertas);
                    WebFormHelper.SetarValorNoRadioButtonList(true, rblGerenciarMatriculasAbertas);
                }
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            consultarMatriculasUsuario();
        }
        
        private void consultarMatriculasUsuario()
        {
            var listaOfertas = rblGerenciarMatriculasAbertas.SelectedValue == "S"
                ? new ManterOferta().ObterTodasIQueryable().Where(x => x.DataFimInscricoes != null && x.DataFimInscricoes >= DateTime.Now)
                : new ManterOferta().ObterTodasIQueryable().Where(x => x.DataFimInscricoes != null && x.DataFimInscricoes < DateTime.Now);

            if (ucLupaUsuario.SelectedUser != null)
            {
                var usuario = new classes.Usuario();
                usuario = ucLupaUsuario.SelectedUser;

                var bmMatriculasAluno = (new BMMatriculaTurma()).ObterPorUsuario(usuario.ID)
                    .Select(x => x.MatriculaOferta)
                    .Where(x => listaOfertas.Select(o => o.ID).Contains(x.Oferta.ID));

                var matriculasAluno = bmMatriculasAluno.ToList();

                WebFormHelper.PreencherGrid(matriculasAluno, dgvMatriculaOferta);
                pnlMatricula.Visible = true;
            }
            else
            {
                pnlMatricula.Visible = false;
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Selecione um Usuário");
            }
        }

        protected void lkbAlunosMatriculados_Click(object sender, EventArgs e)
        {
            AlterarStatusTab(lkbAlunosMatriculados, collapseMatriculados);
        }
        
        public IList<classes.StatusMatricula> ListaStatusMatricula { get; set; }

        protected void ddlStatusOferta_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (InModoDeAvaliacao)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Não é possível alterar o Status desta matrícula diretamente. Ela precisa passar pela avaliação de um Consultor Educacional e ser aprovada por um Gestor.");
                return;
            }
            
            var comboStatusOferta = (DropDownList)sender;

            if (comboStatusOferta != null && comboStatusOferta.SelectedItem != null &&
                !string.IsNullOrWhiteSpace(comboStatusOferta.SelectedItem.Value))
            {
                try
                {
                    //Obtém a linha da grid onde a dropdownlist (combo) de oferta, está contida
                    var linhadaGrid = (GridViewRow)comboStatusOferta.NamingContainer;

                    if (linhadaGrid == null) return;

                    var ddlStatusOferta = (DropDownList)linhadaGrid.FindControl("ddlStatusOferta");

                    if (ddlStatusOferta == null) return;

                    var hdfIdMatriculaOferta = (HiddenField)linhadaGrid.FindControl("hdfIdMatriculaOferta");
                    var manterMatriculaOferta = new ManterMatriculaOferta();
                    var ddlTurma = (DropDownList)linhadaGrid.FindControl("ddlTurma");

                    var statusMatriculaOferta = (enumStatusMatricula)Enum.Parse(typeof(enumStatusMatricula), ddlStatusOferta.SelectedItem.Value);

                    int IdMatriculaOferta;
                    classes.MatriculaOferta matriculaOferta = null;
                    if (int.TryParse(hdfIdMatriculaOferta.Value, out IdMatriculaOferta))
                        matriculaOferta = manterMatriculaOferta.ObterMatriculaOfertaPorID(IdMatriculaOferta);

                    var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();
                    var matriculaTurma = matriculaOferta.MatriculaTurma.FirstOrDefault();

                    if (statusMatriculaOferta.Equals(enumStatusMatricula.CanceladoGestor) && !usuarioLogado.IsGestor())
                    {
                        throw new AcademicoException("Apenas o gestor UC pode alterar para este perfil");
                    }

                    if (statusMatriculaOferta.Equals(enumStatusMatricula.CanceladoAdm) && !usuarioLogado.IsAdministrador())
                    {
                        throw new AcademicoException("Apenas o Administrador pode alterar para este perfil");
                    }

                    if (statusMatriculaOferta.Equals(enumStatusMatricula.Inscrito))
                    {
                        if (string.IsNullOrEmpty(ddlTurma.SelectedValue) || int.Parse(ddlTurma.SelectedValue) <= 0)
                        {
                            throw new AcademicoException("Selecione uma turma para alterar a inscrição");
                        }

                        try
                        {
                            new ManterSolucaoEducacional().ValidarPreRequisitosDaMatricula(matriculaOferta);
                        }
                        catch (Exception)
                        {
                            AtualizarStatusDaOferta(enumStatusMatricula.Inscrito, matriculaOferta, ref manterMatriculaOferta);
                            throw;
                        }
                    }

                    if (statusMatriculaOferta.Equals(enumStatusMatricula.CanceladoAluno))
                    {
                        //O usuário do admin, pode ver o status do Cancelado/Aluno, mas nunca pode setar esse status
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Atencao,
                            "Apenas o aluno pode atribuir o status de cancelado pelo aluno");
                    }
                    else
                    {                        
                        if (matriculaTurma == null)
                        {
                            WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "O aluno precisa estar vinculado a uma turma.");
                            return;
                        }

                        if (matriculaOferta.StatusMatricula != enumStatusMatricula.Inscrito && statusMatriculaOferta == enumStatusMatricula.Inscrito)
                        {
                            ExibirModalConfirmacaoStatusInscrito(matriculaTurma);
                        }
                        else
                        {
                            ExibirModalDataConclusao(matriculaTurma, statusMatriculaOferta);
                        }
                    }

                    // Atualizar lista de status disponíveis, pois pode sofrer alteração caso haja
                    // o status de Cancelado\Turma, que possui um comportamento especial.
                    SetarListaComStatusDeMatricula(matriculaOferta);

                    // Trata a exibição da listagem de Status novamente, para manter os mesmos Status,
                    // com as mesmas formas.
                    TratarComboStatusOferta(ddlStatusOferta, matriculaOferta);
                    
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
                catch
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Erro Interno no servidor.");
                }
            }
        }

        private void SetarListaComStatusDeMatricula(classes.MatriculaOferta matriculaOferta)
        {
            if ((ListaStatusMatricula == null || !ListaStatusMatricula.Any()))
            {
                var categoriaConteudo = new ManterSolucaoEducacional().ObterSolucaoEducacionalPorId(matriculaOferta.Oferta.SolucaoEducacional.ID).CategoriaConteudo;

                var listaStatusMatricula = (new ManterStatusMatricula()).ObterStatusMatriculaPorCategoriaConteudo(categoriaConteudo)
                    .Where(p => p.ID != (int)enumStatusMatricula.Reprovado).ToList();

                var manterStatusMatricula = new ManterStatusMatricula();

                var status = manterStatusMatricula.ObterStatusMatriculaPorCategoriaConteudo(categoriaConteudo)
                    .Where(p => p.ID != (int)enumStatusMatricula.Reprovado);

                var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

                // Filtrar Status pela visualização do usuário.
                status = status.Where(x => x.PermiteVisualizacao(usuarioLogado));

                ListaStatusMatricula = status.ToList();
            }
        }

        protected void rblGerenciarMatriculasAbertas_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlMatricula.Visible = false;
        }

        protected void ddlTurma_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var comboTurma = (DropDownList)sender;
                //Se Selecionou uma turma na combo, atualiza ou insere a matricula do usuário nesta turma.
                if (comboTurma != null && comboTurma.SelectedItem != null && !string.IsNullOrWhiteSpace(comboTurma.SelectedItem.Value))
                {
                    MatricularAlunoNaTurma(comboTurma);
                }
                else
                {
                    /* Quando o usuário escolher a turma vazia, ou seja, a opção "Selecione",
                   um alert deverá avisar que os dados do aluno como nota e presença serão perdidos. */
                    //comboTurma.Attributes.Add("OnClientClick="return confirm('Deseja Realmente Excluir este Registro?');"
                    ExcluirAlunoDaTurma(comboTurma);

                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "O usuário foi excluído da turma");
                }
            }
            catch (PoliticaConsequenciaException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
            catch
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Erro Interno no servidor.");
            }
        }

        private void ExcluirAlunoDaTurma(DropDownList comboTurma)
        {
            //Obtém a linha da grid onde a dropdownlist (combo) de oferta, está contida
            var linhadaGrid = (GridViewRow)comboTurma.NamingContainer;

            if (linhadaGrid.RowType == DataControlRowType.Header) linhadaGrid.TableSection = TableRowSection.TableHeader;

            if (linhadaGrid != null)
            {
                var hdfIdMatriculaOferta = (HiddenField)linhadaGrid.FindControl("hdfIdMatriculaOferta");

                int IdMatriculaOferta;
                var manterMatriculaOferta = new ManterMatriculaOferta();
                classes.MatriculaOferta matriculaOferta = null;
                if (int.TryParse(hdfIdMatriculaOferta.Value, out IdMatriculaOferta))
                    matriculaOferta = manterMatriculaOferta.ObterMatriculaOfertaPorID(IdMatriculaOferta);

                if (matriculaOferta != null && matriculaOferta.MatriculaTurma != null &&
                    matriculaOferta.MatriculaTurma.Count > 0)
                {
                    matriculaOferta.MatriculaTurma.Clear();
                    AtualizarStatusDaOferta(matriculaOferta.StatusMatricula, matriculaOferta, ref manterMatriculaOferta);
                }
            }
        }

        protected void dgvMatriculaOferta_PreRender(object sender, EventArgs e)
        {
            GridView gv = (GridView)sender;
            WebFormHelper.PreRenderGridView(gv);
        }

        protected void dgvMatriculaOferta_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header) e.Row.TableSection = TableRowSection.TableHeader;

            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                TratarComboTurma(e);

                var ddlStatusOferta = (DropDownList)e.Row.FindControl("ddlStatusOferta");
                var matriculaOferta = (classes.MatriculaOferta)e.Row.DataItem;
                var statusOferta = (Label)e.Row.FindControl("statusOferta");

                TratarComboStatusOferta(ddlStatusOferta, matriculaOferta, statusOferta);

                //Se ID da Matricula Oferta > 0, a matricula ja foi realizada
                if (matriculaOferta != null && matriculaOferta.ID > 0)
                {
                    var hdfIdMatriculaOferta = (HiddenField)e.Row.FindControl("hdfIdMatriculaOferta");

                    if (hdfIdMatriculaOferta != null)
                        hdfIdMatriculaOferta.Value = matriculaOferta.ID.ToString();

                    var lkbEditarMatriculaTurma = (LinkButton)e.Row.FindControl("lkbEditarMatriculaTurma");
                    var lkbEnviarMatricula = (LinkButton)e.Row.FindControl("lkbEnviarMatricula");
                    var lkbEnviarMatriculaEmailPendente =
                        (LinkButton)e.Row.FindControl("lkbEnviarMatriculaEmailPendente");
                    var ddlTurma = (DropDownList)e.Row.FindControl("ddlTurma");

                    //Exibe o botão editar, caso o usuário esteja matriculado em alguma turma
                    if (matriculaOferta.MatriculaTurma.Any())
                    {
                        ddlTurma.Enabled = !matriculaOferta.StatusMatricula.Equals(enumStatusMatricula.CanceladoAdm);

                        if (lkbEditarMatriculaTurma != null)
                        {
                            classes.MatriculaTurma matriculaTurma = null;

                            var permissaoEditarGestor = matriculaOferta.Oferta.AlteraPeloGestorUC;

                            if (matriculaOferta.MatriculaTurma != null && matriculaOferta.MatriculaTurma.Count > 0)
                                matriculaTurma = matriculaOferta.MatriculaTurma.FirstOrDefault();

                            if (InModoDeAvaliacao)
                            {
                                if (matriculaTurma != null)
                                {
                                    var avaliacao = matriculaTurma.Turma.Avaliacoes.FirstOrDefault();

                                    if (avaliacao != null)
                                    {
                                        if (avaliacao.Status == enumStatusAvaliacao.AguardandoResposta)
                                        {
                                            lkbEditarMatriculaTurma.Visible = true;
                                            lkbEditarMatriculaTurma.CommandName = "avaliarTurma";
                                            lkbEditarMatriculaTurma.CommandArgument = matriculaTurma.ID.ToString();
                                        }
                                        else
                                        {
                                            lkbEditarMatriculaTurma.Visible = true;
                                            lkbEditarMatriculaTurma.CommandName = "exibirDetalhes";
                                            lkbEditarMatriculaTurma.CommandArgument = matriculaTurma.ID.ToString();
                                        }
                                    }
                                    else
                                    {
                                        lkbEditarMatriculaTurma.Visible = true;
                                        lkbEditarMatriculaTurma.CommandName = "exibirDetalhes";
                                        lkbEditarMatriculaTurma.CommandArgument = matriculaTurma.ID.ToString();
                                    }
                                }
                            }
                            else
                            {
                                if (matriculaTurma != null &&
                                    (permissaoEditarGestor == true || _manterUsuario.PerfilAdministrador()))
                                {
                                    lkbEditarMatriculaTurma.CommandArgument = matriculaTurma.ID.ToString();
                                    lkbEditarMatriculaTurma.Visible = true;
                                }
                                else
                                {
                                    lkbEditarMatriculaTurma.Visible = false;
                                }
                            }
                        }
                        if (lkbEnviarMatricula != null)
                        {
                            lkbEnviarMatricula.Visible = false;

                            if (matriculaOferta.Oferta.SolucaoEducacional.Fornecedor.ID == (int)enumFornecedor.WebAula ||
                                matriculaOferta.Oferta.SolucaoEducacional.Fornecedor.ID ==
                                (int)enumFornecedor.MoodleSebrae ||
                                matriculaOferta.Oferta.SolucaoEducacional.Fornecedor.ID == (int)enumFornecedor.FGVOCW)
                            {
                                if (!matriculaOferta.FornecedorNotificado &&
                                    matriculaOferta.StatusMatricula == enumStatusMatricula.Inscrito &&
                                    matriculaOferta.MatriculaTurma != null && matriculaOferta.MatriculaTurma.Count > 0)
                                {
                                    lkbEnviarMatricula.Visible = true;
                                    lkbEnviarMatricula.CommandArgument = matriculaOferta.ID.ToString();
                                }
                            }
                        }

                        var lbEmitirCertificado = (LinkButton)e.Row.FindControl("lbEmitirCertificado");

                        lbEmitirCertificado.Visible = false;

                        if (matriculaOferta.IsAprovado() && matriculaOferta.Oferta.CertificadoTemplate != null)
                        {
                            lbEmitirCertificado.CommandArgument = matriculaOferta.ID.ToString();
                            lbEmitirCertificado.Visible = true;
                        }
                    }
                }
            }
        }

        protected void dgvMatriculaOferta_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var idTurma = int.Parse(e.CommandArgument.ToString());

            if (e.CommandName.Equals("emitirCertificado"))
            {
                var idMatriculaOferta = int.Parse(e.CommandArgument.ToString());

                var matriculaOferta = new ManterMatriculaOferta().ObterMatriculaOfertaPorID(idMatriculaOferta);

                new ucCursos().EmitirCertificado(matriculaOferta);
            }

            if (e.CommandName.Equals("editarMatTurma"))
            {
                try
                {
                    TratarEdicaoDeUmaMatriculaTurma(e);
                    ExibirModalMatriculaTurma();
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }
            }
        }

        private void TratarEdicaoDeUmaMatriculaTurma(GridViewCommandEventArgs e)
        {
            var idMatriculaTurma = int.Parse(e.CommandArgument.ToString());
            ExibirInformacoesDeUmaMatriculaTurma(idMatriculaTurma);
        }

        private void ExibirInformacoesDeUmaMatriculaTurma(int idMatriculaTurma)
        {
            var matriculaTurma = new ManterMatriculaTurma().ObterMatriculaTurmaPorId(idMatriculaTurma);

            if (matriculaTurma != null)
            {
                CarregarDadosDaMatriculaTurmaDeUmAluno(matriculaTurma);
            }
        }

        private void CarregarDadosDaMatriculaTurmaDeUmAluno(classes.MatriculaTurma matriculaTurma)
        {
            PreencherCamposDoCadastroDeMatriculaTurma(matriculaTurma);
        }

        private void PreencherCamposDoCadastroDeMatriculaTurma(classes.MatriculaTurma matriculaTurma)
        {
            var usuario = matriculaTurma.MatriculaOferta.Usuario;
            txtNomeModalMatriculaTurma.Text = usuario.Nome;
            txtUfModalMatriculaTurma.Text = usuario.UF.Nome;
            txtCpfModalMatriculaTurma.Text = usuario.CPF;
            txtEmailModalMatriculaTurma.Text = usuario.Email;
            ucMatriculaTurma1.PreencherCamposDaMatriculaTurma(matriculaTurma);
        }

        /*
        private void PrepararTelaDeTurmaParaEdicao(int idTurma)
        {
            ucTurma1.LimparCampos();
            ucTurma1.CarregarLoad = true;
            ucTurma1.CarregarDados();
            ucTurma1.EsconderBotaoSalvar();


            var turma = ucTurma1.ObterTurma(idTurma);

            ucTurma1.PreencherCampos(turma);

            ExibirModalTurma();
        }
        */
        //Atualiza o status da mátricula no curso do moodle do usuário
        private void AtualizarStatusUsuarioTurma(MatriculaTurma matriculaTurma)
        {

            var matriculaOferta = matriculaTurma.MatriculaOferta.Oferta;

            if (matriculaOferta.SolucaoEducacional.Fornecedor.ID == (int)enumFornecedor.MoodleSebrae)
            {
                try
                {

                    var response = new Sebrae.Academico.BP.Services.SgusWebService.ManterSolucaoEducacionalService().AtualizarStatusMatriculaAlunoMoodle(
                            matriculaTurma.MatriculaOferta.Usuario.CPF,
                            matriculaOferta.IDChaveExterna,
                            (int)matriculaTurma.MatriculaOferta.StatusMatricula);

                }
                catch (Exception e)
                {
                    throw new AcademicoException("Erro ao tentar atualizar staus no Moodle " + e.Message);
                }
            }
        }
        private void TratarComboStatusOferta(DropDownList ddlStatusOferta, classes.MatriculaOferta matriculaOferta, Label statusOferta = null)
        {
            var permiteAlteracao = matriculaOferta.Oferta.AlteraPeloGestorUC;

            // Caso esteja em modo de avaliação, só adiciona o Status atual da matrícula e esconde o dropdown.
            if (!InModoDeAvaliacao && (permiteAlteracao == true || _manterUsuario.PerfilAdministrador()))
            {
                if (ddlStatusOferta != null)
                {
                    var categoriaConteudo = new ManterSolucaoEducacional().ObterSolucaoEducacionalPorId(matriculaOferta.Oferta.SolucaoEducacional.ID).CategoriaConteudo;

                    var listaStatusMatricula = (new ManterStatusMatricula()).ObterStatusMatriculaPorCategoriaConteudo(categoriaConteudo)
                        .Where(p => p.ID != (int)enumStatusMatricula.Reprovado).ToList();

                    // Obter lista usando AutoMapper para não alterar a lista original com a adição
                    // do status "Cancelado\Turma" abaixo.
                    Mapper.Map(ListaStatusMatricula, listaStatusMatricula);

                    if (matriculaOferta.StatusMatricula == enumStatusMatricula.CanceladoTurma)
                    {
                        var cancelado =
                            new ManterStatusMatricula().ObterStatusMatriculaPorID(
                                (int)enumStatusMatricula.CanceladoTurma);

                        listaStatusMatricula.Add(cancelado);

                        listaStatusMatricula = listaStatusMatricula.OrderBy(x => x.Nome).ToList();
                    }

                    // Caso o status atual não exista na lista de status disponíveis, insere ele na lista.
                    if (!listaStatusMatricula.Select(x => x.ID).Contains((int)matriculaOferta.StatusMatricula))
                    {
                        var statusAtual =
                            new ManterStatusMatricula().ObterStatusMatriculaPorID((int)matriculaOferta.StatusMatricula);

                        listaStatusMatricula.Add(statusAtual);

                        // Reordenar a lista.
                        listaStatusMatricula = listaStatusMatricula.OrderBy(x => x.Nome).ToList();
                    }

                    WebFormHelper.PreencherLista(listaStatusMatricula, ddlStatusOferta);

                    // Desabilitar a opção de cancelamento.
                    if (matriculaOferta.StatusMatricula == enumStatusMatricula.CanceladoTurma)
                        ddlStatusOferta.Items.FindByValue(((int)enumStatusMatricula.CanceladoTurma).ToString()).Attributes.Add("disabled", "disabled");

                    var idStatusMatricula = (int)matriculaOferta.StatusMatricula;

                    WebFormHelper.SetarValorNaCombo(idStatusMatricula.ToString(), ddlStatusOferta);
                }
            }
            else
            {
                ddlStatusOferta.Visible = false;

                if (statusOferta != null)
                {
                    statusOferta.Visible = true;
                    statusOferta.Text = matriculaOferta.StatusMatriculaFormatado;
                }
            }
        }

        private void TratarComboTurma(GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header) e.Row.TableSection = TableRowSection.TableHeader;

            //Procura o dropdownlist de turma nas linhas da grid
            var ddlTurma = (DropDownList)e.Row.FindControl("ddlTurma");

            //Se encontrou o dropdownlist de turma, seta o status da matricula turma com o status da oferta
            if (ddlTurma != null)
            {
                var matriculaOferta = (classes.MatriculaOferta)e.Row.DataItem;

                if (matriculaOferta != null)
                {
                    // Preencher dropdown de turmas de acordo com a seleção do usuário.
                    PreencherDropDownTurma(ddlTurma, matriculaOferta);

                    classes.MatriculaTurma matriculaTurma = null;

                    if (matriculaOferta.MatriculaTurma == null && matriculaOferta.Oferta != null &&
                        matriculaOferta.Oferta.ListaTurma.Count > 0)
                    {
                        var lista = new List<classes.MatriculaTurma>();
                        var mTurma =
                            new ManterMatriculaTurma().ObterMatriculaTurmaPorIdUsuarioIdTurma(
                                matriculaOferta.Usuario.ID, matriculaOferta.Oferta.ListaTurma.FirstOrDefault().ID);

                        if (mTurma != null)
                            lista.Add(mTurma);

                        matriculaOferta.MatriculaTurma = lista;
                    }

                    if (matriculaOferta.MatriculaTurma != null && matriculaOferta.MatriculaTurma.Count > 0)
                        matriculaTurma = matriculaOferta.MatriculaTurma.FirstOrDefault();

                    if (matriculaTurma != null)
                    {
                        WebFormHelper.SetarValorNaCombo(matriculaTurma.Turma.ID.ToString(), ddlTurma);
                    }
                }
            }
        }

        protected void btnSimModalConfirmacaoStatusInscrito_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(hdfModalConfirmacaoStatusInscritoIdMatriculaTurma.Value))
                {
                    var manterMatriculaTurma = new ManterMatriculaTurma();
                    var idMatriculaTurma = Convert.ToInt32(hdfModalConfirmacaoStatusInscritoIdMatriculaTurma.Value);
                    var matriculaTurma = manterMatriculaTurma.ObterMatriculaTurmaPorId(idMatriculaTurma);
                    if (matriculaTurma != null)
                    {
                        matriculaTurma.DataTermino = null;

                        manterMatriculaTurma.Salvar(matriculaTurma);

                        var manterMatriculaOferta = new ManterMatriculaOferta();
                        var matriculaOferta =
                            manterMatriculaOferta.ObterMatriculaOfertaPorID(matriculaTurma.MatriculaOferta.ID);

                        AtualizarStatusDaOferta(enumStatusMatricula.Inscrito, matriculaOferta, ref manterMatriculaOferta);

                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso,
                            "O Status da Matrícula da Turma foi Atualizado com Sucesso !");

                        consultarMatriculasUsuario();
                        AtualizarStatusUsuarioTurma(matriculaTurma);
                    }
                }   

                OcultarModalConfirmacaoStatusInscrito();
            }
            catch (PoliticaConsequenciaException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        protected void btnSalvarModalDataConclusao_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(hdfModalDataConclusaoIdMatriculaTurma.Value))
                {
                    var manterMatriculaTurma = new ManterMatriculaTurma();
                    var idMatriculaTurma = Convert.ToInt32(hdfModalDataConclusaoIdMatriculaTurma.Value);
                    var matriculaTurma = manterMatriculaTurma.ObterMatriculaTurmaPorId(idMatriculaTurma);

                    var dataConclusao = DateTime.Now;

                    if (!DateTime.TryParse(txtModalDataConclusao.Text, out dataConclusao))
                    {
                        throw new AcademicoException("Informe uma data válida para conclusão do aluno.");
                    }

                    matriculaTurma.DataTermino = dataConclusao;

                    var data = matriculaTurma.Turma.DataFinal;

                    if (data.HasValue)
                    {
                        if (matriculaTurma.DataTermino.Value.Date > data.Value.Date)
                        {
                            throw new AcademicoException(string.Format("Data de conclusão não pode ser maior que a data final da turma ({0}).", data.Value.ToString("dd/MM/yyyy")));
                        }
                    }

                    manterMatriculaTurma.AlterarMatriculaTurma(matriculaTurma);

                    var manterMatriculaOferta = new ManterMatriculaOferta();
                    var matriculaOferta = manterMatriculaOferta.ObterMatriculaOfertaPorID(matriculaTurma.MatriculaOferta.ID);

                    AtualizarStatusDaOferta((enumStatusMatricula)Convert.ToInt32(hdfModalDataConclusaoIdStatusMatriculaOferta.Value), matriculaOferta, ref manterMatriculaOferta);

                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "O Status da Matrícula da Turma foi Atualizado com Sucesso !");

                    consultarMatriculasUsuario();

                    AtualizarStatusUsuarioTurma(matriculaTurma);
                }

                OcultarModalDataConclusao();
            }
            catch (PoliticaConsequenciaException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        protected void MatricularAlunoNaTurma_MatriculouAlunoEmUmaTurma(object sender, MatricularAlunoEmUmaTurmaEventArgs e)
        {
            OcultarModalMatriculaTurma();
        }

        protected void SelecionarProvaDeUmAluno_SelecionouProvaDeUmAluno(object sender, SelecionarProvaDeUmAlunoEventArgs e)
        {
            OcultarModalMatriculaTurma();
            //Chama o usercontrol de Detalhes
            ExibirInformacoesDetalhadasDaProva(e.ProvaSelecionada.ID);
            pnlUcInformacoesDetalhadasProvasRealizadas.Visible = true;
            ucExibirQuestionarioResposta.Visible = true;
        }

        private void ExibirInformacoesDetalhadasDaProva(int idQuestionarioParticipacao)
        {
            var questionarioParticipacao =
                new ManterQuestionarioParticipacao().ObterQuestionarioParticipacaoPorId(idQuestionarioParticipacao);

            if (questionarioParticipacao != null)
            {
                CarregarInformacoesDetalhadasDaProvaDoAluno(questionarioParticipacao);
            }
        }

        private void CarregarInformacoesDetalhadasDaProvaDoAluno(classes.QuestionarioParticipacao questionarioParticipacao)
        {
            if (questionarioParticipacao != null)
            {
                ucExibirQuestionarioResposta.ExibirInformacoesDetalhadasDaProvaDoAluno(questionarioParticipacao, 0);
            }
        }

        protected void OcultarInformacoesDetalhadasDaProvaRealizada_Click(object sender, EventArgs e)
        {
            OcultarModalInformacoesDetalhadasDaProvaRealizada();
            ExibirInformacoesDeUmaMatriculaTurma(IdMatriculaTurma);
            pnlModalMatriculaTurma.Visible = true;
            ucMatriculaTurma1.Visible = true;
        }

        protected void ExibirResposta_ExibiuResposta(object sender,
            ucExibirQuestionarioResposta.DetalheDaRespostaDaProvaEventArgs e)
        {
            OcultarModalInformacoesDetalhadasDaProvaRealizada();
            ExibirInformacoesDeUmaMatriculaTurma(e.IdMatriculaTurma);
            pnlModalMatriculaTurma.Visible = true;
            ucMatriculaTurma1.Visible = true;
        }

        private void OcultarModalInformacoesDetalhadasDaProvaRealizada()
        {
            OcultarBackDrop();
            pnlUcInformacoesDetalhadasProvasRealizadas.Visible = false;
        }

        private void OcultarModalDataConclusao()
        {
            OcultarBackDrop();

            txtModalDataConclusao.Text = "";
            hdfModalDataConclusaoIdMatriculaOferta.Value = "";
            hdfModalDataConclusaoIdMatriculaTurma.Value = "";
            hdfModalDataConclusaoIdStatusMatriculaOferta.Value = "";

            pnlModalDataConclusao.Visible = false;
        }

        protected void OcultarModalDataConclusao_Click(object sender, EventArgs e)
        {
            OcultarModalDataConclusao();
        }

        private void OcultarModalConfirmacaoStatusInscrito()
        {
            OcultarBackDrop();

            hdfModalConfirmacaoStatusInscritoIdMatriculaOferta.Value = "";
            hdfModalConfirmacaoStatusInscritoIdMatriculaTurma.Value = "";

            pnlModalConfirmacaoStatusInscrito.Visible = false;
        }

        protected void OcultarModalConfirmacaoStatusInscrito_Click(object sender, EventArgs e)
        {
            OcultarModalConfirmacaoStatusInscrito();
        }

        protected void OcultarMatriculaTurma_Click(object sender, EventArgs e)
        {
            OcultarModalMatriculaTurma();
        }

        private void OcultarModalMatriculaTurma()
        {
            OcultarBackDrop();
            pnlModalMatriculaTurma.Visible = false;
        }

        protected void lnkPage_Click(object sender, EventArgs e)
        {
            var linkButton = sender as LinkButton;

            if (linkButton == null) return;

            var pageIndex = int.Parse(linkButton.CommandArgument);

            //BuscarMatriculaOferta(pageIndex);
        }

        private void PreencherDropDownTurma(DropDownList ddlTurma, classes.MatriculaOferta matriculaOferta)
        {
            var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

            // Preencher dropdown de turmas de acordo com a seleção do usuário e com as permissões de visualização.
            if (usuarioLogado.IsAdministrador())
            {
                WebFormHelper.PreencherLista(matriculaOferta.Oferta.ListaTurma, ddlTurma, false, true);
            }
            else if (matriculaOferta.Oferta.SolucaoEducacional.Ativo &&
                     matriculaOferta.Oferta.SolucaoEducacional.UFGestor.ID == usuarioLogado.UF.ID)
            {
                WebFormHelper.PreencherLista(matriculaOferta.Oferta.ListaTurma, ddlTurma, false, true);
            }
        }

        private void MatricularAlunoNaTurma(DropDownList comboTurma)
        {
            //Obtém a linha da grid onde a dropdownlist (combo) de oferta, está contida
            var linhadaGrid = (GridViewRow)comboTurma.NamingContainer;

            if (linhadaGrid != null)
            {
                var hdfIdMatriculaOferta = (HiddenField)linhadaGrid.FindControl("hdfIdMatriculaOferta");

                int IdMatriculaOferta;
                classes.MatriculaOferta matriculaOferta = null;
                if (int.TryParse(hdfIdMatriculaOferta.Value, out IdMatriculaOferta))
                    matriculaOferta = new ManterMatriculaOferta().ObterMatriculaOfertaPorID(IdMatriculaOferta);

                if (matriculaOferta != null)
                {
                    var matriculaTurma = ObterObjetoMatriculaTurma(comboTurma, matriculaOferta);

                    var manterMatriculaOferta = new ManterMatriculaOferta();

                    if (matriculaOferta.MatriculaTurma.Count == 0)
                        matriculaOferta.MatriculaTurma.Add(matriculaTurma);
                    else
                        matriculaOferta.MatriculaTurma[0] = matriculaTurma;

                    manterMatriculaOferta.AtualizarMatriculaOferta(matriculaOferta, false);


                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso,
                        string.Format("O usuário '{0}' foi matriculado na turma '{1}'",
                            matriculaOferta.Usuario.Nome, matriculaTurma.Turma.Nome));
                }
            }
        }

        public classes.MatriculaTurma ObterObjetoMatriculaTurma(DropDownList comboTurma, classes.MatriculaOferta matriculaOferta)
        {
            classes.MatriculaTurma matriculaTurma = null;
            if (matriculaOferta.MatriculaTurma != null && matriculaOferta.MatriculaTurma.Count > 0)
                matriculaTurma = matriculaOferta.MatriculaTurma.FirstOrDefault();

            //Se o usuário não estiver matriculado em nenhuma turma, preenche o objeto matricula turma com os dados da oferta.
            if (matriculaTurma == null)
            {
                int IdTurma = 0;
                int.TryParse(comboTurma.SelectedItem.Value, out IdTurma);

                matriculaTurma = new classes.MatriculaTurma
                {
                    MatriculaOferta = matriculaOferta,
                    Turma = IdTurma > 0 ? new ManterTurma().ObterTurmaPorID(IdTurma) : null,
                    DataMatricula = DateTime.Now
                };

                matriculaTurma.DataLimite = matriculaTurma.CalcularDataLimite();
            }
            else
            {
                //Troca a turma, pois o usuário informou uma nova turma
                int idTurmaEscolhidaNaCombo;

                if (int.TryParse(comboTurma.SelectedItem.Value, out idTurmaEscolhidaNaCombo) && !matriculaTurma.ID.Equals(idTurmaEscolhidaNaCombo))
                {
                    matriculaTurma.TurmaAnterior = matriculaTurma.Turma;

                    /* Troca a Turma do usuário (ou seja, matricula o aluno em uma nova turma), 
                       pois ele escolheu uma nova turma na combo.*/

                    matriculaTurma.Turma = new ManterTurma().ObterTurmaPorID(idTurmaEscolhidaNaCombo);
                }
            }

            return matriculaTurma;
        }

        private void ExibirModalMatriculaTurma()
        {
            ExibirBackDrop();
            pnlModalMatriculaTurma.Visible = true;
        }

        private void ExibirModalConfirmacaoStatusInscrito(classes.MatriculaTurma matriculaTurma)
        {
            ExibirBackDrop();

            pnlModalConfirmacaoStatusInscrito.Visible = true;

            hdfModalConfirmacaoStatusInscritoIdMatriculaOferta.Value = matriculaTurma.MatriculaOferta.ID.ToString();
            hdfModalConfirmacaoStatusInscritoIdMatriculaTurma.Value = matriculaTurma.ID.ToString();
        }

        private void ExibirModalDataConclusao(classes.MatriculaTurma matriculaTurma, enumStatusMatricula statusMatriculaOferta)
        {
            ExibirBackDrop();
            pnlModalDataConclusao.Visible = true;

            txtModalDataConclusao.Text = matriculaTurma.DataTermino.HasValue ? matriculaTurma.DataTermino.Value.ToString("dd/MM/yyyy") : "";
            hdfModalDataConclusaoIdMatriculaTurma.Value = matriculaTurma.ID.ToString();
            hdfModalDataConclusaoIdStatusMatriculaOferta.Value = ((int)statusMatriculaOferta).ToString();
        }

        private void AtualizarStatusDaOferta(enumStatusMatricula statusMatriculaOferta,
            classes.MatriculaOferta matriculaOferta, ref ManterMatriculaOferta manterMatriculaOferta,
            DateTime? dataConclusao = null, double? notaFinal = null, bool atualizarMatriculas = true,
            bool fazerMerge = false)
        {

            if (manterMatriculaOferta == null)
            {
                // O objetivo que for salvo pelo Manter não deve vir de outra sessão (outro Manter/BM)
                manterMatriculaOferta = new ManterMatriculaOferta();
                matriculaOferta = manterMatriculaOferta.ObterMatriculaOfertaPorID(matriculaOferta.ID);
            }

            if (matriculaOferta == null) return;

            //Atualiza o status da Oferta
            matriculaOferta.StatusMatricula = statusMatriculaOferta;

            if (dataConclusao != null || notaFinal != null)
            {
                foreach (var mt in matriculaOferta.MatriculaTurma)
                {
                    // Obter a turma novamente, pois o lazy tá pirando nessa parte.
                    var turma = new ManterTurma().ObterTurmaPorID(mt.Turma.ID);

                    // Só altera caso a data da turma seja menor ou igual à data de conclusão informada.
                    if (dataConclusao.HasValue &&
                        (turma.DataInicio <= dataConclusao))
                    {
                        mt.DataTermino = dataConclusao;
                    }

                    if (notaFinal.HasValue)
                    {
                        mt.MediaFinal = notaFinal;
                    }
                }
            }

            manterMatriculaOferta.AtualizarMatriculaOferta(matriculaOferta, false, fazerMerge);

            new BP.ManterItemTrilhaParticipacao().AtualizarStatusParticipacoesTrilhas(matriculaOferta);
        }

        private enum EnumSelecao
        {
            Usuario,
            Solucao,
            Oferta,
            Turma
        }

    }
}