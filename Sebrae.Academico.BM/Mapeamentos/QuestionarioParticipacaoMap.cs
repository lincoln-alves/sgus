using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class QuestionarioParticipacaoMap : ClassMap<QuestionarioParticipacao>
    {
        public QuestionarioParticipacaoMap()
        {
            Table("TB_QuestionarioParticipacao");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_QuestionarioParticipacao");
            References(x => x.Usuario).LazyLoad().Column("ID_Usuario");
            References(x => x.Questionario).Column("ID_Questionario").Not.LazyLoad();
            References(x => x.TrilhaNivel).Column("ID_TrilhaNivel").Not.LazyLoad();
            References(x => x.Turma).LazyLoad().Column("ID_Turma");
            References(x => x.NivelOcupacional).LazyLoad().Column("ID_NivelOcupacional");
            References(x => x.Uf).LazyLoad().Column("ID_UF");
            References(x => x.TipoQuestionarioAssociacao, "ID_TipoQuestionarioAssociacao").Nullable().Cascade.None().LazyLoad();
            References(x => x.MatriculaTurma).LazyLoad().Column("ID_MatriculaTurma");
            References(x => x.TurmaCapacitacao).LazyLoad().Column("ID_TurmaCapacitacao");
            Map(x => x.DataGeracao).Column("DT_Geracao").Not.Nullable();
            Map(x => x.DataParticipacao).Column("DT_Participacao");
            Map(x => x.DataLimiteParticipacao).Column("DT_LimiteParticipacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.Evolutivo).Column("IN_EVOLUTIVO");
            Map(x => x.TextoEnunciadoPre).Column("TX_ENUNCIADOPRE");
            Map(x => x.TextoEnunciadoPos).Column("TX_ENUNCIADOPOS");
            Map(x => x.NotaMinima).Column("VL_NotaMinima");
            Map(x => x.IdItemTrilha).Column("ID_ItemTrilha");

            HasMany(x => x.ListaItemQuestionarioParticipacao)
                .Cascade.AllDeleteOrphan()
                .KeyColumn("ID_QuestionarioParticipacao")
                .AsBag()
                .Inverse()
                .LazyLoad();

            HasMany(x => x.ListaItemTrilhaParticipacao)
                .Cascade.AllDeleteOrphan()
                .KeyColumn("ID_QuestionarioParticipacao")
                .AsBag()
                .Inverse()
                .LazyLoad();
        }
    }
}
