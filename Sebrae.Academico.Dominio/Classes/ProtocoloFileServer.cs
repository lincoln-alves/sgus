using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class ProtocoloFileServer
    {
        public virtual int Id { get; set; }
        public virtual Protocolo Protocolo { get; set; }
        public virtual FileServer FileServer { get; set; }
        public virtual Usuario Usuario { get; set; }

        public virtual DateTime DataEnvio { get; set; }

        public ProtocoloFileServer()
        {
            DataEnvio = DateTime.Now;
        }
    }
}
