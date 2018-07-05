using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class ProtocoloMap : ClassMap<Protocolo>
    {
        public ProtocoloMap()
        {
            Table("TB_Protocolo");
            LazyLoad();

            Id(x => x.ID).GeneratedBy.Identity().Column("ID_Protocolo");
            Map(x => x.Numero).Column("Numero");
            Map(x => x.Descricao).Column("NM_Descricao");
            Map(x => x.DataEnvio).Column("DT_DataEnvio");
            Map(x => x.DataRecebimento).Column("DT_DataRecebimento");
            Map(x => x.Despacho).Column("TX_Despacho");
            Map(x => x.DespachoReencaminhamento).Column("TX_DespachoReencaminhamento");
            Map(x => x.Arquivado).Column("IN_Arquivado");

            References(x => x.Remetente).Column("ID_UsuarioRementente");
            References(x => x.Destinatario).Column("ID_UsuarioDestinatario");
            References(x => x.UsuarioAssinatura).Column("ID_UsuarioAssinatura");
            HasMany(x => x.Anexos).KeyColumn("Id_Protocolo").AsBag().Inverse();
            References(x => x.ProtocoloPai).Column("ID_ProtocoloPai");
        }
    }
}
