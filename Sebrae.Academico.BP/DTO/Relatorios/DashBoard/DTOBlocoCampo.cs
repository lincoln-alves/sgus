using Sebrae.Academico.BP.DTO.Services.Processo;
using System.Collections.Generic;

namespace Sebrae.Academico.BP.DTO.Relatorios.DashBoard
{
    public class DTOBlocoCampo
    {
        public DTOBlocoCampo(bool possuiEspacoAntes, int alturaMinima)
        {
            PossuiEspacoAntes = possuiEspacoAntes;
            Campos = new List<DTOCampo>();

            // Altura mínima para caso só tenha um campo.
            AlturaBloco = alturaMinima;
        }

        public int AlturaBloco { get; set; }
        public List<DTOCampo> Campos { get; set; }
        public bool PossuiEspacoAntes { get; set; }
    }
}
