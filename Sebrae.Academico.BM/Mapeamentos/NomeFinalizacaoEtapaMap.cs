using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class NomeFinalizacaoEtapaMap : ClassMap<NomeFinalizacaoEtapa>
    {
        public NomeFinalizacaoEtapaMap()
        {
            Table("TB_NomeFinalizacaoEtapa");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_NomeFinalizacaoEtapa");
            Map(x => x.Nome).Column("TX_Nome");
            HasMany(x => x.ListaEtapa).KeyColumn("ID_Etapa");
        }
    }
}
