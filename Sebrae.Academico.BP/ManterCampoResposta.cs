using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP
{
    public class ManterCampoResposta : BusinessProcessBase
    {
        private readonly BMCampoResposta _bmCampo;

        public ManterCampoResposta()
        {
            _bmCampo = new BMCampoResposta();
        }
        
        public IEnumerable<CampoResposta> ObterRespostas(Campo campo)
        {
            return _bmCampo.ObterRespostas(campo);
        }

        public IList<CampoResposta> ObterRespostasEtapaResposta(int idEtapaResposta)
        {
            return _bmCampo.ObterTodosIQueryable().Where(x => x.EtapaResposta.ID == idEtapaResposta).ToList();
        }

        public CampoResposta ObterRespostaDataCapacitacao(int idProcessoResposta)
        {
            var query = _bmCampo.ObterTodosIQueryable();
            return query.Where(x => x.EtapaResposta.ProcessoResposta.ID == idProcessoResposta &&
                x.Campo.Nome == "Data de Início da Capacitação" &&
                x.Campo.TipoDado == (int)enumTipoDado.Data &&
                x.Campo.TipoCampo == (int)enumTipoCampo.Text).FirstOrDefault();
        }

        public IQueryable<CampoResposta> ObterTodosIQueryable()
        {
            return _bmCampo.ObterTodosIQueryable();
        }
    }
}
