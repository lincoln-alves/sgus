using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BP
{
    public class ManterEstiloItemQuestionario : BusinessProcessBase
    {
        private BMEstiloItemQuestionario estiloItemQuestionario;

        public ManterEstiloItemQuestionario()
            : base()
        {
            estiloItemQuestionario = new BMEstiloItemQuestionario();
        }

        public void IncluirEstiloItemQuestionario(EstiloItemQuestionario pEstiloItemQuestionario)
        {
            estiloItemQuestionario.Salvar(pEstiloItemQuestionario);
        }

        public void AlterarTipoQuestionario(EstiloItemQuestionario pEstiloItemQuestionario)
        {
            estiloItemQuestionario.Salvar(pEstiloItemQuestionario);
        }

        public IList<EstiloItemQuestionario> ObterTodosEstiloItemQuestionario()
        {
            return estiloItemQuestionario.ObterTodos();
        }

        public EstiloItemQuestionario ObterEstiloItemQuestionarioPorID(int pId)
        {
            return estiloItemQuestionario.ObterPorID(pId);
        }

        public IList<EstiloItemQuestionario> ObterEstiloItemQuestionarioPorFiltro(EstiloItemQuestionario pEstiloItemQuestionario)
        {
            return estiloItemQuestionario.ObterPorFiltro(pEstiloItemQuestionario);
        }
    }
}
