using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class ViewUsuarioItemTrilhaParticipacaoMap: ClassMap<ViewUsuarioItemTrilhaParticipacao>
    {
        public ViewUsuarioItemTrilhaParticipacaoMap()
        {
            Table("VW_USUARIOITEMTRILHAPARTICIPACAO");
            Id(x => x.Linha, "nu_linha").GeneratedBy.Assigned().CustomSqlType("bigint");
            Map(x => x.TemParticipacao).Column("IN_TemParticipacao");
            References<TrilhaTopicoTematico>(x => x.TopicoTematico, "ID_TrilhaTopicoTematico").Cascade.None().LazyLoad();
            References<Trilha>(x => x.TrilhaOrigem, "Id_Trilha").Cascade.None().LazyLoad();
            References<TrilhaNivel>(x => x.TrilhaNivelOrigem, "Id_TrilhaNivel").Cascade.None().LazyLoad();
            //References<Usuario>(x => x.UsuarioOrigem, "ID_UsuarioItemTrilha").Cascade.None().LazyLoad();
            References<Usuario>(x => x.UsuarioOrigem, "ID_Usuario").Cascade.None().LazyLoad();
            References<ItemTrilha>(x => x.ItemTrilha, "Id_ItemTrilha").Cascade.None().LazyLoad();
            References<UsuarioTrilha>(x => x.UsuarioTrilha, "ID_UsuarioTrilha").Cascade.None().LazyLoad();
            Map(x => x.IdItemTrilhaParticipacao).Column("ID_ItemTrilhaParticipacao");
            
        }
    }
}
