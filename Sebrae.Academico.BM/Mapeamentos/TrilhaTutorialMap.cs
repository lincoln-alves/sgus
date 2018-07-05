using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using NHibernate.Type;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class TrilhaTutorialMap : ClassMap<TrilhaTutorial>
    {
        public TrilhaTutorialMap()
        {
            Table("TB_TrilhaTutorial");
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_TrilhaTutorial");
            Map(x => x.Nome).Column("NM_Nome");
            Map(x => x.Categoria).Column("VL_Categoria").CustomType<enumCategoriaTrilhaTutorial>();
            Map(x => x.Conteudo).Column("TX_Conteudo").CustomType("StringClob");
            Map(x => x.Ordem).Column("VL_Ordem");

            Map(x => x.UsuarioAlteracao, "NM_UsuarioAtualizacao");
            Map(x => x.DataAlteracao, "DT_UltimaAtualizacao");

            HasManyToMany(x => x.MensagensGuia).Table("TB_TrilhaTutorialGuia").ParentKeyColumn("ID_TrilhaTutorial").ChildKeyColumn("ID_MensagemGuia");
        }
    }
}
