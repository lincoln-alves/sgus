using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMRelatorio: BusinessManagerBase, IDisposable
    {
        private RepositorioBase<Relatorio> repositorio;

        public BMRelatorio()
        {
            repositorio = new RepositorioBase<Relatorio>();
        }
        

        public Relatorio ObterPorID(int pID)
        {
            return repositorio.ObterPorID(pID);
        }



        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        
      

    }
}
