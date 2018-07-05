using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class OrcamentoReembolsoMap : ClassMap<OrcamentoReembolso>
    {
        public OrcamentoReembolsoMap()
        {
            Table("TB_OrcamentoReembolso");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_OrcamentoReembolso");

            Map(x => x.Ano).Column("VL_Ano");
            Map(x => x.Orcamento).Column("VL_Orcamento");
            
            HasMany(x => x.ListaCampos)
                .KeyColumn("ID_OrcamentoReembolso")
                .AsBag()
                .Inverse()
                .Cascade
                .None();
        }
    }
}
