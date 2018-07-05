using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Classes.Moodle;

namespace Sebrae.Academico.BM.Mapeamentos.Moodle
{
    public sealed class CategoriaMoodleMap : ClassMap<CategoriaMoodle>
    {
        public CategoriaMoodleMap()
        {
            Table("mdl_course_categories");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("id");          
            Map(x => x.Nome).Column("name");
            Map(x => x.IdNumber).Column("idnumber");
            Map(x => x.Descricao).Column("description");
            Map(x => x.DescricaoFormato).Column("descriptionformat");
            // Mapping only the most used properties
            
        }
    }
}
