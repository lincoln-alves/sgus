using System;

namespace Sebrae.Academico.BP.DTO.Dominio
{
    public class DTOPublicacaoSaber : DTOEntidadeBasica
    {
        //public virtual bool Ativo { get; set; }
        public virtual string TextoResenha { get; set; }
        public virtual string TextoTitulo { get; set; }
        public virtual string TextoAssunto { get; set; }
        public virtual DateTime? DataPublicacao { get; set; }

        public string ListaAutores { get; set; }

    }
}