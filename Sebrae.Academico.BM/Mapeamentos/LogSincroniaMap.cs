using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class LogSincroniaMap : ClassMap<LogSincronia>
    {
        public LogSincroniaMap()
        {
            Table("TB_LogSincronia");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_LogSincronia");
            References(x => x.Usuario).Column("ID_Usuario").Cascade.None().LazyLoad();
            Map(x => x.DataAlteracao).Column("DT_ULTIMAATUALIZACAO");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
            Map(x => x.Hash).Column("HashSincronia");
            Map(x => x.Url).Column("NM_URLPOST");
            Map(x => x.Action).Column("NM_Action");
            Map(x => x.Method).Column("NM_Method");
            Map(x => x.DataCriacao).Column("DT_Evento");
            Map(x => x.Sincronizado).Column("IN_Sincronizado").CustomType<bool>();
            Map(x => x.Acao).Column("IN_ACAO").CustomType<enumAcao>();

            HasMany(x => x.ListaPostParameters).KeyColumn("ID_LogSincronia").AsBag().Cascade.All().Not.KeyNullable();
        }
    }
}