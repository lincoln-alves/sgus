using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class OfertaTrancadaParaPaganteMap : ClassMap<OfertaTrancadaParaPagante>
    {
        public OfertaTrancadaParaPaganteMap()
        {
            Table("TB_OfertaTrancadaParaPagante");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_OfertaTrancadaParaPagante");
            References(x => x.Oferta).Column("ID_Oferta").Not.Nullable();
            References(x => x.NivelOcupacional).Column("ID_NivelOcupacional").Not.Nullable();
            Map(x => x.DataAlteracao).Column("DT_UltimaAlteracao");
        }
    }
}
