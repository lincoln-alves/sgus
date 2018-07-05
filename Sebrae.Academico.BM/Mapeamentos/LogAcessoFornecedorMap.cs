using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.Dominio.Classes;
using FluentNHibernate.Mapping;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class LogAcessoFornecedorMap: ClassMap<LogAcessoFornecedor>
    {
        public LogAcessoFornecedorMap()
        {
            Table("LG_AcessoFornecedor");
            LazyLoad();
            //Id(x => x.ID, "ID_AcessoFornecedor").GeneratedBy.Identity();
            CompositeId().KeyProperty(x => x.IDFornecedor, "ID_Fornecedor")
                         .KeyProperty(x => x.DataAcesso, "DT_Acesso");
            Map(x => x.Metodo, "NM_Metodo").Not.Nullable();
           // Map(x => x.DataAcesso, "DT_Acesso").Not.Nullable();
        }
    }
}
