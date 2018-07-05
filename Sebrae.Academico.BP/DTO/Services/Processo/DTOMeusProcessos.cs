using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Services.Processo
{
    public class DTOMeusProcessos
    {
        public DTOMeusProcessos()
        {
            Processos = new List<DTOProcessoResposta>();
        }
        public virtual List<DTOProcessoResposta> Processos { get; set; }

                
    }
}