using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms
{
    public partial class ucDashBoardRelatoriosMaisAcessados : BaseUserControl
    {

        protected override IList<enumPerfil> PerfisNecessariosParaAcessarAFuncionalidade
        {
            get
            {
                IList<enumPerfil> perfisNecessariosParaAcessarAFuncionalidade = new List<enumPerfil>();
                perfisNecessariosParaAcessarAFuncionalidade.Add(enumPerfil.Administrador);
                perfisNecessariosParaAcessarAFuncionalidade.Add(enumPerfil.Colaborador);
                perfisNecessariosParaAcessarAFuncionalidade.Add(enumPerfil.GestorUC);
                return perfisNecessariosParaAcessarAFuncionalidade;
            }
        }
      
        public void CarregarInformacoesSobreRelatoriosMaisAcessados()
        {
            try
            {
                IList<DTOGeracaoRelatorio> ListaRelatoriosMaisAcessados = new ManterDashBoard().ListarRelatoriosMaisAcessadosPorUsuario();

                if (ListaRelatoriosMaisAcessados != null && ListaRelatoriosMaisAcessados.Count > 0)
                {
                    WebFormHelper.PreencherGrid(ListaRelatoriosMaisAcessados, dgvInformacoesDeRelatoriosMaisAcessados);
                }

            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        protected void dgvInformacoesDeRelatoriosMaisAcessados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                DTOGeracaoRelatorio dtoGeracaoRelatorio = (DTOGeracaoRelatorio)e.Row.DataItem;

                if (dtoGeracaoRelatorio != null)
                {

                    HyperLink hlnkRelatorio = (HyperLink)e.Row.Cells[0].FindControl("hlnkRelatorio");

                    if (hlnkRelatorio != null)
                    {
                        hlnkRelatorio.Text = dtoGeracaoRelatorio.NomeRelatorio;
                        string urlAtual = HttpContext.Current.Request.RawUrl;
                        string caminho = HttpContext.Current.Request.Url.ToString().Replace(urlAtual, "");

                        hlnkRelatorio.NavigateUrl = string.Format("{0}/{1}", caminho, dtoGeracaoRelatorio.LinkRelatorio);
                    }

                }
            }
        }

    }
}