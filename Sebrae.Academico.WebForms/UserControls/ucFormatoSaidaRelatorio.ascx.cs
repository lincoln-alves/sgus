using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.WebForms.UserControls
{
    public partial class ucFormatoSaidaRelatorio : System.Web.UI.UserControl
    {

        public enumTipoSaidaRelatorio TipoSaida
        {
            get
            {
                switch (rblTipoSaida.SelectedValue)
                {
                    case "PDF": return enumTipoSaidaRelatorio.PDF;
                    case "WORD": return enumTipoSaidaRelatorio.WORD;
                    case "EXCEL": return enumTipoSaidaRelatorio.EXCEL;
                    default: return enumTipoSaidaRelatorio.PDF;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

    }
}