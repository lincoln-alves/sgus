using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class PublicoAlvoMap : ClassMap<PublicoAlvo>
    {
        public PublicoAlvoMap()
        {
            Table("TB_PublicoAlvo");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_PublicoAlvo");
            Map(x => x.Nome).Column("NM_PublicoAlvo");
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
            Map(x => x.Tipo).Column("VL_Tipo");
			
			References(x => x.UF).Column("ID_UF");

            HasMany(x => x.ListaOferta).KeyColumn("ID_PublicoAlvo").AsBag().Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }
}
