using Sebrae.Academico.Dominio.Enumeracao;
using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class UsuarioCertificadoCertame
    {
        public virtual int ID { get; set; }
        public virtual CertificadoCertame CertificadoCertame { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual string Chave { get; set; }
        public virtual enumStatusUsuarioCertificadoCertame Status { get; set; }
        public virtual float? Nota { get; set; }
        public virtual string ArquivoBoletim { get; set; }
        public virtual enumSituacaoUsuarioCertificadoCertame? Situacao { get; set; }
        public virtual string Justificativa { get; set; }
        public virtual string NumeroInscricao { get; set; }
        /// <summary>
        /// Data em que o usuário realizou o primeiro download do certificado
        /// </summary>
        public virtual DateTime? DataDownload { get; set; }
        public virtual DateTime? DataDownloadBoletim { get; set; }
    }
}