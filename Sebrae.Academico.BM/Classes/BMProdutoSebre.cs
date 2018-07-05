using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.BM.Classes
{
    public class BMProdutoSebre : BusinessManagerBase, IDisposable
    {
        public RepositorioBase<ProdutoSebrae> repositorio { get; set; }
        public BMProdutoSebre()
        {
            repositorio = new RepositorioBase<ProdutoSebrae>();
        }

        public void Dispose()
        {
            repositorio.Dispose();
        }
    }
}
