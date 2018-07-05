using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class ViewSolicitacaoDemandaMap : ClassMap<ViewSolicitacaoDemanda>
    {
        public ViewSolicitacaoDemandaMap()
        {
            LazyLoad();
            Table("VW_RelatorioSolicitacaoDemandas");
            Id(x => x.IdProcessoResposta, "ID_ProcessoResposta");
            References<Processo>(x => x.Processo, "Processo").Cascade.None().LazyLoad();
            References<Usuario>(x => x.UsuarioDemandante, "Usuario").Cascade.None().LazyLoad();
            References<Etapa>(x => x.EtapaAtual, "EtapaAtual").Cascade.None().LazyLoad();

            Map(x => x.DataAbertura).Column("DataAbertura");
            Map(x => x.CargaHoraria).Column("CargaHoraria");
            Map(x => x.DtInicioCapacitacao).Column("DataInicioCapacitacao");
            Map(x => x.DtTerminoCapacitacao).Column("DataTerminoCapacitacao");
            Map(x => x.Local).Column("Local");
            Map(x => x.Titulo).Column("Titulo");

            Map(x => x.IN_Status).Column("IN_Status");
            
            Map(x => x.ValorPrevistoInscricao).Column("ValorPrevistoInscricao");
            Map(x => x.ValorPrevistoPassagem).Column("ValorPrevistoPassagem");
            Map(x => x.ValorPrevistoDiaria).Column("ValorPrevistoDiaria");

            Map(x => x.ValorExecutadoInscricao).Column("ValorExecutadoInscricao");
            Map(x => x.ValorExecutadoPassagem).Column("ValorExecutadoPassagem");
            Map(x => x.ValorExecutadoDiaria).Column("ValorExecutadoDiaria");

            Map(x => x.ValorTotalPrevisto).Column("ValorTotalPrevisto");
            Map(x => x.ValorTotalExecutado).Column("ValorTotalExecutado");
        }
    }
}
