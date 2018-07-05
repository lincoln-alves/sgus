using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class HierarquiaNucleoUsuarioMap : ClassMap<HierarquiaNucleoUsuario>
    {
        public HierarquiaNucleoUsuarioMap()
        {
            Table("Tb_HierarquiaNucleoUsuario");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_HierarquiaNucleoUsuario");
            References(x => x.HierarquiaNucleo).Column("ID_HierarquiaNucleo").Not.LazyLoad();
            References(x => x.Usuario).Column("ID_Usuario");
            References(x => x.Uf).Column("ID_UF");
            Map(x => x.IsGestor).Column("IN_Gestor");
        }

    }
}
