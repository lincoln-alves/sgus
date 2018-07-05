using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Classes.Moodle;

namespace Sebrae.Academico.BM.Mapeamentos.Moodle
{
    public class InscricaoMap : ClassMap<Inscricao>
    {
        public InscricaoMap()
        {
            Table("mdl_enrol");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("id");
            Map(x => x.IDCurso).Column("courseid");
            Map(x => x.TipoInscricao).Column("enrol");
        }
    }
}
