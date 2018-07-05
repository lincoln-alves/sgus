using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AutoMapper;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.UserControls
{
    public partial class ucQuestionario : UserControl
    {
        private DateTime DataFinal
        {
            get
            {
                return !string.IsNullOrEmpty(ViewState["DataFinal"].ToString()) ? DateTime.Parse(ViewState["DataFinal"].ToString()) : DateTime.Now;
            }
            set
            {
                ViewState["DataFinal"] = value;
            }
        }

        public void IniciarRespostaQuestionario(Questionario questionario)
        {
            hdnIdQuestionario.Value = questionario.ID.ToString();

            rptQuestionario.DataSource = questionario.ListaItemQuestionario;
            rptQuestionario.DataBind();
        }

        /// <summary>
        /// Responder questionário e alterar o Status de uma matrícula em seguida.
        /// </summary>
        /// <param name="questionario">Questionário a ser respondido.</param>
        /// <param name="matriculasTurmasIds">Matrículas Turmas a serem alteradas, em padrão CSV.</param>
        /// <param name="statusPosCadastro">Status para alteração da matrícula</param>
        /// <param name="dataConclusao">Data da conclusão das matrículas turmas</param>
        /// <param name="notaFinal">Nota média final das matrículas turmas</param>
        public void IniciarRespostaQuestionario(Questionario questionario, List<int> matriculasTurmasIds, enumStatusMatricula statusPosCadastro, DateTime? dataConclusao = null, double? notaFinal = null)
        {
            hdnIdMatriculaTurma.Value = string.Join(",", matriculasTurmasIds.Select(x => x.ToString()));

            hdnIdTurma.Value = matriculasTurmasIds.FirstOrDefault().ToString();

            hdnIdStatusMatricula.Value = ((int)statusPosCadastro).ToString();

            if (dataConclusao.HasValue)
            {
                DataFinal = dataConclusao.Value;

                pnlDataConclusao.Visible = true;
                txtDataConclusao.Text = dataConclusao.Value.ToString();
            }

            if (notaFinal.HasValue)
                hdnNotaFinal.Value = notaFinal.ToString();

            IniciarRespostaQuestionario(questionario);
        }

        public void LimparQuestionario()
        {
            hdnIdQuestionario.Value = "";
            hdnIdQuestionarioParticipacao.Value = "";
            hdnIdMatriculaTurma.Value = "";
            hdnIdStatusMatricula.Value = "";
        }

        protected void rptQuestionario_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var itemQuestionario = (ItemQuestionario)e.Item.DataItem;

            var hdnItemQuestionario = (HiddenField)e.Item.FindControl("hdnIdItemQuestionario");
            hdnItemQuestionario.Value = itemQuestionario.ID.ToString();

            //var txtResposta = (TextBox)e.Item.FindControl("txtResposta");
        }

        public void SalvarQuestionario()
        {
            try
            {
                QuestionarioParticipacao questionarioParticipacao;

                if (!string.IsNullOrWhiteSpace(hdnIdQuestionarioParticipacao.Value))
                {
                    questionarioParticipacao = new ManterQuestionarioParticipacao().ObterQuestionarioParticipacaoPorId(int.Parse(hdnIdQuestionarioParticipacao.Value));
                }
                else
                {

                    var questionario = new ManterQuestionario().ObterQuestionarioPorID(int.Parse(hdnIdQuestionario.Value));

                    var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

                    questionarioParticipacao = new QuestionarioParticipacao
                    {
                        Questionario = questionario,
                        DataGeracao = DateTime.Now,
                        Usuario = usuarioLogado,
                        DataParticipacao = DateTime.Now,
                        TipoQuestionarioAssociacao = new ManterTipoQuestionarioAssociacao().ObterTipoQuestionarioAssociacaoPorID(5),
                        TextoEnunciadoPre = questionario.TextoEnunciado,
                        Evolutivo = false,
                        NivelOcupacional = new NivelOcupacional { ID = usuarioLogado.NivelOcupacional.ID },
                        Uf = new Uf { ID = usuarioLogado.UF.ID },
                        Turma = new Turma { ID = int.Parse(hdnIdTurma.Value) }
                    };
                }

                var listaItemQuestionarioParticipacao = new List<ItemQuestionarioParticipacao>();

                // Obter respostas dissertivas do questionário.
                foreach (Control row in rptQuestionario.Controls)
                {
                    var hdnIdItemQuestionario = (HiddenField)row.FindControl("hdnIdItemQuestionario");

                    if (hdnIdItemQuestionario == null)
                        throw new AcademicoException("Questionário mal formatado ou alterado, atualize a página e tente novamente.");

                    int idItemQuestionario;
                    int.TryParse(hdnIdItemQuestionario.Value, out idItemQuestionario);

                    var itemQuestionario = new ManterItemQuestionario().ObterItemQuestionarioPorID(idItemQuestionario);

                    if (itemQuestionario == null)
                        throw new AcademicoException("Questionário mal formatado ou alterado, atualize a página e tente novamente.");

                    var txtResposta = (TextBox)row.FindControl("txtResposta");

                    if (txtResposta == null)
                        throw new AcademicoException("Questionário mal formatado ou alterado, atualize a página e tente novamente.");

                    var itemQuestionarioParticipacao = Mapper.Map<ItemQuestionarioParticipacao>(itemQuestionario);

                    itemQuestionarioParticipacao.QuestionarioParticipacao = questionarioParticipacao;

                    if (itemQuestionarioParticipacao.TipoItemQuestionario.ID == (int)enumTipoItemQuestionario.Discursiva)
                    {
                        // Validação do campo em branco.
                        if (string.IsNullOrWhiteSpace(txtResposta.Text))
                            throw new AcademicoException(string.Format("A questão \"{0}\" é obrigatória",
                                string.IsNullOrWhiteSpace(itemQuestionarioParticipacao.Ordem.ToString())
                                    ? itemQuestionarioParticipacao.Questao
                                    : itemQuestionarioParticipacao.Ordem.ToString()));

                        itemQuestionarioParticipacao.Resposta = txtResposta.Text;
                    }

                    listaItemQuestionarioParticipacao.Add(itemQuestionarioParticipacao);
                }

                questionarioParticipacao.ListaItemQuestionarioParticipacao = listaItemQuestionarioParticipacao;

                // Executar lógica para questionário de cancelamento.
                if (questionarioParticipacao.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Cancelamento)
                {
                    var matriculasTurmasIds = hdnIdMatriculaTurma.Value.Split(',');

                    foreach (var matriculaIdString in matriculasTurmasIds)
                    {
                        int matriculaId;

                        if (int.TryParse(matriculaIdString, out matriculaId))
                        {
                            var matriculaTurma = new MatriculaTurma();
                            var manterMatriculaTurma = new ManterMatriculaTurma();

                            if (matriculaId > 0)
                            {
                                matriculaTurma = manterMatriculaTurma.ObterMatriculaTurmaPorId(matriculaId);

                                // Caso seja Gestor, só altera a matrícula caso seja da sua UF.
                                if (matriculaTurma != null)
                                    questionarioParticipacao.Turma = matriculaTurma.Turma;
                            }

                            // Alterar Status da matrícula.
                            if (matriculaTurma != null && matriculaTurma.ID != 0)
                            {
                                var manterMatriculaOferta = new ManterMatriculaOferta();

                                var matriculaOferta = manterMatriculaOferta.ObterMatriculaOfertaPorID(matriculaTurma.MatriculaOferta.ID);

                                matriculaOferta.StatusMatricula = (enumStatusMatricula)int.Parse(hdnIdStatusMatricula.Value);

                                // Salvar Data de conclusão e nota final em lote
                                if ((!string.IsNullOrWhiteSpace(txtDataConclusao.Text) ||
                                     !string.IsNullOrWhiteSpace(hdnNotaFinal.Value)) &&
                                    matriculaOferta.Oferta.SolucaoEducacional.FormaAquisicao.Presencial)
                                {
                                    var dataConclusao = CommonHelper.TratarData(txtDataConclusao.Text,
                                        "Data Conclusão do lote");

                                    double? notaFinal = null;

                                    double n;

                                    if (double.TryParse(hdnNotaFinal.Value.Trim(), out n))
                                    {
                                        notaFinal = n;
                                    }

                                    foreach (var mt in matriculaOferta.MatriculaTurma)
                                    {
                                        if (dataConclusao.HasValue &&
                                            (!mt.Turma.DataFinal.HasValue || mt.Turma.DataFinal.Value >= dataConclusao))
                                        {
                                            mt.DataTermino = dataConclusao;
                                        }

                                        if (notaFinal.HasValue)
                                        {
                                            mt.MediaFinal = notaFinal;
                                        }
                                    }
                                }

                                manterMatriculaOferta.AtualizarMatriculaOferta(matriculaOferta, false);

                                matriculaTurma.DataTermino = !string.IsNullOrEmpty(txtDataConclusao.Text) ? DateTime.Parse(txtDataConclusao.Text) : DateTime.Now;

                                manterMatriculaTurma.AlterarMatriculaTurma(matriculaTurma);
                            }
                        }
                    }
                }

                // Salvar participação no questionário.
                new ManterQuestionarioParticipacao().Salvar(questionarioParticipacao);

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Questionário respondido com Sucesso.");
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        protected void txtDataConclusao_TextChanged(object sender, EventArgs e)
        {
            DateTime dataAlteracao;
            if (DateTime.TryParse(txtDataConclusao.Text.ToString(), out dataAlteracao)) {
                if (dataAlteracao != DataFinal)
                {
                    pnlAlteracaoData.Visible = true;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "bloquearEnvio", "bloquearEnvio()", true);
                }
            }
        }

        protected void btnCancelarAlteracao_Click(object sender, EventArgs e)
        {
            pnlAlteracaoData.Visible = false;
            txtDataConclusao.Text = DataFinal.ToString();
            Page.ClientScript.RegisterStartupScript(this.GetType(), "habilitarEnvio", "habilitarEnvio()", true);
        }

        protected void btnConfirmarAlteracao_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "habilitarEnvio", "habilitarEnvio()", true);
        }
    }
}