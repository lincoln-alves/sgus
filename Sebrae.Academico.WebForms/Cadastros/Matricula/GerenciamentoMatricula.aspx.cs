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
//using Sebrae.Academico.BP.Services.SgusWebService;
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
    public partial class GerenciamentoMatricula : PageBase
    {
        private List<classes.MatriculaOferta> matriculasFiltradas
        {
            get
            {
                return Session["matriculasFiltradas"] as List<classes.MatriculaOferta>;
            }
            set
            {
                Session["matriculasFiltradas"] = value;
            }
        }

        public bool exibirSelecione = false;
        private readonly ManterUsuario _manterUsuario = new ManterUsuario();

        // Numero de registros por página de matriculas oferta
        private const int QuantidadeMatriculasPorPagina = 30;

        protected override enumFuncionalidade Funcionalidade
        {
            get { return enumFuncionalidade.GerenciamentoMatricula; }
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

        protected override void OnInit(EventArgs e)
        {
            ucCategorias1.TreeNodeCheckChanged += AtualizarComboSolucaoEducacional;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Previne o carregamento de informações desnecessárias no load da página, deixando a tela pesada.
            DesabilitarLoadOfertaTurma();

            try
            {
                if (!Page.IsPostBack)
                {
                    Session["___ddlStatusOferta"] = null;

                    // Previnindo evento de clique duplo
                    WebFormHelper.PrevinirCliqueDuplo(new List<Button>()
                    {
                        btnMatricularEmLote,
                        btnGerenciarVagas,
                        btnEnviarArquivo,
                        btnSalvarOferta,
                        btnSalvarTurma
                    }, this);

                    LogarAcessoFuncionalidade();

                    pnlUcInformacoesDetalhadasProvasRealizadas.Visible = false;
                    lkbMatricularAluno.Attributes.Add("class", "collapsed");
                    lkbAlunosMatriculados.Attributes.Add("class", "collapsed");
                    lkbTurmasCadastradas.Attributes.Add("class", "collapsed");
                    lkbMatricularAlunosEmLote.Attributes.Add("class", "collapsed");
                    lkbAlterarStatusEmLote.Attributes.Add("class", "collapsed");
                    WebFormHelper.PreencherComponenteComOpcoesSimNao(rblGerenciarMatriculasAbertas);
                    WebFormHelper.SetarValorNoRadioButtonList(true, rblGerenciarMatriculasAbertas);

                    PreencherCombos();

                    if (Request["area"] != null)
                    {
                        DvAreaCategoria.Visible = false;
                        DvAreaPrograma.Visible = true;

                        PreencherPrograma();

                        txtSolucaoEducacional.Attributes.Add("data-mensagemVazia", "Selecione um módulo");
                    }

                    if (Request["oferta"] != null)
                    {
                        var ofertaId = int.Parse(Request["oferta"]);

                        var oferta = new ManterOferta().ObterOfertaPorID(ofertaId);

                        if (oferta != null)
                        {
                            txtSolucaoEducacional.Text = oferta.SolucaoEducacional.ID.ToString();
                            txtSolucaoEducacional_OnTextChanged(null, null);

                            txtOferta.Text = oferta.ID.ToString();
                            txtOferta_OnTextChanged(null, null);
                        }
                    }
                }
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private void DesabilitarLoadOfertaTurma()
        {
            ucTurma1.CarregarLoad = false;
            ucOferta1.CarregarLoad = false;
        }

        protected void lkbAlunosMatriculados_Click(object sender, EventArgs e)
        {
            AlterarStatusTab(lkbAlunosMatriculados, collapseMatriculados);
        }

        protected void lbGerenciadorVagas_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtOferta.Text) == false)
            {
                var oferta = new ManterOferta().ObterOfertaPorID(int.Parse(txtOferta.Text));

                var idUfUsuario = new ManterUsuario().ObterUfLogadoSeGestor();

                var dadosPermissaoEstado = oferta.ListaPermissao.FirstOrDefault(x => x.Uf != null && x.Uf.ID == idUfUsuario);

                var totalEstado = dadosPermissaoEstado != null ? dadosPermissaoEstado.QuantidadeVagasPorEstado : 0;
                var totalUtilizado =
                    oferta.ListaMatriculaOferta.Where(
                        x =>
                            x.StatusMatricula != enumStatusMatricula.CanceladoAdm && x.StatusMatricula != enumStatusMatricula.CanceladoGestor &&
                                x.StatusMatricula != enumStatusMatricula.CanceladoAluno && x.UF.ID == idUfUsuario);

                var totalDisponivel = (totalEstado - totalUtilizado.Count());

                lblQtdeVagasDisponiveis.Text = totalDisponivel > 0 ? totalDisponivel.ToString() : "0";
                ddlQtdeVagas.Items.Clear();

                if (totalDisponivel > 0)
                {
                    for (var i = 1; i <= totalDisponivel; i++)
                    {
                        ddlQtdeVagas.Items.Add(new ListItem { Value = i.ToString(), Text = i.ToString() });
                    }
                }
            }

            AlterarStatusTab(lbGerenciadorVagas, collapseGerenciadorVagas);
        }

        protected void lkbTurmasCadastradas_Click(object sender, EventArgs e)
        {
            AlterarStatusTab(lkbTurmasCadastradas, collapseTurmas);
        }

        private void HabilitarBotaoEditarOferta()
        {
            btnEditarOferta.Visible = true;
        }

        private void HabilitarBotaoAdicionarOferta()
        {
            btnAdicionarOferta.Visible = true;
        }

        public void rblGerenciarMatriculasAbertas_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (Request["area"] != null)
            {
                PreencherPrograma();

                txtCapacitacao.Text = "";
                ViewState["_Capacitacao"] = null;

                txtModulo.Text = "";
                ViewState["_Modulo"] = null;

                txtSolucaoEducacional.Text = "";
                ViewState["_SE"] = null;
            }
            else
            {
                txtSolucaoEducacional.Text = "";
                ViewState["_SE"] = null;

                PreencherSolucaoEducacional();
            }

            txtOferta.Text = "";
            ViewState["_Oferta"] = null;
        }

        private void AplicarPermissoesDoFornecedor()
        {
            if (InformouSolucaoEducacional())
            {
                var solucaoEducacional = new ManterSolucaoEducacional().ObterSolucaoEducacionalPorId(int.Parse(txtSolucaoEducacional.Text));

                if (!solucaoEducacional.Fornecedor.PermiteGestaoSGUS)
                {
                    btnAdicionarOferta.Enabled = false;
                    btnAdicionarTurma.Enabled = false;
                }
                else
                {
                    btnAdicionarOferta.Enabled = solucaoEducacional.Fornecedor.PermiteCriarOferta;
                    btnAdicionarTurma.Enabled = solucaoEducacional.Fornecedor.PermiteCriarTurma;
                }
            }
            else
            {
                btnAdicionarOferta.Enabled = false;
                btnAdicionarTurma.Enabled = false;
            }
        }

        private void PreencherOferta()
        {
            if (InformouSolucaoEducacional())
            {
                try
                {
                    HabilitarBotaoAdicionarOferta();

                    //Busca as ofertas associadas à solução educacional informada
                    int id;
                    if (int.TryParse(txtSolucaoEducacional.Text, out id))
                    {
                        PreencherComboOferta(id);
                    }
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
            }
        }

        private bool InformouSolucaoEducacional()
        {
            return !string.IsNullOrWhiteSpace(txtSolucaoEducacional.Text);
        }

        private bool InformouOferta()
        {
            int idOFerta;
            return !string.IsNullOrWhiteSpace(txtOferta.Text) && int.TryParse(txtOferta.Text, out idOFerta);
        }

        private void PreencherComboOferta(int idSe)
        {
            var se = new ManterSolucaoEducacional().ObterSolucaoEducacionalPorId(idSe);

            if (se != null)
            {
                var matriculasAbertas = rblGerenciarMatriculasAbertas.SelectedValue == "S";

                var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

                var ofertas = se.ListaOferta.AsQueryable();

                ofertas = matriculasAbertas
                    ? ofertas.Where(
                        o =>
                            o.DataInicioInscricoes.HasValue && o.DataFimInscricoes.HasValue &&
                            ((o.DataInicioInscricoes.Value.Date <= DateTime.Now.Date &&
                              o.DataFimInscricoes.Value.Date >= DateTime.Now.Date) ||
                             (DateTime.Now.Date <= o.DataInicioInscricoes)))
                    : ofertas.Where(
                        o => o.DataFimInscricoes.HasValue && o.DataFimInscricoes.Value.Date < DateTime.Now.Date);

                //Comentário para retestar a atualização do jenkins (comentário temporário...)
                if (usuarioLogado.IsGestor() && !usuarioLogado.IsNacional())
                    ofertas =
                        ofertas.Where(
                            o =>
                                o.MatriculaGestorUC.HasValue && o.MatriculaGestorUC.Value &&
                                o.ListaPermissao.Any(p => p.Uf != null && p.Uf.ID == usuarioLogado.UF.ID));

                if (usuarioLogado.IsConsultorEducacional())
                    ofertas =
                        ofertas.Where(
                            o =>
                                o.ListaTurma.Any(
                                    t =>
                                        t.ConsultorEducacional != null &&
                                        t.ConsultorEducacional.ID == usuarioLogado.ID));

                ViewState["_Oferta"] = Helpers.Util.ObterListaAutocomplete(ofertas);
            }
            else
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Atencao, "A Solução Educacional selecionada é inválida.");
            }
        }

        public IList<classes.StatusMatricula> ListaStatusMatricula { get; set; }

        /// <summary>
        /// Evento disparado quando o cadastro de uma turma é cancelado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CadastroTurma_CancelouCadastroDaTurma(object sender, CadastrarTurmaEventArgs e)
        {
            SetarListaComStatusDeMatricula();

            Response.Redirect("GerenciamentoMatricula.aspx");
        }

        private void OcultarModalTurma()
        {
            OcultarBackDrop();
            pnlModalTurma.Visible = false;
        }

        /// <summary>
        ///  Evento disparado quando um aluno é matriculado em uma turma.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MatricularAlunoNaTurma_MatriculouAlunoEmUmaTurma(object sender,
            MatricularAlunoEmUmaTurmaEventArgs e)
        {
            OcultarModalMatriculaTurma();
        }

        protected void SelecionarProvaDeUmAluno_SelecionouProvaDeUmAluno(object sender,
            SelecionarProvaDeUmAlunoEventArgs e)
        {
            OcultarModalMatriculaTurma();
            //Chama o usercontrol de Detalhes
            ExibirInformacoesDetalhadasDaProva(e.ProvaSelecionada.ID);
            pnlUcInformacoesDetalhadasProvasRealizadas.Visible = true;
            ucExibirQuestionarioResposta.Visible = true;
        }

        protected void MatriculaOferta_CancelouMatriculaDeUmAlunoEmUmaOferta(object sender,
            MatricularAlunoEmUmaOfertaEventArgs e)
        {
            //pnlMatricularAlunoNaOferta.Visible = false;

            //Já que houve um cancelamento, remove o item do questionário, da lista de questionários da sessão
            //TODO: -> Implementar isso

            //PreencherItensDoQuestionario(questionario);
            //SetarAcaoDaTela(enumAcaoTelaQuestionario.CancelouCadastroDeItem);

        }

        /// <summary>
        /// Evento disparado quando uma matricula em uma oferta for realizada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MatriculaOferta_MatriculouAlunoEmUmaOferta(object sender, MatricularAlunoEmUmaOfertaEventArgs e)
        {
            PopularGridMatriculasOfertas(e.MatriculaOfertaCadastrada.Oferta.ID);
        }

        private void PopularGridMatriculasOfertas(int idOferta)
        {
            var usuarioLogado = _manterUsuario.ObterUsuarioLogado();

            var matriculas = new ManterMatriculaOferta().ObterMatriculaOfertaPorOferta(idOferta, QuantidadeMatriculasPorPagina, 0, usuarioLogado);

            PopularMatriculasOfertas(matriculas.ToList(), usuarioLogado);

            ucMatriculaOferta1.LimparCampos();
        }

        private void SetarListaComStatusDeMatricula()
        {
            if ((ListaStatusMatricula == null || !ListaStatusMatricula.Any()) &&
                string.IsNullOrWhiteSpace(txtSolucaoEducacional.Text) == false)
            {
                var categoriaConteudo =
                    new ManterSolucaoEducacional().ObterSolucaoEducacionalPorId(
                        int.Parse(txtSolucaoEducacional.Text)).CategoriaConteudo;

                var manterStatusMatricula = new ManterStatusMatricula();

                var status = manterStatusMatricula.ObterStatusMatriculaPorCategoriaConteudo(categoriaConteudo).AsQueryable();

                var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

                // Filtrar Status pela visualização do usuário.
                status = status.Where(x => x.PermiteVisualizacao(usuarioLogado));

                ListaStatusMatricula = status.ToList();
            }
        }

        private void PreencherGridComInformacoesDaMatriculaOfertaRealizada(
            classes.MatriculaOferta informacoesdaMatriculaOfertaRealizada)
        {
            if (informacoesdaMatriculaOfertaRealizada != null)
            {
                var matriculas = new List<classes.MatriculaOferta> { informacoesdaMatriculaOfertaRealizada };

                SetarTurmasDaOferta(informacoesdaMatriculaOfertaRealizada.Oferta.ID);

                if (ListaStatusMatricula == null || ListaStatusMatricula.Any())
                    SetarListaComStatusDeMatricula();

                PopularMatriculasOfertas(matriculas);
            }
            else
            {
                pnlMatricula.Visible = false;
            }
        }

        public int IdMatriculaOferta
        {
            get
            {
                if (ViewState["IdMatriculaOferta"] != null)
                {
                    return (int)ViewState["IdMatriculaOferta"];
                }

                return 0;
            }
            set { ViewState["IdMatriculaOferta"] = value; }

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

        private void PreencherCombos()
        {
            try
            {
                if (Request["area"] == null)
                    PreencherSolucaoEducacional();

                ucCategorias1.PreencherCategorias(false, null, null, true);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public void AtualizarComboSolucaoEducacional(object sender, EventArgs e)
        {
            PreencherSolucaoEducacional();
        }

        public IList<classes.Turma> TurmasDaOferta { get; set; }

        protected void btnEditarOferta_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtOferta.Text))
            {
                ChamarTelaDeOfertaParaEdicao(int.Parse(txtOferta.Text));
            }
        }

        private void ChamarTelaDeOfertaParaEdicao(int idOferta)
        {
            classes.Oferta oferta = new ManterOferta().ObterOfertaPorID(idOferta);

            if (oferta != null)
            {
                ucOferta1.CarregarLoad = true;
                ucOferta1.CarregarDados();
                ucOferta1.IdOferta = oferta.ID;
                ucOferta1.AcaoDaTela = (int)ucOferta.enumAcaoDaTela.EdicaoDeUmaOferta;
                ucOferta1.PrepararTelaParaEdicaoDeUmaOferta(oferta);
                ExibirModalOferta();
            }
        }

        protected void btnSalvarOferta_Click(object sender, EventArgs e)
        {
            try
            {
                var oferta = ucOferta1.ObterObjetoOferta();

                var manterOferta = new ManterOferta();

                manterOferta.AlterarOferta(oferta);

                PreencherOferta();

                if (oferta != null)
                    txtOferta.Text = oferta.ID.ToString();

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Oferta salva com sucesso.");

                OcultarModalOferta();

                HabilitarBotaoAdicionarTurma();

                HabilitarBotaoEditarOferta();

                CarregarInformacoesGerais();

                MostrarTab(lkbAlunosMatriculados, collapseMatriculados);
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        protected void btnEnviarEmail_Click(object sender, EventArgs e)
        {
            var idOferta = 0;

            //Se informou a Oferta, seta o id da oferta.
            if (!string.IsNullOrWhiteSpace(txtOferta.Text))
                idOferta = int.Parse(txtOferta.Text);

            var listaMatriculaOferta =
                new ManterMatriculaOferta().ObterMatriculaOfertaPorOferta(idOferta)
                    .Where(
                        x =>
                            x.StatusMatricula != enumStatusMatricula.CanceladoAdm ||
                            x.StatusMatricula != enumStatusMatricula.CanceladoAluno)
                    .ToList();

            var manterOferta = new ManterOferta();
            var btnSender = (Button)sender;

            try
            {

                if (btnSender.ID == "btnEnviarEmailInscrito")
                {
                    listaMatriculaOferta =
                        listaMatriculaOferta.Where(x => x.StatusMatricula == enumStatusMatricula.Inscrito).ToList();
                    manterOferta.EnviarEmailParaAlunosDaOferta(listaMatriculaOferta, false);
                }
                else
                {
                    listaMatriculaOferta =
                        listaMatriculaOferta.Where(
                            x => x.StatusMatricula == enumStatusMatricula.PendenteConfirmacaoAluno).ToList();
                    manterOferta.EnviarEmailParaAlunosDaOferta(listaMatriculaOferta, true);
                }

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Os emails foram enviados com sucesso.");
            }
            catch
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Erro ao montar o e-mail");
            }
        }

        protected void btnAdicionarOferta_Click(object sender, EventArgs e)
        {
            var solucaoEducacional = new classes.SolucaoEducacional { ID = int.Parse(txtSolucaoEducacional.Text) };

            ucOferta1.CarregarLoad = true;
            ucOferta1.CarregarDados();
            ucOferta1.PrepararInformacoesSobreSolucaoEducacional(solucaoEducacional, true);

            ExibirModalOferta();

            ucOferta1.InformarAcaoDeCadastro();
        }

        private void PopularMatriculasOfertas(IList<classes.MatriculaOferta> lista, classes.Usuario usuarioLogado = null)
        {
            // Buscar lista fora dos eventos do GridView para não ficar realizando consultas
            // para cada registro do loop de row. Melhora muito o desempenho, reduzindo a
            // quantidade de consultas.
            SetarListaComStatusDeMatricula();

            PrepararModoAvaliacao(usuarioLogado);
            WebFormHelper.PreencherGrid(lista, dgvMatriculaOferta);
        }

        private void BuscarMatriculaOferta(int page = 0)
        {
            try
            {
                var idOferta = 0;

                //Se informou a Oferta, seta o id da oferta.
                if (!string.IsNullOrWhiteSpace(txtOferta.Text))
                    idOferta = int.Parse(txtOferta.Text);

                var manterMatriculaOferta = new ManterMatriculaOferta();

                // Se for o primeiro carrega todos para a paginação funcionar
                var lista =
                    manterMatriculaOferta.ObterMatriculaOfertaPorOferta(idOferta, QuantidadeMatriculasPorPagina, page);

                // Pega o total para fazer a paginação
                var total = manterMatriculaOferta.ObterQuantidadeMatriculaOfertaPorOferta(idOferta);

                // Mostra a paginação
                PopulateMatriculaOfertaPager(total, page);

                if (lista.Any())
                {
                    SetarListaComStatusDeMatricula();

                    //Obtém a lista de turmas da Oferta
                    var turmasOferta = new ManterMatriculaOferta().ObterTurmasDaOferta(idOferta);

                    var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

                    if (usuarioLogado.IsConsultorEducacional())
                        turmasOferta =
                            turmasOferta.Where(
                                x => x.ConsultorEducacional != null && x.ConsultorEducacional.ID == usuarioLogado.ID);

                    TurmasDaOferta = turmasOferta.ToList();
                }

                PopularMatriculasOfertas(lista);

                pnlMatricula.Visible = true;
                ExibirBotaoAdicionarTurma();

            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private void SetarTurmasDaOferta(int idOferta)
        {
            //Obtém a lista de turmas da Oferta
            var turmasOferta = new ManterMatriculaOferta().ObterTurmasDaOferta(idOferta);

            var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

            if (usuarioLogado.IsConsultorEducacional())
                turmasOferta =
                    turmasOferta.Where(
                        x => x.ConsultorEducacional != null && x.ConsultorEducacional.ID == usuarioLogado.ID);

            TurmasDaOferta = turmasOferta.ToList();
        }

        private void ExibirBotaoAdicionarTurma()
        {
            btnAdicionarTurma.Visible = true;
        }

        protected void btnAdicionarTurma_Click(object sender, EventArgs e)
        {
            try
            {
                PrepararTelaDeTurmaParaInclusao();
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private void PrepararTelaDeTurmaParaInclusao()
        {
            ucTurma1.CarregarLoad = true;
            ucTurma1.CarregarDados();
            ucTurma1.LimparCampos();
            ucTurma1.EsconderBotaoSalvar();

            // Preencher SE e Oferta
            var oferta = new ManterOferta().ObterOfertaPorID(int.Parse(txtOferta.Text));

            ucTurma1.PreencherCampos();

            ucTurma1.SelecionarOferta(oferta, true);

            ucTurma1.AtualizarGruposDoMoodle();

            ExibirModalTurma();
        }

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

        protected void btnAdicionarAluno_Click(object sender, EventArgs e)
        {
            try
            {
                ucMatriculaOferta1.LimparCampos();
                CarregarDadosDaMatriculaOfertaDeUmAluno(null);
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private void CarregarDadosDaMatriculaOfertaDeUmAluno(classes.MatriculaOferta matriculaOferta)
        {
            PreencherCamposDaMatriculaDeUmAluno(matriculaOferta);
        }

        private void PreencherCamposDaMatriculaDeUmAluno(classes.MatriculaOferta matriculaOferta)
        {
            //Persiste o Id da Oferta
            if (InformouOferta())
            {
                ucMatriculaOferta1.IdOferta = int.Parse(txtOferta.Text);
            }

            ucMatriculaOferta1.PreencherCamposDaMatriculaOferta(matriculaOferta);
        }

        private void HabilitarBotaoAdicionarTurma()
        {
            btnAdicionarTurma.Enabled = true;
        }

        protected void dgvMatriculaOferta_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("avaliarTurma"))
            {
                RedirecionarPaginaAvaliacao();
                return;
            }

            if (e.CommandName.Equals("exibirDetalhes"))
            {
                var matriculaTurmaId = int.Parse(e.CommandArgument.ToString());
                ExibirVisualizacaoRapidaMatriculaAvaliacao(matriculaTurmaId);
                ExibirModalVisualizacaoRapidaMatriculaAvaliacao();

                return;
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

            if (e.CommandName.Equals("enviarMatricula"))
            {
                try
                {
                    var idMatriculaOferta = int.Parse(e.CommandArgument.ToString());

                    var mo = new ManterMatriculaOferta().ObterMatriculaOfertaPorID(idMatriculaOferta);

                    if (!mo.FornecedorNotificado)
                    {
                        NotificaFornecedor.Instancia.Notificar(mo);
                    }

                    new ManterMatriculaOferta().AtualizarMatriculaOferta(mo, false);

                    CarregarInformacoesGerais();

                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados enviados com sucesso");
                }
                catch (Exception ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Mensagem do fornecedor: " + ex.Message);
                    return;
                }
            }

            if (e.CommandName.Equals("enviarEmailPendente"))
            {
                try
                {
                    var idMatriculaOferta = int.Parse(e.CommandArgument.ToString());

                    var matriculaOferta =
                        new ManterMatriculaOferta().ObterMatriculaOfertaPorID(idMatriculaOferta);

                    var templateMensagemEmailOfertaExclusiva =
                        TemplateUtil.ObterInformacoes(enumTemplate.EnvioEmailParaPendenteDeConfirmacao);

                    var assuntoDoEmail = templateMensagemEmailOfertaExclusiva.Assunto;

                    var corpoEmail = templateMensagemEmailOfertaExclusiva.TextoTemplate;

                    var emailDoDestinatario = matriculaOferta.Usuario.Email;

                    if (matriculaOferta.StatusMatricula == enumStatusMatricula.PendenteConfirmacaoAluno)
                    {
                        assuntoDoEmail = assuntoDoEmail.Replace("#NOME_CURSO", matriculaOferta.NomeSolucaoEducacional);
                        corpoEmail = new ManterOferta().CorpoEmail(corpoEmail, matriculaOferta);

                        EmailUtil.Instancia.EnviarEmail(emailDoDestinatario.Trim(),
                            assuntoDoEmail, corpoEmail);
                    }

                    CarregarInformacoesGerais();
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados enviados com sucesso");
                }
                catch (Exception ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Mensagem do fornecedor: " + ex.Message);
                    return;
                }
            }

            if (e.CommandName.Equals("emitirCertificado"))
            {
                var idMatriculaOferta = int.Parse(e.CommandArgument.ToString());

                var matriculaOferta =
                    new ManterMatriculaOferta().ObterMatriculaOfertaPorID(idMatriculaOferta);

                new ucCursos().EmitirCertificado(matriculaOferta);
            }
        }

        public int QuantidadeQuestoes { get; set; }

        public List<classes.Questao> Questoes { get; set; }

        private void ExibirVisualizacaoRapidaMatriculaAvaliacao(int matriculaTurmaId)
        {
            var matriculaTurma = new ManterMatriculaTurma().ObterMatriculaTurmaPorId(matriculaTurmaId);

            var manterAvaliacao = new ManterAvaliacao();

            var questoes = manterAvaliacao.ObterQuestoes().ToList();

            QuantidadeQuestoes = questoes.Count();

            Questoes = questoes;

            rptQuestoes.DataSource = Questoes;
            rptQuestoes.DataBind();

            rptMatriculas.DataSource = new List<classes.MatriculaTurma> { matriculaTurma };
            rptMatriculas.DataBind();
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

        protected void dgvMatriculaOferta_RowDataBound(object sender, GridViewRowEventArgs e)
        {
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

                        if (matriculaOferta.StatusMatricula == enumStatusMatricula.PendenteConfirmacaoAluno)
                        {
                            lkbEnviarMatriculaEmailPendente.Visible = true;
                            lkbEnviarMatriculaEmailPendente.CommandArgument = matriculaOferta.ID.ToString();
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

        private void TratarComboStatusOferta(DropDownList ddlStatusOferta, classes.MatriculaOferta matriculaOferta, Label statusOferta = null)
        {
            var permiteAlteracao = matriculaOferta.Oferta.AlteraPeloGestorUC;

            // Caso esteja em modo de avaliação, só adiciona o Status atual da matrícula e esconde o dropdown.
            if (!InModoDeAvaliacao && (permiteAlteracao == true || _manterUsuario.PerfilAdministrador()))
            {
                if (ddlStatusOferta != null)
                {
                    var listaStatusMatricula = new List<classes.StatusMatricula>();

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

                    if (!string.IsNullOrWhiteSpace(matriculaOferta.NotaFinal))
                    {
                        var concluido = listaStatusMatricula.Where(x => x.ID == (int)enumStatusMatricula.Concluido).FirstOrDefault();
                        if (matriculaOferta.StatusMatricula != (enumStatusMatricula)concluido.ID)
                        {
                            listaStatusMatricula.Remove(concluido);
                        }
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

        private void TratarComboTurma(GridViewRowEventArgs e)
        {
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

        /// <summary>
        /// Método criado para manipular, dinamicamente, o evento SelectedIndexChanged da combo de turma (existente na grid)
        /// </summary>
        /// <param name="sender">objeto que disparou o evento</param>
        /// <param name="e">Argumentos do evento</param>
        protected void ddlTurma_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var comboTurma = (DropDownList)sender;
                //Se Selecionou uma turma na combo, atualiza ou insere a matricula do usuário nesta turma.
                if (comboTurma != null && comboTurma.SelectedItem != null &&
                    !string.IsNullOrWhiteSpace(comboTurma.SelectedItem.Value))
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
                //}
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

        private void MatricularAlunoNaTurma(DropDownList comboTurma)
        {
            //Obtém a linha da grid onde a dropdownlist (combo) de oferta, está contida
            var linhadaGrid = (GridViewRow)comboTurma.NamingContainer;

            if (linhadaGrid != null)
            {
                var hdfIdMatriculaOferta = (HiddenField)linhadaGrid.FindControl("hdfIdMatriculaOferta");

                var matriculaOferta =
                    new ManterMatriculaOferta().ObterMatriculaOfertaPorID(int.Parse(hdfIdMatriculaOferta.Value));

                if (matriculaOferta != null)
                {
                    var matriculaTurma = ObterObjetoMatriculaTurma(comboTurma, matriculaOferta);

                    var manterMatriculaOferta = new ManterMatriculaOferta();

                    if (matriculaOferta.MatriculaTurma.Count == 0)
                        matriculaOferta.MatriculaTurma.Add(matriculaTurma);
                    else
                        matriculaOferta.MatriculaTurma[0] = matriculaTurma;

                    manterMatriculaOferta.AtualizarMatriculaOferta(matriculaOferta, false);

                    CarregarInformacoesGerais();

                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso,
                        string.Format("O usuário '{0}' foi matriculado na turma '{1}'",
                            matriculaOferta.Usuario.Nome, matriculaTurma.Turma.Nome));
                }
            }
        }

        private void ExcluirAlunoDaTurma(DropDownList comboTurma)
        {
            //Obtém a linha da grid onde a dropdownlist (combo) de oferta, está contida
            var linhadaGrid = (GridViewRow)comboTurma.NamingContainer;

            if (linhadaGrid != null)
            {
                var hdfIdMatriculaOferta = (HiddenField)linhadaGrid.FindControl("hdfIdMatriculaOferta");

                var manterMatriculaOferta = new ManterMatriculaOferta();
                var matriculaOferta = manterMatriculaOferta.ObterMatriculaOfertaPorID(int.Parse(hdfIdMatriculaOferta.Value));

                if (matriculaOferta != null && matriculaOferta.MatriculaTurma != null &&
                    matriculaOferta.MatriculaTurma.Count > 0)
                {
                    matriculaOferta.MatriculaTurma.Clear();
                    AtualizarStatusDaOferta(matriculaOferta.StatusMatricula, matriculaOferta, ref manterMatriculaOferta);
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
                matriculaTurma = new classes.MatriculaTurma
                {
                    MatriculaOferta = matriculaOferta,
                    Turma = new ManterTurma().ObterTurmaPorID(int.Parse(comboTurma.SelectedItem.Value)),
                    DataMatricula = DateTime.Now
                };

                matriculaTurma.DataLimite = matriculaTurma.CalcularDataLimite();
            }
            else
            {
                //Troca a turma, pois o usuário informou uma nova turma
                var idTurmaEscolhidaNaCombo = int.Parse(comboTurma.SelectedItem.Value);

                if (!matriculaTurma.ID.Equals(idTurmaEscolhidaNaCombo))
                {
                    matriculaTurma.TurmaAnterior = matriculaTurma.Turma;

                    /* Troca a Turma do usuário (ou seja, matricula o aluno em uma nova turma), 
                       pois ele escolheu uma nova turma na combo.*/
                    matriculaTurma.Turma = new ManterTurma().ObterTurmaPorID(int.Parse(comboTurma.SelectedItem.Value));

                }
            }

            return matriculaTurma;
        }

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

                    var matriculaOferta = manterMatriculaOferta.ObterMatriculaOfertaPorID(int.Parse(hdfIdMatriculaOferta.Value));

                    // Se certificar do carregamento da Oferta e da Solução Educacional.
                    if (matriculaOferta.Oferta == null)
                    {
                        int id;
                        if (int.TryParse(txtOferta.Text, out id))
                        {
                            matriculaOferta.Oferta = new ManterOferta().ObterOfertaPorID(id);
                        }
                    }

                    if (int.Parse(comboStatusOferta.SelectedItem.Value) == (int)enumStatusMatricula.CanceladoAdm)
                    {
                        matriculaOferta = new ManterMatriculaOferta().VerificarFilaEspera(matriculaOferta);
                    }

                    var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

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
                            AtualizarStatusDaOferta(enumStatusMatricula.Inscrito, matriculaOferta,
                                ref manterMatriculaOferta);
                            throw;
                        }
                    }

                    if (statusMatriculaOferta.Equals(enumStatusMatricula.CanceladoAdm) || statusMatriculaOferta.Equals(enumStatusMatricula.CanceladoGestor))
                    {
                        VerificarQuestionarioCancelamento(matriculaOferta, statusMatriculaOferta, ddlStatusOferta);
                    }
                    else if (statusMatriculaOferta.Equals(enumStatusMatricula.CanceladoAluno))
                    {
                        //O usuário do admin, pode ver o status do Cancelado/Aluno, mas nunca pode setar esse status
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Atencao, "Apenas o aluno pode atribuir o status de cancelado pelo aluno");
                    }
                    else
                    {
                        /*AtualizarStatusDaOferta(statusMatriculaOferta, matriculaOferta, ref manterMatriculaOferta);

                        ddlTurma.Enabled = true;

                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "O Status da Matrícula da Turma foi Atualizado com Sucesso !");*/
                        var matriculaTurma = matriculaOferta.MatriculaTurma.FirstOrDefault();
                        if (matriculaTurma == null)
                        {
                            WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "O aluno precisa estar vinculado a uma turma.");
                            return;
                        }

                        if (statusMatriculaOferta == enumStatusMatricula.Inscrito || statusMatriculaOferta == enumStatusMatricula.FilaEspera)
                        {
                            ExibirModalConfirmacaoStatusInscrito(matriculaTurma, statusMatriculaOferta);
                        }
                        else
                        {
                            ExibirModalDataConclusao(matriculaTurma, statusMatriculaOferta);
                        }
                    }

                    // Atualizar lista de status disponíveis, pois pode sofrer alteração caso haja
                    // o status de Cancelado\Turma, que possui um comportamento especial.
                    SetarListaComStatusDeMatricula();

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

        private void VerificarQuestionarioCancelamento(classes.MatriculaOferta matriculaOferta, enumStatusMatricula statusMatriculaOferta, DropDownList ddlStatusOferta)
        {
            var matriculaTurma = matriculaOferta.MatriculaTurma.FirstOrDefault();

            if (matriculaTurma == null)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "O aluno precisa estar vinculado a uma turma.");
                return;
            }

            var questionarioAssociacao = matriculaTurma.Turma.ListaQuestionarioAssociacao.FirstOrDefault(x => x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Cancelamento);

            // Caso a turma não possua questionário de cancelamento, prosseguir com alteração normal do Status.
            if (questionarioAssociacao == null || questionarioAssociacao.Questionario == null)
            {
                //AtualizarStatusDaOferta(statusMatriculaOferta, matriculaOferta, ref manterMatriculaOferta);
                ExibirModalDataConclusao(matriculaTurma, statusMatriculaOferta);

                CarregarInformacoesGerais();

                return;
            }

            // Responder questionário e alterar Status da Matrícula após responder o questionário.
            ucQuestionario.IniciarRespostaQuestionario(questionarioAssociacao.Questionario, new List<int> { matriculaTurma.ID }, enumStatusMatricula.CanceladoAdm, matriculaTurma.Turma.DataFinal);

            ExibirModalQuestionarioCancelamento();

            ddlStatusOferta.SelectedValue = ((int)matriculaOferta.StatusMatricula).ToString();
        }
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

        private void AtualizarStatusUsuarioTurma()
        {

            var manterMatriculaTurma = new ManterMatriculaTurma();

            var idMatriculaTurma = Convert.ToInt32(hdfModalConfirmacaoStatusInscritoIdMatriculaTurma.Value);
            var matriculaTurma = manterMatriculaTurma.ObterMatriculaTurmaPorId(idMatriculaTurma);

            var matriculaOferta = matriculaTurma.MatriculaOferta.Oferta;

            if (matriculaOferta.SolucaoEducacional.Fornecedor.ID == (int)enumFornecedor.MoodleSebrae)
            {
                var idUsuario = matriculaTurma.MatriculaOferta.Usuario.ID;
                var chaveExterna = matriculaOferta.IDChaveExterna;
                var cpfUsuario = matriculaTurma.MatriculaOferta.Usuario.CPF;
            }
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

            // Remove todos os questionários respondidos pelo cancelamento da matrícula.
            RemoverQuestionarios(matriculaOferta);

            if (ucMatriculaOferta1.ExisteComStatusRepetido(statusMatriculaOferta, matriculaOferta))
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Não é possível inserir uma nova matrícula com o mesmo status");
                return;
            }

            //Atualiza o status da Oferta
            matriculaOferta.StatusMatricula = statusMatriculaOferta;

            //if (dataConclusao != null || notaFinal != null){
            if (matriculaOferta.MatriculaTurma.Any())
            {
                foreach (var mt in matriculaOferta.MatriculaTurma)
                {
                    // Obter a turma novamente, pois o lazy tá pirando nessa parte.
                    var turma = new ManterTurma().ObterTurmaPorID(mt.Turma.ID);

                    // Só altera caso a data da turma seja menor ou igual à data de conclusão informada.
                    if (dataConclusao.HasValue && turma.DataInicio <= dataConclusao)
                    {
                        mt.DataTermino = dataConclusao;
                    }

                    if (statusMatriculaOferta == enumStatusMatricula.Inscrito || statusMatriculaOferta == enumStatusMatricula.FilaEspera)
                    {
                        mt.ValorNotaProvaOnline = null;
                        mt.Nota1 = null;
                        mt.Nota2 = null;
                        mt.MediaFinal = null;
                    }
                    else
                    {
                        if (notaFinal.HasValue)
                        {
                            mt.MediaFinal = notaFinal;
                        }
                    }
                }
            }

            manterMatriculaOferta.AtualizarMatriculaOferta(matriculaOferta, false, fazerMerge);

            if (atualizarMatriculas)
            {
                BuscarMatriculaOferta();
            }

            new BP.ManterItemTrilhaParticipacao().AtualizarStatusParticipacoesTrilhas(matriculaOferta);
        }

        /// <summary>
        /// Remove todos os questionários respondidos pelo cancelamento da matrícula.
        /// </summary>
        /// <param name="matriculaOferta"></param>
        private static void RemoverQuestionarios(classes.MatriculaOferta matriculaOferta)
        {
            var manterMatriculaTurma = new ManterMatriculaTurma();

            foreach (var item in matriculaOferta.MatriculaTurma)
            {
                var matriculaTurma = manterMatriculaTurma.ObterMatriculaTurmaPorId(item.ID);

                if (matriculaTurma == null) continue;

                var questionarios = matriculaTurma.Questionarios.Where(x => x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionario.Cancelamento).ToList();

                if (questionarios.Count <= 0) continue;

                foreach (var questionario in questionarios)
                {
                    matriculaTurma.Questionarios.Remove(questionario);
                }
                manterMatriculaTurma.Salvar(matriculaTurma);
            }
        }


        private void AplicarPermissoesDoGestor()
        {
            try
            {
                if (rblGerenciarMatriculasAbertas.SelectedValue == "S")
                {
                    dvMatricularAluno.Visible = true;
                    dvGerenciadorVagas.Visible = true && !string.IsNullOrWhiteSpace(txtOferta.Text);
                }
                else
                    dvMatricularAluno.Visible = false;

                if (!_manterUsuario.PerfilAdministrador() && !_manterUsuario.PerfilAdministradorTrilha())
                {
                    if (!string.IsNullOrWhiteSpace(txtOferta.Text))
                    {
                        var oferta = new ManterOferta().ObterOfertaPorID(int.Parse(txtOferta.Text));

                        if (oferta != null)
                            if (oferta.MatriculaGestorUC.HasValue && oferta.MatriculaGestorUC.Value == false)
                                dvMatricularAluno.Visible = false;
                            else
                                dvMatricularAluno.Visible = true;

                        if (!oferta.PermiteCadastroTurmaPeloGestorUC.HasValue ||
                            oferta.PermiteCadastroTurmaPeloGestorUC.Value == false)
                            btnAdicionarTurma.Visible = false;
                    }
                }
                //#2383
                dvMatricularAluno.Visible = true;
                dvTurmasCadastradas.Visible = true;
            }
            catch
            {
            }
        }

        private void CarregarInformacoesGerais()
        {
            BuscarMatriculaOferta();
            EsconderPanelParaAdicionarOuEditarOferta();
            EsconderPanelMatricularAlunoNaTurma();

            var idOferta = int.Parse(txtOferta.Text);
            ucMatriculaOferta1.IdOferta = idOferta;

            //ucOferta1.IdSolucaoEducacional = int.Parse(txtSolucaoEducacional.Text);

            PreencherGridComTurmasDaOferta(idOferta);
        }

        private void PreencherGridComTurmasDaOferta(int idOferta)
        {
            //Carrega a grid com as Turmas da Oferta
            var turmas = new ManterTurma().ObterTodosIQueryable()
                .Select(x => new classes.Turma
                {
                    ID = x.ID,
                    Nome = x.Nome,
                    DataInicio = x.DataInicio,
                    DataFinal = x.DataFinal,
                    Oferta = new classes.Oferta
                    {
                        ID = x.Oferta.ID
                    }
                })
                .Where(x => x.Oferta.ID == idOferta)
                .ToList();

            var idsTurmas = turmas.Select(x => x.ID).ToList();

            var matriculasTurmas = new ManterMatriculaTurma().ObterTodosIQueryable()
                .Select(x => new classes.MatriculaTurma
                {
                    ID = x.ID,
                    Turma = new classes.Turma
                    {
                        ID = x.Turma.ID
                    },
                    MatriculaOferta = new classes.MatriculaOferta
                    {
                        ID = x.MatriculaOferta.ID,
                        StatusMatricula = x.MatriculaOferta.StatusMatricula
                    }
                })
                .Where(x => idsTurmas.Contains(x.Turma.ID))
                .ToList();

            foreach (var turma in turmas)
            {
                turma.ListaMatriculas = matriculasTurmas.Where(x => x.Turma.ID == turma.ID).ToList();
            }

            WebFormHelper.PreencherGrid(turmas, dgvTurmasDaOferta);

            if (idOferta > 0)
                btnVerDetalhes.Visible = true;
        }

        private void EsconderPanelMatricularAlunoNaTurma()
        {
            ucMatriculaOferta1.LimparCampos();
        }

        public void EsconderPanelParaAdicionarOuEditarOferta()
        {
            ucOferta1.LimparCampos();
        }

        private void ExibirModalTurma()
        {
            ExibirBackDrop();
            pnlModalTurma.Visible = true;
        }

        private void ExibirModalMatriculaTurma()
        {
            ExibirBackDrop();
            pnlModalMatriculaTurma.Visible = true;
        }

        private void ExibirModalVisualizacaoRapidaMatriculaAvaliacao()
        {
            ExibirBackDrop();
            pnlModalVisualizacaoRapida.Visible = true;
        }

        private void OcultarModalMatriculaTurma()
        {
            OcultarBackDrop();
            pnlModalMatriculaTurma.Visible = false;
            BuscarMatriculaOferta();
        }

        private void OcultarModalVisualizacaoRapida()
        {
            OcultarBackDrop();
            pnlModalVisualizacaoRapida.Visible = false;
            BuscarMatriculaOferta();
        }

        private void ExibirModalOferta()
        {
            ExibirBackDrop();
            pnlModalOferta.Visible = true;
        }

        private void OcultarModalOferta()
        {
            OcultarBackDrop();
            pnlModalOferta.Visible = false;
        }

        private void ExibirModalQuestionarioCancelamento()
        {
            ExibirBackDrop();
            pnlQuestionarioCancelamento.Visible = true;
        }

        private void OcultarModalQuestionarioCancelamento()
        {
            OcultarBackDrop();
            pnlQuestionarioCancelamento.Visible = false;
            ucQuestionario.LimparQuestionario();
        }

        protected void OcultarModalOferta_Click(object sender, EventArgs e)
        {
            OcultarModalOferta();
        }

        protected void OcultarModalDataConclusao_Click(object sender, EventArgs e)
        {
            OcultarModalDataConclusao();
        }

        private void ExibirModalConfirmacaoStatusInscrito(classes.MatriculaTurma matriculaTurma, enumStatusMatricula statusMatriculaOferta)
        {
            ExibirBackDrop();

            pnlModalConfirmacaoStatusInscrito.Visible = true;
            hdfStatusMatricula.Value = ((int)statusMatriculaOferta).ToString();
            hdfModalConfirmacaoStatusInscritoIdMatriculaOferta.Value = matriculaTurma.MatriculaOferta.ID.ToString();
            hdfModalConfirmacaoStatusInscritoIdMatriculaTurma.Value = matriculaTurma.ID.ToString();
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

        protected void btnSimModalConfirmacaoStatusInscrito_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(hdfModalConfirmacaoStatusInscritoIdMatriculaTurma.Value))
                {
                    var manterMatriculaTurma = new ManterMatriculaTurma();
                    var idMatriculaTurma = Convert.ToInt32(hdfModalConfirmacaoStatusInscritoIdMatriculaTurma.Value);
                    var matriculaTurma = manterMatriculaTurma.ObterMatriculaTurmaPorId(idMatriculaTurma);
                    var statusMatricula = (enumStatusMatricula)Convert.ToInt32(hdfStatusMatricula.Value);
                    if (matriculaTurma != null)
                    {
                        matriculaTurma.DataTermino = null;

                        manterMatriculaTurma.Salvar(matriculaTurma);

                        var manterMatriculaOferta = new ManterMatriculaOferta();
                        var matriculaOferta =
                            manterMatriculaOferta.ObterMatriculaOfertaPorID(matriculaTurma.MatriculaOferta.ID);

                        AtualizarStatusDaOferta(statusMatricula, matriculaOferta, ref manterMatriculaOferta);
                        AtualizarStatusUsuarioTurma(matriculaTurma);

                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "O Status da Matrícula da Turma foi Atualizado com Sucesso !");
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


        private void ExibirModalDataConclusao(classes.MatriculaTurma matriculaTurma, enumStatusMatricula statusMatriculaOferta)
        {
            ExibirBackDrop();
            pnlModalDataConclusao.Visible = true;

            txtModalDataConclusao.Text = matriculaTurma.DataTermino.HasValue ? matriculaTurma.DataTermino.Value.ToString("dd/MM/yyyy") : "";
            hdfModalDataConclusaoIdMatriculaTurma.Value = matriculaTurma.ID.ToString();
            hdfModalDataConclusaoIdStatusMatriculaOferta.Value = ((int)statusMatriculaOferta).ToString();
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

                    var dataInicio = matriculaTurma.Turma.DataInicio;
                    if (dataInicio != null && matriculaTurma.DataTermino.Value.Date < dataInicio.Date
                        && matriculaTurma.MatriculaOferta.Oferta.SolucaoEducacional.Fornecedor.ID == (int)enumFornecedor.UCSebrae)
                    {
                        throw new AcademicoException(string.Format("Data de conclusão não pode ser menor que a data inicial da turma ({0}).", dataInicio.ToString("dd/MM/yyyy")));
                    }

                    var dataFinal = matriculaTurma.Turma.DataFinal;
                    if (dataFinal.HasValue && matriculaTurma.DataTermino.Value.Date > dataFinal.Value.Date
                        && matriculaTurma.MatriculaOferta.Oferta.TipoOferta != enumTipoOferta.Continua)
                    {
                        throw new AcademicoException(string.Format("Data de conclusão não pode ser maior que a data final da turma ({0}).", dataFinal.Value.ToString("dd/MM/yyyy")));
                    }

                    manterMatriculaTurma.AlterarMatriculaTurma(matriculaTurma);

                    var manterMatriculaOferta = new ManterMatriculaOferta();
                    var matriculaOferta = manterMatriculaOferta.ObterMatriculaOfertaPorID(matriculaTurma.MatriculaOferta.ID);

                    AtualizarStatusDaOferta((enumStatusMatricula)Convert.ToInt32(hdfModalDataConclusaoIdStatusMatriculaOferta.Value), matriculaOferta, ref manterMatriculaOferta);

                    AtualizarStatusUsuarioTurma(matriculaTurma);

                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "O Status da Matrícula da Turma foi Atualizado com Sucesso !");
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

        protected void OcultarMatriculaTurma_Click(object sender, EventArgs e)
        {
            OcultarModalMatriculaTurma();
        }

        protected void OcultarVisualizacaoRapida_Click(object sender, EventArgs e)
        {
            OcultarModalVisualizacaoRapida();
        }

        protected void OcultarTurma_Click(object sender, EventArgs e)
        {
            OcultarModalTurma();
        }

        protected void btnGerenciarVagas_OnClick(object sender, EventArgs e)
        {
            var idUf = new ManterUsuario().ObterUfLogadoSeGestor();

            if (idUf > 0)
            {
                var idOferta = int.Parse(txtOferta.Text);
                var oferta = new ManterOferta().ObterOfertaPorID(idOferta);
                var uf = new ManterUf().ObterUfPorID(idUf);

                var ofertaPermissao = oferta.ListaPermissao.FirstOrDefault(x => x.Uf != null && x.Uf.ID == idUf);

                var qtdeAtualPermitida = 0;

                if (ofertaPermissao != null)
                {
                    qtdeAtualPermitida = ofertaPermissao.QuantidadeVagasPorEstado;
                    ofertaPermissao.QuantidadeVagasPorEstado = (qtdeAtualPermitida -
                                                                int.Parse(ddlQtdeVagas.SelectedValue));

                    new ManterOfertaPermissao().Salvar(ofertaPermissao);
                }

                var oGerenciadorVaga = new classes.OfertaGerenciadorVaga
                {
                    Oferta = oferta,
                    Usuario = new ManterUsuario().ObterUsuarioLogado(),
                    UF = uf,
                    VagasAnteriores = qtdeAtualPermitida,
                    VagasRecusadas = int.Parse(ddlQtdeVagas.SelectedValue),
                    Descricao = txtObservacaoQtdeVagas.Text,
                    Vigente = true,
                    Contemplado = false,
                    DataSolicitacao = DateTime.Today
                };

                new ManterOfertaGerenciadorVaga().Cadastrar(oGerenciadorVaga);

                txtObservacaoQtdeVagas.Text = string.Empty;

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados cadastrados com sucesso.");
                lbGerenciadorVagas_Click(sender, e);
            }
            else
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Esta funcionalidade é para gestores estaduais.");
            }
        }

        protected void dgvTurmasDaOferta_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var idTurma = int.Parse(e.CommandArgument.ToString());

            if (e.CommandName.Equals("excluirTurma"))
            {
                try
                {
                    var manterTurma = new ManterTurma();

                    manterTurma.ExcluirTurma(idTurma);

                    var idOferta = int.Parse(txtOferta.Text);

                    PreencherGridComTurmasDaOferta(idOferta);

                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Turma excluída com sucesso.");
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }
            }
            else if (e.CommandName.Equals("editarTurma"))
            {
                PrepararTelaDeTurmaParaEdicao(idTurma);
                ExibirModalTurma();
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

        protected void dgvProvasRealizadas_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {

                classes.QuestionarioParticipacao questionarioParticipacao = (classes.QuestionarioParticipacao)e.Row.DataItem;

                if (questionarioParticipacao != null && questionarioParticipacao.ID > 0)
                {

                    decimal percObtido;

                    ManterUsuarioTrilha utBm;

                    classes.UsuarioTrilha ut;

                    var manterQuestionarioParticipacao = new BP.Services.SgusWebService.ManterQuestionarioParticipacao();

                    manterQuestionarioParticipacao.CalcularPercentualDaProva(questionarioParticipacao, out percObtido, out utBm, out ut);

                    //Calcula as notas somente para as provas
                    if (questionarioParticipacao.TipoQuestionarioAssociacao != null &&
                        questionarioParticipacao.TipoQuestionarioAssociacao.ID.Equals(
                            (int)enumTipoQuestionarioAssociacao.Prova))
                    {

                        decimal notaProva = 0;

                        foreach (
                            classes.ItemQuestionarioParticipacao itemQuestionarioParticipacao in
                                questionarioParticipacao.ListaItemQuestionarioParticipacao)
                        {
                            if (itemQuestionarioParticipacao.ValorAtribuido.HasValue)
                            {
                                notaProva += itemQuestionarioParticipacao.ValorAtribuido.Value;
                            }
                        }

                        Label lblNotaProva = (Label)e.Row.FindControl("lblNotaProva");
                        lblNotaProva.Text = notaProva.ToString();
                    }

                    Label lblTipoQuestionarioAssociacao = (Label)e.Row.FindControl("lblTipoQuestionarioAssociacao");
                    switch (questionarioParticipacao.TipoQuestionarioAssociacao.ID)
                    {
                        case (int)enumTipoQuestionarioAssociacao.Prova:
                            lblTipoQuestionarioAssociacao.Text = "Prova";
                            break;

                        case (int)enumTipoQuestionarioAssociacao.Pre:

                            if (questionarioParticipacao.Evolutivo)
                            {
                                lblTipoQuestionarioAssociacao.Text = "Diagnóstico Pré";
                            }
                            else
                            {
                                lblTipoQuestionarioAssociacao.Text = "Questionário Pré";
                            }

                            break;

                        case (int)enumTipoQuestionarioAssociacao.Pos:

                            if (questionarioParticipacao.Evolutivo)
                            {
                                lblTipoQuestionarioAssociacao.Text = "Diagnóstico Pós";
                            }
                            else
                            {
                                lblTipoQuestionarioAssociacao.Text = "Questionário Pós";
                            }

                            break;
                    }

                }

            }

        }

        protected void btnVerDetalhes_OnClick(object sender, EventArgs e)
        {
            int idSe, idOferta;

            if (int.TryParse(txtSolucaoEducacional.Text, out idSe) &&
                int.TryParse(txtOferta.Text, out idOferta))
            {
                var se = new ManterSolucaoEducacional().ObterSolucaoEducacionalPorId(idSe);
                var oferta = se.ListaOferta.FirstOrDefault(x => x.ID == idOferta);

                liTituloGrande.Text = se.Nome;
                liTituloPequeno.Text = oferta != null ? oferta.Nome : "";

                string contentModal = se.Apresentacao + "<p><strong>Quantidade de vagas:</strong><br /> {0}</p><p><strong>Data de incio das inscrições:</strong><br /> {1}</p><p><strong>Data fim das inscrições:</strong><br /> {2}</p><p><strong>Informações adicionais:</strong><br /> {3}</p>";
                liTextoPortal.Text = string.Format(contentModal, oferta.ObterQuantidadeVagas(), oferta.DataInicioInscricoes, oferta.DataFimInscricoes, oferta.InformacaoAdicional); ;

                pnlLupaDetalhes.Visible = true;
            }
            else
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Por favor, informe a Solução Educacional e a Oferta");
            }
        }

        protected void btnVerTermoAceite_OnClick(object sender, EventArgs e)
        {
            var idSe = int.Parse(txtSolucaoEducacional.Text);
            var termoAceite = new ManterTermoAceite().ObterPorSolucacaoEducacional(idSe);

            lblTermoAceite.Text = termoAceite.Nome;
            lblTermoAceiteTexto.Text = termoAceite.TextoTermoAceite;

            pnlTermosAceite.Visible = true;
        }

        protected void btnVerPoliticaConsequencia_OnClick(object sender, EventArgs e)
        {
            var idSe = int.Parse(txtSolucaoEducacional.Text);
            var termoAceite = new ManterTermoAceite().ObterPorSolucacaoEducacional(idSe);

            lblPoliticaConsequencia.Text = termoAceite.Nome;
            lblPoliticaConsequenciaTexto.Text = termoAceite.TextoPoliticaConsequencia;

            pnlPoliticaConsequencia.Visible = true;
        }

        protected void OcultarModal_Click(object sender, EventArgs e)
        {
            pnlLupaDetalhes.Visible = false;
            pnlTermosAceite.Visible = false;
            pnlPoliticaConsequencia.Visible = false;
            OcultarModalQuestionarioCancelamento();
        }

        public void PreencherPrograma()
        {
            var bmUsuario = new BMUsuario();
            //Por data de início e data fim e permissões de capacitação
            var listaProgramas = rblGerenciarMatriculasAbertas.SelectedValue == "S"
                ? new ManterPrograma().ObterPorInscricoesAbertas()
                : new ManterPrograma().ObterTodosProgramas();

            if (!bmUsuario.PerfilAdministrador())
            {
                int idUf = bmUsuario.ObterUfLogadoSeGestor();
                if (idUf > 0)
                    listaProgramas =
                        listaProgramas.Where(x => x.ListaPermissao.Any(l => l.Uf == null || l.Uf.ID == idUf));
            }

            ViewState["_Programa"] = Helpers.Util.ObterListaAutocomplete(listaProgramas);
        }

        public void PreencherCapacitacao()
        {
            if (!string.IsNullOrWhiteSpace(txtPrograma.Text))
            {
                var bmCapacitacao = new ManterCapacitacao();

                var lista = bmCapacitacao.ObterPorIdPrograma(int.Parse(txtPrograma.Text));

                if (rblGerenciarMatriculasAbertas.SelectedValue == "S")
                    lista =
                        new ManterCapacitacao().ObterPorIdPrograma(int.Parse(txtPrograma.Text), true)
                            .Where(x => x.PermiteMatriculaPeloGestor);

                ViewState["_Capacitacao"] = Helpers.Util.ObterListaAutocomplete(lista.OrderBy(x => x.Nome));
            }
        }

        public void PreencherModulo()
        {
            if (!string.IsNullOrWhiteSpace(txtCapacitacao.Text))
            {
                var manterModulo = new ManterModulo();
                var lista = manterModulo.ObterPorCapacitacaoIQueryable(int.Parse(txtCapacitacao.Text)).OrderBy(x => x.Nome);

                ViewState["_Modulo"] = Helpers.Util.ObterListaAutocomplete(lista);
            }
        }

        public void PreencherSolucaoEducacionalModulo()
        {
            if (!string.IsNullOrWhiteSpace(txtModulo.Text))
            {
                // TODO: Criar Manter.
                var bmModulo = new ManterModulo();
                var modulo = bmModulo.ObterPorId(int.Parse(txtModulo.Text));

                if (modulo.ListaSolucaoEducacional == null) return;

                var somenteMatriculasAbertas = rblGerenciarMatriculasAbertas.SelectedValue == "S";

                // Obtém Ids das SEs do módulo, caso o usuário só queira cursos com matrículas abertas.
                var idsSesModulo = modulo.ListaSolucaoEducacional.Where(s =>
                    !somenteMatriculasAbertas
                    ||
                    (s.SolucaoEducacional.ListaOferta.Any(o => o.DataInicioInscricoes <= DateTime.Now && o.DataFimInscricoes >= DateTime.Now)))
                .Select(s => s.ID).ToList();
                var manterSolucaoEducacional = new ManterSolucaoEducacional();
                // Obtém as Ids das SEs do Gestor, caso o usuário seja Gestor.
                //var idsSesGestor = manterSolucaoEducacional.ObterTodosPorGestor().Select(s => s.ID).ToList();

                // Obtém todas as SEs do módulo e do gestor.
                var ses = manterSolucaoEducacional.ObterTodosPorGestor()
                    .Where(s => idsSesModulo.Contains(s.ID));// || idsSesGestor.Contains(s.ID)

                ViewState["_SE"] = Helpers.Util.ObterListaAutocomplete(ses);
            }
        }

        public void PreencherSolucaoEducacional()
        {
            pnlMatricula.Visible = false;
            btnVerDetalhes.Visible = false;
            btnModoAvaliacao.Visible = false;
            btnVerTermoAceite.Visible = false;
            btnVerPoliticaConsequencia.Visible = false;

            var categorias = ucCategorias1.IdsCategoriasMarcadas.ToList();

            var matriculasAbertas = rblGerenciarMatriculasAbertas.SelectedValue == "S";

            var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

            var isGestor = usuarioLogado.IsGestor();

            var manterSe = new ManterSolucaoEducacional();

            var lista = manterSe.ObterTodosSolucaoEducacional();

            // Obtém as SEs pela UF ou pela permissão.
            if (usuarioLogado.IsGestor())
                lista = lista.Where(s =>
                    (s.UFGestor != null && s.UFGestor.ID == usuarioLogado.UF.ID) ||
                    (s.ListaPermissao.Any(p => p.Uf != null && p.Uf.ID == usuarioLogado.UF.ID)));

            lista = matriculasAbertas
                ? lista.Where(
                    s =>
                        s.ListaOferta.Any(
                            o =>
                                o.DataInicioInscricoes.HasValue && o.DataFimInscricoes.HasValue &&
                                ((o.DataInicioInscricoes.Value.Date <= DateTime.Now.Date &&
                                  o.DataFimInscricoes.Value.Date >= DateTime.Now.Date) ||
                                 (DateTime.Now.Date <= o.DataInicioInscricoes))))
                : lista.Where(
                    s =>
                        s.ListaOferta.Any(
                            o => o.DataFimInscricoes.HasValue && o.DataFimInscricoes.Value.Date < DateTime.Now.Date));

            if (usuarioLogado.UF.ID != 1) // Gestor Nacional não filtra os dados.
                lista = isGestor ? lista.Where(p => p.ListaOferta.Any(o => o.MatriculaGestorUC.HasValue && o.MatriculaGestorUC.Value)) : lista;

            // Se o usuário visualizando for consultor educacional, deve trazer somente as soluçõecujas turmas ele está vinculado.
            if (usuarioLogado.IsConsultorEducacional())
                lista =
                    lista.Where(
                        x =>
                            x.ListaOferta.Any(
                                o =>
                                    o.ListaTurma.Any(
                                        t =>
                                            t.ConsultorEducacional != null &&
                                            t.ConsultorEducacional.ID == usuarioLogado.ID)));


            // Filtra as SEs encontradas pela categoria selecionada.
            if (categorias.Any())
                lista = lista.Where(x => x.CategoriaConteudo != null && categorias.Contains(x.CategoriaConteudo.ID));

            txtSolucaoEducacional.Text = "";
            ViewState["_SE"] = Helpers.Util.ObterListaAutocomplete(lista);

            txtOferta.Text = "";
            ViewState["_Oferta"] = null;
        }

        protected void txtPrograma_OnTextChanged(object sender, EventArgs e)
        {
            txtCapacitacao.Text = "";
            ViewState["_Capacitacao"] = null;

            txtModulo.Text = "";
            ViewState["_Modulo"] = null;

            txtSolucaoEducacional.Text = "";
            ViewState["_SE"] = null;

            txtOferta.Text = "";
            ViewState["_Oferta"] = null;

            pnlMatricula.Visible = false;
            PreencherCapacitacao();
        }

        protected void txtCapacitacao_OnTextChanged(object sender, EventArgs e)
        {
            txtModulo.Text = "";
            ViewState["_Modulo"] = null;

            txtSolucaoEducacional.Text = "";
            ViewState["_SE"] = null;

            txtOferta.Text = "";
            ViewState["_Oferta"] = null;

            pnlMatricula.Visible = false;
            PreencherModulo();
        }

        protected void txtModulo_OnTextChanged(object sender, EventArgs e)
        {
            txtSolucaoEducacional.Text = "";
            ViewState["_SE"] = null;

            txtOferta.Text = "";
            ViewState["_Oferta"] = null;

            pnlMatricula.Visible = false;
            PreencherSolucaoEducacionalModulo();
        }

        protected void dgvTurmasDaOferta_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                if (!_manterUsuario.PerfilAdministrador())
                {
                    LinkButton lkbExcluir = (LinkButton)e.Row.FindControl("lkbExcluir");
                    LinkButton lkbEditar = (LinkButton)e.Row.FindControl("lkbEditar");
                    if (lkbExcluir != null)
                    {
                        lkbExcluir.Visible = false;
                        lkbEditar.Visible = false;
                    }
                }
            }
        }
        protected void btnAtualizarStatus_Click(object sender, EventArgs e)
        {
            if (ddlStatusEmLoteOferta.SelectedIndex != 0)
            {
                if (txtDtConclusaoLote.Text != string.Empty)
                {
                    try
                    {
                        List<classes.MatriculaOferta> matriculas = new List<classes.MatriculaOferta>();
                        var manterMatriculaOferta = new ManterMatriculaOferta();

                        foreach (GridViewRow row in dgvMatriculaOferta.Rows)
                        {
                            if (((System.Web.UI.WebControls.CheckBox)row.FindControl("chkAlterarStatusLote")).Checked)
                            {
                                int idMatriculaOferta;
                                HiddenField hidden = (HiddenField)row.FindControl("hdfIdMatriculaOferta");

                                if (int.TryParse(hidden.Value, out idMatriculaOferta))
                                {
                                    var matriculaOferta = manterMatriculaOferta.ObterMatriculaOfertaPorID(idMatriculaOferta);
                                    matriculas.Add(matriculaOferta);
                                }
                            }
                        }

                        var idSolucaoEducacional = 0;

                        var idOferta = 0;

                        //Se informou a solução Educacional, seta o id da Solução Educacional.
                        if (!string.IsNullOrWhiteSpace(txtSolucaoEducacional.Text))
                        {
                            idSolucaoEducacional = int.Parse(txtSolucaoEducacional.Text);
                        }

                        //Se informou a Oferta, seta o id da oferta.
                        if (!string.IsNullOrWhiteSpace(txtOferta.Text))
                        {
                            idOferta = int.Parse(txtOferta.Text);
                        }

                        var solucaoEducacional =
                            new ManterSolucaoEducacional().ObterSolucaoEducacionalPorId(idSolucaoEducacional);

                        var statusMatriculaOferta =
                            (enumStatusMatricula)
                                Enum.Parse(typeof(enumStatusMatricula), ddlStatusEmLoteOferta.SelectedItem.Value);

                        var dataConclusao = CommonHelper.TratarData(txtDtConclusaoLote.Text, "Data Conclusão do lote");

                        double? notaFinal = null;

                        if (!string.IsNullOrWhiteSpace(txtNotaFinalLote.Text))
                        {
                            double n;

                            if (double.TryParse(txtNotaFinalLote.Text.Trim(), out n))
                            {
                                notaFinal = n;
                            }
                            else
                            {
                                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Nota final do lote inválida");
                                return;
                            }
                        }

                        var usuarioLogado = _manterUsuario.ObterUsuarioLogado();

                        if (matriculas.Any())
                        {
                            var exibirMensagemFinal = true;
                            var mensagemAlerta = "";
                            var tipoMensagem = enumTipoMensagem.Atencao;

                            if (dataConclusao != null)
                            {
                                if (matriculas.Any(x => x.MatriculaTurma != null
                                    && x.MatriculaTurma.Any(m => m.Turma.DataFinal.HasValue && dataConclusao > m.Turma.DataFinal.Value) &&
                                    x.MatriculaTurma.Any(y => y.Turma.Oferta.SolucaoEducacional.CategoriaConteudo.Sigla == "ED")))
                                {
                                    throw new AcademicoException("A Data de Conclusão não pode ser superior a Data Limite da Turma em Cursos de Ensina a Distância");
                                }

                                mensagemAlerta = MontarMensagemDeRetorno(matriculas, dataConclusao, mensagemAlerta, ref tipoMensagem);
                            }

                            if (statusMatriculaOferta == enumStatusMatricula.CanceladoAdm)
                            {
                                // Obter matrículas em turmas sem questionário de cancelamento.
                                var matriculasSemQuestionario = matriculas
                                    .Where(mo =>
                                        !mo.MatriculaTurma.Any(mt =>
                                            mt.Turma.ListaQuestionarioAssociacao.Any(q =>
                                                q.TipoQuestionarioAssociacao.ID ==
                                                (int)enumTipoQuestionarioAssociacao.Cancelamento))).ToList();

                                // Obter matrículas em turmas que possuam questionário de cancelamento.
                                var matriculasComQuestionario = matriculas
                                    .Where(x => !matriculasSemQuestionario.Select(y => y.ID).Contains(x.ID)).ToList();

                                // Atualizar matrículas sem questionário.
                                foreach (var matricula in matriculasSemQuestionario)
                                    AtualizarStatusDaOferta(statusMatriculaOferta, matricula, ref manterMatriculaOferta, dataConclusao, notaFinal);

                                // Exibir questionário para matrículas com questionário.
                                if (matriculasComQuestionario.Any())
                                {
                                    var matriculasTurmasIds =
                                        matriculasComQuestionario.SelectMany(x => x.MatriculaTurma)
                                            .Select(x => x.ID)
                                            .ToList();

                                    //var csvIdMatriculasTurmas = string.Join(",", matriculasTurmas.Select(n => n.ID.ToString()).ToArray());

                                    // Obtém somente o primeiro questionário da lista de questionários disponíveis entre as turmas.
                                    var questionario = matriculasComQuestionario
                                        .FirstOrDefault().MatriculaTurma
                                        .FirstOrDefault().Turma.ListaQuestionarioAssociacao
                                        .FirstOrDefault(
                                            x =>
                                                x.TipoQuestionarioAssociacao.ID ==
                                                (int)enumTipoQuestionarioAssociacao.Cancelamento)
                                        .Questionario;

                                    // Responder questionário e alterar Status das Matrículas após responder o questionário.
                                    ucQuestionario.IniciarRespostaQuestionario(questionario, matriculasTurmasIds,
                                        enumStatusMatricula.CanceladoAdm, dataConclusao, notaFinal);

                                    ExibirModalQuestionarioCancelamento();

                                    exibirMensagemFinal = false;

                                    // Exibir mensagem de preocupação apenas se houverem ambas matrículas com e sem questionário de cancelamento.
                                    if (matriculasSemQuestionario.Any())
                                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Atencao,
                                            "Algumas matrículas foram alteradas, porém há turmas com questionários de cancelamento a serem respondidos. Responda o questionário primeiramente para alterar os Status em lote das matrículas nestas turmas.");
                                }
                            }
                            else
                            {
                                foreach (var matriculaOferta in matriculas)
                                {
                                    //Recarregada a variavel com problema, pois o Hibernate não esta carregando corretamente.
                                    var matriculaOfertaBd = new ManterMatriculaOferta().ObterMatriculaOfertaPorID(matriculaOferta.ID);
                                    AtualizarStatusDaOferta(statusMatriculaOferta, matriculaOfertaBd,
                                        ref manterMatriculaOferta, dataConclusao, notaFinal, false, true);
                                }
                            }

                            ddlStatusEmLoteOferta.SelectedIndex = 0;

                            txtDtConclusaoLote.Text = "";

                            txtNotaFinalLote.Text = "";

                            CarregarInformacoesGerais();

                            // Exibr mensagens de alerta, caso necessário
                            if (!string.IsNullOrWhiteSpace(mensagemAlerta))
                            {
                                WebFormHelper.ExibirMensagem(tipoMensagem, mensagemAlerta);
                            }
                            else
                            {
                                if (exibirMensagemFinal)
                                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Informações alteradas em lote com sucesso.");
                            }
                        }
                    }
                    catch (PoliticaConsequenciaException ex)
                    {
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    }
                    catch (Exception ex)
                    {
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    }
                }
                else
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Preencha a Data de Conclusão do Lote.");
                }
            }
            else
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Selecione o novo Status.");
            }
        }

        /// <summary>
        /// Montar uma mensagem especificando possíveis erros ou alertas na alteração em lote de matrículas.
        /// </summary>
        /// <param name="lista">Lista com todas as matrículas do lote</param>
        /// <param name="dataConclusao">Data de conclusão da matrícula</param>
        /// <param name="mensagemAlerta">Mensagem declarada fora do escopo</param>
        /// <param name="tipoMensagem">Tipo de alerta da mensagem. Caso ocorra erro de fato, se torna um enum de erro, senão, permanece como alerta.</param>
        /// <returns></returns>
        private static string MontarMensagemDeRetorno(List<classes.MatriculaOferta> lista, DateTime? dataConclusao, string mensagemAlerta,
            ref enumTipoMensagem tipoMensagem)
        {
            // Obtém as matrículas em turmas cuja data de fim seja menor que a data de conclusão informada no lote.
            var matriculasComPrazoEstourado =
                lista.Where(
                    x =>
                        x.MatriculaTurma != null &&
                        x.MatriculaTurma.Any(
                            m => m.Turma.DataFinal.HasValue && dataConclusao > m.Turma.DataFinal.Value));

            var matriculasComPrazoAdiantado =
                lista.Where(
                    x =>
                        x.MatriculaTurma != null &&
                        x.MatriculaTurma.Any(
                            m => dataConclusao < m.Turma.DataInicio));

            // Obtém as turmas com prazo estourado distintamente.
            var qtTurmasComPrazoEstourado = matriculasComPrazoEstourado
                .SelectMany(x => x.MatriculaTurma)
                .Select(x => x.Turma.ID)
                .Distinct()
                .Count();

            // Obtém as turmas com prazo adiantado distintamente.
            var qtTurmasComPrazoAdiantado = matriculasComPrazoAdiantado
                .SelectMany(x => x.MatriculaTurma)
                .Select(x => x.Turma.ID)
                .Distinct()
                .Count();

            // Caso existam matrículas com datas posteriores à data fim da turma, exibe uma mensagem ao usuário
            // dizendo que existem essas matrículas matrículas. Concatena com os textos certos para plurais, pra ficar lindo.
            if (matriculasComPrazoEstourado.Any())
            {
                mensagemAlerta =
                    string.Format(
                        "A data de conclusão informada é maior em {0} turma{1}, mas os dados foram salvos.",
                        qtTurmasComPrazoEstourado,
                        qtTurmasComPrazoEstourado > 1 ? "s" : "");
            }


            // Caso existam matrículas com datas anteriores à data fim da turma,
            // exibe uma mensagem ao usuário dizendo que há matrículas que não tiveram suas datas
            // de conclusão alteradas. Concatena com os textos certos para plurais, pra ficar lindo.
            if (matriculasComPrazoAdiantado.Any())
            {
                tipoMensagem = enumTipoMensagem.Erro;

                var ctMatriculas = matriculasComPrazoAdiantado.Count();

                mensagemAlerta += (!string.IsNullOrWhiteSpace(mensagemAlerta) ? Environment.NewLine : "") +
                                  string.Format(
                                      "A data de conclusão informada é menor que a data de início em {0} turma{1}. Portanto, {2} matrícula{3} não {4} alteraç{5} na data de conclusão.",
                                      qtTurmasComPrazoAdiantado,
                                      qtTurmasComPrazoAdiantado > 1 ? "s" : "",
                                      ctMatriculas,
                                      ctMatriculas > 1 ? "s" : "",
                                      ctMatriculas > 1 ? "tiveram" : "teve",
                                      ctMatriculas > 1 ? "ões" : "ão");
            }

            return mensagemAlerta;
        }

        protected void btnEnviarArquivo_Click(object sender, EventArgs e)
        {
            if ((fileUpload.PostedFile != null) && (fileUpload.PostedFile.ContentLength > 0))
            {
                try
                {
                    #region Upload do arquivo

                    var arquivos = new ManterConfiguracaoSistema().ObterConfiguracaoSistemaPorID((int)enumConfiguracaoSistema.EnderecoArquivos);
                    string ext = Path.GetExtension(fileUpload.PostedFile.FileName);
                    string caminhoDoArquivo = (arquivos.Registro + "/uploads/" + Guid.NewGuid().ToString() + ext);
                    fileUpload.PostedFile.SaveAs(caminhoDoArquivo);

                    #endregion

                    #region Preenchendo gridView

                    // Conexão para criar objeto ADO.
                    string connstring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + caminhoDoArquivo +
                                        ";Extended Properties='Excel 8.0;HDR=NO;IMEX=1';";

                    using (OleDbConnection conn = new OleDbConnection(connstring))
                    {
                        conn.Open();
                        //Recupera o nome de todas as tabelas
                        DataTable tabelas = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables,
                            new object[] { null, null, null, "Table" });

                        //Recupera o nome da primeira tabela
                        string nomeDaPrimeiraTabela = tabelas.Rows[0][2].ToString();

                        //Query String 
                        string sql = string.Format("SELECT F1, F2, F3 FROM [{0}] WHERE F3 IS NOT NULL AND F1 IS NOT NULL AND F1 NOT LIKE 'Nome' ORDER BY F1 ASC",
                            nomeDaPrimeiraTabela);
                        OleDbDataAdapter ada = new OleDbDataAdapter(sql, connstring);
                        DataSet set = new DataSet();
                        ada.Fill(set);
                        grdMatriculaEmLote.DataSource = set;
                        grdMatriculaEmLote.DataBind();
                    }

                    #endregion

                    // Excluindo arquivo usado para exportação.
                    File.Delete(caminhoDoArquivo);

                    // Chamando script para retornar aberto o collapse no postback
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "script", "matriculasEmLote()");
                }
                catch (Exception ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
            }
            else
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Atencao, "Selecione um arquivo para enviar");
            }
        }

        public void PopularStatusMatricula(ListControl dropdown)
        {
            var listaStatusMatricula = new ManterStatusMatricula().ObterTodosIncluindoEspecificos();
            WebFormHelper.PreencherLista(listaStatusMatricula, dropdown, false, true);
        }

        public void PopularTurmas(ListControl dropdown)
        {
            int idOferta = 0;

            //Se informou a Oferta, seta o id da oferta.
            if (!string.IsNullOrWhiteSpace(txtOferta.Text))
            {
                idOferta = int.Parse(txtOferta.Text);
            }

            SetarTurmasDaOferta(idOferta);

            WebFormHelper.PreencherLista(TurmasDaOferta, dropdown, false, true);
        }

        protected void grdMatriculaEmLote_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            var drpStatusMatricula = (DropDownList)e.Row.FindControl("ddlStatusOfertaMatriculaEmLote");
            var drpTurma = (DropDownList)e.Row.FindControl("ddlTurmaEmLote");

            if (drpStatusMatricula != null && drpTurma != null)
            {
                PopularStatusMatricula(drpStatusMatricula);
                PopularStatusMatricula(ddlStatusTodosEmLote);
                PopularTurmas(drpTurma);
                PopularTurmas(ddlTurmaTodosEmLote);

                lblMatriculaTodos.Visible = true;
                ddlStatusTodosEmLote.Visible = true;

                lblTurmaTodos.Visible = true;
                ddlTurmaTodosEmLote.Visible = true;

                btnMatricularEmLote.Visible = true;
            }
        }

        protected void btnMatricularEmLote_OnClick(object sender, EventArgs e)
        {
            var manterMatriculaOferta = new ManterMatriculaOferta();
            var manterMatriculaTurma = new ManterMatriculaTurma();
            var matriculas = new List<Tuple<classes.MatriculaOferta, classes.MatriculaTurma>>();
            bool naoEncontrado = false;
            bool naoIncluido = false;

            try
            {
                foreach (GridViewRow row in grdMatriculaEmLote.Rows)
                {
                    var matriculaOferta = new classes.MatriculaOferta();
                    matriculaOferta.Oferta = new ManterOferta().ObterOfertaPorID(int.Parse(txtOferta.Text));

                    var dropdownSelecionado = (DropDownList)row.FindControl("ddlStatusOfertaMatriculaEmLote");
                    matriculaOferta.StatusMatricula = (enumStatusMatricula)int.Parse(dropdownSelecionado.SelectedValue);

                    int idStatus;
                    if (int.TryParse(dropdownSelecionado.SelectedValue, out idStatus) && idStatus <= 0)
                    {
                        throw new AcademicoException("Selecione um status matrícula");
                    }

                    var drpTurma = (DropDownList)row.FindControl("ddlTurmaEmLote");

                    int idTurma;
                    if (int.TryParse(drpTurma.SelectedValue, out idTurma) && idTurma <= 0)
                    {
                        throw new AcademicoException("Selecione uma turma");
                    }

                    var cpf = row.Cells[2].Text.PadLeft(11, '0');

                    var usuario = _manterUsuario.ObterPorCPF(cpf);

                    if (usuario != null)
                    {
                        matriculaOferta.Usuario = usuario;
                        matriculaOferta.UF = usuario.UF;
                        matriculaOferta.NivelOcupacional = usuario.NivelOcupacional;

                        try
                        {
                            manterMatriculaOferta.VerificarPoliticaDeConsequencia(matriculaOferta);
                        }
                        catch
                        {
                            row.BackColor = Color.Red;
                            naoIncluido = true;
                            continue;
                        }

                        var matriculaTurma = new classes.MatriculaTurma();
                        matriculaTurma.MatriculaOferta = matriculaOferta;

                        matriculaTurma.Turma = new ManterTurma().ObterTurmaPorID(int.Parse(drpTurma.SelectedValue));

                        matriculas.Add(new Tuple<classes.MatriculaOferta, classes.MatriculaTurma>(matriculaOferta, matriculaTurma));
                    }
                    else
                    {
                        naoEncontrado = true;
                        row.BackColor = Color.Yellow;
                    }
                }

                foreach (var item in matriculas)
                {
                    manterMatriculaOferta.IncluirMatriculaOferta(item.Item1);
                    manterMatriculaTurma.IncluirMatriculaTurma(item.Item2);
                }

                if (naoEncontrado || naoIncluido)
                {
                    string mensagem = "Os registros marcados não foram incluidos. \n";

                    mensagem += naoEncontrado ? "Amarelo: Usuário não encontrado. \n" : "";
                    mensagem += naoIncluido ? "Verde: Usuário bloqueado pelas políticas de consequência. \n" : "";
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, mensagem);
                    PreencherSolucaoEducacional();
                    return;
                }

                grdMatriculaEmLote.DataSource = null;
                grdMatriculaEmLote.DataBind();

                BuscarMatriculaOferta();

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Matriculas realizadas com sucesso !");

                ddlStatusTodosEmLote.Visible = false;
                lblTurmaTodos.Visible = false;
                ddlTurmaTodosEmLote.Visible = false;
                lblMatriculaTodos.Visible = false;
                btnMatricularEmLote.Visible = false;
            }
            catch (AcademicoException Ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, Ex.Message);
            }
            catch (Exception Ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, Ex.Message);

                grdMatriculaEmLote.DataSource = null;
                grdMatriculaEmLote.DataBind();

                ddlStatusTodosEmLote.Visible = false;

                lblTurmaTodos.Visible = false;
                ddlTurmaTodosEmLote.Visible = false;

                lblMatriculaTodos.Visible = false;
                btnMatricularEmLote.Visible = false;
            }
        }

        protected void ddlStatusTodosEmLote_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in grdMatriculaEmLote.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    var drp = (DropDownList)row.FindControl("ddlStatusOfertaMatriculaEmLote");
                    if (drp != null)
                    {
                        drp.SelectedValue = ddlStatusTodosEmLote.SelectedValue;
                    }
                }
            }
        }

        protected void ddlTurmaTodosEmLote_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in grdMatriculaEmLote.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    var drp = (DropDownList)row.FindControl("ddlTurmaEmLote");
                    if (drp != null)
                    {
                        drp.SelectedValue = ddlTurmaTodosEmLote.SelectedValue;
                    }
                }
            }
        }

        protected void lkbMatricularAluno_OnClick(object sender, EventArgs e)
        {
            AlterarStatusTab(lkbMatricularAluno, collapseMatriculaOferta);
            var matriculasAbertas = rblGerenciarMatriculasAbertas.SelectedValue == "S";


        }

        protected void lkbMatricularAlunosEmLote_OnClick(object sender, EventArgs e)
        {
            AlterarStatusTab(lkbMatricularAlunosEmLote, collapseAlunosEmLote);
        }

        private void exibirOcultarAlteracaoEmLote(bool selecionar)
        {
            foreach (GridViewRow row in dgvMatriculaOferta.Rows)
            {
                var checkbox = row.FindControl("chkAlterarStatusLote");
                checkbox.Visible = selecionar;
            }
        }

        protected void lkbAlterarStatusEmLote_OnClick(object sender, EventArgs e)
        {
            exibirOcultarAlteracaoEmLote(true);

            SetarListaComStatusDeMatricula();

            var listaStatusMatricula = ListaStatusMatricula;

            WebFormHelper.PreencherLista(listaStatusMatricula, ddlStatusEmLoteOferta, false, true);

            if (AlterarStatusTab(lkbAlterarStatusEmLote, collapseStatusEmLote))
            {
                exibirOcultarAlteracaoEmLote(true);
            }
            else
            {
                exibirOcultarAlteracaoEmLote(false);
            }
        }

        protected void lnkDownloadPlanilha_OnClick(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog { Filter = "(*.xls)|*.xlsx" };

            var enderecoPasta = new ManterConfiguracaoSistema().ObterConfiguracaoSistemaPorID((int)enumConfiguracaoSistema.EnderecoArquivos);
            try
            {
                var file = enderecoPasta.Registro + "/planilha.xlsx";
                Response.AppendHeader("content-disposition", "attachment; filename=planilha.xlsx");
                Response.ContentType = "application/octet-stream";
                Response.TransmitFile(file);
            }
            catch (Exception)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Não foi possível baixar o arquivo");
            }
        }

        protected void btnSalvarTurma_OnClick(object sender, EventArgs e)
        {
            try
            {
                ucTurma1.SalvarTurma();

                OcultarModalTurma();

                CarregarInformacoesGerais();
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        protected void btnEnviarQuestionario_OnClick(object sender, EventArgs e)
        {
            ucQuestionario.SalvarQuestionario();
            OcultarModalQuestionarioCancelamento();

            CarregarInformacoesGerais();
        }

        protected void lnkPage_Click(object sender, EventArgs e)
        {
            var linkButton = sender as LinkButton;

            if (linkButton == null) return;

            var pageIndex = int.Parse(linkButton.CommandArgument);

            BuscarMatriculaOferta(pageIndex);

            //#2060
            //PopulateMatriculaOfertaPager(matriculasFiltradas.Count, pageIndex);
        }

        private void PopulateMatriculaOfertaPager(int recordCount, int currentPage)
        {
            var dblPageCount = (double)(recordCount / Convert.ToDecimal(QuantidadeMatriculasPorPagina));

            var pageCount = (int)Math.Ceiling(dblPageCount);

            var pages = new List<ListItem>();

            if (pageCount > 0)
            {
                for (var i = 0; i < pageCount; i++)
                {
                    pages.Add(new ListItem((i + 1).ToString(), i.ToString(), (i == currentPage)));
                }
            }
            rptMatriculaOfertaPager.DataSource = pages;
            rptMatriculaOfertaPager.DataBind();
        }

        /// <summary>
        /// Permitir edição da matrícula apenas caso o curso atual não seja da Formação de Multiplicadores. Em resumo: uma gambi aí.
        /// Mas falando sério, esse método vai verificar se o curso é da FM e possibilitar que o Consultor Educacional responda o
        /// questionário de avaliação dos alunos, e permitir ao Gestor avaliar estes questionários do Consultor Educacional.
        /// </summary>
        /// <param name="usuarioLogado">Caso o código que chame este método já tenha buscado o usuário logado, ele pode ser informado aqui para evitar uma consulta extra.</param>
        private void PrepararModoAvaliacao(classes.Usuario usuarioLogado = null)
        {
            // Certificar que o usuário logado não seja null.
            usuarioLogado = usuarioLogado ?? _manterUsuario.ObterUsuarioLogado();

            if (usuarioLogado.IsAdministrador())
            {
                InModoDeAvaliacao = false;
                return;
            }

            int ofertaId;

            if (int.TryParse(txtOferta.Text, out ofertaId))
            {
                var oferta = new ManterOferta().ObterOfertaPorID(ofertaId);

                var solucao = oferta.SolucaoEducacional;

                if (solucao != null)
                {
                    var configFm = new ManterConfiguracaoSistema().ObterConfiguracaoSistemaPorID(
                        (int)enumConfiguracaoSistema.IdFormacaoDeFormadores);

                    int fmId;

                    if (configFm != null && int.TryParse(configFm.Registro, out fmId))
                    {
                        var fmIds = new ManterCategoriaConteudo().ObterTodasCategoriasFilhas(fmId).Select(x => x.ID).ToList();

                        if (fmIds.Any() && fmIds.Contains(solucao.CategoriaConteudo.ID))
                        {
                            // Caso todas estas condições sejam satisfeitas, poderá mudar para o modo de avaliação.
                            InModoDeAvaliacao = true;
                            btnModoAvaliacao.Visible = true;

                            // Buscar turma que possui consultor educacional.
                            var turma = oferta.ListaTurma.FirstOrDefault(x => x.ConsultorEducacional != null);

                            if (turma != null)
                            {
                                var avaliacao = turma.Avaliacoes.FirstOrDefault();

                                if (avaliacao != null)
                                {
                                    switch (avaliacao.Status)
                                    {
                                        case enumStatusAvaliacao.AguardandoResposta:

                                            // Caso o usuário logado não seja o consultor educacional da turma, esconde o botão.
                                            if (usuarioLogado.IsConsultorEducacional())
                                            {
                                                if (turma.ConsultorEducacional.ID == usuarioLogado.ID)
                                                {
                                                    btnModoAvaliacao.Text = "Inserir resultados";
                                                    btnModoAvaliacao.Enabled = true;
                                                    return;
                                                }
                                            }

                                            if (usuarioLogado.IsGestor() && turma.ConsultorEducacional.UF.ID == usuarioLogado.UF.ID)
                                            {
                                                btnModoAvaliacao.Text = "Analisar resultados";
                                                btnModoAvaliacao.Enabled = false;
                                                return;
                                            }

                                            btnModoAvaliacao.Visible = false;

                                            break;
                                        case enumStatusAvaliacao.AguardandoGestor:
                                            if (usuarioLogado.IsGestor())
                                            {
                                                if (turma.ConsultorEducacional.UF.ID == usuarioLogado.UF.ID)
                                                {
                                                    btnModoAvaliacao.Text = "Analisar resultados";
                                                }
                                                else
                                                {
                                                    btnModoAvaliacao.Visible = false;
                                                }

                                                return;
                                            }

                                            if (usuarioLogado.IsConsultorEducacional())
                                            {
                                                btnModoAvaliacao.Text = "Visualizar preenchimento";
                                                return;
                                            }

                                            btnModoAvaliacao.Visible = false;

                                            break;
                                        case enumStatusAvaliacao.Aprovada:
                                            btnModoAvaliacao.Text = "Avaliação finalizada";
                                            btnModoAvaliacao.Enabled = false;

                                            break;
                                        default:
                                            throw new ArgumentOutOfRangeException();
                                    }
                                }
                                else
                                {
                                    // Caso não tenha avaliação, então nem começou ainda, portanto somente o Consultor Educacional deve visualizar o botão.
                                    if (usuarioLogado.IsConsultorEducacional())
                                    {
                                        btnModoAvaliacao.Text = "Inserir resultados";
                                    }

                                    if (usuarioLogado.IsGestor() && turma.ConsultorEducacional.UF.ID == usuarioLogado.UF.ID)
                                    {
                                        btnModoAvaliacao.Text = "Analisar resultados";
                                        btnModoAvaliacao.Enabled = false;
                                        return;
                                    }
                                }

                                return;
                            }
                        }
                    }
                }
            }

            btnModoAvaliacao.Visible =
            InModoDeAvaliacao = false;
        }

        protected void btnModoAvaliacao_OnClick(object sender, EventArgs e)
        {
            RedirecionarPaginaAvaliacao();
        }

        private void RedirecionarPaginaAvaliacao()
        {
            int ofertaId;

            if (int.TryParse(txtOferta.Text, out ofertaId))
            {
                var oferta = new ManterOferta().ObterOfertaPorID(ofertaId);

                if (oferta != null)
                {
                    var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

                    // Obter somente a turma do Consultor Educacional atual.
                    var turma =
                        oferta.ListaTurma.FirstOrDefault(
                            x => x.ConsultorEducacional != null && x.ConsultorEducacional.ID == usuarioLogado.ID);

                    if (turma != null)
                    {
                        if (!turma.PodeVisualizarAvaliacao(usuarioLogado))
                        {
                            WebFormHelper.ExibirMensagem(enumTipoMensagem.Atencao,
                                "Esta turma ainda não foi avaliada. Aguarde o aviso do Consultor Educacional.");
                            return;
                        }

                        Response.Redirect("AvaliarTurma.aspx?Id=" + turma.ID);
                    }

                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Turma não existente. Tente novamente.");
                }
                else
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Oferta inválida ou removida. Tente novamente.");
                }
            }
        }

        protected void rptQuestoes_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                var questao = (classes.Questao)e.Item.DataItem;

                var ltrTitulo = (Literal)e.Item.FindControl("ltrTitulo");
                var ltrQuestao = (Literal)e.Item.FindControl("ltrQuestao");

                ltrTitulo.Text = questao.Titulo;
                ltrQuestao.Text = questao.Pergunta;
            }
        }

        protected void rptMatriculas_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                var matricula = (classes.MatriculaTurma)e.Item.DataItem;

                var hdnIdMatriculaTurma = (HiddenField)e.Item.FindControl("hdnIdMatriculaTurma");
                var ltrNome = (Literal)e.Item.FindControl("ltrNome");
                var ltrUf = (Literal)e.Item.FindControl("ltrUf");

                hdnIdMatriculaTurma.Value = matricula.ID.ToString();
                ltrNome.Text = matricula.MatriculaOferta.Usuario.Nome;
                ltrUf.Text = matricula.MatriculaOferta.UF.Sigla;

                var rptQuestaoResposta = (Repeater)e.Item.FindControl("rptQuestaoResposta");

                var avaliacao = matricula.Turma.Avaliacoes.FirstOrDefault();

                rptQuestaoResposta.DataSource = ObterQuestoesResposta(matricula, avaliacao);
                rptQuestaoResposta.DataBind();
            }
        }

        /// <summary>
        /// Obtém a listagem de questão resposta. Caso não tenha, cria os objetos agora. Caso possua parcialmente, obtém somente os preenchidos e cria o resto.
        /// </summary>
        /// <returns></returns>
        private IList<classes.QuestaoResposta> ObterQuestoesResposta(classes.MatriculaTurma matricula,
            classes.Avaliacao avaliacao)
        {
            var retorno = new List<classes.QuestaoResposta>();

            for (var i = 0; i < QuantidadeQuestoes; i++)
            {
                var questao = Questoes[i];

                classes.QuestaoResposta resposta;

                // Obtém ou cria uma nova resposta utilizando mágica.
                if (avaliacao != null)
                {
                    resposta = avaliacao.Respostas.FirstOrDefault(x => x.MatriculaTurma.ID == matricula.ID && x.Questao.ID == questao.ID) ??
                               new classes.QuestaoResposta
                               {
                                   Questao = questao,
                                   MatriculaTurma = matricula
                               };
                }
                else
                {
                    resposta = new classes.QuestaoResposta
                    {
                        Questao = questao,
                        MatriculaTurma = matricula
                    };
                }

                retorno.Add(resposta);
            }

            return retorno;
        }

        protected void rptQuestaoResposta_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var questaoResposta = (classes.QuestaoResposta)e.Item.DataItem;

            var literalValor = (Literal)e.Item.FindControl("ltrValor");

            if (questaoResposta.Questao.Tipo == enumTipoQuestao.Dominio)
            {
                var ltrLabel = (Literal)e.Item.FindControl("ltrLabel");
                ltrLabel.Visible = true;
                ltrLabel.Text = "Domínio";

                literalValor.Text = questaoResposta.Dominio.HasValue ? questaoResposta.Dominio.Value.GetDescription() : "";
            }

            if (questaoResposta.Questao.Tipo == enumTipoQuestao.Dissertativa)
            {
                literalValor.Text = questaoResposta.Comentario;
            }

            if (questaoResposta.Questao.Tipo == enumTipoQuestao.Resultado)
            {
                literalValor.Text = questaoResposta.StatusMatricula != null
                    ? ((enumStatusMatricula)questaoResposta.StatusMatricula.ID).GetDescription()
                    : "";
            }
        }

        private void selecionarTodosAlteracaoEmLote(bool selecionar)
        {
            foreach (GridViewRow row in dgvMatriculaOferta.Rows)
            {
                System.Web.UI.WebControls.CheckBox checkbox = (System.Web.UI.WebControls.CheckBox)row.FindControl("chkAlterarStatusLote");
                checkbox.Checked = selecionar;
            }
        }

        protected void chkTodosAlteracaoEmLote_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTodosAlteracaoEmLote.Checked)
            {
                selecionarTodosAlteracaoEmLote(true);
            }
            else
            {
                selecionarTodosAlteracaoEmLote(false);
            }
        }

        protected void txtSolucaoEducacional_OnTextChanged(object sender, EventArgs e)
        {
            AdministrarSelecaoSolucaoOfertaTurma(EnumSelecao.Solucao);
        }

        protected void txtOferta_OnTextChanged(object sender, EventArgs e)
        {
            //AdministrarSelecaoSolucaoOfertaTurma(EnumSelecao.Oferta);

            lblMatriculaTodos.Visible = false;
            lblTurmaTodos.Visible = false;
            btnMatricularEmLote.Visible = false;
            ddlStatusTodosEmLote.Visible = false;
            ddlTurmaTodosEmLote.Visible = false;
            btnVerDetalhes.Visible = false;

            if (InformouSolucaoEducacional() && InformouOferta())
            {
                HabilitarBotaoAdicionarTurma();
                HabilitarBotaoEditarOferta();
                CarregarInformacoesGerais();
                MostrarTab(lkbAlunosMatriculados, collapseMatriculados);
                ucMatriculaOferta1.PreencherComboTurma();
                AplicarPermissoesDoGestor();
            }
            else
            {
                if (string.IsNullOrWhiteSpace(txtOferta.Text))
                {
                    //Esconde o painel com informações 
                    pnlMatricula.Visible = false;
                    btnModoAvaliacao.Visible = false;
                }
            }
            grdMatriculaEmLote.DataSource = null;
            grdMatriculaEmLote.DataBind();
        }

        protected void txtTurma_OnTextChanged(object sender, EventArgs e)
        {
            AdministrarSelecaoSolucaoOfertaTurma(EnumSelecao.Turma);
        }

        /// <summary>
        /// Método que administra toda a seleção de Usuário, Solução, Oferta e Turma no mesmo lugar. Muita coisa
        /// das regras são repetidas, portanto fica melhor manter o código em um só lugar.
        /// </summary>
        private void AdministrarSelecaoSolucaoOfertaTurma(EnumSelecao selecao, bool chamadaRecursiva = false)
        {
            if (chamadaRecursiva == false)
            {
                lblMatriculaTodos.Visible = false;
                lblTurmaTodos.Visible = false;
                btnMatricularEmLote.Visible = false;
                ddlStatusTodosEmLote.Visible = false;
                ddlTurmaTodosEmLote.Visible = false;

                grdMatriculaEmLote.DataSource = null;
                grdMatriculaEmLote.DataBind();

                AplicarPermissoesDoGestor();
            }

            switch (selecao)
            {
                case EnumSelecao.Solucao:
                    if (string.IsNullOrWhiteSpace(txtSolucaoEducacional.Text))
                    {
                        AdministrarSelecaoSolucaoOfertaTurma(EnumSelecao.Usuario, true);

                        txtOferta.Text = "";
                        ViewState["_Oferta"] = null;
                    }
                    else
                    {
                        var seId = int.Parse(txtSolucaoEducacional.Text);

                        AplicarPermissoesDoFornecedor();

                        PreencherOferta();

                        var termoAceite = new ManterTermoAceite().ObterPorSolucacaoEducacional(seId);

                        pnlLupaDetalhes.Visible = false;
                        btnVerDetalhes.Visible = false;
                        btnModoAvaliacao.Visible = false;

                        if (termoAceite != null)
                        {
                            btnVerTermoAceite.Visible = true;
                            btnVerPoliticaConsequencia.Visible = true;
                        }
                        else
                        {
                            btnVerTermoAceite.Visible = false;
                            btnVerPoliticaConsequencia.Visible = false;
                        }
                    }

                    break;
                case EnumSelecao.Oferta:
                    if (string.IsNullOrWhiteSpace(txtOferta.Text))
                    {
                        AdministrarSelecaoSolucaoOfertaTurma(EnumSelecao.Solucao, true);
                    }
                    else
                    {
                        var ofertaId = int.Parse(txtOferta.Text);

                        btnVerDetalhes.Visible = false;

                        CarregarInformacoesGerais();
                        ucMatriculaOferta1.PreencherComboTurma();
                    }

                    break;
            }
        }

        private enum EnumSelecao
        {
            Usuario,
            Solucao,
            Oferta,
            Turma
        }

        private bool validarSelecaoCampos()
        {
            if (string.IsNullOrEmpty(txtOferta.Text))
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Selecione uma oferta");
                return false;
            }

            return true;
        }
    }
}