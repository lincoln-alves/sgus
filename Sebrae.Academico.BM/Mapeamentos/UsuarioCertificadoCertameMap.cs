using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class UsuarioCertificadoCertameMap : ClassMap<UsuarioCertificadoCertame>
    {
        public UsuarioCertificadoCertameMap()
        {
            Table("TB_UsuarioCertificadoCertame");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_UsuarioCertificadoCertame");
            Map(x => x.Chave).Column("VL_ChaveAutenticacao").Nullable();
            Map(x => x.DataDownload).Column("DT_DataDownload").Nullable();
            Map(x => x.DataDownloadBoletim).Column("DT_DataDownloadBoletim").Nullable();
            Map(x => x.Status).Column("VL_Status").CustomType<enumStatusUsuarioCertificadoCertame>();
            Map(x => x.Nota).Column("VL_Nota");
            Map(x => x.ArquivoBoletim).Column("VL_ArquivoBoletim");
            Map(x => x.Situacao).Column("VL_Situacao").CustomType<enumSituacaoUsuarioCertificadoCertame>(); 
            Map(x => x.Justificativa).Column("TX_Justificativa");
            Map(x => x.NumeroInscricao).Column("VL_Inscricao");

            References(x => x.Usuario).Column("ID_Usuario");
            References(x => x.CertificadoCertame).Column("ID_CertificadoCertame");
        }
    }
}