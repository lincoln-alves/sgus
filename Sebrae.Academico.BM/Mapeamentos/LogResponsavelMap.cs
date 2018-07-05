using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class LogResponsavelMap : ClassMap<LogResponsavel>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public LogResponsavelMap()
        {
            Table("LG_LogResponsavel");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_LogResponsavel");
            Map(x => x.Data).Column("DT_DataAlteracao").Not.Nullable();
            References(x => x.Responsavel).Column("ID_Responsavel").Not.Nullable();
            References(x => x.UsuarioAlteracao).Column("ID_UsuarioAlteracao").Not.Nullable();
            References(x => x.Turma).Column("ID_Turma").Not.Nullable();
        }

    }
}
