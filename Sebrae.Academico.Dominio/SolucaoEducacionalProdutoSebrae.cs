using Sebrae.Academico.Dominio.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.Dominio
{
    public class SolucaoEducacionalProdutoSebrae : EntidadeBasica
    {
        public virtual SolucaoEducacional SolucaoEducacional { get; set; }
        public virtual ProdutoSebrae ProdutoSebrae { get; set; }

        public override bool Equals(object obj)
        {
            var objeto = obj as SolucaoEducacionalProdutoSebrae;
            return objeto == null ? false : SolucaoEducacional.ID == objeto.SolucaoEducacional.ID
                && ProdutoSebrae.ID == objeto.ProdutoSebrae.ID;
        }

        public override int GetHashCode()
        {
            int result = 17;
            result = 31 * result + ID;
            result = 31 * result + SolucaoEducacional.GetHashCode();
            result = 31 * result + ProdutoSebrae.GetHashCode();
            return result;
        }
    }
}
