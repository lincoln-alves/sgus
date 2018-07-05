using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.DTO.Filtros;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using classes = Sebrae.Academico.Dominio.Classes;


namespace Sebrae.Academico.WebForms.Cadastros
{
    public partial class GerenciarPagamento : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                base.LogarAcessoFuncionalidade();
                PreencherCombos();
            }
        }

        protected override System.Collections.Generic.IList<enumPerfil> PerfisNecessariosParaAcessarAFuncionalidade
        {
            get { throw new NotImplementedException(); }
        }

        protected override enumFuncionalidade Funcionalidade
        {
            get { return enumFuncionalidade.GerenciadorPagamento; }
        }


        private void PreencherCombos()
        {
            try
            {
                PreencherComboConfiguracoesPagamento();
                PreencherListaUf();
                PreencherListaNivelOcupacional();
                PreencherListaPerfil();
                this.btnVoltar.Visible = false;

            }
            catch (Exception ex)
            {
                throw ex;
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

                //ListarConfiguracaoPagamento tmp = new ListarConfiguracaoPagamento();

                ConfiguracaoPagamentoDTOFiltro filtro = this.MontarFiltroDeConfiguracaoPagamento();
                IList<ViewConfiguracaoPagamentoPublicoAlvo> ListaViewConfiguracaoPagamentoPublicoAlvo = new ManterConfiguracaoPagamento().ObterInformacoesDePagamento(filtro);
                //WebFormHelper.PreencherGrid(ListaViewConfiguracaoPagamentoPublicoAlvo, this.dgvInformacoesDePagamento);

                if (ListaViewConfiguracaoPagamentoPublicoAlvo != null && ListaViewConfiguracaoPagamentoPublicoAlvo.Count > 0)
                {
                    WebFormHelper.PreencherGrid(ListaViewConfiguracaoPagamentoPublicoAlvo, this.dgvInformacoesDePagamento);
                    pnlInformacoesDePagamento.Visible = true;
                }
                else
                {
                    pnlInformacoesDePagamento.Visible = false;
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Nenhuma Informação Encontrada");
                }
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

        #region "Eventos do Grid"

        protected void dgvInformacoesDePagamento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            LinkButton lkbHistPagto = (LinkButton)e.Row.FindControl("lkbHistPagto");
            LinkButton lkbInformarPagto = (LinkButton)e.Row.FindControl("lkbInformarPagto");

            if (lkbHistPagto != null && lkbInformarPagto != null)
            {
                //TODO: -> Aplicar as regras para exibir os links
            }

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
                    //this.UpdHistorico.Visible = true;
                    this.btnVoltar.Visible = true;

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
                    this.InformarPagamento();
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }

            }
        }

        private void TratarHistoricoDePagamento()
        {
            throw new NotImplementedException();
        }

        private void InformarPagamento()
        {
            throw new NotImplementedException();
        }

        #endregion

        protected void dgvHistoricoDePagamento_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void dgvHistoricoDePagamento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //TODO: -> Aplicar as regras para exibir os links
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            try
            {
                IList<ConfiguracaoPagamento> ListaViewConfiguracaoPagamento = new List<ConfiguracaoPagamento>();
                WebFormHelper.PreencherGrid(ListaViewConfiguracaoPagamento, this.dgvHistoricoDePagamento);

                //this.UpdHistorico.Visible = false;
                this.dgvInformacoesDePagamento.Visible = true;
                this.btnVoltar.Visible = false;

            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                return;
            }

        }

        protected void btnEnviarArquivo_Click(object sender, EventArgs e)
        {
            try
            {
                bool VerificaData;
                //string Caminho, StrCaminho, LinhaCompleta, Identificacao, DataLote, Convenio
                string Dia, Mes, Ano, Aaaammdd;
                string Data = "01-01-1900";
                //string ValorRecebido, DataCredito, RefTranNossoNumero;
                //string Planilha = "";
                //int IdUsuario;
                //Array Linhas;

                VerificaData = new ManterConfiguracaoPagamento().VerificarDataArquivoDeCobrancaRcb001(this.txtData.Text);
                Data = this.txtData.Text;
                Data = Data.Replace("/", "");
                Data = Data.Replace("-", "");

                //transforma a data para ddmmaaaa
                Dia = Data.Substring(0, 2);
                Mes = Data.Substring(2, 2);
                Ano = Data.Substring(4, 4);

                if (Dia.Length < 2)
                {
                    Dia = "0" + Dia;
                }

                if (Mes.Length < 2)
                {
                    Mes = "0" + Mes;
                }

                Aaaammdd = Ano + Mes + Dia;

                //        if (string.IsNullOrWhiteSpace(this.fupldArquivoRetorno.PostedFile.FileName))
                //        {
                //            //this.Label1.Text = "Escolha um arquivo texto no botão acima.";
                //            //this.Label1.Visible = true;
                //            WebFormHelper.ExibirMensagem("Escolha um arquivo texto no botão acima.");
                //            return;
                //        }

                //        if (this.fupldArquivoRetorno.HasFile)
                //        {
                //            try
                //            {
                //                string filename = Path.GetFileName(this.fupldArquivoRetorno.FileName);
                //                this.fupldArquivoRetorno.SaveAs(Server.MapPath("~/") + filename);
                //                this.fupldArquivoRetorno.PostedFile.SaveAs("c:/temp/sebrae");
                //                Planilha = this.fupldArquivoRetorno.PostedFile.FileName;
                //            }
                //            catch (Exception ex)
                //            {
                //                throw new Exception("Upload status: The file could not be uploaded. The following error occured: " + ex.Message);
                //            }
                //        }


                //        if ((this.fupldArquivoRetorno.PostedFile != null))
                //        {
                //            Planilha = this.fupldArquivoRetorno.PostedFile.FileName;
                //            Caminho = "C:/temp/sebrae";
                //            //Caminho = Server.MapPath("../universidadecorporativasebrae/xml/")

                //            StrCaminho = Caminho + Planilha;

                //            //Salvamos o mesmo 
                //            this.fupldArquivoRetorno.PostedFile.SaveAs(StrCaminho);
                //            Planilha = StrCaminho;
                //        }

                //        Linhas = File.ReadAllLines(Planilha);

                //        foreach (string Linha in Linhas)
                //        {
                //            LinhaCompleta = Linha;

                //            Identificacao = LinhaCompleta.Substring(1, 1);
                //            //Registro Header
                //            if (Identificacao == "A")
                //            {
                //                DataLote = LinhaCompleta.Substring(66, 8);
                //                if (Aaaammdd != DataLote)
                //                {
                //                    WebFormHelper.ExibirMensagem("Data do arquivo não confere com a data entrada, nenhum registro processado.", "GerenciarPagamento.aspx");
                //                }
                //                //Registro Footer
                //            }
                //            else if (Identificacao == "Z")
                //            {
                //                WebFormHelper.ExibirMensagem("Arquivo processado com sucesso.", "GerenciarPagamento.aspx");
                //                //Registro Detalhe
                //            }
                //            else
                //            {
                //                Convenio = LinhaCompleta.Substring(65, 7);
                //                //Convenio do Sebrae
                //               if (Convenio == "2575011")
                //                {
                //                    ValorRecebido = LinhaCompleta.Substring(82, 12);
                //                    DataCredito = LinhaCompleta.Substring(30, 8);
                //                    RefTranNossoNumero = LinhaCompleta.Substring(65, 17);

                //                    if (RefTranNossoNumero.Substring(1, 7) == "2575011")
                //                    {

                //                        classes.UsuarioPagamento UsuarioPagamento = new ManterUsuarioPagamento().ObterInformacoesDePagamentoDoUsuarioNossoNumero(RefTranNossoNumero);
                //                        IdUsuario = UsuarioPagamento.ID;

                //                        new ManterUsuarioPagamento().AtualizarInformacoesDePagamento(RefTranNossoNumero);

                //                        //Atualiza status do usuario
                //                        Usuario pUsuario = new ManterUsuario().ObterPorCPF(UsuarioPagamento.Usuario.CPF);
                //                        pUsuario.Situacao = "Ativo";
                //                        new ManterUsuario().Salvar(pUsuario);

                //                        //EnviaEmail(nome, email);
                //                    }

                ////                            //Verifica e libera acesso ao portal - Acertado com Marcelo em 16/12/2013 que será visto depois como será esta liberação.

                //                }
                //            }
                //        }


            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                return;
            }
        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {

            //if (string.IsNullOrWhiteSpace(this.FileUpload1.PostedFile.FileName))
            //{
            //    //this.Label1.Text = "Escolha um arquivo texto no botão acima.";
            //    //this.Label1.Visible = true;
            //    WebFormHelper.ExibirMensagem("Escolha um arquivo texto no botão acima.");
            //    return;
            //}

            //if (FileUploadControl.HasFile)
            //{
            //    try
            //    {
            //        string filename = Path.GetFileName(FileUploadControl.FileName);
            //        FileUploadControl.SaveAs(Server.MapPath("~/") + filename);
            //        StatusLabel.Text = "Upload status: File uploaded!";
            //    }
            //    catch (Exception ex)
            //    {
            //        StatusLabel.Text = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message;
            //    }
            //}
        }

        protected void botao_Click(object sender, EventArgs e)
        {
            string dia = null;
            string mes = null;
            string ano = null;
            string linhacompleta = null;
            string identificacao = null;
            string datalote = null;
            string convenio = null;
            decimal valorrecebido = 0;
            string datadocredito = null;
            string liquidacao = null;
            string RefTranNossoNumero = null;

            if (string.IsNullOrWhiteSpace(this.FileUpload1.PostedFile.FileName))
            {
                //this.Label1.Text = "Escolha um arquivo texto no botão acima.";
                //this.Label1.Visible = true;
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Escolha um arquivo texto no botão acima.");
                return;
            }

            if (string.IsNullOrWhiteSpace(this.txtData2.Text))
            {
                //this.Label1.Text = "Entre com a data desejada para o processamento, normalmente será o dia anterior.";
                //this.Label1.Visible = true;
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Escolha um arquivo texto no botão acima.");
                return;
            }

            this.txtData2.Text = this.txtData2.Text.Replace("/", "");
            this.txtData2.Text = this.txtData2.Text.Replace("-", "");

            //transforma a data para DDMMAA
            dia = this.txtData2.Text.Substring(1, 2);
            mes = this.txtData2.Text.Substring(3, 2);
            ano = this.txtData2.Text.Substring(5, 2);

            if (int.Parse(dia) > 31)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Atencao, "Dia inválido");
                return;
            }
            if (int.Parse(mes) > 12)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Atencao, "Mês inválido");
                return;
            }

            string ddmmaa = dia + mes + ano;

            string Planilha = FileUpload1.PostedFile.FileName;
            //utilizar a linha abaixo quando rodando em produção
            //string caminho = Server.MapPath("../universidadecorporativasebrae/xml/");
            //Utilizar a linha abaixo quando rodando no studio localmente
            string caminho = "C:\\temp\\sebrae\\";

            string strcaminho = caminho + Planilha;

            //Salvamos o mesmo 
            FileUpload1.PostedFile.SaveAs(strcaminho);
            Planilha = strcaminho;

            string[] linhas = File.ReadAllLines(Planilha);
            //string[] linhas = File.ReadAllLines(Server.MapPath("../universidadecorporativasebrae/xml/cnab20131011.txt"));

            foreach (string linha in linhas)
            {
                linhacompleta = linha;
                identificacao = linhacompleta.Substring(1, 1);

                if (int.Parse(identificacao) == 0) //registro header
                {
                    datalote = linhacompleta.Substring(90, 6);
                    if (ddmmaa == datalote)
                    {
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Data do arquivo não confere com a data entrada, nenhum registro processado.");
                        return;
                    }
                }
                else if (int.Parse(identificacao) == 9) //registro footer
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Arquivo processado com sucesso.");
                    return;
                }
                else
                {
                    convenio = linhacompleta.Substring(32, 7);
                    if (convenio == "2575011")
                    {
                        valorrecebido = decimal.Parse(linhacompleta.Substring(254, 13));
                        datadocredito = linhacompleta.Substring(176, 6);
                        liquidacao = linhacompleta.Substring(109, 2);
                        RefTranNossoNumero = linhacompleta.Substring(64, 17);

                        if (liquidacao == "03" || liquidacao == "13")
                        {
                            //Houve problema com a liquidação - 03 = Comando recusado (Motivo indicado na posição 087/088), 13 = Abatimento Cancelado
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(RefTranNossoNumero))
                            {
                                ManterUsuarioPagamento manterUsuarioPagamento = new ManterUsuarioPagamento();
                                manterUsuarioPagamento.AtualizarInformacoesDePagamento(RefTranNossoNumero);

                                //enviaremail

                            }

                        }
                    }
                }


            }

        }

    }
}