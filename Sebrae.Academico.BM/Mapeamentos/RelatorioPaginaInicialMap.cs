
using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class RelatorioPaginaInicialMap : ClassMap<RelatorioPaginaInicial>
    {
        public RelatorioPaginaInicialMap()
        {
            Table("TB_RelatorioPaginaInicial");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_RelatorioPaginaInicial");
            Map(d => d.Nome).Column("TX_Titulo");
            Map(d => d.Tag).Column("TX_TagRelatorio");
            Map(d => d.Funcao).Column("TX_Funcao");
            Map(d => d.TodosPerfis).Column("IN_TodosPerfis");
            Map(d => d.TodasUfs).Column("IN_TodasUfs");

            HasManyToMany(a => a.Perfis)
                .Table("TB_RelatorioPaginaInicialPerfil")
                .ParentKeyColumn("ID_RelatorioPaginaInicial")
                .ChildKeyColumn("ID_Perfil").Cascade.All();

            HasManyToMany(a => a.Ufs)
                .Table("TB_RelatorioPaginaInicialUf")
                .ParentKeyColumn("ID_RelatorioPaginaInicial")
                .ChildKeyColumn("ID_Uf").Cascade.All();
        }
    }
}
