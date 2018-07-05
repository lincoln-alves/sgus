using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class TagMap : ClassMap<Tag>
    {
        public TagMap()
        {
            Table("TB_Tag");
            LazyLoad();
            Id(x => x.ID, "ID_Tag").GeneratedBy.Identity();
            Map(x => x.Nome, "NM_Tag").Nullable();
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
            Map(x => x.InSinonimo).Column("IN_Sinonimo");
            Map(x => x.NumeroNivel).Column("NU_Nivel");

            References(x => x.TagPai).Column("ID_TagPai");
            HasMany(x => x.ListaTagFilhos).Cascade.All().KeyColumn("ID_TagPai");

            


        }
    }
}
