using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace Sebrae.Academico.WebForms.Relatorios.Demandas
{
    public partial class SolicitacaoDemandasForm : System.Web.UI.Page
    {
        public bool ExibirCampo(object obj)
        {
            var campo = (string)obj;

            var hashCache = "";
            if (Request["hashCache"] != null)
            {
                hashCache = Request["hashCache"];
            }

            ListItemCollection chkListaCamposVisiveis = Cache["dsCamposVisiveis_" + hashCache] as ListItemCollection;

            foreach (ListItem item in chkListaCamposVisiveis)
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

        protected void Page_Load(object sender, EventArgs e)
        {
            var hashCache = "";
            if (Request["hashCache"] != null)
            {
                hashCache = Request["hashCache"];
            }
            var resultado = Cache["dsRelatorio_" + hashCache] as List<DTORelatorioSolicitacaoDemanda>;
            var camposCabecalho = Cache["dsCamposCabecalho_" + hashCache] as List<DTOCabecalho>;
            var camposVisiveis = Cache["dsCamposVisiveis_" + hashCache];

            var quantidadeRegistro = resultado == null ? 0 : resultado.Count();

            rptRelatorio.DataSource = resultado;
            rptRelatorio.DataBind();

            rptCabecalho.DataSource = camposCabecalho;
            rptCabecalho.DataBind();
        }

        protected void rptRelatorio_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem != null)
            {
                Repeater rptCampos = (Repeater)e.Item.FindControl("rptCampos");

                DTORelatorioSolicitacaoDemanda solicitacao = (DTORelatorioSolicitacaoDemanda)e.Item.DataItem;

                var sidEtapa = Cache["idEtapa"];
                int idEtapa;

                int.TryParse(sidEtapa.ToString(), out idEtapa);

                var listaCamposResposta = ObterCamposPadrao(solicitacao.EtapaResposta, idEtapa);

                rptCampos.DataSource = listaCamposResposta;
                rptCampos.DataBind();
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
    }
}