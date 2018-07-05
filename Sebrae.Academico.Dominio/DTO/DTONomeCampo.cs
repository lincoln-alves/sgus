using System;
using System.Collections.Generic;

namespace Sebrae.Academico.Dominio.DTO
{
    /// <summary>
    /// Classe de DTO criada para armazenar informações de Notificacoes.
    /// </summary>
    public class DTONomeCampo
    {
        public virtual int ID { get; set; }
        public virtual string Nome { get; set; }
        public virtual List<int> TipoCampo { get; set; }
        public virtual List<int> TipoDado { get; set; }

    }

}
