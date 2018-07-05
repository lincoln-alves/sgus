using System.Text.RegularExpressions;
using AutoMapper;
using Sebrae.Academico.BP.DTO.Services.ManterSolucaoEducacionalService;
using Sebrae.Academico.BP.DTO.Services.Trilhas.Mapa;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.AutoMapper
{
    public class EntityToDTOMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "EntityToDTOMappingProfile"; }
        }

        public EntityToDTOMappingProfile()
        {
            CreateMap<Usuario, DTOUsuarioSAS>()
                .ForMember(src => src.Sexo, opt => opt.MapFrom(dst => (dst.Sexo == "Masculino" ? 1 : 0)))
                .ForMember(src => src.Cpf, opt => opt.MapFrom(dst => dst.CPF))
                .ForMember(src => src.SSID, opt => opt.MapFrom(dst => dst.SID_Usuario))
                .ForMember(src => src.DataNascimento, opt => opt.ResolveUsing<DateOrDefault>())
                .ForMember(src => src.Telefone, opt => opt.ResolveUsing<FirstTelphoneOrDefault>())
                .ForMember(src => src.Cidade, opt => opt.Ignore())
                .ForMember(src => src.UF, opt => opt.Ignore());

            CreateMap<ItemQuestionario, ItemQuestionarioParticipacao>()
                .ForMember(x => x.QuestionarioParticipacao, opt => opt.Ignore())
                .ForMember(x => x.ValorAtribuido, opt => opt.Ignore())
                .ForMember(x => x.Resposta, opt => opt.Ignore())
                .ForMember(x => x.ListaOpcoesParticipacao, opt => opt.Ignore())
                .ForMember(x => x.ID, opt => opt.Ignore())
                .ForMember(x => x.Gabarito, y => y.MapFrom(opt => opt.NomeGabarito))
                .ForMember(x => x.InAvaliaProfessorFormatado, opt => opt.Ignore());

            CreateMap<DTO.Dominio.DTOUf, Uf>()
                .ForMember(dst => dst.ListaTrilhaPermissao, opt => opt.Ignore())
                .ForMember(dst => dst.ListaCategoriaConteudoUF, opt => opt.Ignore())
                .ForMember(dst => dst.ListaNacionalizacaoUf, opt => opt.Ignore())
                .ForMember(dst => dst.ListaTrilhaPermissao, opt => opt.Ignore())
                .ForMember(dst => dst.ListaOfertaPermissao, opt => opt.Ignore())
                .ForMember(dst => dst.ListaRelatoriosPaginaInicial, opt => opt.Ignore())
                .ForMember(dst => dst.ListaProgramaPermissao, opt => opt.Ignore())
                .ForMember(dst => dst.TermosAceite, opt => opt.Ignore())
                .ForMember(dst => dst.Regiao, opt => opt.Ignore())
                .ForMember(dst => dst.PublicosAlvos, opt => opt.Ignore())
                .ForMember(dst => dst.ListaEtapaPermissao, opt => opt.Ignore());

            CreateMap<DTO.Dominio.DTONivelOcupacional, NivelOcupacional>()
                .ForMember(dst => dst.ListaEtapaPermissao, opt => opt.Ignore());

            CreateMap<DTO.Dominio.DTOPerfil, Perfil>()
                .ForMember(dst => dst.ListaRelatorioPaginaInicial, opt => opt.Ignore())
                .ForMember(dst => dst.Auditoria, opt => opt.Ignore())
                .ForMember(dst => dst.DataAlteracao, opt => opt.Ignore())
                .ForMember(dst => dst.UsuarioAlteracao, opt => opt.Ignore());

            CreateMap<DTO.Dominio.DTOFormaAquisicao, FormaAquisicao>()
                .ForMember(dst => dst.CargaHoraria, opt => opt.Ignore())
                .ForMember(dst => dst.CargaHorariaFormatada, opt => opt.Ignore())
                .ForMember(dst => dst.EnviarPortal, opt => opt.Ignore())
                .ForMember(dst => dst.EnviarPortalFormatado, opt => opt.Ignore())
                .ForMember(dst => dst.Imagem, opt => opt.Ignore())
                .ForMember(dst => dst.ListaItemTrilha, opt => opt.Ignore())
                .ForMember(dst => dst.ListaSolucaoEducacional, opt => opt.Ignore())
                .ForMember(dst => dst.PermiteAlterarCargaHoraria, opt => opt.Ignore())
                .ForMember(dst => dst.Presencial, opt => opt.Ignore())
                .ForMember(dst => dst.Responsavel, opt => opt.Ignore())
                .ForMember(dst => dst.TipoFormaDeAquisicao, opt => opt.Ignore())
                .ForMember(dst => dst.Uf, opt => opt.Ignore())
                .ForMember(dst => dst.UFResponsavel, opt => opt.Ignore())
                .ForMember(dst => dst.Auditoria, opt => opt.Ignore())
                .ForMember(dst => dst.DataAlteracao, opt => opt.Ignore())
                .ForMember(dst => dst.Descricao, opt => opt.Ignore())
                .ForMember(dst => dst.UsuarioAlteracao, opt => opt.Ignore());

            CreateMap<DTO.Dominio.DTOCategoriaConteudo, CategoriaConteudo>()
                .ForMember(dst => dst.Apresentacao, opt => opt.Ignore())
                .ForMember(dst => dst.Auditoria, opt => opt.Ignore())
                .ForMember(dst => dst.CargaHoraria, opt => opt.Ignore())
                .ForMember(dst => dst.CategoriaConteudoPai, opt => opt.Ignore())
                .ForMember(dst => dst.DataAlteracao, opt => opt.Ignore())
                .ForMember(dst => dst.Descricao, opt => opt.Ignore())
                .ForMember(dst => dst.IdNode, opt => opt.Ignore())
                .ForMember(dst => dst.LiberarInscricao, opt => opt.Ignore())
                .ForMember(dst => dst.ListaCategoriaConteudoFilhos, opt => opt.Ignore())
                .ForMember(dst => dst.ListaCategoriaConteudoUF, opt => opt.Ignore())
                .ForMember(dst => dst.ListaPermissao, opt => opt.Ignore())
                .ForMember(dst => dst.ListaSolucaoEducacional, opt => opt.Ignore())
                .ForMember(dst => dst.ListaStatusMatricula, opt => opt.Ignore())
                .ForMember(dst => dst.ListaTags, opt => opt.Ignore())
                .ForMember(dst => dst.ListaTrilha, opt => opt.Ignore())
                .ForMember(dst => dst.ListaUsuario, opt => opt.Ignore())
                .ForMember(dst => dst.MetaFm, opt => opt.Ignore())
                .ForMember(dst => dst.PossuiAreas, opt => opt.Ignore())
                .ForMember(dst => dst.PossuiFiltroCategorias, opt => opt.Ignore())
                .ForMember(dst => dst.PossuiStatus, opt => opt.Ignore())
                .ForMember(dst => dst.Sigla, opt => opt.Ignore())
                .ForMember(dst => dst.TermoAceiteCategoriaCounteudo, opt => opt.Ignore())
                .ForMember(dst => dst.UF, opt => opt.Ignore())
                .ForMember(dst => dst.UsuarioAlteracao, opt => opt.Ignore());

            #region Questionarios

            CreateMap<DTO.Dominio.DTOTipoItemQuestionario, TipoItemQuestionario>()
                .ForMember(dst => dst.ListaEstilosItemQuestionario, opt => opt.Ignore())
                .ForMember(dst => dst.ListaItemQuestionario, opt => opt.Ignore())
                .ForMember(dst => dst.TodosEstilos, opt => opt.Ignore())
                .ForMember(dst => dst.Auditoria, opt => opt.Ignore())
                .ForMember(dst => dst.DataAlteracao, opt => opt.Ignore())
                .ForMember(dst => dst.Descricao, opt => opt.Ignore())
                .ForMember(dst => dst.UsuarioAlteracao, opt => opt.Ignore());


            CreateMap<DTO.Dominio.DTOEstiloItemQuestionario, EstiloItemQuestionario>()
                .ForMember(dst => dst.ListaItemQuestionario, opt => opt.Ignore());

            CreateMap<DTO.Dominio.DTOItemQuestionarioOpcoes, ItemQuestionarioOpcoes>()
                .ForMember(dst => dst.Auditoria, opt => opt.Ignore())
                .ForMember(dst => dst.DataAlteracao, opt => opt.Ignore())
                .ForMember(dst => dst.Descricao, opt => opt.Ignore())
                .ForMember(dst => dst.UsuarioAlteracao, opt => opt.Ignore());

            CreateMap<DTO.Dominio.DTOItemQuestionario, ItemQuestionario>()
                .ForMember(dst => dst.Auditoria, opt => opt.Ignore())
                .ForMember(dst => dst.DataAlteracao, opt => opt.Ignore())
                .ForMember(dst => dst.UsuarioAlteracao, opt => opt.Ignore());

            CreateMap<DTO.Dominio.DTOTipoQuestionario, TipoQuestionario>()
                .ForMember(dst => dst.ListaQuestionario, opt => opt.Ignore());

            CreateMap<DTO.Dominio.DTOQuestionarioPermissao, QuestionarioPermissao>()
                .ForMember(dst => dst.ID, opt => opt.Ignore())
                .ForMember(dst => dst.Nome, opt => opt.Ignore())
                .ForMember(dst => dst.Questionario, opt => opt.Ignore())
                .ForMember(dst => dst.Auditoria, opt => opt.Ignore())
                .ForMember(dst => dst.DataAlteracao, opt => opt.Ignore())
                .ForMember(dst => dst.Descricao, opt => opt.Ignore())
                .ForMember(dst => dst.UsuarioAlteracao, opt => opt.Ignore());

            CreateMap<DTO.Dominio.DTOQuestionario, Questionario>()
                .ForMember(dst => dst.ListaQuestionarioAssociacao, opt => opt.Ignore())
                .ForMember(dst => dst.ListaQuestionarioParticipacao, opt => opt.Ignore())
                .ForMember(dst => dst.ListaCampos, opt => opt.Ignore())
                .ForMember(dst => dst.ListaItemTrilha, opt => opt.Ignore())
                .ForMember(dst => dst.Auditoria, opt => opt.Ignore())
                .ForMember(dst => dst.DataAlteracao, opt => opt.Ignore())
                .ForMember(dst => dst.Descricao, opt => opt.Ignore())
                .ForMember(dst => dst.UsuarioAlteracao, opt => opt.Ignore());

            #endregion
        }

        public class FirstTelphoneOrDefault : IValueResolver<Usuario, DTOUsuarioSAS, string>
        {
            public string Resolve(Usuario source, DTOUsuarioSAS dest, string value, ResolutionContext context)
            {
                if (!string.IsNullOrEmpty(source.TelefoneExibicao) && Regex.Replace(source.TelefoneExibicao, @"\D", "").Length >= 10)
                    return Regex.Replace(source.TelefoneExibicao, @"\D", "").ToString();

                if (!string.IsNullOrEmpty(source.TelResidencial) && Regex.Replace(source.TelResidencial, @"\D", "").Length >= 10)
                    return Regex.Replace(source.TelResidencial, @"\D", "").ToString();

                if (!string.IsNullOrEmpty(source.TelCelular) && Regex.Replace(source.TelCelular, @"\D", "").Length >= 10)
                    return Regex.Replace(source.TelCelular, @"\D", "").ToString();

                return "6132081124";
            }
        }

        public class DateOrDefault : IValueResolver<Usuario, DTOUsuarioSAS, string>
        {
            public string Resolve(Usuario source, DTOUsuarioSAS dest, string value, ResolutionContext context)
            {
                if (source.DataNascimento.HasValue)
                    return source.DataNascimento.Value.ToString("ddMMyyyy");
                return "01011900";
            }
        }
    }
}