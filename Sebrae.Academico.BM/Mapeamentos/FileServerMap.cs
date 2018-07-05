using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class FileServerMap : ClassMap<FileServer>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public FileServerMap()
        {
            Table("TB_FileServer");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_FileServer");
            Map(x => x.NomeDoArquivoOriginal).Column("NM_ArquivoOriginal").Not.Nullable();
            Map(x => x.TipoArquivo).Column("TP_ARQUIVO").Not.Nullable();
            Map(x => x.MediaServer).Column("IN_MEDIASERVER").Not.Nullable();
            Map(x => x.NomeDoArquivoNoServidor).Column("NM_ArquivoServidor").Not.Nullable();
            References(x => x.Uf).Column("ID_UF").Cascade.SaveUpdate().Nullable();
            References(x => x.ProtocoloFileServer).Column("ID_ProtocoloFileServer").Cascade.SaveUpdate().Nullable();

            HasMany(x => x.ListaItemTrilha).KeyColumn("ID_FILESERVER").AsBag().Inverse()
              .LazyLoad().Cascade.AllDeleteOrphan();

            HasMany(x => x.ListaUsuario).KeyColumn("ID_FILESERVER").AsBag().Inverse()
              .LazyLoad().Cascade.All(); //.CascadeçTrilha.AllDeleteOrphan();

            HasMany(x => x.ListaTrilhaAtividadeFormativaParticipacao).KeyColumn("ID_FILESERVER").AsBag().Inverse()
              .LazyLoad().Cascade.AllDeleteOrphan();

            HasMany(x => x.ListaItemTrilhaParticipacao).KeyColumn("ID_FILESERVER").AsBag().Inverse()
              .LazyLoad().Cascade.AllDeleteOrphan();

        }

    }
}