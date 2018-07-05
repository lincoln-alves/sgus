using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class PosicaoDadoCertificadoCertameMap : ClassMap<PosicaoDadoCertificadoCertame>
    {
        public PosicaoDadoCertificadoCertameMap()
        {
            Table("TB_PosicaoDadoCertificadoCertame");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_PosicaoDadoCertificadoCertame");
            Map(x => x.Ano).Column("VL_Ano");
            Map(x => x.Dado).Column("VL_Dado");
            Map(x => x.X).Column("VL_X");
            Map(x => x.Y).Column("VL_Y");
        }
    }
}