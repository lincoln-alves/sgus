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
    public class BMItemTrilha : BusinessManagerBase, IDisposable
    {
        #region "Atributos Privados"

        private RepositorioBase<ItemTrilha> repositorio = null;

        #endregion

        #region "Construtor"

        public BMItemTrilha()
        {
            repositorio = new RepositorioBase<ItemTrilha>();
        }

        #endregion

        #region "Métodos Privados"

        /// <summary>
        /// Validação das informações de um Item de uma Trilha.
        /// </summary>
        /// <param name="pItemTrilha"></param>
        private void ValidarItemTrilhaInformado(ItemTrilha pItemTrilha)
        {
            ValidarInstancia(pItemTrilha);

            //Verifica se o nome do item trilha está nulo
            if (string.IsNullOrWhiteSpace(pItemTrilha.Nome)) throw new AcademicoException("Nome. Campo Obrigatório");

            //Verifica se o Tópico Temático está nulo
            if (pItemTrilha.Missao == null) throw new AcademicoException("Missão. Campo Obrigatório");

            //Pontos
            if (pItemTrilha.QuantidadePontosParticipacao < 0) throw new AcademicoException("Valor Inválido para o Campo Pontos. Informe um Valor entre 1 e 9");

            //validando se a Forma de Aquisição foi Informada.
            if (pItemTrilha.FormaAquisicao == null)
                throw new AcademicoException("É necessário preencher uma Forma de Aquisição");
        }

        #endregion

        #region "Métodos Públicos"

        public void Salvar(ItemTrilha pItemTrilha)
        {
            ValidarItemTrilhaInformado(pItemTrilha);

            //Se Id =0, significa insert.
            if (pItemTrilha.ID == 0)
            {
                pItemTrilha.DataCriacao = DateTime.Now;
            }
            repositorio.Salvar(pItemTrilha);
        }

        public IQueryable<ItemTrilha> ObterTodos()
        {
            return repositorio.ObterTodos().OrderBy(x => x.Nome).AsQueryable();
        }

        public IQueryable<ItemTrilha> ObterTodosIQueryable()
        {
            return repositorio.ObterTodosIQueryable();
        }

        public ItemTrilha ObterPorID(int pId)
        {
            var query = repositorio.session.Query<ItemTrilha>();
            query = query.Fetch(x => x.FileServer);
            query = query.Fetch(x => x.SolucaoEducacional);

            return query.FirstOrDefault(x => x.ID == pId);
        }

        public void Excluir(ItemTrilha pItemTrilha)
        {

            this.ValidarInstancia(pItemTrilha);

            if (this.ValidarDependencias(pItemTrilha))
                throw new AcademicoException("Exclusão de registro negada. Existem Registros Dependentes deste Item de Trilha.");

            repositorio.Excluir(pItemTrilha);
        }

        public IQueryable<ItemTrilha> ObterPorFiltro(ItemTrilha pItemTrilha)
        {
            ValidarInstancia(pItemTrilha);

            var query = repositorio.session.Query<ItemTrilha>();

            query = query.Fetch(x => x.SolucaoEducacional);
            query = query.Fetch(x => x.Missao);
            query = query.Fetch(x => x.Usuario);

            if (pItemTrilha.Missao != null)
            {
                if (pItemTrilha.Missao.ID != 0)
                    query = query.Where(x => x.Missao.ID == pItemTrilha.Missao.ID);

                if (pItemTrilha.Missao.PontoSebrae != null)
                {
                    if(pItemTrilha.Missao.PontoSebrae.ID != 0)
                        query = query.Where(x => x.Missao.PontoSebrae.ID == pItemTrilha.Missao.PontoSebrae.ID);

                    if (pItemTrilha.Missao.PontoSebrae.TrilhaNivel != null)
                    {
                        if (pItemTrilha.Missao.PontoSebrae.TrilhaNivel.ID != 0)
                            query = query.Where(x => x.Missao.PontoSebrae.TrilhaNivel.ID == pItemTrilha.Missao.PontoSebrae.TrilhaNivel.ID);
                    }
                }
            }

            if (pItemTrilha.SolucaoEducacionalAtividade != null && pItemTrilha.SolucaoEducacionalAtividade.ID > 0)
                query = query.Where(x => x.SolucaoEducacionalAtividade.ID == pItemTrilha.SolucaoEducacionalAtividade.ID);

            if (pItemTrilha.FormaAquisicao != null && pItemTrilha.FormaAquisicao.ID > 0)
                query = query.Where(x => x.FormaAquisicao.ID == pItemTrilha.FormaAquisicao.ID);

            if (!string.IsNullOrWhiteSpace(pItemTrilha.Nome))
                query = query.Where(x => x.Nome.Trim().ToUpper().Contains(pItemTrilha.Nome.Trim().ToUpper()));

            if (pItemTrilha.Usuario != null && pItemTrilha.Usuario.ID > 0)
                query = query.Where(x => x.Usuario.ID == pItemTrilha.Usuario.ID);

            if (pItemTrilha.Ativo != null)
                query = query.Where(x => x.Ativo == pItemTrilha.Ativo);

            if (pItemTrilha.UsuarioAssociado != null)
                query = pItemTrilha.UsuarioAssociado.Value ? query.Where(x => x.Usuario != null) : query.Where(x => x.Usuario == null);

            return query.DistinctBy(x => x.ID).AsQueryable();
        }

        public IQueryable<ItemTrilha> ObterItemTrilhasPorNivelTrilhaObjetivo(int idObjetivo, int idTrilhaNivel)
        {         
            var query = repositorio.session.Query<ItemTrilha>();
            query = query.Where(x => x.Missao.ID == idObjetivo && x.Missao.PontoSebrae.TrilhaNivel.ID == idTrilhaNivel);
            return query.AsQueryable();
        }

        public bool ValidaItensPorTopicotematico(TrilhaNivel pTrilhaNivel, Usuario pUsuario, TrilhaTopicoTematico pTopicoTematico)
        {
            ICriteria criteria = repositorio.session.CreateCriteria<ItemTrilha>();
            criteria = criteria.Add(Expression.Eq("TrilhaNivel", pTrilhaNivel));
            criteria = criteria.Add(Expression.Eq("TrilhaTopicoTematico", pTopicoTematico));
            criteria = criteria.Add(Expression.IsNull("Usuario"));
            criteria = criteria.SetProjection(Projections.RowCount());

            int qtdSemUsuario = (int)criteria.List()[0];

            criteria = repositorio.session.CreateCriteria<ItemTrilha>();
            criteria = criteria.Add(Expression.Eq("TrilhaNivel", pTrilhaNivel));
            criteria = criteria.Add(Expression.Eq("TrilhaTopicoTematico", pTopicoTematico));
            criteria = criteria.Add(Expression.Eq("Usuario", pUsuario));
            criteria = criteria.SetProjection(Projections.RowCount());

            int qtdComUsuario = (int)criteria.List()[0];

            return !((qtdSemUsuario - qtdComUsuario) == 0);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(repositorio);
            GC.Collect();
        }

        #endregion

        #region "Métodos Protected"

        protected override bool ValidarDependencias(object pItemTrilha)
        {
            ItemTrilha itemTrilha = (ItemTrilha)pItemTrilha;
            return (itemTrilha.ListaItemTrilhaParticipacao != null && itemTrilha.ListaItemTrilhaParticipacao.Count > 0);
        }
        
        #endregion
        
    }
}
