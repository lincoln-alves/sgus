using Sebrae.Academico.Dominio.Enumeracao;
using System;
using Sebrae.Academico.InfraEstrutura.Core.Extensions.Others;

namespace Sebrae.Academico.InfraEstrutura.Core.Helper
{
    public class ResponseException : ApplicationException
    {
        public enumResponseStatusCode StatusCode { get; set; }

        public ResponseException(enumResponseStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }

        public ResponseException(enumResponseStatusCode statusCode) : base(statusCode.GetDescription())
        {
            StatusCode = statusCode;
        }
    }
}