using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class ProdutoSebraeMap : ClassMap<ProdutoSebrae>
    {
        public ProdutoSebraeMap()
        {
            Table("TB_ProdutoSebrae");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_ProdutoSebrae");
            Map(x => x.Nome).Column("NM_Nome");
            Map(x => x.DataAlteracao).Column("DT_Alteracao");
        }
    }
}
