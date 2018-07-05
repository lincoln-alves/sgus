using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class HelperTagMap : ClassMap<HelperTag>
    {
        public HelperTagMap()
        {
            Table("TB_HelperTag");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_HelperTag");
            Map(x => x.Chave).Column("VL_Chave");
            Map(x => x.Descricao).Column("TX_Descricao").Length(4000);

            References(x => x.Pagina).Column("ID_Pagina");
        }
    }
}
