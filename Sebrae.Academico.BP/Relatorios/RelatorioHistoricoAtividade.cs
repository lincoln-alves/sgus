using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioHistoricoAtividade : BusinessProcessBaseRelatorio, IDisposable
    {

        protected override enumRelatorio Relatorio
        {
            get { return enumRelatorio.HistoricoDeAtividades; }
        }

        public IList<Trilha> ObterTrilhaTodos()
        {
            using (BMTrilha trilhaBM = new BMTrilha())
            {
                return trilhaBM.ObterTrilhas();
            }
        }


        public IList<TrilhaNivel> ObterTrilhaNivelPorTrilha(int pIdTrilha)
        {
            using (BMTrilhaNivel trilhaNivelBM = new BMTrilhaNivel())
            {
                using (BMTrilha tr = new BMTrilha())
                {
                    return trilhaNivelBM.ObterPorTrilha(tr.ObterPorId(pIdTrilha));
                }
            }
        }

        public IList<DTORelatorioHistoricoAtividadeDadosBasicos> ObterDTORelatorioHistoricoAtividadeDadosBasicos(IList<DTOAlunoDaTrilha> listaDTOAlunoDaTrilha)
        {

            var listaRelAtivDTO = listaDTOAlunoDaTrilha.Select(x =>
                   new DTORelatorioHistoricoAtividadeDadosBasicos()
                   {
                       IDTopicoTematico = x.IdTopicoTematico,
                       NivelOcupacional = x.NivelOcupacional,
                       Nivel = x.Nivel,
                       Trilha = x.Trilha,
                       StatusMatricula = x.StatusMatricula,
                       Nome = x.Nome,
                       UF = x.UF,
                       TopicoTematico = x.TopicoTematico,
                       DataInicio = x.DataInicio,
                       DataLimite = x.DataLimite,
                       StatusAtividadeFormativa = x.StatusAtividadeFormativa
                   }).ToList();

            return listaRelAtivDTO;

        }

        public IList<DTORelatorioHistoricoAtividadeSolucoesPortifolio> ConsultaHistoricoAtividadeSolucoesPortifolio(IList<DTOAlunoDaTrilha> listaDTOAlunoDaTrilha,
                                                                                                                    int pIdTopicoTematico)
        {

            IList<DTORelatorioHistoricoAtividadeSolucoesPortifolio> lstResult = new List<DTORelatorioHistoricoAtividadeSolucoesPortifolio>();
            DTORelatorioHistoricoAtividadeSolucoesPortifolio dtoRelatorioHistoricoAtividadeSolucoesPortifolio = null;

            IList<DTOAlunoDaTrilha> listaFiltradaDTOAlunoDaTrilha = listaDTOAlunoDaTrilha.Where(x => x.IdTopicoTematico == pIdTopicoTematico).ToList();

            foreach (DTOAlunoDaTrilha dtoAlunoDaTrilha in listaFiltradaDTOAlunoDaTrilha)
            {

                dtoRelatorioHistoricoAtividadeSolucoesPortifolio = new DTORelatorioHistoricoAtividadeSolucoesPortifolio()
                {
                    IDTopicoTematico = dtoAlunoDaTrilha.IdTopicoTematico,
                    NomeSolucao = dtoAlunoDaTrilha.NomeItemTrilha,
                    TemParticipacaoNaSolucao = dtoAlunoDaTrilha.TemParticipacaoNaSolucao,
                    TemParticipacaoNoTopico = dtoAlunoDaTrilha.TemParticipacaoNoTopico,
                    IsAutoIndicativa = dtoAlunoDaTrilha.IsAutoIndicativa,
                    Objetivo = dtoAlunoDaTrilha.Objetivo,
                };

                lstResult.Add(dtoRelatorioHistoricoAtividadeSolucoesPortifolio);

            }

            return lstResult;

        }

        public IList<DTORelatorioHistoricoAtividadeSolucoesAutoindicativa> ConsultarHistoricoAtividadeSolucoesAutoIndicativas(IList<DTOAlunoDaTrilha> listaDTOAlunoDaTrilha,
                                                                                                                              int pIdTopicoTematico)
        {

            IList<DTORelatorioHistoricoAtividadeSolucoesAutoindicativa> lstResult = new List<DTORelatorioHistoricoAtividadeSolucoesAutoindicativa>();
            DTORelatorioHistoricoAtividadeSolucoesAutoindicativa dtoRelatorioHistoricoAtividadeSolucoesAutoindicativa = null;

            IList<DTOAlunoDaTrilha> listaDTOAlunoDaTrilhaFiltrada = listaDTOAlunoDaTrilha.Where(x => x.IdTopicoTematico == pIdTopicoTematico && x.IsAutoIndicativa == true).ToList();

            foreach (DTOAlunoDaTrilha dtoAlunoDaTrilha in listaDTOAlunoDaTrilhaFiltrada)
            {

                dtoRelatorioHistoricoAtividadeSolucoesAutoindicativa = new DTORelatorioHistoricoAtividadeSolucoesAutoindicativa()
                {
                    IDTopicoTematico = dtoAlunoDaTrilha.IdTopicoTematico,
                    NomeSolucao = dtoAlunoDaTrilha.NomeItemTrilha,
                    TemParticipacaoNaSolucao = dtoAlunoDaTrilha.TemParticipacaoNaSolucao,
                    TemParticipacaoNoTopico = dtoAlunoDaTrilha.TemParticipacaoNoTopico,
                    IsAutoIndicativa = dtoAlunoDaTrilha.IsAutoIndicativa,
                    Objetivo = dtoAlunoDaTrilha.Objetivo
                };

                lstResult.Add(dtoRelatorioHistoricoAtividadeSolucoesAutoindicativa);

            }

            return lstResult;

        }

        public IList<DTORelatorioHistoricoAtividadeSprint> ConsultaHistoricoAtividadeSprint(int pIdUsuarioTrilha, int pIdTopicoTematico)
        {
            using (BMTrilhaAtividadeFormativaParticipacao tafpBm = new BMTrilhaAtividadeFormativaParticipacao())
            {
                using (BMUsuarioTrilha usuarioTrilhaBM = new BMUsuarioTrilha())
                {
                    UsuarioTrilha ut = usuarioTrilhaBM.ObterPorId(pIdUsuarioTrilha);
                    var lstTAFP = ut.ListaTrilhaAtividadeFormativaParticipacao;
                    IList<DTORelatorioHistoricoAtividadeSprint> lstResult = new List<DTORelatorioHistoricoAtividadeSprint>();

                    foreach (var r in lstTAFP.Where(x => x.TrilhaTopicoTematico.ID == pIdTopicoTematico).ToList())
                    {
                        lstResult.Add(new DTORelatorioHistoricoAtividadeSprint()
                        {
                            IDTopicoTematico = r.TrilhaTopicoTematico.ID,
                            TopicoTematico = r.TrilhaTopicoTematico.NomeExibicao,
                            RegistroAprendizagem = r.TextoParticipacao
                        });
                    }

                    return lstResult.Distinct().ToList();

                }
            }
        }

        public IList<DTORelatorioHistoricoAtividadeTopicoTematicoCount> ConsultarHistoricoAtividadeTopicoTematico(IList<DTOAlunoDaTrilha> listaDTOAlunoDaTrilha)
        {

            IList<DTORelatorioHistoricoAtividadeTopicoTematicoCount> listaDTORelatorioHistoricoAtividadeTopicoTematicoCount =
                new List<DTORelatorioHistoricoAtividadeTopicoTematicoCount>();

            DTORelatorioHistoricoAtividadeTopicoTematicoCount dtoRelatorioHistoricoAtividadeTopicoTematicoCount = null;

            using (BMItemTrilha itemBM = new BMItemTrilha())
            {
                using (BMUsuarioTrilha utBM = new BMUsuarioTrilha())
                {

                    foreach (DTOAlunoDaTrilha dtoAlunoDaTrilha in listaDTOAlunoDaTrilha)
                    {
                        dtoRelatorioHistoricoAtividadeTopicoTematicoCount = new DTORelatorioHistoricoAtividadeTopicoTematicoCount()
                        {
                            IDTopicoTematico = dtoAlunoDaTrilha.IdTopicoTematico,
                            TopicoTomatico = dtoAlunoDaTrilha.TopicoTematico,
                            TemParticipacaoNaSolucao = dtoAlunoDaTrilha.TemParticipacaoNaSolucao,
                            TemParticipacaoNoTopico = dtoAlunoDaTrilha.TemParticipacaoNoTopico,
                            IsAutoIndicativa = dtoAlunoDaTrilha.IsAutoIndicativa,
                            Objetivo = dtoAlunoDaTrilha.Objetivo
                        };

                        listaDTORelatorioHistoricoAtividadeTopicoTematicoCount.Add(dtoRelatorioHistoricoAtividadeTopicoTematicoCount);
                    }


                    listaDTORelatorioHistoricoAtividadeTopicoTematicoCount.Distinct().ToList();

                }
            }

            return listaDTORelatorioHistoricoAtividadeTopicoTematicoCount;
        }

        public void Dispose()
        {
            GC.Collect();
        }

        public IList<UsuarioTrilha> ObterUsuarioTrilhaPorTrilhaNivelStatus(int idTrilha, int idNivel, string Situacao)
        {
            using (BMUsuarioTrilha userTrilhaBM = new BMUsuarioTrilha())
            {
                this.RegistrarLogExecucao();
                return userTrilhaBM.ObterPorFiltroTrilhaNivelSituacao(idTrilha, idNivel, Situacao);
            }
        }

        public UsuarioTrilha ObterUsuarioTrilhaPorID(int pIdUsuarioTrilha)
        {
            using (BMUsuarioTrilha userTrilhaBM = new BMUsuarioTrilha())
            {
                return userTrilhaBM.ObterPorId(pIdUsuarioTrilha);
            }
        }

        public IList<DTORelatorioHistoricoAtividadeNotaProva> ConsultaHistoricoAtividadeNotaProva(int pIdUsuarioTrilha)
        {
            using (BMUsuarioTrilha utBM = new BMUsuarioTrilha())
            {
                UsuarioTrilha ut = utBM.ObterPorId(pIdUsuarioTrilha);
                using (BMQuestionarioAssociacao qaBM = new BMQuestionarioAssociacao())
                {
                    return (from qp in new BMQuestionarioParticipacao().ObterPorUsuario(ut.Usuario)

                            where qp.TrilhaNivel != null && qp.TrilhaNivel.ID == ut.TrilhaNivel.ID
                                  && qp.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Prova



                            select new DTORelatorioHistoricoAtividadeNotaProva()
                            {
                                DataParticipacaoProva = qp.DataParticipacao,
                                DataGeracaoProva = qp.DataGeracao,
                                NotaProva = CommonHelper.CalcularPercentualDaProva(qp)
                            }).ToList();
                }
            }
        }


        public IList<DTORelatorioHistoricoAtividadeDiagnostico> ConsultarHistoricoAtividadeDiagnostico(int pIdUsuarioTrilha)
        {
            using (BMUsuarioTrilha utBM = new BMUsuarioTrilha())
            {
                UsuarioTrilha ut = utBM.ObterPorId(pIdUsuarioTrilha);
                using (BMQuestionarioAssociacao qaBM = new BMQuestionarioAssociacao())
                {
                    List<QuestionarioParticipacao> qps = new BMQuestionarioParticipacao().ObterPorUsuario(ut.Usuario).Where(
                            x => x.TrilhaNivel != null
                            && x.TrilhaNivel.ID == ut.TrilhaNivel.ID
                            && x.Evolutivo == true).ToList();
                            
                    List<DTORelatorioHistoricoAtividadeDiagnostico> retorno = new List<DTORelatorioHistoricoAtividadeDiagnostico>();
                    foreach (QuestionarioParticipacao qp in qps)
                    {
                        foreach(ItemQuestionarioParticipacao iq in qp.ListaItemQuestionarioParticipacao)
                        {
                            DTORelatorioHistoricoAtividadeDiagnostico atual = retorno.FirstOrDefault(x => x.ObjetivoItem == iq.Feedback);
                            if (atual == null)
                            {
                                atual = new DTORelatorioHistoricoAtividadeDiagnostico();
                                atual.ObjetivoItem = iq.Feedback;
                                atual.NotaPosItemI = qp.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pos ? iq.ListaOpcoesParticipacao.FirstOrDefault(x => x.RespostaSelecionada == true && x.TipoDiagnostico == enumTipoDiagnostico.Importancia).Nome : string.Empty;
                                atual.NotaPreItemI = qp.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pre ? iq.ListaOpcoesParticipacao.FirstOrDefault(x => x.RespostaSelecionada == true && x.TipoDiagnostico == enumTipoDiagnostico.Importancia).Nome : string.Empty;
                                atual.NotaPosItemD = qp.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pos ? iq.ListaOpcoesParticipacao.FirstOrDefault(x => x.RespostaSelecionada == true && x.TipoDiagnostico == enumTipoDiagnostico.Dominio).Nome : string.Empty;
                                atual.NotaPreItemD = qp.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pre ? iq.ListaOpcoesParticipacao.FirstOrDefault(x => x.RespostaSelecionada == true && x.TipoDiagnostico == enumTipoDiagnostico.Dominio).Nome : string.Empty;
                                retorno.Add(atual);

                            }
                            else
                            {
                                atual.ObjetivoItem = iq.Feedback;
                                atual.NotaPosItemI = qp.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pos ? iq.ListaOpcoesParticipacao.FirstOrDefault(x => x.RespostaSelecionada == true && x.TipoDiagnostico == enumTipoDiagnostico.Importancia).Nome : atual.NotaPosItemI;
                                atual.NotaPreItemI = qp.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pre ? iq.ListaOpcoesParticipacao.FirstOrDefault(x => x.RespostaSelecionada == true && x.TipoDiagnostico == enumTipoDiagnostico.Importancia).Nome : atual.NotaPreItemI;
                                atual.NotaPosItemD = qp.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pos ? iq.ListaOpcoesParticipacao.FirstOrDefault(x => x.RespostaSelecionada == true && x.TipoDiagnostico == enumTipoDiagnostico.Dominio).Nome : atual.NotaPosItemD;
                                atual.NotaPreItemD = qp.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pre ? iq.ListaOpcoesParticipacao.FirstOrDefault(x => x.RespostaSelecionada == true && x.TipoDiagnostico == enumTipoDiagnostico.Dominio).Nome : atual.NotaPreItemD;
                            }

                        }
                        
                    }
                    return retorno;
                }
            }
        }
    }
}
