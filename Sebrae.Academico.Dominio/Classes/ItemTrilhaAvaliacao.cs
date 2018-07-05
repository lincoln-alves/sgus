using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class ItemTrilhaAvaliacao
    {
        public virtual int ID { get; protected set; }
        public virtual string Resenha { get; protected set; }
        public virtual int Avaliacao { get; protected set; }
        public virtual UsuarioTrilha UsuarioTrilha { get; protected set; }
        public virtual DateTime? DataAlteracao { get; set; }
        public virtual ItemTrilha ItemTrilha { get; protected set; }

        public ItemTrilhaAvaliacao()
        {

        }

        public ItemTrilhaAvaliacao(string resenha, int avaliacao, UsuarioTrilha usuarioTrilha, ItemTrilha itemTrilha)
        {
            Resenha = resenha;
            Avaliacao = avaliacao;
            UsuarioTrilha = usuarioTrilha;
            ItemTrilha = itemTrilha;
        }
    }
}
