using System;


namespace Sebrae.Academico.Dominio.Classes
{
    public class LogFaleConosco : EntidadeBasicaPorId
    {
        private string _CPF;
        public virtual string CPF
        {
            get { return _CPF; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _CPF = value.Substring(0, value.ToString().Length);
                }
            }
        }

        private string _Nome;
        public virtual string Nome
        {
            get { return _Nome; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _Nome = value.Substring(0, value.ToString().Length);
                }
            }
        }

        private string _Email;
        public virtual string Email
        {
            get { return _Email; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    _Email = value.Substring(0, value.ToString().Length);
            }
        }


        private string _Assunto;
        public virtual string Assunto
        {
            get { return _Assunto; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _Assunto = value.Substring(0, value.ToString().Length);
                }
            }
        }

        private string _Mensagem;
        public virtual string Mensagem
        {
            get { return _Mensagem; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _Mensagem = value.Substring(0, value.ToString().Length);
                }
            }
        }

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
        public virtual DateTime DataSolicitacao { get; set; }
    }

}