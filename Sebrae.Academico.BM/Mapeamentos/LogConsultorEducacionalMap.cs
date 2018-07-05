using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class LogConsultorEducacionalMap : ClassMap<LogConsultorEducacional>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public LogConsultorEducacionalMap()
        {
            Table("LG_LogConsultorEducacional");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_LogConsultorEducacional");
            Map(x => x.Data).Column("DT_DataAlteracao").Not.Nullable();
            References(x => x.ConsultorEducacional).Column("ID_ConsultorEducacional").Not.Nullable();
            References(x => x.UsuarioAlteracao).Column("ID_UsuarioAlteracao").Not.Nullable();
            References(x => x.Turma).Column("ID_Turma").Not.Nullable();
        }
    }
}
