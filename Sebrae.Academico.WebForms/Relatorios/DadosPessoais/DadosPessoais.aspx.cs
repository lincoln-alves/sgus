using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.BP.Relatorios;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.Relatorios.DadosPessoais
{
    public partial class DadosPessoais : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;            

            ListBoxesUF.PreencherItens(new ManterUf().ObterTodosIQueryable().Select(x => new { x.ID, x.Nome }), "ID", "Nome");
            ListBoxesNivelOcupacional.PreencherItens(new ManterNivelOcupacional().ObterTodosNivelOcupacional().Select(x => new { x.ID, x.Nome }), "ID", "Nome");            
            var relUsuarioCadastrado = new RelatorioUsuarioCadastrado();
            ListBoxesPerfil.PreencherItens(relUsuarioCadastrado.ObterPerfilTodos(), "ID", "Nome");
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            using (var relDadosPessoais = new RelatorioDadosPessoais())
            {
                
                var ufIds = ListBoxesUF.RecuperarIdsSelecionados<int>().ToList();
                var NivelOcupacionalIds = ListBoxesNivelOcupacional.RecuperarIdsSelecionados<int>().ToList();
                var perfilIds = ListBoxesPerfil.RecuperarIdsSelecionados<int>().ToList();                

                var pNome = string.IsNullOrWhiteSpace(txtNome.Text) ? null : txtNome.Text.Trim();

                var pCpf = string.IsNullOrWhiteSpace(txtCPF.Text) ? null : txtCPF.Text.Replace("-", "").Replace(".", "");          

                var lstGrid = relDadosPessoais.ConsultarDadosPessoais(pNome, pCpf, NivelOcupacionalIds, ufIds, perfilIds);

                DateTime dtIni, dtFim;
                if (DateTime.TryParse(txtDataInicio.Text, out dtIni) && DateTime.TryParse(txtDataFinal.Text, out dtFim))
                {
                    lstGrid = lstGrid.Where(x => (x.DT_Insercao.HasValue && x.DT_Insercao >= dtIni) && (x.DT_Insercao.HasValue && x.DT_Insercao <= dtFim)).ToList();
                }

                Session.Add("dsRelatorio", lstGrid);               


                dgRelatorio.DataSource = lstGrid;
                dgRelatorio.DataBind();
                WebFormHelper.ValidarVisibilidadeCamposGrid(dgRelatorio, chkListaCamposVisiveis.Items);

                if (lstGrid != null && lstGrid.Count > 0){
                    componenteGeracaoRelatorio.Visible = true;
                    ucFormatoSaidaRelatorio.Visible = true;
                    btnPesquisar.CssClass = "btn btn-default mostrarload";
                    Filtros.CssClass = "panel-collapse collapse";
                }else{
                    componenteGeracaoRelatorio.Visible = false;
                    ucFormatoSaidaRelatorio.Visible = false;
                }
            }
        }

        protected void btnGerarRelatorio_Click(object sender, EventArgs e)
        {

            if (Session["dsRelatorio"] == null)
                btnPesquisar_Click(null, null);
            
            var dt = Session["dsRelatorio"];
            WebFormHelper.GerarRelatorio("DadosPessoais.rptDadosPessoais.rdlc", dt, ucFormatoSaidaRelatorio.TipoSaida,
                chkListaCamposVisiveis.Items);
        }

        protected void dgRelatorio_Sorting(object sender, GridViewSortEventArgs e)
        {


            WebFormHelper.OrdenarListaGrid((IList<DTORelatorioDadosPessoais>) Session["dsRelatorio"],
                dgRelatorio,
                e.SortExpression,
                e.SortDirection,
                "dsRelatorio");

        }

        protected void dgRelatorio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            WebFormHelper.PaginarGrid((IList<DTORelatorioDadosPessoais>) Session["dsRelatorio"], dgRelatorio,
                e.NewPageIndex);

        }
    }
}