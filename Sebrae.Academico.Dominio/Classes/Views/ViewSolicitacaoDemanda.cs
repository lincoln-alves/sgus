using Sebrae.Academico.Dominio.Enumeracao;
using System;
using System.ComponentModel;

namespace Sebrae.Academico.Dominio.Classes
{
    public class ViewSolicitacaoDemanda
    {
        public virtual int IdProcessoResposta { get; set; }
        public virtual Processo Processo { get; set; }
        public virtual Usuario UsuarioDemandante { get; set; }

        public virtual string CargaHoraria { get; set; }
        public virtual DateTime? DataAbertura { get; set; }
        public virtual DateTime DtInicioCapacitacao { get; set; }
        public virtual DateTime DtTerminoCapacitacao { get; set; }
        public virtual string Local { get; set; }
        public virtual string Titulo { get; set; }

        public virtual int IN_Status { get; set; }
        
        public virtual Etapa EtapaAtual { get; set; }

        public virtual string ValorPrevistoInscricao { get; set; }
        public virtual string ValorPrevistoPassagem { get; set; }
        public virtual string ValorPrevistoDiaria { get; set; }

        public virtual string ValorExecutadoInscricao { get; set; }
        public virtual string ValorExecutadoPassagem { get; set; }
        public virtual string ValorExecutadoDiaria { get; set; }

        public virtual float ValorTotalPrevisto { get; set; }
        public virtual float ValorTotalExecutado { get; set; }

        public virtual string Status
        {
            get { return GetDescription((enumStatusProcessoResposta)IN_Status); }
        }

        
        public virtual DateTime? DataInicioCapacitacao
        {
            get {

                if(DtInicioCapacitacao.Date != DateTime.MinValue)
                {
                    return DtInicioCapacitacao.Date;
                }
                else
                {
                    return null;
                }
                
            }
        }

        public virtual DateTime? DataTerminoCapacitacao
        {
            get {

                if (DtTerminoCapacitacao.Date != DateTime.MinValue)
                {
                    return DtTerminoCapacitacao.Date;
                }
                else
                {
                    return null;
                }

            }
        }
        
        private static string GetDescription(Enum element)
        {
            var type = element.GetType();

            var memberInfo = type.GetMember(element.ToString());

            if (memberInfo.Length <= 0) return element.ToString();

            var attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? ((DescriptionAttribute)attributes[0]).Description : element.ToString();
        }
    }
}
