using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Classes.Moodle;

namespace Sebrae.Academico.BM.Mapeamentos.Moodle
{
    public sealed class UsuarioMoodleMap : ClassMap<UsuarioMoodle>
    {
        public UsuarioMoodleMap()
        {
            Table("mdl_user");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("id");
            Map(x => x.Usuario).Column("username");
            Map(x => x.Email).Column("email");
            Map(x => x.Senha).Column("password");
            Map(x => x.Nome).Column("firstname");
            Map(x => x.Sobrenome).Column("lastname");

            HasMany(x => x.ListaMoodleInscricoes).KeyColumn("userid").AsBag().Cascade.None();            
        }
    }
}
