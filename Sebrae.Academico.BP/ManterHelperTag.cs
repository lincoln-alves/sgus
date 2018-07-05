using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BP
{
    public class ManterHelperTag : BusinessProcessBase
    {
        private readonly BMHelperTag _bmHelperTag;

        public ManterHelperTag()
        {
            _bmHelperTag = new BMHelperTag();
        }

        public IQueryable<HelperTag> ObterTodos()
        {
            return _bmHelperTag.ObterTodos();
        }

        public HelperTag ObterPorId(int id)
        {
            return _bmHelperTag.ObterPorId(id);
        }

        public HelperTag ObterPorChavePagina(string chave, int paginaId)
        {
            return _bmHelperTag.ObterPorChavePagina(chave, paginaId);
        }

        public void Salvar(HelperTag helper)
        {
            _bmHelperTag.Salvar(helper);
        }
    }
}
