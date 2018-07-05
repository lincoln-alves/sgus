using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.BP.DTO.Relatorios.DashBoard;
using Sebrae.Academico.BP.DTO.Services.Processo;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Extensions.Others;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sebrae.Academico.BP
{
    public class ManterEtapaResposta : RepositorioBase<EtapaResposta>
    {
        public List<EtapaResposta> ObterEtapaRespostasInativas(int IdProcessoResposta, int Ordem)
        {
            var query = ObterTodosIQueryable();
            return query.Where(x => x.ProcessoResposta.ID == IdProcessoResposta && x.Etapa.Ordem == Ordem && !x.Ativo).ToList();
        }

        public List<EtapaResposta> ObterPorProcessoRespostaId(int pId)
        {
            var query = ObterTodosIQueryable();
            return query.Where(d => d.ProcessoResposta.ID == pId && d.Ativo).ToList();
        }

        public IQueryable<EtapaResposta> ObterPorProcessoRespostaIdGeral (int processoId)
        {
            return ObterTodosIQueryable().Where(d => d.ProcessoResposta.ID == processoId);
        }

        public List<EtapaResposta> ObterEtapasEntreRangeDeOrdem(int ordemIn, int ordemOut, int idProcessoResposta)
        {
            var query = ObterTodosIQueryable();
            return query.Where(x => x.ProcessoResposta.ID == idProcessoResposta && x.Etapa.Ordem >= ordemIn && x.Etapa.Ordem <= ordemOut).ToList();
        }

        public List<EtapaResposta> ObterTodasDisponiveisImpressao(int idProcessoResposta)
        {
            var query = ObterTodosIQueryable();
            return query.Where(e => e.ProcessoResposta.ID == idProcessoResposta).OrderBy(x => x.Etapa.Ordem).ToList();
        }

        public EtapaResposta ObterUltimaEtapaRespostaPorProcessoResposta(int idProcessoResposta)
        {
            var listaEtapaResposta = ObterTodos().Where(x => x.ProcessoResposta.ID == idProcessoResposta).OrderByDescending(y => y.ID).FirstOrDefault();
            return listaEtapaResposta;
        }

        public IEnumerable<DTODemandaNucleo> ObterDTOEtapasPendentesPorNucleo()
        {
            var listaEtapas = ObterTodosIQueryable().Where(x => x.Ativo);

            var demandasNucleo = new List<DTODemandaNucleo>();

            var nucleos = new ManterHierarquiaNucleo().ObterTodos();

            foreach (var item in nucleos)
            {
                var demanda = new DTODemandaNucleo();

                demanda.IdNucleo = item.ID;
                demanda.Nucleo = item.Nome;

                var result = listaEtapas.Where(x => x.PermissoesNucleoEtapaResposta.Any(y => y.EtapaPermissaoNucleo.HierarquiaNucleoUsuario.HierarquiaNucleo.ID == demanda.IdNucleo)).ToList();

                demanda.NoPrazo = result.Count(x => x.Status == (int)enumStatusEtapaResposta.Aguardando
                        && (x.DataPreenchimento != null || x.DataAlteracao != null || x.PrazoEncaminhamento != null)
                        && x.ObterStatusEncaminhamento(x.PrazoEncaminhamento.HasValue ? x.PrazoEncaminhamento.Value :
                        x.Etapa.ObterPrazoParaEncaminhamentoDaDemanda(
                         x.DataPreenchimento.HasValue ? x.DataPreenchimento.Value : x.DataAlteracao.Value)) == enumPrazoEncaminhamentoDemanda.NoPrazo);

                demanda.Vencidas = result.Count(x => x.Status == (int)enumStatusEtapaResposta.Aguardando
                        && (x.DataPreenchimento != null || x.DataAlteracao != null || x.PrazoEncaminhamento != null)
                        && x.ObterStatusEncaminhamento(x.PrazoEncaminhamento.HasValue ? x.PrazoEncaminhamento.Value :
                        x.Etapa.ObterPrazoParaEncaminhamentoDaDemanda(
                        x.DataPreenchimento.HasValue ? x.DataPreenchimento.Value : x.DataAlteracao.Value)) == enumPrazoEncaminhamentoDemanda.ForaDoPrazo);

                demanda.AExpirar = result.Count(x => x.Status == (int)enumStatusEtapaResposta.Aguardando 
                        && (x.DataPreenchimento != null || x.DataAlteracao != null || x.PrazoEncaminhamento != null) 
                        && x.ObterStatusEncaminhamento(x.PrazoEncaminhamento.HasValue ? x.PrazoEncaminhamento.Value :
                        x.Etapa.ObterPrazoParaEncaminhamentoDaDemanda(
                         x.DataPreenchimento.HasValue ? x.DataPreenchimento.Value : x.DataAlteracao.Value)) == enumPrazoEncaminhamentoDemanda.AExpirar);

                demanda.Encerradas = result.Count(x => x.Status != (int)enumStatusEtapaResposta.Aguardando);

                demandasNucleo.Add(demanda);

            }

            return demandasNucleo;
        }

        public IEnumerable<EtapaResposta> ObterPorPermissaoDoNucleo(int idNucleo)
        {
            var query = ObterTodosIQueryable();
            var result = query.SelectMany(x => x.PermissoesNucleoEtapaResposta.Where(y => y.EtapaPermissaoNucleo.HierarquiaNucleoUsuario.HierarquiaNucleo.ID == idNucleo));
            return result.Select(x => x.EtapaResposta);
        }

        public IEnumerable<DTOEtapaNucleo> ObterDTOPorPermissaoDoNucleoPorFiltro(int idNucleo, enumPrazoEncaminhamentoDemanda status, int limit, int page)
        {
            //var query = ObterTodos();
            var query = ObterTodosIQueryable();

            var lista = query.Where(x => x.Ativo 
            && x.PermissoesNucleoEtapaResposta.Any(y => y.EtapaPermissaoNucleo.HierarquiaNucleoUsuario.HierarquiaNucleo.ID == idNucleo)).ToList();

            IEnumerable<EtapaResposta> result;

            if (status != enumPrazoEncaminhamentoDemanda.Encerrada)
            {
                result = lista.Where(x => x.Status == (int)enumStatusEtapaResposta.Aguardando
                            && (x.PrazoEncaminhamento.HasValue || x.DataPreenchimento.HasValue || x.DataAlteracao.HasValue)
                            && x.ObterStatusEncaminhamento(x.PrazoEncaminhamento.HasValue ? x.PrazoEncaminhamento.Value :
                            x.Etapa.ObterPrazoParaEncaminhamentoDaDemanda((x.DataPreenchimento.HasValue ? x.DataPreenchimento.Value : x.DataAlteracao.Value))) == status)
                       .ToList();
            }
            else
            {
                result = lista.Where(x => x.Status != (int)enumStatusEtapaResposta.Aguardando).ToList();
            }

            if (limit > 0)
                result = result.Skip(page * limit).Take(limit);

            var etapas = result;

            return etapas.Select(x => new DTOEtapaNucleo
            {
                IdEtapaResposta = x.ID,
                IdProcessoResposta = x.ProcessoResposta.ID,
                NomeEtapa = x.Etapa.Nome,
                NomeProcessoResposta = x.ProcessoResposta.Processo.Nome,
                Prazo = x.PrazoEncaminhamento != null ? x.PrazoEncaminhamento.Value : (DateTime?)null,
                Responsaveis = x.ObterAnalistas(new List<UsuarioCargo>(), new Usuario()).Select(u => u.Nome).ToList(),
                Etapas = ObterDTOEtapaNucleoOdernadoPorIdEtapaResposta(x.ProcessoResposta),
                Demandante = x.ProcessoResposta.Usuario.Nome
            });
        }

        public IEnumerable<DTOEtapaNucleo> ObterDTOEtapaNucleoOdernadoPorIdEtapaResposta(ProcessoResposta processo)
        {

            return processo.ListaEtapaResposta
                .OrderBy(x => x.ID)
                .Select(x => new DTOEtapaNucleo
                {
                    IdEtapaResposta = x.ID,
                    NomeEtapa = x.Etapa.Nome,
                    DataPreenchimento = x.DataPreenchimento,
                    Prazo = x.PrazoEncaminhamento != null ? x.PrazoEncaminhamento.Value : (DateTime?)null,
                    OrdemEtapa = x.Etapa.Ordem
                });
        }

        public int ObterTotalPermissaoDoNucleoPorFiltro(int idNucleo, enumPrazoEncaminhamentoDemanda status)
        {
            var query = ObterTodosIQueryable();

            // Enxugando query com select
            query = query.Select(x =>
            new EtapaResposta
            {
                Ativo = x.Ativo,
                Status = x.Status,
                PrazoEncaminhamento = x.PrazoEncaminhamento,
                DataPreenchimento = x.DataPreenchimento,
                DataAlteracao = x.DataAlteracao,
                Etapa = x.Etapa,
                PermissoesNucleoEtapaResposta = x.PermissoesNucleoEtapaResposta
            });

            var result = query.Where(x => x.Ativo 
            && x.PermissoesNucleoEtapaResposta.Any(y => y.EtapaPermissaoNucleo.HierarquiaNucleoUsuario.HierarquiaNucleo.ID == idNucleo)).ToList();

            if (status != enumPrazoEncaminhamentoDemanda.Encerrada)
            {
                result = result.Where(x => x.Status == (int)enumStatusEtapaResposta.Aguardando &&
                    (x.PrazoEncaminhamento.HasValue || x.DataPreenchimento.HasValue || x.DataAlteracao.HasValue)
                    && x.ObterStatusEncaminhamento(x.PrazoEncaminhamento.HasValue ? x.PrazoEncaminhamento.Value :
                        x.Etapa.ObterPrazoParaEncaminhamentoDaDemanda(x.DataPreenchimento.HasValue ? x.DataPreenchimento.Value : x.DataAlteracao.Value)) == status).ToList();
            }
            else
            {
                result = result.Where(x => x.Status != (int)enumStatusEtapaResposta.Aguardando).ToList();
            }

            return result.Count();
        }


        public List<EtapaResposta> ObterEtapasRespostaAnalistas(List<HierarquiaNucleoUsuario> usuarios) {            

            List<int> ids = usuarios.Select(x => x.ID).ToList();

            var query = ObterTodosIQueryable();

            var result = query.Where(x => x.Ativo && x.Status == (int)enumStatusEtapaResposta.Aguardando && x.PermissoesNucleoEtapaResposta.Any()).ToList();
            result = result.Where(x => x.PermissoesNucleoEtapaResposta.Any(y => ids.Contains(y.EtapaPermissaoNucleo.HierarquiaNucleoUsuario.ID))).ToList();

            return result;

        }
    }
}
