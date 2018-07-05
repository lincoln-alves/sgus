using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class NomeReprovacaoEtapaMap : ClassMap<NomeReprovacaoEtapa>
    {
        public NomeReprovacaoEtapaMap()
        {
            Table("TB_NomeReprovacaoEtapa");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_NomeReprovacaoEtapa");
            Map(x => x.Nome).Column("TX_Nome");
            HasMany(x => x.ListaEtapa).KeyColumn("ID_Etapa");
        }
    }
}
