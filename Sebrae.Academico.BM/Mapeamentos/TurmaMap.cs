using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class TurmaMap : ClassMap<Turma>
    {
        public TurmaMap()
        {
            Table("TB_Turma");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_Turma");
            References(x => x.Oferta).Column("ID_Oferta").Cascade.None().Not.Nullable().UniqueKey("UK_TURMA_OFERTA");
            References(x => x.Professor).Column("ID_Professor").Cascade.None();
            Map(x => x.IDChaveExterna).Column("ID_ChaveExterna");
            Map(x => x.Nome).Column("NM_Turma").UniqueKey("UK_TURMA_OFERTA");
            Map(x => x.Local).Column("NM_Local");
            Map(x => x.DataInicio).Column("DT_Inicio");
            Map(x => x.DataFinal).Column("DT_Final");
            Map(x => x.TipoTutoria).Column("NM_TipoTutoria");
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
            Map(x => x.NotaMinima).Column("VL_NotaMinima");
            Map(x => x.InAberta).Column("IN_Aberta");
            Map(x => x.QuantidadeMaximaInscricoes).Column("QT_MaxInscricoes");
            Map(x => x.AcessoWifi).Column("TP_AcessoWifi");
            Map(x => x.Sequencia).Column("SQ_Sequencia");
            Map(x => x.AcessoAposConclusao).Column("IN_AcessoAposConclusao");
            Map(x => x.Status).Column("VL_Status").CustomType<enumStatusTurma?>().Nullable();
            Map(x => x.InAvaliacaoAprendizagem).Column("IN_AvaliacaoAprendizagem");

            References(x => x.Responsavel).Column("ID_Responsavel").Cascade.None();
            References(x => x.ConsultorEducacional).Column("ID_ConsultorEducacional").Cascade.None();

            HasMany(x => x.ListaMatriculas).KeyColumn("Id_Turma").NotFound.Ignore().AsBag().Inverse()
                .LazyLoad().Cascade.AllDeleteOrphan();

            HasMany(x => x.ListaQuestionarioAssociacao).KeyColumn("ID_Turma").NotFound.Ignore().AsBag().Inverse()
                .LazyLoad().Cascade.All();

            HasMany(x => x.ListaQuestionarioParticipacao).KeyColumn("ID_Turma").LazyLoad().Cascade.All();

            HasMany(x => x.JustificativasStatus).KeyColumn("ID_Turma").Cascade.All();

            HasMany(x => x.Avaliacoes).KeyColumn("ID_Turma").LazyLoad().Cascade.All();

            HasManyToMany(a => a.Informes)
                .Table("TB_InformeTurma")
                .ParentKeyColumn("ID_Turma")
                .ChildKeyColumn("ID_Informe");

            HasManyToMany(a => a.Professores)
                .Table("TB_TurmaProfessor")
                .ParentKeyColumn("ID_Turma")
                .ChildKeyColumn("ID_Usuario");
        }
    }
}
