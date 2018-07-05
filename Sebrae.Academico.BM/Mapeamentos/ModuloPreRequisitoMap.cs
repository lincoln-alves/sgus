using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class ModuloPreRequisitoMap : ClassMap<ModuloPreRequisito>
    {
        public ModuloPreRequisitoMap()
        {
            Table("TB_ModuloPreRequisito");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_ModuloPreRequisito");
            References(x => x.ModuloPai).Column("ID_ModuloPai");
            References(x => x.ModuloFilho).Column("ID_ModuloFilho");
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
        }   
    }
}
