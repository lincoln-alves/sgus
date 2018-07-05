using System;
using System.Configuration;
using System.Web;
using Sebrae.Academico.BM.AutoMapper;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.WebForms.Bundles;
using System.Web.Optimization;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.DTO.Relatorios;
using System.Diagnostics;
using System.Web.Routing;
using Microsoft.AspNet.FriendlyUrls;

namespace Sebrae.Academico.WebForms
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            //Code that runs on application startup
            if (System.Diagnostics.Debugger.IsAttached)
            {
                log4net.Config.XmlConfigurator.Configure();
            }

            AutoMapperConfig.RegisterMappings();
            BP.AutoMapper.AutoMapperConfig.RegisterMappings();
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Rotas
            RegisterRoutes(RouteTable.Routes);
        }

        /// <summary>
        /// Método utilizado para registrar rotas amigáveis
        /// </summary>
        /// <param name="routes"></param>
        private void RegisterRoutes(RouteCollection routes)
        {
            routes.EnableFriendlyUrls();

            // Media Server
            //routes.Add(new HttpHandlerRouteHandler<MediaServer>("ms/{*id}"));
            routes.Add("MediaServer", new Route("ms/{*Id}", new HttpHandlerRouteHandler<MediaServer>()));


            //routes.MapPageRoute("MediaServer", "ms/{identificador}", "~/MediaServer.ashx");
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            //TODO: Usuário colocado para testes. retirar assim que a tela de login for ativada.
            //Session.Add("usuarioSGUS", new ManterUsuario().ObterUsuarioPorID(1));

            // Só roda quando o código é compilado como debug
            #if DEBUG
            var debugLocal = ConfigurationManager.AppSettings["debugLocal"];
            var idUsuarioLocal = ConfigurationManager.AppSettings["localUser"];

            if (!string.IsNullOrEmpty(debugLocal) && !string.IsNullOrEmpty(idUsuarioLocal) && debugLocal == "S")
            {
                int id;
                if (int.TryParse(idUsuarioLocal, out id))
                {
                    var bmUsuarios = new BMUsuario();
                    var usuario = bmUsuarios.ObterPorId(id);
                    new BMUsuario().SetarUsuarioLogado(usuario);
                }
            }
            #endif
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var debugLocal = ConfigurationManager.AppSettings["debugLocal"];

            // Só vai para tela de erro caso não esteja em modo de debug.
            if (string.IsNullOrEmpty(debugLocal) || debugLocal != "S")
            {
                Application["LastError"] = Server.GetLastError();
                Application["ErrorPage"] = Request.Url.AbsolutePath;

                // Clear the error from the server.
                Server.ClearError();

                Response.Redirect("~/Erro.aspx", false);
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}