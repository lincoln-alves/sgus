using System.Collections.Generic;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.BP.DTO;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioAcessosPaginas : BusinessProcessBaseRelatorio
    {

        protected override enumRelatorio Relatorio
        {
            get { return enumRelatorio.AcessoDeUsuarios; }
        }

        public IList<DTORelatorioAcessosPaginas> ConsultarRelatorioAcessosPaginas()
        {
            return (IList<DTORelatorioAcessosPaginas>)System.Web.HttpContext.Current.Session["dsRelatorio"];
        }

    }
}
