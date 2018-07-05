using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.BM.Mapeamentos.Procedures;

namespace Sebrae.Academico.BM.Classes
{

    public class BMTrilhaNivel : BusinessManagerBase, IDisposable
    {
        #region Atributos

        private RepositorioBase<TrilhaNivel> repositorio = null;

        #endregion

        #region Construtor

        public BMTrilhaNivel()
        {
            repositorio = new RepositorioBase<TrilhaNivel>();
        }

        #endregion

        public void Salvar(TrilhaNivel pTrilhaNivel)
        {
            ValidarTrilhaNivelInformado(pTrilhaNivel);
            repositorio.Salvar(pTrilhaNivel);
        }


        public IList<DTOTrilhaNivelPermissao> ObterListaDePermissoes(int idUsuario, bool superAcesso = false)
        {
            ProcTrilhaNivelPermissao procTrilhaNivelPermissao = new ProcTrilhaNivelPermissao();
            return procTrilhaNivelPermissao.Executar(idUsuario,superAcesso);
        }


        /// <summary>
        /// Validação das informações de um Nivel Ocupacional.
        /// </summary>
        /// <param name="pNivelOcupacional"></param>
        public void ValidarTrilhaNivelInformado(TrilhaNivel pTrilhaNivel)
        {
            ValidarInstancia(pTrilhaNivel);

            if (string.IsNullOrWhiteSpace(pTrilhaNivel.Nome)) throw new AcademicoException("Nome. Campo Obrigatório");

            //Verifica se a Trilha Nivel é Pré-requisito dele mesmo

            if (pTrilhaNivel.PreRequisito != null && pTrilhaNivel.ID == pTrilhaNivel.PreRequisito.ID)
                throw new AcademicoException(string.Format("Este Nível de Trilha {0} não pode ser Pré-requisito dele mesmo.", pTrilhaNivel.Nome));
        }

        public void Excluir(TrilhaNivel pTrilhaNivel)
        {
            if (this.ValidarDependencias(pTrilhaNivel))
                throw new AcademicoException("Exclusão de registro negada. Existem Registros Dependentes desta Trilha Nível.");

            repositorio.Excluir(pTrilhaNivel);
        }

        public IList<TrilhaNivel> ObterTodos()
        {
            return repositorio.ObterTodos().AsParallel().OrderBy(x => x.Nome).ToList<TrilhaNivel>();
        }


        public IList<TrilhaNivel> ObterPorTrilha(Trilha trilha)
        {
            return repositorio.session.Query<TrilhaNivel>().AsParallel().Where(x => x.Trilha.ID == trilha.ID).ToList();
        }

        public IList<TrilhaNivel> ObterPorTrilha(int id)
        {
            IList<TrilhaNivel> ListaTrilhaNivel = null;
            var query = repositorio.session.Query<TrilhaNivel>();
            ListaTrilhaNivel = query.AsParallel().Where(x => x.Trilha.ID == id).ToList<TrilhaNivel>();
            return ListaTrilhaNivel;
        }

        public TrilhaNivel ObterPorID(int pId)
        {
            return repositorio.session.Query<TrilhaNivel>().FirstOrDefault(x => x.ID == pId);
        }

        protected override bool ValidarDependencias(object pTrilhaNivel)
        {
            TrilhaNivel trilhaNivel = (TrilhaNivel)pTrilhaNivel;

            return ((trilhaNivel.ListaPreRequisito != null && trilhaNivel.ListaPreRequisito.Count > 0) ||
                    (trilhaNivel.ListaItemTrilha != null && trilhaNivel.ListaItemTrilha.Count > 0) ||
                    (trilhaNivel.ListaUsuarioTrilha != null && trilhaNivel.ListaUsuarioTrilha.Count > 0));
        }

        public IList<TrilhaNivel> ObterTrilhaNivelPreRequisito()
        {
            IList<TrilhaNivel> lstTrilha = (from l in repositorio.session.QueryOver<TrilhaNivel>()
                                            where l.PreRequisito != null
                                            select l.PreRequisito).List<TrilhaNivel>().Distinct().ToList<TrilhaNivel>();

            return lstTrilha;
        }

        public IList<TrilhaNivel> ObterPorFiltro(TrilhaNivel pTrilhaNivel)
        {
            var query = repositorio.session.Query<TrilhaNivel>();

            if (pTrilhaNivel != null)
            {
                if (!string.IsNullOrWhiteSpace(pTrilhaNivel.Nome))
                    query = query.Where(x => x.Nome.Contains(pTrilhaNivel.Nome));

                if (pTrilhaNivel.Trilha != null)
                    query = query.Where(x => x.Trilha.ID == pTrilhaNivel.Trilha.ID);
            }


            return query.ToList<TrilhaNivel>();
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.SuppressFinalize(this);

        }

        public bool ONivelDaTrilhaEhPreRequisito(int idTrilhaNivel)
        {
            var query = repositorio.session.Query<TrilhaNivel>();
            bool possuiPreRequisito = false;

            possuiPreRequisito = query.Any(x => x.PreRequisito.ID == idTrilhaNivel);
            return possuiPreRequisito;

        }

        public void Evict(TrilhaNivel pTrilhaNivel)
        {
            repositorio.Evict(pTrilhaNivel);
        }

        public IQueryable<TrilhaNivel> ObterTodosIQueryable()
        {
            return repositorio.ObterTodosIQueryable();
        }
    }
}
