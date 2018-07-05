using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BP
{
    public class ManterMetaFm : BusinessProcessBase
    {

        private readonly BMMetaFm _bm;

        public ManterMetaFm()
        {
            _bm = new BMMetaFm();
        }

        public IEnumerable<MetaFm> ObterTodos()
        {
            return _bm.ObterTodos();
        }
    }
}