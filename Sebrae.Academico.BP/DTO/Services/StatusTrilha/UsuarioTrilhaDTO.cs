using System;

namespace Sebrae.Academico.BP.DTO.Services.StatusTrilha
{
    public class UsuarioTrilhaDTO
    {
        public virtual string Status { get; set; }
        public virtual string UFMatricula { get; set; }
        public virtual string NivelOcupacionalMatricula { get; set; }
        public int Nota { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public DateTime? DataLimite { get; set; }
        public DateTime? DataUltimoAcesso { get; set; }
    }
}
