using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Dominio.DTO;

namespace Sebrae.Academico.Dominio.Classes
{
    public class PublicoAlvo : EntidadeBasicaPorId
    {
        public virtual string Nome { get; set; }
        public virtual IList<OfertaPublicoAlvo> ListaOferta { get; set; }

        /// <summary>
        /// Definição do tipo de público-alvo. Este campo foi criado pra suprir a demanda #2335.
        /// 1 - Gestores
        /// 2 - Formadores
        /// 3 - Facilitadores
        /// </summary>
        public virtual int Tipo { get; set; }
		
		public virtual Uf UF { get; set; }

        public PublicoAlvo()
        {
            this.ListaOferta = new List<OfertaPublicoAlvo>();
        }
    }
}
