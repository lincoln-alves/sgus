using System;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMEnvioInforme : BusinessManagerBase, IDisposable
    {
        private readonly RepositorioBase<EnvioInforme> _repositorio;

        public BMEnvioInforme()
        {
            _repositorio = new RepositorioBase<EnvioInforme>();
        }

        public void Salvar(EnvioInforme envioInforme)
        {
            _repositorio.Salvar(envioInforme);
        }

        public IQueryable<EnvioInforme> ObterTodos()
        {
            return
                _repositorio.session.Query<EnvioInforme>()
                    .Fetch(x => x.Informe)
                    .AsQueryable();
        }

        public EnvioInforme ObterPorId(int id)
        {
            return _repositorio.ObterPorID(id);
        }

        public IQueryable<EnvioInforme> ObterPorInforme(int informeId)
        {
            return _repositorio.session.Query<EnvioInforme>().Where(x => x.Informe.ID == informeId);
        }

        public void Excluir(EnvioInforme envioInforme)
        {
            _repositorio.Excluir(envioInforme);
        }

        public void Dispose()
        {
            _repositorio.Dispose();
            GC.Collect();
        }
    }
}