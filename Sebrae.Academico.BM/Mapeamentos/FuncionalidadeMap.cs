using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class FuncionalidadeMap : ClassMap<Funcionalidade>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public FuncionalidadeMap()
        {
            Table("TB_Funcionalidade");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_FORNECEDOR");
            Map(x => x.Nome).Column("NM_Funcionalidade");
            Map(x => x.Link).Column("LK_Funcionalidade");
        }
    }
}

