using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BP
{
    public class ManterFormaAquisicao : RepositorioBase<FormaAquisicao>
    {
        #region "Atributos Privados"

        private BMFormaAquisicao bmFormaAquisicao = null;

        #endregion

        #region "Construtor"

        public ManterFormaAquisicao()
            : base()
        {
            bmFormaAquisicao = new BMFormaAquisicao();
        }

        #endregion

        #region "Métodos Públicos"

        public IList<DTORelatorioUsuarioMatriculadoSEFormaAquisicao>
            ConsultarRelatorioUsuarioMatriculadoSeFormaAquisicao(List<int> idsFormaAquisicao, int? pStatusMatricula,
                DateTime? pDataInicioTurma, DateTime? pDataFimTurma, int? idUf = null, IEnumerable<int> pUfResponsavel = null)
        {
            var ids = idsFormaAquisicao.Any() ? string.Join(",", idsFormaAquisicao) : null;

            var lstParam = new Dictionary<string, object>
            {
                {
                    "P_FORMA_AQUISICAO", ids
                },
                {
                    "P_STATUS_MATRICULA", pStatusMatricula
                },
                {
                    "P_DATA_FINAL_TURMA", pDataFimTurma
                },
                {
                    "P_DATA_INICIO_TURMA", pDataInicioTurma
                },
                {
                    "P_ID_UF", idUf
                }
            };

            if (pUfResponsavel != null && pUfResponsavel.Any())
                lstParam.Add("P_UFResponsavel", string.Join(", ", pUfResponsavel));
            else
                lstParam.Add("P_UFResponsavel", DBNull.Value);

            return
                bmFormaAquisicao.ExecutarProcedure<DTORelatorioUsuarioMatriculadoSEFormaAquisicao>(
                    "SP_REL_USUARIO_MATRICULADO_SE_FORMA_AQUISICAO", lstParam);
        }

        public IList<DTOConcluinte> ObterRelatorioConcluinte(int? pFormaAquisicao, int? pUf, IEnumerable<int> pUfResponsavel) {
            var lstParam = new Dictionary<string, object>
            {
                {
                    "pFormaAquisicao", pFormaAquisicao
                },
                {
                    "pUF", pUf
                }
            };

            if (pUfResponsavel != null && pUfResponsavel.Any())
                lstParam.Add("P_UFResponsavel", string.Join(", ", pUfResponsavel));
            else
                lstParam.Add("P_UFResponsavel", DBNull.Value);

            return bmFormaAquisicao.ExecutarProcedure<DTOConcluinte>("SP_REL_CONCLUINTES", lstParam);
        }

        public void IncluirFormaAquisicao(FormaAquisicao pFormaAquisicao)
        {
            PreencherInformacoesDeAuditoria(pFormaAquisicao, new ManterUsuario().ObterUsuarioLogado().CPF);
            bmFormaAquisicao.Salvar(pFormaAquisicao);
        }

        public void AlterarFormaAquisicao(FormaAquisicao pFormaAquisicao)
        {
            PreencherInformacoesDeAuditoria(pFormaAquisicao, new ManterUsuario().ObterUsuarioLogado().CPF);
            bmFormaAquisicao.Salvar(pFormaAquisicao);
        }

        public FormaAquisicao ObterFormaAquisicaoPorID(int pId)
        {
            return bmFormaAquisicao.ObterPorID(pId);
        }
        
        public IList<FormaAquisicao> ObterTodasFormaAquisicao()
        {
            return bmFormaAquisicao.ObterTodos();
        }

        public void ExcluirFormaAquisicao(int IdFormaAquisicao)
        {
            
            try
            {
                FormaAquisicao formaAquisicao = null;

                if (IdFormaAquisicao > 0)
                {
                    formaAquisicao = bmFormaAquisicao.ObterPorID(IdFormaAquisicao);
                }

                bmFormaAquisicao.Excluir(formaAquisicao);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public IList<FormaAquisicao> ObterFormaAquisicoPorFiltro(FormaAquisicao pFormaAquisicao)
        {
            return bmFormaAquisicao.ObterPorFiltro(pFormaAquisicao);
        }

        #endregion
    }
}
