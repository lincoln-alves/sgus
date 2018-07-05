using Sebrae.Academico.BM.Classes.SGC;

namespace Sebrae.Academico.BP.SGC
{
    public class ManterCredenciadoArea : BusinessProcessBase
    {
        private readonly BMCredenciadoArea _bmCredenciadoArea;

        public ManterCredenciadoArea()
        {
            _bmCredenciadoArea = new BMCredenciadoArea();
        }
    }
}

