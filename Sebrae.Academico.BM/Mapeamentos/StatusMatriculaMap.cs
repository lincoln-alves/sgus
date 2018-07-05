using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class StatusMatriculaMap : ClassMap<StatusMatricula>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public StatusMatriculaMap()
        {
            Table("TB_STATUSMATRICULA");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Assigned().Column("ID_StatusMatricula");
            Map(x => x.Nome).Column("NM_STATUSMATRICULA");
            Map(x => x.Especifico).Column("IN_Especifico");

            HasManyToMany(a => a.ListaCategoriaConteudo)
                .Table("TB_CategoriaConteudoStatusMatricula")
                .ParentKeyColumn("ID_StatusMatricula")
                .ChildKeyColumn("ID_CategoriaConteudo");
        }
    }
}