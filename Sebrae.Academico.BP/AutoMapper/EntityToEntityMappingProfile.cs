using AutoMapper;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BP.AutoMapper
{
    public class EntityToEntityMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "EntityToEntityMappingProfile"; }
        }

        public EntityToEntityMappingProfile()
        {
            CreateMap<Pagina, Pagina>();
        }
    }
}