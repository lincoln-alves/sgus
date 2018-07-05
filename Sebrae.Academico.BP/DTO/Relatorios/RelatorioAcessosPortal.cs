using Sebrae.Academico.BP.DTO;
using Sebrae.Academico.BP.Relatorios;
using Sebrae.Academico.Dominio.Enumeracao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioAcessosPortal : BusinessProcessBaseRelatorio
    {

        protected override enumRelatorio Relatorio
        {
            get { return enumRelatorio.AcessoDeUsuarios; }
        }

        public IList<DTORelatorioAcessosPortal> ConsultarRelatorioAcessosPortal()
        {
            return (IList<DTORelatorioAcessosPortal>)System.Web.HttpContext.Current.Session["dsRelatorio"];
        }
    }
}
