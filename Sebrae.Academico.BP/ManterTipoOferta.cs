using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP
{
    public class ManterTipoOferta : BusinessProcessBase
    {
        #region "Atributos Privados"

        private BMTipoOferta bmTipoOferta = null;

        #endregion

        #region "Construtor"

        /// <summary>
        /// Método Construtor da classe
        /// </summary>
        public ManterTipoOferta()
            : base()
        {
            bmTipoOferta = new BMTipoOferta();
        }

        #endregion

        #region "Métodos Públicos"

        public void IncluirTipoOferta(TipoOferta ptipoOferta)
        {
            bmTipoOferta.Salvar(ptipoOferta);
        }

        public void AlterarTipoOferta(TipoOferta ptipoOferta)
        {
            bmTipoOferta.Salvar(ptipoOferta);
        }

        public IList<TipoOferta> ObterTodosTiposDeOferta()
        {
            return bmTipoOferta.ObterTodos();
        }

        public TipoOferta ObterTipoOfertaPorID(int pId)
        {
            return bmTipoOferta.ObterPorID(pId);
        }

        public void ExcluirTipoOferta(int IdTipoOferta)
        {
            try
            {
                TipoOferta tipoOferta = null;

                if (IdTipoOferta > 0)
                {
                    tipoOferta = bmTipoOferta.ObterPorID(IdTipoOferta);
                }

                bmTipoOferta.Excluir(tipoOferta);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public IList<TipoOferta> ObterTipoOfertaPorFiltro(TipoOferta pTipoOferta)
        {
            return bmTipoOferta.ObterPorFiltro(pTipoOferta);
        }

        #endregion

    }
}
