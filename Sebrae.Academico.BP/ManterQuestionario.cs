using System;
using System.Linq;
using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.BP.DTO.Relatorios.DashBoard;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BP.DTO.Dominio;
using System.Web.Script.Serialization;

namespace Sebrae.Academico.BP
{
    public class ManterQuestionario : BusinessProcessBase
    {
        private BMQuestionario bmQuestionario = null;

        /// <summary>
        /// Método Construtor da classe
        /// </summary>
        public ManterQuestionario()
            : base()
        {
            bmQuestionario = new BMQuestionario();
        }

        public object ConsultarQuestionarioPesquisaTutor(string pTutor, string pFormaAquisicao, string pNivelOcupacional,
            string pUf, DateTime? pDataInicial, DateTime? pDataFinal,
            int pSolucaoEducacional, int pOferta, int pTurma, int pQuestionario, IEnumerable<int> categorias,
            string pTipoRelatorio)
        {
            IDictionary<string, object> lstParams = new Dictionary<string, object>();

            if (string.IsNullOrEmpty(pTutor)) lstParams.Add("pTutor", DBNull.Value);
            else lstParams.Add("pTutor", pTutor);

            if (string.IsNullOrEmpty(pFormaAquisicao)) lstParams.Add("pFormaAquisicao", DBNull.Value);
            else lstParams.Add("pFormaAquisicao", pFormaAquisicao);

            lstParams.Add("pSolucaoEducacional", pSolucaoEducacional);
            lstParams.Add("pOferta", pOferta);
            lstParams.Add("pTurma", pTurma);
            lstParams.Add("pQuestionario", pQuestionario);
            lstParams.Add("pTipoRelatorio", pTipoRelatorio);

            if (pUf != null && pUf.Any()) lstParams.Add("pUf", string.Join(", ", pUf));
            else lstParams.Add("pUf", DBNull.Value);

            if (pNivelOcupacional != null && pNivelOcupacional.Any())
                lstParams.Add("pNivelOcupacional", string.Join(", ", pNivelOcupacional));
            else lstParams.Add("pNivelOcupacional", DBNull.Value);

            if (pDataInicial.HasValue) lstParams.Add("pDataIni", pDataInicial.Value);
            else lstParams.Add("pDataIni", DBNull.Value);

            if (pDataFinal.HasValue) lstParams.Add("pDataFim", pDataFinal.Value);
            else lstParams.Add("pDataFim", DBNull.Value);

            if (pTipoRelatorio.ToLower().Equals("respondente"))
                return bmQuestionario.ExecutarProcedureTable("SP_REL_QUESTIONARIO_TUTOR", lstParams);
            return bmQuestionario.ExecutarProcedure<object>("SP_REL_QUESTIONARIO_TUTOR", lstParams);
        }

        public IList<DTOQuestionarioConsolidado> ObterQuestionarioReacao(int? pSolucaoEducacional, int? pOferta, int? pTurma, DateTime? pDataIni = null, DateTime? pDataFim = null)
        {
            IDictionary<string, object> lstParameter = new Dictionary<string, object>();

            if (pDataIni.HasValue)
            {
                lstParameter.Add("pDataIni", pDataIni.Value);
            }
            if (pDataFim.HasValue)
            {
                lstParameter.Add("pDataFim", pDataFim.Value);
            }
            if (pSolucaoEducacional.HasValue)
            {
                lstParameter.Add("pSolucaoEducacional", pSolucaoEducacional.Value);
            }
            if (pOferta.HasValue)
            {
                lstParameter.Add("pOferta", pOferta.Value);
            }
            if (pTurma.HasValue)
            {
                lstParameter.Add("pTurma", pTurma.Value);
            }

            return bmQuestionario.ExecutarProcedure<DTOQuestionarioConsolidado>("SP_REL_QUESTIONARIO_REACAO",
                lstParameter);
        }

        public object ConsultarQuestionarioPesquisaTutores(string pTutor, string pFormaAquisicao,
            string pNivelOcupacional,
            string pUf, DateTime? pDataInicial, DateTime? pDataFinal,
            int pSolucaoEducacional, int pOferta, int pTurma, int pQuestionario, IEnumerable<int> categorias,
            string pTipoRelatorio)
        {
            IDictionary<string, object> lstParams = new Dictionary<string, object>();

            if (string.IsNullOrEmpty(pTutor)) lstParams.Add("pTutor", DBNull.Value);
            else lstParams.Add("pTutor", pTutor);

            if (string.IsNullOrEmpty(pFormaAquisicao)) lstParams.Add("pFormaAquisicao", DBNull.Value);
            else lstParams.Add("pFormaAquisicao", pFormaAquisicao);

            lstParams.Add("pSolucaoEducacional", pSolucaoEducacional);
            lstParams.Add("pOferta", pOferta);
            lstParams.Add("pTurma", pTurma);
            lstParams.Add("pQuestionario", pQuestionario);
            lstParams.Add("pTipoRelatorio", pTipoRelatorio);

            if (pUf != null && pUf.Any()) lstParams.Add("pUf", string.Join(", ", pUf));
            else lstParams.Add("pUf", DBNull.Value);

            if (pNivelOcupacional != null && pNivelOcupacional.Any())
                lstParams.Add("pNivelOcupacional", string.Join(", ", pNivelOcupacional));
            else lstParams.Add("pNivelOcupacional", DBNull.Value);

            if (pDataInicial.HasValue) lstParams.Add("pDataIni", pDataInicial.Value);
            else lstParams.Add("pDataIni", DBNull.Value);

            if (pDataFinal.HasValue) lstParams.Add("pDataFim", pDataFinal.Value);
            else lstParams.Add("pDataFim", DBNull.Value);
            if (pTipoRelatorio.ToLower().Equals("respondente"))
                return bmQuestionario.ExecutarProcedureTable("SP_REL_QUESTIONARIO_TUTOR", lstParams);
            return bmQuestionario.ExecutarProcedure<object>("SP_REL_QUESTIONARIO_TUTOR", lstParams);
        }

        public IList<DTORelatorioPesquisa> ConsultarQuestionarioPesquisa(string pFormaAquisicao,
            string pNivelOcupacional, string pUf, DateTime? pDataInicial, DateTime? pDataFinal,
            int pSolucaoEducacional, int pOferta, int pTurma, int? pTipoQuestionario, int pQuestionario,
            IEnumerable<int> categorias, string pTipoRelatorio)
        {
            IDictionary<string, object> lstParams = new Dictionary<string, object>();

            if (string.IsNullOrEmpty(pFormaAquisicao)) lstParams.Add("pFormaAquisicao", DBNull.Value);
            else lstParams.Add("pFormaAquisicao", pFormaAquisicao);

            lstParams.Add("pTurma", pTurma);
            lstParams.Add("ptipoQuestionario", pTipoQuestionario ?? 2);
            lstParams.Add("pQuestionario", pQuestionario);
            lstParams.Add("pTipoRelatorio", pTipoRelatorio);

            if (pUf != null && pUf.Any()) lstParams.Add("pUf", string.Join(", ", pUf));
            else lstParams.Add("pUf", DBNull.Value);

            if (pNivelOcupacional != null && pNivelOcupacional.Any())
                lstParams.Add("pNivelOcupacional", string.Join(", ", pNivelOcupacional));
            else lstParams.Add("pNivelOcupacional", DBNull.Value);

            if (pDataInicial.HasValue) lstParams.Add("pDataIni", pDataInicial.Value);
            else lstParams.Add("pDataIni", DBNull.Value);

            if (pDataFinal.HasValue) lstParams.Add("pDataFim", pDataFinal.Value);
            else lstParams.Add("pDataFim", DBNull.Value);

            return bmQuestionario.ExecutarProcedure<DTORelatorioPesquisa>("SP_REL_QUESTIONARIO", lstParams);
        }

        public object ConsultarRelatorioQuestionarios(string pFormaAquisicao, string pNivelOcupacional, string pUf,
            DateTime? pDataInicial, DateTime? pDataFinal,
            int pSolucaoEducacional, int pOferta, int pTurma, int? pTipoQuestionario, int pQuestionario,
            IEnumerable<int> categorias, string pTipoRelatorio)
        {
            IDictionary<string, object> lstParams = new Dictionary<string, object>();

            if (string.IsNullOrEmpty(pFormaAquisicao)) lstParams.Add("pFormaAquisicao", DBNull.Value);
            else lstParams.Add("pFormaAquisicao", pFormaAquisicao);

            lstParams.Add("pTurma", pTurma);
            lstParams.Add("ptipoQuestionario", pTipoQuestionario ?? 2);
            lstParams.Add("pQuestionario", pQuestionario);
            lstParams.Add("pTipoRelatorio", pTipoRelatorio);

            if (pUf != null && pUf.Any()) lstParams.Add("pUf", string.Join(", ", pUf));
            else lstParams.Add("pUf", DBNull.Value);

            if (pNivelOcupacional != null && pNivelOcupacional.Any())
                lstParams.Add("pNivelOcupacional", string.Join(", ", pNivelOcupacional));
            else lstParams.Add("pNivelOcupacional", DBNull.Value);

            if (pDataInicial.HasValue) lstParams.Add("pDataIni", pDataInicial.Value);
            else lstParams.Add("pDataIni", DBNull.Value);

            if (pDataFinal.HasValue) lstParams.Add("pDataFim", pDataFinal.Value);
            else lstParams.Add("pDataFim", DBNull.Value);

            if (pTipoRelatorio.ToLower().Equals("respondente"))
                return bmQuestionario.ExecutarProcedureTable("SP_REL_QUESTIONARIO", lstParams);
            return bmQuestionario.ExecutarProcedure<DTORelatorioPesquisa>("SP_REL_QUESTIONARIO", lstParams);
        }

        public IList<DTOIndiceSatisfacao> ObterIndiceSatisfacaoCredenciados(DateTime? dataInicio, DateTime? dataFim,
            int idUf)
        {
            IDictionary<string, object> lstParam = new Dictionary<string, object>();

            lstParam.Add("pDataInicio",
                (!dataInicio.HasValue ? "01/01/" + DateTime.Now.Year : dataInicio.Value.ToString("MM/dd/yyyy")) +
                " 00:00:00");
            lstParam.Add("pDataFim",
                (!dataFim.HasValue ? DateTime.Now.Date.ToString("MM/dd/yyyy") : dataFim.Value.ToString("MM/dd/yyyy")) +
                " 23:59:59");

            if (idUf != 0)
            {
                lstParam.Add("IdUf", idUf);
            }

            return bmQuestionario.ExecutarProcedure<DTOIndiceSatisfacao>("DASHBOARD_REL_IndiceSatisfacaoCredenciados",
                lstParam, 3600);
        }

        public IList<DTOIndiceSatisfacao> ObterIndiceSatisfacao(DateTime? dataInicio, DateTime? dataFim, int idUf)
        {
            IDictionary<string, object> lstParam = new Dictionary<string, object>();

            lstParam.Add("pDataInicio",
                (!dataInicio.HasValue ? "01/01/" + DateTime.Now.Year : dataInicio.Value.ToString("MM/dd/yyyy")) +
                " 00:00:00");
            lstParam.Add("pDataFim",
                (!dataFim.HasValue ? DateTime.Now.Date.ToString("MM/dd/yyyy") : dataFim.Value.ToString("MM/dd/yyyy")) +
                " 23:59:59");

            if (idUf != 0)
            {
                lstParam.Add("IdUf", idUf);
            }

            return bmQuestionario.ExecutarProcedure<DTOIndiceSatisfacao>("DASHBOARD_REL_IndiceSatisfacao", lstParam,
                3600);
        }

        public void IncluirQuestionario(Questionario pQuestionario)
        {
            try
            {
                this.PreencherInformacoesDeAuditoria(pQuestionario);
                bmQuestionario.Salvar(pQuestionario);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AlterarQuestionario(Questionario pQuestionario)
        {
            try
            {
                this.PreencherInformacoesDeAuditoria(pQuestionario);
                bmQuestionario.FazerMergeAoSalvar(pQuestionario);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void PreencherInformacoesDeAuditoria(Questionario pQuestionario)
        {
            base.PreencherInformacoesDeAuditoria(pQuestionario);
            pQuestionario.ListaQuestionarioAssociacao.ToList().ForEach(x => this.PreencherInformacoesDeAuditoria(x));
        }

        public IList<Questionario> ObterTodosQuestionarios()
        {
            return bmQuestionario.ObterTodos();
        }

        public IQueryable<Questionario> ObterTodosIQueryable()
        {
            return bmQuestionario.ObterTodosIQueryable();
        }

        public IQueryable<Questionario> ObterQuestionariosCancelamento(CategoriaConteudo categoria = null)
        {
            return bmQuestionario.ObterQuestionariosCancelamento(categoria);
        }

        public IQueryable<Questionario> ObterQuestionariosAbandono(CategoriaConteudo categoria = null)
        {
            return bmQuestionario.ObterQuestionariosAbandono(categoria);
        }

        private IList<Questionario> ObterQuestionariosPorFiltro(enumTipoQuestionario tipoQuestionario,
            int? idCategoria = null)
        {
            return bmQuestionario.ObterQuestionariosPorFiltro(tipoQuestionario, idCategoria);
        }

        public IList<Questionario> ObterQuestionariosDeAvaliacaoProva()
        {
            return this.ObterQuestionariosPorFiltro(enumTipoQuestionario.AvaliacaoProva);
        }

        public IList<Questionario> ObterQuestionariosDePesquisaPorCategoria(int idCategoria)
        {
            return bmQuestionario.ObterQuestionariosPorCategoria(enumTipoQuestionario.Pesquisa, idCategoria);
        }

        public IList<Questionario> ObterTodosPorCategoria(int idCategoria)
        {
            return bmQuestionario.ObterTodosPorCategoria(idCategoria);
        }

        public IList<Questionario> ObterQuestionariosPorCategoriaGestor(enumTipoQuestionario? tipo, int idCategoria,
            Uf uf)
        {
            return tipo != null
                ? bmQuestionario.ObterQuestionariosPorCategoriaGestor(tipo, uf, idCategoria)
                : bmQuestionario.ObterQuestionariosPorCategoriaGestor(enumTipoQuestionario.Pesquisa, uf, idCategoria);
        }

        public IList<Questionario> ObterQuestionariosDePesquisaPorCategoriaGestor(int idCategoria, Uf uf)
        {
            return bmQuestionario.ObterQuestionariosPorCategoriaGestor(enumTipoQuestionario.Pesquisa, uf, idCategoria);
        }

        public IList<Questionario> ObterQuestionariosDePesquisa()
        {
            return ObterQuestionariosPorFiltro(enumTipoQuestionario.Pesquisa);
        }

        public Questionario ObterQuestionarioPorID(int pId)
        {
            return bmQuestionario.ObterPorId(pId);
        }

        public void ExcluirQuestionario(int idQuestionario, Usuario usuarioLogado)
        {
            try
            {
                Questionario questionario = null;

                if (idQuestionario > 0)
                {
                    questionario = bmQuestionario.ObterPorId(idQuestionario);
                }

                if (questionario == null)
                    throw new AcademicoException("Questionário não encontrado");

                // Verificar se o gestor logado tem permissão para remover o questionário.
                if (usuarioLogado.IsGestor() && (questionario.Uf == null || questionario.Uf.ID != usuarioLogado.UF.ID))
                    throw new AcademicoException("Você não tem permissão para remover este questionário");

                bmQuestionario.Excluir(questionario);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public IList<Questionario> ObterQuestionarioPorFiltro(Questionario pQuestionario)
        {
            return bmQuestionario.ObterPorFiltro(pQuestionario);
        }

        public void IncluirQuestionarioPermissao(List<QuestionarioPermissao> questionarioPermissao,
            Questionario questionario)
        {
            foreach (var item in questionarioPermissao)
            {
                item.Questionario.ID = questionario.ID;
                new BMQuestionarioPermissao().Salvar(item);
            }
        }

        public IQueryable<Questionario> ObterQuestionarioPorUf(Usuario usuarioLogado)
        {
            var query =
                bmQuestionario.ObterTodosIQueryable()
                    .Where(x => x.TipoQuestionario.ID != (int) enumTipoQuestionario.Dinamico);

            if (usuarioLogado.IsGestor())
            {
                var idUf = usuarioLogado.UF.ID;

                query =
                    query.Where(
                        x =>
                            // Busca tanto pela UF vinculada como pela UF em permissões. I Know Right! This is just messed up!
                            (x.Uf == null || (x.Uf != null && x.Uf.ID == idUf)) ||
                            x.ListaQuestionarioPermissao.Any(p => p.Uf != null && p.Uf.ID == idUf));
            }

            return query;
        }

        public void Salvar(Questionario questionario)
        {
            bmQuestionario.Salvar(questionario);
        }

        public Questionario DuplicarQuestionario(Questionario questionarioOriginal, bool salvar = true)
        {
            var questionario = questionarioOriginal.NovoQuestionario(questionarioOriginal);

            if (questionario == null)
                return null;

            var novoQuestionario = new Questionario
            {
                TipoQuestionario = questionario.TipoQuestionario,
                Nome = questionario.Nome + " - V2",
                PrazoMinutos = questionario.PrazoMinutos,
                QtdQuestoesProva = questionario.QtdQuestoesProva,
                TextoEnunciado = questionario.TextoEnunciado
            };

            novoQuestionario.TipoQuestionario = questionario.TipoQuestionario;
            novoQuestionario.Nome = questionario.Nome;
            novoQuestionario.Descricao = questionario.Descricao;

            foreach (var itemQuestionario in questionario.ListaItemQuestionario.ToList())
            {
                var novoItemQuestionario = new ItemQuestionario
                {
                    Questionario = novoQuestionario,
                    InAvaliaProfessor = itemQuestionario.InAvaliaProfessor,
                    TipoItemQuestionario = itemQuestionario.TipoItemQuestionario,
                    EstiloItemQuestionario = itemQuestionario.EstiloItemQuestionario,
                    Feedback = itemQuestionario.Feedback,
                    NomeGabarito = itemQuestionario.NomeGabarito,
                    Questao = itemQuestionario.Questao,
                    Comentario = itemQuestionario.Comentario,
                    ValorQuestao = itemQuestionario.ValorQuestao
                };

                foreach (var opcao in itemQuestionario.ListaItemQuestionarioOpcoes.ToList())
                {
                    var novoItemQuestionarioOpcao = new ItemQuestionarioOpcoes
                    {
                        ItemQuestionario = novoItemQuestionario,
                        Nome = opcao.Nome,
                        RespostaCorreta = opcao.RespostaCorreta,
                        TipoDiagnostico = opcao.TipoDiagnostico
                    };

                    novoItemQuestionario.ListaItemQuestionarioOpcoes.Add(novoItemQuestionarioOpcao);
                }

                novoQuestionario.ListaItemQuestionario.Add(novoItemQuestionario);
            }

            foreach (var permissao in questionario.ListaQuestionarioPermissao)
            {
                var novaPermissao = new QuestionarioPermissao
                {
                    Descricao = permissao.Descricao,
                    FormaAquisicao = permissao.FormaAquisicao,
                    NivelOcupacional = permissao.NivelOcupacional,
                    Nome = permissao.Nome,
                    Perfil = permissao.Perfil,
                    Questionario = novoQuestionario,
                    Uf = permissao.Uf
                };

                novoQuestionario.ListaQuestionarioPermissao.Add(novaPermissao);
            }

            novoQuestionario.Uf = questionarioOriginal.Uf;

            if (salvar)
                Salvar(novoQuestionario);

            return novoQuestionario;
        }

        /// <summary>
        /// Filtra os questionários por tipo
        /// </summary>
        /// <param name="tipos">Lista de questionários do tipo enumTipoQuestionario</param>
        /// <returns>Lista de questionários com todos os tipos filtrados</returns>
        public IList<Questionario> ObterQuestionariosTipo(IEnumerable<enumTipoQuestionario> tipos)
        {
            var questionarios =  bmQuestionario.ObterTodosIQueryable();
            var questionariosFiltrados = new List<Questionario>();

            foreach (var tipo in tipos)
            {
                questionariosFiltrados.AddRange(questionarios.Where(x => x.TipoQuestionario != null && x.TipoQuestionario.ID == (int)tipo).ToList()); 
            }

            return questionariosFiltrados.ToList();
        }

        public IQueryable<Questionario> ObterQuestionariosItemTrilha()
        {
            return
                bmQuestionario.ObterTodosIQueryable()
                    .Where(x => x.TipoQuestionario.ID == (int)enumTipoQuestionario.AtividadeTrilha && x.Ativo == true);
        }

        public int QuantidadeQuestionariosAgrupados(Questionario questionario)
        {
            return questionario.ListaItemQuestionario.Count(x => x.TipoItemQuestionario.ID == (int)enumTipoItemQuestionario.AgrupadorDeQuestoes);
        }

        public string MapearJsonQuestionario(Questionario questionario)
        {
            QuestionarioPermissao permissaoFormaAquisicao;

            var questionarioDto = new DTOQuestionario
            {
                Id = questionario.ID,
                Nome = questionario.Nome,
                PrazoMinutos = questionario.PrazoMinutos,
                QtdQuestoesProva = questionario.QtdQuestoesProva,
                NotaMinima = questionario.NotaMinima,
                TextoEnunciado = questionario.TextoEnunciado,
                TipoQuestionario = new DTOTipoQuestionario
                {
                    Id = questionario.TipoQuestionario.ID,
                    Nome = questionario.TipoQuestionario.Nome
                },
                FormaAquisicaoId =
                    (permissaoFormaAquisicao =
                        questionario.ListaQuestionarioPermissao.FirstOrDefault(x => x.FormaAquisicao != null)) != null
                        ? (int?)permissaoFormaAquisicao.FormaAquisicao.ID
                        : null,
                Ativo = questionario.Ativo,
                ListaPerfil =
                    questionario.ListaQuestionarioPermissao.Where(x => x.Perfil != null)
                        .Select(x => new { x.Perfil.ID })
                        .Select(x => x.ID)
                        .ToList(),
                ListaNivelOcupacional =
                    questionario.ListaQuestionarioPermissao.Where(x => x.NivelOcupacional != null)
                        .Select(x => new { x.NivelOcupacional.ID })
                        .Select(x => x.ID)
                        .ToList(),
                ListaUf =
                    questionario.ListaQuestionarioPermissao.Where(x => x.Uf != null)
                        .Select(x => new { x.Uf.ID })
                        .Select(x => x.ID)
                        .ToList(),
                ListaCategoriaConteudo = questionario.ListaCategoriaConteudo.Select(c => new DTOCategoriaConteudo
                {
                    ID = c.ID,
                    Nome = c.Nome
                }).ToList(),
                ListaItemQuestionario =
                    questionario.ListaItemQuestionario.Select(x => new DTOItemQuestionario
                    {
                        Comentario = x.Comentario,
                        EstiloItemQuestionario = new DTOEstiloItemQuestionario
                        {
                            ID = x.EstiloItemQuestionario != null ? x.EstiloItemQuestionario.ID : 0,
                            Nome = x.EstiloItemQuestionario != null ? x.EstiloItemQuestionario.Nome : ""
                        },
                        ExibeFeedback = x.ExibeFeedback,
                        Feedback = x.Feedback,
                        IdQuestionario = x.Questionario.ID,
                        InAvaliaProfessor = x.InAvaliaProfessor,
                        ListaItemQuestionarioOpcoes = x.ListaItemQuestionarioOpcoes.Select(o => new DTOItemQuestionarioOpcoes
                        {
                            ID = o.ID,
                            Nome = o.Nome,
                            ItemQuestionario = new DTOItemQuestionario
                            {
                                ID = o.ItemQuestionario.ID,
                                NomeGabarito = o.ItemQuestionario.NomeGabarito,
                                ValorQuestao = o.ItemQuestionario.ValorQuestao,
                                TipoItemQuestionario = new DTOTipoItemQuestionario
                                {
                                    ID = o.ItemQuestionario.TipoItemQuestionario.ID,
                                     Nome = o.ItemQuestionario.TipoItemQuestionario.Nome
                                }
                            },
                            OpcaoInt = o.OpcaoInt,
                            OpcaoVinculada = o.OpcaoVinculada != null ? new DTOItemQuestionarioOpcoes
                            {
                                ID = o.OpcaoVinculada.ID
                            } : null,
                            RespostaCorreta = o.RespostaCorreta,
                            TipoDiagnostico = (int)o.TipoDiagnostico,
                        }).ToList(),
                        Ordem = x.Ordem,
                        Questao = x.Questao,
                        NomeGabarito = x.NomeGabarito,
                        ValorQuestao = x.ValorQuestao,
                        TipoItemQuestionario = new DTOTipoItemQuestionario
                        {
                            ID = x.TipoItemQuestionario.ID,
                            Nome = x.TipoItemQuestionario.Nome
                        },
                        QuestionarioEnunciado = new DTOItemQuestionario
                        {
                            ID = x.Questionario.ID,
                            IdQuestionario = x.QuestionarioEnunciado != null && x.QuestionarioEnunciado.Questionario != null ? x.QuestionarioEnunciado.Questionario.ID : 0
                        },
                        Questionario = new DTOQuestionario
                        {
                            Nome = x.Questionario.Nome,
                            Id = x.Questionario.ID
                        }
                    }).ToList(),
                ListaQuestionarioPermissao = questionario.ListaQuestionarioPermissao.Select(p => new DTOQuestionarioPermissao
                {
                    FormaAquisicao = p.FormaAquisicao != null ? new DTOFormaAquisicao
                    {
                        ID = p.FormaAquisicao.ID,
                        Nome = p.FormaAquisicao.Nome
                    } : null,
                    NivelOcupacional = p.NivelOcupacional != null ? new DTONivelOcupacional
                    {
                        ID = p.NivelOcupacional.ID,
                        Nome = p.NivelOcupacional.Nome
                    } : null,
                    Perfil = p.Perfil != null ? new DTOPerfil
                    {
                        ID = p.Perfil.ID,
                        Nome = p.Perfil.Nome
                    } : null,
                    Uf = p.Uf != null ? new DTOUf
                    {
                        ID = p.Uf.ID,
                        Nome = p.Uf.Nome
                    } : null
                }).ToList()
            };

            return new JavaScriptSerializer().Serialize(questionarioDto);
        }
    }
}