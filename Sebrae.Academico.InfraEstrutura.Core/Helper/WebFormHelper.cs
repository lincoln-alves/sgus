using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Dominio.Enumeracao;
using System.ComponentModel;

namespace Sebrae.Academico.InfraEstrutura.Core.Helper
{
    public class WebFormHelper
    {
        public static void PreencherComponenteComOpcoesSimNao(ListControl pControle, bool inserirOpcaoSelecione = false,
            bool? simOuNao = null)
        {
            pControle.Items.Clear();

            var sim = new ListItem(Constantes.Sim, Constantes.SiglaSim);
            var nao = new ListItem(Constantes.Nao, Constantes.SiglaNao);

            if (simOuNao != null)
            {
                switch (simOuNao)
                {
                    case true:
                        sim.Selected = true;
                        break;
                    case false:
                        nao.Selected = true;
                        break;
                }
            }

            pControle.Items.Add(sim);
            pControle.Items.Add(nao);

            if (inserirOpcaoSelecione)
                pControle.Items.Insert(0, new ListItem("-- Selecione --", string.Empty));

        }

        public static void PreencherComponenteComTipoFormaDeAquisicao(ListControl controle,
            bool inserirOpcaoSeleciona = false)
        {
            int[] tipos = { (int)enumTipoFormaAquisicao.SolucaoEducacional, (int)enumTipoFormaAquisicao.Trilha };
            controle.Items.Clear();
            controle.Items.Add(new ListItem("Solução Educacional", tipos[0].ToString()));
            controle.Items.Add(new ListItem("Trilha", tipos[1].ToString()));

            if (inserirOpcaoSeleciona)
                controle.Items.Insert(0, new ListItem("-- Selecione --", string.Empty));

        }

        public static void PreencherComponenteComFormasDePagamento(ListControl pControle,
            bool inserirOpcaoSelecione = false)
        {
            pControle.Items.Clear();
            pControle.Items.Add(new ListItem(Constantes.Boleto, ((int)enumFormaPagamento.Boleto).ToString()));
            pControle.Items.Add(new ListItem(Constantes.DebitoEmConta,
                ((int)enumFormaPagamento.DebitoEmConta).ToString()));

            if (inserirOpcaoSelecione)
                pControle.Items.Insert(0, new ListItem("-- Selecione --", string.Empty));

        }

        public static void PreencherComponenteComOpcoesSexo(ListControl pControle, bool inserirOpcaoSelecione = false)
        {
            pControle.Items.Clear();
            pControle.Items.Add(new ListItem("Masculino", "Masculino"));
            pControle.Items.Add(new ListItem("Feminino", "Feminino"));

            if (inserirOpcaoSelecione)
                pControle.Items.Insert(0, new ListItem("-- Selecione --", string.Empty));

        }

        public static string ObterFormaDePagamentoFormatado(enumFormaPagamento formaPagamento)
        {
            string formaDePagamentoFormatado = string.Empty;

            switch (formaPagamento)
            {
                case enumFormaPagamento.Boleto:
                    formaDePagamentoFormatado = Constantes.Boleto;
                    break;
                case enumFormaPagamento.DebitoEmConta:
                    formaDePagamentoFormatado = Constantes.DebitoEmConta;
                    break;
            }

            return formaDePagamentoFormatado;
        }

        public static void PreencherComponenteComEstadoCivil(ListControl pControle, bool inserirOpcaoSelecione = false)
        {
            pControle.Items.Clear();
            pControle.Items.Add(new ListItem("Solteiro(a)", "Solteiro(a)"));
            pControle.Items.Add(new ListItem("Casado(a)", "Casado(a)"));
            pControle.Items.Add(new ListItem("Divorciado(a)", "Divorciado(a)"));
            pControle.Items.Add(new ListItem("Desquitado(a)", "Desquitado(a)"));
            pControle.Items.Add(new ListItem("Viúvo(a)", "Viúvo(a)"));

            if (inserirOpcaoSelecione)
                pControle.Items.Insert(0, new ListItem("-- Selecione --", string.Empty));

        }


        public static void PreencherLista(ListItemCollection pLista, ListControl pControle,
            bool pInsereOpcaoTodos = false, bool pInsereOpcaoSelecione = false)
        {
            if (pControle != null)
            {
                if (pLista != null && pLista.Count > 0)
                    pControle.DataSource = pLista;
                else
                    pControle.Items.Clear();

                pControle.DataValueField = "Value";
                pControle.DataTextField = "Text";
                pControle.DataBind();

                if (pInsereOpcaoTodos)
                {
                    pControle.Items.Insert(0, new ListItem("-- Todos --", string.Empty));
                    return;
                }

                if (pInsereOpcaoSelecione)
                {
                    pControle.Items.Insert(0, new ListItem("-- Selecione --", string.Empty));
                    return;
                }
            }
        }

        /// <summary>
        /// Preenche um ListControl com valores de um tipo enum
        /// </summary>
        /// <param name="tipoEnum"></param>
        /// <param name="controle"></param>
        public static void PreencherListaCustomizado(Type tipoEnum, ListControl controle)
        {
            if(tipoEnum.BaseType.Name != "Enum")
            {
                throw new Exception("Não é possível enviar um tipo diferente de enum");
            }

            controle.DataSource = Enum.GetNames(tipoEnum).Select(o => new { Text = o, Value = (int)(Enum.Parse(tipoEnum, o)) });
            controle.DataTextField = "Text";
            controle.DataValueField = "Value";
            controle.DataBind();
        }

        public static void PreencherListaCustomizado<T>(IList<T> pLista, ListControl pControle,
            string dataValueField, string dataTextField,
            bool pInsereOpcaoTodos = false, bool pInsereOpcaoSelecione = false)
        {
            if (pControle != null & pLista != null && pLista.Count > 0)
            {
                pControle.DataSource = pLista;
                pControle.DataValueField = dataValueField;
                pControle.DataTextField = dataTextField;

                pControle.DataBind();

                if (pInsereOpcaoTodos)
                {
                    pControle.Items.Insert(0, new ListItem("-- Todos --", string.Empty));
                    return;
                }

                if (pInsereOpcaoSelecione)
                {
                    pControle.Items.Insert(0, new ListItem("-- Selecione --", string.Empty));
                }
            }
            else
            {
                if (pControle != null)
                    pControle.Items.Clear();
            }
        }

        public static void PreencherLista<T>(IQueryable<T> pLista, ListControl pControle, bool pInsereOpcaoTodos = false,
            bool pInsereOpcaoSelecione = false, bool pPreencherComSiglaSomente = false,
            bool pPreencherComIdSomente = false)
        {
            PreencherLista(pLista.ToList(), pControle, pInsereOpcaoTodos, pInsereOpcaoSelecione,
                pPreencherComSiglaSomente, pPreencherComIdSomente);
        }

        public static void PreencherLista<T>(IList<T> pLista, ListControl pControle, bool pInsereOpcaoTodos = false,
            bool pInsereOpcaoSelecione = false, bool pPreencherComSiglaSomente = false,
            bool pPreencherComIdSomente = false, bool pSelectAll = false)
        {
            if (pControle != null)
            {
                if (pLista != null && pLista.Count > 0)
                    pControle.DataSource = pLista;
                else
                    pControle.Items.Clear();
                if (pLista != null && pLista.Count > 0)
                {
                    if (pPreencherComSiglaSomente)
                    {
                        pControle.DataValueField = "Sigla";
                    }
                    else
                    {
                        pControle.DataValueField = "ID";
                    }

                    if (pPreencherComIdSomente)
                    {
                        pControle.DataValueField = "Id";
                    }
                    else
                    {
                        pControle.DataTextField = "Nome";
                    }

                    if (pSelectAll)
                        foreach (ListItem listItem in pControle.Items)
                            listItem.Selected = true;

                    pControle.DataBind();
                }
                if (pInsereOpcaoTodos)
                {
                    pControle.Items.Insert(0, new ListItem("-- Todos --", "0"));
                    return;
                }
                else if (pInsereOpcaoSelecione)
                {
                    pControle.Items.Insert(0, new ListItem("-- Selecione --", "0"));
                    return;
                }
            }
            else
            {
                pControle.Items.Clear();
            }
        }


        public static string ObterStringAleatoria()
        {
            string textoCriptografado = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(1, 8);
            string textoCriptografadoComHashMD5 = CriptografiaHelper.ObterHashMD5(textoCriptografado);
            return textoCriptografadoComHashMD5;
        }

        public static string ObterSenhaAleatoria()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(1, 8);
        }

        public static void SetarValorNaCombo(string pValor, DropDownList pCombo, bool desabilita = false)
        {
            if (!string.IsNullOrWhiteSpace(pValor))
            {
                try
                {
                    pCombo.ClearSelection();
                    pCombo.Items.FindByValue(pValor).Selected = true;
                    pCombo.Enabled = !desabilita;
                }
                catch
                {
                    pCombo.Enabled = !desabilita;
                }
            }
        }

        public static void SetarValorNaComboPorTexto(string pValor, DropDownList pCombo, bool desabilita = false)
        {
            if (!string.IsNullOrWhiteSpace(pValor))
            {
                try
                {
                    pCombo.ClearSelection();
                    pCombo.Items.FindByText(pValor).Selected = true;
                    pCombo.Enabled = !desabilita;
                }
                catch
                {
                    pCombo.Enabled = !desabilita;
                }
            }
        }

        public static void SetarValorNoRadioButtonList(string pValor, RadioButtonList pRadioButtonList,
            bool desabilita = false)
        {
            pRadioButtonList.Items.FindByValue(pValor).Selected = true;
            pRadioButtonList.Enabled = !desabilita;
        }

        public static void SetarValorNoRadioButtonList(bool? pValor, RadioButtonList pRadioButtonList,
            bool desabilita = false)
        {
            var valorInformado = "N";

            if (pValor.HasValue && pValor.Equals(true))
                valorInformado = "S";

            // Desmarcar (Selected = false) todos os itens que não estão selecionados.
            foreach (ListItem item in pRadioButtonList.Items)
                item.Selected = item.Value == valorInformado;

            pRadioButtonList.Enabled = !desabilita;
        }

        public static void PreencherGrid<T>(IList<T> plista, GridView pGrid)
        {
            if (plista != null && plista.Count > 0)
            {
                pGrid.DataSource = plista;
                pGrid.DataBind();
                pGrid.Visible = true;
            }
            else
            {
                pGrid.DataSource = plista;
                pGrid.DataBind();
            }

            if (pGrid.Rows.Count > 0)
            {
                pGrid.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        public static void PreRenderGridView(GridView gv)
        {
            if ((gv.ShowHeader == true && gv.Rows.Count > 0) || (gv.ShowHeaderWhenEmpty == true))
            {
                gv.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            if (gv.ShowFooter == true && gv.Rows.Count > 0)
            {
                gv.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        public static void LimparVariaveisDeSessao()
        {
            HttpContext.Current.Session["Trilha"] = null;
            HttpContext.Current.Session["Questionario"] = null;
        }

        private static IList<ListItem> ObterListaOpcoesSimNao()
        {
            IList<ListItem> ListaOpcoesSimNao = new List<ListItem>();
            ListaOpcoesSimNao.Add(new ListItem("S", "Sim"));
            ListaOpcoesSimNao.Add(new ListItem("N", "Não"));
            return ListaOpcoesSimNao;
        }

        public static void ExibirMensagem(enumTipoMensagem TipoMensagem, string pMensagem)
        {

            string cleanMessage = pMensagem.Replace("\\", "\\\\");
            cleanMessage = pMensagem.Replace("'", "\\'").Replace("\n", "\\n").Replace("\r", "\\r");

            string type = "alert";
            switch (TipoMensagem)
            {
                case enumTipoMensagem.Alerta:
                    type = "alert";
                    break;
                case enumTipoMensagem.Sucesso:
                    type = "success";
                    break;
                case enumTipoMensagem.Erro:
                    type = "error";
                    break;
                case enumTipoMensagem.Atencao:
                    type = "warning";
                    break;
                case enumTipoMensagem.Informacao:
                    type = " information";
                    break;


            }

            string script = @"  
            var n = noty({
                text: '" + cleanMessage + @"',
                type: '" + type + @"',
                layout: 'center'
                });
            ";

            Page page = HttpContext.Current.CurrentHandler as Page;

            if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("mensagem"))
            {
                ScriptManager.RegisterStartupScript(page, typeof(WebFormHelper), "mensagem", script, true);
            }
        }

        public static void ExibirMensagem(enumTipoMensagem TipoMensagem, string pMensagem,
            string pPaginaParaRedirecionar)
        {
            string cleanMessage = pMensagem.Replace("\\", "\\\\");
            cleanMessage = pMensagem.Replace("'", "\\'").Replace("\n", "\\n").Replace("\r", "\\r");

            Tuple<enumTipoMensagem, string> Mensagem = new Tuple<enumTipoMensagem, string>(TipoMensagem, cleanMessage);
            HttpContext.Current.Session.Add("tpMensagem", Mensagem);

            //string script = "\nalert('" + cleanMessage + "');";
            string script = string.Format(" window.location.href='{0}';", pPaginaParaRedirecionar);

            Page page = HttpContext.Current.CurrentHandler as Page;

            if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("mensagem"))
            {
                ScriptManager.RegisterStartupScript(page, typeof(WebFormHelper), "mensagem", script, true);
            }
        }

        public static void OrdenarListaGrid<T>(IList<T> pLista, GridView pGrid, string pSortExpression,
            SortDirection pSortDirection, string pNomeVariavelSessaoDataSet, bool executarBindGridView = true)
        {
            IList<T> lstSort;
            if (pSortDirection == SortDirection.Ascending)
                lstSort = (from r in pLista
                           orderby r.GetType().GetProperty(pSortExpression).GetValue(r, null)
                           select r).ToList();
            else
                lstSort = (from r in pLista
                           orderby r.GetType().GetProperty(pSortExpression).GetValue(r, null) descending
                           select r).ToList();

            // Altera a ordenação da variável de sessão do GridView.
            if (HttpContext.Current.Session[pNomeVariavelSessaoDataSet] != null)
                HttpContext.Current.Session[pNomeVariavelSessaoDataSet] = lstSort;
            else
                HttpContext.Current.Session.Add(pNomeVariavelSessaoDataSet, lstSort);

            if (executarBindGridView)
            {
                pGrid.DataSource = lstSort;
                pGrid.DataBind();
            }
        }

        public static void PaginarGrid<T>(IList<T> pLista, GridView pGrid, int pNovoIndicedaPagina)
        {
            pGrid.DataSource = pLista;
            pGrid.PageIndex = pNovoIndicedaPagina;
            pGrid.DataBind();
        }

        public static void GerarRelatorioComGrafico(string caminhoReport, List<string> caminhoSubReports,
            object pListaDados, object pListaDadosAgrupados, enumTipoSaidaRelatorio pTipoSaidaRelatorio,
            ListItemCollection pCamposVisiveis)
        {
            string binPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "bin");
            Assembly assembly = Assembly.LoadFrom(binPath + "\\Sebrae.Academico.Reports.dll");
            Stream stream = assembly.GetManifestResourceStream("Sebrae.Academico.Reports." + caminhoReport);

            ReportViewer rv = new ReportViewer();
            rv.LocalReport.LoadReportDefinition(stream);

            foreach (var item in caminhoSubReports)
            {
                stream = assembly.GetManifestResourceStream("Sebrae.Academico.Reports." + item);
                rv.LocalReport.LoadSubreportDefinition(item, stream);
            }


            rv.LocalReport.DataSources.Clear();
            rv.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", pListaDados));

            informacoesAgrupadasPorDataDeAcesso = pListaDadosAgrupados;
            rv.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessing);

            if (pCamposVisiveis != null)
                ValidarVisibilidadeCampos(rv, pCamposVisiveis);
            rv.LocalReport.Refresh();

            var resultado = rv.LocalReport.Render(pTipoSaidaRelatorio.ToString());

            GerarArquivoRelatorio(pTipoSaidaRelatorio, resultado);
        }

        private static object informacoesAgrupadasPorDataDeAcesso { get; set; }

        private static void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            ReportDataSource r = new ReportDataSource("DataSet1", informacoesAgrupadasPorDataDeAcesso);
            e.DataSources.Add(r);
        }

        public static void GerarRelatorio(string caminhoReport, object pListaDados,
            enumTipoSaidaRelatorio pTipoSaidaRelatorio, ListItemCollection pCamposVisiveis, object totalizador = null)
        {
            var rv = GerarRelatorio(caminhoReport, pListaDados, pCamposVisiveis, totalizador);

            // Renderiza o relatório. Super pesado em relatórios grandes. Utilize o sistema de solicitação de relatórios para esses relatórios grandes.
            //if (rv.LocalReport.IsReadyForRendering) { 
            var resultado = rv.LocalReport.Render(pTipoSaidaRelatorio.ToString());

            GerarArquivoRelatorio(pTipoSaidaRelatorio, resultado);
            //}
        }

        public static ReportViewer GerarRelatorio(string caminhoReport, object pListaDados,
            ListItemCollection pCamposVisiveis, object totalizador = null)
        {
            var binPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");
            var assembly = Assembly.LoadFrom(binPath + "\\Sebrae.Academico.Reports.dll");
            var stream = assembly.GetManifestResourceStream("Sebrae.Academico.Reports." + caminhoReport);

            var rv = new ReportViewer();
            rv.LocalReport.LoadReportDefinition(stream);
            rv.LocalReport.DataSources.Clear();

            rv.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", pListaDados));

            if (totalizador != null)
            {
                //DataTable teste = ConvertToDataTable((List<DTOTotalizador>)totalizador);
                rv.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", totalizador));
                rv.LocalReport.SubreportProcessing += SetSubDataSource;


            }

            //SetImagensRelatório(rv);
            try
            {
                if (pCamposVisiveis != null)
                    ValidarVisibilidadeCampos(rv, pCamposVisiveis);
            }
            catch (Exception e)
            {
                throw e;
            }

            rv.LocalReport.Refresh();

            return rv;
        }

        public static void SetSubDataSource(object sender, SubreportProcessingEventArgs e)
        {
            var mainSource = ((LocalReport)sender).DataSources["DataSet2"];

            var dado = mainSource.Value;

            object subsource = null;

            if (dado is int || dado is long)
            {
                subsource = (int)dado;
            }

            if (dado is List<DTOTotalizador>)
            {
                subsource = dado as List<DTOTotalizador>;
            }



            if (subsource != null)
                e.DataSources.Add(new ReportDataSource("SubDataSet1", subsource));
        }

        private static MemoryStream GenerateRdl(IList<string> titulosHeader, IList<RdlColumnHeader> fields = null)
        {
            var ms = new MemoryStream();
            var gen = new RdlGenerator
            {
                Fields = fields,
                TituloHeader = titulosHeader
            };

            gen.WriteXml(ms);
            ms.Position = 0;

            // Para ler o XML exposto, descomentar abaixo e ver o valor da stringXml.
            //var stringXml = new StreamReader(ms).ReadToEnd();
            //ms.Position = 0;

            return ms;
        }

        private static IList<RdlColumnHeader> GetAvailableFields(DataTable dataTable,
            IDictionary<string, string> lsGroup = null)
        {
            var availableFields = new List<RdlColumnHeader>();
            if (lsGroup == null) lsGroup = new Dictionary<string, string>();
            for (var i = 0; i < dataTable.Columns.Count; i++)
            {
                var item = dataTable.Columns[i];
                var group = "";
                if (lsGroup.Any(p => p.Key == item.ColumnName))
                {
                    group = lsGroup.First(p => p.Key == item.ColumnName).Value;
                }
                availableFields.Add(new RdlColumnHeader
                {
                    Text = item.Caption,
                    Value = item.ColumnName,
                    Width = 2.6323,
                    Visible = true,
                    GroupBy = group
                });
            }

            return availableFields;
        }

        private static IList<RdlColumnHeader> GetColumnHeadersTotalizadores()
        {
            var availableFields = new List<RdlColumnHeader>
            {
                new RdlColumnHeader
                {
                    Text = "Descrição",
                    Value = "Descricao",
                    Width = 12.5,
                    Visible = true
                },

                new RdlColumnHeader
                {
                    Text = "Dado",
                    Value = "DadoString",
                    Width = 12.5,
                    Visible = true
                }
            };

            return availableFields;
        }

        private static IList<RdlColumnHeader> MergeFielsCamposVisiveis(DataTable table,
            ListItemCollection pCamposVisiveis, IDictionary<string, string> lsGroup = null)
        {
            var allFields = GetAvailableFields(table, lsGroup);
            if (pCamposVisiveis == null) return allFields;
            foreach (ListItem item in pCamposVisiveis)
            {
                var obj = allFields.FirstOrDefault(p => p.Value == item.Value);
                if (obj == null) continue;

                obj.Visible = item.Selected;
                obj.Width = 5;

                allFields[allFields.IndexOf(obj)] = obj;
            }
            return allFields;
        }

        public static MemoryStream PrepararRelatorioVariasTabelas(IList<string> titulosHeader, DataSet ds,
            ListItemCollection pCamposVisiveis)
        {
            var ms = new MemoryStream();
            var gen = new RdlGeneratorTabelas
            {
                TituloHeader = titulosHeader
            };

            foreach (DataTable table in ds.Tables)
            {
                var allFields = MergeFielsCamposVisiveis(table, pCamposVisiveis);
                gen.AddGrid(table.TableName, table, allFields);
            }

            gen.WriteXml(ms);
            ms.Position = 0;

            // Para ler o XML exposto, descomentar abaixo e ver o valor da stringXml.
            //var stringXml = new StreamReader(ms).ReadToEnd();

            return ms;
        }

        public static IList<ReportDataSource> GerarDataSetsRelatorio(DataSet ds, string nomeclatura = "DataSet")
        {
            var dataSets = new List<ReportDataSource>();
            var i = 1;
            foreach (DataTable table in ds.Tables)
            {
                dataSets.Add(new ReportDataSource(nomeclatura + i.ToString(), table.Copy()));
                //deixei o ToString() para facilitar a leitura caso alguem precise fazer alguma alteração.

                i++;
            }
            return dataSets;
        }

        public static ReportViewer GerarRelatorio(MemoryStream relatorio, IList<ReportDataSource> dataSets,
            enumTipoSaidaRelatorio pTipoSaidaRelatorio, ListItemCollection pCamposVisiveis,
            IList<string> ignorarCampos = null)
        {
            var rv = new ReportViewer();

            rv.LocalReport.LoadReportDefinition(relatorio);

            rv.LocalReport.DataSources.Clear();
            var i = 1;
            foreach (var item in dataSets)
            {
                rv.LocalReport.DataSources.Add(item);
                i++;
            }
            try
            {
                if (pCamposVisiveis != null) ValidarVisibilidadeCampos(rv, pCamposVisiveis);
            }
            catch
            {
            }

            rv.LocalReport.Refresh();

            var resultado = rv.LocalReport.Render(pTipoSaidaRelatorio.ToString());

            GerarArquivoRelatorio(pTipoSaidaRelatorio, resultado);

            return rv;
        }

        public static ReportViewer GerarRelatorioTable(IList<string> titulosHeader, DataTable pListaDados,
            enumTipoSaidaRelatorio pTipoSaidaRelatorio, ListItemCollection pCamposVisiveis,
            IList<string> ignorarCampos = null, IDictionary<string, string> lsGroup = null)
        {
            var allFields = MergeFielsCamposVisiveis(pListaDados, pCamposVisiveis, lsGroup);

            var mRdl = GenerateRdl(titulosHeader, allFields);

            var dataSets = new List<ReportDataSource> { new ReportDataSource("DataSet1", pListaDados.Copy()) };

            return GerarRelatorio(mRdl, dataSets, pTipoSaidaRelatorio, pCamposVisiveis, ignorarCampos);
        }

        public static void GerarRelatorioDoIndice(string caminhoReport, object pListaDados,
            enumTipoSaidaRelatorio pTipoSaidaRelatorio, ListItemCollection pCamposVisiveis, int indice,
            int quantidadePorPaginas)
        {
            var binPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");
            var assembly = Assembly.LoadFrom(binPath + "\\Sebrae.Academico.Reports.dll");
            var stream = assembly.GetManifestResourceStream("Sebrae.Academico.Reports." + caminhoReport);
            IList objetos = null;

            if (pListaDados != null)
            {
                objetos = pListaDados as IList;

                if (objetos.Count > 0)
                {
                    objetos = objetos.Cast<object>().ToList().Skip(indice * quantidadePorPaginas).Take(100).ToList();
                }
            }

            var rv = new ReportViewer();
            rv.LocalReport.LoadReportDefinition(stream);
            rv.LocalReport.DataSources.Clear();
            rv.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", objetos));

            //SetImagensRelatório(rv);
            try
            {
                if (pCamposVisiveis != null)
                    ValidarVisibilidadeCampos(rv, pCamposVisiveis);
            }
            catch
            {
            }
            rv.LocalReport.Refresh();

            var resultado = rv.LocalReport.Render(pTipoSaidaRelatorio.ToString());

            GerarArquivoRelatorio(pTipoSaidaRelatorio, resultado);
        }

        public static void GerarArquivoRelatorio(enumTipoSaidaRelatorio pTipoSaidaRelatorio, byte[] resultado,
            string nomeArquivo = "relatorio.")
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearHeaders();
            var extensao = string.Empty;
            switch (pTipoSaidaRelatorio)
            {
                case enumTipoSaidaRelatorio.PDF:
                    HttpContext.Current.Response.ContentType = "application/pdf";
                    extensao = "pdf";
                    break;
                case enumTipoSaidaRelatorio.WORD:
                    HttpContext.Current.Response.ContentType = "application/msword";
                    extensao = "doc";
                    break;

                case enumTipoSaidaRelatorio.EXCEL:
                    HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                    extensao = "xls";
                    break;
            }

            HttpContext.Current.Response.AddHeader("Content-Disposition",
                "attachment; filename=\"" + nomeArquivo + "." + extensao + "\"");
            HttpContext.Current.Response.AddHeader("Content-Length", resultado.Length.ToString());
            HttpContext.Current.Response.OutputStream.Write(resultado, 0, resultado.Length);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

        public static void GerarArquivoRelatorio(enumTipoSaidaRelatorio pTipoSaidaRelatorio, GridView grid,
           string nomeArquivo = "relatorio")
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearHeaders();
            var extensao = string.Empty;
            switch (pTipoSaidaRelatorio)
            {
                case enumTipoSaidaRelatorio.PDF:
                    HttpContext.Current.Response.ContentType = "application/pdf";
                    extensao = "pdf";
                    break;
                case enumTipoSaidaRelatorio.WORD:
                    HttpContext.Current.Response.ContentType = "application/msword";
                    extensao = "doc";
                    break;

                case enumTipoSaidaRelatorio.EXCEL:
                    HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                    extensao = "xls";
                    break;
            }

            System.IO.StringWriter stringWrite = new System.IO.StringWriter();

            System.Web.UI.HtmlTextWriter htmlWrite =
            new HtmlTextWriter(stringWrite);

            grid.RenderControl(htmlWrite);

            HttpContext.Current.Response.AddHeader("Content-Disposition",
                "attachment; filename=\"" + nomeArquivo + "." + extensao + "\"");

            HttpContext.Current.Response.Write(stringWrite.ToString());
            HttpContext.Current.Response.End();
        }

        private static void SetImagensRelatorio(ReportViewer rv)
        {
            rv.LocalReport.EnableExternalImages = true;

            ReportParameter ImagemCabecalho;
            ReportParameter ImagemRodape;


            ImagemCabecalho = new ReportParameter("Header",
                HttpContext.Current.Server.MapPath("~/img") + "\\Cabecalho.gif");
            ImagemRodape = new ReportParameter("Footer",
                HttpContext.Current.Server.MapPath("~/img") + "\\relatorio_footer_retrato.jpg");

            rv.LocalReport.SetParameters(ImagemCabecalho);
            rv.LocalReport.SetParameters(ImagemRodape);
        }

        private static void ValidarVisibilidadeCampos(ReportViewer pRelatorio, ListItemCollection pCamposVisiveis)
        {
            var parameters = new ReportParameter[pCamposVisiveis.Count];
            var i = 0;

            foreach (ListItem item in pCamposVisiveis)
            {
                parameters[i] = new ReportParameter(item.Value + "Visible", item.Selected ? "F" : "V");

                i++;
            }

            pRelatorio.LocalReport.SetParameters(parameters);
        }

        public static void ValidarVisibilidadeCamposGrid(GridView pGrid, ListItemCollection pCamposVisiveis)
        {
            foreach (ListItem item in pCamposVisiveis)
            {
                foreach (var colunaObjeto in pGrid.Columns)
                {
                    if (colunaObjeto.GetType() == typeof(BoundField))
                    {
                        var coluna = (BoundField)colunaObjeto;

                        if (coluna.DataField == item.Value || coluna.DataField == item.Text ||
                            coluna.HeaderText.ToUpper() == item.Text.ToUpper())
                            coluna.Visible = item.Selected;
                    }
                }
            }

            pGrid.DataBind();
        }

        public static void PreencherListaStatusMatricula(ListControl pLista, bool pInseririOpcaoTodos,
            bool pInserirOpcaoSelecione)
        {

            if (pLista.Items.Count > 0)
                pLista.Items.Clear();

            if (pInseririOpcaoTodos)
                pLista.Items.Add(new ListItem("- Todos -", string.Empty));
            else if (pInserirOpcaoSelecione)
                pLista.Items.Add(new ListItem("- Selecione -", string.Empty));

            foreach (var st in (IList<enumStatusMatricula>)Enum.GetValues(typeof(enumStatusMatricula)))
            {
                pLista.Items.Add(new ListItem(st.ToString(), ((int)st).ToString()));
            }


        }

        public static string ObterDescricaoDeOpcaoSimOuNao(bool pagou)
        {
            string simNaoFormatado = string.Empty;

            if (pagou)
            {
                simNaoFormatado = Constantes.Sim;
            }
            else
            {
                simNaoFormatado = Constantes.Nao;
            }

            return simNaoFormatado;
        }

        public static int? ConverterParaInteiroNull(string valor)
        {
            int? retorno = null;
            if (!string.IsNullOrEmpty(valor))
            {
                int teste = 0;
                if (int.TryParse(valor, out teste))
                    retorno = teste;
            }
            return retorno;
        }

        public static int? ConverterParaInteiro(string valor)
        {
            int retorno = 0;
            if (!string.IsNullOrEmpty(valor))
            {
                int teste = 0;
                if (int.TryParse(valor, out teste))
                    retorno = teste;
            }
            return retorno;
        }

        public static string FormatarCPF(string cpf)
        {
            string retono = string.Empty;
            try
            {
                retono = Convert.ToUInt64(cpf).ToString(@"000\.000\.000\-00");
            }
            catch
            {
                retono = string.Empty;
            }
            return retono;
        }

        public static int[] ObterValoresSelecionadosCheckBoxList(CheckBoxList checkList)
        {

            List<int> values_list = new List<int>();

            for (int i = 0; i < checkList.Items.Count; i++)
            {

                if (checkList.Items[i].Selected)
                {
                    values_list.Add(Convert.ToInt32(checkList.Items[i].Value));
                }
            }

            return values_list.ToArray();
        }

        /*
          Pega os valores selecionados de um repeater com checkbox dentro
         */

        public static int[] ObterValoresSelecionadosRepeaterCheckBox(Repeater repeater, string checkBoxId,
            string atributeName)
        {

            List<int> values_list = new List<int>();

            for (int i = 0; i < repeater.Items.Count; i++)
            {
                CheckBox ckUF = (CheckBox)repeater.Items[i].FindControl(checkBoxId);

                if (ckUF.Checked)
                {
                    values_list.Add(Convert.ToInt32(ckUF.Attributes[atributeName]));
                }
            }

            return values_list.ToArray();
        }

        /// <summary>
        /// Preenche texto na linha inteira do footer da GridView. A propriedade ShowFooter da GridView deve estar como "true"
        /// </summary>
        public static void PreencherTextoFooter(GridView gridView, string texto, HorizontalAlign align)
        {
            if (!gridView.ShowFooter || gridView.FooterRow == null)
                return;

            gridView.FooterRow.Cells[0].Text = texto;
            gridView.FooterRow.Cells[0].ColumnSpan = gridView.Columns.Count;
            gridView.FooterRow.Cells[0].HorizontalAlign = align;

            //Removendo as outras células
            while (gridView.FooterRow.Cells.Count > 1)
            {
                gridView.FooterRow.Cells.RemoveAt(1);
            }
        }

        public static void PrevinirCliqueDuplo(List<Button> controles, Page pagina)
        {
            foreach (Button botao in controles)
            {
                botao.Attributes.Add("onclick",
                    " this.disabled = true; " + pagina.ClientScript.GetPostBackEventReference(botao, null) + ";");
            }
        }

        public static List<Dictionary<string, object>> GetDictionaryFromDataTable(DataTable dataTable)
        {
            return (from DataRow row in dataTable.Rows
                    select
                        dataTable.Columns.Cast<DataColumn>()
                            .ToDictionary(column => column.ColumnName, column => row[column])).ToList();
        }

    }
}
