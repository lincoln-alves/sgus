using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class AreaTematicaPermissaoMap : ClassMap<AreaTematicaPermissao>
    {
        public AreaTematicaPermissaoMap()
        {
            Table("TB_AreaTematicaPermissao");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_AreaTematicaPermissao");
            References(x => x.AreaTematica).Column("ID_AreaTematica").LazyLoad();
            References(x => x.Uf).Column("ID_UF").LazyLoad();
            References(x => x.NivelOcupacional).Column("ID_NivelOcupacional").LazyLoad();
            References(x => x.Perfil).Column("ID_Perfil").LazyLoad();
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
        }
    }
}
