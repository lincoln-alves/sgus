using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes.Moodle;

namespace Sebrae.Academico.BM.Mapeamentos.Moodle
{
    class GroupMap : ClassMap<GroupMoodle>
    {
        public GroupMap()
        {
            Table("mdl_groups");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("id");
            Map(x => x.CourseID).Column("courseid");
            Map(x => x.IdNumber).Column("idnumber");
            Map(x => x.Name).Column("name");

        }
    }
}
