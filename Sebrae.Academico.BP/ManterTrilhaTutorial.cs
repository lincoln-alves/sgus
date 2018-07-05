using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.BP.Auth;

namespace Sebrae.Academico.BP
{
    public class ManterTrilhaTutorial : BusinessProcessBase, IDisposable
    {
        #region "Atributos Privados"

        private BMTrilhaTutorial bmTrilhaTutorial;

        #endregion

        #region "Construtor"

        /// <summary>
        /// Método Construtor da classe
        /// </summary>
        public ManterTrilhaTutorial()
            : base()
        {
            bmTrilhaTutorial = new BMTrilhaTutorial();
        }


        #endregion

        #region "Métodos Públicos"

        public void IncluirTrilhaTutorial(TrilhaTutorial pTrilhaTutorial)
        {
            base.PreencherInformacoesDeAuditoria(pTrilhaTutorial);

            // Garante que será inserido no final da ordem
            pTrilhaTutorial = this.setAsLastOne(pTrilhaTutorial);

            bmTrilhaTutorial.Salvar(pTrilhaTutorial);
        }

        public dynamic obterTrilhaTutorialPorCategoriaIdPaginado(int categoriaId, int tutorialId, UserIdentity usuarioLogado)
        {
            return bmTrilhaTutorial.obterTrilhaTutorialPorCategoriaIdPaginado(categoriaId, tutorialId, usuarioLogado.Usuario.ID, usuarioLogado.Nivel);
        }

        public void AlterarTrilhaTutorial(TrilhaTutorial pTrilhaTutorial)
        {
            base.PreencherInformacoesDeAuditoria(pTrilhaTutorial);
            bmTrilhaTutorial.Salvar(pTrilhaTutorial);
        }

        public IList<TrilhaTutorial> ObterTodosTrilhaTutorials()
        {
            return bmTrilhaTutorial.ObterTodos();
        }

        public IQueryable<TrilhaTutorial> ObterTodosIQueryable()
        {
            return bmTrilhaTutorial.ObterTodosIQueryable();
        }

        public void MarcarLidoAcessoTutorial(UsuarioTrilha matricula, int categoria_id)
        {
            bmTrilhaTutorial.MarcarLidoAcessoTutorial(matricula, categoria_id);
        }

        public TrilhaTutorial ObterTrilhaTutorialPorID(int pId)
        {
            return bmTrilhaTutorial.ObterPorID(pId);
        }

        public void ExcluirTrilhaTutorial(int idTrilhaTutorial)
        {
            try
            {
                TrilhaTutorial TrilhaTutorial = null;

                if (idTrilhaTutorial > 0)
                {
                    TrilhaTutorial = bmTrilhaTutorial.ObterPorID(idTrilhaTutorial);
                }

                bmTrilhaTutorial.Excluir(TrilhaTutorial);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public IList<TrilhaTutorial> ObterTrilhaTutorialPorFiltro(TrilhaTutorial pTrilhaTutorial)
        {
            return bmTrilhaTutorial.ObterPorFiltro(pTrilhaTutorial);
        }        

        public void Dispose()
        {
            GC.Collect();
        }        

        public TrilhaTutorial setAsLastOne(TrilhaTutorial pTrilhaTutorial)
        {
            var lastOrder = bmTrilhaTutorial.ObterUltimaOrdemPorCategoria(pTrilhaTutorial);

            pTrilhaTutorial.Ordem = lastOrder + 1;

            return pTrilhaTutorial;
        }

        public void AlterarOrdemTrilhaTutorial(dynamic obj)
        {            
            // TODO: Verificar se realmente é necessário o update antes de realizá-lo
            foreach (var items in obj)
            {
                if(items != null) { 
                    foreach (var item in items) {                    
                        TrilhaTutorial model = this.bmTrilhaTutorial.ObterPorID(Convert.ToInt16(item["id"]));
                        model.Ordem = Convert.ToByte(item["ordem"]);
                        bmTrilhaTutorial.Salvar(model);
                    }
                }
            }
        }

        public TrilhaTutorialAcesso ObterTrilhaTutorialAcessoPorCategoria(UsuarioTrilha usuarioTrilha, enumCategoriaTrilhaTutorial categoriaId)
        {
            return bmTrilhaTutorial.ObterTrilhaTutorialAcessoPorCategoria(usuarioTrilha, categoriaId);
        }

        public bool TutorialLido(UsuarioTrilha usuarioTrilha, enumCategoriaTrilhaTutorial categoria)
        {
            return bmTrilhaTutorial.TutorialLido(usuarioTrilha, categoria);
        }


        #endregion

    }
}
