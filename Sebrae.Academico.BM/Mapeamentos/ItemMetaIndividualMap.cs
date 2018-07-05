using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class ItemMetaIndividualMap: ClassMap<ItemMetaIndividual>
    {
        public ItemMetaIndividualMap()
        {
            Table("TB_ItemMetaIndividual");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_ItemMetaIndividual");
            References(x => x.MetaIndividual).Column("ID_MetaIndividual");
            References(x => x.SolucaoEducacional).Column("ID_SolucaoEducacional");
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
        }
    }
}
