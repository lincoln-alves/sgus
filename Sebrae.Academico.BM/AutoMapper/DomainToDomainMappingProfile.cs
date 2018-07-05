using System.Linq;
using AutoMapper;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.AutoMapper
{
    public class DomainToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainToDomainMappings"; }
        }

        public DomainToDomainMappingProfile(){
            CreateMap<Processo, Processo>();

            CreateMap<Etapa, Etapa>()
                .ForMember(x => x.Processo, opt => opt.Ignore())                
                .ForMember(x => x.ListaCampos, opt => opt.Ignore());

            CreateMap<Campo, Campo>()
                .ForMember(x => x.ID, opt => opt.Ignore())
                .ForMember(x => x.Etapa, opt => opt.Ignore())
                .ForMember(x => x.ListaCamposVinculados, opt => opt.Ignore());

            CreateMap<Alternativa, Alternativa>()
                .ForMember(x => x.ID, opt => opt.Ignore());

            CreateMap<CampoMeta, CampoMeta>()
                .ForMember(x => x.ID, opt => opt.Ignore())
                .ForMember(x => x.FirstMetaValueId, opt => opt.Ignore())
                .ForMember(x => x.FirstMetaValue, opt => opt.Ignore());

            CreateMap<CampoMetaValue, CampoMetaValue>()
                .ForMember(x => x.ID, opt => opt.Ignore())
                .ForMember(x => x.Campo, opt => opt.Ignore());

            CreateMap<StatusMatricula, StatusMatricula>();

            CreateMap<TermoAceite, TermoAceite>()
                .ForMember(x => x.ID, opt => opt.Ignore())
                .ForMember(x => x.ListaCategoriaConteudo,
                    y => y.MapFrom(termo => termo.ListaCategoriaConteudo.Select(c => new TermoAceiteCategoriaConteudo
                    {
                        CategoriaConteudo = c.CategoriaConteudo,
                        TermoAceite = termo
                    })));

            CreateMap<EnvioInforme, EnvioInforme>()
                .ForMember(x => x.ID, opt => opt.Ignore())
                .ForMember(x => x.DataEnvio, opt => opt.Ignore())
                .ForMember(x => x.Perfis, y => y.MapFrom(opt => opt.Perfis.Select(p => new Perfil { ID = p.ID })))
                .ForMember(x => x.NiveisOcupacionais, y => y.MapFrom(opt => opt.NiveisOcupacionais.Select(p => new NivelOcupacional { ID = p.ID })))
                .ForMember(x => x.Ufs, y => y.MapFrom(opt => opt.Ufs.Select(p => new Uf { ID = p.ID })));
        }
        
    }
}
