using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BP
{
    public class ManterTurmaCapacitacao : BusinessProcessBase
    {
        private BMTurmaCapacitacao bmTurmaCapacitacao = null;

        public ManterTurmaCapacitacao()
            : base()
        {
            bmTurmaCapacitacao = new BMTurmaCapacitacao();
        }

        public IList<TurmaCapacitacao> ObterPorIdPrograma(int idPrograma) {
            return bmTurmaCapacitacao.ObterPorPrograma(idPrograma).ToList();
        }

        public IList<TurmaCapacitacao> ObterPorOferta(int idOferta) {
            return bmTurmaCapacitacao.ObterPorCapacitacao(idOferta).ToList();
        }

        public TurmaCapacitacao ObterPorId(int idTurmaCapacitacao) {
            return bmTurmaCapacitacao.ObterPorId(idTurmaCapacitacao);
        }
    }
}
