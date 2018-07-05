using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class TrilhaFaqMap : ClassMap<TrilhaFaq>
    {
        public TrilhaFaqMap()
        {
            Table("TB_TrilhaFaq");
            LazyLoad();

            Id(x => x.ID).GeneratedBy.Identity().Column("ID_TrilhaFaq");
            Map(x => x.Nome).Column("NM_Nome");
            Map(x => x.Descricao).Column("NM_Descricao");

            References(x => x.Assunto).Column("ID_AssuntoTrilhaFaq");
        }
    }
}
