using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using System;

namespace Sebrae.Academico.BP
{
    public class ManterUsuarioTrilhaMensagemGuia : BusinessProcessBase, IDisposable
    {
        private readonly BMUsuarioTrilhaMensagemGuia _bmUsuarioTrilhaMensagemGuia;

        public ManterUsuarioTrilhaMensagemGuia()
        {
            _bmUsuarioTrilhaMensagemGuia = new BMUsuarioTrilhaMensagemGuia();
        }

        public UsuarioTrilhaMensagemGuia Salvar(UsuarioTrilhaMensagemGuia mensagemGuia)
        {
            _bmUsuarioTrilhaMensagemGuia.Salvar(mensagemGuia);

            return mensagemGuia;
        }

        public IQueryable<UsuarioTrilhaMensagemGuia> ObterTodos()
        {
            return _bmUsuarioTrilhaMensagemGuia.ObterTodos();
        }

        public void Dispose()
        {
            _bmUsuarioTrilhaMensagemGuia.Dispose();
        }
    }
}
