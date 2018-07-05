using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class LogAcessoFuncionalidadeMap: ClassMap<LogAcessoFuncionalidade>
    {
        public LogAcessoFuncionalidadeMap()
        {
            Table("LG_AcessoFuncionalidade");
            LazyLoad();
            
            CompositeId().KeyProperty(x => x.IDUsuario, "ID_Usuario")
                         .KeyProperty(x => x.IDFuncionalidade, "ID_Funcionalidade")
                         .KeyProperty(x => x.DataAcesso, "DT_Acesso");
        }

    }
}
