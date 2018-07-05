using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sebrae.Academico.WebForms.UserControls
{
    public partial class ucTutorias : System.Web.UI.UserControl
    {
        public void PreencherCampos(Usuario usuario)
        {
            if (usuario != null)
            {
                using (var manterTurma = new ManterTurmaProfessor())
                {
                    var cursos = manterTurma.ObterTurmaProfessorPorProfessor(usuario.ID);

                    txtSemTurmas.Visible = cursos.Count() > 0 ? false : true;

                    dgvTutorias.DataSource = cursos;
                    dgvTutorias.DataBind();
                }
            }
        }
    }
}