using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.BP.DTO.Relatorios.DashBoard;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.BP
{
    public class ManterTurma : BusinessProcessBase
    {
        private BMTurma bmTurma;

        /// <summary>
        /// Método Construtor da classe
        /// </summary>
        public ManterTurma()
            : base()
        {
            bmTurma = new BMTurma();
        }

        public IList<DTORelatorioUsuarioMatriculadoPorTurma> ConsultarUsuarioMatriculadoPorTurma(int? pIdFormaAquisicao, int? pIdSolucaoEducacional, int? pIdOferta, int? pIdTurma, int? pIdUf)
        {
            var lstParam = new Dictionary<string, object>
            {
                {
                    "P_FORMA_AQUISICAO", pIdFormaAquisicao
                },
                {
                    "P_SOLUCAO_EDUCACIONAL", pIdSolucaoEducacional
                },
                {
                    "P_OFERTA", pIdOferta
                },
                {
                    "P_TURMA", pIdTurma
                },
                {
                    "P_UF", pIdUf
                }
            };


            return bmTurma.ExecutarProcedure<DTORelatorioUsuarioMatriculadoPorTurma>("SP_REL_USUARIO_MATRICULADO_TURMA", lstParam);
        }

        public IList<DTOMonitoramentoTurma> ObterTotalStatus(string idCategoriaConteudo = null, DateTime? dataInicio = null, DateTime? dataFim = null, string status = null, IEnumerable<int> pUfResponsavel = null) {
            var lstParam = new Dictionary<string, object>
            {
                {
                    "IdCategoriaConteudo", idCategoriaConteudo
                },
                {
                    "IdStatusTurma", status
                },
                {
                    "dataInicio", dataInicio
                },
                {
                    "dataFim", dataFim
                }
            };

            if (pUfResponsavel != null && pUfResponsavel.Any())
                lstParam.Add("P_UFResponsavel", string.Join(", ", pUfResponsavel));
            else
                lstParam.Add("P_UFResponsavel", DBNull.Value);

            return bmTurma.ExecutarProcedure<DTOMonitoramentoTurma>("DASHBOARD_REL_MatriculasTurmasTotais", lstParam);
        }

        public List<DTOMonitoramentoTurma> ExecutarProcedureBase(string procedure, string idCategoriaConteudo = null, int? ano = null, IEnumerable<int> pUfResponsavel = null) {
            var listPrm = new Dictionary<string, object>{
                {"idCategoriaConteudo", idCategoriaConteudo}
            };

            if (pUfResponsavel != null && pUfResponsavel.Any())
                listPrm.Add("P_UFResponsavel", string.Join(", ", pUfResponsavel));
            else
                listPrm.Add("P_UFResponsavel", DBNull.Value);

            if (ano != 0){
                listPrm.Add("ano", ano);
            }

            return bmTurma.ExecutarProcedure<DTOMonitoramentoTurma>(procedure, listPrm).ToList();
        }

        public void IncluirTurma(Turma pTurma)
        {
            try
            {
                pTurma.NomeSalvo = pTurma.Oferta.SolucaoEducacional.CategoriaConteudo.Sigla + ".SE" +
                                   pTurma.Oferta.SolucaoEducacional.Sequencia + ".OF" + pTurma.Oferta.Sequencia + ".T" +
                                   (pTurma.Sequencia.HasValue ? pTurma.Sequencia.Value.ToString() : "1");

                PreencherInformacoesDeAuditoria(pTurma);

                bmTurma.Salvar(pTurma);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public void AlterarTurma(Turma turma, LogResponsavel logResponsavel = null, LogConsultorEducacional logConsultorEducacional = null, enumStatusTurma? statusAnterior = null)
        {
            PreencherInformacoesDeAuditoria(turma);

            bmTurma.Salvar(turma, logResponsavel, logConsultorEducacional);

            // Notifica os envolvidos e altera os status das matrículas na turma, caso necessário.
            if (turma.Oferta.SolucaoEducacional.CategoriaConteudo.PossuiGerenciamentoStatus() && statusAnterior != turma.Status)
            {
                VerificarStatus(turma, statusAnterior);
            }
        }

        private void VerificarStatus(Turma turma, enumStatusTurma? statusAnterior)
        {
            var exceptionCancelamento = string.Empty;

            var matriculasTurma = new BMMatriculaTurma().ObterPorTurma(turma);

            if (turma.Status == enumStatusTurma.Cancelada)
            {
                try
                {
                    NotificarCancelamento(turma);
                }
                catch (AcademicoException ex)
                {
                    exceptionCancelamento = ex.Message;
                }

                // Alterar status das matrículas para Cancelado ADM.
                foreach (var matricula in matriculasTurma)
                {
                    matricula.MatriculaOferta.StatusMatricula = enumStatusMatricula.CanceladoTurma;
                    new ManterMatriculaOferta().Salvar(matricula.MatriculaOferta);
                    new ManterItemTrilhaParticipacao().AtualizarStatusParticipacoesTrilhas(matricula.MatriculaOferta);
                }
            }

            // Caso a turma esteja sendo alterada de cancelamento para prevista, altera as matrículas para Inscrito.
            if (statusAnterior == enumStatusTurma.Cancelada && turma.Status == enumStatusTurma.Prevista)
            {
                // Alterar status das matrículas para Inscrito.
                foreach (var matricula in matriculasTurma)
                {
                    matricula.MatriculaOferta.StatusMatricula = enumStatusMatricula.Inscrito;
                    new ManterMatriculaOferta().Salvar(matricula.MatriculaOferta);
                    new ManterItemTrilhaParticipacao().AtualizarStatusParticipacoesTrilhas(matricula.MatriculaOferta);
                }
            }

            if (turma.Status == enumStatusTurma.Confirmada)
            {
                try
                {
                    NotificarConfirmacao(turma);
                }
                catch (AcademicoException ex)
                {
                    throw new AcademicoException(
                        "Turma salva com sucesso, porém ocorreram os seguintes erros no envio de e-mail: " +
                        Environment.NewLine +
                        (!string.IsNullOrWhiteSpace(exceptionCancelamento)
                            ? exceptionCancelamento
                            : "") + Environment.NewLine + ex.Message);
                }
            }
        }

        private void NotificarCancelamento(Turma turma)
        {
            var envolvidos = ObterEnvolvidos(turma);
            
            var template = TemplateUtil.ObterInformacoes(enumTemplate.CancelamentoTurma);

            var assuntoDoEmail = template.Assunto;

            var corpoEmail = template.TextoTemplate;

            if (string.IsNullOrWhiteSpace(corpoEmail))
                throw new AcademicoException("Não há uma template para e-mail de cancelamento de turma cadastrada no sistema.");

            corpoEmail = corpoEmail.Replace("#JUSTIFICATIVA", turma.ObterJustificativa());

            foreach (var envolvido in envolvidos)
            {
                EmailUtil.Instancia.EnviarEmail(envolvido.Email.Trim(), assuntoDoEmail, corpoEmail);
            }
        }

        private void NotificarConfirmacao(Turma turma)
        {
            var envolvidos = ObterEnvolvidos(turma);

            var template = TemplateUtil.ObterInformacoes(enumTemplate.ConfirmacaoTurma);

            var assuntoDoEmail = template.Assunto;

            var corpoEmail = template.TextoTemplate;

            if (string.IsNullOrWhiteSpace(corpoEmail))
                throw new AcademicoException("Não há uma template para e-mail de confirmação de turma cadastrada no sistema.");

            foreach (var envolvido in envolvidos)
            {
                EmailUtil.Instancia.EnviarEmail(envolvido.Email.Trim(), assuntoDoEmail, corpoEmail);
            }
        }

        private IEnumerable<Usuario> ObterEnvolvidos(Turma turma)
        {
            var envolvidos = new List<Usuario>();

            // Adicionar alunos.
            if (turma.ListaMatriculas.Any())
                envolvidos.AddRange(
                    turma.ListaMatriculas.Select(
                        x => new ManterUsuario().ObterUsuarioPorID(x.MatriculaOferta.Usuario.ID)));

            // Adicionar professores.
            if(turma.Professores.Any())
                envolvidos.AddRange(turma.Professores);

            // Adicionar Responsável.
            if (turma.Responsavel != null)
                envolvidos.Add(turma.Responsavel);

            // Adicionar Consultor Educacional.
            if(turma.ConsultorEducacional != null)
                envolvidos.Add(turma.ConsultorEducacional);

            return envolvidos.Distinct();
        }

        public IQueryable<Turma> ObterTodasTurma()
        {
            return bmTurma.ObterTodos();
        }

        public IList<Turma> ObterTurmasPorUsuarioId(int idUsuario)
        {
            BMTurmaProfessor bmTurmaProfessor = new BMTurmaProfessor();
            return bmTurmaProfessor.ObterPorFiltro(new TurmaProfessor { Professor = new Usuario { ID = idUsuario } }).Select(x => x.Turma).ToList();
        }

        public Turma ObterTurmaPorID(int pId, bool carregarProfessores = true)
        {
            return bmTurma.ObterPorID(pId, carregarProfessores);
        }

        public IQueryable<Turma> ObterTurmaPorFiltro(string nome, string idChaveExterna, int idOferta, int idSolucaoEducacional)
        {
            return bmTurma.ObterPorFiltro(nome, idChaveExterna, idOferta, idSolucaoEducacional);
        }

        public void ExcluirTurma(int IdTurma)
        {
            try
            {
                Turma turma = null;

                if (IdTurma > 0)
                {
                    turma = bmTurma.ObterPorID(IdTurma);
                }

                bmTurma.Excluir(turma);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public IQueryable<Turma> ObterTurmasPorOferta(int idOferta, bool filtarPorGestor = true)
        {
            if (!filtarPorGestor)
                return ObterTodasTurma().Where(x => x.Oferta.ID == idOferta);

            var ses = new ManterSolucaoEducacional().ObterTodosPorGestor();

            var oferta = ses.SelectMany(x => x.ListaOferta).FirstOrDefault(x => idOferta == 0 || x.ID == idOferta);

            var retorno = oferta != null ? oferta.ListaTurma : new List<Turma>();

            return retorno.AsQueryable();
        }

        public int? ObterProximoCodigoSequencial(Oferta oferta)
        {
            return bmTurma.ObterProximoCodigoSequencial(oferta);
        }

        public bool AlterouOferta(int idTurma, Oferta novaOferta)
        {
            return bmTurma.AlterouOferta(idTurma, novaOferta);
        }

        public void LimparSessao()
        {
            bmTurma.LimparSessao();
        }

        public IQueryable<Turma> ObterTurmasPorQuestionario(int idOferta, int? idProfessor)
        {
            return bmTurma.ObterTurmasPorQuestionario(idOferta, idProfessor);
        }

        public IQueryable<Turma> ObterTurmasPendentes(Usuario usuario)
        {
            var parametros = new Dictionary<string, object> { { "pUsuarioId", usuario.ID } };

            var idsTurmas = bmTurma.ExecutarProcedure<DTOTurma>("SP_TURMAS_ABANDONO_PENDENTES", parametros);

            return idsTurmas.Select(turma => ObterTurmaPorID(turma.ID, false)).AsQueryable();
        }

        public IQueryable<Turma> ObterTodosIQueryable()
        {
            return bmTurma.ObterTodosIQueryable();
        }
    }
}
