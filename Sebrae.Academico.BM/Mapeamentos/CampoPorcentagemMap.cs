using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class CampoPorcentagemMap : ClassMap<CampoPorcentagem>
    {
        public CampoPorcentagemMap()
        {
            Table("TB_CampoPorcentagem");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_CampoPorcentagem");
            References(x => x.Campo).Column("ID_Campo");
            References(x => x.CampoRelacionado).Column("ID_CampoRelacionado");
            Map(x => x.UltimaAtualizacao).Column("DT_UltimaAtualizacao");
        }
    }
}
