using System;
using System.Linq;
using System.Web.UI;
using Sebrae.Academico.BP;
using classes = Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.WebForms.Cadastros.Informe
{
    public partial class VisualizarInforme : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int informeId;
            classes.Informe informe;

            var manterInforme = new ManterInforme();

            if (Request["Id"] != null && int.TryParse(Request["Id"], out informeId) &&
                (informe = manterInforme.ObterPorId(informeId)) != null)
            {
                // Criar HTML com a turma informada.

                var template = manterInforme.ObterTemplateHTML(informe);

                // Substituir imagens de anexo do e-mail pelo caminho relativo.
                template = template.Replace("cid:Header", "../../img/newsletter/header.jpg");
                template = template.Replace("cid:Footer", "../../img/newsletter/footer.jpg");
                template = template.Replace("cid:RightArrow", "../../img/newsletter/right-arrow.jpg");
                template = template.Replace("cid:PageFlip", "../../img/newsletter/page-flip.jpg");

                ltrMainTemplate.Text = template;
            }
        }
    }
}