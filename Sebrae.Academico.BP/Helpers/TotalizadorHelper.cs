using System.Collections.Generic;
using System.Web;
using Sebrae.Academico.Dominio.DTO;

namespace Sebrae.Academico.BP.Helpers
{
    public class TotalizadorHelper
    {
        public List<DTOTotalizador> ObterUltimoTotalizador()
        {
            if (HttpContext.Current.Session["dsTotalizador"] != null)
            {
                return HttpContext.Current.Session["dsTotalizador"] as List<DTOTotalizador>;
            }
            return new List<DTOTotalizador>();
        }
    }
}
