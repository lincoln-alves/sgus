using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class ProgramaSolucaoEducacional : EntidadeBasicaPorId
    {
        public virtual Programa Programa { get; set; }
        public virtual SolucaoEducacional SolucaoEducacional { get; set; }

        public override bool Equals(object obj)
        {
            ProgramaSolucaoEducacional objeto = obj as ProgramaSolucaoEducacional;
            return objeto == null ? false : Programa.Equals(objeto.Programa)
                && SolucaoEducacional.Equals(objeto.SolucaoEducacional);
        }

        public override int GetHashCode()
        {
            return Programa.ID.GetHashCode() + SolucaoEducacional.ID.GetHashCode();
        }
    }
}
