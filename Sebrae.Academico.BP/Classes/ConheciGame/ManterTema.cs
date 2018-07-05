using Sebrae.Academico.Dominio.Classes.ConheciGame;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System.Collections.Generic;
using System.Linq;

namespace Sebrae.Academico.BP.Classes.ConheciGame
{
    public class ManterTema : RepositorioBaseConheciGame<Tema>
    {
        public IEnumerable<Tema> ObterPorConteudo(int idConteudo)
        {
            return this.ObterTodosQueryble().Where(x => x.Conteudo.ID == idConteudo);
        }
    }
}
