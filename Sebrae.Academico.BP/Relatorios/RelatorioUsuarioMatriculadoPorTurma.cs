using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioUsuarioMatriculadoPorTurma : BusinessProcessBaseRelatorio, IDisposable
    {
        protected override enumRelatorio Relatorio
        {
            get { return enumRelatorio.ListagemUsuariosMatriculadosPorTurma; }
        }

        public void Dispose()
        {
            GC.Collect();
        }

        public IList<FormaAquisicao> ObterFormaAquisicaoTodos()
        {
            using (BMFormaAquisicao bmFormaAquisicao = new BMFormaAquisicao())
            {
                return bmFormaAquisicao.ObterTodos();
            }
        }

        public IQueryable<Oferta> ObterOfertaPorSolucaoEducacional(int idSolucaoEducacional)
        {
            using (var bm = new BMOferta())
            {
                return bm.ObterTodos().Where(o => o.SolucaoEducacional.ID == idSolucaoEducacional);
            }
        }

        public IQueryable<SolucaoEducacional> ObterSolucaoEducacionalPorFormaAquisicao(int formaAquisicaoId = 0)
        {
            using (var seBm = new BMSolucaoEducacional())
            {
                var retorno = seBm.ObterTodos();

                if(formaAquisicaoId > 0)
                    retorno = retorno.Where(s => s.FormaAquisicao.ID == formaAquisicaoId);

                return retorno;
            }
        }

        public IQueryable<SolucaoEducacional> ObterSolucaoEducacionalPorFormaAquisicao(int formaAquisicaoId, Uf uf)
        {
            using (var seBm = new BMSolucaoEducacional())
            {
                var retorno = seBm.ObterTodos();

                if (formaAquisicaoId != 0)
                    retorno = retorno.Where(s => s.FormaAquisicao.ID == formaAquisicaoId);

                if (uf != null)
                    seBm.FiltrarPermissaoVisualizacao(ref retorno, uf.ID);

                return retorno;
            }
        }

        public IQueryable<Turma> ObterTurmaPorOferta(int pOferta)
        {
            using (var turmaBm = new BMTurma())
            {
                return turmaBm.ObterPorFiltro(null, null, pOferta, 0);
            }
        }

        public IList<DTORelatorioUsuarioMatriculadoPorTurma> ConsultarUsuarioMatriculadoPorTurma(int? pIdFormaAquisicao, int? pIdSolucaoEducacional, int? pIdOferta, int? pIdTurma, int? pIdUf)
        {
            RegistrarLogExecucao();

            return (new ManterTurma()).ConsultarUsuarioMatriculadoPorTurma(pIdFormaAquisicao, pIdSolucaoEducacional,
                pIdOferta, pIdTurma, pIdUf);
        }
    }
}
