using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.Classes
{
    public class OfertaGerenciadorVaga : EntidadeBasica
    {
        public virtual Oferta Oferta { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Uf UF { get; set; }
        public virtual bool? Contemplado { get; set; }
        public virtual bool? Vigente { get; set; }
        public virtual string Resposta { get; set; }
        public virtual int VagasAnteriores { get; set; }
        public virtual int VagasRecusadas { get; set; }
        public virtual DateTime DataSolicitacao { get; set; }
    }
}
