using Sebrae.Academico.Dominio.Classes.Views;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sebrae.Academico.BP.Services
{
    public class ViewUsuariosDemitidosServices : RepositorioBase<ViewUsuariosDemitidos>
    {

        public IList<ViewUsuariosDemitidos> ObterUsuariosDemitidosHoje()
        {
            var todos = ObterTodos();
            return todos.Where(x => x.DataDemissao.Date.Equals(DateTime.Now.Date)).ToList();
        }
    }
}
