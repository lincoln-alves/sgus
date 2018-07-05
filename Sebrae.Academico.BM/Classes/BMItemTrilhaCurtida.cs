using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMItemTrilhaCurtida : BusinessManagerBase, IDisposable
    {
        #region "Atributos Privados"

        private RepositorioBase<ItemTrilhaCurtida> repositorio = null;

        #endregion

        #region "Construtor"

        public BMItemTrilhaCurtida()
        {
            repositorio = new RepositorioBase<ItemTrilhaCurtida>();
        }

        #endregion

        #region "Métodos Privados"

        /// <summary>
        /// Validação das informações de um Item de uma Trilha.
        /// </summary>
        /// <param name="pItemTrilha"></param>
        private void ValidarInformado(ItemTrilhaCurtida pItemTrilhaCurtida)
        {
            ValidarInstancia(pItemTrilhaCurtida);
            
            //Verifica se o item da trilha está nulo
            if (pItemTrilhaCurtida.ItemTrilha == null) throw new AcademicoException("Item da Trilha. Campo Obrigatório");

            //Verifica se o usuario da trilha está nulo
            if (pItemTrilhaCurtida.UsuarioTrilha == null) throw new AcademicoException("Usuario da Trilha. Campo Obrigatório");

            //Verifica se o valor da curtida está vazio
            if (pItemTrilhaCurtida.ValorCurtida == 0) throw new AcademicoException("Valor da Curtida. Campo Obrigatório");

            //Verifica se o valor da curtida está vazio
            if (pItemTrilhaCurtida.ValorDescurtida == 0) throw new AcademicoException("Valor da Descurtida. Campo Obrigatório");
        }

        #endregion

        #region "Métodos Públicos"

        public void Salvar(ItemTrilhaCurtida pItemTrilhaCurtida)
        {
            ValidarInformado(pItemTrilhaCurtida);

            //Se Id =0, significa insert.
            if (pItemTrilhaCurtida.ID == 0)
                pItemTrilhaCurtida.DataCriacao = DateTime.Now;

            repositorio.Salvar(pItemTrilhaCurtida);
        }
        
        public IQueryable<ItemTrilhaCurtida> ObterTodos()
        {
            return repositorio.ObterTodosIQueryable();
        }

        public ItemTrilhaCurtida Obter(int pId)
        {
            if (pId == 0)
                return null;
            var query = repositorio.session.Query<ItemTrilhaCurtida>();
            return query.FirstOrDefault(x => x.ID == pId);
        }

        public void Excluir(ItemTrilhaCurtida pItemTrilhaCurtida)
        {
            this.ValidarInstancia(pItemTrilhaCurtida);
            
            repositorio.Excluir(pItemTrilhaCurtida);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(repositorio);
            GC.Collect();
        }

        #endregion
        
        
    }
}
