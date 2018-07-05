using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class TrilhaAtividadeFormativaParticipacaoMap : ClassMap<TrilhaAtividadeFormativaParticipacao>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public TrilhaAtividadeFormativaParticipacaoMap()
        {
            Table("TB_TRILHAATIVIDADEFORMATIVAPARTICIPACAO");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_TRILHAATIVIDADEFORMATIVAPARTICIPACAO");
            References(x => x.UsuarioTrilha).Column("ID_USUARIOTRILHA").Fetch.Select();
            References(x => x.TrilhaTopicoTematico).Column("ID_TRILHATOPICOTEMATICO");
            References(x => x.Monitor).Column("ID_IdUsuarioMonitor");
            
            Map(x => x.TextoParticipacao).Column("TX_PARTICIPACAO");
            Map(x => x.DataEnvio).Column("DT_Envio").Not.Nullable();
            Map(x => x.DataPrazoAvaliacao).Column("DT_PrazoAvaliacao");
            //Map(x => x.Comentario).Column("TX_COMENTARIO");
            //Map(x => x.HabilitaReenvio).Column("IN_HABILITAREENVIO");
            References(x => x.FileServer).Column("ID_FileServer").Cascade.All();
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
            Map(x => x.Autorizado).Column("IN_Autorizado");
            Map(x => x.Visualizado).Column("IN_Visualizado");
            Map(x => x.TipoParticipacao, "TP_Participacao").CustomType<enumTipoParticipacaoTrilha>();

        }

    }
}
