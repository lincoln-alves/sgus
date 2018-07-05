using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioQuestionario : BusinessProcessBaseRelatorio, IDisposable
    {
        protected override enumRelatorio Relatorio
        {
            get { return enumRelatorio.Questionario; }
        }

        public IQueryable<Questionario> ListaQuestionario(DTOFiltroRelatorioQuestionario filtro)
        {
            var resultado = new ManterQuestionario().ObterTodosIQueryable();

            if (filtro.IdTurma != null)
                resultado = resultado.Where(x => x.ListaQuestionarioParticipacao.Any(y => y.Turma.ID == filtro.IdTurma));

            if (filtro.IdOferta != null)
                resultado = resultado.Where(x => x.ListaQuestionarioParticipacao.Any(y => y.Turma.Oferta.ID == filtro.IdOferta));

            if (filtro.IdSolucaoEducacional != null)
                resultado = resultado.Where(x => x.ListaQuestionarioParticipacao.Any(y => y.Turma.Oferta.SolucaoEducacional.ID == filtro.IdSolucaoEducacional));

            if (filtro.IdProcesso != null)
                resultado = resultado.Where(x => x.ListaCampos.Any(c => c.Etapa.Processo.ID == filtro.IdProcesso));

            if (filtro.IdTipoQuestionario != null)
                resultado = resultado.Where(x => x.TipoQuestionario.ID == filtro.IdTipoQuestionario);

            return resultado;
        }

        public IQueryable<SolucaoEducacional> ListaSolucaoEducacional()
        {
            return new ManterSolucaoEducacional().ObterComQuestionario();
        }

        public IQueryable<SolucaoEducacional> ListaSolucaoEducacional(int idQuestionario)
        {
            return new ManterSolucaoEducacional().ObterComQuestionario(idQuestionario);
        }

        public Usuario ObterProfessorPorId(int idUsuario)
        {
            return new ManterUsuario().ObterUsuarioPorID(idUsuario);
        }

        public IQueryable<SolucaoEducacional> ListaSolucaoEducacionalPorCategorias(List<int> categorias)
        {
            var lista = new ManterSolucaoEducacional().ObterTodosSolucaoEducacional();

            if (categorias != null && categorias.Any())
                lista = lista.Where(x => categorias.Contains(x.CategoriaConteudo.ID));

            return lista;
        }

        public IQueryable<SolucaoEducacional> ListaSolucaoEducacionalPorProfessor(DTOFiltroRelatorioQuestionario filtro)
        {
            if (filtro.IdProfessor == null)
                return null;

            // Obtém os ids das SEs que possuem o professor informado e que possuam turmas com questionários pós de pesquisa.
            var ses = new ManterTurmaProfessor().ObterTurmaProfessorPorProfessor(filtro.IdProfessor.Value).Where(x =>
                x.Turma.ListaQuestionarioAssociacao.Any(
                    qa =>
                        qa.TipoQuestionarioAssociacao.ID ==
                        (int) enumTipoQuestionarioAssociacao.Pos &&
                        qa.Questionario.TipoQuestionario.ID ==
                        (int) enumTipoQuestionario.Pesquisa))
                .Select(x => x.Turma.Oferta.SolucaoEducacional.ID)
                .Distinct()
                .ToList();

            // Tem que consultar novamente porque o Distinct não funciona acima porque a tabela possui campo text, que não pode ser distinguível.
            return new ManterSolucaoEducacional().ObterTodosSolucaoEducacional().Where(x => ses.Contains(x.ID));
        }

        public IQueryable<Oferta> ListaOferta(DTOFiltroRelatorioQuestionario filtro)
        {
            if (filtro.IdSolucaoEducacional == null)
                return null;

            if (filtro.IsRelatorioTutor)
            {
                if (filtro.IdProfessor == null)
                    return null;

                // Obtém os ids das Ofertas da SE informada que possuem o professor informado e que possuam turmas com questionários pós de pesquisa.
                var ofertas =
                    new ManterTurmaProfessor().ObterTurmaProfessorPorProfessor(filtro.IdProfessor.Value).Where(x =>

                        x.Turma.Oferta.SolucaoEducacional.ID == filtro.IdSolucaoEducacional.Value &&

                        x.Turma.ListaQuestionarioAssociacao.Any(
                            qa =>
                                qa.TipoQuestionarioAssociacao.ID ==
                                (int) enumTipoQuestionarioAssociacao.Pos &&
                                qa.Questionario.TipoQuestionario.ID ==
                                (int) enumTipoQuestionario.Pesquisa))
                        .Select(x => x.Turma.Oferta.ID)
                        .Distinct()
                        .ToList();

                // Tem que consultar novamente porque o Distinct não funciona acima porque a tabela possui campo text, que não pode ser distinguível.
                return new ManterOferta().ObterTodasOfertas().Where(x => ofertas.Contains(x.ID));
            }

            var retorno =
                new ManterOferta().ObterOfertaPorSolucaoEducacional(new SolucaoEducacional
                {
                    ID = filtro.IdSolucaoEducacional.Value
                });

            if(filtro.IdQuestionario > 0)
            {
                retorno = retorno.Where(x => x.ListaTurma.Any(t => t.ListaQuestionarioAssociacao.Any(q => q.Questionario.ID == filtro.IdQuestionario)));
            }


            return retorno.Where(x => x.ListaTurma.Any(t => t.ListaQuestionarioAssociacao.Any())).OrderBy(x => x.Nome).AsQueryable();
        }

        public IQueryable<Turma> ListaTurma(DTOFiltroRelatorioQuestionario filtro)
        {
            if (filtro.IdOferta == null)
                return null;

            if (filtro.IsRelatorioTutor)
            {
                if (filtro.IdProfessor == null)
                    return null;

                // Obtém os ids das Turmas da Oferta informada que possuem o professor informado e que possuam turmas com questionários pós de pesquisa.
                var turmas =
                    new ManterTurmaProfessor().ObterTurmaProfessorPorProfessor(filtro.IdProfessor.Value).Where(x =>

                        x.Turma.Oferta.ID == filtro.IdOferta.Value &&

                        x.Turma.ListaQuestionarioAssociacao.Any(
                            qa =>
                                qa.TipoQuestionarioAssociacao.ID ==
                                (int) enumTipoQuestionarioAssociacao.Pos &&
                                qa.Questionario.TipoQuestionario.ID ==
                                (int) enumTipoQuestionario.Pesquisa))
                        .Select(x => x.Turma.ID)
                        .Distinct()
                        .ToList();

                // Tem que consultar novamente porque o Distinct não funciona acima porque a tabela possui campo text, que não pode ser distinguível.
                return new ManterTurma().ObterTodasTurma().Where(x => turmas.Contains(x.ID)).OrderBy(x => x.Nome).AsQueryable();
            }

            return new ManterTurma().ObterTurmasPorOferta(filtro.IdOferta.Value, false);
        }

        public IQueryable<Usuario> ListaProfessor()
        {
            //Obter professores de turmas que possuam questionários pós de pesquisa.

            var profs = new ManterTurmaProfessor().ObterTodos().Where(x =>

                x.Turma.ListaQuestionarioAssociacao.Any(
                    qa =>
                        qa.TipoQuestionarioAssociacao.ID ==
                        (int) enumTipoQuestionarioAssociacao.Pos &&
                        qa.Questionario.TipoQuestionario.ID ==
                        (int) enumTipoQuestionario.Pesquisa))
                .Select(x => x.Professor.ID)
                .Distinct();

            return new ManterUsuario().ObterTodos().OrderBy(x => x.Nome).Where(x => profs.Contains(x.ID));
        }

        public IList<Uf> ListaUf()
        {
            return new ManterUf().ObterTodosUf().OrderBy(x => x.Nome).ToList();
        }

        public IList<NivelOcupacional> ListaNivelOcupacional()
        {
            return new ManterNivelOcupacional().ObterTodosNivelOcupacional().OrderBy(x => x.Nome).ToList();
        }

        public object ConsultarRelatorioQuestionarios(string pFormaAquisicao, string pNivelOcupacional, string pUf,
            DateTime? pDataInicial, DateTime? pDataFinal,
            int pSolucaoEducacional, int pOferta, int pTurma, int? pTipoQuestionario, int pQuestionario,
            IEnumerable<int> categorias, string pTipoRelatorio)
        {
            RegistrarLogExecucao();
            return (new ManterQuestionario()).ConsultarRelatorioQuestionarios(pFormaAquisicao, pNivelOcupacional, pUf,
                pDataInicial, pDataFinal, pSolucaoEducacional, pOferta, pTurma, pTipoQuestionario, pQuestionario,
                categorias, pTipoRelatorio);
        }

        public IList<DTORelatorioPesquisa> ConsultarQuestionarioPesquisa(string pFormaAquisicao,
            string pNivelOcupacional, string pUf, DateTime? pDataInicial, DateTime? pDataFinal,
            int pSolucaoEducacional, int pOferta, int pTurma, int? pTipoQuestionario, int pQuestionario,
            IEnumerable<int> categorias, string pTipoRelatorio)
        {
            RegistrarLogExecucao();
            return (new ManterQuestionario()).ConsultarQuestionarioPesquisa(pFormaAquisicao, pNivelOcupacional, pUf,
                pDataInicial, pDataFinal, pSolucaoEducacional, pOferta, pTurma, pTipoQuestionario, pQuestionario,
                categorias, pTipoRelatorio);
        }

        public DTORelatorioQuestionarioRespondente ObterRelatorioRespondente(DTOFiltroRelatorioQuestionario filtro)
        {
            if (!filtro.PossuiDados())
                throw new AcademicoException("Não existem filtros para efetuar uma pesquisa.");

            if (filtro.IsRelatorioTutor && filtro.IdTurma == null)
                throw new AcademicoException("O campo \"Turma\" é obrigatório no relatório de tutor.");

            if (filtro.IdQuestionario == 0)
                throw new AcademicoException("Selecione um questionário.");

            if (filtro.IdOferta != null && filtro.IdTurma == null)
                throw new AcademicoException("O campo \"Turma\" é obrigatório.");

            Questionario questionario;
            Turma turma;

            if (filtro.IsRelatorioTutor)
            {
                var associacao = new ManterTurma().ObterTurmaPorID(filtro.IdTurma.Value)
                    .ListaQuestionarioAssociacao.FirstOrDefault(
                        x =>
                            x.TipoQuestionarioAssociacao.ID == (int) enumTipoQuestionarioAssociacao.Pos &&
                            x.Questionario.TipoQuestionario.ID == (int) enumTipoQuestionario.Pesquisa);

                if (associacao == null)
                    throw new AcademicoException("A turma selecionada não possui questionário pós de pesquisa.");

                questionario = associacao.Questionario;
                turma = associacao.Turma;
            }
            else
            {
                questionario = new ManterQuestionario().ObterQuestionarioPorID(filtro.IdQuestionario);
                if (filtro.IdTurma != null)
                {
                    turma = new ManterTurma().ObterTurmaPorID(filtro.IdTurma.Value);
                }
                else
                {
                    turma = new Turma();
                }
            }

            if (questionario == null)
                throw new AcademicoException(
                    "Nenhum questionário foi encontrado. Se certifique que o questionário selecionado ainda está cadastrado.");

            var listaItemQuestionario = questionario.ListaItemQuestionario.OrderBy(x => x.Ordem).ToList();

            // Se não houverem questões relacionadas ao questionário, não tem como emitir o relatório.
            if (!listaItemQuestionario.Any())
                throw new AcademicoException("O questionário \"" + questionario.Nome +
                                             "\" não possui questões cadastradas, portanto não é possível emitir o relatório.");

            List<DTORelatorioQuestionarioQuestao> questoes;

            var enunciados = ObterEnunciados(out questoes, listaItemQuestionario, questionario, turma);


            // Obter consulta do questionário, mas não enumera ainda.
            // You know what i see here? Possibilities...
            var consulta = new ManterQuestionarioParticipacao().ObterPorQuestionarioPorFiltro(filtro);

            // Retornar dados do relatório.
            return new DTORelatorioQuestionarioRespondente
            {
                Enunciados = enunciados,
                Questoes = questoes,
                Consulta = consulta,
                TotalRespostas = consulta.Count(),
                TotalQuestoes = listaItemQuestionario.Count()
            };
        }

        public List<DTORelatorioQuestionarioEstatistico> ObterRelatorioEstatistico(
            List<DTORelatorioQuestionarioEnunciado> lstCabecalho,
            List<DTORelatorioQuestionarioQuestao> lstTopico,
            IQueryable<QuestionarioParticipacao> consulta)
        {
            var listaConsolidado = new List<DTORelatorioQuestionarioEstatistico>();

            var listaParticipacao = ConverterDtoRespostas(consulta.ToList(), lstTopico);

            foreach (var cabecalho in lstCabecalho)
            {
                var consolidado = new DTORelatorioQuestionarioEstatistico();

                var lst = listaParticipacao.SelectMany(p => p.Respostas.Where(x => x.Nota.HasValue).ToList()).ToList();

                consolidado.Principal = cabecalho.Nome;
                consolidado.QtdeItens = lstTopico.Count(x => x.IdEnunciado == cabecalho.Id);

                if (lst.Any(x => x.Questao.IdEnunciado == cabecalho.Id))
                    consolidado.MediaFinal =
                        Math.Round(
                            lst.Where(x => x.Questao.IdEnunciado == cabecalho.Id)
                                .Average(x => x.Nota != null ? x.Nota.Value : 0), 2);

                var ct = 0;

                foreach (var topico in lstTopico.Where(x => x.IdEnunciado == cabecalho.Id).ToList())
                {
                    if (ct++ > 0)
                        consolidado = new DTORelatorioQuestionarioEstatistico();

                    var topicoAtual = lst.Where(x => x.Questao.Id == topico.Id && x.Nota.HasValue).ToList();

                    consolidado.Nome = topico.Nome;

                    try
                    {
                        consolidado.Max = topicoAtual.Max(x => x.Nota != null ? x.Nota.Value : 0);
                        consolidado.Media = Math.Round(topicoAtual.Average(x => x.Nota != null ? x.Nota.Value : 0), 2);
                        consolidado.Min = topicoAtual.Min(x => x.Nota != null ? x.Nota.Value : 0);

                        var dp = DesvioPadrao(topicoAtual.Select(x => x.Nota != null ? x.Nota.Value : 0));

                        consolidado.DP = double.IsNaN(dp) ? double.NaN : Math.Round(dp, 2);
                        consolidado.Moda = Moda(topicoAtual);
                    }
                    catch
                    {
                        // ignored
                    }

                    listaConsolidado.Add(consolidado);
                }
            }

            return listaConsolidado;
        }

        private class ProfessorDto
        {
            public int ID { get; set; }
            public string Nome { get; set; }
            public List<int> ItensQuestionarioParticipacaoIds { get; set; }
        }

        private static List<DTORelatorioQuestionarioEnunciado> ObterEnunciados(
            out List<DTORelatorioQuestionarioQuestao> questoes, List<ItemQuestionario> listaItemQuestionario,
            Questionario questionario, Turma turma)
        {
            questoes = new List<DTORelatorioQuestionarioQuestao>();

            var listaEnunciados = new List<DTORelatorioQuestionarioEnunciado>();

            DTORelatorioQuestionarioEnunciado enunciado = null;

            var idEnunciado = 0;

            var questionariosParticipacaoIds = new ManterQuestionarioParticipacao()
                .ObterTodosIQueryable()
                .Where(x => x.Questionario.ID == questionario.ID && x.Turma != null && x.Turma.ID == turma.ID)
                .Select(x => x.ID)
                .ToList();

            var professores = new ManterRespostaParticipacaoProfessor().ObterTodosIQueryable()
                .Join(new ManterTurmaProfessor().ObterTodos(), rpp => rpp.Professor.ID, tp => tp.Professor.ID, (rpp, tp) =>
                    new
                    {
                        rpp,
                        tp
                    })
                .Where(x => x.tp.Turma.ID == turma.ID && questionariosParticipacaoIds.Contains(x.rpp.QuestionarioParticipacao.ID))
                .Select(x => new
                {
                    x.rpp.Professor.ID,
                    x.rpp.Professor.Nome,
                    ItemQuestionarioParticipacaoID = x.rpp.ItemQuestionarioParticipacao.ID
                })
                .ToList()
                .GroupBy(x => new { x.ID, x.Nome })
                .Select(x => new ProfessorDto
                {
                    ID = x.Key.ID,
                    Nome = x.Key.Nome,
                    ItensQuestionarioParticipacaoIds = x.Select(y => y.ItemQuestionarioParticipacaoID).Distinct().ToList()
                })
                .ToList();

            foreach (var item in listaItemQuestionario)
            {
                // OOYL
                // Buscar o enunciado da sequencia e atribuir todas as questões que estão entre ele e o próximo enunciado como
                // subquestões deste enunciado. Caso a ordem das questões esteja mal formada, tudo abaixo deixará de funcionar.
                if (item.TipoItemQuestionario.ID == (int) enumTipoItemQuestionario.AgrupadorDeQuestoes)
                {
                    if (enunciado != null)
                        listaEnunciados.Add(enunciado);

                    enunciado = new DTORelatorioQuestionarioEnunciado();
                    idEnunciado = item.ID;
                    enunciado.Id = item.ID;
                    enunciado.Nome = item.Questao;
                }
                else
                {
                    if (enunciado == null)
                        enunciado = new DTORelatorioQuestionarioEnunciado();

                    enunciado.QuestoesRelacionadas.Add(new DTORelatorioQuestionarioQuestao
                    {
                        Id = item.ID,
                        IdEnunciado = idEnunciado,
                        Nome = item.Questao
                    });

                    if (item.InAvaliaProfessor && professores.Any())
                    {
                        foreach (var professor in professores)
                        {
                            questoes.Add(new DTORelatorioQuestionarioQuestao
                            {
                                Id = item.ID,
                                IdEnunciado = idEnunciado,
                                Nome = item.Questao, // É fundamental que o nome permaneça o mesmo.
                                AvaliaProfessor = true,
                                IdProfessor = professor.ID,
                                NomeProfessor = professor.Nome,
                                ItensQuestionarioParticipacaoIds = professor.ItensQuestionarioParticipacaoIds
                            });
                        }
                    }
                    else
                    {
                        questoes.Add(new DTORelatorioQuestionarioQuestao
                        {
                            Id = item.ID,
                            IdEnunciado = idEnunciado,
                            Nome = item.Questao
                        });
                    }
                }
            }

            if (enunciado != null)
                listaEnunciados.Add(enunciado);

            return listaEnunciados;
        }

        public static List<DTORelatorioQuestionarioParticipacao> ConverterDtoRespostas(
            IList<QuestionarioParticipacao> listaQuestionarioParticipacao,
            IReadOnlyList<DTORelatorioQuestionarioQuestao> listaQuestoes)
        {
            var listaParticipacao = new List<DTORelatorioQuestionarioParticipacao>();

            var manterRpp = new ManterRespostaParticipacaoProfessor();

            foreach (var questionarioParticipacao in listaQuestionarioParticipacao)
            {
                // Obtém todas as questões do questionário clone.
                var itensQuestionarioParticipacao =
                    new ManterQuestionarioParticipacao().ListaItemQuestionarioParticipacao(
                        questionarioParticipacao.ID).ToList();

                // Remover questões do tipo Informação, pois não interessam para obter a nota.
                itensQuestionarioParticipacao = itensQuestionarioParticipacao
                    .Where(x => x.TipoItemQuestionario.ID != (int) enumTipoItemQuestionario.AgrupadorDeQuestoes)
                    .ToList();

                // Obter todas as respostas selecionadas de todas as questões da participação, para fazer uma consulta só
                // e utilizar esses dados no loop que monta o Dto de respostas.
                var todasRespostasSelecionadas =
                    new ManterItemQuestionarioParticipacaoOpcoes().ObterRespostasSelecionadas(
                        itensQuestionarioParticipacao).ToList();

                var professores =
                    manterRpp.ObterTodosIQueryable()
                        .Where(x => x.QuestionarioParticipacao.ID == questionarioParticipacao.ID)
                        .Select(x => new Usuario
                        {
                            ID = x.Professor.ID,
                            Nome = x.Professor.Nome
                        }).Distinct().ToList();

                var participacao = PreencherRespostas(listaQuestoes, itensQuestionarioParticipacao, todasRespostasSelecionadas,
                        questionarioParticipacao, professores);

                // Adicionar participação, com todas as respostas.
                listaParticipacao.Add(participacao);

            }

            return listaParticipacao;
        }

        private static DTORelatorioQuestionarioParticipacao PreencherRespostas(IReadOnlyList<DTORelatorioQuestionarioQuestao> listaQuestoes,
            List<ItemQuestionarioParticipacao> itensQuestionarioParticipacao,
            List<ItemQuestionarioParticipacaoOpcoes> todasRespostasSelecionadas,
            QuestionarioParticipacao questionarioParticipacao, List<Usuario> professores)
        {
            // Objeto DTO contendo vários dados do clone do questionário.
            var participacaoDto = ObterObjetoParticipacaoDto(questionarioParticipacao);

            // Obter nota a partir da opção selecionada, ou texto caso seja questão discursiva.
            foreach (var itemQuestionario in itensQuestionarioParticipacao)
            {
                if (itemQuestionario.InAvaliaProfessor && professores.Any())
                {
                    var questoes = listaQuestoes.Where(
                        x => x.Nome.ToLower().Trim() == itemQuestionario.Questao.ToLower().Trim());

                    foreach (var questao in questoes)
                    {
                        var resposta = new DTORelatorioQuestionarioResposta
                        {
                            Questao = questao
                        };

                        Usuario professor = null;

                        if (questao.IdProfessor != null)
                        {
                            professor = new Usuario
                            {
                                ID = questao.IdProfessor.Value,
                                Nome = questao.NomeProfessor
                            };
                        }

                        PreencherResposta(itemQuestionario, resposta, todasRespostasSelecionadas, professor);
                        
                        // Adicionar resposta vinculada à pergunta.
                        participacaoDto.Respostas.Add(resposta);
                    }
                }
                else
                {
                    var resposta = new DTORelatorioQuestionarioResposta
                    {
                        // Relaciona a resposta com a questão através do texto da questão.
                        // Absurdo, mas não tem outra opção de relacionar uma questão clone
                        // com a questão original. Chora, deita e rola... Muito.
                        Questao = listaQuestoes.FirstOrDefault(
                            x => x.Nome.ToLower().Trim() == itemQuestionario.Questao.ToLower().Trim())
                    };

                    // Só adiciona a resposta para uma pergunta que exista na lista informada.
                    if (resposta.Questao == null)
                        continue;

                    PreencherResposta(itemQuestionario, resposta, todasRespostasSelecionadas);

                    // Adicionar resposta vinculada à pergunta.
                    participacaoDto.Respostas.Add(resposta);
                }


                
            }

            return participacaoDto;
        }

        private static void PreencherResposta(ItemQuestionarioParticipacao itemQuestionario,
            DTORelatorioQuestionarioResposta resposta,
            List<ItemQuestionarioParticipacaoOpcoes> todasRespostasSelecionadas,
            Usuario professor = null)
        {
            RespostaParticipacaoProfessor rpp;

            try
            {
                rpp = professor != null
                    ? new ManterRespostaParticipacaoProfessor().ObterTodosIQueryable()
                        .FirstOrDefault(
                            x =>
                                x.ItemQuestionarioParticipacao.ID == itemQuestionario.ID &&
                                x.Professor.ID == professor.ID)
                    : null;
            }
            catch (Exception)
            {
                rpp = null;
            }


            // Caso seja uma questão discursiva, inserir a resposta informada.
            if (itemQuestionario.TipoItemQuestionario.ID == (int) enumTipoItemQuestionario.Discursiva)
            {
                resposta.NotaTexto = rpp != null ? rpp.Resposta : itemQuestionario.Resposta;
            }
            else
            {
                var respostaSelecionada =
                    todasRespostasSelecionadas.FirstOrDefault(
                        x => x.ItemQuestionarioParticipacao.ID == itemQuestionario.ID);

                string nota;

                if (rpp != null)
                {
                    var notaProfessor =
                        new ManterRespostaParticipacaoProfessorOpcoes().ObterTodosIQueryable()
                            .FirstOrDefault(x => x.RespostaParticipacaoProfessor.ID == rpp.ID && x.RespostaSelecionada == true);

                    nota = notaProfessor?.ItemQuestionarioParticipacaoOpcoes.Nome;
                    resposta.NotaTexto = notaProfessor?.ItemQuestionarioParticipacaoOpcoes.Nome;
                    resposta.IdProfessor = rpp.Professor.ID;
                }
                else
                {
                    nota = respostaSelecionada?.Nome;
                    resposta.NotaTexto = respostaSelecionada?.Nome;
                }

                if (nota != null)
                {
                    // Solução alternativa para obter as notas nos casos dos valores 10 e 0, que são concatenados com strings.
                    // Não existe um campo que salve o valor numérico da resposta.
                    nota =
                        nota.ToLower().Replace("10 - satisfatório", "10")
                            .Replace("0 - insatisfatório", "0");

                    int n;
                    if (int.TryParse(nota, out n))
                    {
                        resposta.Nota = n;
                    }
                    else
                    {
                        resposta.Nota = null;
                    }
                }
                else
                {
                    // Se caiu aqui é porque o questionário resposta está mal formatado.
                    resposta.Nota = null;
                    resposta.NotaTexto = null;
                }
            }
        }

        private static DTORelatorioQuestionarioParticipacao ObterObjetoParticipacaoDto(
            QuestionarioParticipacao questionarioParticipacao)
        {
            var participacao = new DTORelatorioQuestionarioParticipacao();

            var existeTurma = questionarioParticipacao.Turma != null;

            var existeUsuario = questionarioParticipacao.Usuario != null;

            // Só manda o ID de volta para fins de debug.
            participacao.IdQuestionarioResposta = questionarioParticipacao.ID;

            participacao.Curso = existeTurma
                ? questionarioParticipacao.Turma.Oferta.SolucaoEducacional.Nome
                : string.Empty;

            participacao.Oferta = existeTurma
                ? questionarioParticipacao.Turma.Oferta.Nome
                : string.Empty;

            participacao.Turma = existeTurma
                ? questionarioParticipacao.Turma.Nome
                : string.Empty;

            participacao.Data = questionarioParticipacao.DataParticipacao;

            participacao.Nome = existeUsuario
                ? questionarioParticipacao.Usuario.Nome
                : string.Empty;

            participacao.Questionario = questionarioParticipacao.Questionario.Nome;

            participacao.UF = questionarioParticipacao.Usuario != null &&
                              questionarioParticipacao.Usuario.UF != null
                ? questionarioParticipacao.Usuario.UF.Sigla
                : string.Empty;

            participacao.NivelOcupacional =
                questionarioParticipacao.Usuario != null &&
                questionarioParticipacao.Usuario.NivelOcupacional != null
                    ? questionarioParticipacao.Usuario.NivelOcupacional.Nome
                    : string.Empty;

            return participacao;
        }

        private static double DesvioPadrao(IEnumerable<int> source, int buffer = 1)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            var sourceEnumerable = source as int[] ?? source.ToArray();

            if (!sourceEnumerable.Any())
                return double.NaN;

            var data = sourceEnumerable.ToList();
            var average = data.Average();
            var differences = data.Select(u => Math.Pow(average - u, 2.0)).ToList();
            return Math.Sqrt(differences.Sum()/(differences.Count() - buffer));
        }

        private static int Moda(IEnumerable<DTORelatorioQuestionarioResposta> respostas)
        {
            var dados = new Dictionary<int, int>();

            foreach (var item in respostas.Where(x => x.Nota.HasValue))
            {
                if (dados.Any(x => x.Key == item.Nota))
                {
                    var t = dados.FirstOrDefault(x => x.Key == item.Nota);
                    var k = t.Key;
                    var v = (t.Value + 1);

                    dados.Remove(t.Key);
                    dados.Add(k, v);
                }
                else
                {
                    dados.Add(item.Nota.Value, 1);
                }
            }

            return dados.OrderByDescending(x => x.Value).FirstOrDefault().Key;
        }

        public void Dispose()
        {
            GC.Collect();
        }

        public IList<Processo> ListaProcessos()
        {
            return new ManterProcesso().ObterTodosProcessos().OrderBy(x => x.Nome).ToList();
        }

        public IQueryable<Processo> ListarProcessosIQueryable()
        {
            return new ManterProcesso().ObterTodosIQueryable();
        }

        public List<StatusMatricula> ListaStatus()
        {
            return new ManterStatusMatricula().ObterTodosQuePossuemQuestionarios().OrderBy(x => x.Nome).ToList();
        }
    }
}