using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.WebForms.UserControls;
using classes = Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.WebForms.Cadastros
{
    public partial class EdicaoQuestionario : PageBase
    {

        #region "Atributos Privados"

        private Questionario questionarioEdicao = null;
        private ManterQuestionario manterQuestionario = new ManterQuestionario();

        private int IdQuestionario
        {
            get
            {
                if (Session["ViewStateIdDoQuestionario"] != null)
                {
                    return (int)Session["ViewStateIdDoQuestionario"];
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                Session["ViewStateIdDoQuestionario"] = value;
            }

        }

        private void ExibirModal()
        {
            base.ExibirBackDrop();
            pnlModal.Visible = true;
        }

        protected void OcultarModal_Click(object sender, EventArgs e)
        {
            this.OcultarModal();
        }

        private void OcultarModal()
        {
            base.OcultarBackDrop();
            pnlModal.Visible = false;
        }

        /// <summary>
        /// ID Lógico do Questionário.
        /// </summary>
        private int IdLogicoDoQuestionario
        {
            get
            {
                if (Session["ViewStateIdLogicoDoQuestionario"] != null)
                {
                    return int.Parse(Session["ViewStateIdLogicoDoQuestionario"].ToString());
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                Session["ViewStateIdLogicoDoQuestionario"] = value;
            }

        }

        protected override IList<enumPerfil> PerfisNecessariosParaAcessarAFuncionalidade
        {
            get { throw new NotImplementedException(); }
        }


        /// <summary>
        /// ID do item do Questionário (Id da Pergunta)
        /// </summary>
        private int IdItemQuestionario
        {
            get
            {
                if (Session["ViewStateIdItemDoQuestionario"] != null)
                {
                    return (int)Session["ViewStateIdItemDoQuestionario"];
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                Session["ViewStateIdItemDoQuestionario"] = value;
            }

        }

        /// <summary>
        /// ID Lógico do item do Questionário (Id Lógico da Pergunta)
        /// </summary>
        private int IdLogicoItemQuestionario
        {
            get
            {
                if (Session["ViewStateIdLogicoItemDoQuestionario"] != null)
                {
                    return int.Parse(Session["ViewStateIdLogicoItemDoQuestionario"].ToString());
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                Session["ViewStateIdLogicoItemDoQuestionario"] = value;
            }

        }

        public int AcaoDaTelaDeQuestionario
        {
            get
            {
                if (Session["ViewStateAcaoDaTelaDeQuestionario"] != null)
                {
                    return (int)Session["ViewStateAcaoDaTelaDeQuestionario"];
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                Session["ViewStateAcaoDaTelaDeQuestionario"] = value;
            }

        }


        #endregion

        #region "Eventos Customizados"

        protected void ItemQuestionario_CancelouAdicaoDeItemNoQuestionario(object sender, AdicionarQuestionarioEventArgs e)
        {
            Questionario questionario = e.QuestionarioSelecionado;

            //Já que houve um cancelamento, remove o item do questionário, da lista de questionários da sessão
            //this.QuestionarioDaSessao.ListaItemQuestionario.Remove();

            this.PreencherItensDoQuestionario(questionario);
            this.SetarAcaoDaTela(enumAcaoTelaQuestionario.CancelouCadastroDeItem);
            OcultarModal();
        }

        protected void ItemQuestionario_AdicionouItemAoQuestionario(object sender, Sebrae.Academico.WebForms.UserControls.ucQuestoes.AdicionarItemNoQuestionarioEventArgs e)
        {
            IList<ItemQuestionario> listaItemQuestionario = e.ListaItemQuestionario;
            this.PreencherItemsDoQuestionario(listaItemQuestionario);
            this.SetarAcaoDaTela(enumAcaoTelaQuestionario.AdicionouUmItem);
            OcultarModal();
        }

        #endregion

        private void PreencherItemsDoQuestionario(IList<ItemQuestionario> ListaItemQuestionario)
        {
            if (ListaItemQuestionario != null && ListaItemQuestionario.Count > 0)
            {
                WebFormHelper.PreencherGrid(ListaItemQuestionario, dgvItensDoQuestionario);
            }

        }

        protected override enumFuncionalidade Funcionalidade
        {
            get
            {
                return enumFuncionalidade.Questionario;
            }
        }

        #region "Enumeração"

        /// <summary>
        /// Enumeração referente às ações feitas na tela de cadastro de trilha.
        /// </summary>
        private enum enumAcaoTelaQuestionario
        {
            Nenhum = 0,
            NovoRegistroDeItem = 1,
            EdicaoDeUmItem = 2,
            AdicionouUmItem = 3,
            ChamouTelaDeCadastroDeItem = 4,
            CancelouCadastroDeItem = 5
        }

        #endregion

        #region "Métodos Privados"

        private void SetarAcaoDaTela(enumAcaoTelaQuestionario acaoTelaQuestionario)
        {
            this.AcaoDaTelaDeQuestionario = (int)acaoTelaQuestionario;
        }

        #endregion

        private void PreencherItensDoQuestionario(Questionario questionario)
        {
            this.PreencherGridDeItensDoQuestionario(questionario);
        }

        private void PreencherGridDeItensDoQuestionario(Questionario questionario)
        {
            if (questionario != null && questionario.ListaItemQuestionario != null && questionario.ListaItemQuestionario.Count > 0)
            {
                WebFormHelper.PreencherGrid(questionario.ListaItemQuestionario, this.dgvItensDoQuestionario);
            }
        }

        #region "Eventos do Grid"

        protected void dgvItensDoQuestionario_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            string[] arrayComIdItemQuestionarioEIdLogico = e.CommandArgument.ToString().Split(new char[] { '|' });
            this.IdItemQuestionario = int.Parse(arrayComIdItemQuestionarioEIdLogico[0]);
            this.IdLogicoItemQuestionario = int.Parse(arrayComIdItemQuestionarioEIdLogico[1].ToString());

            if (e.CommandName.Equals("editar"))
            {
                try
                {
                    ExibirModal();
                    this.TratarEdicaoDeUmItemDoQuestionario();
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }

            }
            else if (e.CommandName.Equals("excluir"))
            {
                try
                {
                    this.TratarExclusaoDeUmItemDoQuestionario();
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }
            }


        }

        #endregion

        private void TratarExclusaoDeUmItemDoQuestionario()
        {
            ItemQuestionario itemQuestionario = new ManterItemQuestionario().ObterItemQuestionarioPorID(this.IdItemQuestionario);

            if (itemQuestionario != null)
            {
                if (this.QuestionarioDaSessao.ListaItemQuestionario.Where(x => x.ID == this.IdItemQuestionario).Any())
                {
                    ItemQuestionario itemQuestionarioComIdDoBanco = this.QuestionarioDaSessao.ListaItemQuestionario.Where(x => x.ID == this.IdItemQuestionario).ToList()[0];
                    this.QuestionarioDaSessao.ListaItemQuestionario.Remove(itemQuestionarioComIdDoBanco);
                    this.AtualizarTela();
                }
            }
            else
            {
                if (this.QuestionarioDaSessao.ListaItemQuestionario.Where(x => x.IdLogico == this.IdLogicoItemQuestionario).Any())
                {
                    ItemQuestionario itemQuestionarioComIdLogico = this.QuestionarioDaSessao.ListaItemQuestionario.Where(x => x.IdLogico == this.IdLogicoItemQuestionario).ToList()[0];
                    this.QuestionarioDaSessao.ListaItemQuestionario.Remove(itemQuestionarioComIdLogico);
                    this.AtualizarTela();
                }
            }

        }


        private void TratarEdicaoDeUmItemDoQuestionario()
        {
            ItemQuestionario itemQuestionario = new ManterItemQuestionario().ObterItemQuestionarioPorID(this.IdItemQuestionario);

            /* Se encontrou o item do questionário pelo seu id, significa que estamos recuperando um 
               item de questionário (ou seja, a pergunta de um questionário) do banco de dados */
            if (itemQuestionario != null)
            {
                //this.CarregarDadosDoCadastroDeItensDoQuestionario(itemQuestionario);

                //novo

                ItemQuestionario itemQuestionarioEditada = this.QuestionarioDaSessao.ListaItemQuestionario.FirstOrDefault(x => x.ID == this.IdItemQuestionario);
                if (itemQuestionarioEditada != null)
                {
                    if (itemQuestionarioEditada.StatusRegistro.Equals(enumStatusRegistro.Alterado))
                    {
                        itemQuestionario = (ItemQuestionario)itemQuestionarioEditada.Clone();
                    }
                }

                this.CarregarDadosDoCadastroDeItensDoQuestionario(itemQuestionario);

                //novo
            }
            else
            {
                /* Se não encontrou o item do questionário, significa que o item não está gravado no banco, ou seja,
                   estamos recuperando o item do questionário que está em memória (na variavel de sessao this.QuestionarioDaSessao) */
                if (this.QuestionarioDaSessao.ListaItemQuestionario.Where(x => x.IdLogico == this.IdLogicoItemQuestionario).Any())
                {
                    //Obtém o item do questionário através do IdLogico do item do questionário
                    ItemQuestionario itemQuestionarioComIdLogico = this.QuestionarioDaSessao.ListaItemQuestionario.Where(x => x.IdLogico == this.IdLogicoItemQuestionario).ToList()[0];
                    this.CarregarDadosDoCadastroDeItensDoQuestionario(itemQuestionarioComIdLogico);
                }
            }

        }

        private void AtualizarTela()
        {
            //Atualiza a grid de Itens do Questionário
            WebFormHelper.PreencherGrid(this.QuestionarioDaSessao.ListaItemQuestionario, dgvItensDoQuestionario);
        }

        /// <summary>
        /// Propriedade utilizada para manipular as informações 
        /// de um questionário que está na memória do servidor.
        /// </summary>
        public Questionario QuestionarioDaSessao
        {
            get
            {
                Questionario questionario = null;

                if (Session["Questionario"] != null)
                {
                    questionario = (Questionario)Session["Questionario"];
                }
                else
                {
                    questionario = new Questionario();
                    Session["Questionario"] = questionario;
                }

                return questionario;
            }
            set
            {
                Session["Questionario"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.lkbDadosQuestionario.Attributes.Add("class", "collapsed");
                this.lkbQuestoes.Attributes.Add("class", "collapsed");

                this.LimparQuestionarioDaSessao();
                this.PreencherCombos();

                //Update, ou seja, estamos alterado os dados de um questionário
                if (Session["QuestionarioEdit"] != null)
                {
                    this.IdQuestionario = int.Parse(Session["QuestionarioEdit"].ToString());
                    questionarioEdicao = manterQuestionario.ObterQuestionarioPorID(this.IdQuestionario);
                    this.AtualizarQuestionarioDaSessao(questionarioEdicao);
                    this.PreencherCampos(questionarioEdicao);
                    pnlOpcoesQuestionario.Visible = true;
                    base.ExibirTab(this.lkbDadosQuestionario, collapseDados);
                }



            }

            try
            {
                //Faz o Log de Acesso
                base.LogarAcessoFuncionalidade();
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }

        }

        private void LimparQuestionarioDaSessao()
        {
            this.QuestionarioDaSessao = null;
        }

        private void LimparInformacoesDaMemoria()
        {
            this.IdItemQuestionario = 0;
            this.IdLogicoItemQuestionario = 0;
            this.ucQuestoes1.IdItemQuestionario = 0;
            this.ucQuestoes1.IdLogicoDoItemQuestionario = 0;
            this.ucQuestoes1.IdResposta = 0;
            this.QuestionarioDaSessao = null;
            this.IdQuestionario = 0;
            this.IdLogicoDoQuestionario = 0;
        }

        /// <summary>
        /// Atualiza as informações do questionário que está na sessão com o questionário informado.
        /// </summary>
        /// <param name="questionario"></param>
        private void AtualizarQuestionarioDaSessao(Questionario questionario)
        {
            this.QuestionarioDaSessao = questionario;
        }

        private void PreencherCampos(Questionario questionarioEdicao)
        {
            if (questionarioEdicao != null)
            {
                this.txtNome.Text = questionarioEdicao.Nome;

                //Tipo de Questionário
                WebFormHelper.SetarValorNaCombo(questionarioEdicao.TipoQuestionario.ID.ToString(), ddlTipoQuestionario);

                //Prazo em Minutos
                this.txtPrazoemMinutos.Text = questionarioEdicao.PrazoMinutos.ToString();

                //Quantidade de Questões
                if (questionarioEdicao.QtdQuestoesProva.HasValue)
                {
                    this.txtQtdQuestoesDaProva.Text = questionarioEdicao.QtdQuestoesProva.Value.ToString();
                }

                //Texto do Enunciado Pré
                if (!string.IsNullOrWhiteSpace(questionarioEdicao.TextoEnunciado))
                {
                    this.txtTextoEnunciadoPre.Text = questionarioEdicao.TextoEnunciado;
                }

                this.ddlTipoQuestionario.Enabled = false;

                this.PreencherInformacoesDeItensDoQuestionario(questionarioEdicao);

                if (questionarioEdicao.TipoQuestionario.ID.Equals((int)enumTipoQuestionario.Pesquisa))
                {
                    this.divPrazoemMinutos.Visible = false;
                    this.divQtdQuestoesDaProva.Visible = false;
                }
                else if (questionarioEdicao.TipoQuestionario.ID.Equals((int)enumTipoQuestionario.AvaliacaoProva))
                {
                    this.divPrazoemMinutos.Visible = true;
                    this.divQtdQuestoesDaProva.Visible = true;
                }

            }
        }

        private void PreencherInformacoesDeItensDoQuestionario(Questionario questionario)
        {
            //Se o questionário possui iten(s) cadastrado(s), exibe estes itens na tela.
            if (questionario != null && questionario.ListaItemQuestionario != null
                && questionario.ListaItemQuestionario.Count > 0)
            {
                WebFormHelper.PreencherGrid(questionario.ListaItemQuestionario, dgvItensDoQuestionario);
            }
            else
            {
                IList<ItemQuestionario> ListaItemQuestionarioVazia = new List<ItemQuestionario>();
                WebFormHelper.PreencherGrid(ListaItemQuestionarioVazia, dgvItensDoQuestionario);
            }
        }

        private void PreencherCombos()
        {
            try
            {
                PreencherComboTipoQuestionario();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void PreencherComboTipoQuestionario()
        {
            ManterTipoQuestionario manterTipoQuestionario = new ManterTipoQuestionario();
            IList<classes.TipoQuestionario> ListaTipoQuestionario = manterTipoQuestionario.ObterTodosTipoQuestionario();
            WebFormHelper.PreencherLista(ListaTipoQuestionario, this.ddlTipoQuestionario, false, true);
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {

                if (Session["QuestionarioEdit"] == null)
                {
                    manterQuestionario = new ManterQuestionario();
                    questionarioEdicao = ObterObjetoQuestionario();
                    manterQuestionario.IncluirQuestionario(questionarioEdicao);
                    this.LimparQuestionarioDaSessao();
                }
                else
                {
                    //this.SetarAcaoDaTela(enumAcaoTelaQuestionario.EdicaoDeUmItem);
                    questionarioEdicao = ObterObjetoQuestionario();
                    manterQuestionario.AlterarQuestionario(questionarioEdicao);
                    this.LimparQuestionarioDaSessao();
                }

                Session.Remove("QuestionarioEdit");

            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                return;
            }

            WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados Gravados com Sucesso !", "ListarQuestionario.aspx");

        }

        private Questionario ObterObjetoQuestionario()
        {
            //Insert
            if (this.QuestionarioDaSessao.ID == 0)
            {
                manterQuestionario = new ManterQuestionario();
                questionarioEdicao = this.QuestionarioDaSessao;
            }
            //Update
            else
            {
                questionarioEdicao = new ManterQuestionario().ObterQuestionarioPorID(this.QuestionarioDaSessao.ID);
                //Implementação do padrão Prototype
                questionarioEdicao = (Questionario)this.QuestionarioDaSessao.Clone();
            }

            if (questionarioEdicao == null)
            {
                throw new AcademicoException("Preencha as informações do Questionário");
            }
            else
            {
                //Se o Questionário não possuir itens cadastrados, exibe mensagem de erro.
                if (questionarioEdicao.ListaItemQuestionario == null || (questionarioEdicao.ListaItemQuestionario != null && questionarioEdicao.ListaItemQuestionario.Count == 0))
                {
                    throw new AcademicoException("É necessário Cadastrar um item para este Questionário. Não há itens Cadastrados para este Questionário.");
                }
            }

            //Tipo de Questionário
            if (ddlTipoQuestionario != null && ddlTipoQuestionario.SelectedItem != null &&
                !string.IsNullOrWhiteSpace(ddlTipoQuestionario.SelectedItem.Value))
            {
                questionarioEdicao.TipoQuestionario = new TipoQuestionario()
                {
                    ID = int.Parse(ddlTipoQuestionario.SelectedItem.Value),
                    Nome = ddlTipoQuestionario.SelectedItem.Text
                };
            }
            else
            {
                questionarioEdicao.TipoQuestionario = null;
            }

            //Nome
            if (!string.IsNullOrWhiteSpace(txtNome.Text))
            {
                questionarioEdicao.Nome = txtNome.Text.Trim();
            }
            else
            {
                questionarioEdicao.Nome = null;
            }

            //Prazo em Minutos
            if (!string.IsNullOrWhiteSpace(this.txtPrazoemMinutos.Text))
            {
                int prazoEmMinutos = 0;
                if (!int.TryParse(this.txtPrazoemMinutos.Text.Trim(), out prazoEmMinutos))
                    throw new AcademicoException("Valor Inválido para o Campo Prazo em Minutos.");
                else
                    questionarioEdicao.PrazoMinutos = prazoEmMinutos;
            }

            if (!string.IsNullOrWhiteSpace(this.txtQtdQuestoesDaProva.Text))
            {
                int qtdQuestoesDaProva = 0;
                if (!int.TryParse(this.txtQtdQuestoesDaProva.Text.Trim(), out qtdQuestoesDaProva))
                    throw new AcademicoException("Valor Inválido para o Campo Qtd de Questões da Prova.");
                else
                    questionarioEdicao.QtdQuestoesProva = qtdQuestoesDaProva;
            }

            //Texto do Enunciado Pré
            questionarioEdicao.TextoEnunciado = this.txtTextoEnunciadoPre.Text.Trim();

            return questionarioEdicao;
        }

        private string GerarIdLogicoDaResposta()
        {
            return WebFormHelper.ObterStringAleatoria();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.LimparInformacoesDaMemoria();
            Session.Remove("QuestionarioEdit");
            Response.Redirect("ListarQuestionario.aspx");
        }

        protected void ddlTipoQuestionario_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTipoQuestionario.SelectedItem != null && !string.IsNullOrWhiteSpace(ddlTipoQuestionario.SelectedItem.Value))
            {
                if (int.Parse(ddlTipoQuestionario.SelectedItem.Value).Equals((int)enumTipoQuestionario.Pesquisa))
                {
                    //Esconde o checkbox de Resposta correta do usercontrol de Questionário
                    this.ucQuestoes1.EsconderDivRespostaCorreta();
                    this.ucQuestoes1.EsconderDivGabarito();
                    this.ucQuestoes1.EsconderDivValorQuestao();
                    this.ucQuestoes1.IdTipoQuestionarioSelecionado = (int)enumTipoQuestionario.Pesquisa;
                    this.divPrazoemMinutos.Visible = false;
                    this.divQtdQuestoesDaProva.Visible = false;
                }
                else if (int.Parse(ddlTipoQuestionario.SelectedItem.Value).Equals((int)enumTipoQuestionario.AvaliacaoProva))
                {
                    this.divPrazoemMinutos.Visible = true;
                    this.ucQuestoes1.ExibirDivValorQuestao();
                    this.ucQuestoes1.ExibirDivGabarito();
                    this.ucQuestoes1.IdTipoQuestionarioSelecionado = (int)enumTipoQuestionario.AvaliacaoProva;
                    this.divPrazoemMinutos.Visible = true;
                    this.divQtdQuestoesDaProva.Visible = true;
                }
            }
            pnlOpcoesQuestionario.Visible = true;
            ddlTipoQuestionario.Enabled = false;
            base.ExibirTab(this.lkbDadosQuestionario, collapseDados);
            dgvItensDoQuestionario.DataBind();
        }

        private void CarregarDadosDoCadastroDeItensDoQuestionario(ItemQuestionario itemQuestionario)
        {

            enumTipoQuestionario? tipoQuestionario = null;

            if (this.ddlTipoQuestionario.SelectedItem != null && !string.IsNullOrWhiteSpace(this.ddlTipoQuestionario.SelectedItem.Value))
            {
                tipoQuestionario = (enumTipoQuestionario)int.Parse(this.ddlTipoQuestionario.SelectedItem.Value.ToString());
            }

            this.ucQuestoes1.IdTipoQuestionarioSelecionado = (int)tipoQuestionario;


            this.ucQuestoes1.PreencherCamposDoCadastroDeItensDoQuestionario(itemQuestionario);
        }

        private void VerificarCamposObrigatoriosDoQuestionario()
        {
            if (this.ddlTipoQuestionario.SelectedItem == null ||
                (this.ddlTipoQuestionario.SelectedItem != null && string.IsNullOrWhiteSpace(this.ddlTipoQuestionario.SelectedItem.Value)))
            {
                throw new AcademicoException("Tipo de Questionário. Campo Obrigatório");
            }
        }

        private void DesabilitarTipoDeQuestionario()
        {
            this.ddlTipoQuestionario.Enabled = false;
        }

        private void HabilitarTipoDeQuestionario()
        {
            this.ddlTipoQuestionario.Enabled = true;
        }

        protected void btnAdicionarItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.ucQuestoes1.LimparCampos();
                this.VerificarCamposObrigatoriosDoQuestionario();
                this.DesabilitarTipoDeQuestionario();

                this.CarregarDadosDoCadastroDeItensDoQuestionario(null);
                this.DesabilitarTipoDeQuestionario();
                ExibirModal();
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }

        }
        protected void lkbDadosQuestionario_Click(object sender, EventArgs e)
        {
            base.AlterarStatusTab(this.lkbDadosQuestionario, collapseDados);
        }
        protected void lkbQuestoes_Click(object sender, EventArgs e)
        {
            base.AlterarStatusTab(this.lkbQuestoes, collapseQuestoes);
        }

    }
}