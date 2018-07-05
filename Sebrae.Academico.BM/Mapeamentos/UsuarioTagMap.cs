using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class UsuarioTagMap : ClassMap<UsuarioTag>
    {
        public UsuarioTagMap()
        {
            Table("TB_UsuarioTag");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_UsuarioTag");
            References(x => x.Tag, "ID_TAG");
            References(x => x.Usuario, "ID_Usuario");
            Map(x => x.DataValidade).Column("DT_Validade").Nullable();
            Map(x => x.Adicionado).Column("IN_Adicionado");
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");

        }
    }
}
