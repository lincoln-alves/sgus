using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMTemplate : BusinessManagerBase, IDisposable
    {
        #region Atributos

        private RepositorioBase<Template> repositorio;

        #endregion
        
        public BMTemplate()
        {
            repositorio = new RepositorioBase<Template>();
        }

        public Template ObterPorID(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

        public IList<Template> ObterTodos()
        {
            return repositorio.ObterTodos().ToList<Template>();
        }
        
        public void Salvar(Template template)
        {
            repositorio.Salvar(template);
        }

        public void Salvar(IList<Template> listaTemplate)
        {
            repositorio.Salvar(listaTemplate);
        }
        
        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }


    }
}


