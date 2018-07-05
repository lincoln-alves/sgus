using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP
{
    public class ManterMissao : BusinessProcessBase, IDisposable
    {
        private readonly BMMissao _bmMissao;

        /// <summary>
        /// Método Construtor da classe
        /// </summary>
        public ManterMissao()
        {
            _bmMissao = new BMMissao();
        }

        public void IncluirMissao(Missao pMissao)
        {
            _bmMissao.Salvar(pMissao);
        }

        public void AlterarMissao(Missao pMissao)
        {
            _bmMissao.Salvar(pMissao);
        }

        public IList<Missao> ObterTodosMissaos()
        {
            return _bmMissao.ObterTodos();
        }

        public Missao ObterPorID(int pId)
        {
            return _bmMissao.ObterPorID(pId);
        }

        public void ExcluirMissao(int idMissao)
        {
            try
            {
                Missao missao = null;

                if (idMissao > 0)
                {
                    missao = _bmMissao.ObterPorID(idMissao);
                }

                _bmMissao.Excluir(missao);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public IList<Missao> ObterMissaoPorFiltro(Missao pMissao)
        {
            return _bmMissao.ObterPorFiltro(pMissao);
        }

        public Missao ObterPorTexto(string textoMissao)
        {
            if (string.IsNullOrWhiteSpace(textoMissao))
            {
                throw new AcademicoException("Informe o texto do Missao");
            }

            return _bmMissao.ObterPorTexto(textoMissao);
        }

        public void Dispose()
        {
            GC.Collect();
        }

        public IQueryable<Missao> ObterTodosIQueryable()
        {
            return _bmMissao.ObterTodosIQueryable();
        }

        public IQueryable<Missao> ObterPorPontoSebrae(PontoSebrae pontoSebrae)
        {
            return ObterTodosIQueryable().Where(x => x.PontoSebrae.ID == pontoSebrae.ID);
        }

        public IQueryable<Missao> ObterPorPontoSebraeTrilha(Trilha trilha)
        {
            return ObterTodosIQueryable().Where(x => x.PontoSebrae.TrilhaNivel.Trilha.ID == trilha.ID);
        }
    }
}