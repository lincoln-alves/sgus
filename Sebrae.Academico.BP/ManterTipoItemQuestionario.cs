using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP
{
    public class ManterTipoItemQuestionario : BusinessProcessBase
    {
        #region "Atributos Privados"

        private BMTipoItemQuestionario bmTipoItemQuestionario = null;

        #endregion

        #region "Construtor"

        /// <summary>
        /// Método Construtor da classe
        /// </summary>
        public ManterTipoItemQuestionario()
            : base()
        {
            bmTipoItemQuestionario = new BMTipoItemQuestionario();
        }

        #endregion

        #region "Métodos Públicos"

        public void IncluirTipoItemQuestionario(TipoItemQuestionario pTipoItemQuestionario)
        {
            bmTipoItemQuestionario.Salvar(pTipoItemQuestionario);
        }

        public void AlterarTipoItemQuestionario(TipoItemQuestionario pTipoItemQuestionario)
        {
            bmTipoItemQuestionario.Salvar(pTipoItemQuestionario);
        }

        public IList<TipoItemQuestionario> ObterTodosTipoItemQuestionario()
        {
            return bmTipoItemQuestionario.ObterTodos();
        }

        public TipoItemQuestionario ObterTipoOfertaPorID(int pId)
        {
            return bmTipoItemQuestionario.ObterPorID(pId);
        }

        public void ExcluirTipoOferta(int IdTipoItemQuestionario)
        {
            try
            {
                TipoItemQuestionario tipoItemQuestionario = null;

                if (IdTipoItemQuestionario > 0)
                {
                    tipoItemQuestionario = bmTipoItemQuestionario.ObterPorID(IdTipoItemQuestionario);
                }

                bmTipoItemQuestionario.ExcluirTipoOferta(tipoItemQuestionario);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public IList<TipoItemQuestionario> ObterTipoOfertaPorFiltro(TipoItemQuestionario pTipoItemQuestionario)
        {
            return bmTipoItemQuestionario.ObterPorFiltro(pTipoItemQuestionario);
        }

        #endregion

        public TipoItemQuestionario ObterPorID(int tipoId)
        {
            return bmTipoItemQuestionario.ObterPorID(tipoId);
        }
    }
}
