using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Classes.Moodle;

namespace Sebrae.Academico.BM.Mapeamentos.Moodle
{
    public class UsuarioMoodleInscricaoMap : ClassMap<UsuarioMoodleInscricao>
    {
        public UsuarioMoodleInscricaoMap()
        {
            Table("mdl_user_enrolments");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("id");
            Map(x => x.Status).Column("status");
            Map(x => x.TempoInicio).Column("timestart");
            Map(x => x.TempoFim).Column("timeend");
            Map(x => x.IDModificador).Column("modifierid");
            Map(x => x.TempoCriacao).Column("timecreated");
            Map(x => x.TempoModificacao).Column("timemodified");

            References(x => x.UsuarioMoodle).Column("userid");
            References(x => x.Inscricao).Column("enrolid");
        }
    }
}
