using Nancy.Security;
using Sebrae.Academico.BP.DTO.Services.Trilhas;
using Sebrae.Academico.BP.Services;

namespace Sebrae.Academico.Trilhas.Modules
{
    public class MapaPreviewModule : GenericModule
    {
        public MapaPreviewModule() : base("Preview")
        {
            // Retornar os dados do mapa.
            Get["/{nivelId:int}"] = p =>
            {
                return new DtoResponse(new TrilhaServices().ObterDadosMapaPreview(p.nivelId));
            };
        }
    }
}