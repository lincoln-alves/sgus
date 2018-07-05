using System.Collections.Generic;


namespace Sebrae.Academico.BP.DTO.Services.StatusTrilha
{
    public class RetornoStatusTrilha
    {
        public virtual RetornoWebService Retorno { get; set; }
        public virtual List<TrilhaDTO> ListaTrilhas { get; set; }

        public RetornoStatusTrilha()
        {
            Retorno = new RetornoWebService();
            ListaTrilhas = new List<TrilhaDTO>();
        }
    }
}
