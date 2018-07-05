using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Classes.Moodle;

namespace Sebrae.Academico.BM.Mapeamentos.Moodle
{
    public class LogMoodleMap : ClassMap<Sebrae.Academico.Dominio.Classes.Moodle.LogMoodle>
    {
        public LogMoodleMap()
        {
            base.Table("mdl_log");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("id");
            Map(x => x.Tempo).Column("time");
            Map(x => x.Ip).Column("ip");
            Map(x => x.Acao).Column("action");
            Map(x => x.Module).Column("module");
            Map(x => x.Url).Column("url");
            Map(x => x.Info).Column("info");
            References(x => x.Curso).Column("course");
        }
    }
}
