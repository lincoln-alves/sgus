using Sebrae.Academico.Dominio.Enumeracao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Services
{
    public class DTOConsultaDisponibilidadeMatriculaNivelTrilha : RetornoWebService
    {
        public string TermoDeAceite { get; set; }
        public int CodigoDisponibilidade { get; set; }
        public string TextoDisponibilidade { get; set; }
    }
}
