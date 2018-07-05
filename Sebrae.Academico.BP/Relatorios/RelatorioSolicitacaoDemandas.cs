using Sebrae.Academico.BP.DTO.Relatorios;
using System.Collections.Generic;
using System.Linq;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioSolicitacaoDemandas
    {
        public IEnumerable<DTORelatorioSolicitacaoDemanda> ObterSolicitacoes()
        {
            var processoResposta = new ManterProcessoResposta().ObterTodosIQueryable();
            var etapaResposta = new ManterEtapaResposta().ObterTodosIQueryable();
            var campoResposta = new ManterCampoResposta().ObterTodosIQueryable();
            var usuarios = new ManterUsuario().ObterTodosIQueryable();

            var result = (from pr in processoResposta
                          join er in etapaResposta on pr.ID equals er.ProcessoResposta.ID
                          join cr in campoResposta on er.ID equals cr.EtapaResposta.ID
                          join u in usuarios on pr.Usuario.ID equals u.ID
                          where pr.Processo != null && er.Ativo
                          select new DTORelatorioSolicitacaoDemanda
                          {
                              NumeroProcesso = pr.ID,
                              IdProcesso = pr.Processo.ID,
                              IdEtapa = er.Etapa.ID,
                              IdUsuario = u.ID,
                              UsuarioDemandante = u.Nome,
                              Unidade = u.Unidade,
                              Demanda = pr.Processo.Nome,
                              DataAbertura = pr.DataSolicitacao,
                              Status = pr.Status,
                              EtapaAtual = etapaResposta.Where(e => e.ProcessoResposta.ID == pr.ID && e.Ativo).OrderByDescending(e => e.ID).Select(e => e.Etapa.Nome).FirstOrDefault(),
                              EtapaResposta = er,
                              IdEtapaResposta = er.ID
                          }).Distinct();

            return result.ToList();
        }
    }
}
