using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class TermoAceiteCategoriaConteudoMap : ClassMap<TermoAceiteCategoriaConteudo>
    {
        public TermoAceiteCategoriaConteudoMap()
        {
            Table("TB_TermoAceiteCategoriaConteudo");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_TermoAceiteCategoriaConteudo");
            References(x => x.CategoriaConteudo, "ID_CategoriaConteudo");
            References(x => x.TermoAceite, "ID_TermoAceite");
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
        }
    }
}
