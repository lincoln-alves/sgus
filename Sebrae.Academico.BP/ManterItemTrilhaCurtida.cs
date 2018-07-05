using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP
{
    public class ManterItemTrilhaCurtida : BusinessProcessBase
    {

        #region "Atributos Privados"

        private BMItemTrilhaCurtida bmItemTrilhaCurtida = new BMItemTrilhaCurtida();

        #endregion

        #region "Construtor"

        /// <summary>
        /// Método Construtor da classe
        /// </summary>
        public ManterItemTrilhaCurtida() : base() { }

        #endregion

        #region "Métodos Públicos"

        public void Incluir(ItemTrilhaCurtida pItemTrilhaCurtida)
        {
            bmItemTrilhaCurtida.Salvar(pItemTrilhaCurtida);
        }

        public void Alterar(ItemTrilhaCurtida pItemTrilhaCurtida)
        {
            base.PreencherInformacoesDeAuditoria(pItemTrilhaCurtida);
            Incluir(pItemTrilhaCurtida);
        }

        public IQueryable<ItemTrilhaCurtida> ObterTodos()
        {
            return bmItemTrilhaCurtida.ObterTodos();
        }

        public ItemTrilhaCurtida Obter(int pId)
        {
            return bmItemTrilhaCurtida.Obter(pId);
        }

        public void Excluir(int pId)
        {
            try
            {
                ItemTrilhaCurtida itemTrilhaCurtida = Obter(pId);
                bmItemTrilhaCurtida.Excluir(itemTrilhaCurtida);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public IEnumerable<ItemTrilhaCurtida> ObterCurtidasUsuario(ItemTrilha itemTrilha, UsuarioTrilha usuario)
        {
            return ObterTodos()
                    .Where(x => x.ItemTrilha != null && x.ItemTrilha.ID == itemTrilha.ID)
                    .GroupBy(x => x.UsuarioTrilha)
                    // Esse .ToList() é por causa de uma limitação do NHibernate com o GroupBy.
                    // Infelizmente é necessário.
                    .ToList()
                    .Select(x => x.LastOrDefault())
                    .ToList();
        }

        public int ObterTotalCurtidasUsuario(ItemTrilha itemTrilha, UsuarioTrilha usuario, enumTipoCurtida tipo)
        {
            return ObterTodos()
                    .Where(x => x.ItemTrilha != null && x.ItemTrilha.ID == itemTrilha.ID)
                    .GroupBy(x => x.UsuarioTrilha)
                    // Esse .ToList() é por causa de uma limitação do NHibernate com o GroupBy.
                    // Infelizmente é necessário.
                    .ToList()
                    .Select(x => x.LastOrDefault())
                    .ToList().Count(x => x.TipoCurtida == tipo);
        }

        #endregion

    }
}
