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
    public class BMLogAcessoFuncionalidade : BusinessManagerBase, IDisposable
    {
        #region "Atributos Privados"

        private RepositorioBase<LogAcessoFuncionalidade> repositorio;

        #endregion

        #region "Construtor"

        public BMLogAcessoFuncionalidade()
        {
            repositorio = new RepositorioBase<LogAcessoFuncionalidade>();
        }

        #endregion

        #region "Métodos Públicos"

        public void Salvar(LogAcessoFuncionalidade pLogAcessoFuncionalidade)
        {
            repositorio.Salvar(pLogAcessoFuncionalidade);
        }

        public IList<LogAcessoFuncionalidade> ObterTodos()
        {
            return repositorio.ObterTodos().ToList<LogAcessoFuncionalidade>();
        }

        public LogAcessoFuncionalidade ObterPorID(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        #endregion
    }
}
