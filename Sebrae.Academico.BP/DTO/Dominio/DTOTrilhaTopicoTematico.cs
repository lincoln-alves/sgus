using System.Collections.Generic;

namespace Sebrae.Academico.BP.DTO.Dominio
{
    public class DTOTrilhaTopicoTematico
    {
        public int ID { get; set; }
        public string Nome { get; set; }

        public List<DTOObjetivo> Objetivos { get; set; }
    }
}
