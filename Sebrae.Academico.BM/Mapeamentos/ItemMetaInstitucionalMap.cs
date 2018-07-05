using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class ItemMetaInstitucionalMap : ClassMap<ItemMetaInstitucional>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public ItemMetaInstitucionalMap()
        {
            Table("TB_ItemMetaInstitucional");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_ItemMetaInstitucional");
            References(x => x.MetaInstitucional).Column("ID_MetaInstitucional");
            References(x => x.SolucaoEducacional).Column("ID_SolucaoEducacional");
            References(x => x.Usuario).Column("ID_Usuario");
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
            

        }
    }

}