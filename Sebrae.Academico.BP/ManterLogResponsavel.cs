using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BP
{
    public class ManterLogResponsavel : BusinessProcessBase
    {

        private BMLogResponsavel bmLog;

        public ManterLogResponsavel()
        {
            bmLog = new BMLogResponsavel();
        }

        public IEnumerable<LogResponsavel> ObterPorTurma(Turma turma)
        {
            return bmLog.ObterPorTurma(turma);
        }
    }
}
