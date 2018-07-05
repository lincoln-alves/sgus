using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Extensions.Others;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP.DTO.Relatorios.DashBoard;
using System.Linq;
using Sebrae.Academico.BP.DTO.Services.Processo;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System.Web;

namespace Sebrae.Academico.WebForms
{
    public partial class DashBoard_Demandas : System.Web.UI.Page
    {
        /// <summary>
        /// TODO: Encapsular funções de pdf;
        /// TODO: Remover todas as chamadas que utilizam System.Linq; 
        /// Essa camada é apenas de visualização, não devemos ter processamento de dados nesta classe
        /// </summary>

        private int _demandasPorPagina = 10;

        public int IdNucleo
        {
            get
            {
                return ViewState["idNucleo"] != null ? (int)ViewState["idNucleo"] : 0;
            }
            set
            {
                ViewState["idNucleo"] = value;
            }
        }

        public int Status
        {
            get
            {
                return ViewState["status"] != null ? (int)ViewState["status"] : 0;
            }
            set
            {
                ViewState["status"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

            if (usuarioLogado == null || !usuarioLogado.IsAdministrador())
                Response.Redirect("/Dashboard.aspx");

            if (!Page.IsPostBack)
            {
                PreencherNucleos();
            }
        }

        private void PreencherNucleos()
        {
            var etapas = new ManterEtapaResposta().ObterDTOEtapasPendentesPorNucleo();
            rptNucleos.DataSource = etapas;
            rptNucleos.DataBind();
        }

        protected void OcultarModal_Click(object sender, EventArgs e)
        {
            OcultarModal();
        }

        private void ExibirModal()
        {
            ExibirResultado(rptDemandas.Items.Count > 0);
            pnlModal.Visible = true;
        }

        private void OcultarModal()
        {
            pnlModal.Visible = false;
        }

        protected void CarregarEtapasNucleo_Click(object sender, EventArgs e)
        {
            LinkButton botao = (LinkButton)sender;

            IdNucleo = int.Parse(botao.CommandArgument);
            Status = int.Parse(botao.CommandName);

            PreencherInformacoesNucleo(IdNucleo);
            PreencherEtapas(IdNucleo, (enumPrazoEncaminhamentoDemanda)Status);

            var total = new ManterEtapaResposta().ObterTotalPermissaoDoNucleoPorFiltro(IdNucleo, (enumPrazoEncaminhamentoDemanda)Status);
            PopulateDemandasPager(total, _demandasPorPagina);

            DefinirCor((enumPrazoEncaminhamentoDemanda)Status);
            ExibirModal();
        }

        private void PreencherInformacoesNucleo(int idNucleo)
        {
            var nucleo = new ManterHierarquiaNucleo().ObterPorId(idNucleo);

            lblNucleo.Text = nucleo.Nome;
            lblStatus.Text = EnumExtensions.GetDescription((enumPrazoEncaminhamentoDemanda)Status);
        }

        private void PreencherEtapas(int Idnucleo, enumPrazoEncaminhamentoDemanda status, int pagina = 0)
        {
            var etapas = new ManterEtapaResposta().ObterDTOPorPermissaoDoNucleoPorFiltro(Idnucleo, status, _demandasPorPagina, pagina);
            rptDemandas.DataSource = etapas;
            rptDemandas.DataBind();
        }

        private void DefinirCor(enumPrazoEncaminhamentoDemanda status)
        {
            switch (status)
            {
                case enumPrazoEncaminhamentoDemanda.NoPrazo:
                    mHeader.Attributes["class"] = "no-prazo-modal";
                    break;
                case enumPrazoEncaminhamentoDemanda.AExpirar:
                    mHeader.Attributes["class"] = "expirar-modal";
                    break;
                case enumPrazoEncaminhamentoDemanda.ForaDoPrazo:
                    mHeader.Attributes["class"] = "vencida-modal";
                    break;
                case enumPrazoEncaminhamentoDemanda.Encerrada:
                    mHeader.Attributes["class"] = "encerrada-modal";
                    break;
                default:
                    break;
            }
        }

        private void ExibirResultado(bool exibir)
        {
            pnlEtapas.Visible = exibir ? true : false;
            pnlSemResultado.Visible = exibir ? false : true;
        }

        private void PopulateDemandasPager(int recordCount, int currentPage)
        {
            var dblPageCount = (double)(recordCount / Convert.ToDecimal(_demandasPorPagina));

            var pageCount = (int)Math.Ceiling(dblPageCount);

            var pages = new List<ListItem>();

            if (pageCount > 0)
            {
                for (var i = 0; i < pageCount; i++)
                {
                    pages.Add(new ListItem((i + 1).ToString(), i.ToString(), (i == currentPage)));
                }
            }
            rptDemandasPager.DataSource = pages;
            rptDemandasPager.DataBind();
        }

        protected void lnkPage_Click(object sender, EventArgs e)
        {
            var linkButton = sender as LinkButton;

            if (linkButton == null) return;

            var pageIndex = int.Parse(linkButton.CommandArgument);

            PreencherEtapas(IdNucleo, (enumPrazoEncaminhamentoDemanda)Status, pageIndex);
        }

        /// <summary>
        /// TODO: Separa função de PDF em helper de Demandas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPdf_Click(object sender, EventArgs e)
        {
            var linkButton = sender as LinkButton;

            if (linkButton == null) return;

            var idProcessoResposta = int.Parse(linkButton.CommandArgument);

            var listaProcesso = new Sebrae.Academico.BP.Services.SgusWebService.ManterProcesso().ObterDetalhamentoProcesso(idProcessoResposta);

            // Create a new PDF document
            var document = new PdfDocument();

            var qtdEtapa = 0;
            var alturaLinha = 20;
            var margin = 30;
            var tamanhoTituloEtapa = 70;
            var posicaoInicialEtapa = 100;
            var padding = margin + 5;
            var alturaMaximaBlocoCampos = 700;

            foreach (var etapa in listaProcesso.Etapas)
            {
                // Create an empty page
                var page = document.AddPage();
                page.Size = PdfSharp.PageSize.A4;
                page.Orientation = PdfSharp.PageOrientation.Portrait;

                // Get an XGraphics object for drawing
                var gfx = XGraphics.FromPdfPage(page);

                // Create a font
                var font = new XFont("Verdana", 10);
                var textFormatter = new XTextFormatter(gfx);

                var gruposCampos = new List<DTOBlocoCampo>();

                var grupoIndex = 0;
                var alturaBloco = 0;

                foreach (var campo in etapa.ListaCampos)
                {
                    var alturaCampo =
                        CalcularAlturaCampo(gfx.MeasureString(campo.Titulo + ": " + campo.Resposta, font).Width,
                            (int)page.Width, alturaLinha);

                    alturaBloco += alturaCampo;

                    // Atualizar a altura do bloco.
                    if (gruposCampos.Any() && gruposCampos[grupoIndex] != null)
                    {
                        var altura = gruposCampos[grupoIndex].AlturaBloco;
                        var novaAltura = alturaBloco + (gruposCampos[grupoIndex].PossuiEspacoAntes ? tamanhoTituloEtapa : 0);

                        gruposCampos[grupoIndex].AlturaBloco = altura > novaAltura ? altura : novaAltura;
                    }

                    // Verificar se o bloco possui espaço antes, para as informações da Etapa.
                    var possuiEspacoAntes = true;

                    // Se os campos atingirem o limite da exibição, pular para a próxima página.
                    // Dá um espaço em cima de 3 linhas para o conteúdo da etapa.
                    if (alturaBloco > (alturaMaximaBlocoCampos - (alturaLinha * 3)))
                    {
                        alturaBloco = 0;
                        grupoIndex++;
                        possuiEspacoAntes = false;
                    }

                    if (gruposCampos.Count() != grupoIndex + 1)
                    {
                        gruposCampos.Add(new DTOBlocoCampo(possuiEspacoAntes, tamanhoTituloEtapa + alturaBloco));
                    }

                    gruposCampos[grupoIndex].Campos.Add(campo);
                }

                foreach (var grupoCampos in gruposCampos)
                {
                    gfx.DrawRectangle(XPens.Black, XBrushes.White,
                        new XRect(alturaLinha, grupoCampos.PossuiEspacoAntes ? posicaoInicialEtapa : 20, page.Width - 45,
                            grupoCampos.AlturaBloco));

                    if (gruposCampos.IndexOf(grupoCampos) == 0)
                    {
                        EscreverHeaderEtapa(textFormatter, idProcessoResposta, listaProcesso, page, font, qtdEtapa,
                            etapa, padding, grupoCampos.PossuiEspacoAntes ? posicaoInicialEtapa : 20, alturaLinha, gfx);
                    }

                    //Campos
                    var qtdCampos = 0;

                    foreach (var campo in grupoCampos.Campos)
                    {
                        // Escrever dados do Campo.
                        EscreverCampo(campo, gfx, font, page, alturaLinha, textFormatter, padding, posicaoInicialEtapa,
                            qtdCampos, tamanhoTituloEtapa, grupoCampos.PossuiEspacoAntes);

                        qtdCampos++;
                    }

                    // Se não for a última página, adiciona mais uma página ao PDF.
                    if (gruposCampos.IndexOf(grupoCampos) != gruposCampos.Count() - 1)
                    {
                        page = document.AddPage();
                        gfx = XGraphics.FromPdfPage(page);
                        textFormatter = new XTextFormatter(gfx);
                    }
                }

                // Dar continuidade ao número da etapa.
                qtdEtapa++;
            }

            var streamOutput = new MemoryStream();
            document.Save(streamOutput, false);

            Response.AddHeader("Content-Disposition",
                "attachment; filename=historicoDemanda_" + idProcessoResposta + ".pdf");
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(streamOutput.ToArray());
            Response.Flush();
        }

        private void EscreverCampo(DTOCampo campo, XGraphics gfx, XFont font, PdfPage page, int alturaLinha,
            XTextFormatter textFormatter, int padding, int posicaoInicialEtapa, int qtdCampos, int tamanhoTituloEtapa, bool possuiEspacoAntes)
        {
            textFormatter.Alignment = XParagraphAlignment.Left;

            var alturaCampo = CalcularAlturaCampo(gfx.MeasureString(campo.ObterTexto(), font).Width, (int)page.Width,
                alturaLinha);

            var rect = new XRect(padding,
                ((possuiEspacoAntes ? posicaoInicialEtapa : 40) +
                 (alturaLinha * qtdCampos) +
                 (possuiEspacoAntes ? tamanhoTituloEtapa : 0)), page.Width - 60, alturaCampo);

            textFormatter.DrawString(campo.ObterTexto(), font, XBrushes.Black, rect);
        }


        private int CalcularAlturaCampo(double larguraBruta, int larguraMaximaPagina, int alturaLinha)
        {
            var resultado = alturaLinha;
            if (larguraBruta > larguraMaximaPagina)
            {
                var qtdLinhas = (larguraBruta / larguraMaximaPagina);
                resultado = (((int)qtdLinhas + 1) * alturaLinha);
            }

            return resultado;
        }

        private static void EscreverHeaderEtapa(XTextFormatter textFormatter, int idProcessoResposta,
           DTODetalhamentoProcesso listaProcesso, PdfPage page, XFont font, int qtdEtapa, DTOEtapaInfo etapa,
           int padding,
           int posicaoInicialEtapa, int alturaLinha, XGraphics gfx)
        {
            textFormatter.Alignment = XParagraphAlignment.Left;

            // Demanda
            textFormatter.DrawString("#" + idProcessoResposta + " - " + listaProcesso.Processo.Nome,
                new XFont("Verdana", 10, XFontStyle.Bold), XBrushes.Black,
                new XRect(30, 20, page.Width - 60, page.Height - 30));

            // Demandante
            textFormatter.DrawString("Demandado por: " + listaProcesso.Processo.Demandante, font, XBrushes.Black,
                new XRect(30, 50, page.Width - 60, page.Height - 30));

            // Data de Solicitacao
            textFormatter.DrawString("Data de Solicitação: " + listaProcesso.Processo.DataSolicitacao, font,
                XBrushes.Black, new XRect(30, 70, page.Width - 60, page.Height - 30));

            // Nome da Etapa
            textFormatter.DrawString((qtdEtapa + 1) + " - ETAPA: " + etapa.Nome,
                new XFont("Verdana", 10, XFontStyle.Bold), XBrushes.Black,
                new XRect(padding, (posicaoInicialEtapa + 5), page.Width - 60, page.Height - 30));

            // Requer Aprovação 
            textFormatter.DrawString("Requer Aprovação: " + ((etapa.RequerAprovacao) ? "SIM" : "NÃO"), font,
                XBrushes.Black,
                new XRect(padding, (posicaoInicialEtapa + alturaLinha), (page.Width / 2), page.Height - 30));

            // Situacao
            textFormatter.DrawString("Status: " + etapa.Situacao.Nome, font, XBrushes.Black,
                new XRect((page.Width / 2), (posicaoInicialEtapa + alturaLinha), (page.Width / 2), page.Height - 30));

            // Concluido por
            textFormatter.DrawString(ObterNomeDoAnalista(etapa, gfx, font, page.Width, padding), font, XBrushes.Black,
                new XRect(padding, (posicaoInicialEtapa + alturaLinha + alturaLinha), (page.Width / 2), page.Height - 30));

            // Data de Preenchimento
            textFormatter.DrawString("Data de Preenchimento: " + etapa.DataPreenchimento, font, XBrushes.Black,
                new XRect((page.Width / 2), (posicaoInicialEtapa + alturaLinha + alturaLinha), (page.Width / 2),
                    page.Height - 30));
        }

        /// <summary>
        /// Obter o nome do analista. Caso o nome seja muito grande, começa a abreviar os nomes depois do primeiro nome.
        /// </summary>
        /// <param name="etapa"></param>
        /// <param name="gfx"></param>
        /// <param name="font"></param>
        /// <param name="pageWidth"></param>
        /// <param name="padding"></param>
        /// <returns></returns>
        private static string ObterNomeDoAnalista(DTOEtapaInfo etapa, XGraphics gfx, XFont font, double pageWidth, int padding)
        {
            var texto = etapa.Situacao.Nome + " por " + etapa.Analista.Nome.ToUpper();

            var metadeWidth = pageWidth / 2;

            var nome = etapa.Analista.Nome;

            while (gfx.MeasureString(texto, font).Width > metadeWidth - padding)
            {
                var nomes = nome.Split(' ');

                var atingiuLimite = false;

                // 'i = 1' para pular o primeiro nome.
                for (var i = 1; i < nomes.Length; i++)
                {
                    if (ObterPronomes(true).Contains(nomes[i]) == false && nomes[i].Length != 2)
                    {
                        nomes[i] = nomes[i].Substring(0, 1) + ".";
                        break;
                    }

                    // Se chegou no final, não há mais nada pra abreviar, então só retorna o que deu.
                    if (i == nomes.Length - 1)
                    {
                        atingiuLimite = true;
                    }
                }

                texto = etapa.Situacao.Nome.Trim() + " por " + (nome = string.Join(" ", nomes)).ToUpper();

                if (atingiuLimite)
                    return texto;
            }

            return texto;
        }

        /// <summary>
        /// Retornar uma lista de pronomes.
        /// </summary>
        /// <param name="maiusculas">True: Retorna a lista com todos os pronomes em letras maiúsculas.</param>
        /// <returns></returns>
        private static List<string> ObterPronomes(bool maiusculas = false)
        {
            var lista = new List<string>
            {
                "de",
                "do",
                "da",
                "das",
                "dos"
            };

            return maiusculas
                ? lista.Select(x => x.ToUpper()).ToList()
                : lista;
        }
    }
}