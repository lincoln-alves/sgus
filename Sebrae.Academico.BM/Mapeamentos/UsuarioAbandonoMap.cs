using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class UsuarioAbandonoMap: ClassMap<UsuarioAbandono>
    {
        public UsuarioAbandonoMap()
        {
            Table("TB_UsuarioAbandono");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_UsuarioAbandono");
            References(x => x.Usuario, "ID_Usuario");
            Map(x => x.DataInicioAbandono).Column("DT_InicioAbandono").Not.Nullable();
            Map(x => x.DataFimAbandono).Column("DT_FimAbandono").Not.Nullable();
            Map(x => x.Desconsiderado).Column("IN_Desconsiderado").Not.Nullable();
            Map(x => x.Origem).Column("TX_Origem");
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao").Length(100);
        }
    }
}
