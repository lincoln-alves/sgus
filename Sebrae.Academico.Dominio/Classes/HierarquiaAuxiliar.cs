using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Dominio.DTO;

namespace Sebrae.Academico.Dominio.Classes
{
    public class HierarquiaAuxiliar : EntidadeBasica
    {
        public virtual string CodUnidade { get; set; }
        public virtual Usuario Usuario { get; set; }

        // Item não mapeado em banco
        public virtual string NomeDiretoria { get; set; }

        /// <summary>
        /// Construtor Padrão
        /// </summary>
        public HierarquiaAuxiliar()
        {
        }



    }
}
