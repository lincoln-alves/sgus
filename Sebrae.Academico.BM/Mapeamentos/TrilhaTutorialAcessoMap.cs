using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class TrilhaTutorialAcessoMap : ClassMap<TrilhaTutorialAcesso>
    {
        public TrilhaTutorialAcessoMap()
        {
            Table("TB_TrilhaTutorialAcesso");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_TrilhaTutorialAcesso");
            References(x => x.UsuarioTrilha).Column("ID_UsuarioTrilha");
            Map(x => x.Categoria).Column("VL_Categoria").CustomType<enumCategoriaTrilhaTutorial>();            

            Map(x => x.UsuarioAlteracao, "NM_UsuarioAtualizacao");
            Map(x => x.DataAlteracao, "DT_UltimaAtualizacao");
        }
    }
}
