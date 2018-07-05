using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class FornecedorUFMap : ClassMap<FornecedorUF>
    {
        public FornecedorUFMap()
        {
            Table("TB_FornecedorUF");
            LazyLoad();
            Id(x => x.ID).Column("ID_FornecedorUF");
            References(x => x.UF).Column("ID_UF");
            References(x => x.Fornecedor).Column("ID_Fornecedor");
        }
    }
}
