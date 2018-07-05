using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;

namespace Sebrae.Academico.BM.Classes
{
    public class BMModulo : BusinessManagerBase, IDisposable
    {
        #region Atributos

        private RepositorioBase<Modulo> repositorio;

        #endregion

        #region "Construtor"

        public BMModulo()
        {
            repositorio = new RepositorioBase<Modulo>();
        }

        #endregion

        public IList<Modulo> ObterTodos()
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<Modulo>();
            return query.ToList<Modulo>();
        }

        public Modulo ObterPorId(int pId)
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<Modulo>();
            return query.FirstOrDefault(x => x.ID == pId);
        }
        
        public IList<Modulo> ObterPorFiltro(Modulo modulo)
        {
            var query = repositorio.session.Query<Modulo>();

            if (!string.IsNullOrEmpty(modulo.Nome))
                query = query.Where(x => x.Nome.Contains(modulo.Nome));

            if (modulo.Capacitacao.ID > 0)
            {
                query = query.Where(x => x.Capacitacao.ID == modulo.Capacitacao.ID);
            }
            else if (modulo.Capacitacao.ID == 0 && modulo.Capacitacao.Programa.ID > 0)
            {
                query = query.Where(x => x.Capacitacao.Programa.ID == modulo.Capacitacao.Programa.ID);
            }

            return query.ToList<Modulo>();
        }

        public IQueryable<Modulo> ObterTodosIQueryable() {
            return repositorio.session.Query<Modulo>();
        }

        public IQueryable<Modulo> ObterPorCapacitacao(int idCapacitacao)
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            return repositorio.session.Query<Modulo>()
                .Where(x => x.Capacitacao.ID == idCapacitacao);
        }

        public void Salvar(Modulo pModulo)
        {
            ValidarModuloInformada(pModulo);

            repositorio.Salvar(pModulo);
        }

        public void Excluir(Modulo pModulo)
        {
            repositorio.Excluir(pModulo);
        }

        private void ValidarModuloInformada(Modulo pModulo) {
            var m = ObterPorFiltro(new Modulo {Nome = pModulo.Nome, Capacitacao = pModulo.Capacitacao}).FirstOrDefault();
            if (m != null && m.ID != pModulo.ID) {
                throw new AcademicoException("Nome informado já existe na oferta informada.");
            }
        }


        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

    }
}
