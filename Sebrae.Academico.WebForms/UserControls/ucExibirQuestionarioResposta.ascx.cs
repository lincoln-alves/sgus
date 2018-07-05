using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.UserControls
{
    public partial class ucExibirQuestionarioResposta : System.Web.UI.UserControl
    {
        #region "Eventos Relacionados ao Histórico de Pagamento"

        public delegate void RespostaExibida(object sender, DetalheDaRespostaDaProvaEventArgs e);
        public event RespostaExibida ExibiuResposta;

        public class DetalheDaRespostaDaProvaEventArgs : EventArgs
        {
            public int IdMatriculaTurma { get; set; }

            public DetalheDaRespostaDaProvaEventArgs(int pIdMatriculaTurma)
            {
                IdMatriculaTurma = pIdMatriculaTurma;
            }
        }

        #endregion

        private void AdicionaBrNaDiv()
        {
            //Adiciona um br
            this.dvDetalhesDaProva.InnerHtml = "<br />";
        }

        public void ExibirInformacoesDetalhadasDaProvaDoAluno(QuestionarioParticipacao questionarioParticipacao, int idMatriculaTurma)
        {
            this.IdMatriculaTurma = idMatriculaTurma;
            IList<ItemQuestionarioParticipacao> ListaDePerguntas = questionarioParticipacao.ListaItemQuestionarioParticipacao;
            StringBuilder sbQuestao = new StringBuilder();

            if (questionarioParticipacao.Evolutivo)
            {
                sbQuestao.AppendLine(@"
                                 <table class='table table-condensed table-bordered'>
				                    <thead>
					                    <tr>
						                    <td colspan='4' align='center'>Formulário de Diagnóstico</td>
					                    </tr>
				                    </thead>          
                                    <tr>
						                <td>Tema</td>
						                <td>Objetivos</td>
						                <td>Domínio</td>
						                <td>Importância</td>
					                </tr>");
                foreach (ItemQuestionarioParticipacao pergunta in ListaDePerguntas)
                {
                    string respostaDominioTexto = string.Empty;
                    string respostaImportanciaTexto = string.Empty;

                    var respostaDominio = pergunta.ListaOpcoesParticipacao.Where(x => x.TipoDiagnostico == enumTipoDiagnostico.Dominio && x.RespostaSelecionada.HasValue).FirstOrDefault();
                    if (respostaDominio != null)
                        respostaDominioTexto = respostaDominio.Nome;

                    var respostaImportancia = pergunta.ListaOpcoesParticipacao.Where(x => x.TipoDiagnostico == enumTipoDiagnostico.Importancia && x.RespostaSelecionada.HasValue).FirstOrDefault();
                    if (respostaImportancia != null)
                        respostaImportanciaTexto = respostaImportancia.Nome;

                    sbQuestao.AppendLine(string.Format(@"
                        <tr>
							<td>{0}</td>
							<td>{1}</td>
							<td>
							  	{2}
							</td>

							<td>
							  	{3}
							</td>
						</tr>", pergunta.Questao, pergunta.Feedback, respostaDominioTexto, respostaImportanciaTexto));
                }
                sbQuestao.AppendLine("</table>");

            }
            else
            {
                sbQuestao.AppendLine(@"
                            <table class='table table-condensed table-bordered'>
                                 <thead>
				                    <tr>
					                    <td>Perguntas</td>
					                    <td>Respostas</td>
				                    </tr>
                                  </thead>");
                foreach (ItemQuestionarioParticipacao pergunta in ListaDePerguntas)
                {

                    sbQuestao.AppendLine(string.Format(@"
                        <tr>
							<td class='expander'>{0}</td>
							<td>", pergunta.Questao));

                    foreach (ItemQuestionarioParticipacaoOpcoes item in pergunta.ListaOpcoesParticipacao)
                    {
                        string selecionada = string.Empty;
                        string correta = string.Empty;
                        if (item.RespostaSelecionada.HasValue)
                            selecionada = "checked";
                        if (item.RespostaCorreta.HasValue && item.RespostaCorreta.Value)
                            correta = " - <span style='color:red'>Gabarito</span>";
                        sbQuestao.AppendLine(string.Format(@"
									<input type='radio'disabled {0}>{1}{2}<br />
							", selecionada, item.Nome, correta));
                    }
                    sbQuestao.AppendLine("</td></tr>");
                }
                sbQuestao.AppendLine("</table>");
            }
            this.dvDetalhesDaProva.InnerHtml = sbQuestao.ToString();
        }

        /// <summary>
        /// Persiste o Id da Turma.
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

        protected void btnFechar_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ExibiuResposta != null)
                {
                    ExibiuResposta(this, new DetalheDaRespostaDaProvaEventArgs(this.IdMatriculaTurma));
                }
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                return;
            }
        }



    }


}