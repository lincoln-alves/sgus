using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;
using System.Linq;

namespace Sebrae.Academico.BP
{
    public class ManterModulo : BusinessProcessBase
    {

        private BMModulo bmModulo = null;

        public ManterModulo()
            : base()
        {
            bmModulo = new BMModulo();
        }
              
        public Modulo ObterPorId(int pId)
        {
            return bmModulo.ObterPorId(pId);
        }

        public IQueryable<Modulo> ObterPorCapacitacaoIQueryable(int idCapacitacao)
        {
            return bmModulo.ObterPorCapacitacao(idCapacitacao);
        }

        public IList<Modulo> ObterPorCapacitacao(int idCapacitacao)
        {
            return bmModulo.ObterPorCapacitacao(idCapacitacao).ToList();
        }

        public IList<Modulo> ObterPorPrograma(int idPrograma)
        {
            var query = bmModulo.ObterTodosIQueryable();
            return query.Where(p => p.Capacitacao.Programa.ID == idPrograma).ToList();
        }

        public IList<Modulo> ObterTodos()
        {
            return bmModulo.ObterTodosIQueryable().ToList();
        } 
    }
}