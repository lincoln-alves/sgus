using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class ProgramaCategoriaConteudoMap: ClassMap<ProgramaCategoriaConteudo>
    {
        public ProgramaCategoriaConteudoMap()
        {
            Table("TB_ProgramaCategoriaConteudo");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_ProgramaCategoriaConteudo");
            References(x => x.CategoriaConteudo, "ID_CategoriaConteudo");
            References(x => x.Programa, "ID_Programa");
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
        }
    }
}
