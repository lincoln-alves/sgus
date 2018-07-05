
using System.Collections.Generic;
namespace Sebrae.Academico.Trilhas.Dominio.Classes
{
    public class TrilhaTopicoTematico: EntidadeBasica
    {
        public virtual string DescricaoTextoEnvio { get; set; }
        public virtual string DescricaoArquivoEnvio { get; set; }
        public virtual string QtdMinimaPontosAtivFormativa { get; set; }
        
        public virtual IList<ItemTrilha> ListaItemTrilha { get; set; }

        public virtual IList<TrilhaAtividadeInformativaParticipacao> ListaTrilhaAtividadeInformativaParticipacao { get; set; }
    }
}
