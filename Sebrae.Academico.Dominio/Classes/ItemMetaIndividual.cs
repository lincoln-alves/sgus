using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class ItemMetaIndividual : EntidadeBasicaPorId
    {
        public virtual MetaIndividual MetaIndividual { get; set; }
        public virtual SolucaoEducacional SolucaoEducacional { get; set; }

        public override bool Equals(object obj)
        {
            ItemMetaIndividual objeto = obj as ItemMetaIndividual;
            return objeto == null ? false : MetaIndividual.Equals(objeto.MetaIndividual)
                && SolucaoEducacional.Equals(objeto.SolucaoEducacional);
        }

        public override int GetHashCode()
        {
            return MetaIndividual.ID.GetHashCode() + SolucaoEducacional.ID.GetHashCode();
        }
    }
}
