using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class ProtocoloFileServerMap : ClassMap<ProtocoloFileServer>
    {
        public ProtocoloFileServerMap()
        {
            Table("TB_ProtocoloFileServer");
            Id(x => x.Id).GeneratedBy.Identity().Column("Id_ProtocoloFileServer");
            References(x => x.FileServer).Column("Id_FileServer").Cascade.All();
            References(x => x.Protocolo).Column("Id_Protocolo").Cascade.All();
            References(x => x.Usuario).Column("Id_Usuario").Cascade.All();
            Map(x => x.DataEnvio).Column("DT_Envio").Not.Nullable();
        }
    }
}
