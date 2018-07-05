using System;
using System.Collections.Generic;

namespace Sebrae.Academico.Dominio.Classes
{
    public class Protocolo : EntidadeBasica
    {
        public virtual long Numero { get; set; }

        public virtual Usuario Remetente { get; set; }

        public virtual Usuario UsuarioAssinatura { get; set; }

        public virtual Usuario Destinatario { get; set; }

        public virtual DateTime DataEnvio { get; set; }

        public virtual DateTime? DataRecebimento { get; set; }

        public virtual IList<ProtocoloFileServer> Anexos { get; set; }

        public virtual Protocolo ProtocoloPai { get; set; }

        public virtual string Despacho { get; set; }

        public virtual string DespachoReencaminhamento { get; set; }

        public virtual bool Arquivado { get; set; }


        public Protocolo()
        {
            Anexos = new List<ProtocoloFileServer>();
        }
    }
}
