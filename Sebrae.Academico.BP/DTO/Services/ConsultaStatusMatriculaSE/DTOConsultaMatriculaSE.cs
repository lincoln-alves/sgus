using System;

namespace Sebrae.Academico.BP.DTO.Services
{

    public class DTOConsultaMatriculaSEOferta
    {
        public string NomeOferta { get; set; }

        public string TipoOferta { get; set; }

        public DateTime? DataInicio { get; set; }

        public DateTime? DataFim { get; set; }

        public DateTime? DataInicioInscricoes { get; set; }

        public DateTime? DataFimInscricoes { get; set; }
    }

    public class DTOConsultaMatriculaSETurma
    {
        public string NomeTurma { get; set; }

        public string NomeProfessor { get; set; }

        public string EmailProfessor { get; set; }

        public string Local { get; set; }

        public DateTime? DataInicio { get; set; }

        public DateTime? DataFim { get; set; }
    }


    public class DTOConsultaMatriculaSE
    {
        public DTOConsultaMatriculaSEOferta Oferta { get; set; }
        public DTOConsultaMatriculaSETurma Turma { get; set; }
    }
}
