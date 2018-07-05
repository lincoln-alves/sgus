using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.BP.DTO.Services.Questionario;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Seguranca.Autenticacao;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.BP.Services.SgusWebService
{
    public class ManterQuestionarioParticipacao : BusinessProcessServicesBase
    {
        public string UsuarioLogado { get; set; }

        public ManterQuestionarioParticipacao()
        {
        }

        public ManterQuestionarioParticipacao(string usuarioLogado)
        {
            UsuarioLogado = usuarioLogado;
        }

        /// <summary>
        /// Caso o usuário não possua seu clone do questionário, criar o clone.
        /// </summary>
        /// <param name="pQuestionario"></param>
        /// <param name="tipoQuestionario"></param>
        /// <param name="cpfAuditoria"></param>
        /// <param name="itemTrilhaId"></param>
        /// <param name="matricula"></param>
        public void CadastrarQuestionarioParticipacao(DTOCadastroQuestionarioParticipacao pQuestionario,
            TipoQuestionarioAssociacao tipoQuestionario, string cpfAuditoria, UsuarioTrilha matricula = null)
        {
            // Se o ID do ItemTrilha for informado, cria um objeto Dummy sem precisar buscar no banco.
            QuestionarioAssociacao questionarioAssociacao = null;
            if(pQuestionario.IdItemTrilha == null)
            {
                questionarioAssociacao = new BMQuestionario().ObterPorQuestionarioAssociacao(new QuestionarioAssociacao
                {                    
                    TrilhaNivel = new BMTrilhaNivel().ObterPorID(pQuestionario.IdTrilhaNivel),
                    Turma = new BMTurma().ObterPorID(pQuestionario.IdTurma),
                    TurmaCapacitacao = new BMTurmaCapacitacao().ObterPorId(pQuestionario.IdTurmaCapacitacao),
                    TipoQuestionarioAssociacao = tipoQuestionario,
                    Auditoria = new Auditoria(cpfAuditoria),
                    Evolutivo = pQuestionario.Evolutivo
                }, pQuestionario.IdUsuario);
            }
            else
            {
                questionarioAssociacao = new QuestionarioAssociacao
                {
                    TrilhaNivel = new BMTrilhaNivel().ObterPorID(pQuestionario.IdTrilhaNivel),
                    TipoQuestionarioAssociacao = tipoQuestionario,
                    Auditoria = new Auditoria(cpfAuditoria),
                    Evolutivo = pQuestionario.Evolutivo,
                    Questionario = new BMItemTrilha().ObterPorID(pQuestionario.IdItemTrilha.Value).Questionario
                };
            }

            if (questionarioAssociacao == null || questionarioAssociacao.Questionario == null)
                throw new AcademicoException("Não existem questionarios associados");

            var lsTurmaProfessor = new List<Usuario>();
            if (questionarioAssociacao.Turma != null)
            {
                lsTurmaProfessor = new BMTurmaProfessor()
                    .ObterPorFiltro(new TurmaProfessor { Turma = questionarioAssociacao.Turma })
                    .Select(p => p.Professor).ToList();
            }

            var usuario = (new BMUsuario()).ObterPorId(pQuestionario.IdUsuario);

            // Iniciar criação do clonezão bichãozão.
            var questionarioParticipacao = new QuestionarioParticipacao
            {
                DataGeracao = DateTime.Now,
                NivelOcupacional = usuario.NivelOcupacional,
                Questionario = questionarioAssociacao.Questionario,
                TrilhaNivel = questionarioAssociacao.TrilhaNivel,
                Turma = questionarioAssociacao.Turma,
                TurmaCapacitacao = questionarioAssociacao.TurmaCapacitacao,
                Usuario = usuario,
                Uf = usuario.UF,
                IdItemTrilha = pQuestionario.IdItemTrilha,
                Auditoria = new Auditoria(cpfAuditoria),
                TipoQuestionarioAssociacao = questionarioAssociacao.TipoQuestionarioAssociacao,
                ListaItemQuestionarioParticipacao = new List<ItemQuestionarioParticipacao>(),
                TextoEnunciadoPre = questionarioAssociacao.Questionario.TextoEnunciado,
                NotaMinima = questionarioAssociacao.Questionario.NotaMinima
            };

            if (questionarioAssociacao.Questionario.PrazoMinutos.HasValue &&
                questionarioAssociacao.Questionario.PrazoMinutos.Value > 0)
                questionarioParticipacao.DataLimiteParticipacao =
                    DateTime.Now.AddMinutes(questionarioAssociacao.Questionario.PrazoMinutos.Value);
            else
                questionarioParticipacao.DataLimiteParticipacao = DateTime.Now.AddMinutes(120);

            // Se for Prova ou Atividade Trilha, sorteia as questões.
            var lstIq = questionarioAssociacao.TipoQuestionarioAssociacao.ID ==
                        (int)enumTipoQuestionarioAssociacao.Prova ||
                        questionarioAssociacao.TipoQuestionarioAssociacao.ID ==
                        (int)enumTipoQuestionarioAssociacao.AtividadeTrilha
                ? SortearQuestoesProva(questionarioAssociacao)
                : questionarioAssociacao.Questionario.ListaItemQuestionario;

            var listaTempRespostaParticipacaoProfessor = new List<RespostaParticipacaoProfessor>();

            foreach (var itemQuestionario in lstIq.OrderBy(x => x.Ordem).ToList())
            {
                var iqp = new ItemQuestionarioParticipacao
                {
                    Auditoria = new Auditoria(cpfAuditoria),
                    Gabarito = itemQuestionario.NomeGabarito,
                    Questao = itemQuestionario.Questao,
                    QuestionarioParticipacao = questionarioParticipacao,
                    TipoItemQuestionario = itemQuestionario.TipoItemQuestionario,
                    EstiloItemQuestionario = itemQuestionario.EstiloItemQuestionario,
                    ValorQuestao = itemQuestionario.ValorQuestao,
                    ListaOpcoesParticipacao = new List<ItemQuestionarioParticipacaoOpcoes>(),
                    Feedback = itemQuestionario.Feedback,
                    Ordem = itemQuestionario.Ordem,
                    InAvaliaProfessor = itemQuestionario.InAvaliaProfessor,
                    ExibeFeedback = itemQuestionario.ExibeFeedback,
                    RespostaObrigatoria = itemQuestionario.RespostaObrigatoria != null ? itemQuestionario.RespostaObrigatoria : true
                };

                // Se for de colunas relacionadas, deve embaralhar as opções das colunas.
                var opcoes = itemQuestionario.TipoItemQuestionario.ID ==
                             (int)enumTipoItemQuestionario.ColunasRelacionadas
                    ?
                    // O OrderBy no final ordena os false primeiro, depois os true.
                    SortearOpcoesItemQuestionario(itemQuestionario).OrderBy(x => x.OpcaoVinculada != null).ToList()
                    : itemQuestionario.ListaItemQuestionarioOpcoes;

                foreach (var itemQuestionarioOpcao in opcoes)
                {
                    var iqop = new ItemQuestionarioParticipacaoOpcoes
                    {
                        Auditoria = new Auditoria(cpfAuditoria),
                        Descricao = itemQuestionarioOpcao.Descricao,
                        ItemQuestionarioParticipacao = iqp,
                        Nome = itemQuestionarioOpcao.Nome,
                        RespostaCorreta = itemQuestionarioOpcao.RespostaCorreta,
                        RespostaSelecionada = null,
                        Usuario = usuario,
                        TipoDiagnostico = itemQuestionarioOpcao.TipoDiagnostico,

                        // Se houver opção vinculada, ela já deverá estar na lista por causa do OrderBy acima.
                        OpcaoVinculada = itemQuestionarioOpcao.OpcaoVinculada != null
                            ? iqp.ListaOpcoesParticipacao.FirstOrDefault(
                                x => x.Nome == itemQuestionarioOpcao.OpcaoVinculada.Nome)
                            : null
                    };

                    iqp.ListaOpcoesParticipacao.Add(iqop);
                }

                if (iqp.InAvaliaProfessor && itemQuestionario.TipoItemQuestionario.ID != (int)enumTipoItemQuestionario.AgrupadorDeQuestoes)
                {
                    foreach (var professor in lsTurmaProfessor)
                    {
                        var rpp = new RespostaParticipacaoProfessor
                        {
                            Professor = professor,
                            QuestionarioParticipacao = questionarioParticipacao,
                            ItemQuestionarioParticipacao = iqp,
                            ListaRespostaParticipacaoOpcoes = new List<RespostaParticipacaoProfessorOpcoes>()
                        };

                        foreach (var item in iqp.ListaOpcoesParticipacao)
                        {
                            rpp.ListaRespostaParticipacaoOpcoes.Add(new RespostaParticipacaoProfessorOpcoes
                            {
                                ItemQuestionarioParticipacao = iqp,
                                ItemQuestionarioParticipacaoOpcoes = item,
                                RespostaParticipacaoProfessor = rpp
                            });
                        }

                        listaTempRespostaParticipacaoProfessor.Add(rpp);
                    }
                }

                questionarioParticipacao.ListaItemQuestionarioParticipacao.Add(iqp);
            }

            new BMQuestionarioParticipacao().Salvar(questionarioParticipacao);

            // Relacionar a participação com um ItemTrilhaParticipacao.
            VerificarItemTrilhaParticipacao(pQuestionario.IdItemTrilha, matricula, questionarioParticipacao,
                cpfAuditoria);

            var manterRespostaParticipacaoProfessor = new ManterRespostaParticipacaoProfessor();

            foreach (var item in listaTempRespostaParticipacaoProfessor)
            {
                manterRespostaParticipacaoProfessor.IncluirRespostaParticipacaoProfessor(item);

                foreach (var op in item.ListaRespostaParticipacaoOpcoes)
                {
                    new BMRespostaParticipacaoProfessorOpcoes().Salvar(op);
                }
            }
        }

        /// <summary>
        /// Criar participação "dummy" no ItemTrilha quando for tipo Atividade Trilha.
        /// </summary>
        /// <param name="itemTrilhaId"></param>
        /// <param name="matricula"></param>
        /// <param name="questionarioParticipacao"></param>
        /// <param name="cpfAuditoria"></param>
        private void VerificarItemTrilhaParticipacao(int? itemTrilhaId, UsuarioTrilha matricula,
            QuestionarioParticipacao questionarioParticipacao, string cpfAuditoria)
        {
            try
            {
                // Verifica se existe uma participação que não foi aprovada.
                // A aprovação será realizada quando o aluno responder o questionário e a nota for suficiente.
                if (questionarioParticipacao.Questionario.TipoQuestionario.ID ==
                    (int)enumTipoQuestionario.AtividadeTrilha &&
                    itemTrilhaId.HasValue)
                {
                    var itemTrilha = new ManterItemTrilha().ObterItemTrilhaPorID(itemTrilhaId.Value);

                    if (itemTrilha == null)
                        throw new ResponseException(enumResponseStatusCode.RegistroNaoEncontrado,
                            "Solução Sebrae não encontrada.");

                    if (matricula == null)
                        throw new ResponseException(enumResponseStatusCode.RegistroNaoEncontrado,
                            "Matrícula não encontrada.");

                    if (!itemTrilha.ListaItemTrilhaParticipacao.ToList().Any(
                        x =>
                            x.UsuarioTrilha.ID == matricula.ID && x.Autorizado == null &&
                            x.TipoParticipacao == enumTipoParticipacaoTrilha.AtividadeTrilha))
                    {
                        // Criar uma participação que servirá de "Dummy" para relacionar o aluno com o questionário participação.
                        var itemTrilhaParticipacao = new ItemTrilhaParticipacao
                        {
                            UsuarioTrilha = matricula,
                            ItemTrilha = itemTrilha,
                            DataEnvio = DateTime.Now,
                            TipoParticipacao = enumTipoParticipacaoTrilha.AtividadeTrilha,
                            QuestionarioParticipacao = questionarioParticipacao,
                            Auditoria = new Auditoria(cpfAuditoria),
                            FileServer = null
                        };

                        // Criar participação.
                        new BP.ManterItemTrilhaParticipacao().Salvar(itemTrilhaParticipacao);
                    }
                }
            }
            catch (Exception)
            {
                var bmQuestionarioParticipacao = new BMQuestionarioParticipacao();

                // Remove o questionário recém-criado e retorna o erro.
                bmQuestionarioParticipacao.Remover(bmQuestionarioParticipacao.ObterPorID(questionarioParticipacao.ID));

                throw new ResponseException(enumResponseStatusCode.RegistroNaoEncontrado,
                    "Não foi possível relacionar a atividade com o aluno. Tente novamente.");
            }
        }

        public void CadastrarQuestionarioEvolutivo(DTOCadastroQuestionarioParticipacao pQuestionario,
            TipoQuestionarioAssociacao tipoQuestionario, AuthenticationRequest autenticacao)
        {
            var questionarioAssociacao =
                (new BMQuestionario()).ObterPorQuestionarioAssociacao(new QuestionarioAssociacao()
                {
                    TrilhaNivel = new BMTrilhaNivel().ObterPorID(pQuestionario.IdTrilhaNivel),
                    Turma = new BMTurma().ObterPorID(pQuestionario.IdTurma),
                    TurmaCapacitacao = new BMTurmaCapacitacao().ObterPorId(pQuestionario.IdTurmaCapacitacao),
                    TipoQuestionarioAssociacao = tipoQuestionario,
                    Auditoria = new Auditoria(autenticacao.Login),
                    Evolutivo = true
                }, pQuestionario.IdUsuario);


            var us = new BMUsuario().ObterPorId(pQuestionario.IdUsuario);

            if (questionarioAssociacao != null)
            {
                var qp = ObterObjetoQuestionarioParticipacao(autenticacao, questionarioAssociacao, us);

                var enunciadoDinamico = TemplateUtil.ObterInformacoes(enumTemplate.EnunciadoQuestionarioDinamico);

                if (enunciadoDinamico != null)
                    qp.TextoEnunciadoPre = enunciadoDinamico.TextoTemplate;

                var bmViewTrilha = new BM.Views.BMViewTrilha();

                var listaViewTrilha = bmViewTrilha.ObterObjetivosDaTrilha(questionarioAssociacao.TrilhaNivel);

                foreach (var viewTrilha in listaViewTrilha)
                {
                    var itemQuestionarioParticipacao = ObterObjetoItemQuestionarioParticipacao(autenticacao, qp,
                        viewTrilha);

                    ObterQuestoesDiagnostico(itemQuestionarioParticipacao, autenticacao.Login);

                    qp.ListaItemQuestionarioParticipacao.Add(itemQuestionarioParticipacao);
                }

                var bmQuestionarioParticipacao = new BMQuestionarioParticipacao();

                bmQuestionarioParticipacao.Salvar(qp);

            }
            else
            {
                throw new AcademicoException("Não existem questionarios associados");
            }
        }

        private ItemQuestionarioParticipacao ObterObjetoItemQuestionarioParticipacao(AuthenticationRequest autenticacao,
            QuestionarioParticipacao qp, ViewTrilha viewTrilha)
        {
            var itemQuestionarioParticipacao = new ItemQuestionarioParticipacao()
            {
                Auditoria = new Auditoria(autenticacao.Login),
                Gabarito = null,
                Questao = viewTrilha.TopicoTematico.NomeExibicao,
                QuestionarioParticipacao = qp,
                TipoItemQuestionario = enumTipoItemQuestionario.Diagnostico,
                EstiloItemQuestionario = enumEstiloItemQuestionario.CaixaSelecao,
                ValorQuestao = null,
                Feedback = viewTrilha.Objetivo,
                ListaOpcoesParticipacao = new List<ItemQuestionarioParticipacaoOpcoes>(),
                InAvaliaProfessor = false,
                RespostaObrigatoria = true
            };
            return itemQuestionarioParticipacao;
        }

        private void ObterQuestoesDiagnostico(ItemQuestionarioParticipacao itemQuestionarioParticipacao, string login)
        {
            ItemQuestionarioParticipacaoOpcoes respostaImportancia = null;
            ItemQuestionarioParticipacaoOpcoes respostaDominio = null;

            for (byte i = 0; i <= 11; i++)
            {
                respostaImportancia = new ItemQuestionarioParticipacaoOpcoes();
                respostaImportancia.ItemQuestionarioParticipacao = itemQuestionarioParticipacao;

                respostaDominio = new ItemQuestionarioParticipacaoOpcoes();
                respostaDominio.ItemQuestionarioParticipacao = itemQuestionarioParticipacao;

                respostaImportancia.Auditoria = new Auditoria(login);
                respostaDominio.Auditoria = new Auditoria(login);

                respostaImportancia.OpcaoInt = i;
                respostaDominio.OpcaoInt = i;

                respostaImportancia.Nome = (i == 11 ? "NA" : i.ToString());
                respostaImportancia.TipoDiagnostico = enumTipoDiagnostico.Importancia;

                respostaDominio.Nome = (i == 11 ? "NA" : i.ToString());
                respostaDominio.TipoDiagnostico = enumTipoDiagnostico.Dominio;

                itemQuestionarioParticipacao.ListaOpcoesParticipacao.Add(respostaImportancia);
                itemQuestionarioParticipacao.ListaOpcoesParticipacao.Add(respostaDominio);
            }
        }

        private static QuestionarioParticipacao ObterObjetoQuestionarioParticipacao(AuthenticationRequest autenticacao,
            QuestionarioAssociacao questionarioAssociacao, Usuario us)
        {
            QuestionarioParticipacao qp = new QuestionarioParticipacao()
            {
                DataGeracao = DateTime.Now,
                NivelOcupacional = us.NivelOcupacional,
                Questionario = questionarioAssociacao.Questionario,
                TrilhaNivel = questionarioAssociacao.TrilhaNivel,
                Turma = questionarioAssociacao.Turma,
                TurmaCapacitacao = questionarioAssociacao.TurmaCapacitacao,
                Usuario = us,
                Uf = us.UF,
                Auditoria = new Auditoria(autenticacao.Login),
                Evolutivo = true,
                TipoQuestionarioAssociacao = questionarioAssociacao.TipoQuestionarioAssociacao,
                ListaItemQuestionarioParticipacao = new List<ItemQuestionarioParticipacao>(),
                TextoEnunciadoPre = questionarioAssociacao.Questionario.TextoEnunciado,
            };
            return qp;
        }

        /// <summary>
        /// Sorteia as questões para geração da prova.
        /// </summary>
        /// <param name="questionarioAssociacao"></param>
        /// <returns></returns>
        private IList<ItemQuestionario> SortearQuestoesProva(QuestionarioAssociacao questionarioAssociacao)
        {
            var lstResult = questionarioAssociacao.Questionario.ListaItemQuestionario;
            var qtdQuestao = (questionarioAssociacao.Questionario.QtdQuestoesProva.HasValue
                ? questionarioAssociacao.Questionario.QtdQuestoesProva.Value
                : 0);

            var rnd = new Random();
            var query =
                from i in lstResult
                let r = rnd.Next(0, lstResult.Count)
                orderby r
                select i;

            var shuffled = query.ToList();

            if (qtdQuestao > 0)
            {
                return shuffled.Take(qtdQuestao).ToList();
            }

            return shuffled;
        }

        private IList<ItemQuestionarioOpcoes> SortearOpcoesItemQuestionario(
            ItemQuestionario itemQuestionario)
        {
            var lstResult = itemQuestionario.ListaItemQuestionarioOpcoes;

            var rnd = new Random();

            // O macete é o lstResult.Count(), que vai se atualizando on-the-go.
            var query =
                from i in lstResult
                let r = rnd.Next(0, lstResult.Count())
                orderby r
                select i;

            return query.ToList();
        }

        public DTOQuestionarioParticipacao ListarQuestionarioParticipacao(
            DTOCadastroQuestionarioParticipacao pQuestionario, string cpfAuditoria, bool novasTrilhas = false,
            UsuarioTrilha matricula = null)
        {
            if (pQuestionario.Evolutivo)
                throw new AcademicoException("Esse método não pode ser utilizado para questionário evolutivo!");

            var tipoQuestionarioAssociacao = ObterTipoQuestionarioAssociacao(pQuestionario);

            TurmaCapacitacao turmaCapacitacao = null;
            if (pQuestionario.IdTurmaCapacitacao > 0)
                turmaCapacitacao = (new ManterTurmaCapacitacao()).ObterPorId(pQuestionario.IdTurmaCapacitacao);

            TrilhaNivel trilhaNivel = null;
            if (pQuestionario.IdTrilhaNivel > 0)
                trilhaNivel = new BMTrilhaNivel().ObterPorID(pQuestionario.IdTrilhaNivel);

            Turma turma = null;

            IList<DTOProfessor> lsTurmaProfessor = null;

            if (pQuestionario.IdTurma > 0)
            {
                turma = new BMTurma().ObterPorID(pQuestionario.IdTurma);

                lsTurmaProfessor = new BMTurmaProfessor()
                    .ObterPorFiltro(new TurmaProfessor { Turma = turma })
                    .Select(p => new DTOProfessor
                    {
                        ID = p.Professor.ID,
                        Nome = p.Professor.Nome
                    }).ToList();
            }

            if (turma == null && trilhaNivel == null && turmaCapacitacao == null)
                throw new AcademicoException("Turma ou trilha não encontrado");

            var lstQuestionarioParticipacao = pQuestionario.IdItemTrilha == null
                ? (
                    new BMQuestionario()).ObterQuestionarioParticipacaoPorFiltro(
                        new QuestionarioParticipacao
                        {
                            Usuario = (new BMUsuario()).ObterPorId(pQuestionario.IdUsuario),
                            TrilhaNivel = trilhaNivel,
                            Turma = turma,
                            TurmaCapacitacao = turmaCapacitacao,
                            Evolutivo = false,                            
                            TipoQuestionarioAssociacao = tipoQuestionarioAssociacao,
                        }).AsQueryable()
                : new BMQuestionarioParticipacao().ObterDisponiveisPorItemTrilhaUsuario(
                    pQuestionario.IdItemTrilha.Value, pQuestionario.IdUsuario);

            var lstQuestionarioParticipacaoProva =
                lstQuestionarioParticipacao.Where(
                    x =>
                        x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Prova &&
                        x.DataParticipacao == null).AsQueryable();

            var participacoesExcetoProvas =
                lstQuestionarioParticipacao.Where(
                    x => x.TipoQuestionarioAssociacao.ID != (int)enumTipoQuestionarioAssociacao.Prova
                         && (!x.ListaItemTrilhaParticipacao.Any()
                             || x.ListaItemTrilhaParticipacao.Any(itp =>
                                 itp.Autorizado != true && (itp.ItemTrilha != null ? itp.ItemTrilha.ID == (pQuestionario.IdItemTrilha.HasValue ? pQuestionario.IdItemTrilha : 0) : true)))
                    );

            DTOQuestionarioParticipacao resultado = null;

            if (tipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Prova)
            {
                ValidarInformacoesProva(pQuestionario, novasTrilhas);

                if (!lstQuestionarioParticipacaoProva.Any(x => x.DataLimiteParticipacao >= DateTime.Now))
                {
                    resultado = CadastrarProva(pQuestionario, tipoQuestionarioAssociacao, cpfAuditoria);
                }
                else
                {
                    resultado =
                        GetListaDto(
                            lstQuestionarioParticipacaoProva.Where(x => x.DataLimiteParticipacao >= DateTime.Now)
                                .ToList()).FirstOrDefault();
                }
            }
            else
            {
                if (!participacoesExcetoProvas.Any())
                {
                    // Me diga.. Você sangra? Com o método abaixo, vai sangrar.
                    CadastrarQuestionarioParticipacao(pQuestionario, tipoQuestionarioAssociacao, cpfAuditoria, matricula);

                    resultado = GetListaDto(participacoesExcetoProvas.ToList()).FirstOrDefault();                    
                }
                else
                    resultado = GetListaDto(participacoesExcetoProvas.ToList()).FirstOrDefault();
            }

            if (resultado != null && lsTurmaProfessor != null)
                resultado.ListaProfessor = lsTurmaProfessor.ToList();

            if (resultado != null && pQuestionario.IdTurma > 0)
                resultado.IdTurma = pQuestionario.IdTurma;

            return resultado;
        }

        private static TipoQuestionarioAssociacao ObterTipoQuestionarioAssociacao(
            DTOCadastroQuestionarioParticipacao pQuestionario)
        {
            TipoQuestionarioAssociacao tipoQuestionarioAssociacao;
            if (pQuestionario.Pre)
                tipoQuestionarioAssociacao = new TipoQuestionarioAssociacao
                {
                    ID = (int)enumTipoQuestionarioAssociacao.Pre
                };

            else if (pQuestionario.Pos)
                tipoQuestionarioAssociacao = new TipoQuestionarioAssociacao
                {
                    ID = (int)enumTipoQuestionarioAssociacao.Pos
                };

            else if (pQuestionario.Cancelamento)
                tipoQuestionarioAssociacao = new TipoQuestionarioAssociacao
                {
                    ID = (int)enumTipoQuestionarioAssociacao.Cancelamento
                };

            else if (pQuestionario.Abandono)
                tipoQuestionarioAssociacao = new TipoQuestionarioAssociacao
                {
                    ID = (int)enumTipoQuestionarioAssociacao.Abandono
                };

            else if (pQuestionario.AtividadeTriha)
                tipoQuestionarioAssociacao = new TipoQuestionarioAssociacao
                {
                    ID = (int)enumTipoQuestionarioAssociacao.AtividadeTrilha
                };

            else if (pQuestionario.Eficacia)
                tipoQuestionarioAssociacao = new TipoQuestionarioAssociacao
                {
                    ID = (int)enumTipoQuestionarioAssociacao.Eficacia
                };
            else
                tipoQuestionarioAssociacao = new TipoQuestionarioAssociacao
                {
                    ID = (int)enumTipoQuestionarioAssociacao.Prova
                };
            return tipoQuestionarioAssociacao;
        }

        public DTOQuestionarioParticipacao ListarQuestionarioAvulsoDemandasParticipacao(int idQuestionario,
            int idUsuario, int tipo, AuthenticationRequest autenticacao, bool edicao = true)
        {
            var tipoQuestionarioAssociacao = new TipoQuestionarioAssociacao() { ID = tipo };
            Questionario questionario = new BMQuestionario().ObterPorId(idQuestionario);

            if (questionario == null)
                throw new AcademicoException("Questionário não encontrado ");

            var lstQuestionarioParticipacao = (
                new BMQuestionario()).ObterQuestionarioParticipacaoPorFiltro(
                    new QuestionarioParticipacao
                    {
                        Usuario = (new BMUsuario()).ObterPorId(idUsuario),
                        TrilhaNivel = null,
                        Turma = null,
                        Evolutivo = false,
                        TipoQuestionarioAssociacao = tipoQuestionarioAssociacao,
                        Questionario = questionario
                    });

            var questionarioParticipacao =
                lstQuestionarioParticipacao.Where(x => x.TipoQuestionarioAssociacao.ID == tipo);

            if ((enumTipoQuestionario)tipo == enumTipoQuestionario.Avulso)
                ValidarParticipacaoQuestionarioAvulso(questionarioParticipacao.ToList());

            if (!questionarioParticipacao.Any() || !edicao)
            {
                CadastrarQuestionarioAvulsoParticipacao(idUsuario, questionario, tipoQuestionarioAssociacao,
                    autenticacao);
                return ListarQuestionarioAvulsoDemandasParticipacao(idQuestionario, idUsuario, tipo, autenticacao);
            }

            return GetListaDto(questionarioParticipacao.ToList()).LastOrDefault();
        }

        public DTOQuestionarioParticipacao ListarRespostasQuestionarioParticipacao(int idQuestionarioParticipacao, AuthenticationRequest autenticacao)
        {
            var participacao = new ManterQuestionarioParticipacao().ObterPorId(idQuestionarioParticipacao);
            var listQuestionarioParticipacao = new List<QuestionarioParticipacao>();
            listQuestionarioParticipacao.Add(participacao);
            return GetListaDto(listQuestionarioParticipacao.ToList()).FirstOrDefault();
        }

        public void ValidarParticipacaoQuestionarioAvulso(IList<QuestionarioParticipacao> questionarios)
        {
            // Caso já tenha participado retorna erro para quesitonários do tipo avulso
            if (questionarios.Any() && questionarios.FirstOrDefault().DataParticipacao != null)
            {
                throw new AcademicoException("Você já realizou esse questionário.");
            }
        }

        public DTOQuestionarioParticipacao GerarQuestionarioEvolutivo(DTOCadastroQuestionarioParticipacao pQuestionario,
            AuthenticationRequest autenticacao)
        {
            if (!pQuestionario.Evolutivo)
                throw new AcademicoException("Esse método não pode ser utilizado para questionário não evolutivo!");

            TipoQuestionarioAssociacao tipoQuestionarioAssociacao;

            if (pQuestionario.Pre)
                tipoQuestionarioAssociacao = new TipoQuestionarioAssociacao()
                {
                    ID = (int)enumTipoQuestionarioAssociacao.Pre
                };
            else
                tipoQuestionarioAssociacao = new TipoQuestionarioAssociacao()
                {
                    ID = (int)enumTipoQuestionarioAssociacao.Pos
                };

            try
            {
                IList<QuestionarioParticipacao> lstQuestionarioParticipacao = (
                    new BMQuestionario()).ObterQuestionarioParticipacaoPorFiltro(
                        new QuestionarioParticipacao()
                        {
                            Usuario = (new BMUsuario()).ObterPorId(pQuestionario.IdUsuario),
                            TrilhaNivel = new BMTrilhaNivel().ObterPorID(pQuestionario.IdTrilhaNivel),
                            TipoQuestionarioAssociacao = tipoQuestionarioAssociacao,
                            Evolutivo = true
                        });


                IEnumerable<QuestionarioParticipacao> lstQuestionarioParticipacaoPrePos =
                    lstQuestionarioParticipacao.Where(
                        x => x.TipoQuestionarioAssociacao.ID != (int)enumTipoQuestionarioAssociacao.Prova);


                if (lstQuestionarioParticipacaoPrePos.Count() == 0)
                {
                    CadastrarQuestionarioEvolutivo(pQuestionario, tipoQuestionarioAssociacao, autenticacao);
                    return GerarQuestionarioEvolutivo(pQuestionario, autenticacao);
                }
                else
                    return GetListaDto(lstQuestionarioParticipacaoPrePos.ToList()).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
                return null;
            }

        }


        private DTOQuestionarioParticipacao CadastrarProva(DTOCadastroQuestionarioParticipacao pQuestionario,
            TipoQuestionarioAssociacao tipoQuestionarioAssociacao, string cpfAuditoria)
        {
            // Me diga.. Você sangra? Com o método abaixo, vai sangrar.
            CadastrarQuestionarioParticipacao(pQuestionario, tipoQuestionarioAssociacao, cpfAuditoria);

            var lstQuestionarioParticipacao =
                (new BMQuestionario()).ObterQuestionarioParticipacaoPorFiltro(new QuestionarioParticipacao()
                {
                    Usuario = (new BMUsuario()).ObterPorId(pQuestionario.IdUsuario),
                    TrilhaNivel =
                        pQuestionario.IdTrilhaNivel > 0
                            ? new BMTrilhaNivel().ObterPorID(pQuestionario.IdTrilhaNivel)
                            : null,
                    Turma = pQuestionario.IdTurma > 0 ? new BMTurma().ObterPorID(pQuestionario.IdTurma) : null,
                    TipoQuestionarioAssociacao =
                        new TipoQuestionarioAssociacao() { ID = (int)enumTipoQuestionarioAssociacao.Prova }
                });

            return
                GetListaDto(
                    lstQuestionarioParticipacao.Where(
                        x => x.DataParticipacao == null && x.DataLimiteParticipacao >= DateTime.Now).ToList())
                    .FirstOrDefault();
        }


        private void ValidarInformacoesProva(DTOCadastroQuestionarioParticipacao pQuestionario,
            bool novasTrilhas = false)
        {
            var usuario = new BMUsuario().ObterPorId(pQuestionario.IdUsuario);

            if (pQuestionario.IdTrilhaNivel > 0)
            {
                var tn = new BMTrilhaNivel().ObterPorID(pQuestionario.IdTrilhaNivel);

                var count = new BMQuestionarioParticipacao()
                    .ObterPorUsuarioTrilhaNivel(usuario, tn)
                    .Count(x => x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Prova
                                && !(x.DataParticipacao == null && x.DataLimiteParticipacao > DateTime.Now));

                if (count >= 1)
                {
                    var usuarioTrilha = new BMUsuarioTrilha()
                        .ObterPorFiltro(new UsuarioTrilha
                        {
                            Usuario = usuario,
                            TrilhaNivel = tn,
                            NovasTrilhas = novasTrilhas
                        })
                        .FirstOrDefault(
                            x =>
                                x.StatusMatricula == enumStatusMatricula.Inscrito ||
                                x.StatusMatricula == enumStatusMatricula.Reprovado ||
                                x.StatusMatricula == enumStatusMatricula.Aprovado);

                    if (usuarioTrilha == null)
                        throw new ResponseException(enumResponseStatusCode.RegistroNaoEncontrado,
                            "Usuário não matriculado na Trilha");

                    if (usuarioTrilha.DataLimite.Date < DateTime.Now.Date)
                        throw new ResponseException(enumResponseStatusCode.ErroRegraNegocioTrilhas,
                            "Data atual é superio à Data de Limite do usuário na trilha selecionada.");

                    //if (!(usuarioTrilha.NovaProvaLiberada && usuarioTrilha.DataLiberacaoNovaProva.HasValue && usuarioTrilha.DataLiberacaoNovaProva.Value.Date <= DateTime.Today))
                    //   throw new ResponseException(enumResponseStatusCode.ErroRegraNegocioTrilhas, "A data para liberação de realização de nova prova é " + usuarioTrilha.DataLiberacaoNovaProva.Value + ". Aproveite para estudar mais até lá");
                }
            }
            else if (pQuestionario.IdTurma > 0)
            {
                var turma = new BMTurma().ObterPorID(pQuestionario.IdTurma);

                var count = new BMQuestionarioParticipacao()
                    .ObterPorUsuarioTurma(usuario, turma)
                    .Count(x => x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Prova
                                && !(x.DataParticipacao == null && x.DataLimiteParticipacao > DateTime.Now));

                if (count >= 1)
                {
                    var matriculaOferta = new BMMatriculaOferta()
                        .ObterPorFiltro(new MatriculaOferta() { Usuario = usuario, Oferta = turma.Oferta })
                        .FirstOrDefault(
                            x =>
                                x.StatusMatricula == enumStatusMatricula.Inscrito ||
                                x.StatusMatricula == enumStatusMatricula.Reprovado ||
                                x.StatusMatricula == enumStatusMatricula.Aprovado);

                    if (matriculaOferta == null)
                    {
                        throw new AcademicoException(
                            "Não foi possível completar esta operação. Usuário não inscrito na turma informada");
                    }

                }
            }
            else
            {
                throw new AcademicoException("Não foi possível processar a solicitação");
            }
        }

        public string GetTextPlusTags(QuestionarioParticipacao qp, string texto)
        {
            if (texto == null)
                return null;

            if (qp.TrilhaNivel != null && qp.TrilhaNivel.Trilha != null)
            {
                texto = texto.Replace("#TRILHANIVEL", qp.TrilhaNivel.Nome)
                                .Replace("#TRILHA", qp.TrilhaNivel.Trilha.Nome);

            }

            if (qp.Questionario.ListaQuestionarioAssociacao.Any(x => x.Turma != null))
            {

                var idsTurmas = qp.Questionario.ListaQuestionarioAssociacao.Where(y => y.Turma != null).Select(y => y.Turma.ID).ToList();

                var manterMatriculaTurma = new ManterMatriculaTurma();

                try
                {
                    var ultimaMatricula = manterMatriculaTurma.ObterTodosIQueryable()
                        .Join(new ManterMatriculaOferta().ObterTodosIQueryable(), mt => mt.MatriculaOferta.ID, mo => mo.ID,
                            (mt, mo) => new
                            {
                                mt,
                                mo
                            })
                        .Join(new ManterOferta().ObterTodasIQueryable(), join => join.mo.Oferta.ID, o => o.ID,
                        (join, o) => new
                        {
                            join.mo,
                            join.mt,
                            o
                        })
                        .Where(x => x.mo.Usuario.ID == qp.Usuario.ID)
                        .Select(x => new MatriculaTurma
                        {
                            Turma = new Turma
                            {
                                ID = x.mt.Turma.ID,
                                Nome = x.mt.Turma.Nome,
                                Oferta = new Oferta
                                {
                                    Nome = x.o.Nome,
                                    SolucaoEducacional = new SolucaoEducacional
                                    {
                                        Nome = x.o.SolucaoEducacional.Nome
                                    }
                                }
                            },
                            MatriculaOferta = new MatriculaOferta
                            {
                                DataSolicitacao = x.mo.DataSolicitacao,
                                DataGeracaoCertificado = x.mo.DataGeracaoCertificado
                            }
                        })
                        .ToList()
                        .FirstOrDefault(x => idsTurmas.Contains(x.Turma.ID));

                    if (ultimaMatricula != null && ultimaMatricula.ID != 0)
                    {
                        texto = texto.Replace("#SOLUCAO", ultimaMatricula.Turma.Oferta.SolucaoEducacional.Nome)
                                .Replace("#OFERTA", ultimaMatricula.Turma.Oferta.Nome)
                                .Replace("#TURMA", ultimaMatricula.Turma.Nome)
                                .Replace("#DATAMATRICULA", ultimaMatricula.MatriculaOferta.DataSolicitacao.ToString("dd/MM/yyyy"));

                        if (ultimaMatricula.MatriculaOferta.DataGeracaoCertificado.HasValue)
                        {
                            texto = texto.Replace("#DATATERMINO", ultimaMatricula.MatriculaOferta.DataGeracaoCertificado.Value.ToString("dd/MM/yyyy"));
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            if (qp.Questionario.ListaQuestionarioAssociacao.Any(x => x.TrilhaNivel != null))
            {
                var QuestNivelTrilhas = qp.Questionario.ListaQuestionarioAssociacao.Where(y => y.TrilhaNivel != null).Select(y => y.TrilhaNivel);

                var manterUsuarioTrilha = new ManterUsuarioTrilha();

                UsuarioTrilha ultimaMatriculaTrilha = manterUsuarioTrilha.
                    ObterTodosUsuarioTrilha()
                    .Where(x => x.Usuario == qp.Usuario && QuestNivelTrilhas.Contains(x.TrilhaNivel) && x.NovasTrilhas == true)
                    .OrderByDescending(usuario => usuario.ID)
                    .FirstOrDefault();

                if (ultimaMatriculaTrilha != null && ultimaMatriculaTrilha.ID != 0)
                {
                    texto = texto.Replace("#TRILHANIVEL", ultimaMatriculaTrilha.TrilhaNivel.Nome)
                                    .Replace("#TRILHA", ultimaMatriculaTrilha.TrilhaNivel.Trilha.Nome)
                                    .Replace("#DATAINICIOTRILHA", ultimaMatriculaTrilha.DataInicioFormatada)
                                    .Replace("#DATAFIMTRILHA", ultimaMatriculaTrilha.DataLimiteFormatada);
                }
            }

            return texto.Replace("#NOME_ALUNO", qp.Usuario.Nome)
                        .Replace("#DATADEHOJE", DateTime.Now.ToString("dd/MM/yyyy"));
        }

        public string GetTextPlusTags(UsuarioTrilha matriculaUsuarioTrilha, string texto)
        {
            if (texto == null)
                return null;

            if (matriculaUsuarioTrilha != null)
            {
                texto = texto.Replace("#TRILHA", matriculaUsuarioTrilha.TrilhaNivel.Trilha.Nome)
                                        .Replace("#TRILHANIVEL", matriculaUsuarioTrilha.TrilhaNivel.Nome)
                                        .Replace("#DATAINICIOTRILHA", matriculaUsuarioTrilha.DataInicioFormatada)
                                        .Replace("#DATAFIMTRILHA", matriculaUsuarioTrilha.DataLimiteFormatada);
            }

            return texto.Replace("#NOME_ALUNO", matriculaUsuarioTrilha.Usuario.Nome)
                        .Replace("#DATADEHOJE", DateTime.Now.ToString("dd/MM/yyyy"));
        }

        public IList<DTOQuestionarioParticipacao> GetListaDto(IList<QuestionarioParticipacao> lstOrigem)
        {
            var lstResult = new List<DTOQuestionarioParticipacao>();

            foreach (var qp in lstOrigem)
            {
                var qpDto = new DTOQuestionarioParticipacao()
                {
                    DataGeracao = qp.DataGeracao,
                    DataParticipacao = qp.DataParticipacao,
                    ID = qp.ID,
                    TipoQuestionarioAssociacao = new DTOTipoQuestionarioAssociacao(),
                    DataLimiteParticipacao = qp.DataLimiteParticipacao,
                    TextoEnunciadoPre = this.GetTextPlusTags(qp, qp.TextoEnunciadoPre),
                    TextoEnunciadoPos = this.GetTextPlusTags(qp, qp.TextoEnunciadoPos),
                    ListaItemQuestionarioParticipacao = new List<DTOItemQuestionarioParticipacao>(),
                    Questionario = new DTO.Services.Questionario.DTOQuestionario(),
                    TipoQuestionario = qp.Questionario.TipoQuestionario.ID
                };

                CommonHelper.SincronizarDominioParaDTO(qp.TipoQuestionarioAssociacao, qpDto.TipoQuestionarioAssociacao);

                qpDto.Questionario = new DTO.Services.Questionario.DTOQuestionario
                {
                    ID = qp.Questionario.ID,
                    NomeQuestionario = qp.Questionario.Nome,
                };

                // Só exibr o feedback se o questionário não for Prova nem Atividade Trilha.
                var exibirFeedback = qp.Questionario.TipoQuestionario.ID != (int)enumTipoQuestionario.AtividadeTrilha &&
                                     qp.Questionario.TipoQuestionario.ID != (int)enumTipoQuestionario.AvaliacaoProva;

                foreach (var iq in qp.ListaItemQuestionarioParticipacao.OrderBy(x => x.Ordem).ToList())
                {
                    var iqDto = new DTOItemQuestionarioParticipacao
                    {
                        ID = iq.ID,
                        Gabarito = iq.Gabarito,
                        Questao = iq.Questao,
                        ValorQuestao = iq.ValorQuestao,
                        Resposta = new List<string>(),
                        ValorAribuido = iq.ValorAtribuido,
                        Feedback = iq.ExibeFeedback == true && exibirFeedback ? iq.Feedback : null,
                        InAvaliaProfessor = iq.InAvaliaProfessor,
                        TipoItemQuestionario = new DTOTipoItemQuestionario(),
                        EstiloItemQuestionario = new DTOEstiloItemQuestionario(),
                        ListaOpcoes = new List<DTOItemQuestionarioParticipacaoOpcoes>(),
                        ListaRespostaProfessor = new List<DTORespostaProfessor>(),
                        RespostaObrigatoria = iq.RespostaObrigatoria
                    };

                    if (iq.Resposta != null) iqDto.Resposta.Add(iq.Resposta);

                    CommonHelper.SincronizarDominioParaDTO(iq.TipoItemQuestionario, iqDto.TipoItemQuestionario);
                    if (iq.EstiloItemQuestionario == null)
                        iq.EstiloItemQuestionario = new EstiloItemQuestionario
                        {
                            ID = (int)enumEstiloItemQuestionario.CaixaSelecao
                        };
                    CommonHelper.SincronizarDominioParaDTO(iq.EstiloItemQuestionario, iqDto.EstiloItemQuestionario);

                    foreach (var iop in iq.ListaOpcoesParticipacao.OrderBy(x => x.OpcaoInt).ToList())
                    {

                        var iopDto = new DTOItemQuestionarioParticipacaoOpcoes
                        {
                            ID = iop.ID,
                            Nome = iop.Nome,
                            RespostaCorreta = iop.RespostaCorreta,
                            RespostaSelecionada = new List<bool?>(),
                            OpcaoVinculada = iop.OpcaoVinculada != null ? new DTOItemQuestionarioParticipacaoOpcoes() {
                                RespostaSelecionada = new List<bool?>() { iop.OpcaoVinculada.RespostaSelecionada },
                                RespostaCorreta = iop.OpcaoVinculada.RespostaCorreta,
                                PossuiOpcaoVinculada = iop.OpcaoVinculada.OpcaoVinculada != null,
                                IndexOpcaoSelecionada = iq.ListaOpcoesParticipacao.IndexOf(iop.OpcaoSelecionada)
                            } : null,
                            PossuiOpcaoVinculada = iop.OpcaoVinculada != null
                        };

                        iopDto.RespostaSelecionada.Add(iop.RespostaSelecionada);

                        iqDto.ListaOpcoes.Add(iopDto);
                    }

                    if (iq.InAvaliaProfessor && iq.TipoItemQuestionario.ID != (int)enumTipoItemQuestionario.AgrupadorDeQuestoes)
                    {
                            var manterRespostaProfessor = new ManterRespostaParticipacaoProfessor();
                            var manterRespostaProfessorOpcoes = new ManterRespostaParticipacaoProfessorOpcoes();

                            var respostasProfessor = manterRespostaProfessor.ObterPorItemQuestionarioParticipacao(iq)
                                .Select(x => new RespostaParticipacaoProfessor
                                {
                                    ID = x.ID,
                                    Professor = new Usuario
                                    {
                                        ID = x.Professor.ID
                                    }
                                })
                                .ToList();

                            foreach (var rpp in respostasProfessor)
                            {
                                var rppo = manterRespostaProfessorOpcoes.ObterPorRespostaProfessor(rpp)
                                    .Select(x => new RespostaParticipacaoProfessorOpcoes
                                    {
                                        ID = x.ID,
                                        ItemQuestionarioParticipacaoOpcoes = new ItemQuestionarioParticipacaoOpcoes
                                        {
                                            ID = x.ItemQuestionarioParticipacaoOpcoes.ID
                                        }
                                    }).ToList();

                                var rppDto = new DTORespostaProfessor
                                {
                                    ID = rpp.ID,
                                    ProfessorId = rpp.Professor.ID,
                                    Resposta = rpp.Resposta,
                                    ListaRespostaOpcoes =
                                        rppo.Select(
                                            x => new DTORespostaProfessorOpcao
                                            {
                                                ID = x.ID,
                                                OpcaoId = x.ItemQuestionarioParticipacaoOpcoes.ID
                                            }).ToList()
                                };

                                iqDto.ListaRespostaProfessor.Add(rppDto);
                            }
                    }

                    qpDto.ListaItemQuestionarioParticipacao.Add(iqDto);
                }

                lstResult.Add(qpDto);
            }

            return lstResult;
        }

        public void InformarRespostas(DTOQuestionarioParticipacao pQuestionario, string cpfAuditoria,
            out Tuple<bool, decimal, string> pFinalizacaoTrilhaNivel,
            ref DTOQuestionarioParticipacao questionarioParticipacaoResposta, bool novasTrilhas = false)
        {
            var questionarioParticipacao = new BMQuestionarioParticipacao().ObterPorID(pQuestionario.ID);

            questionarioParticipacao.Auditoria = new Auditoria(UsuarioLogado);
            pFinalizacaoTrilhaNivel = new Tuple<bool, decimal, string>(false, 0, "");

            if (questionarioParticipacao.DataParticipacao != null && questionarioParticipacao.Questionario.TipoQuestionario.ID != (int)enumTipoQuestionario.Demanda)
            {
                throw new AcademicoException(
                    string.Format("Não é possível executar esta operação. O Usuário já registrou sua participação.({0})", pQuestionario.ID.ToString()), 10);
            }

            var ultrapassouLimiteTempoProva = false;

            // Chamando novamente pois a sessão está sendo limpa após o salvamento, para evitar conflito.
            questionarioParticipacao = (new BMQuestionarioParticipacao()).ObterPorID(pQuestionario.ID);

            // Questionários do tipo pesquisa não devem ser bloqueados pela data limite de participação
            if (questionarioParticipacao.Questionario.TipoQuestionario.ID == (int)enumTipoQuestionario.AvaliacaoProva &&
                (questionarioParticipacao.DataLimiteParticipacao != null &&
                 DateTime.Now > questionarioParticipacao.DataLimiteParticipacao))
            {

                if (questionarioParticipacao.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Prova)
                {
                    ultrapassouLimiteTempoProva = true;
                }
                else
                    throw new AcademicoException("Não é possível executar esta operação. Tempo esgotado.");
            }

            // É isso aqui que define se o questionário foi respondido ou não.
            questionarioParticipacao.DataParticipacao = DateTime.Now;

            if (questionarioParticipacao.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Prova)
                questionarioParticipacao.ValorProva = 0;

            foreach (var iqpDto in pQuestionario.ListaItemQuestionarioParticipacao)
            {
                var itemQuestionarioParticipacao =
                    questionarioParticipacao.ListaItemQuestionarioParticipacao.FirstOrDefault(x => x.ID == iqpDto.ID);

                if (itemQuestionarioParticipacao == null)
                    continue;

                itemQuestionarioParticipacao.Auditoria = new Auditoria(cpfAuditoria);
                
                // Caso a questão avalie o professor, preenche cada resposta com a avaliação do professor.
                SalvarAvaliacaoProfessor(itemQuestionarioParticipacao, iqpDto);

                itemQuestionarioParticipacao.Resposta = iqpDto.Resposta.FirstOrDefault();
                // Abaixo só interessa salvar caso a questão não esteja avaliando o professor.
                if (itemQuestionarioParticipacao.TipoItemQuestionario.ID == (int)enumTipoItemQuestionario.Discursiva || 
                    itemQuestionarioParticipacao.InAvaliaProfessor)
                    continue;

                foreach (var iqpoDto in iqpDto.ListaOpcoes)
                {
                    var itemQuestionarioParticipacaoOpcoes =
                        itemQuestionarioParticipacao.ListaOpcoesParticipacao.FirstOrDefault(y => y.ID == iqpoDto.ID);

                    if (itemQuestionarioParticipacaoOpcoes == null)
                        continue;

                    switch ((enumTipoItemQuestionario) itemQuestionarioParticipacao.TipoItemQuestionario.ID)
                    {
                        case enumTipoItemQuestionario.Discursiva:
                        case enumTipoItemQuestionario.Objetiva:
                        case enumTipoItemQuestionario.MultiplaEscolha:
                        case enumTipoItemQuestionario.AgrupadorDeQuestoes:
                        case enumTipoItemQuestionario.Diagnostico:
                            itemQuestionarioParticipacaoOpcoes.RespostaSelecionada =
                                iqpoDto.RespostaSelecionada.Count > 0
                                    ? iqpoDto.RespostaSelecionada[0]
                                    : null;

                            itemQuestionarioParticipacaoOpcoes.Auditoria = new Auditoria(cpfAuditoria);

                            if (itemQuestionarioParticipacaoOpcoes.RespostaCorreta == true)
                            {
                                if (itemQuestionarioParticipacaoOpcoes.RespostaSelecionada ==
                                    itemQuestionarioParticipacaoOpcoes.RespostaCorreta)
                                {
                                    questionarioParticipacao.ValorProva +=
                                        itemQuestionarioParticipacao.ValorQuestao ?? 0;

                                    itemQuestionarioParticipacao.ValorAtribuido =
                                        ultrapassouLimiteTempoProva
                                            ? 0
                                            : itemQuestionarioParticipacao.ValorQuestao;
                                }
                            }

                            break;
                        case enumTipoItemQuestionario.VerdadeiroOuFalso:
                            itemQuestionarioParticipacaoOpcoes.RespostaSelecionada = true;
                            break;
                        case enumTipoItemQuestionario.ColunasRelacionadas:
                            // Caso a questão seja de coluna relacionada e haja um index de opção, busca qual opção o usuário
                            // vinculou na tela.
                            if (iqpoDto.IndexOpcaoSelecionada.HasValue)
                            {
                                var opcaoColuna1 =
                                    itemQuestionarioParticipacao.ListaOpcoesParticipacao.Where(
                                        x => x.OpcaoVinculada != null).ToList()[
                                            iqpoDto.IndexOpcaoSelecionada
                                                .Value];

                                if (opcaoColuna1 != null)
                                {
                                    opcaoColuna1.OpcaoSelecionada = itemQuestionarioParticipacaoOpcoes;
                                }
                            }

                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            new BMQuestionarioParticipacao().Salvar(questionarioParticipacao);

            try
            {
                // Caso seja questionário de cancelamento, cancelar a matrícula do aluno.
                if (questionarioParticipacao.TipoQuestionarioAssociacao.ID ==
                    (int)enumTipoQuestionarioAssociacao.Cancelamento)
                {
                    var mt =
                        new ManterMatriculaTurma().ObterMatriculaTurmaPorIdUsuarioIdTurma(
                            questionarioParticipacao.Usuario.ID, pQuestionario.IdTurma.Value);

                    if (mt != null)
                    {
                        mt.DataTermino = DateTime.Now;
                        var manterMt = new BMMatriculaTurma();
                        manterMt.Salvar(mt);

                        var manterMo = new BMMatriculaOferta();
                        var mo = manterMo.ObterPorID(mt.MatriculaOferta.ID);
                        mo.StatusMatricula = enumStatusMatricula.CanceladoAluno;
                        manterMo.Salvar(mo);
                    }
                }
            }
            catch (Exception)
            {
                throw new AcademicoException(
                    "O questionário foi respondido com sucesso, porém houve um erro ao tentar cancelar a matrícula automaticamente. Cancele a matrícula manualmente no menu Meus Cursos");
            }

            try
            {
                // Processar seja questionário de Atividade Trilha.
                if (questionarioParticipacao.TipoQuestionarioAssociacao.ID ==
                    (int)enumTipoQuestionarioAssociacao.AtividadeTrilha)
                {
                    var itemTrilhaParticipacao = questionarioParticipacao.ListaItemTrilhaParticipacao.FirstOrDefault();

                    if (itemTrilhaParticipacao != null)
                    {
                        var nota = questionarioParticipacao.ObterNota();

                        // Aprovar a participação do indivíduo.
                        var aprovado = questionarioParticipacao.IsAprovado(nota);

                        itemTrilhaParticipacao.Autorizado = aprovado;
                        itemTrilhaParticipacao.DataAvaliacao = questionarioParticipacao.DataParticipacao;

                        new BP.ManterItemTrilhaParticipacao().Salvar(itemTrilhaParticipacao);

                        pFinalizacaoTrilhaNivel =
                            new Tuple<bool, decimal, string>(aprovado, nota, string.Empty);

                        new ManterTrilhaTopicoTematicoParticipacao().IncluirUltimaParticipacao(
                            itemTrilhaParticipacao.UsuarioTrilha, itemTrilhaParticipacao.ItemTrilha);
                    }
                }
            }
            catch (Exception)
            {
                throw new AcademicoException(
                    "O questionário foi respondido com sucesso, porém houve um erro ao tentar registrar a aprovação na Atividade Trilha.");
            }

            try
            {
                // Caso seja questionário de abandono, retorna o IdTurma do próximo questionário de abandono pendente, caso exista.
                if (questionarioParticipacao.TipoQuestionarioAssociacao.ID ==
                    (int)enumTipoQuestionarioAssociacao.Abandono)
                {
                    var turmasPendentes = new ManterTurma().ObterTurmasPendentes(questionarioParticipacao.Usuario);

                    if (turmasPendentes.Any())
                        questionarioParticipacaoResposta.IdTurma = turmasPendentes.FirstOrDefault().ID;
                }

                if (questionarioParticipacao.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pos ||
                    questionarioParticipacao.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Eficacia)
                {
                    var questionariosEficaciaPos = new BMQuestionarioParticipacao().ObterTodosQuestionariosComParticipacaoQueryble()
                        .Where(x => x.Usuario.ID == questionarioParticipacao.Usuario.ID && x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pos ||
                        x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Eficacia && x.DataParticipacao == null).Select(x => x.Turma);

                    if (questionariosEficaciaPos.Any())
                        questionarioParticipacaoResposta.IdTurma = questionariosEficaciaPos.FirstOrDefault().ID;
                }
            }
            catch (Exception)
            {
                //ignored;
            }

            if (ultrapassouLimiteTempoProva)
            {
                //O sistema salva a prova mas não dá retorno nem calcula nota.
                pFinalizacaoTrilhaNivel = new Tuple<bool, decimal, string>(false, 0,
                    "Seu prazo para preenchimento da prova está expirado.");
                return;
            }

            if (questionarioParticipacao.TrilhaNivel != null &&
                questionarioParticipacao.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Prova)
            {
                var pResultadoProva = ValidarPercentualparaEncerramentoTrilhaNivel(questionarioParticipacao,
                    novasTrilhas);
                pFinalizacaoTrilhaNivel = new Tuple<bool, decimal, string>(pResultadoProva.Item1, pResultadoProva.Item2,
                    string.Empty);
            }

            questionarioParticipacaoResposta.TipoQuestionario =
                questionarioParticipacao.Questionario.TipoQuestionario.ID;

            questionarioParticipacaoResposta.ID = questionarioParticipacao.ID;
        }

        /// <summary>
        /// Salva a avaliação do tutor para uma questão, em questões que avaliem o tutor.
        /// </summary>
        /// <param name="itemQuestionarioParticipacao"></param>
        /// <param name="iqpDto"></param>
        private static void SalvarAvaliacaoProfessor(ItemQuestionarioParticipacao itemQuestionarioParticipacao,
            DTOItemQuestionarioParticipacao iqpDto)
        {
            if (itemQuestionarioParticipacao.InAvaliaProfessor == false)
                return;

            // Pula agrupador de questões.
            if (itemQuestionarioParticipacao.TipoItemQuestionario.ID ==
                (int) enumTipoItemQuestionario.AgrupadorDeQuestoes)
                return;

            var manterRespostaParticipacaoProfessor = new ManterRespostaParticipacaoProfessor();
            var manterRespostaParticipacaoProfessorOpcoes = new ManterRespostaParticipacaoProfessorOpcoes();

            foreach (var rppDto in iqpDto.ListaRespostaProfessor)
            {
                var rpp = manterRespostaParticipacaoProfessor.ObterRespostaParticipacaoProfessorPorID(rppDto.ID);

                var listaRppo =
                    manterRespostaParticipacaoProfessorOpcoes.ObterTodosIQueryable()
                        .Where(x => x.RespostaParticipacaoProfessor.ID == rpp.ID);

                rpp.Resposta = rppDto.Resposta;

                manterRespostaParticipacaoProfessor.Salvar(rpp);

                foreach (var rppoDto in rppDto.ListaRespostaOpcoes.Where(x => x.RespostaSelecionada))
                {
                    var rppo = listaRppo.FirstOrDefault(x => x.ID == rppoDto.ID);

                    if (rppo == null)
                        throw new AcademicoException("Não foi possível obter a relação dos tutores com as questões.");

                    rppo.RespostaSelecionada = true;

                    manterRespostaParticipacaoProfessorOpcoes.Salvar(rppo);
                }
            }








            //var idsProfessores = pQuestionario.ListaProfessor.Select(x => x.ID).ToList();

            //var respostaParticipacaoProfessor = manterRespostaParticipacaoProfessor.ObterTodosIQueryable()
            //    .Where(
            //        x =>
            //            x.ItemQuestionarioParticipacao.ID == itemQuestionarioParticipacao.ID &&
            //            idsProfessores.Contains(x.Professor.ID));

            //if (!respostaParticipacaoProfessor.Any())
            //    throw new AcademicoException("Não foi possível obter a relação dos tutores com as questões.");

            //for (var i = 0; i < pQuestionario.ListaProfessor.Count; i++)
            //{
            //    var professor = pQuestionario.ListaProfessor[i];

            //    var rpp =
            //        respostaParticipacaoProfessor.FirstOrDefault(x => x.Professor.ID == professor.ID);

            //    if (rpp == null)
            //        throw new AcademicoException("Não foi possível obter a relação dos tutores com as questões.");

            //    if (itemQuestionarioParticipacao.TipoItemQuestionario.ID == (int)enumTipoItemQuestionario.Discursiva)
            //    {
            //        rpp.Resposta = iqpDto.Resposta[i];

            //        manterRespostaParticipacaoProfessor.Salvar(rpp);
            //    }
            //    else
            //    {
            //        foreach (
            //            var opcaoDto in
            //                iqpDto.ListaOpcoes.Where(
            //                    x => x.RespostaSelecionada != null && x.RespostaSelecionada.Any(y => y == true)))
            //        {
            //            var iqpo =
            //                itemQuestionarioParticipacao.ListaOpcoesParticipacao.FirstOrDefault(
            //                    y => y.ID == opcaoDto.ID);

            //            if (iqpo == null)
            //                continue;

            //            var rppo =
            //                manterRespostaParticipacaoProfessorOpcoes.ObterTodosIQueryable()
            //                    .FirstOrDefault(
            //                        x =>
            //                            x.ItemQuestionarioParticipacaoOpcoes.ID == iqpo.ID &&
            //                            x.RespostaParticipacaoProfessor.ID == rpp.ID);

            //            if (rppo == null)
            //                continue;

            //            rppo.RespostaSelecionada = true;

            //            manterRespostaParticipacaoProfessorOpcoes.Salvar(rppo);
            //        }
            //    }
            //}

            //if (itemQuestionarioParticipacao.TipoItemQuestionario.ID == (int) enumTipoItemQuestionario.Discursiva)
            //{
            //    if (iqpDto.Resposta.Count > 0)
            //    {
            //        itemQuestionarioParticipacao.Resposta = iqpDto.Resposta[0];

            //        for (var i = 0; i < pQuestionario.ListaProfessor.Count; i++)
            //        {
            //            var rpp =
            //                manterRespostaParticipacaoProfessor.ObterRespostaParticipacaoProfessorPorFiltro(new RespostaParticipacaoProfessor
            //                {
            //                    ItemQuestionarioParticipacao = itemQuestionarioParticipacao,
            //                    Professor =
            //                        (new ManterUsuario()).ObterUsuarioPorID(
            //                            pQuestionario.ListaProfessor[i].ID),
            //                }).FirstOrDefault();

            //            if (rpp == null) continue;

            //            rpp.Resposta = iqpDto.Resposta[i];

            //            (new BMRespostaParticipacaoProfessor()).Salvar(rpp);
            //        }
            //    }
            //}
            //else
            //{

            //}
        }

        public Tuple<bool, decimal> ValidarPercentualparaEncerramentoTrilhaNivel(QuestionarioParticipacao qp,
            bool novasTrilhas = false)
        {
            decimal percObtido;
            ManterUsuarioTrilha manterUsuarioTrilha;

            UsuarioTrilha usuarioTrilha;

            CalcularPercentualDaProva(qp, out percObtido, out manterUsuarioTrilha, out usuarioTrilha, novasTrilhas);

            usuarioTrilha.DataLiberacaoNovaProva = null;
            usuarioTrilha.NovaProvaLiberada = false;

            if (qp.TrilhaNivel.NotaMinima.HasValue && percObtido >= qp.TrilhaNivel.NotaMinima.Value)
            {
                usuarioTrilha.DataFim = DateTime.Now;
                usuarioTrilha.StatusMatricula = enumStatusMatricula.Aprovado;
                manterUsuarioTrilha.Salvar(usuarioTrilha);
                return new Tuple<bool, decimal>(true, percObtido);
            }
            else
            {
                usuarioTrilha.DataLiberacaoNovaProva = DateTime.Now.AddDays(30.0);
                usuarioTrilha.NovaProvaLiberada = true;

                usuarioTrilha.StatusMatricula = enumStatusMatricula.Reprovado;
                manterUsuarioTrilha.Salvar(usuarioTrilha);
                return new Tuple<bool, decimal>(false, percObtido);
            }
        }

        public void CalcularPercentualDaProva(QuestionarioParticipacao qp, out decimal percObtido,
            out ManterUsuarioTrilha utBm, out UsuarioTrilha ut, bool novasTrilhas = false)
        {
            decimal totalPontosPossiveis = 0;
            decimal totalPontosConseguidos = 0;
            foreach (ItemQuestionarioParticipacao iqp in qp.ListaItemQuestionarioParticipacao)
            {
                totalPontosPossiveis += iqp.ValorQuestao == null ? 0 : iqp.ValorQuestao.Value;
                totalPontosConseguidos += iqp.ValorAtribuido == null ? 0 : iqp.ValorAtribuido.Value;
            }

            if (totalPontosPossiveis > 0)
            {
                percObtido = (totalPontosConseguidos / totalPontosPossiveis) * 10;
            }
            else
            {
                percObtido = 0;
            }

            utBm = new ManterUsuarioTrilha();

            ut =
                utBm.ObterPorFiltro(new UsuarioTrilha
                {
                    TrilhaNivel = qp.TrilhaNivel,
                    Usuario = qp.Usuario,
                    NovasTrilhas = novasTrilhas
                }).FirstOrDefault();
            if (ut != null)
                ut.NotaProva = percObtido;
        }



        public DTOQuestionarioParticipacao ConsultarResultadoProvaTrilha(int IdUsuario, int IdTrilhaNivel)
        {
            IList<QuestionarioParticipacao> lstQuestionarioParticipacao = (
                new BMQuestionario()).ObterQuestionarioParticipacaoPorFiltro(
                    new QuestionarioParticipacao()
                    {
                        Usuario = (new BMUsuario()).ObterPorId(IdUsuario),
                        TrilhaNivel = new BMTrilhaNivel().ObterPorID(IdTrilhaNivel)
                    });

            lstQuestionarioParticipacao =
                lstQuestionarioParticipacao.Where(
                    x =>
                        x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Prova &&
                        x.DataParticipacao != null).OrderByDescending(x => x.DataGeracao).ToList();
            return GetListaDto(lstQuestionarioParticipacao.ToList()).FirstOrDefault();
        }

        public void CadastrarQuestionarioAvulsoParticipacao(int idUsuario, Questionario questionario,
            TipoQuestionarioAssociacao tipoQuestionario, AuthenticationRequest autenticacao)
        {
            // Questionários avulsos não possuem associacao por turma ou trilha
            Usuario us = (new BMUsuario()).ObterPorId(idUsuario);

            QuestionarioParticipacao qp = new QuestionarioParticipacao()
            {
                DataGeracao = DateTime.Now,
                NivelOcupacional = us.NivelOcupacional,
                Questionario = questionario,
                Usuario = us,
                Uf = us.UF,
                Auditoria = new Auditoria(autenticacao.Login),
                TipoQuestionarioAssociacao = tipoQuestionario,
                ListaItemQuestionarioParticipacao = new List<ItemQuestionarioParticipacao>(),
                TextoEnunciadoPre = questionario.TextoEnunciado,
            };

            if (questionario.PrazoMinutos.HasValue && questionario.PrazoMinutos.Value > 0)
                qp.DataLimiteParticipacao = DateTime.Now.AddMinutes(questionario.PrazoMinutos.Value);
            else
                qp.DataLimiteParticipacao = DateTime.Now.AddMinutes(120);

            IList<ItemQuestionario> lstIq;

            lstIq = questionario.ListaItemQuestionario;

            foreach (ItemQuestionario iq in lstIq)
            {
                ItemQuestionarioParticipacao iqp = new ItemQuestionarioParticipacao()
                {
                    Auditoria = new Auditoria(autenticacao.Login),
                    Gabarito = iq.NomeGabarito,
                    Questao = iq.Questao,
                    QuestionarioParticipacao = qp,
                    TipoItemQuestionario = iq.TipoItemQuestionario,
                    EstiloItemQuestionario = iq.EstiloItemQuestionario,
                    ValorQuestao = iq.ValorQuestao,
                    ListaOpcoesParticipacao = new List<ItemQuestionarioParticipacaoOpcoes>(),
                    Feedback = iq.Feedback,
                    InAvaliaProfessor = iq.InAvaliaProfessor,
                    RespostaObrigatoria = iq.RespostaObrigatoria
                };


                foreach (ItemQuestionarioOpcoes iqo in iq.ListaItemQuestionarioOpcoes)
                {
                    ItemQuestionarioParticipacaoOpcoes iqop = new ItemQuestionarioParticipacaoOpcoes()
                    {
                        Auditoria = new Auditoria(autenticacao.Login),
                        Descricao = iqo.Descricao,
                        ItemQuestionarioParticipacao = iqp,
                        Nome = iqo.Nome,
                        RespostaCorreta = iqo.RespostaCorreta,
                        RespostaSelecionada = null,
                        Usuario = us,
                        TipoDiagnostico = iqo.TipoDiagnostico
                    };

                    iqp.ListaOpcoesParticipacao.Add(iqop);
                }

                qp.ListaItemQuestionarioParticipacao.Add(iqp);
            }
            (new BMQuestionarioParticipacao()).Salvar(qp);

        }

        public DTOQuestionarioParticipacao ObterQuestionarioSimulado(DTOCadastroQuestionarioParticipacao pQuestionario,
            enumTipoQuestionarioAssociacao tipoAssociacao, UsuarioTrilha matriculaUsuarioTrilha)
        {
            Questionario questionario;

            var bmQuestionario = new BMQuestionario();

            switch (tipoAssociacao)
            {
                case enumTipoQuestionarioAssociacao.Pos:
                    questionario =
                    bmQuestionario.ObterTodosIQueryable()
                        .AsQueryable()
                        .FirstOrDefault(
                            q =>
                                q.ListaQuestionarioAssociacao.Any(
                                    qa =>
                                        qa.TrilhaNivel.ID == pQuestionario.IdTrilhaNivel &&
                                        qa.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pos));

                    break;
                case enumTipoQuestionarioAssociacao.Prova:
                    questionario =
                    bmQuestionario.ObterTodosIQueryable()
                        .AsQueryable()
                        .FirstOrDefault(
                            q =>
                                q.ListaQuestionarioAssociacao.Any(
                                    qa =>
                                        qa.TrilhaNivel.ID == pQuestionario.IdTrilhaNivel &&
                                        qa.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Prova));
                    break;
                case enumTipoQuestionarioAssociacao.AtividadeTrilha:
                    if (pQuestionario.IdItemTrilha == null)
                        throw new ResponseException(enumResponseStatusCode.SolucaoSebraeNaoInformada);

                    questionario =
                        new ManterItemTrilha().ObterItemTrilhaPorID(pQuestionario.IdItemTrilha.Value).Questionario;
                    break;
                default:
                    throw new AcademicoException("Tipo de questionário inválido.");
            }

            if (questionario == null)
                throw new ResponseException(enumResponseStatusCode.QuestionarioNaoEncontrado);

            var retorno = new DTOQuestionarioParticipacao
            {
                ID = questionario.ID,
                TextoEnunciadoPre = this.GetTextPlusTags(matriculaUsuarioTrilha, questionario.TextoEnunciado),
                TipoQuestionario = questionario.TipoQuestionario.ID,
                TipoQuestionarioAssociacao = new DTOTipoQuestionarioAssociacao { ID = (int)tipoAssociacao },
                DataGeracao = DateTime.Now,
                DataLimiteParticipacao =
                    questionario.PrazoMinutos.HasValue && questionario.PrazoMinutos.Value > 0
                        ? DateTime.Now.AddMinutes(questionario.PrazoMinutos.Value)
                        : DateTime.Now.AddMinutes(120),
                ListaItemQuestionarioParticipacao = new List<DTOItemQuestionarioParticipacao>(),

            };

            // Só exibr o feedback se o questionário não for Prova nem Atividade Trilha.
            var exibirFeedback = questionario.TipoQuestionario.ID != (int)enumTipoQuestionario.AtividadeTrilha &&
                                 questionario.TipoQuestionario.ID != (int)enumTipoQuestionario.AvaliacaoProva;

            foreach (var itemQuestionario in questionario.ListaItemQuestionario.OrderBy(x => x.Ordem).ToList())
            {
                var iqp = new DTOItemQuestionarioParticipacao
                {
                    ID = itemQuestionario.ID,
                    Gabarito = itemQuestionario.NomeGabarito,
                    Questao = itemQuestionario.Questao,
                    TipoItemQuestionario = new DTOTipoItemQuestionario { ID = itemQuestionario.TipoItemQuestionario.ID },
                    EstiloItemQuestionario = itemQuestionario.EstiloItemQuestionario != null
                        ? new DTOEstiloItemQuestionario { ID = itemQuestionario.EstiloItemQuestionario.ID }
                        : null,
                    ValorQuestao = itemQuestionario.ValorQuestao,
                    ListaOpcoes = new List<DTOItemQuestionarioParticipacaoOpcoes>(),
                    Feedback = itemQuestionario.ExibeFeedback == true && exibirFeedback ? itemQuestionario.Feedback : null,
                    RespostaObrigatoria = itemQuestionario.RespostaObrigatoria
                };

                // Se for de colunas relacionadas, deve embaralhar as opções das colunas.
                var opcoes = itemQuestionario.TipoItemQuestionario.ID ==
                             (int)enumTipoItemQuestionario.ColunasRelacionadas
                    ?
                    // O OrderBy no final ordena os false primeiro, depois os true.
                    SortearOpcoesItemQuestionario(itemQuestionario).OrderBy(x => x.OpcaoVinculada != null).ToList()
                    : itemQuestionario.ListaItemQuestionarioOpcoes;

                foreach (var itemQuestionarioOpcao in opcoes)
                {
                    var iqop = new DTOItemQuestionarioParticipacaoOpcoes
                    {
                        ID = itemQuestionarioOpcao.ID,
                        Nome = itemQuestionarioOpcao.Nome,
                        RespostaCorreta = itemQuestionarioOpcao.RespostaCorreta,
                        RespostaSelecionada = null,

                        // Se houver opção vinculada, ela já deverá estar na lista por causa do OrderBy acima.
                        OpcaoVinculada = itemQuestionarioOpcao.OpcaoVinculada != null
                            ? iqp.ListaOpcoes.FirstOrDefault(x => x.ID == itemQuestionarioOpcao.OpcaoVinculada.ID)
                            : null
                    };

                    iqp.ListaOpcoes.Add(iqop);
                }

                retorno.ListaItemQuestionarioParticipacao.Add(iqp);
            }

            return retorno;
        }

        public QuestionarioParticipacao ObterPorId(int id)
        {
            return new BMQuestionarioParticipacao().ObterPorID(id);
        }
    }
}