using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMSolicitacaoRelatorio : BusinessManagerBase, IDisposable
    {
        private readonly RepositorioBase<SolicitacaoRelatorio> _repositorio;

        public BMSolicitacaoRelatorio()
        {
            _repositorio = new RepositorioBase<SolicitacaoRelatorio>();
        }

        public IEnumerable<SolicitacaoRelatorio> ObterTodos()
        {
            return _repositorio.session.Query<SolicitacaoRelatorio>(); ;
        }

        public SolicitacaoRelatorio ObterPorId(int pId)
        {
            return _repositorio.session.Query<SolicitacaoRelatorio>().FirstOrDefault(x => x.ID == pId);
        }

        public IEnumerable<SolicitacaoRelatorio> ObterPorUsuario(int idUsuario)
        {
            return _repositorio.session.Query<SolicitacaoRelatorio>().Where(d => d.Usuario.ID == idUsuario);
        }

        public void Salvar(SolicitacaoRelatorio model)
        {
            _repositorio.Salvar(model);
        }

        public void Excluir(SolicitacaoRelatorio model)
        {
            _repositorio.Excluir(model);
        }

        public void Dispose()
        {
            _repositorio.Dispose();
            GC.Collect();
        }
    }
}
