using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;
using Sebrae.Academico.BM.AutoMapper;
using AutoMapper;

namespace Sebrae.Academico.BM.Classes
{
    public class BMCampoMetaValue : BusinessManagerBase, IDisposable
    {
        #region Atributos

        private RepositorioBase<CampoMetaValue> repositorio;

        #endregion

        #region "Construtor"

        public BMCampoMetaValue()
        {
            repositorio = new RepositorioBase<CampoMetaValue>();
        }

        #endregion

        public IList<CampoMetaValue> ObterTodos()
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<CampoMetaValue>();
            return query.ToList<CampoMetaValue>();
        }

        public CampoMetaValue ObterPorId(int id)
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<CampoMetaValue>();
            return query.Where(x => x.ID == id).FirstOrDefault();
        }        

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public void Salvar(CampoMetaValue pCampoMetaValue)
        {
            repositorio.Salvar(pCampoMetaValue);
        }

    }

}
