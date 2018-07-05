using System;
using System.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMUsuarioTrilhaMensagemGuia : BusinessManagerBase, IDisposable
    {

        private readonly RepositorioBase<UsuarioTrilhaMensagemGuia> _repositorio;
        
        public BMUsuarioTrilhaMensagemGuia()
        {
            _repositorio = new RepositorioBase<UsuarioTrilhaMensagemGuia>();
        }

        public void Salvar(UsuarioTrilhaMensagemGuia usuarioTrilhaMensagemGuia)
        {
            _repositorio.LimparSessao();

            try
            {
                using (_repositorio.ObterTransacao())
                {
                    _repositorio.SalvarSemCommit(usuarioTrilhaMensagemGuia);

                    _repositorio.Commit();
                }
            }
            catch (Exception)
            {
                _repositorio.RollbackTransaction();
            }
        }

        public void Dispose()
        {
            _repositorio.Dispose();
            GC.Collect();
        }

        public IQueryable<UsuarioTrilhaMensagemGuia> ObterTodos()
        {
            return _repositorio.ObterTodosIQueryable();
        }
    }
}
