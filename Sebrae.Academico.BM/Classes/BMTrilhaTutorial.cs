using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BM.Classes
{
    public class BMTrilhaTutorial : BusinessManagerBase, IDisposable
    {
        #region "Atributos Privados"

        private RepositorioBase<TrilhaTutorial> repositorio = null;
        private RepositorioBase<TrilhaTutorialAcesso> repositorioTrilhaTutorialAcesso = null;

        #endregion

        #region "Construtor"

        public BMTrilhaTutorial()
        {
            repositorio = new RepositorioBase<TrilhaTutorial>();
            repositorioTrilhaTutorialAcesso = new RepositorioBase<TrilhaTutorialAcesso>();
        }

        #endregion

        #region "Métodos Privados"

        /// <summary>
        /// Validação das informações de um TrilhaTutorial.
        /// </summary>
        /// <param name="pTrilhaTutorial"></param>
        private void ValidarTrilhaTutorialInformado(TrilhaTutorial pTrilhaTutorial)
        {
            ValidarInstancia(pTrilhaTutorial);

            //Verifica se o nome do TrilhaTutorial está nulo
            if (string.IsNullOrWhiteSpace(pTrilhaTutorial.Nome)) throw new AcademicoException("Nome. Campo Obrigatório");
        }

        #endregion

        #region "Métodos Públicos"

        public void Salvar(TrilhaTutorial pTrilhaTutorial)
        {
            ValidarTrilhaTutorialInformado(pTrilhaTutorial);            
            repositorio.Salvar(pTrilhaTutorial);
        }

        public void Salvar(TrilhaTutorialAcesso pTrilhaTutorialAcesso)
        {                        
            new RepositorioBase<TrilhaTutorialAcesso>().Salvar(pTrilhaTutorialAcesso);
        }

        public IList<TrilhaTutorial> ObterTodos()
        {
            return repositorio.ObterTodos().OrderBy(x => x.Nome).ToList<TrilhaTutorial>();
        }

        public IQueryable<TrilhaTutorial> ObterTodosIQueryable()
        {
            return repositorio.ObterTodos().OrderBy(x => x.Nome).AsQueryable();
        }

        public TrilhaTutorial ObterPorID(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

        public void Excluir(TrilhaTutorial pTrilhaTutorial)
        {

            this.ValidarInstancia(pTrilhaTutorial);

            if (this.ValidarDependencias(pTrilhaTutorial))
                throw new AcademicoException("Exclusão de registro negada. Existem Registros Dependentes deste TrilhaTutorial.");

            repositorio.Excluir(pTrilhaTutorial);
        }

        public void MarcarLidoAcessoTutorial(UsuarioTrilha matricula, int categoria_id)
        {
            var query = repositorio.session.Query<TrilhaTutorialAcesso>();

            var trilhaTutorialAcesso = query.Where(x => x.UsuarioTrilha == matricula && x.Categoria == ((enumCategoriaTrilhaTutorial)categoria_id)).FirstOrDefault();

            if(trilhaTutorialAcesso == null)
            {
                trilhaTutorialAcesso = new TrilhaTutorialAcesso()
                {
                    UsuarioTrilha = matricula,
                    Categoria = (enumCategoriaTrilhaTutorial)categoria_id
                };
                trilhaTutorialAcesso.Auditoria.UsuarioAuditoria = matricula.Usuario.CPF;
                this.Salvar(trilhaTutorialAcesso);
            }            
        }

        public IList<TrilhaTutorial> ObterPorFiltro(TrilhaTutorial pTrilhaTutorial)
        {
            var query = repositorio.session.Query<TrilhaTutorial>();

            if (pTrilhaTutorial != null)
            {
                if (!string.IsNullOrWhiteSpace(pTrilhaTutorial.Nome))
                    query = query.Where(x => x.Nome.Contains(pTrilhaTutorial.Nome));

                if ((int) pTrilhaTutorial.Categoria != 0) { 
                    query = query.Where(x => x.Categoria == pTrilhaTutorial.Categoria);
                }
            }

            return query.OrderBy(x => x.Categoria).OrderBy(x => x.Ordem).ToList<TrilhaTutorial>();
        }

        public int ObterUltimaOrdemPorCategoria(TrilhaTutorial trilhaTutorial) {
            var query = repositorio.session.Query<TrilhaTutorial>();

            var lastOne = query.Where(x => x.Categoria == trilhaTutorial.Categoria).OrderByDescending(x => x.Ordem).FirstOrDefault();

            return (lastOne != null) ? (int) lastOne.Ordem : 0;
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public dynamic obterTrilhaTutorialPorCategoriaIdPaginado(int categoriaId, int tutorialId, int userId, TrilhaNivel nivel)
        {
            var query = repositorio.session.Query<TrilhaTutorial>();

            query = query.Where(x => (int) x.Categoria == categoriaId);

            if(tutorialId != 0)
            {
                query = query.Where(x => x.ID == tutorialId);
            }

            var trilhaTutorial = query.OrderBy(x => x.Ordem).FirstOrDefault();

            // Preenche as HashTags com o conteúdo desejado
            trilhaTutorial.Conteudo = formataConteudo(trilhaTutorial.Conteudo, userId, nivel);

            var query_prev = repositorio.session.Query<TrilhaTutorial>();

            var trilhaTutorialPrev = query_prev.Where(x => (int)x.Categoria == categoriaId && x.Ordem < trilhaTutorial.Ordem).OrderByDescending(x => x.Ordem).FirstOrDefault();

            var query_next = repositorio.session.Query<TrilhaTutorial>();

            var trilhaTutorialNext = query_next.Where(x => (int)x.Categoria == categoriaId && x.Ordem > trilhaTutorial.Ordem).OrderBy(x => x.Ordem).FirstOrDefault();

            return new {
                Nome = trilhaTutorial.Nome,
                Conteudo = trilhaTutorial.Conteudo,
                Categoria = (int) trilhaTutorial.Categoria,
                previous = trilhaTutorialPrev == null ? -1 : trilhaTutorialPrev.ID,
                next     = trilhaTutorialNext == null ? -1 : trilhaTutorialNext.ID
            };
        }

        public TrilhaTutorialAcesso ObterTrilhaTutorialAcessoPorCategoria(UsuarioTrilha usuarioTrilha, enumCategoriaTrilhaTutorial categoriaId)
        {
            return
                repositorio.session.Query<TrilhaTutorialAcesso>()
                    .FirstOrDefault(x => x.UsuarioTrilha.ID == usuarioTrilha.ID && x.Categoria == categoriaId);
        }

        public bool TutorialLido(UsuarioTrilha usuarioTrilha, enumCategoriaTrilhaTutorial categoriaId)
        {
            return
                repositorioTrilhaTutorialAcesso.ObterTodosIQueryable()
                    .Where(x => x.UsuarioTrilha.ID == usuarioTrilha.ID && x.Categoria == categoriaId)
                    .Select(x => new {x.ID})
                    .Any();
        }

        #endregion

        public string formataConteudo(string text, int userId, TrilhaNivel nivel) {
            var bmUsuario = new BMUsuario();
            var usuario = bmUsuario.ObterPorId(userId);

            return text
                    .Replace("#ALUNO", usuario.Nome)
                    .Replace("#CPF", usuario.CPF)
                    .Replace("#EMAILALUNO", usuario.Email)
                    .Replace("#TRILHANOME", nivel.Nome)
                    .Replace("#TRILHA", nivel.Trilha.Nome);
        }


    }
}
