using System.Collections.Generic;


namespace Sebrae.Academico.BP.DTO.Services.ListaProgramas
{
    public class DTOListaProgramaPrograma 
    {
        public virtual string NomePrograma { get; set; }
        public virtual string CodigoPrograma { get; set; }
        public virtual List<DTOListaProgramaMatriculaPrograma>  ListaMatriculaPrograma { get; set; }
        public virtual List<DTOListaProgramaSolucaoEducacional> ListaSolucaoEducacional { get; set; }

        public DTOListaProgramaPrograma()
        {
            this.ListaMatriculaPrograma = new List<DTOListaProgramaMatriculaPrograma>();
            this.ListaSolucaoEducacional = new List<DTOListaProgramaSolucaoEducacional>();
        } 
    }
}
