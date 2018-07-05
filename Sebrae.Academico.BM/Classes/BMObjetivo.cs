using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMObjetivo : BusinessManagerBase, IDisposable
    {
        #region "Atributos Privados"

        private RepositorioBase<Objetivo> repositorio = null;

        #endregion

        #region "Construtor"

        public BMObjetivo()
        {
            repositorio = new RepositorioBase<Objetivo>();
        }

        #endregion

        #region "Métodos Privados"

        /// <summary>
        /// Validação das informações de um Objetivo.
        /// </summary>
        /// <param name="pObjetivo"></param>
        private void ValidarObjetivoInformado(Objetivo pObjetivo)
        {
            ValidarInstancia(pObjetivo);

            //Verifica se o nome do Objetivo está nulo
            if (string.IsNullOrWhiteSpace(pObjetivo.Nome)) throw new AcademicoException("Nome. Campo Obrigatório");

        }

        #endregion

        #region "Métodos Públicos"

        public void Salvar(Objetivo pObjetivo)
        {
            ValidarObjetivoInformado(pObjetivo);

            var objetivoDb = ObterPorTexto(pObjetivo.Nome);

            if (objetivoDb != null && pObjetivo.ID != objetivoDb.ID)
                throw new AcademicoException("Já existe outro objetivo com este texto. Altere o texto e salve novamente.");

            repositorio.Salvar(pObjetivo);
        }

        // Retorna os objetivos de um nível de trilha
        public IQueryable<Objetivo> ObterObjetivoPorTrilhaNivel(int idTrilhaNivel)
        {
            var query = repositorio.session.Query<Objetivo>();
            query = query.Where(x => x.ListaItemTrilha.Any(y => y.Missao.PontoSebrae.TrilhaNivel.ID == idTrilhaNivel));
            return query.AsQueryable();
        }

        public IList<Objetivo> ObterTodos()
        {
            return repositorio.ObterTodos().OrderBy(x => x.Nome).ToList<Objetivo>();
        }

        public Objetivo ObterPorID(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

        public void Excluir(Objetivo pObjetivo)
        {

            this.ValidarInstancia(pObjetivo);

            if (this.ValidarDependencias(pObjetivo))
                throw new AcademicoException("Exclusão de registro negada. Existem Registros Dependentes deste Objetivo.");

            repositorio.Excluir(pObjetivo);
        }

        public IList<Objetivo> ObterPorFiltro(Objetivo pObjetivo)
        {
            var query = repositorio.session.Query<Objetivo>();

            if (pObjetivo != null)
            {
                if (!string.IsNullOrWhiteSpace(pObjetivo.Nome))
                    query = query.Where(x => x.Nome.Contains(pObjetivo.Nome));

                if (!string.IsNullOrWhiteSpace(pObjetivo.ChaveExterna))
                    query = query.Where(x => x.ChaveExterna.Equals(pObjetivo.ChaveExterna));
            }

            return query.ToList<Objetivo>();
        }

        public Objetivo ObterPorChaveExterna(string chaveExterna, int currentId)
        {
            var query = repositorio.session.Query<Objetivo>();
            query = query.Where(x => x.ChaveExterna == chaveExterna && x.ID != currentId);
            return query.FirstOrDefault();
        }

        public Objetivo ObterPorTexto(string textoObjetivo)
        {
            var query = repositorio.session.Query<Objetivo>();
            Objetivo objetivo = null;

            if (textoObjetivo != null)
            {
                objetivo = query.FirstOrDefault(x => string.Equals(x.Nome.Trim(), textoObjetivo.Trim()));
            }

            return objetivo;
        }
       
       
        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        #endregion

        #region "Métodos Protected"

        protected override bool ValidarDependencias(object pObjetivo)
        {
            Objetivo objetivo = (Objetivo)pObjetivo;
            return (objetivo.ListaItemTrilha.Count > 0);
        }

        #endregion

        public IQueryable<Objetivo> ObterTodosIQueryable()
        {
            return repositorio.ObterTodosIQueryable();
        }
    }
}
