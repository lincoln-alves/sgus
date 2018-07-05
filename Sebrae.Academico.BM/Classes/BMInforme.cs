using System;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMInforme : BusinessManagerBase, IDisposable
    {
        private readonly RepositorioBase<Informe> _repositorio;

        public BMInforme()
        {
            _repositorio = new RepositorioBase<Informe>();
        }

        public void Salvar(Informe informe)
        {
            _repositorio.Salvar(informe);
        }

        public IQueryable<Informe> ObterTodos()
        {
            return
                _repositorio.session.Query<Informe>()
                    .Fetch(x => x.Envios)
                    .OrderBy(x => x.Ano)
                    .ThenBy(x => x.Mes)
                    .AsQueryable();
        }

        public Informe ObterPorId(int id)
        {
            return _repositorio.ObterPorID(id);
        }

        public void Excluir(Informe informe)
        {
            _repositorio.Excluir(informe);
        }

        public void Dispose()
        {
            _repositorio.Dispose();
            GC.Collect();
        }
    }
}