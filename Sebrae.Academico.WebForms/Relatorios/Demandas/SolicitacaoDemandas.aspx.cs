using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.BP.Relatorios;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Extensions.Others;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using RelatoriosHelper = Sebrae.Academico.BP.Helpers.RelatoriosHelper;

namespace Sebrae.Academico.WebForms.Relatorios.Demandas
{
    public partial class SolicitacaoDemandas : System.Web.UI.Page
    {
        public string[] CamposCabecalho
        {
            get
            {
                return new string[]
                {
                    "Número do Processo",
                    "Usuário Demandante",
                    "Demanda",
                    "Data Abertura",
                    "Etapa Atual",
                    "Status"
                };
            }
        }

        public bool ExibirCampo(object obj)
        {
            var campo = (string)obj;
            foreach (ListItem item in chkListaCamposVisiveis.Items)
            {
                if (campo.Contains(item.Text) && item.Selected)
                {
                    return true;
                }

                if (campo.Contains(item.Value) && item.Selected)
                {
                    return true;
                }
            }

            return false;
        }

        public ListItem[] CheckBoxPadrao
        {
            get
            {
                return new ListItem[] {
                        new ListItem
                    {
                        Text = "Número do Processo",
                        Value = "Número do Processo",
                        Selected = true
                    },
                        new ListItem
                    {
                        Text = "Usuário Demandante",
                        Value = "Usuário Demandante",
                        Selected = true,
                        Enabled = false
                    },
                      new ListItem
                    {
                        Text = "Demanda",
                        Value = "Demanda",
                        Selected = true,
                        Enabled = false
                    },
                        new ListItem
                    {
                        Text = "Data Abertura",
                        Value = "Data Abertura",
                        Selected = true
                    },
                        new ListItem
                    {
                        Text = "Etapa em Andamento",
                        Value = "Etapa Atual",
                        Selected = true
                    },
                        new ListItem
                    {
                        Text = "Status",
                        Value = "Status",
                        Selected = true
                    },
                        new ListItem
                    {
                        Text = "Valor Previsto de Inscrição",
                        Value = "Valor Previsto de Inscrição",
                        Selected = true
                    },
                         new ListItem
                    {
                        Text = "Valor Previsto de Passagem",
                        Value = "Valor Previsto de Passagem",
                        Selected = true
                    },
                        new ListItem
                    {
                        Text = "Valor Previsto de Diária",
                        Value = "Valor Previsto de Diária",
                        Selected = true
                    },
                    new ListItem
                    {
                        Text = "Valor Executado de Inscrição",
                        Value = "Valor Executado de Inscrição",
                        Selected = true
                    },
                    new ListItem
                    {
                        Text = "Valor Executado de Passagem",
                        Value = "Valor Executado de Passagem",
                        Selected = true
                    },
                    new ListItem
                    {
                        Text = "Valor Executado de Diárias",
                        Value = "Valor Executado de Diárias",
                        Selected = true
                    },
                    new ListItem
                    {
                        Text = "Data de Início da Capacitação",
                        Value = "Data de Início da Capacitação",
                        Selected = true
                    },
                    new ListItem
                    {
                        Text = "Data de Término da Capacitação",
                        Value = "Data de Término da Capacitação",
                        Selected = true
                    },
                    new ListItem
                    {
                        Text = "Unidade",
                        Value = "Unidade",
                        Selected = true
                    },
                    new ListItem
                    {
                        Text = "Local",
                        Value = "Local",
                        Selected = true
                    },
                    new ListItem
                    {
                        Text = "Título da Capacitação",
                        Value = "Título da Capacitação",
                        Selected = true
                    },
                    new ListItem
                    {
                        Text = "Carga Horária",
                        Value = "Carga Horária",
                        Selected = true
                    }
                };
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                HttpCookie filtroCookie = Request.Cookies["activeFiltroGroup"];
                if (filtroCookie != null)
                {
                    Filtros.CssClass = "panel-collapse collapse in";
                }

                return;
            }

            using (var bm = new BMProcesso())
            {
                WebFormHelper.PreencherLista(bm.ObterTodos().OrderBy(x => x.Nome).ToList(), cbxProcesso, true, false);
            }

            var listaStatusResposta = ((IEnumerable<enumStatusProcessoResposta>)Enum.GetValues(typeof(enumStatusProcessoResposta))).Select(c => new { ID = (int)c, Nome = c.GetDescription() }).ToList();
            WebFormHelper.PreencherListaCustomizado(listaStatusResposta, cbxStatus, "ID", "Nome", true, false);

            var listaUnidades = new ManterUsuario().ObterTodosIQueryable()
                .Select(x => x.Unidade)
                .Distinct()
                .ToList()
                .Select(nomeUnidade => new { ID = nomeUnidade, Nome = nomeUnidade })
                .Where(x => !string.IsNullOrWhiteSpace(x.Nome))
                .OrderBy(y => y.Nome)
                .AsEnumerable();

            listUnidades.PreencherItens(listaUnidades, "ID", "Nome");


            if (!IsPostBack)
            {
                if (chkListaCamposVisiveis.Items.Count == 0)
                {
                    PreencherCheckBoxPadrao();
                }
            }
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            using (var bm = new BMProcesso())
            {
                int idProcesso, idEtapa;

                var solicitacoes = new RelatorioSolicitacaoDemandas().ObterSolicitacoes();

                int idProcessoResposta;
                if(int.TryParse(txtProcessoResposta.Text, out idProcessoResposta))
                {
                    solicitacoes = solicitacoes.Where(x => x.EtapaResposta.ProcessoResposta.ID == idProcessoResposta);
                }

                if (int.TryParse(cbxProcesso.SelectedValue, out idProcesso) && idProcesso > 0)
                {
                    solicitacoes = solicitacoes.Where(x => x.IdProcesso == idProcesso);
                }

                if (int.TryParse(cbxDemanda.SelectedValue, out idEtapa) && idEtapa > 0)
                {
                    solicitacoes = solicitacoes.Where(x => x.IdEtapa == idEtapa);
                }

                var usuario = ucLupaUsuario.SelectedUser;

                if (usuario != null)
                {
                    solicitacoes = solicitacoes.Where(x => x.IdUsuario == usuario.ID);
                }

                DateTime dtAbertura, dtIni, dtFim;
                DateTime? dtIniConvertido = null, dtFimConvertido = null;

                if (DateTime.TryParse(txtDataAbertura.Text, out dtAbertura))
                {
                    solicitacoes = solicitacoes.Where(x => x.DataAbertura.Date == dtAbertura.Date);
                }

                if (DateTime.TryParse(txtDataInicio.Text, out dtIni))
                {
                    dtIniConvertido = dtIni;
                    solicitacoes = solicitacoes.Where(x =>
                    x.EtapaResposta.ListaCampoResposta.Any(c => c.Nome == "Data de Início da Capacitação" && DateTime.Parse(c.Resposta) >= dtIniConvertido));
                }

                if (DateTime.TryParse(txtDataFim.Text, out dtFim))
                {
                    dtFimConvertido = dtFim;

                    solicitacoes = solicitacoes.Where(x =>
                                     x.EtapaResposta.ListaCampoResposta.Any(c => c.Nome == "Data de Término da Capacitação" && DateTime.Parse(c.Resposta) >= dtFimConvertido));
                }

                int idStatus = 0;
                if (int.TryParse(cbxStatus.SelectedValue, out idStatus))
                {
                    solicitacoes = solicitacoes.Where(x => (int)x.Status == idStatus);
                }

                var unidades = listUnidades.RecuperarIdsSelecionados<string>().ToList();
                if (unidades.Any())
                {
                    solicitacoes = solicitacoes.Where(x => unidades.Any(u => u == x.Unidade));
                }

                var consulta = solicitacoes.ToList();

                if (consulta != null && consulta.Any())
                {
                    btnPesquisar.CssClass = "btn btn-default mostrarload";
                    Filtros.CssClass = "panel-collapse collapse";

                    var camposPrevisto = consulta.Select(x => x.EtapaResposta.ListaCampoResposta.Where(cr =>
                    cr.Campo.Nome == "Valor Previsto de Inscrição" ||
                    cr.Campo.Nome == "Valor Previsto de Passagem" ||
                    cr.Campo.Nome == "Valor Previsto de Diária").Select(c => c.Resposta));

                    var resulCamposPrevisto = camposPrevisto.Where(x => x != null).Select(x => x.FirstOrDefault());
                    var valorTotalPrevisto = resulCamposPrevisto.Where(x => x != null).Sum(x => double.Parse(x));

                    lblValorTotalPrevisto.Text = string.Format("<b>Valor Total Previsto:</b> {0}", valorTotalPrevisto);

                    var camposExecutado = consulta.Select(x => x.EtapaResposta.ListaCampoResposta.Where(cr =>
                    cr.Campo.Nome == "Valor Executado de Inscrição" ||
                    cr.Campo.Nome == "Valor Executado de Passagem" ||
                    cr.Campo.Nome == "Valor Executado de Diária"
                    ).Select(c => c.Resposta));

                    var resulCamposExecutado = camposExecutado.Where(x => x != null).Select(x => x.FirstOrDefault());
                    var valorTotalExecutado = resulCamposExecutado.Where(x => x != null).Sum(x => double.Parse(x));

                    lblValorTotalExecutado.Text = string.Format("<b>Valor Total Executado:</b> {0}", valorTotalExecutado);

                    componenteGeracaoRelatorio.Visible = true;
                }
                else
                {
                    componenteGeracaoRelatorio.Visible = false;
                }

                divTotalizadores.Visible = true;
                lblQuantidadeEncontrada.Text = string.Format("<b>Total de registros encontrados:</b> {0}", consulta.Count());


                var usuarioSessao = new ManterUsuario().ObterUsuarioLogado();
                var hashCache = usuarioSessao.ID.ToString();

                Session["hashCache"] = hashCache;

                rptCabecalho.DataSource = null;
                rptCabecalho.DataBind();

                rptRelatorio.DataSource = solicitacoes;
                rptRelatorio.DataBind();

                dvResultado.Visible = true;

                Cache["dsRelatorio_" + hashCache] = Session["dsRelatorio"] = consulta;
                Cache["idEtapa"] = idEtapa;
            }
        }

        protected void dgRelatorio_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells.Count > 0)
                {
                    foreach (TableCell cell in e.Row.Cells)
                    {
                        if (cell.Text == "" || cell.Text == "&nbsp;") //selecione o index correto do campo no gridView
                        {
                            cell.Text = "--";
                        }
                    }
                }

            }
        }

        protected void cbxProcesso_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idSelecionado;

            if (int.TryParse(cbxProcesso.SelectedValue, out idSelecionado))
            {
                var etapas = new ManterEtapa().ObterPorProcessoId(idSelecionado);
                WebFormHelper.PreencherLista(etapas, cbxDemanda, false, true);

                PreencherCheckBoxPadrao();
            }
        }

        protected void btnGerarRelatorio_Click(object sender, EventArgs e)
        {
            if (Session["dsRelatorio"] == null)
                btnPesquisar_Click(null, null);

            var dt = Session["dsRelatorio"];

            Cache["dsCamposVisiveis_" + Session["hashCache"]] = chkListaCamposVisiveis.Items;

            var quantidadeRegistro = dt == null ? 0 : ((List<DTORelatorioSolicitacaoDemanda>)dt).Count();

            var saida = (enumTipoSaidaRelatorio)int.Parse(rblTipoSaida.SelectedValue);

            var requestUrl = new ManterConfiguracaoSistema().ObterConfiguracaoSistemaPorID(
             (int)enumConfiguracaoSistema.EnderecoSGUS).Registro + "/Relatorios/Demandas/SolicitacaoDemandasForm.aspx?hashCache=" + Session["hashCache"];

            var nomeAmigavel = "Histórico de Solicitação de Demandas";

            var nomeRelatorio = "HistoricoSolicitacaoDemandas";

            RelatoriosHelper.ExecutarThreadSolicitacaoRelatorioRequisicao(requestUrl, saida, nomeRelatorio, nomeAmigavel, quantidadeRegistro);
        }

        protected void rptRelatorio_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem != null)
            {
                Repeater rptCampos = (Repeater)e.Item.FindControl("rptCampos");

                DTORelatorioSolicitacaoDemanda solicitacao = (DTORelatorioSolicitacaoDemanda)e.Item.DataItem;

                int idEtapa;
                int.TryParse(cbxDemanda.SelectedValue, out idEtapa);

                var listaCamposResposta = ObterCamposPadrao(solicitacao.EtapaResposta, idEtapa);

                rptCampos.DataSource = listaCamposResposta;
                rptCampos.DataBind();

                if (rptCabecalho.Items.Count == 0)
                {
                    var camposCabecalho = new List<DTOCabecalho>();

                    camposCabecalho.AddRange(CamposCabecalho.Select(nomeCampo => new DTOCabecalho { Nome = nomeCampo }));

                    camposCabecalho.AddRange(listaCamposResposta.Select(x => new DTOCabecalho { Nome = x.Campo.Nome }));

                    rptCabecalho.DataSource = camposCabecalho;
                    rptCabecalho.DataBind();

                    Cache["dsCamposCabecalho_" + Session["hashCache"]] = camposCabecalho;
                }
            }
        }

        public IEnumerable<CampoResposta> ObterCamposPadrao(EtapaResposta etapa, int idEtapa)
        {
            var camposEtapaResposta = etapa.ListaCampoResposta;
            var listaCamposResposta = new List<CampoResposta>().ToList();

            var cargaHoraria = camposEtapaResposta.FirstOrDefault(x => x.Campo.Nome.Contains("Carga-horária"));
            if (cargaHoraria == null)
            {
                listaCamposResposta.Add(new CampoResposta
                {
                    Campo = new Campo
                    {
                        Nome = "Carga-horária"
                    },
                    Resposta = "-"
                });
            }
            else
            {
                listaCamposResposta.Add(cargaHoraria);
            }

            var dataInicioCapacitacao = camposEtapaResposta.FirstOrDefault(x => x.Campo.Nome == "Data de Início da Capacitação");
            if (dataInicioCapacitacao == null)
            {
                listaCamposResposta.Add(new CampoResposta
                {
                    Campo = new Campo
                    {
                        Nome = "Data de Início da Capacitação"
                    },
                    Resposta = "-"
                });
            }
            else
            {
                listaCamposResposta.Add(dataInicioCapacitacao);
            }

            var dataTerminoCapacitacao = camposEtapaResposta.FirstOrDefault(x => x.Campo.Nome == "Data de Término da Capacitação");
            if (dataTerminoCapacitacao == null)
            {
                listaCamposResposta.Add(new CampoResposta
                {
                    Campo = new Campo
                    {
                        Nome = "Data de Término da Capacitação"
                    },
                    Resposta = "-"
                });
            }
            else
            {
                listaCamposResposta.Add(dataTerminoCapacitacao);
            }

            var local = listaCamposResposta.FirstOrDefault(x => x.Campo.Nome == "Local");
            if (local == null)
            {
                listaCamposResposta.Add(new CampoResposta
                {
                    Campo = new Campo
                    {
                        Nome = "Local"
                    },
                    Resposta = "-"
                });
            }
            else
            {
                listaCamposResposta.Add(local);
            }

            var tituloCapacitacao = camposEtapaResposta.FirstOrDefault(x => x.Campo.Nome == "Título da Capacitação");
            if (tituloCapacitacao == null)
            {
                listaCamposResposta.Add(new CampoResposta
                {
                    Campo = new Campo
                    {
                        Nome = "Título da Capacitação"
                    },
                    Resposta = "-"
                });
            }
            else
            {
                listaCamposResposta.Add(tituloCapacitacao);
            }

            var valorPrevistoInscricao = camposEtapaResposta.FirstOrDefault(x => x.Campo.Nome == "Valor Previsto de Inscrição");
            if (valorPrevistoInscricao == null)
            {
                listaCamposResposta.Add(new CampoResposta
                {
                    Campo = new Campo
                    {
                        Nome = "Valor Previsto de Inscrição"
                    },
                    Resposta = "-"
                });
            }
            else
            {
                listaCamposResposta.Add(valorPrevistoInscricao);
            }

            var valorPrevistoPassagem = camposEtapaResposta.FirstOrDefault(x => x.Campo.Nome == "Valor Previsto de Passagem");
            if (valorPrevistoPassagem == null)
            {
                listaCamposResposta.Add(new CampoResposta
                {
                    Campo = new Campo
                    {
                        Nome = "Valor Previsto de Passagem"
                    },
                    Resposta = "-"
                });
            }
            else
            {
                listaCamposResposta.Add(valorPrevistoPassagem);
            }

            var valorPrevistoDiaria = camposEtapaResposta.FirstOrDefault(x => x.Campo.Nome == "Valor Previsto de Diária");
            if (valorPrevistoDiaria == null)
            {
                listaCamposResposta.Add(new CampoResposta
                {
                    Campo = new Campo
                    {
                        Nome = "Valor Previsto de Diária"
                    },
                    Resposta = "-"
                });
            }
            else
            {
                listaCamposResposta.Add(valorPrevistoDiaria);
            }

            var valorExecutadoInscricao = camposEtapaResposta.FirstOrDefault(x => x.Campo.Nome == "Valor Executado de Inscrição");
            if (valorExecutadoInscricao == null)
            {
                listaCamposResposta.Add(new CampoResposta
                {
                    Campo = new Campo
                    {
                        Nome = "Valor Executado de Inscrição"
                    },
                    Resposta = "-"
                });
            }
            else
            {
                listaCamposResposta.Add(valorExecutadoInscricao);
            }

            var valorExecutadoPassagem = camposEtapaResposta.FirstOrDefault(x => x.Campo.Nome == "Valor Executado de Passagem");
            if (valorExecutadoPassagem == null)
            {
                listaCamposResposta.Add(new CampoResposta
                {
                    Campo = new Campo
                    {
                        Nome = "Valor Executado de Passagem"
                    },
                    Resposta = "-"
                });
            }
            else
            {
                listaCamposResposta.Add(valorExecutadoPassagem);
            }

            var valorExecutadoDiaria = camposEtapaResposta.FirstOrDefault(x => x.Campo.Nome == "Valor Executado de Diária");
            if (valorExecutadoDiaria == null)
            {
                listaCamposResposta.Add(new CampoResposta
                {
                    Campo = new Campo
                    {
                        Nome = "Valor Executado de Diária"
                    },
                    Resposta = "-"
                });
            }
            else
            {
                listaCamposResposta.Add(valorExecutadoDiaria);
            }

            if (idEtapa > 0)
            {
                var camposDaEtapa = etapa.ListaCampoResposta.Where(x => !listaCamposResposta.Contains(x));
                listaCamposResposta.AddRange(camposDaEtapa);
            }

            return listaCamposResposta;
        }

        protected void cbxDemanda_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkListaCamposVisiveis.Items.Clear();
            PreencherCheckBoxPadrao();

            int idEtapa;
            if (int.TryParse(cbxDemanda.SelectedValue, out idEtapa) && idEtapa > 0)
            {
                var campos = new ManterCampo().ObterPorEtapa(idEtapa);
                foreach (var campo in campos)
                {
                    chkListaCamposVisiveis.Items.Add(
                       new ListItem
                       {
                           Text = campo.Nome,
                           Value = campo.Nome
                       }
                   );
                }
            }
        }

        protected void PreencherCheckBoxPadrao()
        {
            chkListaCamposVisiveis.Items.AddRange(CheckBoxPadrao);
        }
    }

    public class DTOCabecalho
    {
        public string Nome { get; set; }
    }
}