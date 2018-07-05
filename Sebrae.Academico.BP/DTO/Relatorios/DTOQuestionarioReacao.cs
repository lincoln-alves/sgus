using System;

namespace Sebrae.Academico.BP.DTO.Relatorios
{
    public class DTOQuestionarioConsolidado
    {
        public int ID_Turma { get; set; }
        public int ID_SolucaoEducacional { get; set; }
        public string NM_SolucaoEducacional { get; set; }
        public string NM_Oferta { get; set; }
        public string NM_Turma { get; set; }
        public DateTime DT_Inicio { get; set; }
        public DateTime DT_Final { get; set; }
        public int QtdeAlunosTurma { get; set; }
        public int QtdeAlunosResponderamQuestionario { get; set; }
        public int QtdeAlunosFinalizaram { get; set; }
        public int PctAlunosQueResponderamQuestionario { get; set; }
        public int PctAlunosFinalizaramQueResponderamQuestionario { get; set; }
    }
}
