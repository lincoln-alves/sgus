using System;
using Nancy;
using Nancy.Responses;
using Sebrae.Academico.BP.Auth;
using Sebrae.Academico.BP.DTO.Services.Trilhas;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Util.Classes;

using NewRelicAgent = NewRelic.Api.Agent.NewRelic;

namespace Sebrae.Academico.Trilhas
{
    public static class Errors
    {
        public static Response Handle(NancyContext ctx, Exception exception)
        {
            var returnCode = HttpStatusCode.BadRequest;

            var respModel = new DtoResponse(exception.Message)
            {
                Data = null,
                StatusCode = 0,
                Stack = exception.StackTrace
            };

            if (exception.GetType() == typeof(ResponseException))
            {
                var responseEx = ((ResponseException)exception);
                returnCode = HttpStatusCode.OK;                
                respModel.StatusCode = (int) responseEx.StatusCode;

                // Por padrão pega a mensagem via StatusCode Description, do contrário usa a customizada
                if(exception.Message=="")
                    respModel.Message = responseEx.StatusCode.GetDescription();
            }

            if (exception.GetType() == typeof(AcademicoException))
            {
                returnCode = HttpStatusCode.OK;
                respModel.StatusCode = 100;
            }

            var resp = new JsonResponse<DtoResponse>(respModel, new DefaultJsonSerializer())
            {
                StatusCode = returnCode
            };

            // Todas as rotas que retornam - Tentando mapear todas as transações que não possuem status de erro
            if (returnCode == HttpStatusCode.OK)
            {
                var routeDescription = ctx.ResolvedRoute.Description;
                NewRelicAgent.SetTransactionName("Custom", $"Trilhas Exception {routeDescription.Method} {routeDescription.Path}");
            }

            // Faz com que o new relic mostre mais informações sobre a pessoa que causou o erro            
            if (ctx.CurrentUser != null) { 
                var currentUser = (UserIdentity) ctx.CurrentUser;
                NewRelicAgent.AddCustomParameter("JWT_TOKEN", currentUser.JwtToken);
                NewRelicAgent.AddCustomParameter("CPF", currentUser.UserName);
                NewRelicAgent.AddCustomParameter("Nome", currentUser.Usuario.Nome);                
                NewRelicAgent.AddCustomParameter("Trilha ID", $"{currentUser.Nivel.ID}");                
                NewRelicAgent.AddCustomParameter("Trilha Nome", $"{currentUser.Nivel.Nome}");                
            }
            NewRelicAgent.AddCustomParameter("Stack Trace", exception.StackTrace);

            resp.Headers.Add("Access-Control-Allow-Origin", "*");
            resp.Headers.Add("Access-Control-Allow-Methods", "POST,GET,PUT,DELETE");
            resp.Headers.Add("Access-Control-Allow-Headers", "Accept, Origin, Content-type, Authorization");

            return resp;
        }
    }
}