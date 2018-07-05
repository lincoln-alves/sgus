using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using System.Collections.Generic;

namespace Sebrae.Academico.Dominio.DTO
{
    public class DtoTrilhaSolucaoSebraeMissoes
    {
        public virtual int ID { get; set; }
        public virtual string Nome { get; set; }
        //public virtual PontoSebrae PontoSebrae { get; set; }

        //public virtual IList<ItemTrilha> ListaItemTrilha { get; set; }
    }
}
