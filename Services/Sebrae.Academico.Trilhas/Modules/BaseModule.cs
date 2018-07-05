using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;

using NewRelicAgent = NewRelic.Api.Agent.NewRelic;
using System.Threading;
using System.Threading.Tasks;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BM.Classes.PortalUC;
using Sebrae.Academico.Dominio.Classes.Views;
using JWT;
using Sebrae.Academico.BP;

namespace Sebrae.Academico.Trilhas.Modules
{
    public abstract class BaseModule : NancyModule
    {
        /// <summary>
        /// Módulo base para os outros, caso seja necessário customizar algo compartilhado.
        /// </summary>
        public BaseModule()
        {
            TrackNewRelic();
            After += SalvarLog;
        }

        /// <summary>
        /// Módulo base para os outros, caso seja necessário customizar algo compartilhado.
        /// </summary>
        /// <param name="path"></param>
        public BaseModule(string path) : base(path)
        {
            TrackNewRelic();
            After += SalvarLog;
        }

        /// <summary>
        /// Para que seja possível acompanhar as requisições feitas de forma separada 
        /// via new relic esse método deve ser chamado a cada requisição
        /// dessa forma todos os módulos devem herdar essa classe
        /// </summary>
        public void TrackNewRelic()
        {
            Before += ctx =>
            {
                var routeDescription = ctx.ResolvedRoute.Description;
                NewRelicAgent.SetTransactionName("Custom", $"Trilhas {routeDescription.Method} {routeDescription.Path}");
               
                return null;
            };
           
        }            

        public static void SalvarLog(NancyContext ctx)
        {
           
            if (ctx.Request.Method == "OPTIONS" || string.IsNullOrEmpty(ctx.Request.Headers.Authorization))
            {
                return;
            }

            ThreadStart LogThreadStart = CriarStartThreadDoLog(ctx);

            var logThread = new Thread(LogThreadStart) { IsBackground = true };

            try
            {
                logThread.Start();
            }
            catch (Exception)
            {
                logThread.Abort();
            }
        }

        private static ThreadStart CriarStartThreadDoLog(NancyContext ctx)
        {
            return () =>
            {
                var jwtToken = ctx.Request.Headers.Authorization;
                var usuario = JsonWebToken.DecodeToObject(jwtToken, "", false) as IDictionary<string, object>;

                var url = ctx.Request.Path;
                var method = ctx.Request.Method;
                var iP = ctx.Request.UserHostAddress;

                var acao = PegarAcaoPeloMethodEUrl(method, url);

                var logPortal = new LogAcoesPortal
                {
                    ID_Usuario = (int)usuario["id"],
                    Url = url,
                    Acao = acao,
                    IP = iP,
                    Datacesso = DateTime.Now
                };

                try { 
                    new BMLogAcoesPortal().Salvar(logPortal);
                }
                catch
                {
                    
                }
            };
        }

        private static string PegarAcaoPeloMethodEUrl(string method, string url)
        {

            if (method == "POST")
            {
                if (url.Contains("matricula"))
                {
                    return "Inscrição";
                }

                return "Envio";
            }

            if (method == "DELETE")
            {
                return "Exclusão";
            }

            if (url.Contains("vivencia/finalizar"))
            {
                return "Envio";
            }

            if (url.Contains("Certificado"))
            {
                return "Emissão";
            }

            return "Visualização";
        }
    }
}