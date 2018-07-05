using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class ModuloMap : ClassMap<Modulo>
    {
        public ModuloMap()
        {
            Table("TB_Modulo");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_Modulo");
            References(x => x.Capacitacao).Column("ID_Capacitacao");
            Map(x => x.Nome).Column("NM_Modulo");
            Map(x => x.Descricao).Column("TX_Descricao");
            Map(x => x.DataInicio).Column("DT_Inicio");
            Map(x => x.DataFim).Column("DT_Fim");
            References(x => x.Certificado).Column("ID_Certificado");

            HasMany(x => x.ListaSolucaoEducacional).KeyColumn("ID_Modulo").AsBag().Inverse()
                .LazyLoad().Cascade.All();

            HasMany(x => x.ListaModuloPai).KeyColumn("ID_ModuloFilho").AsBag().Inverse()
                .LazyLoad().Cascade.All();
        }
    }
}
