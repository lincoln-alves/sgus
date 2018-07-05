using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Extensions.Others;
using System;
using System.Collections.Generic;
using System.Linq;
using Nancy.Extensions;

namespace Sebrae.Academico.BP.DTO.Services.Processo
{
    public class DTOVisualizarProcessos
    {
        public DTOFiltrosAcompanharMeusProcessos Filtros { get; set; }

        public List<DTOProcessoRespostaAcompanhar> Processos { get; set; }

        public int TotalPaginas { get; set; }

        public void ConfigurarFiltros()
        {
            Filtros = new DTOFiltrosAcompanharMeusProcessos
            {
                Demandantes = Processos.DistinctBy(x => x.IdDemandante)
                    .Select(x => new DTOValorFiltro(x.IdDemandante, x.Demandante)).OrderBy(x => x.Valor).ToList(),

                Processos = Processos.DistinctBy(x => x.IdProcesso)
                    .Select(x => new DTOValorFiltro(x.IdProcesso, x.NomeProcesso)).OrderBy(x => x.Valor).ToList(),

                Etapas = Processos.DistinctBy(x => x.IdEtapa).Where(x => x.IdEtapa != null)
                    .Select(x => new DTOValorFiltro(x.IdEtapa.Value, x.NomeEtapa)).OrderBy(x => x.Valor).ToList()
            };
        }

        public void Paginar(int paginaAtual, int itensPorPagina)
        {
            TotalPaginas = (int) Math.Ceiling(Processos.Count / (decimal)itensPorPagina);

            if (paginaAtual > 1)
            {
                Processos = Processos.Skip(itensPorPagina * (paginaAtual - 1)).ToList();
            }

            Processos = Processos.Take(itensPorPagina).ToList();
        }
    }

    public class DTOFiltrosAcompanharMeusProcessos
    {
        public List<DTOValorFiltro> Demandantes { get; set; }
        public List<DTOValorFiltro> Processos { get; set; }
        public List<DTOValorFiltro> Etapas { get; set; }
    }

    public class DTOValorFiltro
    {
        protected DTOValorFiltro()
        {
            
        }

        public DTOValorFiltro(int id, string valor)
        {
            Id = id;
            Valor = valor;
        }

        public int Id { get; set; }
        public string Valor { get; set; }
    }

    public class DTOProcessoRespostaAcompanhar
    {
        public int IdProcessoResposta { get; set; }
        public int IdProcesso { get; set; }
        public string NomeProcesso { get; set; }
        public DateTime DataAberturaDemanda { get; set; }
        public int? IdEtapa { get; set; }
        public string NomeEtapa { get; set; }
        public int IdDemandante { get; set; }
        public string Demandante { get; set; }
        public string Unidade { get; set; }
        public DateTime? DataUltimoEnvio { get; set; }
        public DateTime? DataAlteracaoProcessoResposta { get; set; }
        public DateTime? PrazoEncaminhamento { get; set; }
        public string DataInicioCapacitacao { get; set; }
        public int? StatusEncaminhamento { get; set; }
        public int StatusProcesso { get; set; }

        public string StatusEncaminhamentoFormatado
        {
            get
            {
                return StatusEncaminhamento != null
                    ? ((enumPrazoEncaminhamentoDemanda) StatusEncaminhamento.Value).GetDescription()
                    : "";
            }
        }
    }
}