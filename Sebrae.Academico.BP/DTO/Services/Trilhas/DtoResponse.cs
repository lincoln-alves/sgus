using System;
using System.Collections.Generic;
using Sebrae.Academico.BP.DTO.Services.Trilhas.Solucao;

namespace Sebrae.Academico.BP.DTO.Services.Trilhas
{
    public class DtoResponse
    {
        public DtoResponse(object data)
        {
            Data = data;
            StatusCode = 0;
        }

        public DtoResponse(object data, List<DTOMensagemGuia> mensagensGuia)
        {
            Data = data;
            StatusCode = 0;
            MensagensGuia = mensagensGuia;
        }

        public DtoResponse(string message)
        {
            StatusCode = 100;
            Message = message;
        }

        public DtoResponse(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        /// <summary>
        /// referência ao inteiro do enum enumResponseStatusCode        
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Mensagem amigável de erro.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Retorno da requisição quando sucesso.
        /// </summary>
        public object Data { get; set; }
        
        /// <summary>
        /// Retorno da excessão quando houver.
        /// </summary>
        public object Stack { get; set; }

        public List<DTOMensagemGuia> MensagensGuia { get; set; }
    }
}
