using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class InformeMap : ClassMap<Informe>
    {
        public InformeMap()
        {
            Table("TB_Informe");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_Informe");
            Map(x => x.Numero).Column("VL_Numero");
            Map(x => x.Mes).Column("VL_Mes");
            Map(x => x.Ano).Column("VL_Ano");

            HasManyToMany(x => x.Turmas)
                .Table("TB_InformeTurma")
                .ParentKeyColumn("ID_Informe")
                .ChildKeyColumn("ID_Turma");

            HasMany(x => x.Envios).KeyColumn("ID_Informe").Not.KeyNullable().Cascade.All().Inverse();
        }
    }
}
