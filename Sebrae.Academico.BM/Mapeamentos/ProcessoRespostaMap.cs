using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class ProcessoRespostaMap : ClassMap<ProcessoResposta>
    {
        public ProcessoRespostaMap()
        {
            Table("TB_ProcessoResposta");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_ProcessoResposta");
            References(x => x.Processo).Column("ID_Processo");
            References(x => x.Usuario).Column("ID_Usuario");
            References(x => x.UsuarioCancelamento).Column("ID_UsuarioCancelamento");
            Map(x => x.JustificativaCancelamento).Column("NM_JustificativaCancelamento");
            Map(x => x.Concluido).Column("IN_Concluido");
            Map(x => x.DataSolicitacao).Column("DT_DataSolicitacao");
            
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
            Map(x => x.Status).Column("IN_Status").CustomType<enumStatusProcessoResposta>();
            HasMany(x => x.ListaEtapaResposta).KeyColumn("ID_ProcessoResposta").AsBag().Inverse().Cascade.None();
            Map(x => x.VersaoEtapa).Column("VL_Versao");
        }
    }
}