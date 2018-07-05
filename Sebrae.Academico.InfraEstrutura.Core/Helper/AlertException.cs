using System;
using System.Runtime.Serialization;

namespace Sebrae.Academico.InfraEstrutura.Core.Helper
{
    public class AlertException : ApplicationException
    {

        public AlertException(string message) : base(message) { }


        protected AlertException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public AlertException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

    }
}