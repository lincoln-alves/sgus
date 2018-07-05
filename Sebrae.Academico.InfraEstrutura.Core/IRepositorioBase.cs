using System.Collections.Generic;

namespace Sebrae.Academico.InfraEstrutura.Core
{
    public interface IRepositorioBase<T> where T : class 
    {
        #region "Operações"

        void Excluir(T objeto);
        T Salvar(T objeto);
        T ObterPorID(int id);
        IList<T> ListarTodos();

        #endregion

    }
}
