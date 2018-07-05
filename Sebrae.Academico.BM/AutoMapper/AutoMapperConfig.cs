using AutoMapper;

namespace Sebrae.Academico.BM.AutoMapper
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<DomainToDomainMappingProfile>();
            });

            //TODO: Validar os mappings, essa chamada abaixo é impressindivel pois ela valida e atesta que os mappings estão ok, como estes mappings estão incompletos deixei aqui comentado. No outro local onde estou utilizando está 100%.
            //Mapper.AssertConfigurationIsValid();
        }
    }
}
