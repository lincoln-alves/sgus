using System;

namespace Sebrae.Academico.BP.DTO.Services
{
   
    [Serializable]
    public class RetornoWebService : IRetornoWebService
    {
        public int Erro { get; set; }
        public string Mensagem { get; set; }
        public string Stack { get; set; }
    }
}