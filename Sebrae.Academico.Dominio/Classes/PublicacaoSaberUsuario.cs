using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class PublicacaoSaberUsuario : EntidadeBasica
    {
        public virtual Usuario Usuario { get; set; }
        public virtual PublicacaoSaber PublicacaoSaber { get; set; }

        public override bool Equals(object obj)
        {
            PublicacaoSaberUsuario objeto = obj as PublicacaoSaberUsuario;
            return objeto == null ? false : PublicacaoSaber.Equals(objeto.PublicacaoSaber)
                && Usuario.Equals(objeto.Usuario);
        }

        public override int GetHashCode()
        {
            return PublicacaoSaber.ID.GetHashCode() + Usuario.ID.GetHashCode();
        }
    }

}