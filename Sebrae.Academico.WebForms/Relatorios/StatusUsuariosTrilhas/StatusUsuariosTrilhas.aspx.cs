using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.BP.Relatorios;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.BP;
using System.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.BP.DTO.Dominio;

namespace Sebrae.Academico.WebForms.Relatorios.StatusUsuariosTrilhas
{
    public partial class StatusUsuariosTrilhas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            using (RelatorioStatusUsuariosTrilhas relStat = new RelatorioStatusUsuariosTrilhas())
            {
                WebFormHelper.PreencherLista(relStat.ObterTrilhasTodas(), cbxTrilha, true);
                WebFormHelper.PreencherLista(relStat.ObterUFTodas(), cbxUF, true);
                WebFormHelper.PreencherLista(relStat.ObterNivelOcupacionalTodas(), cbxNivelOcupacional, true);

                ucListBoxStatus.PreencherItens(relStat.ObterStatusMatriculaTrilhas().Where(x => x.ID != 7).ToList(), "ID", "Nome");

                var niveis = relStat.ObterNivelTrilha().Select(x => new
                {
                    ID = x.ID,
                    Nome = $"{x.Trilha.Nome} - {x.Nome}"
                }).ToList();

                WebFormHelper.PreencherLista(niveis, cbxNivelTrilha, true);
            }
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            using (var relStat = new RelatorioStatusUsuariosTrilhas())
            {
                DateTime? dataInicio = null;
                DateTime? dataDeConclusao = null;
                DateTime? dataLimite = null;

                DateTime dataTmp;

                if (!string.IsNullOrWhiteSpace(cbxDataInicio.Text))
                {
                    if (DateTime.TryParse(cbxDataInicio.Text, out dataTmp))
                    {
                        dataInicio = dataTmp;
                    }
                    else
                    {
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Data inicial inválida");
                        return;
                    }
                }

                if (!string.IsNullOrWhiteSpace(cbxDataDeConclusao.Text))
                {
                    if (DateTime.TryParse(cbxDataDeConclusao.Text, out dataTmp))
                    {
                        dataDeConclusao = dataTmp;
                    }
                    else
                    {
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Data final inválida");
                        return;
                    }
                }

                if (dataDeConclusao.HasValue && dataInicio.HasValue && dataDeConclusao < dataInicio)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "A data final deve ser maior que a inicial");
                    return;
                }

                if (!string.IsNullOrWhiteSpace(cbxDataLimite.Text))
                {
                    if (DateTime.TryParse(cbxDataLimite.Text, out dataTmp))
                    {
                        dataLimite = dataTmp;
                    }
                    else
                    {
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Data limite inválida");
                        return;
                    }
                }

                var trilhaId = string.IsNullOrWhiteSpace(cbxTrilha.SelectedValue)
                    ? null
                    : (int?)int.Parse(cbxTrilha.SelectedValue);

                var nivelTrilha = string.IsNullOrWhiteSpace(cbxNivelTrilha.SelectedValue)
                    ? null
                    : (int?)int.Parse(cbxNivelTrilha.SelectedValue);

                var nivelOcupacionalId = string.IsNullOrWhiteSpace(cbxNivelOcupacional.SelectedValue)
                    ? null
                    : (int?)int.Parse(cbxNivelOcupacional.SelectedValue);

                var ufId = string.IsNullOrWhiteSpace(cbxUF.SelectedValue)
                    ? null
                    : (int?)int.Parse(cbxUF.SelectedValue);

                var statusSelecionados = ucListBoxStatus.RecuperarIdsSelecionados<int>();

                var statusUsuariosTrilhaList = relStat.ObterStatusTrilhas(statusSelecionados, trilhaId, cbxNomeLupa.SelectedUser?.ID, nivelTrilha, nivelOcupacionalId,
                   ufId, dataInicio, dataDeConclusao, dataLimite);

                Session.Add("dsRelatorio", statusUsuariosTrilhaList);

                dgRelatorio.DataSource = statusUsuariosTrilhaList;

                if (statusUsuariosTrilhaList != null && statusUsuariosTrilhaList.Count > 0)
                {
                    // Converter os resultados em dados totalizadores.
                    var totalizadores = new List<DTOTotalizador>
                    {
                        TotalizadorUtil.GetTotalizador(statusUsuariosTrilhaList,
                            "Total da quantidade de alunos por trilha", "Trilha",
                            enumTotalizacaoRelatorio.Contar),
                        TotalizadorUtil.GetTotalizador(statusUsuariosTrilhaList,
                            "Total da quantidade de alunos por nível de trilha", "NivelTrilha",
                            enumTotalizacaoRelatorio.Contar),
                        TotalizadorUtil.GetTotalizador(statusUsuariosTrilhaList,
                            "Total da quantidade por status (listando os status de matrícula e os números totais de cada status)",
                            "StatusMatricula",
                            enumTotalizacaoRelatorio.Contar),
                        TotalizadorUtil.GetTotalizador(statusUsuariosTrilhaList,
                            "Total da quantidade de alunos UF (listando as UF e os números totais de cada UF)", "UF",
                            enumTotalizacaoRelatorio.Contar)
                    };

                    ucTotalizadorRelatorio.PreencherTabela(totalizadores);

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

            var totalizadores = Session["dsTotalizador"];

            WebFormHelper.GerarRelatorio("StatusUsuariosTrilhas.rptStatusUsuariosTrilhas.rdlc", dt,
                ucFormatoSaidaRelatorio.TipoSaida, chkListaCamposVisiveis.Items, totalizadores);
        }

        protected void dgRelatorio_Sorting(object sender, GridViewSortEventArgs e)
        {
            WebFormHelper.OrdenarListaGrid((IList<DTORelatorioStatusUsuariosTrilhas>)Session["dsRelatorio"],
                dgRelatorio, e.SortExpression, e.SortDirection, "dsRelatorio");
        }

        protected void dgRelatorio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            WebFormHelper.PaginarGrid((IList<DTORelatorioStatusUsuariosTrilhas>)Session["dsRelatorio"], dgRelatorio,
                e.NewPageIndex);
        }

        protected void cbxTrilha_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<DTOTrilhaNivel> niveis;

            var idTrilha = 0;
            if (int.TryParse(cbxTrilha.SelectedValue, out idTrilha))
            {
                niveis = new RelatorioStatusUsuariosTrilhas().ObterNivelTrilha().Where(x => x.Trilha.ID == idTrilha)
                    .Select(x => new DTOTrilhaNivel
                    {
                        ID = x.ID,
                        Nome = x.Nome
                    }).ToList();
            }
            else
            {
                niveis = new RelatorioStatusUsuariosTrilhas().ObterNivelTrilha().Select(x => new DTOTrilhaNivel
                {
                    ID = x.ID,
                    Nome = $"{x.Trilha.Nome} - {x.Nome}"
                }).ToList();
            }

            WebFormHelper.PreencherLista(niveis, cbxNivelTrilha, true);

        }
    }
}