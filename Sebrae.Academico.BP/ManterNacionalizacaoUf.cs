using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using System.Linq;

namespace Sebrae.Academico.BP
{
    public class ManterNacionalizacaoUf : BusinessProcessBase
    {
        private readonly BMNacionalizacaoUf _nacionalizacaoUfBm;

        public ManterNacionalizacaoUf()
        {
            _nacionalizacaoUfBm = new BMNacionalizacaoUf();
        }

        /// <summary>
        /// Verifica se a UF esta nacionalizada.
        /// </summary>
        /// <param name="uf">UF</param>
        /// <returns>bool resultado</returns>
        public bool IsNacionalizado(Uf uf)
        {
            bool resultado = false;

            var listanacionalidacaoUf = _nacionalizacaoUfBm.ObterTodos().Where(x => x.Uf.ID == uf.ID);
            if (listanacionalidacaoUf.Any())
            {
                resultado = true;
            }

            return resultado;
        }
    }
}
