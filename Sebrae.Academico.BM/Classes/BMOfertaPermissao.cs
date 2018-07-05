using System;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMOfertaPermissao : BusinessManagerBase, IDisposable
    {
        private RepositorioBase<OfertaPermissao> repositorio;

        public BMOfertaPermissao()
        {
            repositorio = new RepositorioBase<OfertaPermissao>();
        }

        public OfertaPermissao ObterExistente(OfertaPermissao pOfertaPermissao)
        {
            var query = repositorio.session.Query<OfertaPermissao>();
            query = query.Where(x => x.Uf.ID == pOfertaPermissao.Uf.ID);
            query = query.Where(x => x.Oferta.ID == pOfertaPermissao.Oferta.ID);

            return query.FirstOrDefault();
        }

        public void Salvar(OfertaPermissao pOfertaPermissao)
        {
            repositorio.Salvar(pOfertaPermissao);
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }


        //Retorna todos as Permissoes por oferta.
        public IQueryable<OfertaPermissao> ObterPorOferta(Oferta oferta)
        {
            return repositorio.session.Query<OfertaPermissao>()
                .Where(x => x.Oferta.ID == oferta.ID).AsQueryable();
        }

    }
}
