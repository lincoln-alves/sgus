using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;

namespace Sebrae.Academico.BM.Classes
{
    public class BMEtapaEncaminhamentoUsuario : BusinessManagerBase, IDisposable
    {
        private RepositorioBase<EtapaEncaminhamentoUsuario> repositorio;

        public BMEtapaEncaminhamentoUsuario()
        {
            repositorio = new RepositorioBase<EtapaEncaminhamentoUsuario>();
        }

        public IQueryable<EtapaEncaminhamentoUsuario> ObterTodosIQueryable()
        {
            return repositorio.session.Query<EtapaEncaminhamentoUsuario>();
        }

        public void Salvar(EtapaEncaminhamentoUsuario model)
        {
            repositorio.Salvar(model);
        }


        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }
       
    }
}
