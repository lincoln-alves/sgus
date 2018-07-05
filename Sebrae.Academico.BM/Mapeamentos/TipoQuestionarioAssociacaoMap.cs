using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class TipoQuestionarioAssociacaoMap: ClassMap<TipoQuestionarioAssociacao>
    {
        public TipoQuestionarioAssociacaoMap()
        {
            Table("TB_TipoQuestionarioAssociacao");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Assigned().Column("ID_TIPOQUESTIONARIOASSOCIACAO");
            Map(x => x.Nome).Column("NM_TIPOQUESTIONARIOASSOCIACAO").Not.Nullable();

            HasMany(x => x.ListaQuestionarioAssociacao).KeyColumn("ID_TipoQuestionarioAssociacao").AsBag().Inverse()
                .LazyLoad();//.Cascade.AllDeleteOrphan();

            HasMany(x => x.ListaQuestionarioParticipacao).KeyColumn("ID_TipoQuestionarioAssociacao").AsBag().Inverse()
                .LazyLoad();//.Cascade.AllDeleteOrphan();
        }
    }
}
