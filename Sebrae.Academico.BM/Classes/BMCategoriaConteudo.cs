using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;

namespace Sebrae.Academico.BM.Classes
{
    public class BMCategoriaConteudo : BusinessManagerBase, IDisposable
    {

        #region Atributos

        private RepositorioBase<CategoriaConteudo> repositorio;

        #endregion

        #region "Construtor"

        public BMCategoriaConteudo()
        {
            repositorio = new RepositorioBase<CategoriaConteudo>();
        }
        
        #endregion

        public void Incluir(CategoriaConteudo pCategoriaConteudo)
        {
            ValidarCategoriaConteudoInformada(pCategoriaConteudo);

            //Verifica se a solução educacional já existe
            bool existeRegistroCadastrado = false;// this.VerificarExistenciaDaSolucaoEducacional(pCategoriaConteudo.Nome);

            if (existeRegistroCadastrado)
            {
                throw new AcademicoException(string.Format("A Categoria {0} já existe",
                                                           pCategoriaConteudo.Nome));
            }

            //pCategoriaConteudo.DataAlteracao = DateTime.Now;

            repositorio.Salvar(pCategoriaConteudo);
        }

        private bool VerificarExistenciaDaSolucaoEducacional(string pCategoriaConteudo)
        {
            bool existeRegistro = false;
            var query = repositorio.session.Query<CategoriaConteudo>();
            existeRegistro = query.Any(x => x.Nome.Trim().ToUpper() == pCategoriaConteudo.Trim().ToUpper());
            return existeRegistro;
        }

        public void Alterar(CategoriaConteudo pCategoriaConteudo)
        {
            ValidarCategoriaConteudoInformada(pCategoriaConteudo);
            repositorio.Salvar(pCategoriaConteudo);
        }

        public IQueryable<CategoriaConteudo> ObterTodos()
        {
            return repositorio.session.Query<CategoriaConteudo>().OrderBy(x => x.Nome);
        }

        public IList<CategoriaConteudo> ObterTodosPorUf(Uf uf)
        {
            return repositorio.ObterTodos().Where(c => c.ListaCategoriaConteudoUF.Any(x => x.UF.ID == uf.ID)).ToList();
        }

        public CategoriaConteudo ObterPorID(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

        public void Excluir(CategoriaConteudo pCategoriaConteudo)
        {
            var termoAceite = new BMTermoAceite().ObterPorCategoriaConteudo(pCategoriaConteudo.ID);

            if (termoAceite != null)
            {
                throw new AcademicoException("Exclusão de registro negada. Existe um termo de aceite vinculado a categoria conteúdo.");
            }

            if (this.ValidarDependencias(pCategoriaConteudo))
                throw new AcademicoException("Exclusão de registro negada. Existem Registros Dependentes desta Categoria de Solução Educacional.");

            if (pCategoriaConteudo.TermoAceiteCategoriaCounteudo != null)
                new BMTermoAceite().ExcluirTermoAceiteCategoriaConteudo(pCategoriaConteudo.TermoAceiteCategoriaCounteudo.ID);

            repositorio.Excluir(pCategoriaConteudo);
        }

        protected override bool ValidarDependencias(object pCategoriaConteudo)
        {
            CategoriaConteudo CategoriaConteudo = (CategoriaConteudo)pCategoriaConteudo;

            return (CategoriaConteudo.ListaSolucaoEducacional != null && CategoriaConteudo.ListaSolucaoEducacional.Count > 0);
        }

        public IList<CategoriaConteudo> ObterPorFiltro(CategoriaConteudo pCategoriaConteudo)
        {
            var query = repositorio.session.Query<CategoriaConteudo>();

            if (pCategoriaConteudo != null)
            {
                if (!string.IsNullOrWhiteSpace(pCategoriaConteudo.Nome))
                    query = query.Where(x => x.Nome.Contains(pCategoriaConteudo.Nome));
            }

            return query.Select(x => new CategoriaConteudo() { ID = x.ID, Nome = x.Nome }).ToList<CategoriaConteudo>();
        }


        private void ValidarCategoriaConteudoInformada(CategoriaConteudo pCategoriaConteudo)
        {
            ValidarInstancia(pCategoriaConteudo);

            if (string.IsNullOrWhiteSpace(pCategoriaConteudo.Nome)) throw new AcademicoException("Nome. Campo Obrigatório");

            if (pCategoriaConteudo.PossuiFiltroCategorias &&
                (pCategoriaConteudo.ListaStatusMatricula == null || !pCategoriaConteudo.ListaStatusMatricula.Any()))
                throw new AcademicoException(
                    "Pelo menos 1 status precisa ser informado caso a opção \"Possui Status relacionados?\" esteja selecionada.");
        }

        public CategoriaConteudo ObterPorNome(string pNome)
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<CategoriaConteudo>();
            return query.FirstOrDefault(x => x.Nome.ToLower() == pNome.ToLower());
        }

        public CategoriaConteudo ObterPorIdNode(int pIdNode)
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<CategoriaConteudo>();
            return query.FirstOrDefault(x => x.IdNode == pIdNode);
        }

        /// <summary>
        /// Obtém a maior categoria pai que possua filtro de Satus ou a própria categoria, caso não possua pai.
        /// </summary>
        /// <param name="categoriaConteudo"></param>
        /// <returns></returns>
        public CategoriaConteudo ObterMaiorCategoriaComFiltroCategoria(CategoriaConteudo categoriaConteudo)
        {
            var pais = ObterPais(categoriaConteudo);

            // Como foi implementado, a primeira categoria da lista ObterPais() sempre será o maior pai.
            var maiorPai = pais.FirstOrDefault(x => x.PossuiFiltroCategorias);

            return maiorPai ?? categoriaConteudo;
        }

        private IList<CategoriaConteudo> ObterPais(CategoriaConteudo categoriaConteudo)
        {
            var retorno = new List<CategoriaConteudo>();

            if (categoriaConteudo.CategoriaConteudoPai != null)
            {
                retorno.AddRange(ObterPais(categoriaConteudo.CategoriaConteudoPai));
                retorno.Add(categoriaConteudo);
            }
            else
            {
                retorno.Add(categoriaConteudo);
            }

            return retorno.Distinct().ToList();
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }
    }
}

