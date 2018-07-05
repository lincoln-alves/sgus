using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class ItemTrilhaParticipacaoMap : ClassMap<ItemTrilhaParticipacao>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public ItemTrilhaParticipacaoMap()
        {
            Table("TB_ItemTrilhaParticipacao");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_ItemTrilhaParticipacao");
            References(x => x.UsuarioTrilha).Column("ID_UsuarioTrilha");
            References(x => x.Monitor).Column("ID_IdUsuarioMonitor");
            References(x => x.ItemTrilha).Column("ID_ItemTrilha");
            References(x => x.FileServer).Column("ID_FileServer").Cascade.All();

            Map(x => x.TextoParticipacao).Column("TX_Participacao");
            Map(x => x.Orientacao).Column("TX_Orientacao");
            Map(x => x.DataEnvio).Column("DT_ENVIO").Not.Nullable();
            Map(x => x.DataPrazoAvaliacao).Column("DT_PrazoAvaliacao");
            Map(x => x.TipoParticipacao, "TP_Participacao").CustomType<enumTipoParticipacaoTrilha>();
            
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
            Map(x => x.Autorizado).Column("IN_Autorizado");
            Map(x => x.Visualizado).Column("IN_Visualizado");

            References(x => x.QuestionarioParticipacao).Column("ID_QuestionarioParticipacao");
            References(x => x.MatriculaOferta).Column("ID_MatriculaOferta");
        }
    }
}