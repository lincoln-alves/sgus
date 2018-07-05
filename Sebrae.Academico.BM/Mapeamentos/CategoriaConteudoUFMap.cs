using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class CategoriaConteudoUFMap : ClassMap<CategoriaConteudoUF>
    {
        CategoriaConteudoUFMap()
        {
            Table("TB_CategoriaConteudoUF");
            LazyLoad();
            Id(x => x.ID).Column("ID_CategoriaConteudoUF");
            References(x => x.Categoria).Column("ID_CategoriaConteudo");
            References(x => x.UF).Column("ID_UF");
        }
    }
}
