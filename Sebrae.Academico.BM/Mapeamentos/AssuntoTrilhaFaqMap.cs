using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class AssuntoTrilhaFaqMap : ClassMap<AssuntoTrilhaFaq>
    {
        public AssuntoTrilhaFaqMap()
        {
            Table("TB_AssuntoTrilhaFaq");
            LazyLoad();

            Id(x => x.ID).GeneratedBy.Identity().Column("ID_AssuntoTrilhaFaq");
            Map(x => x.Nome).Column("NM_Nome");

            HasMany(x => x.ItensFaq).KeyColumn("ID_AssuntoTrilhaFaq").Cascade.All().Inverse();
        }
    }
}
