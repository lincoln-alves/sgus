using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class CargoMap : ClassMap<Cargo>
    {
        public CargoMap()
        {
            Table("TB_Cargo");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_Cargo");
            Map(x => x.Nome).Column("NM_Cargo");
            Map(x => x.Ativo).Column("IN_Ativo");
            Map(x => x.TipoCargo).Column("VL_TipoCargo").CustomType<EnumTipoCargo>();
            Map(x => x.Sigla).Column("VL_Sigla");
            Map(x => x.Ordem).Column("VL_Order");
            References(x => x.CargoPai).Column("ID_CargoPai");
            References(x => x.Uf).Column("ID_UF");

            HasMany(x => x.CargosFilhos).KeyColumn("ID_CargoPai").AsBag().Inverse().Cascade.All();
            HasMany(x => x.UsuariosCargos).KeyColumn("ID_Cargo").AsBag().Inverse();
        }
    }
}