using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP
{
    public class ManterMatriculaCapacitacao : BusinessProcessBase
    {
        private BMMatriculaCapacitacao bmMatriculaCapacitacao = null;
        public ManterMatriculaCapacitacao(): base(){
            bmMatriculaCapacitacao = new BMMatriculaCapacitacao();
        }

        public void AtualizarMatriculaCapacitacao(MatriculaCapacitacao matriculaCapacitacao) {
            if (matriculaCapacitacao == null) return;
            if (matriculaCapacitacao.StatusMatricula.Equals(enumStatusMatricula.CanceladoAdm)) {
                if (matriculaCapacitacao.ListaMatriculaTurmaCapacitacao != null) {
                    new BMMatriculaTurmaCapacitacao().ExcluirLista(matriculaCapacitacao.ListaMatriculaTurmaCapacitacao);
                }
            }
            var lsIgnorarStatusMatricula = new List<enumStatusMatricula> {
                enumStatusMatricula.FilaEspera,
                enumStatusMatricula.Inscrito,
                enumStatusMatricula.PendenteConfirmacaoAluno
            };
            if (!lsIgnorarStatusMatricula.Contains(matriculaCapacitacao.StatusMatricula) && !matriculaCapacitacao.DataFim.HasValue){
                matriculaCapacitacao.DataFim = DateTime.Now;
            }else{
                matriculaCapacitacao.DataFim = null;
            }
            bmMatriculaCapacitacao.Salvar(matriculaCapacitacao);
        }

        public MatriculaCapacitacao ObterPorId(int id) {
            return bmMatriculaCapacitacao.ObterPorId(id);
        }
    }
}
