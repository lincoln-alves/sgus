using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class UsuarioAbandono: EntidadeBasicaPorId
    {
        public virtual DateTime DataInicioAbandono { get; set; }
        public virtual DateTime DataFimAbandono { get; set; }
        public virtual bool Desconsiderado { get; set; }
        public virtual string Origem { get; set; }
        public virtual Usuario Usuario { get; set; }

        public override bool Equals(object obj)
        {
            UsuarioAbandono objeto = obj as UsuarioAbandono;
            return objeto == null ? false : Usuario.Equals(objeto.Usuario)
                && this.DataInicioAbandono.Equals(objeto.DataInicioAbandono);
        }

        public override int GetHashCode()
        {
            return Usuario.ID.GetHashCode() + DataInicioAbandono.GetHashCode();
        }
    }
}
