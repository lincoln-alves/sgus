using System;
using System.Web.UI;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;
using System.Collections.Generic;

namespace Sebrae.Academico.WebForms.UserControls
{
    /// <summary>
    /// User control utilizado no cadastro de uma turma.
    /// </summary>
    public partial class ucInformarPagamento : System.Web.UI.UserControl
    {

        #region "Eventos Relacionados ao Histórico de Pagamento"


        public delegate void PagamentoInformado(object sender, InformarPagamentoEventArgs e);
        public event PagamentoInformado InformouPagamento;


        public class InformarPagamentoEventArgs : EventArgs
        {
            public int IdUsuario { get; set; }

            //private UsuarioPagamento usuarioPagamento { get; set; }

            public InformarPagamentoEventArgs(int pIdUsuario)
            {
                IdUsuario = pIdUsuario;
            }

            //public InformarPagamentoEventArgs(UsuarioPagamento pUsuarioPagamento)
            //{
            //    usuarioPagamento = pUsuarioPagamento;
            //}
        }

        #endregion

        private UsuarioPagamento InformarPagamento = null;
        private ManterUsuarioPagamento ManterUsuarioPagamento = new ManterUsuarioPagamento();

        #region "Atributos Públicos"

        /// <summary>
        /// Data de Início Vigência.
        /// </summary>
        public string DtInicioVigencia
        {
            get
            {
                string dtInicioVigencia = null;

                if (!string.IsNullOrWhiteSpace(this.txtDataInicioVigencia.Text))
                {
                    dtInicioVigencia = this.txtDataInicioVigencia.Text;
                }

                return dtInicioVigencia;
            }
        }

        /// <summary>
        /// Data Fim Vigência
        /// </summary>
        public string DtFimVigencia
        {
            get
            {
                string dtFimVigencia = null;

                if (!string.IsNullOrWhiteSpace(this.txtFimVigencia.Text))
                {
                    dtFimVigencia = this.txtFimVigencia.Text;
                }

                return dtFimVigencia;
            }
        }

        /// <summary>
        /// Data de Início Vigência.
        /// </summary>
        public string DtInicioRenovacao
        {
            get
            {
                string dtInicioRenovacao = null;

                if (!string.IsNullOrWhiteSpace(this.TxtDtInicioRenovacao.Text))
                {
                    dtInicioRenovacao = this.TxtDtInicioRenovacao.Text;
                }

                return dtInicioRenovacao;
            }
        }

        /// <summary>
        /// Data Fim Vigência
        /// </summary>
        public string DtMaxInadimplencia
        {
            get
            {
                string dtMaxInadimplencia = null;

                if (!string.IsNullOrWhiteSpace(this.txtDataMaxInadimplencia.Text))
                {
                    dtMaxInadimplencia = this.txtDataMaxInadimplencia.Text;
                }

                return dtMaxInadimplencia;
            }
        }

        ///// <summary>
        /// Codigo Informação é o RefTrans do BB
        /// </summary>
        public string codInformacao
        {
            get
            {
                string codInformacao = null;

                if (!string.IsNullOrWhiteSpace(this.TxtCodInformacao.Text))
                {
                    codInformacao = this.TxtCodInformacao.Text;
                }

                return codInformacao;
            }
        }

        /// <summary>
        /// Campo Valor
        /// </summary>
        public string valor
        {
            get
            {
                string valor = null;

                if (!string.IsNullOrWhiteSpace(this.txtValor.Text))
                {
                    valor = this.txtValor.Text;
                }

                return valor;
            }
        }

        /// <summary>
        /// Forma Pagamento
        /// </summary>
        public int codFormaPagamento
        {
            get
            {
                int codFormaPagamento = 0;

                codFormaPagamento = this.DdlFormaPagamento.SelectedIndex;

                return codFormaPagamento;
            }
        }

        #endregion

        public UsuarioPagamento ObterObjetoUsuarioPagamento()
        {
            UsuarioPagamento usuarioPagamento = null;

            //Cadastro de um novo pagamento
            if (AcaoDaTela.Equals((int)enumAcaoTelaCadastroDeInformarPagamento.NovoPagamento))
            {
                usuarioPagamento = new Dominio.Classes.UsuarioPagamento();
                usuarioPagamento.Usuario = new ManterUsuario().ObterUsuarioPorID(this.IdUsuario);
            }
            else if (AcaoDaTela.Equals((int)enumAcaoTelaCadastroDeInformarPagamento.EdicaoDePagamento))
            {
                usuarioPagamento = new ManterUsuarioPagamento().ObterInformacoesDePagamentoPorID(this.IdUsuarioPagamento);
            }

            if (!string.IsNullOrWhiteSpace(this.DdlInPago.SelectedItem.Value))
            {
                string foiPago = this.DdlInPago.SelectedItem.Value;

                if (foiPago.ToUpper().Equals("S"))
                {
                    usuarioPagamento.PagamentoEfetuado = true;
                }
                else if (foiPago.ToUpper().Equals("N"))
                {
                    usuarioPagamento.PagamentoEfetuado = false;
                }
            }

            if (!string.IsNullOrWhiteSpace(this.DdlFormaPagamento.SelectedItem.Value))
            {
                int formaPagamento = int.Parse(this.DdlFormaPagamento.SelectedItem.Value.ToString());
                usuarioPagamento.FormaPagamento = (enumFormaPagamento)formaPagamento;
            }
            
            //Configuracao de Pagamento
            usuarioPagamento.ConfiguracaoPagamento = new ManterConfiguracaoPagamento().ObterConfiguracaoPagamentoPorId(int.Parse(ddlConfigPagto.SelectedItem.Value));

            //Data Inicio Vigencia
            if (!string.IsNullOrWhiteSpace(this.txtDataInicioVigencia.Text))
            {
                DateTime dataInicioVigenciaAux = DateTime.Now;

                if (!DateTime.TryParse(this.txtDataInicioVigencia.Text, out dataInicioVigenciaAux))
                {
                    throw new AcademicoException("Valor inválido para o Campo data Início Vigência");
                }
                else
                {
                    usuarioPagamento.DataInicioVigencia = dataInicioVigenciaAux;
                }
            }

            //Data Fim Vigencia
            if (!string.IsNullOrWhiteSpace(this.txtFimVigencia.Text))
            {
                DateTime dataFimVigenciaAux = DateTime.Now;

                if (!DateTime.TryParse(this.txtFimVigencia.Text, out dataFimVigenciaAux))
                {
                    throw new AcademicoException("Valor inválido para o Campo data Fim Vigência");
                }
                else
                {
                    usuarioPagamento.DataFimVigencia = dataFimVigenciaAux;
                }
            }

            //Valor
            if (!string.IsNullOrWhiteSpace(this.txtValor.Text))
            {
                Decimal valorPagamento = 0;
                if (!Decimal.TryParse(this.txtValor.Text, out valorPagamento))
                {
                    throw new AcademicoException("Valor inválido para o Campo Valor Pagamento");
                }
                else
                {
                    usuarioPagamento.ValorPagamento = valorPagamento;
                }
            }

            //Data Inicio Renovação
            if (!string.IsNullOrWhiteSpace(this.TxtDtInicioRenovacao.Text))
            {
                DateTime dataInicioRenovacaoAux = DateTime.Now;

                if (!DateTime.TryParse(this.TxtDtInicioRenovacao.Text.Trim(), out dataInicioRenovacaoAux))
                {
                    throw new AcademicoException("Valor inválido para o Campo Data de Início de Renovação");
                }
                else
                {
                    usuarioPagamento.DataInicioRenovacao = dataInicioRenovacaoAux;
                }
            }

            //Data Max Inadimplencia
            if (!string.IsNullOrWhiteSpace(this.txtDataMaxInadimplencia.Text))
            {

                DateTime dataMaxInadimplenciaAux = DateTime.Now;

                if (!DateTime.TryParse(this.txtDataMaxInadimplencia.Text, out dataMaxInadimplenciaAux))
                {
                    throw new AcademicoException("Valor inválido para o Campo Data Máxima de Inadimplência");
                }
                else
                {
                    usuarioPagamento.DataMaxInadimplencia = dataMaxInadimplenciaAux;
                }
            }

            if (!string.IsNullOrWhiteSpace(this.TxtDtVencimento.Text))
            {

                DateTime dataVencimentoAux = DateTime.Now;

                if (!DateTime.TryParse(this.TxtDtVencimento.Text, out dataVencimentoAux))
                {
                    throw new AcademicoException("Valor inválido para o Campo Data de Vencimento");
                }
                else
                {
                    usuarioPagamento.DataVencimento = dataVencimentoAux;
                }
            }

            //Codigo Informacao
            if (!string.IsNullOrWhiteSpace(this.TxtCodInformacao.Text))
            {
                usuarioPagamento.NossoNumero = this.TxtCodInformacao.Text.Trim();
            }

            //Data Inicio Vigencia
            if (!string.IsNullOrWhiteSpace(this.DtInicioVigencia))
            {
                DateTime dtInicioVigenciaAux = DateTime.Now;

                if (!DateTime.TryParse(this.DtInicioVigencia, out dtInicioVigenciaAux))
                {
                    throw new AcademicoException("Valor inválido para o Campo Data de Início de Vigência");
                }
                else
                {
                    usuarioPagamento.DataInicioVigencia = dtInicioVigenciaAux;
                }
            }

            //Data Fim Vigencia
            if (!string.IsNullOrWhiteSpace(this.DtFimVigencia))
            {
                DateTime dtFimVigenciaAux = DateTime.Now;

                if (!DateTime.TryParse(this.DtFimVigencia, out dtFimVigenciaAux))
                {
                    throw new AcademicoException("Valor inválido para o Campo Data Fim de Vigência");
                }
                else
                {
                    usuarioPagamento.DataFimVigencia = dtFimVigenciaAux;
                }
            }

            //Data Inicio Renovação
            if (!string.IsNullOrWhiteSpace(this.DtInicioRenovacao))
            {
                DateTime dtInicioRenovacaoAux = DateTime.Now;

                if (!DateTime.TryParse(this.TxtDtInicioRenovacao.Text, out dtInicioRenovacaoAux))
                {
                    throw new AcademicoException("Valor inválido para o Campo Data de início Renovação");
                }
                else
                {
                    usuarioPagamento.DataInicioRenovacao = dtInicioRenovacaoAux;
                }
            }

            return usuarioPagamento;
        }

        public void PrepararTelaParaInclusaoDeInformarPagamento()
        {
            //this.ZerarIds();
            this.LimparCampos();
            this.SetarAcaoDaTela(enumAcaoTelaCadastroDeInformarPagamento.NovoPagamento);
        }
        public void PrepararTelaParaEdicaoDeInformarPagamento(UsuarioPagamento pUsuarioPagamento)
        {
            this.PreencherComboConfiguracoesPagamento();
            this.PreencheComboOpcoesSimNao();
            this.PreencherInformacoesInformarPagamento(pUsuarioPagamento);
            this.SetarAcaoDaTela(enumAcaoTelaCadastroDeInformarPagamento.EdicaoDePagamento);
        }


        #region "Métodos Privados"

        private void SetarAcaoDaTela(enumAcaoTelaCadastroDeInformarPagamento acaoTelaCadastroDeTurma)
        {
            this.AcaoDaTela = (int)acaoTelaCadastroDeTurma;
        }

        public int AcaoDaTela
        {
            get
            {
                if (ViewState["ViewStateAcaoDaTelaInformarPagamento"] != null)
                {
                    return (int)ViewState["ViewStateAcaoDaTelaInformarPagamento"];
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                ViewState["ViewStateAcaoDaTelaInformarPagamento"] = value;
            }

        }


        #endregion

        /// <summary>
        /// Limpa os campos da tela. 
        /// internal -> será acessível apenas pelas classes deste assembly.
        /// </summary>
        internal void LimparCampos()
        {
            this.txtDataInicioVigencia.Text = "";
            this.txtFimVigencia.Text = "";
            this.txtValor.Text = "";
            this.TxtDtInicioRenovacao.Text = "";
            this.txtDataMaxInadimplencia.Text = "";
            this.TxtCodInformacao.Text = "";
            this.DdlInPago.SelectedIndex = 0;
            this.DdlFormaPagamento.SelectedIndex = 0;
            this.TxtDtVencimento.Text = string.Empty;
            this.DdlFormaPagamento.ClearSelection();
            this.DdlInPago.ClearSelection();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.PreencherCombos();
            }

        }

        private void PreencherCombos()
        {
            this.PreencheComboOpcoesSimNao();
            this.PreencherComboFormaPagamento();
        }

        private void PreencherComboFormaPagamento()
        {
            WebFormHelper.PreencherComponenteComFormasDePagamento(this.DdlFormaPagamento, true);
        }

        private void PreencheComboOpcoesSimNao()
        {
            WebFormHelper.PreencherComponenteComOpcoesSimNao(this.DdlInPago, true);
        }

        public void PreencheCampos()
        {
            if (Session["UsuarioPagamentoEdit"] != null)
            {
                string cdInformacao = Session["UsuarioPagamentoEdit"].ToString();
                InformarPagamento = ManterUsuarioPagamento.ObterInformacoesDePagamentoPorIDUsuario(int.Parse(cdInformacao));

                PreencherInformacoesInformarPagamento(InformarPagamento);
            }
            else
            {
                //this.ucCascataSolucaoEducacional1.PreencherCombos();
            }
        }

        #region "Obtém valores dos campos Hidden"

       // public string IdConvenio 
       // {
       //     string idConvenio = "";
            
       //     get
       //     {
       //       idConvenio = this.idConv;
       //     }

       //     return idConvenio;
       //}

//        <input type="hidden" runat="server" id="idConv" clientidmode="Static" />
//<input type="hidden" runat="server" id="refTran" clientidmode="Static" />
//<input type="hidden" runat="server" id="valor" clientidmode="Static" />
//<input type="hidden" runat="server" id="qtdPontos" clientidmode="Static" />

        #endregion

        //private void PreencherCombos()
        //{
        //    try
        //    {
        //        CarregarComboProfessor();
        //    }
        //    catch (AcademicoException ex)
        //    {
        //        WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
        //    }
        //}

        //private void CarregarComboProfessor()
        //{
        //    ManterProfessor ManterProfessor = new ManterProfessor();
        //    IList<Professor> ListaProfessor = ManterProfessor.ObterTodosProfessores();
        //    ListaProfessor = ListaProfessor.OrderBy(x => x.Nome).ToList();
        //    WebFormHelper.PreencherLista(ListaProfessor, this.ddlProfessor, false, true);
        //}

        public void InformarAcaoDeEdicao()
        {
            // this.spanAcao.InnerText = "Edição de Turma";
        }

        public void PreencherInformacoesInformarPagamento(UsuarioPagamento pUsuarioPagamento)
        {
            if (pUsuarioPagamento != null)
            {
                string PagamentoEfetuado = string.Empty;

                this.txtDataInicioVigencia.Text = pUsuarioPagamento.DataInicioVigencia.ToShortDateString();
                this.txtFimVigencia.Text = pUsuarioPagamento.DataFimVigencia.ToShortDateString();
                this.txtValor.Text = pUsuarioPagamento.ValorPagamento.ToString();
                this.TxtDtInicioRenovacao.Text = pUsuarioPagamento.DataInicioRenovacao.ToShortDateString();
                this.txtDataMaxInadimplencia.Text = pUsuarioPagamento.DataMaxInadimplencia.ToShortDateString();

                //Data de Vencimento
                if (pUsuarioPagamento.DataVencimento.HasValue)
                {
                    this.TxtDtVencimento.Text = pUsuarioPagamento.DataVencimento.Value.ToShortDateString();
                }

                this.TxtCodInformacao.Enabled = false;

                this.TxtCodInformacao.Text = pUsuarioPagamento.NossoNumero;

                WebFormHelper.SetarValorNaCombo(pUsuarioPagamento.ConfiguracaoPagamento.ID.ToString(), this.ddlConfigPagto);

                this.DdlFormaPagamento.ClearSelection();
                WebFormHelper.SetarValorNaCombo(((int)pUsuarioPagamento.FormaPagamento).ToString(), this.DdlFormaPagamento);
                if (pUsuarioPagamento.PagamentoEfetuado == true)
                {
                    PagamentoEfetuado = Constantes.SiglaSim;
                }
                else
                {
                    PagamentoEfetuado = Constantes.SiglaNao;
                }

                this.DdlInPago.ClearSelection();
                WebFormHelper.SetarValorNaCombo(PagamentoEfetuado, this.DdlInPago);

            }

        }

        public void PreencherComboConfiguracoesPagamento()
        {
            IList<ConfiguracaoPagamento> ListaConfiguracaoPagamentoPublicoAlvo = new ManterConfiguracaoPagamento().ObterTodasConfiguracaoPagamento();
            WebFormHelper.PreencherLista(ListaConfiguracaoPagamentoPublicoAlvo, this.ddlConfigPagto, false, true);
        }

        protected void btnInformarPagamento_Click(object sender, EventArgs e)
        {
            if (this.InformouPagamento != null)
            {

                UsuarioPagamento usuarioPagamento = null;

                try
                {
                    usuarioPagamento = this.ObterObjetoUsuarioPagamento();
                    ManterUsuarioPagamento manterUsuarioPagamento = new ManterUsuarioPagamento(); ;
                    manterUsuarioPagamento.SalvarInformacoesDePagamento(usuarioPagamento);
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }

                this.IdUsuario = usuarioPagamento.Usuario.ID;

                InformouPagamento(this, new InformarPagamentoEventArgs(this.IdUsuario));

                //(IList<ItemQuestionario>)this.QuestionarioDaSessao.ListaItemQuestionario));
                //this.IdLogicoDoItemQuestionario = 0;
                ////ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "myscript", " $('#EdicaoNivel, #modal-background').removeClass('active');", true);
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "myscript", " $('#ModalQuestionario').modal('hide');", true);
                //return;
            }
        }


        public int IdUsuario
        {
            get
            {
                if (ViewState["ViewStateIdUsuario"] != null)
                {
                    return (int)ViewState["ViewStateIdUsuario"];
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                ViewState["ViewStateIdUsuario"] = value;
            }

        }



        public int IdConfiguracaoPagamento
        {
            get
            {
                if (ViewState["ViewStateIdConfiguracaoPagamento"] != null)
                {
                    return (int)ViewState["ViewStateIdConfiguracaoPagamento"];
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                ViewState["ViewStateIdConfiguracaoPagamento"] = value;
            }

        }

        public int IdUsuarioPagamento
        {
            get
            {
                if (ViewState["ViewStateIdUsuarioPagamento"] != null)
                {
                    return (int)ViewState["ViewStateIdUsuarioPagamento"];
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                ViewState["ViewStateIdUsuarioPagamento"] = value;
            }

        }

        protected void btnCalcular_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDataInicioVigencia.Text))
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Favor informar a data de início da vigência.");
            }

            try
            {
                CalcularDatas();
            }
            catch
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Erro ao processar a solicitação");
            }
        }

        private void CalcularDatas()
        {
            ManterUsuarioPagamento manterUsuarioPagamento = new ManterUsuarioPagamento();
            UsuarioPagamento usuarioPagamento = manterUsuarioPagamento.ObterInformacoesCalculadasDeConfiguracaoPagamento(int.Parse(ddlConfigPagto.SelectedItem.Value));

            this.txtFimVigencia.Text = usuarioPagamento.DataFimVigenciaCalculada.ToShortDateString();
            this.TxtDtInicioRenovacao.Text = usuarioPagamento.DataInicioRenovacaoCalculada.ToShortDateString();
            this.txtDataMaxInadimplencia.Text = usuarioPagamento.DataMaxInadimplenciaCalculada.ToShortDateString();
            this.TxtDtVencimento.Text = usuarioPagamento.DataVencimentoCalculada.ToShortDateString();

            //this.txtFimVigencia.Text = configuracaoPagamento.DataInicioCompetencia.AddDays(configuracaoPagamento.QuantidadeDiasValidade).ToShortDateString();
        }
    }


    public class CadastrarInformarPagamentoEventArgs : EventArgs
    {
        public Turma InformacoesDaTurmaCadastrada { get; set; }

        public CadastrarInformarPagamentoEventArgs(Turma turma)
        {
            InformacoesDaTurmaCadastrada = turma;
        }
    }
}