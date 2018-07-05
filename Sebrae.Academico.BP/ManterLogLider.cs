using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BP
{
    public class ManterLogLider : BusinessProcessBase
    {
        private readonly BMLogLider _bmLogLider;

        public ManterLogLider()
        {
            _bmLogLider = new BMLogLider();
        }

        public IQueryable<LogLider> ObterPorAlunoPontoSebrae(UsuarioTrilha aluno, PontoSebrae pontoSebrae)
        {
            return _bmLogLider.ObterPorAlunoPontoSebrae(aluno, pontoSebrae);
        }

        public LogLider Salvar(LogLider logLider)
        {
            return _bmLogLider.Salvar(logLider);
        }
    }
}
