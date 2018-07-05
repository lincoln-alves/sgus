using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Relatorios
{
    public class DTORelatorioInscricoesPorStatusENivel
    {
        public string Status { get; set; }
        
        public int AD { get; set; }
        public int ALI { get; set; }
        public int APA { get; set; }
        public int AnalistaTecnicoI { get; set; }
        public int AnalistaTecnicoII { get; set; }
        public int AnalistaTecnicoIII { get; set; }
        public int AOE { get; set; }
        public int Assessor { get; set; }
        public int AssistenteI { get; set; }
        public int AssistenteII { get; set; }
        public int AssistenteIII { get; set; }
        public int Conselheiro { get; set; }
        public int Credenciado { get; set; }
        public int Dirigente { get; set; }
        public int Estagiario { get; set; }
        public int Gerente { get; set; }
        public int MenorAprendiz { get; set; }
        public int OrientadorALI { get; set; }
        public int Parceiro { get; set; }
        public int PreALI { get; set; }
        public int FuncionarioTemporario { get; set; }
        public int Trainee { get; set; }

        public int Total { get; set; }
        
    }

    public class DTOProcInscricoesPorStatusENivel
    {
        public string Status { get; set; }
        public string NivelOcupacional { get; set; }
        public int Quantidade { get; set; }
    }
}
