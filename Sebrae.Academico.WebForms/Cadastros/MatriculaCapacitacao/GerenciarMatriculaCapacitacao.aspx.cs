using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.WebForms.UserControls;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.BP;

namespace Sebrae.Academico.WebForms.Cadastros.MatriculaCapacitacao
{
    public partial class GerenciarMatriculaCapacitacao : PageBase
    {
        BMUsuario bmUsuario = new BMUsuario();
        bool? isAdmin;

        protected override enumFuncionalidade Funcionalidade
        {
            get
            {
                return enumFuncionalidade.GerenciamentoMatricula;
            }
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
            pnlMatricula.Visible = false;

            if (!Page.IsPostBack)
            {
                PreencherProgramas();                
                WebFormHelper.PreencherComponenteComOpcoesSimNao(rblGerenciarMatriculasAbertas, false);
                WebFormHelper.SetarValorNoRadioButtonList(false, rblGerenciarMatriculasAbertas);
            }
        }

        public void ddlProgama_SelectedIndexChanged(object sender, EventArgs e)
        {
            dvFormularioTurma.Visible = false;
            if (ddlProgama.SelectedIndex > 0)
            {
                var lista = new BMCapacitacao().ObterPorIdPrograma(int.Parse(ddlProgama.SelectedValue)).ToList();
                

                if (!bmUsuario.PerfilAdministrador())
                {
                    //Retirei pois é uma regra que deve ser aplicada a turmas
                    //int ufGestor = bmUsuario.ObterUFLogadoSeGestor();
                    //if (ufGestor > 0)
                    //    lista = lista.Where(x => x.ListaTurmas.Any(t => t.ListaPermissao.Any(p => p.Uf == null || p.Uf.ID == ufGestor))).ToList();

                    if (rblGerenciarMatriculasAbertas.SelectedValue == "S")
                    {
                        lista = new BMCapacitacao().ObterPorIdPrograma(int.Parse(ddlProgama.SelectedValue), true).Where(x=>x.PermiteMatriculaPeloGestor).ToList();
                    }
                }

                if (lista.Count() > 0)
                {
                    ddlCapacitacao.Enabled = true;
                    WebFormHelper.PreencherLista(lista, ddlCapacitacao, false, true);
                }
            }
        }

        public void rblGerenciarMatriculasAbertas_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            PreencherProgramas();
            ddlCapacitacao.Items.Clear();
            PreencherMatriculas();
        }

        private void PreencherProgramas()
        {
            var listaProgramas = new ManterPrograma().ObterTodosProgramas();

            if (!bmUsuario.PerfilAdministrador())
            {
                var idUf = bmUsuario.ObterUfLogadoSeGestor();

                if (idUf > 0)
                    listaProgramas = listaProgramas.Where(x => x.ListaPermissao.Any(l => l.Uf == null || l.Uf.ID == idUf));

                //Por data de início e data fim e permissões de capacitação
                if (rblGerenciarMatriculasAbertas.SelectedValue == "S")
                {
                    listaProgramas = new ManterPrograma().ObterPorInscricoesAbertas();
                }
            }

            WebFormHelper.PreencherLista(listaProgramas.ToList(), ddlProgama, false, true);
        }

        public void ddlCapacitacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            dvFormularioTurma.Visible = false;
            if (ddlCapacitacao.SelectedIndex > 0)
            {
                this.ucMatriculaOferta1.IdCapacitacao = int.Parse(ddlCapacitacao.SelectedValue);
                PreencherMatriculas();

                var capacitacao = new BMCapacitacao().ObterPorId(this.ucMatriculaOferta1.IdCapacitacao);

                if (rblGerenciarMatriculasAbertas.SelectedValue == "S" && capacitacao != null && capacitacao.PermiteMatriculaPeloGestor)
                    dvMatricularAluno.Visible = true;                    
                else
                    dvMatricularAluno.Visible = false;
            }
            else
            {
                pnlMatricula.Visible = false;
            }
        }

        public void PreencherMatriculas()
        {
            if (ddlCapacitacao.SelectedIndex > 0)
            {
                var bmMatriculaCapacitacao = new BMMatriculaCapacitacao();
                var listarMatriculaCapacitacao = bmMatriculaCapacitacao.ObterPorCapacitacao(int.Parse(ddlCapacitacao.SelectedValue));

                if (!bmUsuario.PerfilAdministrador())
                {
                    int idUF = bmUsuario.ObterUfLogadoSeGestor();
                    listarMatriculaCapacitacao = listarMatriculaCapacitacao.Where(x => x.UF.ID == idUF).ToList();
                }

                if (listarMatriculaCapacitacao.Count() > 0)
                {
                    pnlMatricula.Visible = true;
                    WebFormHelper.PreencherGrid(listarMatriculaCapacitacao, dgvMatriculaCapacitacao);
                }
            }
        }

        protected void dgvMatriculaCapacitacao_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idMatriculaTurma = int.Parse(e.CommandArgument.ToString());

            if (e.CommandName.Equals("editarMatTurma"))
            {

            }

            if (e.CommandName.Equals("enviarMatricula"))
            {
                
            }

            if (e.CommandName.Equals("enviarEmailPendente"))
            {
            }
        }

        protected void dgvMatriculaCapacitacao_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {

            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                this.TratarComboTurma(e);
                this.TratarComboStatusOferta(e);

                Dominio.Classes.MatriculaCapacitacao matriculaOferta = (Dominio.Classes.MatriculaCapacitacao)e.Row.DataItem;

                if (matriculaOferta != null && matriculaOferta.ID > 0)
                {
                    HiddenField hdfIdMatriculaOferta = (HiddenField)e.Row.FindControl("hdfIdMatriculaOferta");

                    if (hdfIdMatriculaOferta != null)
                    {
                        hdfIdMatriculaOferta.Value = matriculaOferta.ID.ToString();
                    }
                }

            }

        }

        protected void ddlTurma_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList comboTurma = (DropDownList)sender;

            if (comboTurma != null && comboTurma.SelectedItem != null)
            {
                try
                {
                    //Obtém a linha da grid onde a dropdownlist (combo) de oferta, está contida
                    GridViewRow linhadaGrid = (GridViewRow)comboTurma.NamingContainer;

                    if (linhadaGrid != null)
                    {
                        HiddenField hdfIdMatriculaOferta = (HiddenField)linhadaGrid.FindControl("hdfIdMatriculaOferta");
                        BMMatriculaCapacitacao bmMatriculaCapacitacao = new BMMatriculaCapacitacao();
                        DropDownList ddlStatusOferta = (DropDownList)linhadaGrid.FindControl("ddlStatusOferta");
                        DropDownList ddlTurma = (DropDownList)linhadaGrid.FindControl("ddlTurma");

                        BMMatriculaTurmaCapacitacao bmMatriculaTurmaCapacitacao = new BMMatriculaTurmaCapacitacao();

                        MatriculaTurmaCapacitacao matriculaTurmaCapacitacao = bmMatriculaTurmaCapacitacao.ObterPorMatriculaCapacitacao(Convert.ToInt32(hdfIdMatriculaOferta.Value)).FirstOrDefault();

                        if (matriculaTurmaCapacitacao == null)
                            matriculaTurmaCapacitacao = new MatriculaTurmaCapacitacao();

                        int idTurma = ddlTurma.SelectedIndex > 0 ? int.Parse(ddlTurma.SelectedValue) : 0;

                        if (idTurma > 0 &&
                            (matriculaTurmaCapacitacao.TurmaCapacitacao == null ||
                             matriculaTurmaCapacitacao.TurmaCapacitacao.ID != idTurma))
                        {
                            // Alteração otimizada para não precisar realizar uma consulta por um objeto TurmaCapacitacao.
                            // Em vez disso cria um novo, pois tudo que o NHibernate precisa pra alter o ID no banco
                            // é o ID da TurmaCapacitacao.
                            matriculaTurmaCapacitacao.TurmaCapacitacao = new Dominio.Classes.TurmaCapacitacao
                            {
                                ID = idTurma
                            };

                            if (matriculaTurmaCapacitacao.ID == 0)
                            {
                                matriculaTurmaCapacitacao.DataMatricula = DateTime.Now;
                                matriculaTurmaCapacitacao.MatriculaCapacitacao =
                                    new BMMatriculaCapacitacao().ObterPorId(Convert.ToInt32(hdfIdMatriculaOferta.Value));
                            }
                            bmMatriculaTurmaCapacitacao.Salvar(matriculaTurmaCapacitacao);
                        }
                        else
                        {
                            bmMatriculaTurmaCapacitacao.Excluir(matriculaTurmaCapacitacao);
                        }
                    }
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados alterados com sucesso!");
                }
                catch
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Ocorreu um erro na edição, por favor, tente novamente mais tarde!");
                }
            }
        }

        private void TratarComboTurma(System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            //Procura o dropdownlist de turma nas linhas da grid
            DropDownList ddlTurma = (DropDownList)e.Row.FindControl("ddlTurma");

            //Se encontrou o dropdownlist de turma, seta o status da matricula turma com o status da oferta
            if (ddlTurma != null)
            {
                Dominio.Classes.MatriculaCapacitacao matriculaCapacitacao = (Dominio.Classes.MatriculaCapacitacao)e.Row.DataItem;

                if (matriculaCapacitacao != null)
                {
                    WebFormHelper.PreencherLista(new BMTurmaCapacitacao().ObterPorCapacitacao(int.Parse(ddlCapacitacao.SelectedValue)), ddlTurma, false, true);

                    Dominio.Classes.MatriculaTurmaCapacitacao matriculaTurmaCapacitacao = new BMMatriculaTurmaCapacitacao().ObterPorMatriculaCapacitacao(matriculaCapacitacao.ID).FirstOrDefault();

                    if (matriculaTurmaCapacitacao != null)
                    {
                        WebFormHelper.SetarValorNaCombo(matriculaTurmaCapacitacao.TurmaCapacitacao.ID.ToString(), ddlTurma);
                    }

                }
            }
        }

        private void TratarComboStatusOferta(GridViewRowEventArgs e)
        {
            if (isAdmin.Equals(null))
            {
                isAdmin = bmUsuario.PerfilAdministrador();
            }

            //Procura o dropdownlist de status oferta nas linhas da grid
            DropDownList ddlStatusOferta = (DropDownList)e.Row.FindControl("ddlStatusOferta");

            if (ddlStatusOferta != null)
            {
                Dominio.Classes.MatriculaCapacitacao matriculaCapacitacao = (Dominio.Classes.MatriculaCapacitacao)e.Row.DataItem;

                if (matriculaCapacitacao != null)
                {
                    IList<StatusMatricula> listaStatusMatricula = new BMStatusMatricula().ObterTodosIncluindoEspecificos();//new ManterStatusMatricula().ObterTodosStatusMatricula();
                    
                    int idStatusMatricula = (int)matriculaCapacitacao.StatusMatricula;
                    WebFormHelper.PreencherLista(listaStatusMatricula, ddlStatusOferta);
                    WebFormHelper.SetarValorNaCombo(idStatusMatricula.ToString(), ddlStatusOferta);

                    // Caso não seja administrador e a capacitação não permitir a alteração de status pelo gestor
                    if (isAdmin.HasValue && !isAdmin.Value && !matriculaCapacitacao.Capacitacao.PermiteAlterarSituacao)
                    {
                        ddlStatusOferta.Enabled = false;
                    }
                }
            }
        }

        protected void lkbMatricularAluno_Click(object sender, EventArgs e)
        {
            this.ucMatriculaOferta1.PreencherComboTurma();
            PreencherMatriculas();
            base.AlterarStatusTab(this.lkbMatricularAluno, collapseMatriculaOferta);
            base.AlterarStatusTab(this.lkbMatricularAluno, collapseMatriculaOferta);
        }

        protected void MatriculaOferta_MatriculouAlunoEmUmaOferta(object sender, MatricularAlunoEmUmaCapacitacaoEventArgs e)
        {

            int idPrograma = e.InformacoesDaMatriculaOfertaRealizada.Capacitacao.Programa.ID;
            int idCapacitacao = e.InformacoesDaMatriculaOfertaRealizada.Capacitacao.ID;
            
            ucMatriculaOferta1.LimparCampos();
        }

        protected void ddlStatusOferta_SelectedIndexChanged(object sender, EventArgs e)
        {

            DropDownList comboStatusOferta = (DropDownList)sender;

            if (comboStatusOferta != null && comboStatusOferta.SelectedItem != null &&
                !string.IsNullOrWhiteSpace(comboStatusOferta.SelectedItem.Value))
            {
                try
                {
                    //Obtém a linha da grid onde a dropdownlist (combo) de oferta, está contida
                    GridViewRow linhadaGrid = (GridViewRow)comboStatusOferta.NamingContainer;

                    if (linhadaGrid != null)
                    {
                        HiddenField hdfIdMatriculaOferta = (HiddenField)linhadaGrid.FindControl("hdfIdMatriculaOferta");
                        var manterMatriculaCapacitacao = new ManterMatriculaCapacitacao();
                        DropDownList ddlStatusOferta = (DropDownList)linhadaGrid.FindControl("ddlStatusOferta");
                        DropDownList ddlTurma = (DropDownList)linhadaGrid.FindControl("ddlTurma");

                        if (ddlStatusOferta != null){
                            var statusMatriculaOferta = (enumStatusMatricula)Enum.Parse(typeof(enumStatusMatricula), ddlStatusOferta.SelectedItem.Value);
                            if (statusMatriculaOferta.Equals(enumStatusMatricula.CanceladoAluno)){
                                //O usuário do admin, pode ver o status do Cancelado/Aluno, mas nunca pode setar esse status
                                WebFormHelper.ExibirMensagem(enumTipoMensagem.Atencao, string.Format("Apenas o aluno pode atribuir o status de cancelado pelo aluno"));
                                return;
                            }
                            var matriculaCapacitacao = manterMatriculaCapacitacao.ObterPorId(int.Parse(hdfIdMatriculaOferta.Value));
                            if (matriculaCapacitacao == null) {
                                WebFormHelper.ExibirMensagem(enumTipoMensagem.Atencao, string.Format("Matrícula não encontrada."));
                                return;
                            }
                            matriculaCapacitacao.StatusMatricula = statusMatriculaOferta;
                            manterMatriculaCapacitacao.AtualizarMatriculaCapacitacao(matriculaCapacitacao);
                            WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, statusMatriculaOferta.Equals(enumStatusMatricula.CanceladoAdm) ? string.Format("Cancelamento efetuado com sucesso!") : "O Status da Matrícula da Turma foi Atualizado com Sucesso !");
                        }

                    }
                }catch (AcademicoException ex){
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
            }
        }

        public void lkbCriarTurma(object sender, EventArgs e)
        {
            if (ddlCapacitacao.SelectedIndex > 0)
            {
                PreencherTurmaCapacitacao();
                dvFormularioTurma.Visible = true;
            }
            else
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Você deve escolher uma capacitação para ver ou criar as turmas");
            }
        }

        public void PreencherTurmaCapacitacao()
        {
            var listaTurmas = new BMTurmaCapacitacao().ObterPorCapacitacao(int.Parse(ddlCapacitacao.SelectedValue));

            if (!bmUsuario.PerfilAdministrador() && bmUsuario.ObterUfLogadoSeGestor() > 0)
            {
                listaTurmas = listaTurmas.Where(x=>x.ListaPermissao.Any(p=>p.Uf == null || p.Uf.ID == bmUsuario.ObterUfLogadoSeGestor())).ToList();

                txtNomeTurma.Text = ddlCapacitacao.SelectedItem.Text + ". " + bmUsuario.ObterUsuarioLogado().UF.Sigla + " - " + (listaTurmas.Count() + 1);
                txtNomeTurma.Enabled = false;
            }

            gvTurmas.DataSource = listaTurmas;
            gvTurmas.DataBind();
        }

        public void btnCriarTurma_OnClick(object sender, EventArgs e)
        {
            int qtdeVagas = 0;

            if (string.IsNullOrEmpty(txtNomeTurma.Text) || string.IsNullOrEmpty(txtQuantidadeVagasTurma.Text))
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Você deve informar o nome da turma e a quantidade de vagas");
            }
            else if (!(int.TryParse(txtQuantidadeVagasTurma.Text, out qtdeVagas)))
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "A quantidade de vagas deve ser numérica");
            }
            else
            {
                Dominio.Classes.TurmaCapacitacao turmaCapacitacao = new Dominio.Classes.TurmaCapacitacao();
                turmaCapacitacao.Nome = txtNomeTurma.Text;
                turmaCapacitacao.Capacitacao = new BMCapacitacao().ObterPorId(int.Parse(ddlCapacitacao.SelectedValue));
                                
                TurmaCapacitacaoPermissao turmaCapacitacaoPermissao = new TurmaCapacitacaoPermissao();
                turmaCapacitacaoPermissao.Uf = new BMUf().ObterPorId(bmUsuario.ObterUsuarioLogado().UF.ID);
                turmaCapacitacaoPermissao.QuantidadeVagasPorEstado = qtdeVagas;
                turmaCapacitacaoPermissao.TurmaCapacitacao = turmaCapacitacao;
                turmaCapacitacao.ListaPermissao.Add(turmaCapacitacaoPermissao);

                var filtro = new BMTurmaCapacitacao().ObterPorFiltro(turmaCapacitacao);

                if (filtro.Count() > 0)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "A turma já está cadastrada.");
                }
                else
                {
                    new BMTurmaCapacitacao().Salvar(turmaCapacitacao);
                    PreencherTurmaCapacitacao();
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Turma cadastrada com sucesso.");
                }
            }
        }

        private void CarregarInformacoesGerais()
        {

        }
    }
}