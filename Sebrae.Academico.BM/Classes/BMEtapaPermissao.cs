using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;

namespace Sebrae.Academico.BM.Classes
{
    public class BMEtapaPermissao : BusinessManagerBase, IDisposable
    {
        private RepositorioBase<EtapaPermissao> repositorio;

        public BMEtapaPermissao()
        {
            repositorio = new RepositorioBase<EtapaPermissao>();
        }

        public IQueryable<EtapaPermissao> ObterTodosIQueryable()
        {
            return repositorio.session.Query<EtapaPermissao>().AsQueryable();
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public List<int> ObterIdsPermissoesNiveis(List<int> idsPermitidos, List<int> niveis)
        {
            return
                repositorio.session.Query<EtapaPermissao>()
                .Where(x => idsPermitidos.Contains(x.ID) && x.NivelOcupacional != null && niveis.Contains(x.NivelOcupacional.ID))
                    .Select(x => new { x.ID })
                    .ToList()
                    .Select(x => x.ID)
                    .ToList();
        }
    }
}
