using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Relatorios
{
    public class DTOQuestionarioAvaliacao
    {
        public DTOQuestionarioAvaliacao()
        {
            this.Questionario = new List<DTOQuestionarioAvaliacaoQuestionario>();
            this.Usuario = new List<DTOQuestionarioAvaliacaoUsuario>();
        }
        public List<DTOQuestionarioAvaliacaoQuestionario> Questionario { get; set; }
        public List<DTOQuestionarioAvaliacaoUsuario> Usuario { get; set; }
    }

    public class DTOQuestionarioAvaliacaoQuestionario
    {
        public string Questionario { get; set; }
        public int Nota { get; set; }
    }

    public class DTOQuestionarioAvaliacaoUsuario
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }
        public string UF { get; set; }
        public string NivelOcupacional { get; set; }
        public string DataAdmissao { get; set; }
    }
}
