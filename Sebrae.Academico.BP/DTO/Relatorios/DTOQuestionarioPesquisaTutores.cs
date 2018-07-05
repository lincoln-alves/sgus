using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Relatorios
{
    public class DTOQuestionarioPesquisaTutores
    {
        public string Tutor { get; set; }
        public string Questionario { get; set; }
        public string Enunciado { get; set; }
        public string Categoria { get; set; }
        public string TopicosAvaliados { get; set; }
        public string FormaAquisicao { get; set; }
        public string SolucaoEducacional { get; set; }
        public string Oferta { get; set; }
        public string Turma { get; set; }
        public string NomeParticipante { get; set; }
        public string NivelOcupacional { get; set; }
        public string UF { get; set; }
        public string Moda { get; set; }
        public string Media { get; set; }
        public string DP { get; set; }
        public string Min { get; set; }
        public string Max { get; set; }
        public string MediaFinal { get; set; }
        public DateTime? Data { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string QntAlunosMatriculadosTurma { get; set; }
        public string QntAlunosRespostaQuestionario { get; set; }
        public string QntAlunosChegaramFinalCurso { get; set; }
        public string PctAlunosRespostaQuestionarioVsTotalAlunos { get; set; }
        public string PctAlunosChegaramFinalCurso { get; set; }
    }
}
