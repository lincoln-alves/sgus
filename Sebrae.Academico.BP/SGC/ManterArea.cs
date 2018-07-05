using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes.SGC;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Classes.SGC;

namespace Sebrae.Academico.BP.SGC
{
    public class ManterArea : BusinessProcessBase
    {
        private readonly BMArea _bmArea;

        public IEnumerable<Area> ObterTodos()
        {
            return _bmArea.ObterTodos().OrderBy(x => x.ID);
        }

        public Area ObterPorID(int id)
        {
            return _bmArea.ObterPorID(id);
        }

        public ManterArea()
        {
            _bmArea = new BMArea();
        }
    }
}

