using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class ItemMetaInstitucional : EntidadeBasicaPorId
    {
        public virtual MetaInstitucional MetaInstitucional { get; set; }
        public virtual SolucaoEducacional SolucaoEducacional { get; set; }
        public virtual Usuario Usuario { get; set; }

        public override bool Equals(object obj)
        {
            ItemMetaInstitucional objeto = obj as ItemMetaInstitucional;
            return objeto == null ? false : MetaInstitucional.Equals(objeto.MetaInstitucional)
                && SolucaoEducacional.Equals(objeto.SolucaoEducacional);
        }

        public override int GetHashCode()
        {
            return MetaInstitucional.ID.GetHashCode() + SolucaoEducacional.ID.GetHashCode();
        }
    }

}