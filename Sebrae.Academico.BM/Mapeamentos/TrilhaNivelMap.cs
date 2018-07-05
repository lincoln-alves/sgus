using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BM.Mapeamentos
{

    public sealed class TrilhaNivelMap : ClassMap<TrilhaNivel>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public TrilhaNivelMap()
        {
            Table("TB_TRILHANIVEL");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_TRILHANIVEL");
            References(x => x.Trilha).Column("ID_TRILHA").Cascade.None();
            References(x => x.PreRequisito).Column("ID_TRILHANIVELPREREQ");
            References(x => x.CertificadoTemplate).Column("ID_CERTIFICADOTEMPLATE").Cascade.None();
            References(x => x.Monitor, "ID_UsuarioMonitor");
            References(x => x.TermoAceite, "ID_TermoAceite");
            
            Map(x => x.CargaHoraria).Column("QT_CargaHoraria");
            Map(x => x.Nome).Column("NM_TrilhaNivel");
            Map(x => x.QuantidadeDiasPrazo).Column("QT_DIASPRAZO");
            //Map(x => x.TextoTermoAceite).Column("TX_TermoAceite").CustomType("StringClob").CustomSqlType("nvarchar(max)");
            Map(x => x.NotaMinima).Column("VL_NotaMinima").CustomType("decimal(4,2)");
            Map(x => x.AceitaNovasMatriculas).Column("IN_ACEITANOVASMATRICULAS");
            Map(x => x.ValorOrdem).Column("VL_Ordem").Not.Nullable().Precision(3);
            Map(x => x.DataAlteracao).Column("DT_ULTIMAATUALIZACAO");
            Map(x => x.UsuarioAlteracao).Column("NM_USUARIOATUALIZACAO");
            Map(x => x.Descricao).Column("TX_Descricao");
            Map(x => x.AvisarMonitor).Column("IN_AvisarMonitor");
            Map(x => x.PrazoMonitorDiasUteis).Column("QT_DiasPrazoMonitor").Precision(3);
            Map(x => x.LimiteCancelamento).Column("QT_LimiteCancelamento");
            Map(x => x.ValorPrataPorOuro).Column("VL_PrataPorOuro");
            Map(x => x.QuantidadeMoedasProvaFinal).Column("QT_MoedasProvaFinal");

            Map(x => x.PorcentagensTrofeus).Column("TX_PorcentagensTrofeus");
            

            Map(x => x.QuantidadeMoedasPorCurtida).Column("QT_MoedasPorCurtida");
            Map(x => x.QuantidadeMoedasPorDescurtida).Column("QT_MoedasPorDescurtida");

            Map(x => x.Mapa).Column("VL_Mapa").CustomType<enumTrilhaMapa>();
            
            HasMany(x => x.ListaItemTrilha).KeyColumn("ID_TRILHANIVEL").AsBag().Inverse() 
                .Cascade.All().NotFound.Ignore();

            HasMany(x => x.ListaPreRequisito).KeyColumn("ID_TRILHANIVELPREREQ").AsBag().Inverse() 
                .Cascade.SaveUpdate(); 

            HasMany(x => x.ListaUsuarioTrilha).KeyColumn("ID_TRILHANIVEL").AsBag().Inverse()
                .Cascade.None().NotFound.Ignore(); 

            HasMany(x => x.ListaPermissao).KeyColumn("ID_TrilhaNivel").AsBag().Inverse()
                .Cascade.AllDeleteOrphan().NotFound.Ignore();

            HasMany(x => x.ListaQuestionarioAssociacao).KeyColumn("ID_TrilhaNivel").AsBag().Inverse()
                .Cascade.AllDeleteOrphan();

            HasMany(x => x.ListaQuestionarioParticipacao).KeyColumn("ID_TrilhaNivel").AsBag().Inverse()
                .Cascade.None().NotFound.Ignore();

            HasMany(x => x.ListaPontoSebrae).KeyColumn("ID_TrilhaNivel").AsBag().Inverse()
                .Cascade.None().NotFound.Ignore();



        }
    }
}
