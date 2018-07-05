using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMFuncionalidade : BusinessManagerBase, IDisposable
    {
        private RepositorioBase<Funcionalidade> repositorio;


        public BMFuncionalidade()
        {
            repositorio = new RepositorioBase<Funcionalidade>();
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public void Salvar(Funcionalidade funcionalidade)
        {
            repositorio.Salvar(funcionalidade);
        }

        public IList<Funcionalidade> ObterTodos()
        {
            return repositorio.ObterTodos().OrderBy(x => x.Nome).ToList<Funcionalidade>();
        }

        public Funcionalidade ObterPorID(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

    }
}
