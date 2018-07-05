using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMPagina : BusinessManagerBase, IDisposable
    {

        private RepositorioBase<Pagina> repositorio;

        public BMPagina()
        {
            repositorio = new RepositorioBase<Pagina>();
        }

        public Pagina ObterPorId(int ID)
        {
            return repositorio.ObterPorID(ID);
        }

        public IEnumerable<Pagina> ObterTodos(bool calcularProfundidade = false)
        {
            if (!calcularProfundidade)
                return repositorio.ObterTodos().OrderBy(x => x.Left);

            var paginas = repositorio.ObterTodos();

            foreach (var pagina in paginas)
            {
                pagina.Profundidade = paginas.Count(x => x.Left < pagina.Left && x.Right > pagina.Right);
            }

            return paginas.OrderBy(x => x.Left);
        }

        public void Salvar(Pagina filho, Pagina pai, bool replicarPosicoesFilhos = true)
        {
            // #1758 - Alterado pelo cliente, que deseja que as sub-permissões sejam editáveis.
            if (pai.Left != 1)
                filho.ReplicarPermissao(pai);

            var paginas = ObterTodos().AsQueryable();

            // Verificar se o filho é um cadastro ou uma edição.
            if (filho.ID == 0)
            {
                // Setar LR da página filho
                filho.Left = pai.Right;
                filho.Right = filho.Left + 1;


                foreach (var pagina in paginas)
                {
                    if (pagina.Left == 1)
                    {
                        pagina.Right += 2;
                        continue;
                    }

                    if (pagina.Left >= filho.Right)
                        pagina.Left += 2;

                    if (pagina.Right >= filho.Left)
                        pagina.Right += 2;

                    repositorio.Salvar(pagina);
                }
            }

            if (replicarPosicoesFilhos)
            {
                var filhas = paginas.Where(p => p.IsFilhoDe(filho)).ToList();

                // Replicar as permissões.
                foreach (var pagina in filhas)
                {
                    // #1758 - Alterado pelo cliente, que deseja que as sub-permissões sejam editáveis.
                    pagina.ReplicarPermissao(filho);

                    repositorio.Salvar(pagina);
                }
            }

            // Atualizar página inicial.
            if (pai.Left == 1 && (filho.PaginaInicial.HasValue && filho.PaginaInicial.Value))
                ZerarPaginaInicial();

            repositorio.Salvar(filho);
        }

        public Pagina ObterRoot()
        {
            return repositorio.ObterTodos().FirstOrDefault(x => x.Left == 1);
        }

        private void ZerarPaginaInicial()
        {
            foreach (var pagina in ObterTodos().Where(x => x.PaginaInicial == true))
            {
                pagina.PaginaInicial = null;
                repositorio.Salvar(pagina);
            }
        }

        public void Excluir(Pagina pagina)
        {
            var paginas = ObterTodos();

            foreach (var p in paginas)
            {
                if (p.Left != 1 && p.Left > pagina.Left)
                    p.Left -= 2;

                if (p.Right > pagina.Right)
                    p.Right -= 2;

                repositorio.Salvar(p);
            }

            repositorio.Excluir(pagina);
        }

        private Pagina ObterPorID(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

        public Pagina ObterPorCaminhoRelativo(string caminhoRelativo)
        {
            var query = repositorio.session.Query<Pagina>();
            return query.FirstOrDefault(x => x.CaminhoRelativo == caminhoRelativo);
        }

        public IEnumerable<Pagina> ObterPaginasPorPerfis(List<Perfil> perfis)
        {
            return ObterTodos(true).Where(x => x.TodosPerfis || x.Perfis.Any(p => perfis.Any(p2 => p2.ID == p.ID))).OrderByDescending(p => p.PaginaInicial);
        }

        public Pagina ObterPaginaPorCaminhoRelativo(string paginaAtual)
        {
            return ObterTodos(true).FirstOrDefault(p => p.CaminhoRelativo != null && p.CaminhoRelativo.ToLower() == paginaAtual.ToLower());
        }

        public Pagina ObterPaginaInicial()
        {
            return ObterTodos().FirstOrDefault(p => p.PaginaInicial.HasValue && p.PaginaInicial.Value);
        }

        public Pagina ObterMenuPai(Pagina pagina)
        {
            return ObterTodos(true).FirstOrDefault(p => p.Left < pagina.Left && p.Right > pagina.Right && p.Profundidade == (int)enumTipoPagina.Menu);
        }

        public Pagina ObterPai(Pagina pagina, bool exibirRoot)
        {
            return ObterTodos(true).FirstOrDefault(p => p.Left < pagina.Left && p.Right > pagina.Right && p.Profundidade == pagina.Profundidade - 1
            && (exibirRoot || p.Left != 1 ));
        }

        public void Dispose()
        {
            if (repositorio != null)
            {
                GC.SuppressFinalize(repositorio);
            }
            GC.Collect();
        }
    }
}