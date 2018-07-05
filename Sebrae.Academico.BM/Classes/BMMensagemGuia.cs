using System;
using System.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMMensagemGuia : BusinessManagerBase, IDisposable
    {

        private readonly RepositorioBase<MensagemGuia> _repositorio;
        
        public BMMensagemGuia()
        {
            _repositorio = new RepositorioBase<MensagemGuia>();
        }

        public IQueryable<MensagemGuia> ObterTodos()
        {
            return _repositorio.ObterTodosIQueryable();
        }

        public MensagemGuia Salvar(MensagemGuia mensagemGuia)
        {
            _repositorio.Salvar(mensagemGuia);

            return mensagemGuia;
        }

        public void Dispose()
        {
            _repositorio.Dispose();
            GC.Collect();
        }

        public MensagemGuia ObterPorId(enumMomento mensagemGuiaId)
        {
            return _repositorio.ObterPorID(mensagemGuiaId);
        }
    }
}
