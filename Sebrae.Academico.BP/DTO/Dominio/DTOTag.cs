using System.Collections.Generic;

namespace Sebrae.Academico.BP.DTO.Dominio
{
    public class DTOTag : DTOEntidadeBasica
    {
        public virtual List<DTOTag> ListaFilhos { get; set; }
        public virtual bool Sinonimo { get; set; }
        public virtual byte? NumeroNivel { get; set; }
        public DTOTag()
        {
            this.ListaFilhos = new List<DTOTag>();
        }
    }
}
