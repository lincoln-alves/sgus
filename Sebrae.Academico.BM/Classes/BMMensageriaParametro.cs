using System.Collections.Generic;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMMensageriaParametro : BusinessManagerBase
    {
        #region "Atributos Privados"

        private RepositorioBase<MensageriaParametros> repositorio;

        #endregion

        #region "Construtor"

        public BMMensageriaParametro()
        {
            repositorio = new RepositorioBase<MensageriaParametros>();
        }

        #endregion

        #region "Métodos Públicos"

        public IList<MensageriaParametros> ObterTodos()
        {
            return repositorio.ObterTodos();
        }

        #endregion
    }
}
