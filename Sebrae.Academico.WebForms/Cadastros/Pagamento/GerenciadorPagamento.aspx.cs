using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Dominio.DTO.Filtros;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.WebForms.UserControls;
using classes = Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.WebForms.Cadastros.Pagamento
{
    public partial class GerenciadorPagamento : PageBase
    {
        protected override enumFuncionalidade Funcionalidade
        {
            get
            {
                return enumFuncionalidade.GerenciadorPagamento;
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


        #region "Eventos Customizados"

        protected void InformarPagamento_InformouPagamento(object sender, ucInformarPagamento.InformarPagamentoEventArgs e)
        {
            //IList<ItemQuestionario> listaItemQuestionario = e.ListaItemQuestionario;
            //this.PreencherItemsDoQuestionario(listaItemQuestionario);
            //this.SetarAcaoDaTela(enumAcaoTelaQuestionario.AdicionouUmItem);


            //Exibe o Modal de 

            try
            {
                //Grava as informações do Pagamento
                this.IdUsuario = e.IdUsuario;
                OcultarModalInformarPagamento();

                //Atualiza a grid com as informações de pagamento do usuário
                pnlHistoricoPagamento.Visible = true;

                // pnlUcInformarPagamento.Visible = true;

                //this.dgvInformacoesDePagamento.Visible = false;
                //this.pnlHistoricoPagamento.Visible = true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion


        protected override void OnPreRenderComplete(EventArgs e)
        {
            if (dgvInformacoesDePagamento.Rows.Count > 0)
            {
                dgvInformacoesDePagamento.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
        protected void dgvInformacoesDePagamento_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            WebFormHelper.PaginarGrid((IList<DTOConfiguracaoPagamentoPublicoAlvo>)Session["dadosPagamento"], this.dgvInformacoesDePagamento, e.NewPageIndex);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                PreencherCombos();
                this.pnlUcInformarPagamento.Visible = false;
                this.lkbUsuarios.Attributes.Add("class", "collapsed");
                this.lkbRetornoBancario.Attributes.Add("class", "collapsed");
                base.AlterarStatusTab(this.lkbUsuarios, this.divUsuarios);
            }
        }

        protected void botao_Click(object sender, EventArgs e)
        {
            if (this.ddlTipoArquivo.SelectedItem.ToString() == "CBR643")
            {
                this.ProcessaCBR643();
            }
            else
            {
                this.ProcessaRcb001();
            }

        }

        protected void ProcessaCBR643()
        {
            try
            {
                ManterUsuarioPagamento manterUsuarioPagamento = new ManterUsuarioPagamento();
                manterUsuarioPagamento.ProcessarArquivoDeDebitoDoBancoDoBrasilCBR643(this.FileUpload1.PostedFile);
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, ex.Message);
            }
            catch
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Erro no envio de e-mail ou tipo de arquivo inválido!");
            }


        }

        /// <summary>
        /// Esta rotina processa arquivos de cartão de débito do Banco do Brasil.
        /// </summary>
        protected void ProcessaRcb001()
        {
            try
            {

                ManterUsuarioPagamento manterUsuarioPagamento = new ManterUsuarioPagamento();
                manterUsuarioPagamento.ProcessarArquivoDeDebitoDoBancoDoBrasil(this.FileUpload1.PostedFile);
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, ex.Message);
            }
            catch
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Erro no envio de e-mail ou tipo de arquivo inválido!");
            }

        }

        private void PreencherCombos()
        {
            try
            {
                PreencherComboConfiguracoesPagamento();
                PreencherListaUf();
                PreencherListaNivelOcupacional();
                PreencherListaPerfil();

                this.pnlHistoricoPagamento.Visible = false;

                //this.btnVoltar.Visible = false;
                //this.btnEnviaBB.Visible = false;

            }
            catch
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Ocorreu um erro no preenchimento das listas!");
            }
        }

        public void PreencherComboConfiguracoesPagamento()
        {
            IList<classes.ConfiguracaoPagamento> ListaConfiguracaoPagamentoPublicoAlvo = new ManterConfiguracaoPagamento().ObterTodasConfiguracaoPagamento();
            WebFormHelper.PreencherLista(ListaConfiguracaoPagamentoPublicoAlvo, this.ddlConfigPagto, false, true);
        }

        private void PreencherListaUf()
        {
            WebFormHelper.PreencherLista(new ManterUf().ObterDoUsuarioLogado(), ddlUf, false, true);
        }

        private void PreencherListaNivelOcupacional()
        {
            WebFormHelper.PreencherLista(new ManterNivelOcupacional().ObterTodosNivelOcupacional(), ddlNivelOcupacional, false, true);
        }

        private void PreencherListaPerfil()
        {
            WebFormHelper.PreencherLista(new ManterPerfil().ObterTodosPerfis(), ddlPerfil, false, true);
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

        protected void btnGerarPagamento_Click(object sender, EventArgs e)
        {
            pnlHistoricoPagamento.Visible = false;

            ucInformarPagamento1.IdUsuario = this.IdUsuario;

            ucInformarPagamento1.PreencherComboConfiguracoesPagamento();

            //Abre o panel para cadastrar um pagamento
            pnlUcInformarPagamento.Visible = true;

            ucInformarPagamento1.PrepararTelaParaInclusaoDeInformarPagamento();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                ConfiguracaoPagamentoDTOFiltro filtro = this.MontarFiltroDeConfiguracaoPagamento();
                IList<DTOConfiguracaoPagamentoPublicoAlvo> ListaDTOConfiguracaoPagamentoPublicoAlvo = new ManterConfiguracaoPagamento().ObterInformacoesDePagamentoPorFiltro(filtro);
                WebFormHelper.PreencherGrid(ListaDTOConfiguracaoPagamentoPublicoAlvo, this.dgvInformacoesDePagamento);
                pnlInformacoesDePagamento.Visible = true;

                Session.Add("dadosPagamento", ListaDTOConfiguracaoPagamentoPublicoAlvo);
                base.ExibirTab(this.lkbUsuarios, this.divUsuarios);

            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private ConfiguracaoPagamentoDTOFiltro MontarFiltroDeConfiguracaoPagamento()
        {

            ConfiguracaoPagamentoDTOFiltro filtro = new ConfiguracaoPagamentoDTOFiltro();

            //Todo: Tirar esta marreta
            //filtro.NomeUsuario = "Domingos Sávio Oliveira";

            //Filtro de Configuração de Pagamento
            if (this.ddlConfigPagto.SelectedItem != null &&
                  !string.IsNullOrWhiteSpace(this.ddlConfigPagto.SelectedItem.Value))
            {
                // idConfiguracaoPagamento = int.Parse(this.ddlConfigPagto.SelectedItem.Value);
                filtro.IdConfiguracaoPagamento = int.Parse(this.ddlConfigPagto.SelectedItem.Value);
            }

            //Filtro de Uf
            if (this.ddlUf.SelectedItem != null &&
                 !string.IsNullOrWhiteSpace(this.ddlUf.SelectedItem.Value))
            {
                filtro.IdUF = int.Parse(this.ddlUf.SelectedItem.Value);
            }


            //Filtro de Nível Ocupacional
            if (this.ddlNivelOcupacional.SelectedItem != null &&
                  !string.IsNullOrWhiteSpace(this.ddlNivelOcupacional.SelectedItem.Value))
            {
                filtro.IdNivelOcupacional = int.Parse(this.ddlNivelOcupacional.SelectedItem.Value);
            }

            //Filtro de Perfil
            if (this.ddlPerfil.SelectedItem != null &&
                  !string.IsNullOrWhiteSpace(this.ddlPerfil.SelectedItem.Value))
            {
                filtro.IdPerfil = int.Parse(this.ddlPerfil.SelectedItem.Value);
            }

            //Nome do Usuário
            if (!string.IsNullOrEmpty(txtNome.Text))
            {
                filtro.NomeUsuario = txtNome.Text.Trim();
            }

            return filtro;

        }


        protected void dgvHistoricoDePagamento_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            int idUsuarioPagamento = int.Parse(e.CommandArgument.ToString());

            if (e.CommandName.Equals("editar"))
            {
                try
                {
                    ucInformarPagamento1.IdUsuarioPagamento = idUsuarioPagamento;
                    UsuarioPagamento usuarioPagamento = new ManterUsuarioPagamento().ObterInformacoesDePagamentoPorID(idUsuarioPagamento);
                    ucInformarPagamento1.PrepararTelaParaEdicaoDeInformarPagamento(usuarioPagamento);
                    this.pnlUcInformarPagamento.Visible = true;
                    this.ucInformarPagamento1.Visible = true;
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
            }
            else if (e.CommandName.Equals("linksitebb"))
            {
                try
                {
                    this.EnviaInformacoesParaSiteDoBancoDoBrasil(idUsuarioPagamento);
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
            }
        }

        protected void dgvInformacoesDePagamento_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            //  this.UpdatePanel1.Update();

            if (e.CommandName.Equals("histpagto"))
            {
                try
                {
                    this.IdUsuario = int.Parse(e.CommandArgument.ToString()); //int.Parse(arrayComIdConfiguracaoPagamentoIdUsuario[1].ToString());

                    AtualizarGridComInformacoesDePagamento();
                    this.pnlHistoricoPagamento.Visible = true;

                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }

            }
        }

        private void AtualizarGridComInformacoesDePagamento()
        {
            IList<UsuarioPagamento> ListaUsuarioPagamento = new ManterUsuarioPagamento().ObterInformacoesDePagamentoPorUsuario(this.IdUsuario);
            WebFormHelper.PreencherGrid(ListaUsuarioPagamento, this.dgvHistoricoDePagamento);
        }

        protected void dgvHistoricoDePagamento_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //protected void btnVoltar_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        this.dgvInformacoesDePagamento.Visible = true;
        //        this.dgvHistoricoDePagamento.Visible = false;
        //        this.pnlHistoricoPagamento.Visible = false;
        //        this.btnVoltar.Visible = false;
        //        this.btnEnviaBB.Visible = false;
        //        this.pnlUcInformarPagamento.Visible = false;
        //        this.pnlHistoricoPagamento.Visible = false;

        //    }
        //    catch (AcademicoException ex)
        //    {
        //        WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
        //        return;
        //    }

        //}

        protected void dgvHistoricoDePagamento_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {

                UsuarioPagamento usuarioPagamento = (UsuarioPagamento)e.Row.DataItem;

                if (usuarioPagamento != null && usuarioPagamento.ID > 0)
                {
                    Label lblFormaPagamento = (Label)e.Row.FindControl("lblFormaPagamento");

                    if (lblFormaPagamento != null)
                    {
                        lblFormaPagamento.Text = WebFormHelper.ObterFormaDePagamentoFormatado(usuarioPagamento.FormaPagamento);
                    }

                    Label lblPago = (Label)e.Row.FindControl("lblPago");
                    if (lblPago != null)
                    {
                        lblPago.Text = WebFormHelper.ObterDescricaoDeOpcaoSimOuNao(usuarioPagamento.PagamentoEfetuado);
                    }

                    Button btnEnviaBB = (Button)e.Row.FindControl("btnEnviaBB");

                    //Quando INPago = false, ou seja, se o pagamento ainda não foi feito, então exibe o link para o site do Banco do Brasil.
                    if (!usuarioPagamento.PagamentoEfetuado)
                    {
                        if (btnEnviaBB != null)
                        {
                            btnEnviaBB.CommandArgument = usuarioPagamento.ID.ToString();
                            btnEnviaBB.Visible = true;
                        }
                        else
                        {
                            btnEnviaBB.Visible = false;
                        }
                    }
                    else
                    {
                        btnEnviaBB.Visible = false;
                    }
                }

            }
        }

        private void EnviarDadosDePagamentoParaoBancoDoBrasil(UsuarioPagamento usuarioPagamento)
        {

            {
                //this.valor.Value = usuarioPagamento.ValorPagamento.ToString();
                //this.dtVenc.Value = usuarioPagamento.DataVencimento.Value.ToString("ddMMyyyy");

                string formaPagamento = string.Empty;
                if (usuarioPagamento.FormaPagamento.Equals(enumFormaPagamento.Boleto))
                {
                    if (usuarioPagamento.PagamentoEnviadoBanco)
                    {
                        //21 -> Significa 2ª via de Boleto
                        formaPagamento = "21";
                    }
                    else
                    {
                        formaPagamento = ((int)enumFormaPagamento.Boleto).ToString();
                    }
                }
                else if (usuarioPagamento.FormaPagamento.Equals(enumFormaPagamento.DebitoEmConta))
                {
                    formaPagamento = ((int)enumFormaPagamento.DebitoEmConta).ToString();
                }

                //// tpPagamento;
                //this.urlRetorno.Value = "";
                //this.nome.Value = usuarioPagamento.Usuario.Nome;
                //this.endereco.Value = usuarioPagamento.Usuario.Endereco;
                //this.cidade.Value = usuarioPagamento.Usuario.Cidade;

                ////Pega a UF a partir do código da UF
                //ufAlfaNumerica = new ManterUf().ObterUfPorID(usuarioPagamento.Usuario.UF.ID);

                //this.uf.Value = ufAlfaNumerica.Sigla;
                //this.cep.Value = usuarioPagamento.Usuario.Cep;
                //this.msgLoja.Value = string.Empty;

                string post = @"$().redirect('https://www16.bancodobrasil.com.br/site/mpag/', {  'idConv': '311620', 
                                                            'refTran': '" + usuarioPagamento.NossoNumero + @"',
                                                            'valor': '1500',
                                                            'qtdPontos': '0',
                                                            'dtVenc': '" + usuarioPagamento.DataVencimento.Value.ToString("ddMMyyyy") + @"',
                                                            'tpPagamento': '" + formaPagamento + @"',
                                                            'urlRetorno': 'http://www5.fgv.br/fgvonline/teste1.html',
                                                            'urlInforma': 'http://www5.fgv.br/fgvonline/teste2.html',
                                                            'nome': '" + usuarioPagamento.Usuario.Nome + @"',
                                                            'endereco': 'value2',
                                                            'cidade': 'Rio de Janeiro',
                                                            'uf': 'RJ',
                                                            'cep': '24240170',
                                                            'msgLoja': 'Mensagem da UC'
                                                });";

                Page page = HttpContext.Current.CurrentHandler as Page;
                if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("mensagem"))
                {
                    ScriptManager.RegisterStartupScript(page, typeof(WebFormHelper), "mensagem", post, true);
                }
            }
        }

        protected void OcultarHistoricoPagamento_Click(object sender, EventArgs e)
        {
            this.OcultarModalHistoricoPagamento();
        }

        protected void OcultarInformarPagamento_Click(object sender, EventArgs e)
        {
            this.OcultarModalInformarPagamento();
        }

        private void OcultarModalInformarPagamento()
        {
            base.OcultarBackDrop();
            pnlUcInformarPagamento.Visible = false;
        }

        private void OcultarModalHistoricoPagamento()
        {
            base.OcultarBackDrop();
            pnlHistoricoPagamento.Visible = false;
        }

        protected void EnviaInformacoesParaSiteDoBancoDoBrasil(int idUsuarioPagamento)
        {
            try
            {
                UsuarioPagamento usuarioPagamento = new BMUsuarioPagamento().ObterInformacoesDePagamentoPorID(idUsuarioPagamento);
                this.EnviarDadosDePagamentoParaoBancoDoBrasil(usuarioPagamento);
            }
            catch (Exception ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, ex.Message);
            }

        }

        protected void lkbUsuario_Click(object sender, EventArgs e)
        {
            base.AlterarStatusTab(this.lkbUsuarios, this.divUsuarios);
        }

        protected void lkbRetornoBancario_Click(object sender, EventArgs e)
        {
            base.AlterarStatusTab(this.lkbRetornoBancario, this.divRetornoBancario);
        }



    }
}