using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP.Services.ExternosWebService;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.WebForms.Cadastros.Teste
{
    public partial class EnviarEmailMatricula : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var mo = new Sebrae.Academico.BP.ManterOferta();
            var matriculaOferta = new MatriculaOferta();
            //matriculaOferta.NomeSolucaoEducacional = txtNomeSolucaoEducacional.Text;

            string emailDoDestinatario = string.Empty;

            Template templateMensagemEmailOfertaExclusiva = TemplateUtil.ObterInformacoes(enumTemplate.MatriculaOfertaExclusivo);
            string assuntoDoEmail = templateMensagemEmailOfertaExclusiva.Assunto;

            string corpoEmail = templateMensagemEmailOfertaExclusiva.TextoTemplate;

            emailDoDestinatario = "matriculaOferta.Usuario.Email";

            assuntoDoEmail = assuntoDoEmail.Replace("#NOME_CURSO", "matriculaOferta.NomeSolucaoEducacional");
            corpoEmail = mo.CorpoEmail(corpoEmail, matriculaOferta);

            EmailUtil.Instancia.EnviarEmail(emailDoDestinatario.Trim(), assuntoDoEmail, corpoEmail);
        }
    }
}