using System;
using System.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMLogLider : BusinessManagerBase, IDisposable
    {

        private readonly RepositorioBase<LogLider> _repositorio;

        public BMLogLider()
        {
            _repositorio = new RepositorioBase<LogLider>();
        }

        public LogLider Salvar(LogLider LogLider)
        {
            _repositorio.Salvar(LogLider);

            return LogLider;
        }

        public IQueryable<LogLider> ObterPorAlunoPontoSebrae(UsuarioTrilha aluno, PontoSebrae pontoSebrae)
        {
            return _repositorio.ObterTodosIQueryable().Where(x => x.Aluno.ID == aluno.ID && x.PontoSebrae.ID == pontoSebrae.ID);
        }

        public void Dispose()
        {
            _repositorio.Dispose();
            GC.Collect();
        }
    }
}