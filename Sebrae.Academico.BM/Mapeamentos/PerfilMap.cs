using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{

    public sealed class PerfilMap : ClassMap<Perfil>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public PerfilMap()
        {
            Table("TB_PERFIL");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Assigned().Column("ID_PERFIL");
            Map(x => x.Nome).Column("NM_PERFIL").Not.Nullable();

            HasManyToMany(a => a.ListaRelatorioPaginaInicial)
                .Table("TB_RelatorioPaginaInicialPerfil")
                .ParentKeyColumn("ID_Perfil")
                .ChildKeyColumn("ID_RelatorioPaginaInicial");
        }
    }
}


