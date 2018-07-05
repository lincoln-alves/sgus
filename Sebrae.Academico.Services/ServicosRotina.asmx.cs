using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.BP.Services.SgusWebService;
using Sebrae.Academico.InfraEstrutura.Seguranca.Autenticacao;
using Sebrae.Academico.BP.Services;
using Sebrae.Academico.BP.DTO.Services;
using Sebrae.Academico.BP.Services.Mensageria;
using Sebrae.Academico.Services;
using Sebrae.Academico.Dominio.Classes;
using System.Collections;
using Sebrae.Academico.Util.Classes;


namespace Sebrae.Academico.Trihas.Services
{
    /// <summary>
    /// Summary description for ServicoMensageria
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ServicosRotina : System.Web.Services.WebService
    {

        [WebMethod]
        public void EnviarMensagensatraso()
        {

            new Mensageria().EnviarMensagensatraso();

        }

        [WebMethod]
        public void SincronizarSolucoesEducacionaisPortal()
        {
            //TODO: Pegar todas as SE com IDNode > 0 e Notificar o Drupal

        }

                
        [WebMethod]
        public void RegistrarAbandono()
        {
            new Mensageria().RegistrarAbandono();
        }


        #region "WebMethods referentes a Importação dos dados do usuário"

        private ManterUsuario manterUsuario = null;

        [WebMethod]
        public void SincronizarALI(string cpf)
        {
            this.manterUsuario = new ManterUsuario(true);
            this.manterUsuario.ImportarALI(cpf);
        }

        [WebMethod]
        public void SincronizarADL(string cpf)
        {
            this.manterUsuario = new ManterUsuario(true);
            this.manterUsuario.ImportarADL(cpf);
        }

        [WebMethod]
        public void SincronizarCredenciado(string cpf)
        {
            this.manterUsuario = new ManterUsuario(true);
            this.manterUsuario.ImportarCredenciado(cpf);
        }

        [WebMethod]
        public void SincronizarColaborador(string cpf)
        {
            this.manterUsuario = new ManterUsuario(true);
            this.manterUsuario.ImportarColaborador(cpf);
        }

        #endregion

        #region "Chamada ao WebService do Portal Saber"

        //WebService do Portal Saber
        [WebMethod]
        public void ObterInformacoesDoPortalSaber()
        { 
            
        }


        [WebMethod]
        public void TmpExportarUsuariosMoodle(int idOferta)
        {
            Sebrae.Academico.BP.ManterSolucaoEducacional manterSE = new Sebrae.Academico.BP.ManterSolucaoEducacional();
            manterSE.EnviarDadosOferta(idOferta);
        }

        #endregion

    }
}
