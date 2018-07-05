using System;
using System.Collections.Generic;

namespace Sebrae.Academico.BP.DTO.Services
{
    public class DTOManterOferta
    {
        public virtual string NomedaOferta { get; set; }
        public virtual string TipoOferta { get; set; }
        public virtual string IDChaveExternaOferta { get; set; }
        public virtual string IDChaveExternaOfertaNova { get; set; }
        public virtual bool FiladeEspera { get; set; }
        public virtual DateTime? DataInicioInscricoes { get; set; }
        public virtual DateTime? DataFimInscricoes { get; set; }
        public virtual bool? InscricaoOnline { get; set; }
        public virtual string IDChaveExternaSolucaoEducacional { get; set; }
        public virtual int QTCargaHoraria { get; set; }
        public virtual int? QTDiasPrazo { get; set; }
        public virtual string CodigoMoodle { get; set; }
        public virtual int QuantidadeMaximaInscricoes { get; set; }
        public virtual bool ProximaOferta { get; set; }
        public virtual int IdCertificado { get; set; }
        public virtual int IDOferta { get; set; }
        public virtual List<DTOMatriculaSE> ListaMatriculaAlunoOferta { get; set; }
        public virtual List<DTOManterTurma> ListaTurmasOferta { get; set; }

        public DTOManterOferta()
        {
            this.ListaMatriculaAlunoOferta = new List<DTOMatriculaSE>();
            this.ListaTurmasOferta = new List<DTOManterTurma>();
        }
    }
}
