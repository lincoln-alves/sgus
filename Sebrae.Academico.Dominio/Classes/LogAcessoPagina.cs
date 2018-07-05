using Sebrae.Academico.Dominio.Enumeracao;
using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class LogAcessoPagina
    {
        private string _IP;
        public virtual string IP
        {
            get { return _IP; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _IP = value.Substring(0, value.ToString().Length);
                }
            }
        }
        public virtual int Id { get; set; }
        public virtual DateTime DTSolicitacao { get; set; }
        public virtual Usuario IDUsuario { get; set; }
        public virtual Pagina Pagina { get; set; }
        public virtual string QueryString { get; set; }
        public virtual enumAcaoNaPagina Acao { get; set; }

        public override bool Equals(object obj)
        {
            LogAcessoPagina objeto = obj as LogAcessoPagina;
            return objeto == null ? false : IDUsuario.ID.Equals(objeto.IDUsuario.ID)
                && DTSolicitacao.Equals(objeto.DTSolicitacao);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() * 31;
        }
    }
}
