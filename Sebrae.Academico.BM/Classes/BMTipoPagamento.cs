using System;
using System.Collections.Generic;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMTipoPagamento: BusinessManagerBase, IDisposable
    {
        private RepositorioBase<TipoPagamento> repositorio;

        public BMTipoPagamento()
        {
            repositorio = new RepositorioBase<TipoPagamento>();
        }

        public IList<TipoPagamento> ObterTodos()
        {
            return repositorio.ObterTodos();
        }

        public TipoPagamento ObterPorID(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }
    }
}
