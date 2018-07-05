using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{

    public sealed class TrilhaTopicoTematicoMap : ClassMap<TrilhaTopicoTematico>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public TrilhaTopicoTematicoMap()
        {

            Table("TB_TrilhaTopicoTematico");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_TrilhaTopicoTematico");
            Map(x => x.Nome).Column("NM_TrilhaTopicoTematico");
            Map(x => x.DescricaoTextoEnvio).Column("DE_TextoEnvio");
            Map(x => x.DescricaoArquivoEnvio).Column("DE_ArquivoEnvio");
            Map(x => x.QtdMinimaPontosAtivFormativa).Column("QT_MinimoPontos");
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
            Map(x => x.NomeExibicao).Column("NM_Exibicao");

            HasMany(x => x.ListaItemTrilha).KeyColumn("ID_TRILHATOPICOTEMATICO").AsBag().Inverse() //.AsBag().Inverse().Cascade.None();
               .LazyLoad().Cascade.AllDeleteOrphan();

            References(x => x.FileServer).Column("ID_FileServer").Cascade.All();

            HasMany(x => x.ListaTrilhaAtividadeFormativaParticipacao).KeyColumn("ID_TrilhaTopicoTematico");
            HasMany(x => x.ListaTrilhaTopicoTematicoParticipacao).KeyColumn("ID_TrilhaTopicoTematico").AsBag().Inverse().Cascade.None();

            //HasMany(x => x.ListaTrilhaAtividadeFormativaParticipacao).KeyColumn("ID_TRILHATOPICOTEMATICO").AsBag().Inverse().Cascade.None();
            //References(x => x.TrilhaAtividadeFormativaParticipacao).Cascade.None().Column("ID_TrilhaTopicoTematico");

        }

    }
}
