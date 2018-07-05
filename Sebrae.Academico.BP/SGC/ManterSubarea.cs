using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes.SGC;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Classes.SGC;

namespace Sebrae.Academico.BP.SGC
{
    public class ManterSubarea : BusinessProcessBase
    {
        private readonly BMSubarea _bmSubarea;

        public IEnumerable<Subarea> ObterTodos()
        {
            return _bmSubarea.ObterTodos().OrderBy(x => x.ID);
        }

        public IEnumerable<Subarea> ObterTodosPorUsuarioNaoPertencentesOferta(Usuario usuario, Oferta oferta)
        {
            return _bmSubarea.ObterTodosPorUsuario(usuario).Where(
                x => oferta.ListaPermissao.Any(p => !p.Subareas.Select(s => s.ID).Contains(x.ID))).OrderBy(x => x.ID);
        }

        public Subarea ObterPorID(int id)
        {
            return _bmSubarea.ObterPorID(id);
        }

        public ManterSubarea()
        {
            _bmSubarea = new BMSubarea();
        }

        public IEnumerable<Subarea> ObterPorUsuario(Usuario usuario)
        {
            return _bmSubarea.ObterTodosPorUsuario(usuario);
        }
    }
}

