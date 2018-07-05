using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;
using System.Linq;

namespace Sebrae.Academico.BP
{
    public class ManterOfertaGerenciadorVaga : BusinessProcessBase
    {

        private BMOfertaGerenciadorVaga bmOfertaGerenciadorVaga = null;

        public ManterOfertaGerenciadorVaga()
            : base()
        {
            bmOfertaGerenciadorVaga = new BMOfertaGerenciadorVaga();
        }

        public void Cadastrar(OfertaGerenciadorVaga ofertaGerenciadorVaga)
        {
            bmOfertaGerenciadorVaga.Cadastrar(ofertaGerenciadorVaga);
        }
    }
}