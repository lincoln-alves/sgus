using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BP
{

    public class ManterNomeReprovacaoEtapa
    {
        public List<NomeReprovacaoEtapa> ObterTodos()
        {
            return new BMNomeReprovacaoEtapa().ObterTodos();
        }

        public NomeReprovacaoEtapa ObterPorId(int id)
        {
            return new BMNomeReprovacaoEtapa().ObterPorId(id);
        }
    }
}
