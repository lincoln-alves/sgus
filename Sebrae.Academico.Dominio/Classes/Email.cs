using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class Email : EntidadeBasica
    {
        public virtual Usuario Usuario { get; set; }
        public virtual EmailEnvio EmailEnvio { get; set; }
        public virtual string Assunto { get; set; }
        public virtual string TextoEmail { get; set; }
        public virtual bool Enviado { get; set; }
        public virtual DateTime DataGeracao { get; set; }
        public virtual DateTime? DataEnvio { get; set; }
    }
}
