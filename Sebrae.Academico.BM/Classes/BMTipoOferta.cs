using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMTipoOferta : BusinessManagerBase, IDisposable
    {

        private RepositorioBase<TipoOferta> repositorio;

        public BMTipoOferta()
        {
            repositorio = new RepositorioBase<TipoOferta>();
        }

        public void ValidarTipoOferta(TipoOferta pTipoOferta)
        {

            ValidarDependencias(pTipoOferta);

            if (string.IsNullOrWhiteSpace(pTipoOferta.Nome))
                throw new AcademicoException("Nome. Campo Obrigatório");

        }

        public void Salvar(TipoOferta pTipoOferta)
        {
            ValidarTipoOferta(pTipoOferta);
            repositorio.Salvar(pTipoOferta);
        }

        public TipoOferta ObterPorID(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

        public IList<TipoOferta> ObterTodos()
        {
            return repositorio.ObterTodos();
        }

        public IList<TipoOferta> ObterPorFiltro(TipoOferta pTipoOferta)
        {
            var query = repositorio.session.Query<TipoOferta>();

            if (pTipoOferta != null)
            {
                if (!string.IsNullOrWhiteSpace(pTipoOferta.Nome))
                    query = query.Where(x => x.Nome.ToUpper().Contains(pTipoOferta.Nome.ToUpper()));
            }

            return query.Select(x => new TipoOferta() { ID = x.ID, Nome = x.Nome }).ToList<TipoOferta>();
        }

        protected override bool ValidarDependencias(object pTipoOferta)
        {
            TipoOferta tipoOferta = (TipoOferta)pTipoOferta;

            return (tipoOferta.ListaOferta != null && tipoOferta.ListaOferta.Count > 0);
        }

        public void Excluir(TipoOferta pTipoOferta)
        {
            if (ValidarDependencias(pTipoOferta))
                throw new AcademicoException("Exclusão de registro negada. Existem Registros Dependentes deste Tipo de Oferta.");

            repositorio.Excluir(pTipoOferta);
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }
    }
}
