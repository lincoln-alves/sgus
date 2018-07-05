using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;

namespace Sebrae.Academico.BM.Classes
{
    public class BMEtapaResposta : BusinessManagerBase
    {
        #region Atributos

        private RepositorioBase<EtapaResposta> repositorio;

        #endregion

        #region "Construtor"

        public BMEtapaResposta()
        {
            repositorio = new RepositorioBase<EtapaResposta>();
        }

        #endregion

        public IList<EtapaResposta> ObterTodos()
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<EtapaResposta>();
            return query.ToList<EtapaResposta>();
        }

        public IQueryable<EtapaResposta> ObterTodosIQueryable()
        {
            return repositorio.session.Query<EtapaResposta>();
        }

        public IEnumerable<EtapaResposta> ObterTodosIEnumerable()
        {
            return repositorio.session.Query<EtapaResposta>().AsEnumerable();
        }
    }
}
