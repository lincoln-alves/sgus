using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class UsuarioMap : ClassMap<Usuario>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public UsuarioMap()
        {
            Table("TB_USUARIO");
            
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_USUARIO");
            References<Uf>(x => x.UF).Column("ID_UF");//.LazyLoad();
            References<NivelOcupacional>(x => x.NivelOcupacional).Column("ID_NIVELOCUPACIONAL");
            Map(x => x.Nome).Column("NOME").Not.Nullable();
            Map(x => x.TipoDocumento).Column("TIPODOCUMENTO");
            Map(x => x.MiniCurriculo).Column("MINICURRICULO").Length(2147483647);
            Map(x => x.NumeroIdentidade).Column("NUMIDENTIDADE");
            Map(x => x.OrgaoEmissor).Column("ORGAOEMISSOR");
            Map(x => x.DataExpedicaoIdentidade).Column("DATAEXPEDICAOIDENTIDADE");
            Map(x => x.DataNascimento).Column("DATANASCIMENTO");
            Map(x => x.Sexo).Column("SEXO");
            Map(x => x.Nacionalidade).Column("NACIONALIDADE");
            Map(x => x.EstadoCivil).Column("ESTADOCIVIL");
            Map(x => x.NomeMae).Column("NOMEMAE");
            Map(x => x.NomePai).Column("NOMEPAI");
            Map(x => x.Matricula).Column("MATRICULA");
            Map(x => x.Email).Column("EMAIL");
            Map(x => x.CPF).Column("CPF");
            Map(x => x.DataAdmissao).Column("DATAADMISSAO");
            Map(x => x.Situacao).Column("SITUACAO");
            Map(x => x.DataAtualizacaoCarga).Column("DATAATUALIZACAO");
            Map(x => x.LoginLms).Column("LOGINLMS");
            //Map(x => x.SenhaMD5).Column("SenhaMD5");
            Map(x => x.Senha).Column("SENHA");
            Map(x => x.Endereco).Column("ENDERECO");
            Map(x => x.Bairro).Column("BAIRRO");
            Map(x => x.Cep).Column("CEP");
            Map(x => x.Estado).Column("ESTADO");
            Map(x => x.TelResidencial).Column("TELRESIDENCIAL");
            Map(x => x.TelCelular).Column("TELCELULAR");
            Map(x => x.Cidade).Column("CIDADE");
            Map(x => x.Instituicao).Column("INSTITUICAO");
            Map(x => x.Unidade).Column("UNIDADE");
            Map(x => x.Pais).Column("PAIS");
            Map(x => x.Complemento).Column("COMPLEMENTO");
            //Map(x => x.MaterialDidatico).Column("MATERIALDIDATICO");
            //Map(x => x.Cep2).Column("CEP2");
            //Map(x => x.Endereco2).Column("ENDERECO2");
            //Map(x => x.Pais2).Column("PAIS2");
            //Map(x => x.Complemento2).Column("COMPLEMENTO2");
            //Map(x => x.Bairro2).Column("BAIRRO2");
            //Map(x => x.Cidade2).Column("CIDADE2");
           // Map(x => x.Estado2).Column("ESTADO2");
            Map(x => x.Escolaridade).Column("ESCOLARIDADE");
            Map(x => x.CodigoCampoConhecimento).Column("CODCAMPOCONHECIMENTO");
            Map(x => x.TipoInstituicao).Column("TIPOINSTITUICAO");
            Map(x => x.AnoConclusao).Column("ANOCONCLUSAO");
            Map(x => x.Naturalidade).Column("NATURALIDADE");
            Map(x => x.NomeExibicao).Column("NM_Exibicao");
            Map(x => x.TelefoneExibicao).Column("NU_TelefoneExibicao");
            Map(x => x.TipoTelefoneExibicao).Column("TP_TelefoneExibicao");
            Map(x => x.RamalExibicao).Column("NU_RamalExibicao");
            Map(x => x.DataAtualizacaoCadastralUsuario).Column("DT_AtualizacaoCadastral");
            Map(x => x.SID_Usuario).Column("SID_Usuario");
            Map(x => x.DataInsercao).Column("DT_Insercao");            
            Map(x => x.SuperAdministrador).Column("IN_SuperAdministrador");

            Map(x => x.TrilhaToken).Column("NM_TrilhaToken");
            Map(x => x.TrilhaTokenExpiry).Column("DT_TrilhaTokenExpiry");

            References(x => x.FileServer).Column("ID_FileServer").Cascade.All();

            HasMany(x => x.ListaPerfil)
                .KeyColumn("ID_Usuario").AsBag()
                   .Inverse().Cascade.AllDeleteOrphan().NotFound.Ignore();

            HasMany(x => x.ListaTag)
                .KeyColumn("ID_Usuario").AsBag()
                   .Inverse().LazyLoad().Cascade.AllDeleteOrphan().NotFound.Ignore();
            
            HasMany(x => x.ListaMatriculaOferta)
                .KeyColumn("ID_Usuario").AsBag()
                   .Inverse().LazyLoad().Cascade.All().NotFound.Ignore();

            HasMany(x => x.ListaMatriculasNaTrilha)
              .KeyColumn("ID_Usuario").AsBag().LazyLoad()
                 .Inverse().LazyLoad().Cascade.All().NotFound.Ignore();

            HasMany(x => x.ListaHistoricoExtraCurricular)
              .KeyColumn("ID_Usuario").AsBag().LazyLoad()
                 .Inverse().LazyLoad().Cascade.All().NotFound.Ignore();

            HasMany(x => x.ListaMatriculaPrograma)
              .KeyColumn("ID_Usuario").AsBag().LazyLoad()
                 .Inverse().LazyLoad().Cascade.All().NotFound.Ignore();

            HasMany(x => x.ListaMetaIndividual)
              .KeyColumn("ID_Usuario").AsBag().LazyLoad()
                 .Inverse().Cascade.All().NotFound.Ignore();

            HasMany(x => x.ListaHistoricoPagamento).KeyColumn("ID_Usuario").Inverse();

            HasMany(x => x.ListaProcessoResposta).KeyColumn("ID_Usuario").Inverse();


            Map(x => x.NotificaConteudo).Column("IN_NotificaConteudo");
            Map(x => x.NotificaOferta).Column("IN_NotificaOferta");

            Map(x => x.DataAlteracao, "DT_UltimaAtualizacao").Nullable();
            Map(x => x.UsuarioAlteracao, "NM_UsuarioAtualizacao").Nullable();

            HasManyToMany(a => a.ListaCategoriaConteudo)
                .Table("TB_UsuarioCategoriaConteudo")
                .ParentKeyColumn("ID_Usuario")
                .ChildKeyColumn("ID_CategoriaConteudo");

            HasMany(x => x.ListaUsuarioCargo).KeyColumn("ID_Usuario");

            HasMany(a => a.ListaUsuarioCertificadoCertame).KeyColumn("ID_Usuario");
            HasMany(x => x.ListaEtapaPermissao).KeyColumn("ID_PermissaoUsuario").Inverse();
        }
    }
}