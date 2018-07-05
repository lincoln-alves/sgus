using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioSEOfertaMatricula: BusinessProcessBaseRelatorio, IDisposable
    {
        protected override enumRelatorio Relatorio
        {
            get { return enumRelatorio.ListagemSEComPeriodoOfertaTipoDaOfertaEQuantidadesDeVagasMatriculasEReserva; }
        }

        public IList<FormaAquisicao> ObterFormaAquisicaoTodos()
        {
            using (var formReq = new BMFormaAquisicao())
            {
                return formReq.ObterTodos().OrderBy(x => x.Descricao).ToList();
            }
        }

        public IQueryable<SolucaoEducacional> ObterSolucaoEducacional(int pIdFormaAquisicao = 0)
        {
            return ObterSolucaoEducacionalPorFormaAquisicao(null, pIdFormaAquisicao);
        }

        public IQueryable<SolucaoEducacional> ObterSolucaoEducacionalPorFormaAquisicao(Uf uf = null, int pIdFormaAquisicao = 0)
        {
            using (var solEducBm = new BMSolucaoEducacional())
            {
                var retorno = solEducBm.ObterTodos();

                if (pIdFormaAquisicao != 0)
                    retorno = retorno.Where(s => s.FormaAquisicao.ID == pIdFormaAquisicao);

                if (uf != null)
                    solEducBm.FiltrarPermissaoVisualizacao(ref retorno, uf.ID);

                return retorno;
            }
        }

        public IList<TipoOferta> ObterTipoOfertaTodos()
        {
            using (var toBm = new BMTipoOferta())
            {
                return toBm.ObterTodos().OrderBy(x => x.Nome).ToList();
            }
        }

        public IList<DTORelatorioSEOfertaMatricula> ConsultarSeOfertaMatricula(int? pIdFormaAquisicao,
            int? pIdSolucaoEducacional, int? pIdTipoOferta, DateTime? dtIni, DateTime? dtFim, List<int> pIdUfResponsavel)
        {
            RegistrarLogExecucao();

            return (new ManterSolucaoEducacional()).ConsultarSeOfertaMatricula(pIdFormaAquisicao, pIdSolucaoEducacional,
                pIdTipoOferta, dtIni, dtFim, pIdUfResponsavel);
        }

        public IList<Uf> ObterUFTodos()
        {
            using (var ufBM = new BMUf())
            {
                return ufBM.ObterTodos().OrderBy(x => x.Nome).ToList();
            }
        }


        public void Dispose()
        {
            GC.Collect();
        }
    }
}
