using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.BP.Relatorios;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.WebForms.UserControls;
using RelatoriosHelper = Sebrae.Academico.BP.Helpers.RelatoriosHelper;

namespace Sebrae.Academico.WebForms.Relatorios.DesempenhoAcademico
{
    public partial class DesempenhoAcademico : Page
    {
        readonly ManterUsuario _manterUsuario = new ManterUsuario();

        protected override void OnInit(EventArgs e)
        {

            ucNacionalizarRelatorio.Visible = false;

            ucCategorias1.TreeNodeCheckChanged += AtualizarComboSolucaoEducacional;

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Variável para impedir execuções desecessárias da busca de SEs com filtro de categoria.
            ViewState["buscouCategorias"] = true;

            if (IsPostBack) return;

            ViewState["_SE"] = null;

            var manterUsuario = new ManterUsuario();

            using (var relDa = new RelatorioDesempenhoAcademico())
            {
                AtualizarComboSolucaoEducacional(null, null);

                if (ucNacionalizarRelatorio.IsNacionalizado)
                    ucCategorias1.PreencherCategorias(false, null, null, true);
                else
                    ucCategorias1.PreencherTodasCategorias(false, null);

                ListBoxesStatus.PreencherItens(relDa.ObterStatusMatriculaTodos(), "ID", "Nome");
                ListBoxesNivelOcupacional.PreencherItens(relDa.GetNivelOcupacionalTodos(), "ID", "Nome");
                ListBoxesUF.PreencherItens(relDa.ObterUFTodos(), "ID", "Nome");
                ListBoxesUFResponsavel.PreencherItens(relDa.ObterUFTodos(), "ID", "Nome");
                ListBoxesPublicoAlvo.PreencherItens(relDa.ObterPublicoAlvoTodos(), "ID", "Nome", true);
                ListBoxesFormaDeAquisicao.PreencherItens(relDa.ObterFormaDeAquisicaoTodos(), "ID", "Nome");
            }
        }

        protected void dgRelatorio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            SelecionarPagina(e.NewPageIndex);
        }

        /// <summary>
        /// Filtrar os resultados do GridView por página e executa contagem de acessos somentes nos itens sendo exibidos na página.
        /// </summary>
        /// <param name="pageIndex">Índice da página</param>
        /// <returns></returns>
        private List<DTODesempenhoAcademico> SelecionarPagina(int? pageIndex = null)
        {
            if (pageIndex == null)
                pageIndex = dgRelatorio.PageIndex;

            var query = (IEnumerable<DTODesempenhoAcademico>)Session["dsRelatorio"];

            if (pageIndex != 0)
                query = query.Skip(pageIndex.Value * dgRelatorio.PageSize);

            var relatorio = query.ToList();

            #region Removido pela #2818

            // Contagem de acessos removida pela demanda #2818.
            //var log = new BMLogAcessoSolucaoEducacional();
            //// Executar contagem de acessos para cada usuário do relatório. É pesado e poderia já ter vindo da procedure.
            //// Aqui é um bom lugar otimizar a consulta, num futuro em que ela esteja tão inchada que nem vai mais rodar.
            //relatorio
            //    .ForEach(
            //        l =>
            //            l.SolucaoEducacionalQuantidadeDeAcessos =
            //                log.ObterQuantidadeDeAcessos(l.ID_Turma, l.ID_Oferta, l.ID_SolucaoEducacional));

            #endregion

            WebFormHelper.PaginarGrid(relatorio, dgRelatorio, pageIndex.Value);

            return relatorio;
        }

        protected void dgRelatorio_Sorting(object sender, GridViewSortEventArgs e)
        {
            var query = (IList<DTODesempenhoAcademico>)Session["dsRelatorio"];

            WebFormHelper.OrdenarListaGrid(query, dgRelatorio, e.SortExpression, e.SortDirection, "dsRelatorio", false);

            SelecionarPagina();
        }

        public void AtualizarComboSolucaoEducacional(object sender, EventArgs e)
        {
            if (ViewState["buscouCategorias"] != null && (bool)ViewState["buscouCategorias"])
            {
                var categoria = ucCategorias1.IdsCategoriasMarcadas.ToList();

                var manterSe = new ManterSolucaoEducacional();

                var solucoes = ucNacionalizarRelatorio.IsNacionalizado
                    ? manterSe.ObterTodosPorGestor()
                    : manterSe.ObterTodosSolucaoEducacional();

                if (categoria.Any())
                    solucoes =
                        solucoes.Where(s => s.CategoriaConteudo != null && categoria.Contains(s.CategoriaConteudo.ID));

                ViewState["_SE"] = Helpers.Util.ObterListaAutocomplete(solucoes);


                // Limpar SE, oferta e turma selecionadas.
                txtSolucaoEducacional.Text = "";

                txtOferta.Text = "";
                ViewState["_Oferta"] = null;

                txtTurma.Text = "";
                ViewState["_Turma"] = null;

                ViewState["buscouCategorias"] = null;
            }
        }

        protected void txtSolucaoEducacional_OnTextChanged(object sender, EventArgs e)
        {
            using (var relDa = new RelatorioDesempenhoAcademico())
            {
                var listaOfertas = relDa.ObterOfertas(string.IsNullOrWhiteSpace(txtSolucaoEducacional.Text) ? 0 : int.Parse(txtSolucaoEducacional.Text));

                ViewState["_Oferta"] = Helpers.Util.ObterListaAutocomplete(listaOfertas);


                // Limpar oferta e turma selecionadas.
                txtOferta.Text = "";

                txtTurma.Text = "";
                ViewState["_Turma"] = null;
            }
        }

        protected void txtOferta_OnTextChanged(object sender, EventArgs e)
        {
            using (var relDa = new RelatorioDesempenhoAcademico())
            {

                var listaTurma = relDa.ObterTurmas(string.IsNullOrWhiteSpace(txtOferta.Text) ? 0 : int.Parse(txtOferta.Text));

                ViewState["_Turma"] = Helpers.Util.ObterListaAutocomplete(listaTurma);

                // Limpar turma selecionada.
                txtTurma.Text = "";
            }
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            using (var relDa = new RelatorioDesempenhoAcademico())
            {
                DateTime? dataInicio = null;
                DateTime? dataFim = null;
                DateTime? dataInicioTermino = null;
                DateTime? dataFimTermino = null;
                DateTime? dataDataInicioTurma = null;
                DateTime? dataDataFinalTurma = null;
                DateTime dataTmp;

                if (!string.IsNullOrWhiteSpace(txtDataInicio.Text))
                {
                    if (DateTime.TryParse(txtDataInicio.Text, out dataTmp))
                    {
                        dataInicio = dataTmp;
                    }
                    else
                    {
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Data inicial inválida");
                        return;
                    }
                }

                if (!string.IsNullOrWhiteSpace(txtDataFinal.Text))
                {
                    if (DateTime.TryParse(txtDataFinal.Text, out dataTmp))
                    {
                        dataFim = dataTmp;
                    }
                    else
                    {
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Data final inválida");
                        return;
                    }
                }

                if (dataFim.HasValue && dataInicio.HasValue && dataFim < dataInicio)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "A data final deve ser maior que a inicial");
                    return;
                }

                if (!string.IsNullOrWhiteSpace(txtDataTerminoInicio.Text))
                {
                    if (DateTime.TryParse(txtDataTerminoInicio.Text, out dataTmp))
                    {
                        dataInicioTermino = dataTmp;
                    }
                    else
                    {
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Data inicial inválida");
                        return;
                    }
                }

                if (!string.IsNullOrWhiteSpace(txtDataTerminoFim.Text))
                {
                    if (DateTime.TryParse(txtDataTerminoFim.Text, out dataTmp))
                    {
                        dataFimTermino = dataTmp;
                    }
                    else
                    {
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Data final inválida");
                        return;
                    }
                }

                if (!string.IsNullOrWhiteSpace(txtDataInicioTurma.Text))
                {
                    if (DateTime.TryParse(txtDataInicioTurma.Text, out dataTmp))
                    {
                        dataDataInicioTurma = dataTmp;
                    }
                    else
                    {
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Data inicial turma inválida");
                        return;
                    }
                }

                if (!string.IsNullOrWhiteSpace(txtDataFinalTurma.Text))
                {
                    if (DateTime.TryParse(txtDataFinalTurma.Text, out dataTmp))
                    {
                        dataDataFinalTurma = dataTmp;
                    }
                    else
                    {
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Data fim de turma inválida");
                        return;
                    }
                }

                var categorias = ucCategorias1.IdsCategoriasMarcadas.Any()
                    ? ucCategorias1.IdsCategoriasMarcadas
                    : ucCategorias1.IdsCategoriasExistentes;

               var dTOFiltroDesempenhoAcademico =  new DTOFiltroDesempenhoAcademico(
                   txtNome.Text,
                   txtCPF.Text,
                   ListBoxesNivelOcupacional.RecuperarIdsSelecionados<int>(),
                   ListBoxesUF.RecuperarIdsSelecionados<int>(),
                   ListBoxesPublicoAlvo.RecuperarIdsSelecionados<int>(),
                   dataInicio,
                   dataFim,
                   dataInicioTermino,
                   dataFimTermino,
                   dataDataInicioTurma,
                   dataDataFinalTurma,
                   ListBoxesStatus.RecuperarIdsSelecionados<int>(),
                   string.IsNullOrWhiteSpace(txtSolucaoEducacional.Text) ? 0 : int.Parse(txtSolucaoEducacional.Text),
                   string.IsNullOrWhiteSpace(txtOferta.Text) ? 0 : int.Parse(txtOferta.Text),
                   string.IsNullOrWhiteSpace(txtTurma.Text) ? 0 : int.Parse(txtTurma.Text),
                   categorias,
                   ListBoxesFormaDeAquisicao.RecuperarIdsSelecionados<int>(),
                   ListBoxesUFResponsavel.RecuperarIdsSelecionados<int>()
                );

                var lstDa = relDa.ConsultarDesempenhoAcademico(dTOFiltroDesempenhoAcademico);

                // Insere a lista completa na sessão para não levar somente os resultados da página atual para a impressão.
                Session["dsRelatorio"] = lstDa;

                var resultado = SelecionarPagina();

                if (resultado.Any())
                {
                    var totalizadores = new List<DTOTotalizador>();

                    if (lstDa.Any())
                    {
                        // Caso não possua filtro de SE, Oferta ou Turma, exibe o totalizador que conta a quantidade de matrículas.
                        if (string.IsNullOrWhiteSpace(txtSolucaoEducacional.Text) &&
                            string.IsNullOrWhiteSpace(txtOferta.Text) && string.IsNullOrWhiteSpace(txtTurma.Text))
                        {
                            totalizadores.Add(
                                TotalizadorUtil.GetTotalizador(lstDa, "Total da quantidade de alunos registros", "CPF",
                                    enumTotalizacaoRelatorio.Contar, false));
                        }

                        totalizadores.AddRange(new List<DTOTotalizador>
                        {
                            TotalizadorUtil.GetTotalizador(lstDa, "Total da quantidade de alunos CPF", "CPF", enumTotalizacaoRelatorio.ContarDistintos, false),
                            TotalizadorUtil.GetTotalizadorComposto(lstDa, "Total da quantidade por nível ocupacional", "StatusMatricula", "NivelOcupacional"),
                            TotalizadorUtil.GetTotalizador(lstDa, "Total da quantidade por status", "StatusMatricula", enumTotalizacaoRelatorio.Contar),
                            TotalizadorUtil.GetTotalizadorComposto(lstDa, "Total da quantidade de alunos UF","StatusMatricula", "UF")
                        });

                        ucTotalizadorRelatorio.PreencherTabela(totalizadores);
                    }

                    componenteGeracaoRelatorio.Visible = true;
                    ucFormatoSaidaRelatorio.Visible = true;
                    btnPesquisar.CssClass = "btn btn-default mostrarload";
                    Filtros.CssClass = "panel-collapse collapse";
                }
                else
                {
                    ucTotalizadorRelatorio.LimparTotalizadores();
                    componenteGeracaoRelatorio.Visible = false;
                    ucFormatoSaidaRelatorio.Visible = false;
                }

                WebFormHelper.ValidarVisibilidadeCamposGrid(dgRelatorio, chkListaCamposVisiveis.Items);
            }
        }

        protected void btnGerarRelatorio_Click(object sender, EventArgs e)
        {
            if (Session["dsRelatorio"] == null)
                btnPesquisar_Click(null, null);

            var dt = Session["dsRelatorio"];

            var quantidadeRegistro = dt == null ? 0 : ((List<DTODesempenhoAcademico>)dt).Count();

            var saida = ucFormatoSaidaRelatorio.TipoSaida;

            var items = chkListaCamposVisiveis.Items;

            var nomeRelatorio = "DesempenhoAcademico.rptDesempenhoAcademico.rdlc";

            var nomeAmigavel = "Desempenho Acadêmico";

            var totalizadores = Session["dsTotalizador"];

            // Para exibir o relatório sem passar pelo sistema de solicitações de relatórios,
            // descomente a linha abaixo e comente a linha da thread do relatório mais abaixo.

            //WebFormHelper.GerarRelatorio(nomeRelatorio, dt, saida, items, totalizadores);

            RelatoriosHelper.ExecutarThreadSolicitacaoRelatorio(dt, saida, items, nomeRelatorio, nomeAmigavel, quantidadeRegistro, totalizadores);
        }

        protected void ucNacionalizarRelatorio_OnNacionalizouRelatorio(object sender, NacionalizarRelatorioEventArgs e)
        {
            using (var relDa = new RelatorioDesempenhoAcademico())
            {
                ViewState["_SE"] = Helpers.Util.ObterListaAutocomplete(relDa.ObterSolucoesEducacionais(e.IsNacionalizado ? e.UfSelecionada : null));

                var usuario = new ManterUsuario().ObterUsuarioLogado();

                ucCategorias1.PreencherTodasCategorias(false, null, usuario, true);
            }
        }
    }
}