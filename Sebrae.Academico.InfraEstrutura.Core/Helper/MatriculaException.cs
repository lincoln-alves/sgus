using System;
using System.Runtime.Serialization;

namespace Sebrae.Academico.InfraEstrutura.Core.Helper
{
    public class MatriculaException : ApplicationException
    {
        public MatriculaException(string message) : base(message) { }

        protected MatriculaException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public MatriculaException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
