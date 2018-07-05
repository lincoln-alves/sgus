using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;


namespace Sebrae.Academico.BM.Classes
{
    public class BMLogGeracaoRelatorio: BusinessManagerBase, IDisposable
    {
        #region "Atributos Privados"

        private RepositorioBase<LogGeracaoRelatorio> repositorio;

        #endregion

        #region "Construtor"

        public BMLogGeracaoRelatorio()
        {
            repositorio = new RepositorioBase<LogGeracaoRelatorio>();
        }

        #endregion

        #region "Métodos Públicos"
        
        public void Salvar(LogGeracaoRelatorio pLogExecucao)
        {
            repositorio.Salvar(pLogExecucao);
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        #endregion

    }
}
