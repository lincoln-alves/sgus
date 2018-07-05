using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class EtapaVersaoMap : ClassMap<EtapaVersao>
    {
        public EtapaVersaoMap()
        {
            Table("TB_EtapaVersao");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_EtapaVersao");
            Map(x => x.Ordem).Column("VL_Ordem");
            Map(x => x.Versao).Column("VL_Versao");

            References(x => x.Etapa).Column("ID_Etapa");
        }
    }
}
