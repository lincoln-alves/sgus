using Sebrae.Academico.Dominio.DTO;
using System.Collections.Generic;

namespace Sebrae.Academico.BP.DTO.Dominio
{
    public class DTOObjetivo
    {
        public int ID { get; set; }
        public string Nome { get; set; }

        public List<DTOFormaAquisicao> FormasAquisicao { get; set; }
        public List<DtoTrilhaSolucaoSebrae> Solucoes { get; set; }
    }
}
