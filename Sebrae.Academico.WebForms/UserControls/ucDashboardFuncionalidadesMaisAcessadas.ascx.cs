using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.UserControls
{
    public partial class ucDashboardFuncionalidadesMaisAcessadas : BaseUserControl
    {
       
        protected override IList<enumPerfil> PerfisNecessariosParaAcessarAFuncionalidade
        {
            get
            {
                IList<enumPerfil> perfisNecessariosParaAcessarAFuncionalidade = new List<enumPerfil>();
                perfisNecessariosParaAcessarAFuncionalidade.Add(enumPerfil.Administrador);
                perfisNecessariosParaAcessarAFuncionalidade.Add(enumPerfil.GestorUC);
                return perfisNecessariosParaAcessarAFuncionalidade;
            }
        }

        public void CarregarInformacoesSobreFuncionalidadesMaisAcessadas()
        {
            try
            {
                IList<DTOFuncionalidade> ListaFuncionalidadesMaisAcessadasPorUsuario = new ManterDashBoard().ListarFuncionalidadesMaisAcessadasPorUsuario();

                if (ListaFuncionalidadesMaisAcessadasPorUsuario != null && ListaFuncionalidadesMaisAcessadasPorUsuario.Count > 0)
                {
                    WebFormHelper.PreencherGrid(ListaFuncionalidadesMaisAcessadasPorUsuario, dgvFuncionalidadesMaisAcessadas);
                }
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }



        protected void dgvFuncionalidadesMaisAcessadas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                DTOFuncionalidade dtoFuncionalidade = (DTOFuncionalidade)e.Row.DataItem;

                if (dtoFuncionalidade != null)
                {

                    HyperLink hlnkFuncionalidade = (HyperLink)e.Row.Cells[0].FindControl("hlnkFuncionalidade");

                    if (hlnkFuncionalidade != null)
                    {
                        hlnkFuncionalidade.Text = dtoFuncionalidade.NomeFuncionalidade;
                        string urlAtual = HttpContext.Current.Request.RawUrl;
                        string caminho = HttpContext.Current.Request.Url.ToString().Replace(urlAtual, "");

                        hlnkFuncionalidade.NavigateUrl = string.Format("{0}/{1}", caminho, dtoFuncionalidade.LinkFuncionalidade);

                    }
                }
            }
        }

       

    }
}