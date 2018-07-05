using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMNomeReprovacaoEtapa : BusinessManagerBase, IDisposable
    {
        #region Atributos

        private RepositorioBase<NomeReprovacaoEtapa> repositorio;

        #endregion

        #region "Construtor"

        public BMNomeReprovacaoEtapa()
        {
            repositorio = new RepositorioBase<NomeReprovacaoEtapa>();
        }

        #endregion

        public List<NomeReprovacaoEtapa> ObterTodos()
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<NomeReprovacaoEtapa>();
            return query.ToList();
        }

        public NomeReprovacaoEtapa ObterPorId(int id)
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
