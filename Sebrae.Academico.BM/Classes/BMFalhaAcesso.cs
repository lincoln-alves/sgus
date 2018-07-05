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
    public class BMFalhaAcesso : BusinessManagerBase, IDisposable
    {
        private RepositorioBase<FalhaAcesso> repositorio = null;

        public BMFalhaAcesso()
        {
            repositorio = new RepositorioBase<FalhaAcesso>();
        }

        public void Salvar(FalhaAcesso pFalhaAcesso)
        {
            repositorio.Salvar(pFalhaAcesso);

        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }
    }
}