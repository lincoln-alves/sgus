using System;

namespace Sebrae.Academico.BP.DTO.Services.Trilhas
{
    [Serializable]
    public class RetornoTokenTrilha : RetornoWebService
    {
        public string NomeTrilha { get; set; }
        public Boolean Experimenta { get; set; }

        public RetornoTokenTrilha()
        {
            Mensagem = "";            
            Erro = 0;
            NomeTrilha = "";
        }
        
        public string Token { get; set; }
    }
}