using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class LogSincroniaPostParametersMap : ClassMap<LogSincroniaPostParameters>
    {
        public LogSincroniaPostParametersMap()
        {
            Table("TB_LogSincroniaPostParameters");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_LogSincroniaPostParameters");
            References(x => x.LogSincronia).Column("ID_LogSincronia").Cascade.None().LazyLoad();
            Map(x => x.Registro).Column("VL_Registro");
            Map(x => x.Descricao).Column("DE_Registro");
            Map(x => x.DataAlteracao).Column("DT_ULTIMAATUALIZACAO");
            Map(x => x.UsuarioAlteracao).Column("NM_USUARIOATUALIZACAO");
        }

    }

}