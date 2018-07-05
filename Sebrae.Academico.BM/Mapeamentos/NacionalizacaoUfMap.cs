using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class NacionalizacaoUfMap : ClassMap<NacionalizacaoUf>
    {
        NacionalizacaoUfMap()
        {
            Table("TB_NacionalizacaoUF");
            LazyLoad();
            Id(x => x.ID).Column("ID_NacionalizacaoUF").GeneratedBy.Identity();
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao").Nullable();
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao").Nullable();
            References(x => x.Uf).Column("ID_UF").Cascade.None().Not.Nullable();
        }
    }
}
