using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BM.Mapeamentos.Moodle.Views
{
    public class ViewLogMoodleMap : ClassMap<ViewLogMoodle>
    {
        public ViewLogMoodleMap()
        {
            Table("VW_LogMoodle");
            ReadOnly();
            Id(x => x.ID);
            Map(x => x.Usuario).Column("Usuario");
            //Map(x => x.NomeUsuario).Column("Nome_Usuario");
            //Map(x => x.ID_Curso).Column("ID_Curso");
            //Map(x => x.Action).Column("Action");
            //Map(x => x.Module).Column("module");
            //Map(x => x.NomeOferta).Column("NM_Oferta");
            Map(x => x.ID_Oferta).Column("ID_Oferta");
        }
    }
}
