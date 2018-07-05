using System;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMLogAcessoTurma : BusinessManagerBase, IDisposable
    {
        #region "Atributos Privados"

        RepositorioBase<LogAcessoTurma> repositorio;

        #endregion

        #region "Construtor"

        public BMLogAcessoTurma()
        {
            repositorio = new RepositorioBase<LogAcessoTurma>();
        }

        #endregion

        #region "Métodos Públicos"

        public void Salvar(LogAcessoTurma PLogUsuarioTurma)
        {
            repositorio.Salvar(PLogUsuarioTurma);
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        #endregion
    }
}
