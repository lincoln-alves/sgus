using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Sebrae.Academico.Dominio.DTO;

namespace Sebrae.Academico.WebForms.UserControls
{
    public partial class ucTotalizadorRelatorio : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Limpar o totalizador antes de carregar a tela.
            if(!IsPostBack)
                LimparTotalizadores();
        }

        public string NomeTotalizador { get; set; }

        /// <summary>
        /// Preenche o totalizador dinamicamente, de acordo com os dados fornecidos.
        /// </summary>
        /// <param name="totalizadores"></param>
        public void PreencherTabela(List<DTOTotalizador> totalizadores)
        {
            LimparTotalizadores();

            Session["dsTotalizador"] = totalizadores;

            rptTotalizador.DataSource = totalizadores;
            rptTotalizador.DataBind();

            grupototalizador.Visible = totalizadores.Any();
        }

        public void LimparTotalizadores()
        {
            Session["dsTotalizador"] = null;

            grupototalizador.Visible = false;
        }
    }
}