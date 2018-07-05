using System.Collections.Generic;
using JWT;
using Nancy;
using Nancy.ModelBinding;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.DTO.Services.Trilhas.Auth;

namespace Sebrae.Academico.Trilhas.Modules
{
    public class IndexModule : BaseModule
    {
        public IndexModule()
        {
            Get["/"] = p =>
            {
                return "WS de Trilhas!";
            };
        }
    }
}