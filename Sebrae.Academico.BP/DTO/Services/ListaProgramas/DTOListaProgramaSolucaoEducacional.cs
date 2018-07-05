using System.Collections.Generic;


namespace Sebrae.Academico.BP.DTO.Services.ListaProgramas
{
    public class DTOListaProgramaSolucaoEducacional
    {
        public virtual string NomeSolucaoEducacional { get; set; }
        public virtual string CodigoSolucaoEducacional { get; set; }
        public virtual List<DTOListaProgramaSolucaoEducacionalMatricula> ListaSolucaoEducacionalMatricula { get; set; }
        public virtual List<DTOListaProgramaSolucaoEducacionalTags> ListaTags { get; set; }

        public DTOListaProgramaSolucaoEducacional()
        {
            this.ListaSolucaoEducacionalMatricula = new List<DTOListaProgramaSolucaoEducacionalMatricula>();
            this.ListaTags = new List<DTOListaProgramaSolucaoEducacionalTags>();
        }
    }
}
