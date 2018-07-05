using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.WebForms.Cadastros.MatriculaCapacitacao;

namespace Sebrae.Academico.WebForms.UserControls
{
    public partial class ucMatriculaCapacitacao : System.Web.UI.UserControl
    {
        #region "Eventos referentes ao Cancelamento"

        public delegate void MatriculaDeUmAlunoEmUmaOfertaCancelada(object sender, MatricularAlunoEmUmaOfertaEventArgs e);
        public event MatriculaDeUmAlunoEmUmaOfertaCancelada CancelouMatriculaDeUmAlunoEmUmaOferta;

        #endregion

        #region "Eventos referentes à matricula de um aluno em uma oferta"

        public delegate void MatriculaDeUmAlunoEmUmaCapacitacaoRealizada(object sender, MatricularAlunoEmUmaCapacitacaoEventArgs e);
        public event MatriculaDeUmAlunoEmUmaCapacitacaoRealizada MatriculouAlunoEmUmaCapacitacao;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PreencherCombos();
            }
        }

        #region "Atributos Privados"

        public int IdCapacitacao
        {
            get
            {
                if (ViewState["ViewStateIdCapacitacao"] != null)
                {
                    return (int)ViewState["ViewStateIdCapacitacao"];
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                ViewState["ViewStateIdCapacitacao"] = value;
            }
        }

        #endregion

        public void LimparCampos()
        {
            this.LupaUsuario.LimparCampos();
            this.ddlStatus.ClearSelection();
        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                BMMatriculaCapacitacao manterMatriculaCapacitacao = new BMMatriculaCapacitacao();

                MatriculaCapacitacao matriculaCapacitacao = this.ObterObjetoMatriculaCapacitacao();
                manterMatriculaCapacitacao.Salvar(matriculaCapacitacao);

                if (ddlTurmaCapacitacao.SelectedIndex > 0)
                {
                    MatriculaTurmaCapacitacao matriculaTurmaCapacitacao = ObterObjetoMatriculaTurmaCapacitacao(matriculaCapacitacao.ID);
                    new BMMatriculaTurmaCapacitacao().Salvar(matriculaTurmaCapacitacao);
                }

                //Dispara o evento informando que a matricula em uma oferta foi realizada
                if (MatriculouAlunoEmUmaCapacitacao != null)
                {
                    //Obtem as informações da matricula Oferta (inclusive a lista de turmas da oferta)
                    matriculaCapacitacao = manterMatriculaCapacitacao.ObterPorId(matriculaCapacitacao.ID);

                    MatriculouAlunoEmUmaCapacitacao(this, new MatricularAlunoEmUmaCapacitacaoEventArgs(matriculaCapacitacao));
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "myscript", " $('#EdicaoNivel, #modal-background').removeClass('active');", true);
                }

                GerenciarMatriculaCapacitacao gerenciarMatriculaCapacitacao = (GerenciarMatriculaCapacitacao)this.Page;
                gerenciarMatriculaCapacitacao.PreencherMatriculas(); 

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados cadastrados com sucesso.");
                
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                return;
            }
        }

        private MatriculaTurmaCapacitacao ObterObjetoMatriculaTurmaCapacitacao(int idMatriculaCapacitacao)
        {
            MatriculaTurmaCapacitacao matriculaTurmaCapacitacao = new MatriculaTurmaCapacitacao();
            matriculaTurmaCapacitacao.TurmaCapacitacao = new BMTurmaCapacitacao().ObterPorId(int.Parse(ddlTurmaCapacitacao.SelectedValue));
            matriculaTurmaCapacitacao.DataMatricula = DateTime.Now;
            matriculaTurmaCapacitacao.MatriculaCapacitacao = new BMMatriculaCapacitacao().ObterPorId(idMatriculaCapacitacao);

            return matriculaTurmaCapacitacao;
        }

        private MatriculaCapacitacao ObterObjetoMatriculaCapacitacao()
        {
            MatriculaCapacitacao matriculaCapacitacao = new MatriculaCapacitacao();

            //Usuário
            if (this.LupaUsuario.SelectedUser != null)
            {
                matriculaCapacitacao.Usuario = this.LupaUsuario.SelectedUser;
            }

            //Oferta
            if (this.IdCapacitacao > 0)
            {
                matriculaCapacitacao.Capacitacao = new Capacitacao() { ID = this.IdCapacitacao };
            }

            //UF
            matriculaCapacitacao.UF = matriculaCapacitacao.Usuario.UF;

            //Nivel Ocupacional
            matriculaCapacitacao.NivelOcupacional = matriculaCapacitacao.Usuario.NivelOcupacional;

            //Status
            if (this.ddlStatus.SelectedItem != null && !string.IsNullOrWhiteSpace(this.ddlStatus.SelectedItem.Value))
            {
                matriculaCapacitacao.StatusMatricula = (enumStatusMatricula)int.Parse(this.ddlStatus.SelectedItem.Value);
            }

            matriculaCapacitacao.DataInicio = DateTime.Now;

            return matriculaCapacitacao;
        }

        protected void btnCancelarMatricula_Click(object sender, EventArgs e)
        {
            this.TratarCancelamento();
        }

        private void TratarCancelamento()
        {
            if (CancelouMatriculaDeUmAlunoEmUmaOferta != null)
            {
                //if ((!this.AcaoDaTela.Equals((int)enumAcaoTelaQuestionario.EdicaoDeUmItem))
                //    && !this.AcaoDaTela.Equals((int)enumAcaoTelaQuestionario.EdicaoDeUmaResposta))
                //{
                //    this.ExcluirItemDoQuestionarioDaListaDaSessao();
                //}

                CancelouMatriculaDeUmAlunoEmUmaOferta(this, null);

                //Dispara o evento de cancelamento da matrícula de um aluno
                //CancelouMatriculaDeUmAlunoEmUmaOferta(this, new MatricularAlunoEmUmaOfertaEventArgs(this.MatriculaOfertaDaSessao));

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "myscript", " $('#EdicaoNivel, #modal-background').removeClass('active');", true);
            }
        }


        public void PreencherCamposDaMatriculaOferta(MatriculaOferta pMatriculaOferta)
        {
            if (pMatriculaOferta != null)
            {
                this.PrepararTelaParaEdicaoDeUmaMatriculaOferta(pMatriculaOferta);
            }
            else
            {
                this.PrepararTelaParaInclusaoDeUmaMatricula();
            }
        }

        private void PrepararTelaParaInclusaoDeUmaMatricula()
        {
            this.ZerarIds();
            this.SetarAcaoDaTela(enumAcaoTelaMatriculaOferta.NovaMatricula);
        }


        private void ZerarIds()
        {
            //TODO -> Customizar
        }

        private void PrepararTelaParaEdicaoDeUmaMatriculaOferta(MatriculaOferta pMatriculaOferta)
        {

            //Oferta
            if (pMatriculaOferta.Oferta != null && !pMatriculaOferta.Oferta.ID.Equals(0))
            {
                this.PreencherCombos();
                //WebFormHelper.SetarValorNaCombo(pMatriculaOferta.Oferta.ID.ToString(), ddlOferta, false);
            }

            this.SetarAcaoDaTela(enumAcaoTelaMatriculaOferta.EdicaoDeUmaMatricula);
        }

        #region "Métodos Privados"

        private void SetarAcaoDaTela(enumAcaoTelaMatriculaOferta acaoTelaMatriculaOferta)
        {
            this.AcaoDaTela = (int)acaoTelaMatriculaOferta;
        }

        #endregion

        public int AcaoDaTela
        {
            get
            {
                if (ViewState["ViewStateAcaoDaTelaDeMatriculaOferta"] != null)
                {
                    return (int)ViewState["ViewStateAcaoDaTelaDeMatriculaOferta"];
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                ViewState["ViewStateAcaoDaTelaDeMatriculaOferta"] = value;
            }

        }

        #region "Enumeração"

        /// <summary>
        /// Enumeração referente às ações feitas na tela de matricula de oferta em uma turma.
        /// </summary>
        private enum enumAcaoTelaMatriculaOferta
        {
            NovaMatricula = 1,
            EdicaoDeUmaMatricula = 2,
        }

        #endregion

        private void PreencherCombos()
        {
            try
            {
                this.PreencherComboStatus();
                this.PreencherComboTurma();
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public void PreencherComboTurma()
        {
            if (this.IdCapacitacao > 0)
            {
                var turmas = new BMTurmaCapacitacao().ObterPorCapacitacao(this.IdCapacitacao);

                var bmUsuario = new BMUsuario();
                if (!bmUsuario.PerfilAdministrador())
                {
                    int ufGestor = bmUsuario.ObterUfLogadoSeGestor();
                    if (ufGestor > 0)
                        turmas = turmas.Where(t => t.ListaPermissao.Any(p => p.Uf == null || p.Uf.ID == ufGestor)).ToList();
                }

                if (turmas.Count() > 0)
                {
                    ddlTurmaCapacitacao.Enabled = true;
                    WebFormHelper.PreencherLista(turmas, ddlTurmaCapacitacao, false, true);
                }
            }
        }

        private void PreencherComboStatus()
        {
            ManterStatusMatricula manterStatusMatricula = new ManterStatusMatricula();
            IList<StatusMatricula> ListaStatus = manterStatusMatricula.ObterTodosStatusMatricula();

            //Busca o status CanceladoAluno para remover o mesmo da lista
            StatusMatricula statusCanceladoAluno = ListaStatus.FirstOrDefault(x => x.ID == (int)enumStatusMatricula.CanceladoAluno);
            ListaStatus.Remove(statusCanceladoAluno);

            IList<StatusMatricula> ListaStatusAuxiliar = ListaStatus.Where(x => x.ID == (int)enumStatusMatricula.Inscrito ||
                                                                           x.ID == (int)enumStatusMatricula.PendenteConfirmacaoAluno).ToList();

            WebFormHelper.PreencherLista(ListaStatusAuxiliar, this.ddlStatus, false, true);
        }



    }

    public class MatricularAlunoEmUmaCapacitacaoEventArgs : EventArgs
    {
        //public Usuario UsuarioSelecionado { get; set; }

        public MatriculaCapacitacao InformacoesDaMatriculaOfertaRealizada { get; set; }

        public MatricularAlunoEmUmaCapacitacaoEventArgs(MatriculaCapacitacao matriculaCapacitacao)
        {
            InformacoesDaMatriculaOfertaRealizada = matriculaCapacitacao;
        }
    }

}