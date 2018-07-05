using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BP
{
    public class ManterPosicaoDadoCertificadoCertame : BusinessProcessBase
    {
        private readonly BMPosicaoDadoCertificadoCertame _bmPosicaoDadoCertificadoCertame;

        public ManterPosicaoDadoCertificadoCertame()
        {
            _bmPosicaoDadoCertificadoCertame = new BMPosicaoDadoCertificadoCertame();
        }

        public PosicaoDadoCertificadoCertame ObterPorDadoAno(string dado, int ano)
        {
            return _bmPosicaoDadoCertificadoCertame.ObterPorDadoAno(dado, ano);
        }
    }
}
