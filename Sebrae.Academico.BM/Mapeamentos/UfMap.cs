using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class UfMap : ClassMap<Uf>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public UfMap()
        {
            Table("TB_UF");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Assigned().Column("ID_UF");
            Map(x => x.Sigla).Column("SG_UF").Nullable();
            Map(x => x.Nome).Column("NM_UF").Nullable();
			
            References(x => x.Regiao).Column("ID_Regiao");
			
            HasMany(x => x.ListaProgramaPermissao).KeyColumn("ID_UF").AsBag().Inverse();
            HasMany(x => x.ListaCategoriaConteudoUF).KeyColumn("ID_UF").Cascade.AllDeleteOrphan();
            HasMany(x => x.ListaNacionalizacaoUf).KeyColumn("ID_UF").AsBag().Inverse().Not.KeyNullable();
			HasMany(x => x.PublicosAlvos).KeyColumn("ID_UF");
			HasMany(x => x.ListaEtapaPermissao).KeyColumn("ID_PermissaoUF");
			
            HasManyToMany(a => a.ListaRelatoriosPaginaInicial)
                .Table("TB_RelatorioPaginaInicialUf")
                .ParentKeyColumn("ID_Uf")
                .ChildKeyColumn("ID_RelatorioPaginaInicial");
        }

    }
}
