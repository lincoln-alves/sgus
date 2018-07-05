using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class UsuarioCategoriaConteudoMap : ClassMap<UsuarioCategoriaConteudo>
    {
        public UsuarioCategoriaConteudoMap()
        {
            Table("TB_UsuarioCategoriaConteudo");
            Id(x => x.ID).Column("ID_UsuarioCategoria");
            LazyLoad();
            References(x => x.CategoriaConteudo).Column("ID_CategoriaConteudo");
            References(x => x.Usuario).Column("ID_Usuario");
        }
    }
}
