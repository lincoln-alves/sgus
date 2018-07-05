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
    public class BMTrilha : BusinessManagerBase, IDisposable
    {

        #region Atributos

        private RepositorioBase<Trilha> repositorio;

        #endregion

        public void AbrirTransacao()
        {
            // repositorio.session.Transaction.Begin();
            repositorio.session.BeginTransaction();
            this.Transacao = new TransactionRequired(repositorio.session);
        }

        public void Commit()
        {
            repositorio.session.Transaction.Commit();
            this.Transacao.Commit();
            //repositorio.session.Clear();
        }

        public void RollBack()
        {
            repositorio.session.Transaction.Rollback();
            this.Transacao.RollbackTransaction();
            //repositorio.session.Clear();
        }

        #region "Construtor"


        public BMTrilha()
        {
            repositorio = new RepositorioBase<Trilha>();
        }

        #endregion

        public Trilha ObterPorId(int ID)
        {
            Trilha trilha = null;

            var query = repositorio.session.Query<Trilha>();
            trilha = query.FirstOrDefault(x => x.ID == ID);
            return trilha;
        }

        public IList<Trilha> BuscarporNome(Trilha trilha)
        {
            //return repositorio.GetByProperty("Nome", ptrilha.Nome);
            var query = repositorio.session.Query<Trilha>();
            return query.Where(x => x.Nome == trilha.Nome).ToList<Trilha>();
        }

        private void ValidarTrilhaInformada(Trilha trilha)
        {
            // Validando se a instância da trilha está nula.
            ValidarInstancia(trilha);

            //Validando se a trilha foi informada
            if (String.IsNullOrWhiteSpace(trilha.Nome))
                throw new AcademicoException("Nome. Campo Obrigatório");

            if (String.IsNullOrWhiteSpace(trilha.Descricao))
                throw new AcademicoException("Descrição. Campo Obrigatório");

            //if (trilha.CategoriaConteudo == null)
            //    throw new AcademicoException("A categoria do conteúdo é obrigatória");

            if (trilha.ListaTrilhaNivel != null && trilha.ListaTrilhaNivel.Count > 0)
            {
                foreach (TrilhaNivel trilhaNivel in trilha.ListaTrilhaNivel)
                {
                    this.ValidarTrilhaNivelInformado(trilhaNivel);
                }
            }
        }

        /// <summary>
        /// Validação das informações de um Nivel Ocupacional.
        /// </summary>
        /// <param name="pNivelOcupacional"></param>
        private void ValidarTrilhaNivelInformado(TrilhaNivel pTrilhaNivel)
        {
            ValidarInstancia(pTrilhaNivel);

            if (string.IsNullOrWhiteSpace(pTrilhaNivel.Nome)) throw new AcademicoException("Nome. Campo Obrigatório");

            //Verifica se a Trilha Nivel é Pré-requisito dele mesmo

            if (pTrilhaNivel.PreRequisito != null)
            {
                if (pTrilhaNivel.PreRequisito.ID != 0 && pTrilhaNivel.ID == pTrilhaNivel.PreRequisito.ID)
                    throw new AcademicoException(string.Format("Este Nível de Trilha {0} não pode ser Pré-requisito dele mesmo.", pTrilhaNivel.Nome));
            }

        }

        public void Salvar(Trilha pTrilha)
        {
            ValidarTrilhaInformada(pTrilha);
            repositorio.Salvar(pTrilha);
        }

        public void FazerMergeAoSalvar(Trilha pTrilha)
        {
            ValidarTrilhaInformada(pTrilha);
            repositorio.FazerMerge(pTrilha);
        }

        public IQueryable<Trilha> ObterTrilhasIQueryable()
        {
            return repositorio.ObterTodosIQueryable();
        }

        public IList<Trilha> ObterTrilhas()
        {
            return repositorio.ObterTodos().OrderBy(x => x.Nome).ToList<Trilha>();
        }

        public void Excluir(Trilha pTrilha)
        {
            if (this.ValidarDependencias(pTrilha))
                throw new AcademicoException("Exclusão de registro negada. Existem alunos matriculados na trilha.");

            repositorio.Excluir(pTrilha);
        }

        protected override bool ValidarDependencias(object pTrilha)
        {
            Trilha trilha = (Trilha)pTrilha;

            //Se o nível da trilha possuir usuários, retorna true
            foreach (var nivel in trilha.ListaTrilhaNivel)
            {
                if (nivel.ListaUsuarioTrilha.Count > 0)
                    return true;

                if (nivel.ListaItemTrilha.Count > 0)
                    return true;
            }

            return false;
        }

        public IList<Trilha> ObterTrilhaPorNome(string pNome)
        {
            return repositorio.LikeByProperty("Nome", pNome);
        }

        public IList<Trilha> ObterPorFiltro(Trilha pTrilha)
        {
            var query = repositorio.session.Query<Trilha>();

            if (pTrilha != null)
            {
                if (!string.IsNullOrWhiteSpace(pTrilha.Nome))
                    query = query.Where(x => x.Nome.Contains(pTrilha.Nome));
            }

            //query = query.Fetch(x => x.ListaTrilhaNivel).ThenFetch(x => x.ToList<TrilhaNivel>());
            return query.ToList<Trilha>();
        }

        // Retorna somente as trilhas que o usuário esteja matriculado
        public List<Trilha> ObterPorUsuario(int idUsuario)
        {

            List<Trilha> trilha = new List<Trilha>();

            var query = (from t in repositorio.session.Query<Trilha>()

                         join tn in repositorio.session.Query<TrilhaNivel>() on
                         t.ID equals tn.Trilha.ID

                         join u in repositorio.session.Query<UsuarioTrilha>() on
                         tn.ID equals u.TrilhaNivel.ID

                         where u.Usuario.ID.Equals(idUsuario)

                         select t).GroupBy(x => x.ID);

            IList<IGrouping<int, Trilha>> trilhas = query.ToList();

            if (trilhas.Count() > 0)
            {

                List<int> TrilhaIds = new List<int>();

                foreach (IGrouping<int, Trilha> t in trilhas)
                {
                    TrilhaIds.Add(t.Key);
                }

                var queryTrilhas = repositorio.session.Query<Trilha>();
                queryTrilhas = queryTrilhas.Where(x => TrilhaIds.Contains(x.ID));

                return queryTrilhas.ToList();

            }
            else
            {
                return trilha;
            }

        }

        public void LimparSessao()
        {
            repositorio.LimparSessao();
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public void Evict(Trilha pTrilha)
        {
            repositorio.Evict(pTrilha);
        }
    }
}
