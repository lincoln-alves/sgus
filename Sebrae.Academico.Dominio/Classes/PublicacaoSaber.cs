using System;
using System.Collections.Generic;

namespace Sebrae.Academico.Dominio.Classes
{
    public class PublicacaoSaber : EntidadeBasica
    {
        public virtual Uf UF { get; set; }
        public virtual int IDChaveExterna { get; set; }
        public virtual bool Publicado { get; set; }
        public virtual string TextoResenha { get; set; }
        public virtual string TextoAssunto { get; set; }
        public virtual DateTime? DataPublicacao { get; set; }
        public virtual string TextoLinkCapa { get; set; }
        public virtual string TextoTitulo { get; set; }
        public virtual IList<PublicacaoSaberUsuario> ListaPublicacaoSaberUsuario { get; set; }

        public PublicacaoSaber()
        {
            this.ListaPublicacaoSaberUsuario = new List<PublicacaoSaberUsuario>();
        }

    }

}