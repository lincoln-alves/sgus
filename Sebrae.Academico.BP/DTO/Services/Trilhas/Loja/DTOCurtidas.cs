using Sebrae.Academico.Dominio.Enumeracao;
using System.Collections.Generic;

namespace Sebrae.Academico.BP.DTO.Services.Trilhas.Loja
{
    public class DTOCurtidas
    {
        public int QuantidadeCurtidas { get; set; }
        public int QuantidadeDescurtidas { get; set; }
        public List<DTOUsuarioCurtida> ListaCurtidas { get; set; }
        public enumTipoCurtida TipoCurtida { get; set; }
    }
}
