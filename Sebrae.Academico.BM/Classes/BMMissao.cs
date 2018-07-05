using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMMissao : BusinessManagerBase, IDisposable
    {
        #region "Atributos Privados"

        private RepositorioBase<Missao> repositorio = null;

        #endregion

        #region "Construtor"

        public BMMissao()
        {
            repositorio = new RepositorioBase<Missao>();
        }

        #endregion

        #region "Métodos Privados"

        /// <summary>
        /// Validação das informações de um Missao.
        /// </summary>
        /// <param name="pMissao"></param>
        private void ValidarMissaoInformado(Missao pMissao)
        {
            ValidarInstancia(pMissao);

            //Verifica se o nome do Missao está nulo
            if (string.IsNullOrWhiteSpace(pMissao.Nome)) throw new AcademicoException("Nome. Campo Obrigatório");

        }

        #endregion

        #region "Métodos Públicos"

        public void Salvar(Missao pMissao)
        {
            ValidarMissaoInformado(pMissao);

            var MissaoDb = ObterPorTexto(pMissao.Nome);

            if (MissaoDb != null && pMissao.ID != MissaoDb.ID)
                throw new AcademicoException("Já existe outro Missao com este texto. Altere o texto e salve novamente.");

            repositorio.Salvar(pMissao);
        }

        // Retorna os Missaos de um nível de trilha
        public IQueryable<Missao> ObterMissaoPorTrilhaNivel(int trilhaNivelId)
        {
            return
                repositorio.session.Query<Missao>()
                    .Where(x => x.PontoSebrae.TrilhaNivel.ID == trilhaNivelId)
                    .AsQueryable();
        }

        public IList<Missao> ObterTodos()
        {
            return repositorio.ObterTodos().OrderBy(x => x.Nome).ToList<Missao>();
        }

        public Missao ObterPorID(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

        public void Excluir(Missao pMissao)
        {

            this.ValidarInstancia(pMissao);

            if (this.ValidarDependencias(pMissao))
                throw new AcademicoException("Exclusão de registro negada. Existem Registros Dependentes deste Missao.");

            repositorio.Excluir(pMissao);
        }

        public IList<Missao> ObterPorFiltro(Missao pMissao)
        {
            var query = repositorio.session.Query<Missao>();

            if (pMissao != null)
            {
                if (!string.IsNullOrWhiteSpace(pMissao.Nome))
                    query = query.Where(x => x.Nome.Contains(pMissao.Nome));
            }

            if (pMissao.PontoSebrae != null && pMissao.PontoSebrae.TrilhaNivel != null)
            {
                query = query.Where(x => x.PontoSebrae.TrilhaNivel.ID == pMissao.PontoSebrae.TrilhaNivel.ID);
            }

            return query.ToList<Missao>();
        }

        public Missao ObterPorTexto(string textoMissao)
        {
            var query = repositorio.session.Query<Missao>();
            Missao Missao = null;

            if (textoMissao != null)
            {
                Missao = query.FirstOrDefault(x => string.Equals(x.Nome.Trim(), textoMissao.Trim()));
            }

            return Missao;
        }


        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        #endregion

        #region "Métodos Protected"

        protected override bool ValidarDependencias(object pMissao)
        {
            Missao Missao = (Missao)pMissao;
            return (Missao.ListaItemTrilha.Count > 0);
        }

        #endregion

        public IQueryable<Missao> ObterTodosIQueryable()
        {
            return repositorio.ObterTodosIQueryable();
        }
    }
}
