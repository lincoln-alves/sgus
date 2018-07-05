using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BP
{
    public class ManterTipoQuestionario : BusinessProcessBase
    {
        #region "Atributos Privados"

        private BMTipoQuestionario tipoQuestionario;

        #endregion

        #region "Construtor"

        /// <summary>
        /// Método Construtor da classe
        /// </summary>
        public ManterTipoQuestionario()
            : base()
        {
            tipoQuestionario = new BMTipoQuestionario();
        }

        #endregion

        #region "Métodos Públicos"

        public void IncluirTipoQuestionario(TipoQuestionario pTipoQuestionario)
        {
            tipoQuestionario.Salvar(pTipoQuestionario);
        }

        public void AlterarTipoQuestionario(TipoQuestionario pTipoQuestionario)
        {
            tipoQuestionario.Salvar(pTipoQuestionario);
        }

        public IList<TipoQuestionario> ObterTodos()
        {
            return tipoQuestionario.ObterTodos();
        }

        public IQueryable<TipoQuestionario> ObterTodosIQueryable()
        {
            return tipoQuestionario.ObterTodosIQueryable();
        }

        public TipoQuestionario ObterTipoQuestionarioPorID(int pId)
        {
            return tipoQuestionario.ObterPorID(pId);
        }

        public IList<TipoQuestionario> ObterTipoQuestionarioPorFiltro(TipoQuestionario pTipoQuestionario)
        {
            return tipoQuestionario.ObterPorFiltro(pTipoQuestionario);
        }

        #endregion

    }
}
