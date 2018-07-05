using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.BP
{
    public class ManterCapacitacao : BusinessProcessBase
    {

        private BMCapacitacao bmCapacitacao = null;

        public ManterCapacitacao() : base()
        {
            bmCapacitacao = new BMCapacitacao();
        }

        public void AtualizarNodeIdDrupal(Capacitacao capacitacao, BMConfiguracaoSistema bmConfiguracaoSistema = null, BMLogSincronia bmLogSincronia = null, Usuario usuarioLogado = null)
        {
            var id = SalvaNodeDrupalRest(capacitacao, bmConfiguracaoSistema, bmLogSincronia, usuarioLogado);

            if (!id.HasValue)
                return;

            capacitacao.IdNodePortal = id.Value;

            bmCapacitacao.Salvar(capacitacao);
        }

        public IList<DTORelatorioHorasCapacitacao> ConsultarRelatorioCapacitacao(int? idPerfil, int? idUf,
            int? idNivelOcupacional, int? idFormaAquisicao, int? idSolucaoEducacional, int? idStatusMatricula,
            DateTime? dataInicial, DateTime? dataFinal, IEnumerable<int> pUfResponsavel)
        {
            var parametros = new Dictionary<string, object>
            {
                {
                    "p_Perfil", idPerfil
                },
                {
                    "p_UF", idUf
                },
                {
                    "p_NivelOcupacional", idNivelOcupacional
                },
                {
                    "p_FormaAquisicao", idFormaAquisicao
                },
                {
                    "p_SolucaoEducacional", idSolucaoEducacional
                },
                {
                    "p_StatusMatricula", idStatusMatricula
                },
                {
                    "p_DataInicial", dataInicial
                },
                {
                    "p_DataFinal", dataFinal
                }
            };

            if (pUfResponsavel != null && pUfResponsavel.Any())
                parametros.Add("P_UFResponsavel", string.Join(", ", pUfResponsavel));
            else
                parametros.Add("P_UFResponsavel", DBNull.Value);

            return bmCapacitacao.ExecutarProcedure<DTORelatorioHorasCapacitacao>("SP_REL_HORAS_CAPACITACAO", parametros);
        }

        public void IncluirCapacitacao(Capacitacao pCapacitacao)
        {
            try
            {
                //this.PreencherInformacoesDeAuditoria(pCapacitacao);
                //bmCapacitacao.Salvar(pCapacitacao);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public void ExcluirCapacitacao(int IdCapacitacao)
        {
            if (IdCapacitacao == 0) return;
            try
            {
                var capacitacao = bmCapacitacao.ObterPorId(IdCapacitacao);
                if (capacitacao == null) return;
                if (capacitacao.IdNodePortal.HasValue)
                {
                    DrupalUtil.RemoverNodeDrupalRest(capacitacao.IdNodePortal.Value);
                }
                bmCapacitacao.Excluir(capacitacao);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public IQueryable<Capacitacao> ObterTodasCapacitacoes()
        {
            return bmCapacitacao.ObterTodos();
        }

        public IList<Capacitacao> ObterPorFiltro(Capacitacao filtro)
        {
            return bmCapacitacao.ObterPorFiltro(filtro);
        }

        public void AlterarCapacitacao(Capacitacao pCapacitacao)
        {
            try
            {
                this.PreencherInformacoesDeAuditoria(pCapacitacao);
                bmCapacitacao.Salvar(pCapacitacao);

                AtualizarNodeIdDrupal(pCapacitacao);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void PreencherInformacoesDeAuditoria(Capacitacao pCapacitacao)
        {
            //base.PreencherInformacoesDeAuditoria(pCapacitacao);
            //pCapacitacao.ListaModulos.ToList().ForEach(x => this.PreencherInformacoesDeAuditoria(x));
        }

        public Capacitacao ObterPorID(int idCapacitacao)
        {
            return bmCapacitacao.ObterPorId(idCapacitacao);
        }

        public IList<Capacitacao> ObterPorPrograma(Programa programa)
        {
            IList<Capacitacao> ListaCapacitacao = null;

            try
            {
                if ((programa == null) || (programa != null && programa.ID < 0))
                    throw new AcademicoException("Informe o programa.");

                ListaCapacitacao = bmCapacitacao.ObterPorPrograma(programa);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }

            return ListaCapacitacao;
        }

        public int? SalvaNodeDrupalRest(Capacitacao capacitacao, BMConfiguracaoSistema bmConfiguracaoSistema = null, BMLogSincronia bmLogSincronia = null, Usuario usuarioLogado = null)
        {
            var postParameters = DrupalUtil.InitPostParameters(capacitacao.ID, capacitacao.Nome, null, "oferta");

            postParameters.Add("data[field_data_inicio_inscricao]",
                capacitacao.DataInicioInscricao.HasValue
                    ? DataUtil.ToUnix(capacitacao.DataInicioInscricao.Value).ToString()
                    : "");
            postParameters.Add("data[field_data_fim_inscricao]",
                capacitacao.DataFimInscricao.HasValue ? DataUtil.ToUnix(capacitacao.DataFimInscricao.Value).ToString() : "");
            postParameters.Add("data[field_carga_horaria]", "0");
            postParameters.Add("data[field_solucao_sgus_id]",
                (capacitacao.Programa != null ? capacitacao.Programa.ID : 0).ToString());
            postParameters.Add("data[field_tipo_de_solucao]", "5");

            if (capacitacao.Programa != null)
                DrupalUtil.PermissoesUf(
                    capacitacao.Programa.ListaPermissao.Where(p => p.Uf != null).Select(x => x.Uf.ID).ToList(),
                    ref postParameters);
            if (capacitacao.Programa != null)
                DrupalUtil.PermissoesPerfil(
                    capacitacao.Programa.ListaPermissao.Where(p => p.Perfil != null).Select(x => x.Perfil.ID).ToList(),
                    ref postParameters);
            if (capacitacao.Programa != null)
                DrupalUtil.PermissoesNivelOcupacional(
                    capacitacao.Programa.ListaPermissao.Where(p => p.NivelOcupacional != null)
                        .Select(x => x.NivelOcupacional.ID)
                        .ToList(), ref postParameters);

            try
            {
                return DrupalUtil.SalvaNodeDrupalRest(postParameters, true, bmConfiguracaoSistema, bmLogSincronia, usuarioLogado);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IQueryable<Capacitacao> ObterPorIdPrograma(int idPrograma)
        {
            return ObterPorIdPrograma(idPrograma, false);
        }

        public IQueryable<Capacitacao> ObterPorIdPrograma(int idPrograma, bool somenteComInscricoesAbertas)
        {
            return bmCapacitacao.ObterPorIdPrograma(idPrograma, somenteComInscricoesAbertas);
        }
    }
}