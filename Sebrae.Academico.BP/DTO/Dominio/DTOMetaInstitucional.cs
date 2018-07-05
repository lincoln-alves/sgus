using System;

namespace Sebrae.Academico.BP.DTO.Dominio
{
    public class DTOMetaInstitucional: DTOEntidadeBasica
    {
        public virtual DateTime DataInicioCiclo { get; set; }
        public virtual DateTime DataFimCiclo { get; set; }
    }
}
