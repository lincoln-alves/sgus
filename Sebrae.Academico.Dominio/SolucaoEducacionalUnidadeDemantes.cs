using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.Dominio
{
    public class SolucaoEducacionalUnidadeDemantes : EntidadeBasica
    {
        public virtual SolucaoEducacional SolucaoEducacional { get; set; }
        public virtual Cargo Cargo { get; set; }

        public override bool Equals(object obj)
        {
            var objeto = obj as SolucaoEducacionalUnidadeDemantes;
            return objeto == null ? false : SolucaoEducacional.ID == objeto.SolucaoEducacional.ID
                && Cargo.ID == objeto.Cargo.ID;
        }

        public override int GetHashCode()
        {
            int result = 17;
            result = 31 * result + ID;
            result = 31 * result + SolucaoEducacional.GetHashCode();
            result = 31 * result + Cargo.GetHashCode();
            return result;
        }
    }
}
