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
    public class BMUsuarioTrilhaMoedas : BusinessManagerBase, IDisposable
    {
        #region "Atributos Privados"

        private RepositorioBase<UsuarioTrilhaMoedas> repositorio = null;

        #endregion

        #region "Construtor"

        public BMUsuarioTrilhaMoedas()
        {
            repositorio = new RepositorioBase<UsuarioTrilhaMoedas>();
        }

        #endregion

        #region "Métodos Privados"

        /// <summary>
        /// Validação das informações de um Item de uma Trilha.
        /// </summary>
        /// <param name="pUsuarioTrilhaMoedas"></param>
        private void ValidarInformado(UsuarioTrilhaMoedas pUsuarioTrilhaMoedas)
        {
            ValidarInstancia(pUsuarioTrilhaMoedas);
            
            //Verifica se o usuario da trilha está nulo
            if (pUsuarioTrilhaMoedas.UsuarioTrilha == null) throw new AcademicoException("Usuario da Trilha. Campo Obrigatório");
        }

        #endregion

        #region "Métodos Públicos"

        public void Salvar(UsuarioTrilhaMoedas pUsuarioTrilhaMoedas)
        {
            ValidarInformado(pUsuarioTrilhaMoedas);

            //Se Id =0, significa insert.
            if (pUsuarioTrilhaMoedas.ID == 0)
                pUsuarioTrilhaMoedas.DataCriacao = DateTime.Now;

            repositorio.Salvar(pUsuarioTrilhaMoedas);
        }
        
        /// <summary>
        /// Obtem tudo
        /// </summary>
        /// <returns></returns>
        public IQueryable<UsuarioTrilhaMoedas> ObterTodos()
        {
            return repositorio.ObterTodosIQueryable();
        }

        /// <summary>
        /// Obtem todo o histórico de acordo com o usuario trilha
        /// </summary>
        /// <param name="pUsuarioTrilha"></param>
        /// <returns></returns>
        public IQueryable<UsuarioTrilhaMoedas> ObterTodos(UsuarioTrilha pUsuarioTrilha)
        {
            return repositorio.ObterTodosIQueryable().Where(x => x.UsuarioTrilha.ID == pUsuarioTrilha.ID);
        }

        /// <summary>
        /// Obtem todo o histórico do nivel de acordo com o usuario trilha
        /// </summary>
        /// <param name="pUsuarioTrilha"></param>
        /// <returns></returns>
        public IQueryable<UsuarioTrilhaMoedas> ObterTodos(UsuarioTrilha pUsuarioTrilha, TrilhaNivel pNivel)
        {
            return repositorio.ObterTodosIQueryable().Where(x => x.UsuarioTrilha.ID == pUsuarioTrilha.ID && x.UsuarioTrilha.TrilhaNivel.ID == pNivel.ID);
        }

        /// <summary>
        /// Obtem todo o histórico de acordo com as curtidas
        /// </summary>
        /// <param name="pItemTrilhaCurtida"></param>
        /// <returns></returns>
        public IQueryable<UsuarioTrilhaMoedas> ObterTodos(ItemTrilhaCurtida pItemTrilhaCurtida)
        {
            return repositorio.ObterTodosIQueryable().Where(x => x.Curtida.ID == pItemTrilhaCurtida.ID);
        }

        /// <summary>
        /// Obtem todo o histórico de acordo com as soluções sebrae
        /// </summary>
        /// <param name="pItemTrilha"></param>
        /// <returns></returns>
        public IQueryable<UsuarioTrilhaMoedas> ObterTodos(ItemTrilha pItemTrilha)
        {
            return repositorio.ObterTodosIQueryable().Where(x => x.ItemTrilha.ID == pItemTrilha.ID);
        }

        /// <summary>
        /// Obtem histórico por ID
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        public UsuarioTrilhaMoedas Obter(int pId)
        {
            if (pId == 0)
                return null;
            var query = repositorio.session.Query<UsuarioTrilhaMoedas>();
            return query.FirstOrDefault(x => x.ID == pId);
        }
        
        public void Dispose()
        {
            GC.SuppressFinalize(repositorio);
            GC.Collect();
        }

        #endregion
        
        
    }
}
