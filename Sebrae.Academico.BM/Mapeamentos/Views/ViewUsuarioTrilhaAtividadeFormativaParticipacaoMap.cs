using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;


namespace Sebrae.Academico.BM.Mapeamentos
{
    public class ViewUsuarioTrilhaAtividadeFormativaParticipacaoMap: ClassMap<ViewUsuarioTrilhaAtividadeFormativaParticipacao>
    {
        public ViewUsuarioTrilhaAtividadeFormativaParticipacaoMap()
        {
            Table("VW_UsuarioTrilhaAtividadeFormativaParticipacao");
            Id(x => x.Linha, "nu_linha").GeneratedBy.Assigned().CustomSqlType("bigint");
            Map(x => x.TemParticipacao).Column("IN_TemParticipacao");
            References<TrilhaTopicoTematico>(x => x.TopicoTematico, "ID_TrilhaTopicoTematico").Cascade.None().LazyLoad();
            References<Trilha>(x => x.TrilhaOrigem, "ID_Trilha").Cascade.None().LazyLoad();
            References<TrilhaNivel>(x => x.TrilhaNivelOrigem, "ID_TrilhaNivel").Cascade.None().LazyLoad();
            //References<Usuario>(x => x.UsuarioOrigem, "ID_UsuarioItemTrilha").Cascade.None().LazyLoad();
            References(x => x.UsuarioOrigem, "Id_Usuario").Cascade.None().LazyLoad();
            References<UsuarioTrilha>(x => x.UsuarioTrilha, "ID_UsuarioTrilha").Cascade.None().LazyLoad();
            Map(x => x.IdTrilhaAtividadeFormativaParticipacao).Column("ID_TrilhaAtividadeFormativaParticipacao");
            
        }
    }
}
