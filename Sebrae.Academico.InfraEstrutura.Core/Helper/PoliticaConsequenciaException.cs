using System;
using System.Runtime.Serialization;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.InfraEstrutura.Core.Helper
{
    public class PoliticaConsequenciaException : ApplicationException
    {
        public enumRespostaPoliticaDeConsequencia Consequencia { get; set; }
        public MatriculaOferta MatriculaOferta { get; set; }
        
        public PoliticaConsequenciaException(string message, enumRespostaPoliticaDeConsequencia consequencia) : base(message)
        {
            Consequencia = consequencia;
        }


        protected PoliticaConsequenciaException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public PoliticaConsequenciaException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }

    }
}
