using System;

namespace Sebrae.Academico.Dominio.Classes.Views
{
    public class LogAcoesPortal
    {
        public virtual int ID { get; set; }
        public virtual int ID_Usuario { get; set; }
        public virtual string Url { get; set; }
        public virtual string Acao { get; set; }
        public virtual string IP { get; set; }
        public virtual DateTime Datacesso { get; set; }
    }
}
