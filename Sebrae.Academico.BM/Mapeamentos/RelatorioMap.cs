using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class RelatorioMap: ClassMap<Relatorio>
    {
        public RelatorioMap()
        {
            Table("TB_Relatorio");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_Relatorio");
            Map(x => x.Nome).Column("NM_Relatorio");
            Map(x => x.Link).Column("LK_Relatorio");
        }
    }
}
