using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using AutoMapper;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP
{
    public class ManterPagina : BusinessProcessBase
    {
        private BMPagina bmPagina;

        public ManterPagina()
        {
            bmPagina = new BMPagina();
        }

        public void SalvarPagina(Pagina paginaFilho, Pagina paginaPai, bool replicarPosicoesFilhos = true)
        {
            PreencherInformacoesDeAuditoria(paginaFilho);
            PreencherInformacoesDeAuditoria(paginaPai);

            bmPagina.Salvar(paginaFilho, paginaPai, replicarPosicoesFilhos);
        }

        public Pagina ObterRoot()
        {
            return bmPagina.ObterRoot();
        }

        public void ExcluirPagina(int paginaId)
        {
            if (paginaId == 0)
                throw new AcademicoException("A página selecionada não existe.");

            var pagina = bmPagina.ObterPorId(paginaId);
            
            if (pagina == null)
                throw new AcademicoException("A página selecionada não existe.");

            if (pagina.IsPaginaInicial())
                throw new AcademicoException("A página selecionada é página inicial e não pode ser excluída.");

            if (pagina.PossuiFilho())
                throw new AcademicoException("A página selecionada possui filhos e não pode ser excluída.");

            ExcluirPagina(pagina);
        }

        public void ExcluirPagina(Pagina pagina)
        {
            if (pagina.Left == 1 || pagina.PossuiFilho() || (pagina.PaginaInicial.HasValue && pagina.PaginaInicial.Value))
                throw new AcademicoException("A página selecionada não pode ser excluída.");

            bmPagina.Excluir(pagina);
        }

        public IEnumerable<Pagina> ObterTodasPaginas(bool calcularProfundidade = false, bool exibirRoot = false, bool orderByPaginaInicial = true)
        {
            var lista = bmPagina.ObterTodos(calcularProfundidade);

            if (orderByPaginaInicial)
            {
                lista = lista.OrderByDescending(x => x.PaginaInicial.HasValue && x.PaginaInicial.Value);
            }

            return exibirRoot ? lista : lista.Where(x => x.Left != 1);
        }

        public Pagina ObterPaginaPorID(int paginaId, bool calcularProfundidade = false, bool exibirRoot = false)
        {
            return ObterTodasPaginas(calcularProfundidade, exibirRoot).FirstOrDefault(x => x.ID == paginaId);
        }

        public IEnumerable<Pagina> ObterPaginasPorPerfis(List<Perfil> perfis)
        {
            return bmPagina.ObterPaginasPorPerfis(perfis);
        }

        public Pagina ObterPaginaPorCaminhoRelativo(string paginaAtual)
        {
            return bmPagina.ObterPaginaPorCaminhoRelativo(paginaAtual);
        }

        public Pagina ObterPaginaInicial()
        {
            return bmPagina.ObterPaginaInicial();
        }

        public Pagina ObterMenuPai(Pagina pagina)
        {
            return bmPagina.ObterMenuPai(pagina);
        }

        public Pagina ObterPai(Pagina pagina, bool exibirRoot = false)
        {
            return bmPagina.ObterPai(pagina, exibirRoot);
        }

        public IEnumerable<Pagina> ObterFilhos(Pagina pai, List<Pagina> paginasOriginais = null)
        {
            var retorno = (paginasOriginais ?? ObterTodasPaginas(true)).Where(x => x.Left > pai.Left && x.Right < pai.Right);

            return retorno;
        }

        public void MoverPagina(int paginaId, int novoIndice)
        {
            var pagina = ObterPaginaPorID(paginaId, true);

            if (pagina.Profundidade != (int) enumTipoPagina.Pagina)
                throw new AcademicoException("Esta página não pode ser movida, tente novamente.");

            var agrupador = ObterPai(pagina);

            // Obter todos os filhos, inclusive a página movida, mas na sua posição original.
            var filhos = ObterFilhos(agrupador).Where(x => x.Profundidade == (int) enumTipoPagina.Pagina).ToList();

            AutoMapper.AutoMapperConfig.RegisterMappings();

            var filhosComSubpaginas = Mapper.Map<List<Pagina>>(ObterFilhos(agrupador));
            //var filhosComSubpaginas = ObterFilhos(agrupador).Select(x => new Pagina
            //{
            //    ID = x.ID,
            //    Nome = x.Nome,
            //    Left = x.Left,
            //    Right = x.Right
            //}).ToList();

            var indiceOriginal = filhos.Select(x => x.ID).ToList().IndexOf(paginaId);
            
            // Obter a direção do movimento.
            var direcaoMovimento = novoIndice > indiceOriginal ? enumDirecao.Direita : enumDirecao.Esquerda;

            int index;

            // Através da mágica dos índices, obtém somente as páginas que serão alteradas.
            var paginasParaMover =
                filhos.Where(
                    x =>
                        direcaoMovimento == enumDirecao.Direita
                            ? (index = filhos.Select(y => y.ID).ToList().IndexOf(x.ID)) > indiceOriginal &&
                              index <= novoIndice
                            : (index = filhos.Select(y => y.ID).ToList().IndexOf(x.ID)) < indiceOriginal &&
                              index >= novoIndice)
                    .ToList();

            // Fazer uma checagem final para ver se realmente existem páginas para mover.
            if (!paginasParaMover.Any())
                throw new AcademicoException("Movimento inválido. Não há mais páginas para mover.");

            // Obter o tamanho da página para deslocar as páginas que serão movidas com o espaço necessário.
            var tamanhoDirecao = ((pagina.Right - pagina.Left) + 1)*(direcaoMovimento == enumDirecao.Direita ? -1 : 1);

            // Obter o deslocamento da página sendo movida e das filhas dela. Tem que ser obtido antes de atualizar
            // as páginas sendo movidas para não alterar o resultado.
            var deslocamento = (paginasParaMover.Max(x => x.Right) - paginasParaMover.Min(x => x.Left) + 1)*
                               (direcaoMovimento == enumDirecao.Direita ? 1 : -1);

            foreach (var paginaMover in paginasParaMover)
            {
                // Mover os filhos antes do pai, pois a posição do pai seria alterada a não seria
                // mais possível obter os filhos.
                if (paginaMover.PossuiFilho())
                {
                    foreach (var filho in ObterFilhos(paginaMover, filhosComSubpaginas))
                    {
                        var filhoDb = ObterPaginaPorID(filho.ID);

                        filhoDb.Left += tamanhoDirecao;
                        filhoDb.Right += tamanhoDirecao;

                        // Shit just got serious.
                        SalvarPagina(filhoDb, paginaMover, false);
                    }
                }

                var paginaMoverDb = ObterPaginaPorID(paginaMover.ID);

                paginaMoverDb.Left += tamanhoDirecao;
                paginaMoverDb.Right += tamanhoDirecao;

                // Shit just got serious.
                SalvarPagina(paginaMoverDb, agrupador, false);
            }

            // Mover os filhos antes do pai, pois a posição do pai seria alterada a não seria
            // mais possível obter os filhos.
            if (pagina.PossuiFilho())
            {
                foreach (var filho in ObterFilhos(pagina, filhosComSubpaginas))
                {
                    var filhoDb = ObterPaginaPorID(filho.ID);

                    filhoDb.Left += deslocamento;
                    filhoDb.Right += deslocamento;

                    // Shit just got serious.
                    SalvarPagina(filhoDb, pagina, false);
                }
            }

            var paginaDb = ObterPaginaPorID(pagina.ID);

            paginaDb.Left += deslocamento;
            paginaDb.Right += deslocamento;

            // Shit just got serious.
            SalvarPagina(paginaDb, agrupador, false);
        }
    }
}
