using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMPublicacaoSaberUsuario : BusinessManagerBase, IDisposable
    {
        private RepositorioBase<PublicacaoSaberUsuario> repositorio;

        public BMPublicacaoSaberUsuario()
        {
            repositorio = new RepositorioBase<PublicacaoSaberUsuario>();
        }

        public IList<PublicacaoSaberUsuario> ObterPorIdUsuario(int pIdUsuario)
        {
            var query = repositorio.session.Query<PublicacaoSaberUsuario>(); 
            query = query.Where(x => x.Usuario.ID == pIdUsuario);
            return query.ToList();
        }
           
        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

    }
}
