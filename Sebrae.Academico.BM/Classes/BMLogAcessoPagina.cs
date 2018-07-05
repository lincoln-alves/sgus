using System;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System.Linq;

namespace Sebrae.Academico.BM.Classes
{
    public class BMLogAcessoPagina : BusinessManagerBase, IDisposable
    {
        #region "Atributos Privados"

        private RepositorioBase<LogAcessoPagina> repositorio;

        #endregion

        #region "Construtor"

        public BMLogAcessoPagina()
        {
            repositorio = new RepositorioBase<LogAcessoPagina>();
        }

        #endregion

        #region "Métodos Públicos"

        public void Salvar(LogAcessoPagina registro)
        {           
            repositorio.SalvarSemCommit(registro);
        }

        public IQueryable<LogAcessoPagina> ObterTodosQuerable ()
        {
            return repositorio.ObterTodosIQueryable();
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        #endregion
    }
}
