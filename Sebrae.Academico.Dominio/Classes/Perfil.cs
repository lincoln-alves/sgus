using System;
using System.Collections.Generic;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    public class Perfil: EntidadeBasicaPorId, ICloneable
    {
     
        public virtual string Nome { get; set; }

        public virtual IEnumerable<RelatorioPaginaInicial> ListaRelatorioPaginaInicial { get; set; }
                 
        #region "Implicit Operator"

        /// <summary>
        /// Conversão Implicita para fazermos atribuições com enumeradores do Tipo enumPerfil,
        /// pois o C# não faz conversão implicita.
        /// </summary>
        /// <param name="pPerfil"></param>
        /// <returns></returns>
        public static implicit operator Perfil(enumPerfil pPerfil)
        {
            return new Perfil() { ID = (int)pPerfil };
        }

        /// <summary>
        /// Conversão Implicita para fazermos atribuições com enumeradores do Tipo enumPerfil,
        /// pois o C# não faz conversão implicita.
        /// </summary>
        /// <param name="pPerfil"></param>
        /// <returns></returns>
        public static implicit operator enumPerfil?(Perfil pPerfil)
        {
            enumPerfil? perfil = null;

            if (pPerfil != null && pPerfil.ID > 0)
            {
                perfil = (enumPerfil)pPerfil.ID;
            }

            return perfil;
        }

        #endregion

        #region ICloneable Members

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
        
}
