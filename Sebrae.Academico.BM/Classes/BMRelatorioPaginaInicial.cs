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
    public class BMRelatorioPaginaInicial : BusinessManagerBase, IDisposable
    {
        #region Atributos

        private RepositorioBase<RelatorioPaginaInicial> repositorio;

        #endregion

        #region "Construtor"

        public BMRelatorioPaginaInicial()
        {
            repositorio = new RepositorioBase<RelatorioPaginaInicial>();
        }

        #endregion

        public IList<RelatorioPaginaInicial> ObterTodos()
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<RelatorioPaginaInicial>();
            return query.ToList<RelatorioPaginaInicial>();
        }

        public RelatorioPaginaInicial ObterPorId(int pId)
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<RelatorioPaginaInicial>();
            return query.FirstOrDefault(x => x.ID == pId);
        }

        public void Salvar(RelatorioPaginaInicial model)
        {
            repositorio.Salvar(model);
        }

        public void Excluir(RelatorioPaginaInicial model)
        {
            repositorio.Excluir(model);
        }

        public RelatorioPaginaInicial ObterPorTagRelatorio(string nomeRelatorio)
        {
            return ObterTodos().FirstOrDefault(x => x.Tag == nomeRelatorio);
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

    }
}
