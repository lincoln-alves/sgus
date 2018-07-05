using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class ItemTrilhaAvaliacaoMap : ClassMap<ItemTrilhaAvaliacao>
    {
        public ItemTrilhaAvaliacaoMap()
        {
            Table("TB_ItemTrilhaAvaliacao");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_ItemTrilhaAvaliacao");
            Map(x => x.Resenha).Column("TX_Resenha");
            Map(x => x.Avaliacao).Column("VL_Avaliacao");
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            References(x => x.ItemTrilha).Column("ID_ItemTrilha").Not.Nullable();
            References(x => x.UsuarioTrilha).Column("ID_UsuarioTrilha").Not.Nullable();
        }
    }
}
