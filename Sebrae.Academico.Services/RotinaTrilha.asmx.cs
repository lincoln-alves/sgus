using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Sebrae.Academico.BP;

namespace Sebrae.Academico.Services
{
    /// <summary>
    /// Summary description for RotinaTrilha
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class RotinaTrilha : System.Web.Services.WebService
    {

        [WebMethod]
        public void NotificarMonitores()
        {
            try
            {
                ManterUsuario manter = new ManterUsuario();
                manter.NotificarMonitoresAtraso();
            }
            catch
            {
                return;
            }
        }
    }
}
