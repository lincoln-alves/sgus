using System;
using System.Web.UI;
using Sebrae.Academico.BP;

namespace Sebrae.Academico.WebForms.UserControls
{
    public partial class ucVisualizaCertificadoTemplate : UserControl
    {
        public string Src
        {
            set { ifrmMostraRelat.Src = value; }
            get { return ifrmMostraRelat.Src; }
        }

        public void Fecha() {
            pnlModal.Visible = false;
        }

        public void Abre(int idCertificado)
        {
            // Verificar quantas páginas tem no certificado e setar o height do iframe.
            var certificado = new ManterCertificadoTemplate().ObterCertificadoTemplatePorID(idCertificado);

            var frameHeight = 930;

            if (certificado != null && !string.IsNullOrWhiteSpace(certificado.TextoCertificado2))
                frameHeight = frameHeight * 2;

            ifrmMostraRelat.Attributes.Add("height", frameHeight.ToString());

            pnlModal.Visible = true;
        }

        protected void OcultarModal_Click(object sender, EventArgs e)
        {
            Fecha();
        }
    }
}