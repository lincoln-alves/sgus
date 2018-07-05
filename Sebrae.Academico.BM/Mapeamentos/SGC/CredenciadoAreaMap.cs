using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes.SGC;

namespace Sebrae.Academico.BM.Mapeamentos.SGC
{
    public sealed class CredenciadoAreaMap : ClassMap<CredenciadoArea>
    {
        public CredenciadoAreaMap()
        {
            Table("TB_SGC_CredenciadoSubarea");
            LazyLoad();
            Id(x => x.ID).Column("ID");
            Map(x => x.CPF).Column("CPF");
            Map(x => x.CodigoVinculo).Column("CODSITUACAOVINCULO");
            Map(x => x.DescricaoVinculo).Column("DESCSITUACAOVINCULO");
            Map(x => x.CodigoNatureza).Column("CODNATUREZA");
            Map(x => x.DescricaoNatureza).Column("DESCNATUREZA");

            Map(x => x.Alteracao).Column("DATAALTERACAO");

            References(x => x.Area).Column("IDAREA");
            References(x => x.Subarea).Column("IDSUBAREA");
        }
    }
}
