using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class ViewTrilhaMap : ClassMap<ViewTrilha>
    {
        public ViewTrilhaMap()
        {
            LazyLoad();
            Table("VW_Trilha");
            Id(x => x.Linha, "nu_linha").GeneratedBy.Assigned().CustomSqlType("bigint");
            References<TrilhaTopicoTematico>(x => x.TopicoTematico, "ID_TrilhaTopicoTematico").Cascade.None().LazyLoad();
            References<Trilha>(x => x.TrilhaOrigem, "Id_Trilha").Cascade.None().LazyLoad();
            References<TrilhaNivel>(x => x.TrilhaNivelOrigem, "Id_TrilhaNivel").Cascade.None().LazyLoad();
            References<Usuario>(x => x.UsuarioOrigem, "ID_UsuarioItemTrilha").Cascade.None().LazyLoad();
            References<ItemTrilha>(x => x.ItemTrilha, "Id_ItemTrilha").Cascade.None().LazyLoad();
            References<FileServer>(x => x.Anexo, "Id_FileServer").Cascade.None().LazyLoad();
            Map(x => x.OrdemTrilhaNivel).Column("VL_Ordem");
            Map(x => x.Aprovado).Column("IN_Aprovado").CustomType<enumStatusSolucaoEducacionalSugerida>();
            Map(x => x.Objetivo).Column("DE_Objetivo").Not.Nullable().Length(500);

        }
    }
}
