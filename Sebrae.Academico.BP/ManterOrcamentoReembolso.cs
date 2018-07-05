using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sebrae.Academico.BP
{
    public class ManterOrcamentoReembolso : BusinessProcessBase
    {
        private BMOrcamentoReembolso bm = null;

        public ManterOrcamentoReembolso()
            : base()
        {
            bm = new BMOrcamentoReembolso();
        }

        public List<DTOOrcamentoReembolso> ObterOrcamento()
        {
            var orcamentos =
                bm.ObterTodosIQueryable().Select(x => new
                {
                    x.ID,
                    x.Ano,
                    x.Orcamento
                })
                .ToList();

            var idsOrcamentos = orcamentos.Select(x => x.ID).ToList();


            decimal r;

            var campos = new ManterCampo().ObterTodosIQueryable()
                .Where(x => x.OrcamentoReembolso != null)
                .Join(new ManterCampoResposta().ObterTodosIQueryable(), campo => campo.ID, resp => resp.Campo.ID, (campo, resp) => new
                {
                    campo.OrcamentoReembolso.ID,
                    resp
                })
                .Join(new ManterEtapaResposta().ObterTodosIQueryable(), join => join.resp.EtapaResposta.ID, etapa => etapa.ID, (join, etapa) => new
                {
                    join.ID,
                    join.resp.Resposta,
                    etapa
                })
                .Join(new ManterProcessoResposta().ObterTodosIQueryable(), join => join.etapa.ProcessoResposta.ID, proc => proc.ID, (join, proc) => new
                {
                    join.ID,
                    join.Resposta,
                    join.etapa,
                    proc
                })
                .Where(x => x.etapa.Status == (int)enumStatusEtapaResposta.Aprovado || x.etapa.Status == (int)enumStatusEtapaResposta.Concluido || x.etapa.Status == (int)enumStatusEtapaResposta.Analisado)
                .Select(x => new
                {
                    x.ID,
                    x.Resposta,
                    IdProcesso = x.proc.ID
                })
                .ToList()
                .GroupBy(x => x.IdProcesso)
                .Select(x => new
                {
                    IdProcesso = x.Key,
                    x.FirstOrDefault().ID,
                    x.FirstOrDefault().Resposta
                })
                .Where(x => idsOrcamentos.Contains(x.ID))
                .ToList();

            return orcamentos.Select(x => new DTOOrcamentoReembolso
            {
                ID = x.ID,
                Ano = x.Ano,
                Total = x.Orcamento,
                Utilizado = campos.Where(c => c.ID == x.ID).Sum(c => decimal.TryParse(c.Resposta, out r) ? r : 0)
            })
            .ToList();
        }

        public IQueryable<OrcamentoReembolso> ObterTodos()
        {
            return bm.ObterTodosIQueryable();
        }

        public void Salvar(OrcamentoReembolso orcamento)
        {
            bm.Salvar(orcamento);
        }

        public void Remover(int idOrcamento)
        {
            Remover(ObterPorId(idOrcamento));
        }

        public void Remover(OrcamentoReembolso orcamento)
        {
            bm.Excluir(orcamento);
        }

        public OrcamentoReembolso ObterPorId(int cargoId)
        {
            return bm.ObterPorID(cargoId);
        }

        public bool AnoExiste(int ano)
        {
            return ObterTodos().Where(x => x.Ano == ano).Select(x => new { x.ID }).Any();
        }
    }
}
