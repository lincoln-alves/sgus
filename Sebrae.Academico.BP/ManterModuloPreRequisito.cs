using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BP
{
    public class ManterModuloPreRequisito : BusinessProcessBase
    {
        BMModuloPreRequisito _bmPreRequisitos = null;

        public ManterModuloPreRequisito()
        {
            _bmPreRequisitos = new BMModuloPreRequisito();
        }

        public bool VerificarPreRequisitoPendente(Modulo mod, int idUsuario)
        {
            var lista = _bmPreRequisitos.ObterPorModulo(mod.ID);

            if (!lista.Any())
                return false;

            return lista.Any(
                moduloPreRequisito =>
                    moduloPreRequisito.ModuloPai.ListaSolucaoEducacional.Any(
                        se =>
                            !new BMMatriculaOferta().AprovacaoPorUsuarioESolucaoEducacional(idUsuario,
                                se.SolucaoEducacional.ID)));
        }
    }
}
