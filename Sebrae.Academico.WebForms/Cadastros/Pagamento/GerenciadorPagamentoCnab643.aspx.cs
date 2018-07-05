using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.DTO.Filtros;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using classes = Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Facade.Classes;
using System.Data;

namespace Sebrae.Academico.WebForms.Cadastros.Pagamento
{
    public partial class GerenciadorPagamentoCnab643 : PageBase
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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                PreencherCombos();
                this.pnlUcInformarPagamento.Visible = false;
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
                manterUsuarioPagamento.ProcessarArquivoDeDebitoDoBancoDoBrasilCBR643(this.FileUpload1.PostedFile, this.TxtData.Text);


                //string dia = null;
                //string mes = null;
                //string ano = null;
                //string linhacompleta = null;
                //string identificacao = null;
                //string datalote = null;
                //string convenio = null;
                //decimal valorrecebido = 0;
                //string datadocredito = null;
                //string liquidacao = null;
                //string RefTranNossoNumero = null;

                //if ((this.FileUpload1.PostedFile != null) && 
                //    string.IsNullOrWhiteSpace(this.FileUpload1.PostedFile.FileName))
                //{
                //    //this.Label1.Text = "Escolha um arquivo texto no botão acima.";
                //    //this.Label1.Visible = true;
                //    WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Escolha um arquivo texto no botão acima.");
                //    return;
                //}

                //if (string.IsNullOrWhiteSpace(this.TxtData.Text))
                //{
                //    //this.Label1.Text = "Entre com a data desejada para o processamento, normalmente será o dia anterior.";
                //    //this.Label1.Visible = true;
                //    WebFormHelper.ExibirMensagem(enumTipoMensagem.Atencao, "Escolha um arquivo texto no botão acima.");
                //    return;
                //}

                //this.TxtData.Text = this.TxtData.Text.Replace("/", "");
                //this.TxtData.Text = this.TxtData.Text.Replace("-", "");

                //if (this.TxtData.Text.Length < 5)
                //{
                //    //Considerando que entrou dia e/ou mes sem o zero a esquerda, exemplo: 5/5/2013
                //    WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Entre com a data no formato dd/mm/aaaa.");
                //    return;
                //}

                //dia = this.TxtData.Text.Substring(0, 2);
                //mes = this.TxtData.Text.Substring(2, 2);
                //ano = this.TxtData.Text.Substring(6, 2);

                //if (int.Parse(dia) > 31)
                //{
                //    WebFormHelper.ExibirMensagem(enumTipoMensagem.Atencao, "Dia inválido");
                //    return;
                //}
                //if (int.Parse(mes) > 12)
                //{
                //    WebFormHelper.ExibirMensagem(enumTipoMensagem.Atencao, "Mês inválido");
                //    return;
                //}

                //string ddmmaa = dia + mes + ano;

                //string Planilha = FileUpload1.PostedFile.FileName;
                ////utilizar a linha abaixo quando rodando em produção
                //string caminho = Server.MapPath("../universidadecorporativasebrae/xml/");
                ////Utilizar a linha abaixo quando rodando no studio localmente
                ////string caminho = "C:\\temp\\sebrae\\";

                //string strcaminho = caminho + Planilha;

                ////Salvamos o mesmo 
                //FileUpload1.PostedFile.SaveAs(strcaminho);
                //Planilha = strcaminho;

                //string[] linhas = File.ReadAllLines(Planilha);
                ////string[] linhas = File.ReadAllLines(Server.MapPath("../universidadecorporativasebrae/xml/cnab20131011.txt"));

                //foreach (string linha in linhas)
                //{
                //    linhacompleta = linha;
                //    identificacao = linhacompleta.Substring(0, 1);

                //    if (int.Parse(identificacao) == 0) //registro header
                //    {
                //        datalote = linhacompleta.Substring(94, 6);
                //        if (ddmmaa != datalote)
                //        {
                //            WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Data do arquivo não confere com a data entrada, nenhum registro processado.");
                //            return;
                //        }
                //    }
                //    else if (int.Parse(identificacao) == 9) //registro footer
                //    {
                //        WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Arquivo processado com sucesso.");
                //        return;
                //    }
                //    else
                //    {
                //        convenio = linhacompleta.Substring(31, 7);
                //        if (convenio == "2575011")
                //        {
                //            valorrecebido = decimal.Parse(linhacompleta.Substring(253, 13));
                //            datadocredito = linhacompleta.Substring(175, 6);
                //            datadocredito = datadocredito.Substring(0, 2) + "/" + datadocredito.Substring(2, 2) + "/20" + datadocredito.Substring(4, 2);
                //            liquidacao = linhacompleta.Substring(108, 2);
                //            RefTranNossoNumero = linhacompleta.Substring(63, 17);

                //            if (liquidacao == "03" || liquidacao == "13")
                //            {
                //                //Houve problema com a liquidação - 03 = Comando recusado (Motivo indicado na posição 087/088), 13 = Abatimento Cancelado
                //            }
                //            else
                //            {
                //                if (!string.IsNullOrWhiteSpace(RefTranNossoNumero))
                //                {
                //                    manterUsuarioPagamento = new ManterUsuarioPagamento();
                //                    manterUsuarioPagamento.AtualizarInformacoesDePagamento(RefTranNossoNumero);

                //                    UsuarioPagamento usuarioPagamento = manterUsuarioPagamento.ObterInformacoesDePagamentoDoUsuarioNossoNumero(RefTranNossoNumero);

                //                    //Inserir aqui a gravação de um registro na tabela TB_UsuarioPagamento campos IN_Pago com 1 e DT_Pagamento
                //                    usuarioPagamento.DataPagamento = DateTime.Parse(datadocredito);
                //                    usuarioPagamento.ValorPagamento = valorrecebido;
                //                    manterUsuarioPagamento.IncluirUsuarioPagamento(usuarioPagamento);

                //                    //Envia e-mail para o usuário com o token
                //                    String texto;
                //                    texto = "ESTE É UM E-MAIL AUTOMÁTICO. FAVOR NÃO RESPONDER. \n";
                //                    texto += "Prezado(a) " + usuarioPagamento.Usuario.Nome + "\n";

                //                    texto += "Identificamos o recolhimento da Taxa, no valor de R$15,00, referente a sua participação em ações educacionais da Universidade corporativa Sebrae. \n";
                //                    texto += "Você deve acessar novamente o portal da UCSebrae e criar a sua senha de acesso. Em caso de dúvidas procure apoio no Fale conosco e no FAQ. \n";

                //                    texto += "A UCSebrae busca Promover ambiente de aprendizagem para o desenvolvimento de competências dos Colaboradores internos e externos, ";
                //                    texto += "contribuindo para o alcance dos resultados do SEBRAE junto aos Pequenos Negócios. Conte conosco em seu desenvolvimento para melhor atender ao empreendedor brasileiro! \n";

                //                    texto += "Atenciosamente,\n";
                //                    texto += "Universidade Corporativa Sebrae \n";

                //                    EmailFacade.Instancia.EnviarEmail(usuarioPagamento.Usuario.Email,
                //                                                      "Taxa de participação.",
                //                                                      texto);

                //                }

                //            }
                //        }
                //    }

               // }
            }
            catch 
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Tipo de arquivo inválido ou erro no envio de e-mail!");
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
                manterUsuarioPagamento.ProcessarArquivoDeDebitoDoBancoDoBrasil(this.FileUpload1.PostedFile, this.TxtData.Text);

                //string dia = null;
                //string mes = null;
                //string ano = null;
                //string linhacompleta = null;
                //string identificacao = null;
                //string datalote = null;
                //string convenio = null;
                //decimal valorrecebido = 0;
                //string datadocredito = null;
                //string RefTranNossoNumero = null;

                //if (string.IsNullOrWhiteSpace(this.FileUpload1.PostedFile.FileName))
                //{
                //    WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Escolha um arquivo texto no botão acima.");
                //    return;
                //}

                //if (string.IsNullOrWhiteSpace(this.TxtData.Text))
                //{
                //    WebFormHelper.ExibirMensagem(enumTipoMensagem.Atencao, "Escolha um arquivo texto no botão acima.");
                //    return;
                //}

                //this.TxtData.Text = this.TxtData.Text.Replace("/", "");
                //this.TxtData.Text = this.TxtData.Text.Replace("-", "");

                //if (this.TxtData.Text.Length < 5)
                //{
                //    //Considerando que entrou dia e/ou mes sem o zero a esquerda, exemplo: 5/5/2013
                //    WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Entre com a data no formato ddmmaaaa.");
                //    return;
                //}

                //dia = this.TxtData.Text.Substring(0, 2);
                //mes = this.TxtData.Text.Substring(2, 2);
                //ano = this.TxtData.Text.Substring(4, 4);

                //if (int.Parse(dia) > 31)
                //{
                //    WebFormHelper.ExibirMensagem(enumTipoMensagem.Atencao, "Dia inválido");
                //    return;
                //}
                //if (int.Parse(mes) > 12)
                //{
                //    WebFormHelper.ExibirMensagem(enumTipoMensagem.Atencao, "Mês inválido");
                //    return;
                //}

                //string ddmmaaaa = dia + mes + ano;
                //string aaaaddmm  = ano + mes + dia;

                //string Planilha = FileUpload1.PostedFile.FileName;
                ////utilizar a linha abaixo quando rodando em produção
                //string caminho = Server.MapPath("../universidadecorporativasebrae/xml/");
                ////Utilizar a linha abaixo quando rodando no studio localmente
                ////string caminho = "C:\\temp\\sebrae\\";

                //string strcaminho = caminho + Planilha;

                ////Salvamos o mesmo 
                //FileUpload1.PostedFile.SaveAs(strcaminho);
                //Planilha = strcaminho;

                //string[] linhas = File.ReadAllLines(Planilha);
                ////string[] linhas = File.ReadAllLines(Server.MapPath("../universidadecorporativasebrae/xml/cnab20131011.txt"));

                //foreach (string linha in linhas)
                //{
                //    linhacompleta = linha;
                //    identificacao = linhacompleta.Substring(0, 1);

                //    if (identificacao.ToUpper() == "A") //registro header
                //    {
                //        datalote = linhacompleta.Substring(65, 8);
                //        if (aaaaddmm != datalote)
                //        {
                //            WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Data do arquivo não confere com a data entrada, nenhum registro processado.");
                //            return;
                //        }
                //    }
                //    else if (identificacao.ToUpper() == "Z") //registro footer
                //    {
                //        WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Arquivo processado com sucesso.");
                //        return;
                //    }
                //    else //registro detalhe
                //    {
                //        convenio = linhacompleta.Substring(64, 7);
                //        if (convenio == "2575011")
                //        {
                //            valorrecebido = decimal.Parse(linhacompleta.Substring(81, 12));
                //            datadocredito = linhacompleta.Substring(29, 8);
                //            datadocredito = datadocredito.Substring(6, 2) + "/" + datadocredito.Substring(4, 2) + "/" + datadocredito.Substring(0, 4);
                //            RefTranNossoNumero = linhacompleta.Substring(64, 17);

                //            if (RefTranNossoNumero.Substring(0, 7) == "2575011")
                //            {
                //                ManterUsuarioPagamento manterUsuarioPagamento = new ManterUsuarioPagamento();
                //                manterUsuarioPagamento.AtualizarInformacoesDePagamento(RefTranNossoNumero);

                //                UsuarioPagamento usuarioPagamento = manterUsuarioPagamento.ObterInformacoesDePagamentoDoUsuarioNossoNumero(RefTranNossoNumero);

                //                //Inserir aqui a gravação de um registro na tabela TB_UsuarioPagamento campos IN_Pago com 1 e DT_Pagamento
                //                usuarioPagamento.DataPagamento = DateTime.Parse(datadocredito);
                //                usuarioPagamento.ValorPagamento = valorrecebido;
                //                manterUsuarioPagamento.IncluirUsuarioPagamento(usuarioPagamento);

                //                //Envia e-mail para o usuário com o token
                //                String texto;
                //                texto = "ESTE É UM E-MAIL AUTOMÁTICO. FAVOR NÃO RESPONDER. \n";
                //                texto += "Prezado(a) " + usuarioPagamento.Usuario.Nome + "\n";

                //                texto += "Identificamos o recolhimento da Taxa, no valor de R$15,00, referente a sua participação em ações educacionais da Universidade corporativa Sebrae. \n";
                //                texto += "Você deve acessar novamente o portal da UCSebrae e criar a sua senha de acesso. Em caso de dúvidas procure apoio no Fale conosco e no FAQ. \n";

                //                texto += "A UCSebrae busca Promover ambiente de aprendizagem para o desenvolvimento de competências dos Colaboradores internos e externos, ";
                //                texto += "contribuindo para o alcance dos resultados do SEBRAE junto aos Pequenos Negócios. Conte conosco em seu desenvolvimento para melhor atender ao empreendedor brasileiro! \n";

                //                texto += "Atenciosamente,\n";
                //                texto += "Universidade Corporativa Sebrae \n";

                //                EmailFacade.Instancia.EnviarEmail(usuarioPagamento.Usuario.Email,
                //                                                  "Taxa de participação.",
                //                                                  texto);

                //            }

                //        }
                //    }

                //}
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

                this.btnVoltar.Visible = false;
                this.btnEnviaBB.Visible = false;

                this.TxtData.Text = "";

            }
            catch 
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Ocorreu um erro no preenchimento das listas!");
            }
        }

        private void PreencherComboConfiguracoesPagamento()
        {
            IList<classes.ConfiguracaoPagamento> ListaConfiguracaoPagamentoPublicoAlvo = new ManterConfiguracaoPagamento().ObterTodasConfiguracaoPagamento();
            WebFormHelper.PreencherLista(ListaConfiguracaoPagamentoPublicoAlvo, this.ddlConfigPagto, false, true);
        }

        private void PreencherListaUf()
        {
            WebFormHelper.PreencherLista(new ManterUf().ObterTodosUf(), ddlUf, false, true);
        }

        private void PreencherListaNivelOcupacional()
        {
            WebFormHelper.PreencherLista(new ManterNivelOcupacional().ObterTodosNivelOcupacional(), ddlNivelOcupacional, false, true);
        }

        private void PreencherListaPerfil()
        {
            WebFormHelper.PreencherLista(new ManterPerfil().ObterTodosPerfis(), ddlPerfil, false, true);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                ConfiguracaoPagamentoDTOFiltro filtro = this.MontarFiltroDeConfiguracaoPagamento();
                IList<ViewConfiguracaoPagamentoPublicoAlvo> ListaViewConfiguracaoPagamentoPublicoAlvo = new ManterConfiguracaoPagamento().ObterInformacoesDePagamento(filtro);
                WebFormHelper.PreencherGrid(ListaViewConfiguracaoPagamentoPublicoAlvo, this.dgvInformacoesDePagamento);
                pnlInformacoesDePagamento.Visible = true;
                
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private ConfiguracaoPagamentoDTOFiltro MontarFiltroDeConfiguracaoPagamento()
        {

            ConfiguracaoPagamentoDTOFiltro filtro = new ConfiguracaoPagamentoDTOFiltro();

            //Filtro de Configuração de Pagamento
            if (this.ddlConfigPagto.SelectedItem != null &&
                  !string.IsNullOrWhiteSpace(this.ddlConfigPagto.SelectedItem.Value))
            {
                // idConfiguracaoPagamento = int.Parse(this.ddlConfigPagto.SelectedItem.Value);
                filtro.IdConfiguracaoPagamento = int.Parse(this.ddlConfigPagto.SelectedItem.Value);
            }
            //else
            //{
            //    pnlInformacoesDePagamento.Visible = false;
            //    WebFormHelper.ExibirMensagem("A configuração de pagamento é obrigatória");
            //    return;
            //}

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

        protected void dgvInformacoesDePagamento_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            //  this.UpdatePanel1.Update();

            if (e.CommandName.Equals("histpagto"))
            {
                try
                {
                    int idConfiguracaoPagamento = int.Parse(e.CommandArgument.ToString());
                    IList<ConfiguracaoPagamento> ListaViewConfiguracaoPagamento = new ManterConfiguracaoPagamento().ObterHistoricoDePagamento(idConfiguracaoPagamento);
                    WebFormHelper.PreencherGrid(ListaViewConfiguracaoPagamento, this.dgvHistoricoDePagamento);

                    this.dgvInformacoesDePagamento.Visible = false;
                    this.pnlHistoricoPagamento.Visible = true;

                    this.btnVoltar.Visible = true;
                    this.btnEnviaBB.Visible = true;

                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }

            }
            else if (e.CommandName.Equals("infopagto"))
            {
                try
                {
                    this.pnlUcInformarPagamento.Visible = true;

                    this.dgvInformacoesDePagamento.Visible = false;
                    this.ucInformarPagamento1.Visible = true;

                    this.pnlHistoricoPagamento.Visible = true;

                    this.btnVoltar.Visible = true;
                    this.btnEnviaBB.Visible = true;

                    //Busca dados do Usuário
                    ConfiguracaoPagamentoDTOFiltro filtro = this.MontarFiltroDeConfiguracaoPagamento();
                    IList<ViewConfiguracaoPagamentoPublicoAlvo> ListaViewConfiguracaoPagamentoPublicoAlvo = new ManterConfiguracaoPagamento().ObterInformacoesDePagamento(filtro);

                    ucInformarPagamento1.LimparCampos();

                    if (ListaViewConfiguracaoPagamentoPublicoAlvo != null && ListaViewConfiguracaoPagamentoPublicoAlvo.Count > 0)
                    {
                        int idConfiguracaoPagamento = ListaViewConfiguracaoPagamentoPublicoAlvo[0].Usuario.ID;
                        Session.Add("UsuarioPagamentoEdit", idConfiguracaoPagamento);
                        ucInformarPagamento1.PreencheCampos();
                        pnlInformacoesDePagamento.Visible = true;

                    }

                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }

            }
        }

        protected void dgvHistoricoDePagamento_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            try
            {
                this.dgvInformacoesDePagamento.Visible = true;
                this.dgvHistoricoDePagamento.Visible = false;
                this.pnlHistoricoPagamento.Visible = false;
                this.btnVoltar.Visible = false;
                this.btnEnviaBB.Visible = false;
                this.pnlUcInformarPagamento.Visible = false;
                this.pnlHistoricoPagamento.Visible = false;

            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                return;
            }

        }

        protected void dgvInformacoesDePagamento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            LinkButton lkbHistPagto = (LinkButton)e.Row.FindControl("lkbHistPagto");
            LinkButton lkbInformarPagto = (LinkButton)e.Row.FindControl("lkbInformarPagto");

            if (lkbHistPagto != null && lkbInformarPagto != null)
            {
                //TODO: -> Aplicar as regras para exibir os links
            }

        }

        private void InformarPagamento(int idUsuario, String Nome, String Endereco, String Cidade, String Cep, Uf UF)
        {
            //throw new NotImplementedException();
            //Forma de passagem de parametros por "POST"

            {
                string idconv = "311620";
                string qtdPontos = "0";
                string tpPagamento = "0";
                DateTime? DataVigencia;
                DateTime? DataMaxVigencia;
                DateTime? DataInicioRenovacao;
                DateTime? DataInicioVigencia;
                DateTime? DataFimVigencia;
                DateTime? DataMaxInadimplencia;
                string Dia, Mes, Ano;
                int dia, mes, ano;
                Uf ufAlfaNumerica;

                string xscript = "document.forms[0].enctype='application/x-www-form-urlencoded'; document.forms[0].action='https://www16.bancodobrasil.com.br/site/mpag/';" + "document.forms[0].target='_blank';" + "document.forms[0].submit();" + "document.forms[0].action='GerenciadorPagamentoCnab643.aspx';" + "document.forms[0].target='_self';";

                ClientScript.RegisterHiddenField("idConv", idconv);
                ClientScript.RegisterHiddenField("refTran", this.ucInformarPagamento1.codInformacao.ToString());
                ClientScript.RegisterHiddenField("valor", this.ucInformarPagamento1.valor.ToString());
                ClientScript.RegisterHiddenField("qtdPontos", qtdPontos);

                DataMaxVigencia = CommonHelper.TratarData(this.ucInformarPagamento1.DtMaxInadimplencia.ToString(), "Data Máxima de Inadimplência");
                DataInicioRenovacao = CommonHelper.TratarData(this.ucInformarPagamento1.DtInicioRenovacao.ToString(), "Data Inicio Renovação");
                DataInicioVigencia = CommonHelper.TratarData(this.ucInformarPagamento1.DtInicioVigencia.ToString(), "Data Início Vigência");
                DataFimVigencia = CommonHelper.TratarData(this.ucInformarPagamento1.DtFimVigencia.ToString(), "Data Fim Vigência");
                DataMaxInadimplencia= CommonHelper.TratarData(this.ucInformarPagamento1.DtMaxInadimplencia.ToString(), "Data Max Inadimplência");

                DataVigencia = CommonHelper.TratarData(this.ucInformarPagamento1.DtInicioVigencia.ToString(), "Data de Início");
                dia = Convert.ToDateTime(DataVigencia).Day;
                if (dia <= 9)
                {
                    Dia = "0" + dia;
                }
                else
                {
                    Dia = dia.ToString();
                }
                mes = Convert.ToDateTime(DataVigencia).Month;
                if (mes <= 9)
                {
                    Mes = "0" + mes;
                }
                else
                {
                    Mes = mes.ToString();
                }
                ano = Convert.ToDateTime(DataVigencia).Year;
                Ano = ano.ToString();
                ClientScript.RegisterHiddenField("dtVenc", Dia + Mes + Ano);
                ClientScript.RegisterHiddenField("tpPagamento", tpPagamento);
                ClientScript.RegisterHiddenField("urlRetorno", "http://www5.fgv.br/fgvonline/universidadecorporativasebrae/inicial.aspx");
                ClientScript.RegisterHiddenField("nome", Nome);
                ClientScript.RegisterHiddenField("endereco", Endereco);
                ClientScript.RegisterHiddenField("cidade", Cidade);
                
                //Pega a UF a partir do código da UF
                ManterUf manterUf = new ManterUf();
                ufAlfaNumerica = manterUf.ObterUfPorID(UF.ID);
                ClientScript.RegisterHiddenField("uf", ufAlfaNumerica.Sigla);

                ClientScript.RegisterHiddenField("cep", Cep);
                ClientScript.RegisterHiddenField("msgLoja", "");
                ClientScript.RegisterStartupScript(this.GetType(), "jsenviar", xscript, true);

                //Inserir aqui a gravação de um registro na tabela TB_UsuarioPagamento campos IN_Pago com 1 e DT_Pagamento
                ManterUsuarioPagamento manterUsuarioPagamento = new ManterUsuarioPagamento();
                classes.UsuarioPagamento usuarioPagamento = new UsuarioPagamento();

                //Cadastra dados dos pagamentos
                usuarioPagamento.Usuario = new ManterUsuario().ObterUsuarioPorID(idUsuario);
                usuarioPagamento.ConfiguracaoPagamento = new ManterConfiguracaoPagamento().ObterConfiguracaoPagamentoPorId(int.Parse(this.ddlConfigPagto.SelectedValue));
                usuarioPagamento.DataInicioVigencia = DateTime.Parse(this.ucInformarPagamento1.DtInicioVigencia);
                usuarioPagamento.DataFimVigencia= DateTime.Parse(this.ucInformarPagamento1.DtFimVigencia);
                usuarioPagamento.ValorPagamento = decimal.Parse(this.ucInformarPagamento1.valor.ToString());
                usuarioPagamento.DataInicioRenovacao= DateTime.Parse(this.ucInformarPagamento1.DtInicioRenovacao);
                usuarioPagamento.DataMaxInadimplencia = DateTime.Now.AddDays(368);
                usuarioPagamento.NossoNumero = this.ucInformarPagamento1.codInformacao.ToString();
                usuarioPagamento.PagamentoEfetuado = false;
                usuarioPagamento.FormaPagamento = this.ucInformarPagamento1.codFormaPagamento;
                manterUsuarioPagamento.IncluirUsuarioPagamento(usuarioPagamento);
                
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

        protected void dgvHistoricoDePagamento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //TODO: -> Aplicar as regras para exibir os links

        }

        protected void btnEnviaBB_Click(object sender, EventArgs e)
        {
            try
            {


                this.pnlHistoricoPagamento.Visible = true;

                this.btnVoltar.Visible = true;
                this.btnEnviaBB.Visible = true;

                //Testar preenchimento dos campos aqui.
                if (string.IsNullOrWhiteSpace(this.ucInformarPagamento1.codInformacao))
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Entre com um valor para o campo Cod. Informação.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(this.ucInformarPagamento1.DtMaxInadimplencia))
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Entre com um valor para a data máxima indadimplência.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(this.ucInformarPagamento1.DtInicioRenovacao))
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Entre com um valor para a data início renovação.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(this.ucInformarPagamento1.DtInicioVigencia))
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Entre com um valor para a data início vigência.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(this.ucInformarPagamento1.DtFimVigencia))
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Entre com um valor para a data fim vigência.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(this.ucInformarPagamento1.valor))
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Entre com um valor para o campo valor.");
                    return;
                }

                //Busca dados do Usuário
                ConfiguracaoPagamentoDTOFiltro filtro = this.MontarFiltroDeConfiguracaoPagamento();
                IList<ViewConfiguracaoPagamentoPublicoAlvo> ListaViewConfiguracaoPagamentoPublicoAlvo = new ManterConfiguracaoPagamento().ObterInformacoesDePagamento(filtro);

                if (ListaViewConfiguracaoPagamentoPublicoAlvo != null && ListaViewConfiguracaoPagamentoPublicoAlvo.Count > 0)
                {
                    int idConfiguracaoPagamento = ListaViewConfiguracaoPagamentoPublicoAlvo[0].Usuario.ID;
                    Session.Add("UsuarioPagamentoEdit", idConfiguracaoPagamento);

                    String Nome = ListaViewConfiguracaoPagamentoPublicoAlvo[0].Usuario.Nome;
                    String Endereco = ListaViewConfiguracaoPagamentoPublicoAlvo[0].Usuario.Endereco;
                    String Cidade = ListaViewConfiguracaoPagamentoPublicoAlvo[0].Usuario.Cidade;
                    String Cep = ListaViewConfiguracaoPagamentoPublicoAlvo[0].Usuario.Cep;
                    Uf UF = ListaViewConfiguracaoPagamentoPublicoAlvo[0].Usuario.UF;

                    //Chama User Control InformarPagamento
                    this.InformarPagamento(idConfiguracaoPagamento, Nome, Endereco, Cidade, Cep, UF);

                }
            }
            catch (Exception ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, ex.Message);
            }

        }

    }
}