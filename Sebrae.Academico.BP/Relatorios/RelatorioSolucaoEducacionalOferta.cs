using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioSolucaoEducacionalOferta : BusinessProcessBaseRelatorio, IDisposable
    {
        protected override enumRelatorio Relatorio
        {
            get { return enumRelatorio.SolucaoEducacionalOferta; }
        }

        public IList<DTOSolucaoEducacionalOferta> ConsultarSolucaoEducacionalOferta(int? pIdFormaAquisicao, int? pIdTipoOferta, int? pIdSolucaoEducacional, int? pIdUf, IEnumerable<int> pUfResponsavel)
        {
            RegistrarLogExecucao();

            return (new ManterSolucaoEducacional()).ConsultarSolucaoEducacionalOferta(pIdFormaAquisicao, pIdTipoOferta,
                pIdSolucaoEducacional, pIdUf, pUfResponsavel);
        }

        public void Dispose()
        {
            GC.Collect();
        }

        public IList<TipoOferta> ObterTipoOfertaTodos()
        {
            using (var tipoOfertaBm = new BMTipoOferta())
            {
                return tipoOfertaBm.ObterTodos();
            }
        }

        public IList<FormaAquisicao> ObterFormaAquisicaoTodos()
        {
            using (var formaAquisicaoBm = new BMFormaAquisicao())
            {
                return formaAquisicaoBm.ObterTodos();
            }
        }

        public IQueryable<SolucaoEducacional> ObterSolucaoEducacionalPorFormaAquisicao(Uf ufGestor = null, int pIdFormaAquisicao = 0)
        {
            using (var solEducBm = new BMSolucaoEducacional())
            {
                var retorno = solEducBm.ObterTodos();

                if (pIdFormaAquisicao != 0)
                    retorno = retorno.Where(s => s.FormaAquisicao.ID == pIdFormaAquisicao);

                if (ufGestor != null)
                    solEducBm.FiltrarPermissaoVisualizacao(ref retorno, ufGestor.ID);

                return retorno;
            }
        }

        public IList<Uf> ObterUFTodos()
        {
            using (var ufBM = new BMUf())
            {
                return ufBM.ObterTodos().OrderBy(x => x.Nome).ToList();
            }
        }
    }
}
