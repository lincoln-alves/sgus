using System;
using System.Collections.Generic;

namespace Sebrae.Academico.BP.DTO.Services
{
    public class DTOManterTurma
    {
        public virtual string NomedaTurma { get; set; }
        public virtual string NomeProfessor { get; set; }
        public virtual string CPFProfessor { get; set; }
        public virtual string EmailProfessor { get; set; }
        public virtual string IDChaveExternaTurma { get; set; }
        public virtual string IDChaveExternaTurmaNova { get; set; }
        public virtual string IDChaveExternaOferta { get; set; }
        public virtual string Local { get; set; }
        public virtual DateTime DataInicio { get; set; }
        public virtual DateTime? DataFinal { get; set; }
        public virtual string TipoTutoria { get; set; }
        public virtual int QuantideInscritos { get; set; }

        public virtual int IDQuestionarioPre { get; set; }
        public virtual int IDQuestionarioPos { get; set; }
        public virtual int IDQuestionarioProva { get; set; }

        //public virtual List<DTOMatriculaSE> ListaMaticulaAlunoTurma { get; set; }

        public virtual List<DTOManterTurmaProfessor> ListaTurmaProfessor { get; set; }
        public int ID { get;  set; }

        public DTOManterTurma()
        {
            this.ListaTurmaProfessor = new List<DTOManterTurmaProfessor>();
            QuantideInscritos = 0;
        }
    }
}
