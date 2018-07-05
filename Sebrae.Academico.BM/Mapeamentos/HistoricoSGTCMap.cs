using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class HistoricoSGTCMap: ClassMap<HistoricoSGTC>
    {
        public HistoricoSGTCMap()
        {
            Table("TB_HistoricoSGTC");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_HistoricoSGTC");
            References(x => x.Usuario).Column("ID_Usuario");
            Map(x => x.NomeSolucaoEducacional).Column("NM_SolucaoEducacional").Not.Nullable().Length(500);
            Map(x => x.IDChaveExterna).Column("ID_ChaveExterna").Not.Nullable().Precision(10);
            Map(x => x.DataConclusao).Column("DT_Conclusao").Not.Nullable();
            Map(x => x.CDCertificado).Column("CD_Certificado").Length(500);
        }
    }
}
