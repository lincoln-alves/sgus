using System;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMLogBuscaSite : BusinessManagerBase, IDisposable
    {
        #region "Atributos Privados"

        private RepositorioBase<LogBuscaSite> repositorio;

        #endregion

        #region "Construtor"

        public BMLogBuscaSite()
        {
            repositorio = new RepositorioBase<LogBuscaSite>();
        }

        #endregion

        #region "Métodos Públicos"

        public void Salvar(LogBuscaSite registro)
        {
            repositorio.Salvar(registro);
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        #endregion
    }
}
