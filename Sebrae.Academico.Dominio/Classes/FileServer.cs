using System.Collections.Generic;
using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class FileServer : EntidadeBasicaPorId
    {
        public virtual string NomeDoArquivoOriginal { get; set; }
        public virtual string TipoArquivo { get; set; }
        public virtual string NomeDoArquivoNoServidor { get; set; }
        public virtual bool MediaServer { get; set; }
        public virtual IList<ItemTrilha> ListaItemTrilha { get; set; }
        public virtual IList<Usuario> ListaUsuario { get; set; }
        public virtual IList<TrilhaAtividadeFormativaParticipacao> ListaTrilhaAtividadeFormativaParticipacao { get; set; }
        //public virtual IList<TrilhaTopicoTematico> ListaTrilhaTopicoTematico { get; set; }
        public virtual IList<ItemTrilhaParticipacao> ListaItemTrilhaParticipacao { get; set; }

        public virtual Uf Uf { get; set; }

        public virtual ProtocoloFileServer ProtocoloFileServer { get; set; }
    }
}
