using AutoMapper;

namespace Sebrae.Academico.BP.AutoMapper
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<EntityToDTOMappingProfile>();
                x.AddProfile<EntityToEntityMappingProfile>();
            });

            Mapper.AssertConfigurationIsValid();
        }
    }
}
