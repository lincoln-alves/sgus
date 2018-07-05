using System;
using System.Collections.Generic;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    public class MatriculaTurma : EntidadeBasicaPorId
    {
        public virtual Turma Turma { get; set; }
        public virtual DateTime DataMatricula { get; set; }
        public virtual DateTime DataLimite { get; set; }
        public virtual double? Nota1 { get; set; }
        public virtual double? Nota2 { get; set; }
        public virtual double? MediaFinal { get; set; }
        public virtual double? PercentualPresenca { get; set; }
        public virtual DateTime? DataTermino { get; set; }
        public virtual MatriculaOferta MatriculaOferta { get; set; }
        public virtual double? ValorNotaProvaOnline { get; set; }
        public virtual string Observacao { get; set; }
        public virtual string Feedback { get; set; }
        public virtual IList<QuestionarioParticipacao> Questionarios { get; set; }
        public virtual int TotalPresencas { get; set; }
        public virtual int Presencas { get; set; }


        #region "Propriedades que não serão mapeadas"

        public virtual Turma TurmaAnterior { get; set; }

        #endregion

        public virtual DateTime CalcularDataLimite(Oferta oferta = null)
        {
            oferta = oferta ?? MatriculaOferta.Oferta;

            if (oferta.TipoOferta == enumTipoOferta.Continua)
            {
                if (oferta.DiasPrazo == null)
                    throw new Exception("Campo \"Prazo para realização (dias)\" precisa ser preenchido para ofertas contínuas.");

                if (Turma.DataInicio == null)
                    throw new Exception("Campo \"Data Início\" da turma precisa ser preenchido.");

                return DataMatricula.AddDays(oferta.DiasPrazo.Value);
            }
            if (Turma.DataFinal == null)
                throw new Exception("Campo \"Data Final\" da Turma precisa ser preenchido para obter a data limite.");

            return Turma.DataFinal.Value;
        }
    }
}
