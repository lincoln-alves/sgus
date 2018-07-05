using Nancy.Security;
using Sebrae.Academico.BP.DTO.Services.Trilhas;
using Sebrae.Academico.BP.Services;

namespace Sebrae.Academico.Trilhas.Modules
{
    public class PlanoEnsino : GenericModule
    {
        public PlanoEnsino() : base("PlanoEnsino")
        {
            this.RequiresAuthentication();
            
            Get["/"] = p =>
            {
                var trilhaServices = new TrilhaServices();
                return new DtoResponse(trilhaServices.ObterNiveisPorTrilhaUsuario(AcessoAtual.Nivel.Trilha.ID, AcessoAtual.Usuario.ID));                
            };
        }
    }
}