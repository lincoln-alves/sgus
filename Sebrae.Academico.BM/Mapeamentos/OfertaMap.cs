using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class OfertaMap : ClassMap<Oferta>
    {
        public OfertaMap()
        {
            Table("TB_Oferta");
            LazyLoad();
            Id(x => x.ID, "ID_Oferta").GeneratedBy.Identity();
            Map(x => x.IDChaveExterna, "ID_ChaveExterna");
            Map(x => x.Nome, "NM_Oferta");
            Map(x => x.FiladeEspera, "IN_FilaEspera").CustomSqlType("bit").Not.Nullable();
            Map(x => x.DataInicioInscricoes, "DT_InicioInscricoes").Nullable();
            Map(x => x.DataFimInscricoes, "DT_FimInscricoes").Nullable();
            Map(x => x.InscricaoOnline, "IN_Inscricaoonline").CustomSqlType("bit");
            Map(x => x.ValorPrevisto, "VL_Previsto");
            Map(x => x.ValorRealizado, "VL_Realizado");
            Map(x => x.DataAlteracao, "DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao, "NM_UsuarioAtualizacao");
            Map(x => x.CodigoMoodle, "ID_CodigoMoodle");
            Map(x => x.DiasPrazo, "QT_DiasPrazo").Nullable();
            Map(x => x.CargaHoraria).Column("QT_CargaHoraria");
            Map(x => x.MatriculaGestorUC).Column("IN_MatriculaGestorUC");
            Map(x => x.TipoOferta, "ID_TipoOferta").CustomType<enumTipoOferta>();
            Map(x => x.EmailResponsavel).Column("NM_EmailResponsavel");
            Map(x => x.QuantidadeMaximaInscricoes).Column("QT_MaxInscricoes");
            Map(x => x.AlteraPeloGestorUC).Column("IN_AlteraPeloGestorUC");
            Map(x => x.PermiteCadastroTurmaPeloGestorUC).Column("IN_PermiteCadastroTurmaPeloGestorUC");
            Map(x => x.Sequencia).Column("SQ_Sequencia");
            Map(x => x.Link).Column("TX_Link");
            Map(x => x.IdNodePortal).Column("ID_Node_Portal");
            Map(x => x.InformacaoAdicional).Column("TX_InformacaoAdicional");
            Map(x => x.DistribuicaoVagas, "VL_DistribuicaoVagas").CustomType<enumDistribuicaoVagasOferta>();

            References(x => x.CertificadoTemplate).Column("ID_CertificadoTemplate");
            References(x => x.CertificadoTemplateProfessor).Column("ID_CertificadoTemplateProfessor");
            References(x => x.SolucaoEducacional).Column("ID_SolucaoEducacional").Not.LazyLoad();


            HasMany(x => x.ListaMatriculaOferta).KeyColumn("ID_Oferta").AsBag()
                .Inverse().Cascade.All(); //.Cascade.AllDeleteOrphan();

            HasMany(x => x.ListaPermissao).KeyColumn("ID_Oferta").AsBag().Inverse() //.BatchSize(250) //.Inverse()
                .LazyLoad().Cascade.AllDeleteOrphan();

            HasMany(x => x.ListaTurma).KeyColumn("ID_Oferta").AsBag()
                .Inverse().Cascade.All();

            HasMany(x => x.ListaOfertaGerenciadorVaga).KeyColumn("ID_Oferta").AsBag()
                .Inverse().Cascade.All();

            HasMany(x => x.ListaPublicoAlvo)
                .KeyColumn("ID_Oferta")
                .AsBag()
                .Inverse()
                .LazyLoad()
                .Cascade.AllDeleteOrphan();
            
            HasManyToMany(a => a.ListaNiveisTrancados)
                .Table("TB_OfertaTrancadaParaPagante")
                .ParentKeyColumn("ID_Oferta")
                .ChildKeyColumn("ID_NivelOcupacional");
        }
    }
}
