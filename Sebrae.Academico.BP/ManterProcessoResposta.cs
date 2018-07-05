using Sebrae.Academico.BM.Views;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.BP
{
    public class ManterProcessoResposta : RepositorioBase<ProcessoResposta>, IDisposable
    {
        private RepositorioBase<EtapaVersao> manterEtapaVersao = null;
        private RepositorioBase<Etapa> manterEtapa = null;

        public ManterProcessoResposta()
        {
            manterEtapaVersao = new RepositorioBase<EtapaVersao>();
            manterEtapa = new RepositorioBase<Etapa>();
        }

        /// <summary>
        /// Obtem as etapas referentes a versão do Processo Resposta
        /// </summary>
        /// <param name="processo"></param>
        /// <returns></returns>
        public IEnumerable<Etapa> ObterEtapasPorVersao(ProcessoResposta processoResposta)
        {
            var versoes = manterEtapaVersao.ObterTodosIQueryable().Where(x => processoResposta.VersaoEtapa != null && x.Etapa.Processo.ID == processoResposta.Processo.ID && x.Versao == processoResposta.VersaoEtapa).ToList();

            var etapas = versoes != null && versoes.Any() ? versoes.OrderBy(y => y.Ordem).Select(e => e.Etapa) :
                processoResposta.ListaEtapaResposta.Select(x => x.Etapa).OrderBy(x => x.Ordem);

            return etapas.Where(e => e.ListaEtapaResposta.Any(er => er.ProcessoResposta.ID == processoResposta.ID)).ToList();
        }

        public IEnumerable<Etapa> ObterEtapasNaoIniciadasPorVersao(ProcessoResposta processoResposta, EtapaResposta etapaAtual)
        {
            var versoes = manterEtapaVersao.ObterTodosIQueryable().Where(x => processoResposta.VersaoEtapa != null && x.Etapa.Processo.ID == processoResposta.Processo.ID && x.Versao == processoResposta.VersaoEtapa).ToList();

            var etapas = versoes != null && versoes.Any() ? versoes.OrderBy(x => x.Ordem).Select(e => e.Etapa) :
                processoResposta.Processo.ListaEtapas.Select(x => x).OrderBy(x => x.Ordem);

            if (versoes.Any())
            {
                var etapaAtualNaOrdem = versoes.FirstOrDefault(x => x.Etapa.ID == etapaAtual.Etapa.ID);
                return versoes.Where(x => x.Ordem > etapaAtualNaOrdem.Ordem).OrderBy(x => x.Ordem).Select(x => x.Etapa);
            }
            else
            {
                return etapas.Where(x => x.Ordem > etapaAtual.Etapa.Ordem).OrderBy(x => x.Ordem).ToList();
            }
        }
        
    }
}
