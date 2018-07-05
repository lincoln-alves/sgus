using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using System;

namespace Sebrae.Academico.BP
{
    public class ManterMensagemGuia : BusinessProcessBase, IDisposable
    {
        private readonly BMMensagemGuia _bmMensagemGuia;

        public ManterMensagemGuia()
        {
            _bmMensagemGuia = new BMMensagemGuia();
        }

        public IQueryable<MensagemGuia> ObterTodos()
        {
            return _bmMensagemGuia.ObterTodos();
        }

        public MensagemGuia Salvar(MensagemGuia mensagemGuia)
        {
            return _bmMensagemGuia.Salvar(mensagemGuia);
        }

        public MensagemGuia ObterPorId(int mensagemGuiaId)
        {
            return _bmMensagemGuia.ObterPorId((enumMomento)mensagemGuiaId);
        }

        public MensagemGuia ObterPorId(enumMomento mensagemGuiaId)
        {
            return _bmMensagemGuia.ObterPorId(mensagemGuiaId);
        }

        public void Dispose()
        {
            _bmMensagemGuia.Dispose();
        }
    }
}
