using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;

namespace Sebrae.Academico.BM.Classes
{
    public class BMCapacitacao : BusinessManagerBase, IDisposable
    {
        #region Atributos

        private RepositorioBase<Capacitacao> repositorio;

        #endregion

        #region "Construtor"

        public BMCapacitacao()
        {
            repositorio = new RepositorioBase<Capacitacao>();
        }

        #endregion

        public IQueryable<Capacitacao> ObterTodos()
        {
            return repositorio.session.Query<Capacitacao>();
        }

        public IList<Capacitacao> ObterPorFiltro(Capacitacao filtro)
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<Capacitacao>();

            if (!string.IsNullOrEmpty(filtro.Nome))
                query = query.Where(x => x.Nome.Contains(filtro.Nome));

            if (filtro.Programa.ID > 0)
                query = query.Where(x => x.Programa.ID == filtro.Programa.ID);

            return query.ToList<Capacitacao>();
        }
        
        public Capacitacao ObterPorId(int pIdCapacitacao)
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<Capacitacao>();
            return query.FirstOrDefault(x => x.ID == pIdCapacitacao);
        }

        public IQueryable<Capacitacao> ObterPorIdPrograma(int pIdPrograma)
        {
            return ObterPorIdPrograma(pIdPrograma, false);
        }

        public IQueryable<Capacitacao> ObterPorIdPrograma(int pIdPrograma, bool somenteComInscricoesAbertas)
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<Capacitacao>();

            if (somenteComInscricoesAbertas)
            {
                query = query.Where(x => x.DataInicio <= DateTime.Now && x.DataFim >= DateTime.Now);
            }

            return query.Where(x => x.Programa.ID == pIdPrograma);
        }        

        public void Salvar(Capacitacao pCapacitacao)
        {
            //ValidarCapacitacaoInformada(pCapacitacao);
            repositorio.Salvar(pCapacitacao);

        }

        public void ValidarCapacitacaoInformada(Capacitacao pCapacitacao)
        {

            //this.ValidarInstancia(pPrograma);

            //if (string.IsNullOrWhiteSpace(pPrograma.Nome)) throw new AcademicoException("Nome. Campo Obrigatório");

            //this.VerificarExistenciaDePrograma(pPrograma);
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public void Excluir(Capacitacao capacitacao)
        {
            if (this.ValidarDependencias(capacitacao))
                throw new AcademicoException("Exclusão de registro negada. Existem registros dependentes desta oferta.");

            repositorio.Excluir(capacitacao);
        }

        protected override bool ValidarDependencias(object pCapacitacao)
        {
            Capacitacao capacitacao = (Capacitacao)pCapacitacao;

            return (capacitacao.ListaModulos != null && capacitacao.ListaModulos.Count > 0);
        }

        public IList<Capacitacao> ObterPorPrograma(Programa programa)
        {
            var query = repositorio.session.Query<Capacitacao>();

            query = query.Where(x => x.Programa.ID == programa.ID).Select(x => new Capacitacao() { ID = x.ID, Nome = x.Nome });

            return query.ToList<Capacitacao>();
        }

        public IList<Capacitacao> ObterPorFiltroNoPeriodoInscricoes(Capacitacao filtro)
        {
            var query = repositorio.session.Query<Capacitacao>();

            if (filtro.ID > 0)
                query = query.Where(x => x.ID == filtro.ID);

            if (!string.IsNullOrEmpty(filtro.Nome))
                query = query.Where(x => x.Nome.Contains(filtro.Nome));

            if (filtro.Programa.ID > 0)
                query = query.Where(x => x.Programa.ID == filtro.Programa.ID);

            query = query.Where(x => x.ListaTurmas.Where(p => p.DataInicio != null && p.DataFim != null).Any(t => DateTime.Now >= t.DataInicio && DateTime.Now <= t.DataFim));

            return query.ToList();
        }
        

    }
}
