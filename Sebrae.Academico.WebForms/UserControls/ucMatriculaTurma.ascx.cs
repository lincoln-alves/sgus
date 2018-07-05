using System;
using System.Web.UI.WebControls;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System.Collections.Generic;

namespace Sebrae.Academico.WebForms.UserControls
{
    public partial class ucMatriculaTurma : System.Web.UI.UserControl
    {
        #region "Eventos referentes à matricula de um aluno em uma turma"

        public delegate void MatriculaDeUmAlunoEmUmaTurmaRealizada(object sender, MatricularAlunoEmUmaTurmaEventArgs e);
        public event MatriculaDeUmAlunoEmUmaTurmaRealizada MatriculouAlunoEmUmaTurma;

        #endregion

        #region "Eventos referentes à prova selecionada"

        public delegate void ProvaDeUmAlunoSelecionada(object sender, SelecionarProvaDeUmAlunoEventArgs e);
        public event ProvaDeUmAlunoSelecionada SelecionouUmaProva;

        #endregion

        /// <summary>
        /// ID da Matrícula Turma. O ID da Matrícula Turma é persistido (armazenado) no viewstate da página.
        /// </summary>
        public int IdMatriculaTurma
        {
            get
            {
                if (ViewState["ViewStateIdMatriculaTurma"] != null)
                {
                    return (int)ViewState["ViewStateIdMatriculaTurma"];
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                ViewState["ViewStateIdMatriculaTurma"] = value;
            }

        }

        private void ocultaCamposNota()
        {
            divNotas.Visible = false;
            txtNota1.Text = txtNota2.Text = txtMediaFinal.Text = "";
        }

        private void PreencherInformacoesDaMatriculaTurma(MatriculaTurma matriculaTurma)
        {
            if (matriculaTurma != null)
            {
                var status = new ManterStatusMatricula().ObterStatusMatriculaPorID((int)matriculaTurma.MatriculaOferta.StatusMatricula);

                if (status != null)
                {
                    txtStatusMatricula.Text = status.Nome;
                }

                txtDataMatricula.Text = matriculaTurma.DataMatricula.ToString();
                txtDataLimite.Text = matriculaTurma.CalcularDataLimite().ToString();

                if (matriculaTurma.MatriculaOferta.Oferta.TipoOferta != enumTipoOferta.Continua)
                {
                    txtDataLimite.Text = matriculaTurma.Turma.DataFinal.ToString();
                    txtDataLimite.ReadOnly = true;
                } 

                if (matriculaTurma.MatriculaOferta.StatusMatricula == enumStatusMatricula.Inscrito
                    || matriculaTurma.MatriculaOferta.StatusMatricula == enumStatusMatricula.FilaEspera)
                {
                    txtDataTermino.Text = "";
                    txtDataTermino.ReadOnly = true;
                }
                else
                {
                    //Data Termino
                    if (matriculaTurma.DataTermino.HasValue)
                    {
                        txtDataTermino.Text = matriculaTurma.DataTermino.ToString();
                    }
                    else
                    {
                        txtDataTermino.Text = "";
                    }
                }

                if (matriculaTurma.Turma.InAvaliacaoAprendizagem)
                {
                    if (matriculaTurma.MatriculaOferta.StatusMatricula == enumStatusMatricula.Aprovado)
                    {
                        if (matriculaTurma.MatriculaOferta.IsOuvinte())
                        {
                            ocultaCamposNota();
                        }
                        else
                        {
                            divNotas.Visible = true;

                            //Nota 1
                            txtNota1.Text = matriculaTurma.Nota1.HasValue ? matriculaTurma.Nota1.ToString() : "";

                            //Nota 2
                            txtNota2.Text = matriculaTurma.Nota2.HasValue ? matriculaTurma.Nota2.ToString() : "";

                            //Média
                            txtMediaFinal.Text = matriculaTurma.MediaFinal.HasValue ? matriculaTurma.MediaFinal.ToString() : "";
                        }
                    }
                    else
                    {
                        ocultaCamposNota();
                    }
                }
                else
                {
                    ocultaCamposNota();
                }

                txtObservacoes.Text = matriculaTurma.Observacao;
                txtFeedback.Text = matriculaTurma.Feedback;

                if (!new BMUsuario().PerfilAdministrador())
                {
                    if (matriculaTurma.MatriculaOferta.Oferta.AlteraPeloGestorUC.HasValue && !(matriculaTurma.MatriculaOferta.Oferta.AlteraPeloGestorUC.Value))
                    {
                        btnEnviar.Visible = false;
                    }
                }

                if (new BMUsuario().PerfilAdministrador())
                {
                    txtDataMatricula.ReadOnly = false;
                }
                
                txtPresencas.Text = matriculaTurma.Presencas > 0 ? matriculaTurma.Presencas.ToString() : "";
                txtPresencasTotais.Text = matriculaTurma.TotalPresencas > 0 ? matriculaTurma.TotalPresencas.ToString() : "";
            }
        }

        private MatriculaTurma ObterObjetoMatriculaTurma()
        {
            MatriculaTurma matriculaTurma;

            matriculaTurma = IdMatriculaTurma == 0
                ? new MatriculaTurma()
                : new ManterMatriculaTurma().ObterMatriculaTurmaPorId(IdMatriculaTurma);


            if (!matriculaTurma.MatriculaOferta.IsOuvinte())
            {
                //Nota 1
                if (!string.IsNullOrWhiteSpace(txtNota1.Text))
                {
                    double nota1;
                    if (!double.TryParse(txtNota1.Text.Trim(), out nota1))
                        throw new AcademicoException("Valor Inválido para o campo Nota 1.");

                    matriculaTurma.Nota1 = nota1;
                }
                else
                {
                    matriculaTurma.Nota1 = null;
                }

                //Nota 2
                if (!string.IsNullOrWhiteSpace(txtNota2.Text))
                {
                    double nota2;
                    if (!double.TryParse(txtNota2.Text.Trim(), out nota2))
                        throw new AcademicoException("Valor Inválido para o campo Nota 2.");

                    matriculaTurma.Nota2 = nota2;
                }
                else
                {
                    matriculaTurma.Nota2 = null;
                }

                //Média Final
                if (!string.IsNullOrWhiteSpace(txtMediaFinal.Text))
                {
                    double mediaFinal;
                    if (!double.TryParse(txtMediaFinal.Text.Trim(), out mediaFinal))
                        throw new AcademicoException("Valor Inválido para o campo Média Final.");

                    matriculaTurma.MediaFinal = mediaFinal;
                }
                else
                {
                    matriculaTurma.MediaFinal = null;
                }
            }

            //Data Limite
            if (!string.IsNullOrWhiteSpace(this.txtDataLimite.Text))
            {
                DateTime dataLimite;
                if (!DateTime.TryParse(this.txtDataLimite.Text.Trim(), out dataLimite))
                    throw new AcademicoException("Valor Inválido para o campo Data Limite.");
                else
                    matriculaTurma.DataLimite = dataLimite;
            }
            else
            {
                throw new AcademicoException("A data limite é obrigatória.");
            }

            // Data Matrícula
            DateTime dataMatricula;
            if (DateTime.TryParse(txtDataMatricula.Text, out dataMatricula))
            {
                if (!matriculaTurma.MatriculaOferta.Oferta.DataInicioInscricoes.HasValue || !matriculaTurma.MatriculaOferta.Oferta.DataFimInscricoes.HasValue) {
                    throw new AcademicoException("Oferta sem data inicio ou data fim de inscrições, informação necessária para validações no sistema.");
                }

                if (dataMatricula < matriculaTurma.MatriculaOferta.Oferta.DataInicioInscricoes.Value.Date)
                {
                    throw new AcademicoException("A data de início da matrícula é menor que a data de início de inscrições da oferta.");
                }

                if (dataMatricula > matriculaTurma.MatriculaOferta.Oferta.DataFimInscricoes.Value.Date)
                {
                    throw new AcademicoException("A data de início da matrícula é maior que a data de término de inscrições da oferta.");
                }

                matriculaTurma.DataMatricula = dataMatricula;
            }
            else
            {
                throw new AcademicoException("Valor Inválido para o campo Data de Matrícula.");
            }

            //Data Termino
            if (!string.IsNullOrWhiteSpace(txtDataTermino.Text))
            {
                DateTime dataTermino;

                if (!DateTime.TryParse(txtDataTermino.Text.Trim(), out dataTermino))
                    throw new AcademicoException("Valor Inválido para o campo data de término.");

                if (dataTermino.Date < matriculaTurma.Turma.DataInicio.Date)
                    throw new AcademicoException("A data término não pode ser menor que a data de início da turma.");

                if (dataTermino < matriculaTurma.DataMatricula)
                    throw new AcademicoException("A data término não pode ser menor que a data de matrícula.");

                matriculaTurma.DataTermino = dataTermino.AddHours(23).AddMinutes(59);
            }
            else
            {
                matriculaTurma.DataTermino = null;
            }

            // Observações
            if (!string.IsNullOrWhiteSpace(this.txtObservacoes.Text))
            {
                matriculaTurma.Observacao = this.txtObservacoes.Text;
            }
            else
            {
                matriculaTurma.Observacao = null;
            }
            if (!string.IsNullOrEmpty(this.txtFeedback.Text))
            {
                matriculaTurma.Feedback = this.txtFeedback.Text;
            }
            else
            {
                matriculaTurma.Feedback = null;
            }

            return matriculaTurma;
        }

        private void validaNotasMatricula(MatriculaTurma matriculaTurma)
        {
            if (matriculaTurma.Turma.InAvaliacaoAprendizagem)
            {
                if (matriculaTurma.MatriculaOferta.StatusMatricula == enumStatusMatricula.Aprovado)
                {
                    if (matriculaTurma.Nota1 != null && (matriculaTurma.Nota1 < 0 || matriculaTurma.Nota1 > 10))
                    {
                        throw new AcademicoException("O campo Nota 1 deve ser um numero entre 0 e 10.");
                    }

                    if (matriculaTurma.Nota2 != null && (matriculaTurma.Nota2 < 0 || matriculaTurma.Nota2 > 10))
                    {
                        throw new AcademicoException("O campo Nota 2 deve ser um numero entre 0 e 10.");
                    }

                    if (matriculaTurma.MediaFinal == null || (matriculaTurma.MediaFinal < 0 || matriculaTurma.MediaFinal > 10))
                    {
                        throw new AcademicoException("O campo Média Final deve ser um numero entre 0 e 10.");
                    }

                    if (matriculaTurma.MediaFinal < (double)matriculaTurma.Turma.NotaMinima)
                    {
                        throw new AcademicoException(string.Format("A(s) nota(s) informada(s) deve(m) ser maior ou igual a {0}", matriculaTurma.Turma.NotaMinima));
                    }
                }
            }
        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                MatriculaTurma matriculaTurma = this.ObterObjetoMatriculaTurma();
                ManterMatriculaTurma manterMatriculaTurma = new ManterMatriculaTurma();

                validaNotasMatricula(matriculaTurma);

                if(matriculaTurma.Turma.Oferta.TipoOferta != enumTipoOferta.Continua && matriculaTurma.DataTermino > matriculaTurma.DataLimite)
                {
                    throw new AcademicoException("A Data de Conclusão não pode ser maior que a Data Limite da Turma.");
                }

                var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();
                matriculaTurma.Auditoria = new Auditoria(usuarioLogado.CPF);

                manterMatriculaTurma.AlterarMatriculaTurma(matriculaTurma);

                if (MatriculouAlunoEmUmaTurma != null)
                {
                    MatriculouAlunoEmUmaTurma(this, new MatricularAlunoEmUmaTurmaEventArgs(matriculaTurma));
                }
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                return;
            }
        }

        internal void PreencherCamposDaMatriculaTurma(MatriculaTurma matriculaTurma)
        {
            if (matriculaTurma != null)
            {
                this.PrepararTelaParaEdicaoDeUmaTurma(matriculaTurma);
            }
        }

        public void PrepararTelaParaEdicaoDeUmaTurma(MatriculaTurma matriculaTurma)
        {
            if (matriculaTurma != null)
            {
                this.IdMatriculaTurma = matriculaTurma.ID;
                this.PreencherInformacoesDaMatriculaTurma(matriculaTurma);

                this.ExibirPanelDeProvasRealizadas();

                PreencherGridComProvasRealizadas(matriculaTurma);
            }
        }

        private void PreencherGridComProvasRealizadas(MatriculaTurma matriculaTurma)
        {
            try
            {
                ManterQuestionarioParticipacao manterQuestionarioParticipacao = new ManterQuestionarioParticipacao();
                //IList<QuestionarioParticipacao> ListaProvasDoUsuario = manterQuestionarioParticipacao.ObterProvasDaTrilhaDoUsuario(usuarioTrilha.Usuario.ID, usuarioTrilha.TrilhaNivel.ID);
                IList<QuestionarioParticipacao> ListaProvasDoUsuario = manterQuestionarioParticipacao.ObterProvasDaTurmaDoUsuario(matriculaTurma.MatriculaOferta.Usuario.ID, matriculaTurma.Turma.ID);
                WebFormHelper.PreencherGrid(ListaProvasDoUsuario, this.dgvProvasRealizadas);
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private void ExibirPanelDeProvasRealizadas()
        {
            this.pnlProvasRealizadas.Visible = true;
        }

        private void EsconderPanelDeProvasRealizadas()
        {
            this.pnlProvasRealizadas.Visible = false;
        }

        #region "Eventos do Grid"

        protected void dgvProvasRealizadas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("verprova"))
            {
                try
                {
                    int idQuestionarioParticipacao = int.Parse(e.CommandArgument.ToString());

                    if (this.SelecionouUmaProva != null)
                    {
                        QuestionarioParticipacao provaSelecionada = new ManterQuestionarioParticipacao().ObterQuestionarioParticipacaoPorId(idQuestionarioParticipacao);
                        SelecionouUmaProva(this, new SelecionarProvaDeUmAlunoEventArgs(provaSelecionada, this.IdMatriculaTurma));
                    }
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }

            }
        }

        protected void dgvProvasRealizadas_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {

            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {

                QuestionarioParticipacao questionarioParticipacao = (QuestionarioParticipacao)e.Row.DataItem;

                if (questionarioParticipacao != null && questionarioParticipacao.ID > 0)
                {

                    decimal percObtido;

                    ManterUsuarioTrilha manterUt;

                    UsuarioTrilha ut;

                    var manterQuestionarioParticipacao = new BP.Services.SgusWebService.ManterQuestionarioParticipacao();

                    manterQuestionarioParticipacao.CalcularPercentualDaProva(questionarioParticipacao, out percObtido, out manterUt, out ut);

                    //Calcula as notas somente para as provas
                    if (questionarioParticipacao.TipoQuestionarioAssociacao != null &&
                        questionarioParticipacao.TipoQuestionarioAssociacao.ID.Equals((int)enumTipoQuestionarioAssociacao.Prova))
                    {

                        decimal notaProva = 0;

                        foreach (ItemQuestionarioParticipacao itemQuestionarioParticipacao in questionarioParticipacao.ListaItemQuestionarioParticipacao)
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

        #endregion


    }

    public class SelecionarProvaDeUmAlunoEventArgs : EventArgs
    {
        public QuestionarioParticipacao ProvaSelecionada { get; set; }
        public int IdMatriculaTurma { get; set; }

        public SelecionarProvaDeUmAlunoEventArgs(QuestionarioParticipacao provaSelecionada, int idMatriculaTurma)
        {
            this.ProvaSelecionada = provaSelecionada;
            this.IdMatriculaTurma = idMatriculaTurma;
        }
    }

    public class MatricularAlunoEmUmaTurmaEventArgs : EventArgs
    {
        public MatriculaTurma InformacoesDaMatriculaTurmaRealizada { get; set; }

        public MatricularAlunoEmUmaTurmaEventArgs(MatriculaTurma matriculaTurma)
        {
            InformacoesDaMatriculaTurmaRealizada = matriculaTurma;
        }
    }

}