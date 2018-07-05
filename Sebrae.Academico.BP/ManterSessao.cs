using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Sebrae.Academico.BP
{
    public class ManterSessao : RepositorioBase<Sessao>
    {

        public Sessao ObterObjetoPorId(int id)
        {
            var jsSerialize = new JavaScriptSerializer();
            return ObterTodos().FirstOrDefault(x => (int)jsSerialize.Deserialize<dynamic>(x.Valor)["Id"] == id);
        }

        public Sessao ObterObjetoPorHash(int hash)
        {
            var sessao = ObterTodosIQueryable().FirstOrDefault(x => x.Hash == hash);
            return sessao;
        }

        public void ExcluirPorHash(int hash)
        {
            var sessao = ObterTodosIQueryable().FirstOrDefault(x => x.Hash == hash);
            Excluir(sessao);
        }
    }
}
