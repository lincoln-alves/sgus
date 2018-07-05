using System;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMCargo : BusinessManagerBase, IDisposable
    {
        private readonly RepositorioBase<Cargo> _repositorio;

        public BMCargo()
        {
            _repositorio = new RepositorioBase<Cargo>();
        }

        public IQueryable<Cargo> ObterTodos()
        {
            return _repositorio.session.Query<Cargo>();
        }

        public Cargo ObterPorId(int pId)
        {
            return _repositorio.session.Query<Cargo>().FirstOrDefault(x => x.ID == pId);
        }

        public void Salvar(Cargo model)
        {
            if (model.ID > 0)
            {
                _repositorio.FazerMerge(model);
            }
            else
            {
                _repositorio.Salvar(model);
            }
        }

        public void FazerMerge(Cargo model)
        {
            _repositorio.FazerMerge(model);
        }
        
        public void Excluir(Cargo model)
        {
            _repositorio.Excluir(model);
        }

        public void Dispose()
        {
            _repositorio.Dispose();
            GC.Collect();
        }

        public void Evict(Cargo cargo)
        {
            _repositorio.Evict(cargo);
        }
    }
}