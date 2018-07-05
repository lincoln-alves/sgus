using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BP
{
    public class ManterLogConsultorEducacional : BusinessProcessBase
    {

        private BMLogConsultorEducacional bmLog;

        public ManterLogConsultorEducacional()
        {
            bmLog = new BMLogConsultorEducacional();
        }

        public IEnumerable<LogConsultorEducacional> ObterPorTurma(Turma turma)
        {
            return bmLog.ObterPorTurma(turma);
        }
    }
}
