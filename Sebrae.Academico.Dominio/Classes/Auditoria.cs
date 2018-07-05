using System;

namespace Sebrae.Academico.Dominio.Classes
{
    /// <summary>
    /// Classe para armazenar informações para auditoria de dados.
    /// </summary>
    public class Auditoria
    {

        #region "Atributos Privados"

        private string usuarioAuditoria;
        private DateTime? dataAuditoria;

        #endregion

        #region "Atributos Públicos"

        public virtual string UsuarioAuditoria
        {
            get
            {
                return this.usuarioAuditoria;
            }
            set
            {
                this.usuarioAuditoria = value;
            }
        }


        public virtual DateTime? DataAuditoria
        {
            get
            {
                return this.dataAuditoria;
            }
            set
            {
                this.dataAuditoria = value;
            }
        }


        #endregion

        #region "Construtor"
       
        public Auditoria(string usuarioAlteracao)
        {
            this.dataAuditoria = DateTime.Now;
            this.usuarioAuditoria = usuarioAlteracao;
        }
        public Auditoria()
        {
            this.dataAuditoria = DateTime.Now;
        }
        #endregion

    }

}
