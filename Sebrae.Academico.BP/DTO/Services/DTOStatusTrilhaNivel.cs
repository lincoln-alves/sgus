using System;

namespace Sebrae.Academico.BP.DTO.Services
{
    public class DTOStatusTrilhaNivel
    {
        public string Status { get; set; }
        public int Nota { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public DateTime? DataLimite { get; set; }
    }
}
