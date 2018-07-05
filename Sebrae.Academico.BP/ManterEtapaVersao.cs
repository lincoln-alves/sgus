using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.BP
{
    public class ManterEtapaVersao : RepositorioBase<EtapaVersao>, IDisposable
    {

        public int? ObterVersaoAtualProcesso(Processo processo)
        {
            var query = ObterTodosIQueryable();
            query = query.Where(x => x.Etapa.Processo.ID == processo.ID).Distinct();

            var etapa = query.ToList().OrderByDescending(x => x.Ordem).LastOrDefault();

            return etapa != null ? etapa.Versao : 0;
        }

        public int? ObterVersaoAtualEtapa(Etapa etapa)
        {
            var query = ObterTodosIQueryable();
            query = query.Where(x => x.Etapa.ID == etapa.ID).Distinct();

            var etapaVersao = query.ToList().OrderByDescending(x => x.Ordem).LastOrDefault();

            return etapaVersao != null ? etapaVersao.Versao : (int?)null;
        }
    }
}
