using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Services.HistoricoAcademico;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System;
using Sebrae.Academico.BP.Services.SgusWebService;

namespace Sebrae.Academico.BP.Services
{
    public class HistoricoAcademicoServices : BusinessProcessServicesBase
    {
        public List<DTOItemHistoricoAcademico> ConsultarHistorico(int id_Usuario)
        {
            try
            {
                var listaMatriculaOferta = new BMMatriculaOferta()
                      .ObterPorUsuario(id_Usuario)
                      .Where(x => x.StatusMatricula != enumStatusMatricula.CanceladoAdm && x.StatusMatricula != enumStatusMatricula.PendenteConfirmacaoAluno)
                      .ToList();

                var resultado = PreencherDTOComInformacoesDaMatriculaOferta(listaMatriculaOferta, id_Usuario);

                PreencherDTOComInformacoesDoUsuarioTrilha(id_Usuario, resultado);
                PreencherDTOComInformacoesDaMatriculaPrograma(id_Usuario, resultado);
                PreencherDTOComInformacoesDoHistoricoExtraCurricular(id_Usuario, resultado);
                PreencherDTOComInformacoesDoHistoricoSGTC(id_Usuario, resultado);

                return resultado.OrderByDescending(x => ConverterData(x.DataInicio)).ToList();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<DTOItemHistoricoTutoria> ConsultarHistoricoTutoria(int usuarioId)
        {
            var turmas = new BMTurma().ObterTurmasFechadasDoProfessor(usuarioId).ToList();

            var lista = turmas.Select(t => new DTOItemHistoricoTutoria
            {
                IdTurma = t.ID,
                NomeSolucao = t.Oferta.SolucaoEducacional.Nome,
                IdOferta = t.Oferta.ID,
                Instituicao = Constantes.UCSebrae,
                DataInicio = t.DataInicio.ToString("dd/MM/yyyy"),
                DataFim = t.DataFinal.HasValue ? t.DataFinal.Value.ToString("dd/MM/yyyy") : "",
                TemCertificado = t.Oferta.CertificadoTemplateProfessor != null
            }).ToList();

            return lista;
        }

        private DateTime? ConverterData(string dataString)
        {
            if (string.IsNullOrEmpty(dataString))
                return null;

            var dataSplit = dataString.Split('/');

            return new DateTime(int.Parse(dataSplit[2]), int.Parse(dataSplit[1]), int.Parse(dataSplit[0]));
        }

        private void PreencherDTOComInformacoesDoHistoricoSGTC(int id_Usuario, List<DTOItemHistoricoAcademico> resultado)
        {
            IList<HistoricoSGTC> ListaHistoricoSGTC = new BMHistoricoSGTC().ObterPorUsuario(id_Usuario);

            foreach (var itemHistorico in ListaHistoricoSGTC)
            {

                var item = new DTOItemHistoricoAcademico();
                item.NomeSolucao = itemHistorico.NomeSolucaoEducacional;
                item.Instituicao = Constantes.SGTC;

                item.DataInicio = null;
                item.DataFim = itemHistorico.DataConclusao.ToShortDateString();

                item.IdMatricula = 0;
                item.Situacao = Constantes.Concluido;
                item.Tipo = Constantes.HistoricoSGTC;
                item.TemCertificado = false;
                item.CargaHoraria = Constantes.NaoDefinido;
                resultado.Add(item);
            }
        }

        private void PreencherDTOComInformacoesDoHistoricoExtraCurricular(int id_Usuario, List<DTOItemHistoricoAcademico> resultado)
        {
            IList<HistoricoExtraCurricular> ListaHistoricoExtraCurricular = new BMHistoricoExtraCurricular().ObterPorUsuario(id_Usuario);

            foreach (var itemHistorico in ListaHistoricoExtraCurricular)
            {

                var item = new DTOItemHistoricoAcademico();
                item.NomeSolucao = itemHistorico.SolucaoEducacionalExtraCurricular;
                item.Instituicao = string.IsNullOrEmpty(itemHistorico.Instituicao) ? "" : itemHistorico.Instituicao;

                DateTime? data = itemHistorico.DataInicioAtividade.HasValue ? itemHistorico.DataInicioAtividade : null;
                item.DataInicio = data.HasValue ? data.Value.ToString("dd/MM/yyyy") : "";
                item.DataFim = itemHistorico.DataFimAtividade.HasValue
                    ? itemHistorico.DataFimAtividade.Value.ToString("dd/MM/yyyy")
                    : "";

                item.IdMatricula = 0;
                item.Situacao = Constantes.Concluido;
                item.Tipo = Constantes.HistoricoExtraCurricularUC;
                item.TemCertificado = itemHistorico.FileServer != null;

                if (itemHistorico.FileServer != null)
                {
                    item.LKCertificado = Util.Classes.ConfiguracaoSistemaUtil
                            .ObterInformacoes(enumConfiguracaoSistema.EnderecoSGUS)
                            .Registro + "/MediaServer.ashx?Identificador=" + itemHistorico.FileServer.ID;
                }

                item.CargaHoraria = itemHistorico.CargaHoraria.ToString();

                if (itemHistorico.ID > 0)
                {
                    item.idExtraCurricular = itemHistorico.ID;
                }

                resultado.Add(item);
            }
        }

        private void PreencherDTOComInformacoesDaMatriculaPrograma(int idUsuario, List<DTOItemHistoricoAcademico> resultado)
        {
            var manterMatriculaCapacitacao = new ManterMatriculaCapacitacaoService();
            var listaMatriculaCapacitacao = manterMatriculaCapacitacao.ObterPorUsuario(idUsuario);

            //UST #1158: Exibir cursos, programas e trilhas com status de inscrito no historico academico. Foir removido .Where(x => x.StatusMatricula != enumStatusMatricula.Inscrito)
            foreach (
                var itemHistorico in
                    listaMatriculaCapacitacao)
            {
                var item = new DTOItemHistoricoAcademico
                {
                    NomeSolucao = itemHistorico.Capacitacao.Programa.Nome,
                    Instituicao = Constantes.UCSebrae
                };

                var data = itemHistorico.Capacitacao.DataInicio;
                item.DataInicio = data.ToString("dd/MM/yyyy");
                item.DataFim = itemHistorico.Capacitacao.DataFim.HasValue
                    ? itemHistorico.Capacitacao.DataFim.Value.ToString("dd/MM/yyyy")
                    : "";

                item.IdMatricula = itemHistorico.ID;
                item.Situacao = itemHistorico.StatusMatriculaFormatado;
                item.Tipo = Constantes.ProgramaUC;

                if (itemHistorico.ListaMatriculaTurmaCapacitacao != null &&
                    itemHistorico.ListaMatriculaTurmaCapacitacao.Any())
                {
                    var matriculaTurma = itemHistorico.ListaMatriculaTurmaCapacitacao.FirstOrDefault();
                    if (matriculaTurma == null) continue;
                    item.IdTurma = matriculaTurma.TurmaCapacitacao.ID;
                    if (matriculaTurma.TurmaCapacitacao.ListaQuestionarioAssociacao.Any())
                    {
                        var itemQuestionarioAssociacaoPos =
                            matriculaTurma.TurmaCapacitacao.ListaQuestionarioAssociacao.FirstOrDefault(
                                x => x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pos);
                        if (itemQuestionarioAssociacaoPos != null)
                        {
                            if (
                                !(new BMQuestionarioParticipacao().ObterPorUsuario(itemHistorico.Usuario)
                                    .Any(
                                        x =>
                                            x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pos &&
                                            x.TurmaCapacitacao != null &&
                                            x.TurmaCapacitacao.ID == matriculaTurma.TurmaCapacitacao.ID &&
                                            x.DataParticipacao != null)))
                            {
                                item.QuestionarioPosPendente = true;
                            }
                        }
                    }
                }


                item.TemCertificado = (itemHistorico.Capacitacao.Certificado != null && !item.QuestionarioPosPendente);

                item.CargaHoraria = Constantes.NaoDefinido;
                resultado.Add(item);
            }
        }

        private void PreencherDTOComInformacoesDoUsuarioTrilha(int id_Usuario, List<DTOItemHistoricoAcademico> resultado)
        {
            IList<UsuarioTrilha> ListaUsuarioTrilha = new BMUsuarioTrilha().ObterPorUsuario(id_Usuario);

            //UST #1158: Exibir cursos, programas e trilhas com status de inscrito no historico academico. Foir removido .Where(x => x.StatusMatricula != enumStatusMatricula.Inscrito)
            foreach (var itemHistorico in ListaUsuarioTrilha)
            {
                //if (itemHistorico.StatusMatricula.Equals(enumStatusMatricula.Concluido))
                //{
                var item = new DTOItemHistoricoAcademico
                {
                    NomeSolucao =
                        string.Concat(itemHistorico.TrilhaNivel.Trilha.Nome, " - ", itemHistorico.TrilhaNivel.Nome),
                    Instituicao = Constantes.UCSebrae
                };

                var data = itemHistorico.DataInicio;
                item.DataInicio = data.ToString("dd/MM/yyyy");
                item.DataFim = itemHistorico.DataFim.HasValue ? itemHistorico.DataFim.Value.ToString("dd/MM/yyyy") : "";

                item.IdMatricula = itemHistorico.ID;
                item.Situacao = itemHistorico.StatusMatriculaFormatado;
                item.Tipo = Constantes.TrilhaUC;
                if (itemHistorico.StatusMatricula == enumStatusMatricula.Concluido ||
                    itemHistorico.StatusMatricula == enumStatusMatricula.Aprovado)
                {
                    item.TemCertificado = (itemHistorico.TrilhaNivel.CertificadoTemplate != null);
                }
                item.IdTrilhaNivel = itemHistorico.TrilhaNivel.ID;
                item.CargaHoraria = itemHistorico.TrilhaNivel.CargaHoraria.ToString();
                resultado.Add(item);
                //}
            }
        }

        private List<DTOItemHistoricoAcademico> PreencherDTOComInformacoesDaMatriculaOferta(IList<MatriculaOferta> listaMatriculaOferta, int idUsuario)
        {
            List<DTOItemHistoricoAcademico> resultado = new List<DTOItemHistoricoAcademico>();

            var usuario = (new BMUsuario()).ObterPorId(idUsuario);

            foreach (var itemHistorico in listaMatriculaOferta)
            {
                // Usado para ordenar os objetos corretamente
                itemHistorico.MatriculaTurma = itemHistorico.MatriculaTurma.OrderByDescending(x => x.DataMatricula).ToList();

                var itemMatriculaTurma = itemHistorico.MatriculaTurma.FirstOrDefault();

                if (itemMatriculaTurma == null)
                    continue;

                var item = new DTOItemHistoricoAcademico
                {
                    NomeSolucao = itemHistorico.Oferta.SolucaoEducacional.Nome,
                    Instituicao = Constantes.UCSebrae
                };

                ObterQuestionariosCancelamentoAbandono(itemHistorico, item);

                if (itemHistorico.MatriculaTurma != null && itemHistorico.MatriculaTurma.Count > 0)
                {
                    preencherDataInicioDataFimCurso(itemMatriculaTurma, itemHistorico.Oferta, ref item);

                    //A FGV QUE EFETUA A CORREÇÃO NÃO ATUALIZA O CAMPO QUE NÃO É UTILIZADO NO MÉTODO, ENTÃO PRECISAMOS CRIAR ESTA CONDIÇÃO
                    if (itemHistorico.StatusMatricula == enumStatusMatricula.Aprovado ||
                        itemHistorico.StatusMatricula == enumStatusMatricula.Concluido)
                    {
                        if (!itemMatriculaTurma.DataTermino.HasValue)
                        {
                            if (itemHistorico.Oferta.SolucaoEducacional.Fornecedor.ID == (int)enumFornecedor.FGVOCW)
                            {
                                if (itemHistorico.DataStatusMatricula.HasValue)
                                {
                                    item.DataFim = itemHistorico.DataStatusMatricula.Value.ToString("dd/MM/yyyy");
                                }
                            }
                        }
                    }

                    item.IdTurma = itemMatriculaTurma.Turma.ID;

                    var turma = new BMTurma().ObterPorID((int)item.IdTurma);

                    var questionarioAssociacao = new BMQuestionarioAssociacao().ObterPorTurma(turma).Select(x => x.DataDisparoLinkEficacia);

                    foreach (var itemQuestionarioAssociacao in questionarioAssociacao)
                    {
                        DateTime dateTime;
                        if (DateTime.TryParse(itemQuestionarioAssociacao.ToString(), out dateTime))
                        {
                            item.DataDisparoLinkEficacia = dateTime; 
                        }
                    }

                    // VALIDAR QUESTIONARIOS
                    if (itemMatriculaTurma.Turma.ListaQuestionarioAssociacao.Count > 0 && !itemHistorico.IsOuvinte())
                    // Alunos com status Ouvinte não podem responder questionáris.
                    {
                        //QUESTIONARIO POS
                        var itemQuestionarioAssociacaoPos =
                            itemMatriculaTurma.Turma.ListaQuestionarioAssociacao.FirstOrDefault(
                                x => x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pos);

                        if (itemQuestionarioAssociacaoPos != null)
                        {
                            var temParticipacao = new BMQuestionarioParticipacao()
                                .ObterPorUsuario(itemHistorico.Usuario).Any(
                                    x =>
                                        x.TipoQuestionarioAssociacao != null &&
                                        x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pos &&
                                        x.Turma.ID == itemMatriculaTurma.Turma.ID && x.DataParticipacao != null);

                            // Usado para exibir o quesitonário pós 
                            item.DataDisparoLinkPesquisa = !temParticipacao
                                ? itemQuestionarioAssociacaoPos.DataDisparoLinkPesquisa
                                : null;

                            if (UsuarioAprovado(itemHistorico.StatusMatricula))
                            {
                                if (!temParticipacao)
                                {
                                    item.QuestionarioPosPendente = true;
                                    item.DataDisparoLinkPesquisa =
                                        itemQuestionarioAssociacaoPos.DataDisparoLinkPesquisa ??
                                        itemMatriculaTurma.DataTermino;
                                }
                            }
                        }

                        //QUESTIONARIO EFICACIA
                        var itemQuestionarioAssociacaoEficacia =
                            itemMatriculaTurma.Turma.ListaQuestionarioAssociacao.FirstOrDefault(
                                x => x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Eficacia);
                        if (itemQuestionarioAssociacaoEficacia != null)
                        {
                            var temParticipacao = new BMQuestionarioParticipacao()
                               .ObterPorUsuario(itemHistorico.Usuario).Any(
                                   x =>
                                       x.TipoQuestionarioAssociacao != null &&
                                       x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Eficacia &&
                                       x.Turma.ID == itemMatriculaTurma.Turma.ID && x.DataParticipacao != null);

                            if (!temParticipacao)
                            {
                                item.QuestionarioEficaciaPendente = true;
                                int quantidadeItens = ConsultarQuantidadeItensQuestionarioAssociados(itemMatriculaTurma);
                                item.QuantidadeItensQuestionarioAgrupados = quantidadeItens > 0 ? quantidadeItens : (int?)null;
                            }
                        }
                    }
                }
                else
                {
                    var matriculaTurma = itemHistorico.MatriculaTurma.FirstOrDefault();
                    preencherDataInicioDataFimCurso(matriculaTurma, matriculaTurma.MatriculaOferta.Oferta, ref item);
                }

                item.IdMatricula = itemHistorico.ID;
                item.Situacao = itemHistorico.StatusMatriculaFormatado;
                item.Tipo = Constantes.CursoUC;

                if (itemHistorico.MatriculaTurma != null)
                {
                    var matTurma = (new ManterMatriculaTurma()).ObterMatriculaTurmaPorId(itemMatriculaTurma.ID);
                    item.Feedback = (matTurma.Feedback ?? "").Trim();
                }

                PreencherCertificado(itemHistorico, item, usuario);

                resultado.Add(item);
            }

            return resultado;
        }

        /// <summary>
        /// Atualiza a data início e data fim do curso no histórico acadêmico
        /// </summary>
        /// <param name="matricula"></param>
        /// <param name="historico"></param>
        /// <returns></returns>
        public void preencherDataInicioDataFimCurso(MatriculaTurma matricula, Oferta oferta, ref DTOItemHistoricoAcademico historico)
        {
            if (oferta.TipoOferta != enumTipoOferta.Continua)
            {
                historico.DataInicio = matricula.Turma.DataInicio.ToString("dd/MM/yyyy");
                historico.DataFim = matricula.Turma.DataFinal.HasValue ? matricula.Turma.DataFinal.Value.ToString("dd/MM/yyyy") : "";
            }
            else
            {
                historico.DataInicio = matricula.DataMatricula.ToString("dd/MM/yyyy");
                historico.DataFim = matricula.DataTermino.HasValue
                    ? matricula.DataTermino.Value.ToString("dd/MM/yyyy")
                    : "";
            }
        }

        /// <summary>
        /// Consulta a quantidade de itens questionário agrupados
        /// </summary>
        /// <param name="matricula"></param>
        /// <returns></returns>
        public int ConsultarQuantidadeItensQuestionarioAssociados(MatriculaTurma matricula)
        {
            if (matricula.Turma.ListaQuestionarioAssociacao.Count > 0)
            {
                var itemQuestionarioAssociacaoEficacia =
                    matricula.Turma.ListaQuestionarioAssociacao.FirstOrDefault(
                        x => x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Eficacia);
                if (itemQuestionarioAssociacaoEficacia != null)
                {
                    int quantidade = new ManterQuestionario().QuantidadeQuestionariosAgrupados(itemQuestionarioAssociacaoEficacia.Questionario);
                    return quantidade > 0 ? quantidade : 1;
                }
            }

            return 1;
        }

        private static void ObterQuestionariosCancelamentoAbandono(MatriculaOferta matriculaOferta,
            DTOItemHistoricoAcademico dto)
        {
            if (matriculaOferta.StatusMatricula == enumStatusMatricula.CanceladoAluno ||
                matriculaOferta.StatusMatricula == enumStatusMatricula.Abandono &&
                matriculaOferta.MatriculaTurma != null && matriculaOferta.MatriculaTurma.Any())
            {
                var matriculaTurma = matriculaOferta.MatriculaTurma.FirstOrDefault();

                if (matriculaTurma != null)
                {
                    var turma = matriculaTurma.Turma;

                    var questionariosAssociacoes = new BMQuestionarioAssociacao().ObterPorTurma(turma).ToList();

                    if (matriculaOferta.StatusMatricula == enumStatusMatricula.CanceladoAluno)
                    {
                        // Obter questionário de cancelamento.
                        var associacaoCancelamento =
                            questionariosAssociacoes.FirstOrDefault(
                                q =>
                                    q.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Cancelamento);

                        if (associacaoCancelamento != null)
                        {
                            // Caso não tenha sido respondido, retorna o ID da turma do questionário de cancelamento para o usuário responder.
                            if (!associacaoCancelamento.Questionario.IsRespondido(matriculaOferta.Usuario, turma))
                            {
                                dto.IdTurmaQuestionarioCancelamento = turma.ID;
                            }
                        }
                    }

                    // Obter questionário de abandono.
                    if (matriculaOferta.StatusMatricula == enumStatusMatricula.Abandono)
                    {
                        var associacaoAbandono =
                            questionariosAssociacoes.FirstOrDefault(
                                q => q.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Abandono);

                        if (associacaoAbandono != null)
                        {
                            // Caso não tenha sido respondido, retorna o ID da turma do questionário de abandono para o usuário responder.
                            if (!associacaoAbandono.Questionario.IsRespondido(matriculaOferta.Usuario, turma))
                            {
                                dto.IdTurmaQuestionarioAbandono = turma.ID;
                            }
                        }
                    }
                }
            }
        }

        private void PreencherCertificado(MatriculaOferta itemHistorico, DTOItemHistoricoAcademico item, Usuario usuario)
        {
            if (itemHistorico.IsAprovado() && !itemHistorico.IsOuvinte())
            // Alunos com o status de Ouvinte não podem emitir certificados.
            {
                item.TemCertificado = itemHistorico.Oferta.CertificadoTemplate != null;
                item.CargaHoraria = itemHistorico.Oferta.CargaHoraria.ToString();

                if (itemHistorico.Oferta.SolucaoEducacional.Fornecedor.ID == (int)enumFornecedor.FGVOCW)
                {
                    var nomeArquivoOriginal = "FGVOCW_" + itemHistorico.Usuario.CPF + "_" +
                                              itemHistorico.Oferta.IDChaveExterna + ".pdf";
                    var mFS = new ManterFileServer();
                    var fileServer = mFS.ObterFileServerPorFiltro(new FileServer
                    {
                        NomeDoArquivoOriginal = nomeArquivoOriginal,
                        MediaServer = true
                    }).FirstOrDefault();
                    item.TemCertificado = true;
                    if (fileServer != null)
                    {
                        item.LKCertificado =
                            Util.Classes.ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.EnderecoSGUS)
                                .Registro + "/MediaServer.ashx?Identificador=" + fileServer.ID;
                    }
                    else
                    {
                        item.LKCertificado = "http://www5.fgv.br/ucsebraeocw/CertificadoCursoGratuitoOnline.aspx?cpf=" +
                                             itemHistorico.Usuario.CPF + "&oferta=" +
                                             itemHistorico.Oferta.IDChaveExterna;
                    }
                }

                // Link de acesso ao curso - Somente se tiver sido aprovado
                if (itemHistorico.MatriculaTurma != null && itemHistorico.MatriculaTurma.FirstOrDefault() != null)
                {
                    var matTurma = itemHistorico.MatriculaTurma.FirstOrDefault();
                    if (matTurma.Turma.AcessoAposConclusao)
                    {
                        item.LKAcesso = string.IsNullOrEmpty(matTurma.MatriculaOferta.LinkAcesso)
                            ? (new ConsultarMeusCursos()).ConsultarLinkAcessoFornecedor(
                                matTurma.MatriculaOferta.Oferta.SolucaoEducacional.Fornecedor, usuario,
                                matTurma.MatriculaOferta.Oferta.CodigoMoodle.ToString())
                            : matTurma.MatriculaOferta.LinkAcesso;
                    }
                }
            }
        }

        public bool UsuarioAprovado(enumStatusMatricula status)
        {
            switch (status)
            {
                case enumStatusMatricula.Concluido:
                case enumStatusMatricula.Aprovado:
                case enumStatusMatricula.AprovadoComoMultiplicador:
                case enumStatusMatricula.AprovadoComoMultiplicadorComAcompanhamento:
                case enumStatusMatricula.AprovadoComoFacilitador:
                case enumStatusMatricula.AprovadoComoFacilitadorComAcompanhamento:
                case enumStatusMatricula.AprovadoComoConsultor:
                case enumStatusMatricula.AprovadoComoConsultorComAcompanhamento:
                case enumStatusMatricula.AprovadoComoModerador:
                case enumStatusMatricula.AprovadoComoModeradorComAcompanhamento:
                case enumStatusMatricula.AprovadoComoFacilitadorConsultor:
                case enumStatusMatricula.AprovadoComoGestor:
                case enumStatusMatricula.AprovadoComoFacilitadorConsultorComAcompanhamento:
                    return true;
                default:
                    return false;
            }
        }
    }
}