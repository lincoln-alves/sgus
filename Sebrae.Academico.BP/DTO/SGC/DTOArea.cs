using System.Collections.Generic;

namespace Sebrae.Academico.BP.DTO.SGC
{
    public class DTOArea
    {
        public int id { get; set; }
        public string nome { get; set; }
        public List<DTOSubarea> subAreas { get; set; }
    }
}
