using System;
using System.Runtime.Serialization;

namespace Sebrae.Academico.InfraEstrutura.Core.Helper
{
    public class AcademicoException : ApplicationException
    {
        public int ExceptionCode { get; set; }

        public AcademicoException(string message) : base(message) { }


        protected AcademicoException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public AcademicoException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }

        public AcademicoException(string message, int exceptionCode)
            : base(message)
        {
            ExceptionCode = exceptionCode;
        }
    }
}
