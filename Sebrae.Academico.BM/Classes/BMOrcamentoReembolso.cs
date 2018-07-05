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
    public class BMOrcamentoReembolso : RepositorioBase<OrcamentoReembolso>, IDisposable
    {
        private RepositorioBase<OrcamentoReembolso> repositorio;


        public BMOrcamentoReembolso()
        {
            repositorio = new RepositorioBase<OrcamentoReembolso>();
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }
    }
}
