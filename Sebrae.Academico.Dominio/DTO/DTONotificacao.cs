using System;

namespace Sebrae.Academico.Dominio.DTO
{
    /// <summary>
    /// Classe de DTO criada para armazenar informações de Notificacoes.
    /// </summary>
    public class DTONotificacao
    {
        public virtual string TextoNotificacao{ get; set; }
        public virtual DateTime DataNotificacao { get; set; }
        public virtual bool Visualizado { get; set; }
    }
}
