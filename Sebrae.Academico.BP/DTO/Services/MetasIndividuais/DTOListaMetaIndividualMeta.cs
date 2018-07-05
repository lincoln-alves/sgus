using System;
using System.Collections.Generic;

namespace Sebrae.Academico.BP.DTO.Services.MetasIndividuais
{
    public class DTOListaMetaIndividualMeta
    {
        public virtual string NomeMetaIndividual { get; set; }
        public virtual DateTime ValidadeMetaIndividual { get; set; }
        public virtual List<DTOListaMetaIndividualItemMeta> ListaItemMetaIndividual { get; set; }

        public DTOListaMetaIndividualMeta()
        {
            ListaItemMetaIndividual = new List<DTOListaMetaIndividualItemMeta>();
        }
    }
}
