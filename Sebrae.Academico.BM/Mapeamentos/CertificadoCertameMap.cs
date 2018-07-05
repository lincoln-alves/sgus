using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class CertificadoCertameMap : ClassMap<CertificadoCertame>
    {
        public CertificadoCertameMap()
        {
            Table("TB_CertificadoCertame");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_CertificadoCertame");
            Map(x => x.Ano).Column("VL_Ano");
            Map(x => x.NomeCertificado).Column("TX_NomeCertificacao");
            Map(x => x.Data).Column("DT_Data").Nullable();
            References(x => x.Certificado).Column("ID_FileServer");
            
            HasMany(x => x.UsuariosCertificadosCertames).KeyColumn("ID_CertificadoCertame");
        }
    }
}