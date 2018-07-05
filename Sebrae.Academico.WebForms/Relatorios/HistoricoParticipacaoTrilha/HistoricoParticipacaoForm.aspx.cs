using Sebrae.Academico.BP;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.BP.Services;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Extensions.Others;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BM.Classes;
using System.Web.UI.HtmlControls;

namespace Sebrae.Academico.WebForms.Relatorios.HistoricoParticipacaoTrilha
{
    public partial class HistoricoParticipacaoForm : System.Web.UI.Page
    {
        private ManterMatriculaTrilha manterMatrilhaTrilha = null;
        private CheckBoxList chkListaCamposVisiveis = null;
        //private Dictionary<Type, Type> filtros = null;
        private DTOFiltrosHistoricoParticipacaoTrilha filtros = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            manterMatrilhaTrilha = new ManterMatriculaTrilha();

            var listaUsuarioTrilha = Cache["dsRelatorioHistoricoParticipacao"] as IList<UsuarioTrilha>;
            chkListaCamposVisiveis = Cache["dsCamposRelatorioHistoricoParticipacao"] as CheckBoxList;
            filtros = Cache["dsFiltrosRelatorioHistoricoParticipacao"] as DTOFiltrosHistoricoParticipacaoTrilha;            

            if (listaUsuarioTrilha != null)
            {
                rptUsuariosTrilha.DataSource = listaUsuarioTrilha;
                rptUsuariosTrilha.DataBind();
            }

        }

        protected void rptUsuariosTrilha_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var rptPontosSebraeUsuario = (Repeater)e.Item.FindControl("rptPontosSebraeUsuario");
            var rptsolucaoesDaTrilhaOnline = (Repeater)e.Item.FindControl("rptsolucaoesDaTrilhaOnline");
            var rptsolucaoesDoTrilheiro = (Repeater)e.Item.FindControl("rptsolucaoesDoTrilheiro");
            var rptsolucaoesDesempenhoGeral = (Repeater)e.Item.FindControl("rptSolucaoesDesempenhoGeral");

            // Inception de repeaters.
            // Obter as questões por aqui.
            var usuarioTrilha = (UsuarioTrilha)e.Item.DataItem;

            var dic = new Dictionary<string, object>();
            dic.Add("@ID_Usuariario_Trilha", usuarioTrilha.ID);

            var solucaoesDaTrilha = new BMItemTrilha().ExecutarProcedure<DTOSolucoesDaTrilha>("SP_solucoes_da_trilha", dic);
            var solucaoesDaTrilhaOnline = new BMItemTrilha().ExecutarProcedure<DTOCursosOnlineUCSebrae>("SP_cursos_online_ucsebrae", dic);
            var solucaoesDoTrilheiro = new BMItemTrilha().ExecutarProcedure<DTOSolucoesTrilheiro>("SP_solucoes_do_trilheiro", dic);
            var solucaoesDesempenhoGeral = new BMItemTrilha().ExecutarProcedure<DTOSolucaoesDesempenhoGeral>("SP_solucoes_do_desempenho_geral", dic);


            /* -- Solução da trilha  --*/
            rptPontosSebraeUsuario.DataSource = solucaoesDaTrilha;
            rptPontosSebraeUsuario.DataBind();

            var rptPontosSebraeUsuarioObjetivos = (HtmlTableCell)e.Item.FindControl("rptPontosSebraeUsuarioObjetivos");
            rptPontosSebraeUsuarioObjetivos.InnerText = solucaoesDaTrilha.Count().ToString();

            var rptPontosSebraeUsuarioTotalHoras = (HtmlTableCell)e.Item.FindControl("rptPontosSebraeUsuarioTotalHoras");
            rptPontosSebraeUsuarioTotalHoras.InnerText = solucaoesDaTrilha.Sum(x => x.CargaHoraria).ToString() + "h";

            var rptPontosSebraeUsuarioMoedas = (HtmlTableCell)e.Item.FindControl("rptPontosSebraeUsuarioMoedas");
            rptPontosSebraeUsuarioMoedas.InnerText = solucaoesDaTrilha.Sum(x => x.Moedas).ToString();
            /* -- Solução da trilha  --*/


            /* -- Solução da trilha online --*/
            rptsolucaoesDaTrilhaOnline.DataSource = solucaoesDaTrilhaOnline;
            rptsolucaoesDaTrilhaOnline.DataBind();


            var rptsolucaoesDaTrilhaOnlineObjetivo = (HtmlTableCell)e.Item.FindControl("rptsolucaoesDaTrilhaOnlineObjetivo");
            rptsolucaoesDaTrilhaOnlineObjetivo.InnerText = solucaoesDaTrilhaOnline.Count().ToString();

            var rptsolucaoesDaTrilhaOnlineMoedasProvaFinal = (HtmlTableCell)e.Item.FindControl("rptsolucaoesDaTrilhaOnlineMoedasProvaFinal");
            rptsolucaoesDaTrilhaOnlineMoedasProvaFinal.InnerText = solucaoesDaTrilhaOnline.Sum(x => x.Moedas).ToString();

            var rptsolucaoesDaTrilhaOnlineTotal = (HtmlTableCell)e.Item.FindControl("rptsolucaoesDaTrilhaOnlineTotal");
            rptsolucaoesDaTrilhaOnlineTotal.InnerText = solucaoesDaTrilhaOnline.Sum(x => x.CargaHoraria).ToString();
            /* -- Solução da trilha online --*/


            /* -- Solução do trilheiro --*/
            rptsolucaoesDoTrilheiro.DataSource = solucaoesDoTrilheiro;
            rptsolucaoesDoTrilheiro.DataBind();
            /* -- Solução do trilheiro --*/

            /* --Desempenho geral-- */
            rptsolucaoesDesempenhoGeral.DataSource = solucaoesDesempenhoGeral;
            rptsolucaoesDesempenhoGeral.DataBind();
        }

        public virtual void rptPontosSebraeUsuario_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
        }
        public virtual void rptsolucaoesDaTrilhaOnline_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
        }
        public virtual void rptsolucaoesDoTrilheiro_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
        }
        public virtual void rptsolucaoesDesempenhoGeral_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
        }
    }

}