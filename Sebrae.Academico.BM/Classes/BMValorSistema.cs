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
    public class BMValorSistema : BusinessManagerBase, IDisposable
    {
        private RepositorioBase<ValorSistema> repositorio;

        public BMValorSistema()
        {
            repositorio = new RepositorioBase<ValorSistema>();
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public void Salvar(ValorSistema valorSistema)
        {
            repositorio.Salvar(valorSistema);
        }

        public void Salvar(IList<ValorSistema> listaValorSistema)
        {
            repositorio.Salvar(listaValorSistema);
        }

        public IList<ValorSistema> ObterTodos()
        {
            return repositorio.ObterTodos();
        }

        public ValorSistema ObterPorID(int pId)
        {

            ValorSistema valorSistema = null;
            var query = repositorio.session.Query<ValorSistema>();
            valorSistema = query.FirstOrDefault(x => x.ID == pId);
            return valorSistema;
        }

    }
}