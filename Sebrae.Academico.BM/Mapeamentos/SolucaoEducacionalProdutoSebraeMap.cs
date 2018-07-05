using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class SolucaoEducacionalProdutoSebraeMap : ClassMap<SolucaoEducacionalProdutoSebrae>
    {
        public SolucaoEducacionalProdutoSebraeMap()
        {
            Table("TB_SolucaoEducacionalProdutoSebrae");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_SolucaoEducacionalProdutoSebrae");
            References(x => x.ProdutoSebrae, "ID_ProdutoSebrae");
            References(x => x.SolucaoEducacional, "ID_SolucaoEducacional");
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
        }
    }
}
