using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;


namespace Sebrae.Academico.BM.Classes
{
    public class BMStatusMatricula : BusinessManagerBase, IDisposable
    {

        #region Atributos

        private RepositorioBase<StatusMatricula> repositorio;

        #endregion

        #region "Construtor"


        public BMStatusMatricula()
        {
            repositorio = new RepositorioBase<StatusMatricula>();
        }

        #endregion

        public StatusMatricula ObterPorId(int ID)
        {
            return repositorio.ObterPorID(ID);
        }

        public void Salvar(StatusMatricula pStatusMatricula)
        {
            ValidarStatusMatriculaInformado(pStatusMatricula);
            repositorio.Salvar(pStatusMatricula);
        }

        private void ValidarStatusMatriculaInformado(StatusMatricula pStatusMatricula)
        {
            this.ValidarInstancia(pStatusMatricula);

            if (string.IsNullOrWhiteSpace(pStatusMatricula.Nome))
                throw new Exception("Nome Não Informado. Campo Obrigatório!");

            this.VerificarExistenciaDeStatusMatricula(pStatusMatricula);
        }

        private void VerificarExistenciaDeStatusMatricula(StatusMatricula pStatusMatricula)
        {
            StatusMatricula statusMatricula = this.ObterPorNome(pStatusMatricula);

            if (statusMatricula != null)
            {
                if (pStatusMatricula.ID != statusMatricula.ID)
                {
                    throw new AcademicoException(string.Format("O Status '{0}' já está cadastrado", pStatusMatricula.Nome.Trim()));
                }
            }
        }

        public StatusMatricula ObterPorNome(StatusMatricula pStatusMatricula)
        {
            //return repositorio.GetByProperty("Nome", pStatusMatricula.Nome).FirstOrDefault();
            var query = repositorio.session.Query<StatusMatricula>();
            return query.FirstOrDefault(x => x.Nome == pStatusMatricula.Nome);
        }


        public IList<StatusMatricula> ObterTodos()
        {
            return repositorio.ObterTodos().Where(f => f.Especifico == false).OrderBy(x => x.Nome).ToList<StatusMatricula>();
        }

        public IList<StatusMatricula> ObterTodosEspecificos()
        {
            return repositorio.ObterTodos().Where(f => f.Especifico == true).OrderBy(x => x.Nome).ToList<StatusMatricula>();
        }

        public IList<StatusMatricula> ObterTodosIncluindoEspecificos()
        {
            return repositorio.ObterTodos().OrderBy(x => x.Nome).ToList();
        }

        public IQueryable<StatusMatricula> ObterTodosIQueryable()
        {
            return repositorio.ObterTodosIQueryable().OrderBy(x => x.Nome);
        }

        public void Excluir(StatusMatricula pTrilha)
        {
            repositorio.Excluir(pTrilha);
        }

        public IList<StatusMatricula> ObterPorNome(string pNome)
        {
            return repositorio.LikeByProperty("Nome", pNome);
        }

        public IList<StatusMatricula> ObterStatusMatriculaDeTrilhas()
        {
            var query = repositorio.session.Query<StatusMatricula>();

            var listaStatusMatricula = query.Where(x => x.ID == (int) enumStatusMatricula.Inscrito
                                                        || x.ID == (int) enumStatusMatricula.Aprovado
                                                        || x.ID == (int) enumStatusMatricula.Abandono
                                                        || x.ID == (int) enumStatusMatricula.Concluido
                                                        || x.ID == (int)enumStatusMatricula.CanceladoAdm
                                                        || x.ID == (int)enumStatusMatricula.CanceladoAluno
                                                        || x.ID == (int) enumStatusMatricula.Reprovado).ToList();
            return listaStatusMatricula;
        }

        public IList<StatusMatricula> ObterStatusMatriculaPorCategoriaConteudo(CategoriaConteudo categoriaConteudo)
        {
            if (categoriaConteudo == null)
                return ObterTodos();

            if (categoriaConteudo.PossuiFiltroCategorias)
            {
                return
                    repositorio.session.Query<StatusMatricula>()
                        .Where(
                            s =>
                                s.ListaCategoriaConteudo.Any(c => c.ID == categoriaConteudo.ID))
                        .OrderBy(s => s.Nome)
                        .ToList();
            }

            if (categoriaConteudo.CategoriaConteudoPai != null)
            {
                return ObterStatusMatriculaPorCategoriaConteudo(categoriaConteudo.CategoriaConteudoPai);
            }

            return ObterTodos();
        }


        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }
    }
}
