using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class TipoPagamentoMap: ClassMap<TipoPagamento>
    {
        public TipoPagamentoMap()
        {
            Table("TB_TipoPagamento");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_TipoPagamento");
            Map(x => x.Nome).Column("NM_TipoPagamento").Not.Nullable();
        }
    }
}
