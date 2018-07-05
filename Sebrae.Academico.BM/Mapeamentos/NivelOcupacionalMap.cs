using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class NivelOcupacionalMap : ClassMap<NivelOcupacional>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public NivelOcupacionalMap()
        {
            Table("TB_NIVELOCUPACIONAL");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Assigned().Column("ID_NIVELOCUPACIONAL");
            Map(x => x.Nome).Column("NM_NOME").Not.Nullable();
            HasMany(x => x.ListaEtapaPermissao).KeyColumn("ID_PermissaoOcupacao").Inverse();
        }
    }
}
