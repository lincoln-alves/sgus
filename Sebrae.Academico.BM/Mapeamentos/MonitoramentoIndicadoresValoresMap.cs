using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class MonitoramentoIndicadoresValoresMap : ClassMap<MonitoramentoIndicadoresValores>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public MonitoramentoIndicadoresValoresMap()
        {
            Table("TB_MonitoramentoIndicadoresValores");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_MonitoramentoIndicadoresValores");
            References(x => x.MonitoramentoIndicador).Column("ID_MonitoramentoIndicadores").Cascade.None();
            Map(x => x.Registro).Column("VL_Registro");
            Map(x => x.Descricao).Column("DE_Registro");
            Map(x => x.DataAlteracao).Column("DT_ULTIMAATUALIZACAO");
            Map(x => x.UsuarioAlteracao).Column("NM_USUARIOATUALIZACAO");
        }

    }
}