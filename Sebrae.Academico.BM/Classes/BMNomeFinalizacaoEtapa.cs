using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMNomeFinalizacaoEtapa : BusinessManagerBase, IDisposable
    {
        #region Atributos

        private RepositorioBase<NomeFinalizacaoEtapa> repositorio;

        #endregion

        #region "Construtor"

        public BMNomeFinalizacaoEtapa()
        {
            repositorio = new RepositorioBase<NomeFinalizacaoEtapa>();
        }

        #endregion

        public List<NomeFinalizacaoEtapa> ObterTodos()
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<NomeFinalizacaoEtapa>();
            return query.ToList();
        }

        public NomeFinalizacaoEtapa ObterPorId(int id)
        {
            return repositorio.ObterPorID(id);
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }
    }
}
