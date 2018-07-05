using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using System.Linq;

namespace Sebrae.Academico.BP
{
    public class ManterOfertaPermissao : BusinessProcessBase
    {

        private BMOfertaPermissao bmOfertaPermissao;

        public ManterOfertaPermissao()
            : base()
        {
            bmOfertaPermissao = new BMOfertaPermissao();
        }

        public void Salvar(OfertaPermissao ofertaPermissao)
        {
            bmOfertaPermissao.Salvar(ofertaPermissao);
        }

        public OfertaPermissao ObterExistente(OfertaPermissao ofertaPermissao)
        {
            return bmOfertaPermissao.ObterExistente(ofertaPermissao);
        }
    }
}