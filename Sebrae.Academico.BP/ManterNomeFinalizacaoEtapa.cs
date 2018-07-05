using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BP
{

    public class ManterNomeFinalizacaoEtapa
    {
        public List<NomeFinalizacaoEtapa> ObterTodos()
        {
            return new BMNomeFinalizacaoEtapa().ObterTodos();
        }

        public NomeFinalizacaoEtapa ObterPorId(int id)
        {
            return new BMNomeFinalizacaoEtapa().ObterPorId(id);
        }
    }
}
