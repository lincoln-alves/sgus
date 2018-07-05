using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Classes.Moodle;

namespace Sebrae.Academico.BM.Mapeamentos.Moodle
{
    public class CursoMap : ClassMap<Curso>
    {
        public CursoMap()
        {
            Table("mdl_course");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("id");
            Map(x => x.CodigoCategoria).Column("category");
            Map(x => x.NomeCompleto).Column("fullname");
            Map(x => x.Nome).Column("shortname");
        }
    }
}
