using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sebrae.Academico.InfraEstrutura.Core.Helper
{
    public class EmailException : ApplicationException
    {
        public EmailException(string message) : base(message) { }

        protected EmailException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public EmailException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
