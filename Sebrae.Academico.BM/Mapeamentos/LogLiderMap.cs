using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{

    public sealed class LogLiderMap : ClassMap<LogLider>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public LogLiderMap()
        {
            Table("TB_LogLider");
            LazyLoad();

            Id(x => x.ID, "ID_LogLider").GeneratedBy.Identity();
            Map(x => x.Tempo).Column("VL_Tempo");
            References(x => x.Aluno).Column("ID_Aluno").Cascade.None();
            References(x => x.Lider).Column("ID_Lider").Cascade.None();
            References(x => x.PontoSebrae).Column("ID_PontoSebrae").Cascade.None();
        }
    }
}