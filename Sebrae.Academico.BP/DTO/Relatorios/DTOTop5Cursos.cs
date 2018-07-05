using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Relatorios
{
    public class DTOTop5Cursos
    {
        public IList<DTOTop5CursoPresencial> CursosPresenciais { get; set; }
        public IList<DTOTop5CursoOnline> CursosOnline { get; set; }
    }

    public class DTOTop5CursoPresencial
    {
        public int ID_SolucaoEducacional { get; set; }
        public string NomeSolucaoEducacional { get; set; }
        public float QuantidadeDeUsuariosInscritos { get; set; }
    }

    public class DTOTop5CursoOnline
    {
        public int ID_SolucaoEducacional { get; set; }
        public string NomeSolucaoEducacional { get; set; }
        public float QuantidadeDeUsuariosInscritos { get; set; }
    }
}
