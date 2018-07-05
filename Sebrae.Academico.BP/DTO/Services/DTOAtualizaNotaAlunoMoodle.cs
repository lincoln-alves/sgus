using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.BP.DTO.Services
{
    public class DTOAtualizaNotaAlunoMoodle : RetornoWebService
    {
        public DateTime DtTermino { get; set; }
        public decimal MediaFinal { get; set; }
        public int IdStatusMatricula { get; set; }
        public int CodMoodle { get; set; }
    }
}
