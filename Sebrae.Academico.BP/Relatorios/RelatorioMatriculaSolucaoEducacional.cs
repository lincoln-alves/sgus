using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioMatriculaSolucaoEducacional : BusinessProcessBaseRelatorio, IDisposable
    {
        protected override enumRelatorio Relatorio
        {
            get { return enumRelatorio.MatriculaSolucaoEducacional; }
        }

        public void Dispose()
        {
            GC.Collect();
        }

        public IList<FormaAquisicao> ObterFormaAquisicaoTodos()
        {
            using (var formaAquisicaoBm = new BMFormaAquisicao())
            {
                return formaAquisicaoBm.ObterTodos();
            }
        }

        public IQueryable<SolucaoEducacional> ObterSolucaoEducacionalTodos()
        {
            using (var solEducBm = new BMSolucaoEducacional())
            {
                return solEducBm.ObterTodos();
            }
        }

        public IList<enumStatusMatricula> ObterStatusMatriculaTodos()
        {
            return (IList<enumStatusMatricula>) Enum.GetValues(typeof (enumStatusMatricula));
        }

        public IList<DTORelatorioMatriculaSolucaoEducacional> ConsultarMatriculaSolucaoEducacional(int? pFormaAquisicao,
            int? pSolucaoEducacional, int? pSituacaoMatricula, List<int> pIdUf, DateTime? pDataInicial, DateTime? pDataFinal,
            string pCategorias, List<int> pIdUfResponsavel)
        {
            RegistrarLogExecucao();

            return (new ManterSolucaoEducacional()).ConsultarMatriculaSolucaoEducacional(pFormaAquisicao,
                pSolucaoEducacional, pSituacaoMatricula, pIdUf, pDataInicial, pDataFinal, pCategorias, pIdUfResponsavel);

        }

        public IQueryable<SolucaoEducacional> ObterSolucaoEducacionalPorFormaAquisicao(int idFormaAquisicao = 0)
        {
            using (var solEduBm = new BMSolucaoEducacional())
            {
                return solEduBm.ObterTodos()
                    .Where(s => idFormaAquisicao == 0 || s.FormaAquisicao.ID == idFormaAquisicao);
            }
        }

        public IQueryable<SolucaoEducacional> ObterSolucaoEducacionalPorFormaAquisicao(Uf uf, int idFormaAquisicao = 0)
        {
            using (var solEduBm = new BMSolucaoEducacional())
            {
                var retorno = solEduBm.ObterTodos();

                if (idFormaAquisicao > 0)
                    retorno = retorno.Where(s => s.FormaAquisicao.ID == idFormaAquisicao);

                if (uf != null)
                    solEduBm.FiltrarPermissaoVisualizacao(ref retorno, uf.ID);

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
