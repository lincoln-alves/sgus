using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.BP.Relatorios;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Util.Classes;
using RelatoriosHelper = Sebrae.Academico.BP.Helpers.RelatoriosHelper;

namespace Sebrae.Academico.WebForms.UserControls
{
    public partial class ucRelatorioQuestionario : UserControl
    {
        public ucRelatorioQuestionario()
        {
            // Padrões do relatório.
            MaxPerPage = 50;
        }

        private int MaxPerPage { get; set; }

        /// <summary>
        /// Página atual do relatório respondente.
        /// </summary>
        public int PaginaAtual
        {
            get
            {
                int id;
                return (int.TryParse(ViewState["_PaginaAtual"].ToString(), out id) ? id : 1);
            }
            set
            {
                ViewState["_PaginaAtual"] = value;
            }
        }

        // Define o teor do relatório de acordo com o especificado quando chamar o UserControl.
        public bool RelatorioTutor { get; set; }
        

        protected void Page_Load(object sender, EventArgs e)
        {
            // Persistir o preenchimento das soluções quando o relatório for Consolidado.
            if (ObterTipoRelatorioSelecionado() == enumTipoRelatorioResposta.Consolidado)
                ucCategorias1.TreeNodeCheckChanged += PreencherCombos;

            if (IsPostBack) return;

            // No primeiro load, define a página atual como 0.
            PaginaAtual = 1;

            using (var rel = new RelatorioQuestionario())
            {
                PreencherCombos(null, null);

                ucCategorias1.PreencherCategorias(false);

                if (RelatorioTutor)
                {
                    divDemandas.Visible =
                    divTipoQuestionario.Visible =
                    divQuestionario.Visible = false;

                    // Relatório de tutor sempre será de pesquisa.
                    ddlTipoQuestionario.SelectedValue = ((int) enumTipoQuestionario.Pesquisa).ToString();

                    divTutor.Visible = true;

                    WebFormHelper.PreencherLista(rel.ListaProfessor(), cbxProfessor, false, true);

                    txtSolucaoEducacional.Attributes.Add("data-mensagemVazia", "Selecione um tutor");
                }

                ListBoxesUF.PreencherItens(rel.ListaUf(), "ID", "Nome");
                ListBoxesNivelOcupacional.PreencherItens(rel.ListaNivelOcupacional(), "ID", "Nome");

                var lista = rel.ListaSolucaoEducacional();
                ViewState["_SE"] = Helpers.Util.ObterListaAutocomplete(lista);
            }
        }

        private void PreencherFiltros(enumTipoRelatorioResposta tipo)
        {
            chkListaCamposVisiveis.Items.Clear();

            switch (tipo)
            {
                case enumTipoRelatorioResposta.Consolidado:

                    var listaFiltroConsolidado = new List<ListItem>
                    {
                        new ListItem("Solução Educacional", "NM_SolucaoEducacional"),
                        new ListItem("Oferta", "NM_Oferta"),
                        new ListItem("Turma", "NM_Turma"),
                        new ListItem("Data Início", "DT_Inicio"),
                        new ListItem("Data Final", "DT_Final"),
                        new ListItem("Quantidade de alunos na turma", "QtdeAlunosTurma"),
                        new ListItem("Quantidade de alunos que respondeu o questionário",
                            "QtdeAlunosResponderamQuestionario"),
                        new ListItem("Quantidade de alunos que chegaram ao final do curso (Aprovado, Concluído, etc)",
                            "QtdeAlunosFinalizaram"),
                        new ListItem(
                            "Percentual de alunos que responderam o questionário com relação ao total de alunos da turma",
                            "PctAlunosQueResponderamQuestionario"),
                        new ListItem(
                            "Percentual de alunos que responderam o questionário com relação aos que chegaram ao final do curso (Aprovado, Concluído, etc)",
                            "PctAlunosFinalizaramQueResponderamQuestionario")
                    };

                    WebFormHelper.PreencherListaCustomizado(listaFiltroConsolidado, chkListaCamposVisiveis, "Value",
                        "Text");

                    break;

                case enumTipoRelatorioResposta.Estatistico:

                    var listaFiltroEstatistico = new List<ListItem>
                    {
                        new ListItem("Categoria", "Principal"),
                        new ListItem("Tópico Avaliado", "Nome"),
                        new ListItem("Média", "Media"),
                        new ListItem("D.P", "DP"),
                        new ListItem("Moda", "Moda"),
                        new ListItem("Min", "Min"),
                        new ListItem("Max", "Max"),
                        new ListItem("Qtd de Itens", "QtdeItens"),
                        new ListItem("Média Final", "MediaFinal")
                    };

                    WebFormHelper.PreencherListaCustomizado(listaFiltroEstatistico, chkListaCamposVisiveis, "Value",
                        "Text");

                    break;

                case enumTipoRelatorioResposta.Respondente:

                    var listaFiltroRespondente = new List<ListItem>
                    {
                        new ListItem("Questionario", "Questionario"),
                        new ListItem("Curso", "Curso"),
                        new ListItem("Nome", "Nome"),
                        new ListItem("NivelOcupacional", "NivelOcupacional"),
                        new ListItem("UF", "UF"),
                        new ListItem("Data", "Data")
                    };

                    WebFormHelper.PreencherListaCustomizado(listaFiltroRespondente, chkListaCamposVisiveis, "Value",
                        "Text");
                    break;
            }

            SelecionarTodosFiltros();
        }

        private void SelecionarTodosFiltros()
        {
            foreach (ListItem item in chkListaCamposVisiveis.Items)
            {
                item.Selected = true;
            }
        }

        protected void cbxProfessor_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxProfessor.SelectedIndex > 0)
            {
                using (var rel = new RelatorioQuestionario())
                {
                    var lista =
                        Helpers.Util.ObterListaAutocomplete(rel.ListaSolucaoEducacionalPorProfessor(ObterFiltro()));

                    ViewState["_SE"] = lista;

                    txtOferta.Text = "";

                    LimparCampos();
                }
            }
        }

        protected void txtQuestionario_OnTextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtQuestionario.Text))
            {
                LimparCampos();
            }
            else
            {
                var idQuestionario = int.Parse(txtQuestionario.Text);

                if (idQuestionario == 0)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Atencao, "Questionário inválido selecionado.");
                }
                else
                {
                    using (var rel = new RelatorioQuestionario())
                    {
                        if (string.IsNullOrWhiteSpace(txtSolucaoEducacional.Text))
                        {
                            ViewState["_SE"] = Helpers.Util.ObterListaAutocomplete(rel.ListaSolucaoEducacional(idQuestionario));

                            txtSolucaoEducacional.Text = "";
                        }

                        LimparCampos();
                    }
                }
            }
        }

        protected void txtSolucaoEducacional_OnTextChanged(object sender, EventArgs e)
        {
            using (var rel = new RelatorioQuestionario())
            {
                var filtro = ObterFiltro();

                if (filtro.IdSolucaoEducacional != null)
                {
                    ViewState["_Oferta"] = Helpers.Util.ObterListaAutocomplete(rel.ListaOferta(filtro));    
                }

                txtOferta.Text = "";

                PreencherComboQuestionario();

                var listaQuestionarios = new RelatorioQuestionario().ListaQuestionario(ObterFiltro()).OrderBy(x => x.Nome).ToList();
                
                IList<TipoQuestionario> tiposQuestionario = null;

                if (listaQuestionarios.Any())
                {
                    var questionarioIDs = listaQuestionarios.Select(x => x.ID).ToList();
                    tiposQuestionario = new ManterTipoQuestionario().ObterTodosIQueryable()
                        .Where(x => x.ListaQuestionario.Any(y => questionarioIDs.Contains(y.ID)))
                        .OrderBy(x => x.Nome)
                        .ToList();
                }
                else
                {
                    tiposQuestionario = new ManterTipoQuestionario().ObterTodos().OrderBy(x => x.Nome).ToList();
                }

                WebFormHelper.PreencherLista(tiposQuestionario, ddlTipoQuestionario, true);
                
                
                LimparCampos();
            }
        }

        protected void txtOferta_OnTextChanged(object sender, EventArgs e)
        {
            using (var rel = new RelatorioQuestionario())
            {
                var filtro = ObterFiltro();

                if (filtro.IdOferta.HasValue)
                {
                    ViewState["_Turma"] = Helpers.Util.ObterListaAutocomplete(rel.ListaTurma(filtro));  
                }

                txtTurma.Text = "";

                PreencherComboQuestionario();

            }
        }

        protected void txtTurma_OnTextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtTurma.Text))
            {
                var idTurma = int.Parse(txtTurma.Text);

                if (idTurma > 0)
                {
                    var turmas = new ManterTurma().ObterTodasTurma().Where(x => x.ID == idTurma);

                    var questionariosAssociacaoIDs = turmas
                        .SelectMany(y => y.ListaQuestionarioAssociacao)
                        .Select(z => z.Questionario.ID)
                        .ToList();

                    var questionariosParticipacaoIDs = turmas
                        .SelectMany(y => y.ListaQuestionarioParticipacao)
                        .Select(z => z.Questionario.ID)
                        .ToList();

                    var listaProcesso = new ManterEtapa().ObterTodosIQueryable()
                        .Where(x => x.ListaCampos.Any(y => questionariosAssociacaoIDs.Contains(y.Questionario.ID) || 
                            questionariosParticipacaoIDs.Contains(y.Questionario.ID)))
                            .Select(z => z.Processo)
                            .OrderBy(y => y.Nome)
                            .ToList();

                    ViewState["_Demandas"] = Helpers.Util.ObterListaAutocomplete(listaProcesso);
                }

                txtDemandas.Text = "";

                PreencherComboQuestionario();
            }
            
        }

        private bool IsDadosNull()
        {
            bool isNull;

            switch (ObterTipoRelatorioSelecionado())
            {
                case enumTipoRelatorioResposta.Respondente:
                    isNull = Cache["dsRelatorioRespondente"] == null;
                    break;
                case enumTipoRelatorioResposta.Estatistico:
                    isNull = Cache["dsRelatorioEstatistico"] == null;
                    break;
                case enumTipoRelatorioResposta.Consolidado:
                    isNull = Cache["dsRelatorioConsolidado"] == null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return isNull;
        }

        /// <summary>
        /// Obter dados respondentes (perguntas e respostas dos usuários, agrupadas pelos nomes das questões.
        /// </summary>
        /// <returns></returns>
        public DTORelatorioQuestionarioRespondente ExecutarRelatorioRespondente(bool salvarEmChave = true)
        {
            using (var rel = new RelatorioQuestionario())
            {
                var dadosRespondentes = rel.ObterRelatorioRespondente(ObterFiltro());

                // Armazenar em Cache para ser utilizado posteriormente, caso necessário.
                if (salvarEmChave)
                {
                    Cache["dsRelatorioRespondente"] = dadosRespondentes;
                    Cache["camposSeremExibidosRelatorioRespondente"] =
                        chkListaCamposVisiveisRespondente.Items.Cast<ListItem>()
                            .Where(x => x.Selected)
                            .Select(x => x.Value)
                            .ToList();
                }

                // Obter todos os questionários com as respostas dos usuários.
                return dadosRespondentes;
            }
        }

        /// <summary>
        /// Buscar dados estatísticos, a partir dos dados respondentes.
        /// </summary>
        public List<DTORelatorioQuestionarioEstatistico> ExecutarRelatorioEstatistico()
        {
            try
            {
                using (var rel = new RelatorioQuestionario())
                {
                    // Obtém primeiro os dados respondentes para calcular os dados estatísticos.
                    var dadosRespondentes = ExecutarRelatorioRespondente(false);

                    var resultado = rel.ObterRelatorioEstatistico(dadosRespondentes.Enunciados,
                        dadosRespondentes.Questoes, dadosRespondentes.Consulta);

                    Cache["dsRelatorioEstatistico"] = resultado;

                    return resultado;
                }
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, ex.Message);
            }
            catch
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta,
                    "Resultado não pode ser processado, por favor adicione mais filtros.");
            }

            return new List<DTORelatorioQuestionarioEstatistico>();
        }

        /// <summary>
        /// Buscar dados consolidados, com resumo do curso.
        /// </summary>
        /// <returns></returns>
        public IList<DTOQuestionarioConsolidado> ExecutarRelatorioQuestionarioConsolidado()
        {
            using (var rel = new RelatorioQuestionarioReacao())
            {
                var lstRelatorio = rel.ObterQuestionarioReacao(ObterFiltro());

                Cache["dsRelatorioConsolidado"] = lstRelatorio;

                return lstRelatorio;
            }
        }

        private List<QuestionarioParticipacao> ParticipacoesPaginaAtual(IQueryable<QuestionarioParticipacao> consulta)
        {
            if (PaginaAtual > 1)
                // Remover 1 da PaginaAtual porque o Skip é zero-index.
                consulta = consulta.Skip((PaginaAtual - 1) *MaxPerPage);

            return consulta.Take(MaxPerPage).ToList();
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                var exibirExportacaoRelatorio = false;

                // Esconder dados que serão disponibilizados de acordo com o tipo de questionário.
                divPaginacaoRespondente.Visible =
                divRelatorioRespondente.Visible =
                divRelatorioConsolidado.Visible =
                divRelatorioEstatistico.Visible =
                componenteGeracaoRelatorio.Visible = false;

                // Resetar página atual.
                PaginaAtual = 1;

                switch (ObterTipoRelatorioSelecionado())
                {
                    case enumTipoRelatorioResposta.Respondente:
                        try
                        {
                            ValidarCamposRespondente();

                            var resultadoRespondente = ExecutarRelatorioRespondente();

                            GerarDadosRespondentes(resultadoRespondente);

                            if (resultadoRespondente.TotalRespostas > 0)
                            {
                                divRelatorioRespondente.Visible = true;

                                // Caso há respostas, exibir a exportação do relatório.
                                exibirExportacaoRelatorio = true;

                                // Totalizador simples exibindo somente a quantidade de respostas total.
                                ucTotalizadorRelatorio.PreencherTabela(new List<DTOTotalizador>
                                {
                                    TotalizadorUtil.GetTotalizadorSimples("Total de respostas", resultadoRespondente.TotalRespostas)
                                });

                                // Exibir paginação caso haja mais dados do que o número máximo por página.
                                if (resultadoRespondente.TotalRespostas > MaxPerPage)
                                    divPaginacaoRespondente.Visible = true;
                            }
                            else
                            {
                                WebFormHelper.ExibirMensagem(enumTipoMensagem.Atencao, "Não há dados a serem exibidos.");
                            }

                        }
                        catch (AcademicoException ex)
                        {
                            WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                        }

                        break;

                    case enumTipoRelatorioResposta.Estatistico:
                        divRelatorioEstatistico.Visible = true;

                        var resultadoEstatistico = ExecutarRelatorioEstatistico();

                        WebFormHelper.ValidarVisibilidadeCamposGrid(grdRelatorioEstatistico,
                            chkListaCamposVisiveis.Items);

                        grdRelatorioEstatistico.DataSource = resultadoEstatistico;
                        grdRelatorioEstatistico.DataBind();

                        if (resultadoEstatistico.Any())
                            exibirExportacaoRelatorio = true;

                        break;

                    case enumTipoRelatorioResposta.Consolidado:
                        ValidarCamposRespondente();
                        divRelatorioConsolidado.Visible = true;

                        var resultadoConsolidado = ExecutarRelatorioQuestionarioConsolidado();

                        WebFormHelper.ValidarVisibilidadeCamposGrid(dgRelatorioConsolidado, chkListaCamposVisiveis.Items);

                        dgRelatorioConsolidado.DataSource = resultadoConsolidado;
                        dgRelatorioConsolidado.DataBind();

                        if (resultadoConsolidado.Any())
                            exibirExportacaoRelatorio = true;

                        break;
                }

                if (exibirExportacaoRelatorio)
                    componenteGeracaoRelatorio.Visible = true;
            }
            catch (AcademicoException academicoException)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, academicoException.Message);
            }
        }

        private void GerarDadosRespondentes(DTORelatorioQuestionarioRespondente resultadoRespondente = null)
        {
            resultadoRespondente = resultadoRespondente ??
                                   (Cache["dsRelatorioRespondente"] as DTORelatorioQuestionarioRespondente);

            if(resultadoRespondente == null)
                throw new AcademicoException("Relatório respondente não encontrado para exibição. Tente novamente");

            // Dados padrões da consulta.
            var enunciados = resultadoRespondente.Enunciados.OrderBy(x => x.Id).ToList();
            var questoes = resultadoRespondente.Questoes.OrderBy(x => x.IdEnunciado).ThenBy(x => x.Id).ToList();

            // As respostas não recebem as respostas diretamente. Elas recebem DTOs com as respostas.
            var dtoRespostas = RelatorioQuestionario.ConverterDtoRespostas(
                ParticipacoesPaginaAtual(resultadoRespondente.Consulta),
                questoes);

            rptEnunciados.DataSource = enunciados;
            rptQuestoes.DataSource = questoes;
            rptRespostas.DataSource = dtoRespostas;


            // Glory Bind.
            rptEnunciados.DataBind();
            rptQuestoes.DataBind();
            rptRespostas.DataBind();
        }

        protected void btnGerarRelatorio_Click(object sender, EventArgs e)
        {
            if (IsDadosNull())
                btnPesquisar_Click(null, null);

            var requestUrl = new ManterConfiguracaoSistema().ObterConfiguracaoSistemaPorID(
                (int)enumConfiguracaoSistema.EnderecoSGUS).Registro +
                             "/Relatorios/Questionario/RelatorioQuestionario.aspx?relatorio=" +
                             rblTipoRelatorio.SelectedValue;

            var quantidadeRegistro = 1;

            switch (rblTipoRelatorio.SelectedValue.ToLower())
            {
                case "respondente":
                    quantidadeRegistro = ((DTORelatorioQuestionarioRespondente)Cache["dsRelatorioRespondente"]).TotalRespostas;
                    break;
                case "estatistico":
                    quantidadeRegistro =
                        ((List<DTORelatorioQuestionarioEstatistico>)Cache["dsRelatorioEstatistico"]).Count();

                    break;
            }

            var nomeAmigavel = "Questionário " + rblTipoRelatorio.SelectedValue.ToLower() + " de " + (RelatorioTutor ? "tutor" : "pesquisa");

            var nomeRelatorio = "questionario" + (RelatorioTutor ? "Tutor" : "Pesquisa") + rblTipoRelatorio.SelectedValue;

            RelatoriosHelper.ExecutarThreadSolicitacaoRelatorioRequisicao(requestUrl, enumTipoSaidaRelatorio.EXCEL,
                nomeRelatorio, nomeAmigavel, quantidadeRegistro);
        }

        private void ValidarCamposRespondente()
        {
            if (RelatorioTutor)
            {
                if (cbxProfessor.SelectedIndex == 0)
                    throw new AcademicoException("O tutor é obrigatório em pesquisa por tutor.");

                if (string.IsNullOrWhiteSpace(txtTurma.Text))
                    throw new AcademicoException("A turma é obrigatória em pesquisa por tutor.");
            }

        }

        protected void rptEnunciados_OnItemDataBound(object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var item = (DTORelatorioQuestionarioEnunciado)e.Item.DataItem;
                var thCabecalho = (Literal)e.Item.FindControl("ThCabecalho");

                // Criar HTML da linha dos enunciados.
                if (!string.IsNullOrEmpty(item.Nome))
                {
                    thCabecalho.Text = "<th class=\"text-center\" colspan=\"" + item.QuestoesRelacionadas.Count() + "\">" +
                                       item.Nome + "</th>";
                }
            }
        }

        protected DTOFiltroRelatorioQuestionario ObterFiltro()
        {
            int id;
            Questionario questionario;

            return new DTOFiltroRelatorioQuestionario
            {
                IdProcesso = int.TryParse(txtDemandas.Text, out id) && id != 0 ? (int?) id : null,
                IdTipoQuestionario =
                    int.TryParse(ddlTipoQuestionario.SelectedValue, out id) && id != 0 ? (int?) id : null,
                IdSolucaoEducacional = int.TryParse(txtSolucaoEducacional.Text, out id) && id != 0 ? (int?) id : null,
                IdOferta = int.TryParse(txtOferta.Text, out id) && id != 0 ? (int?) id : null,
                IdTurma = int.TryParse(txtTurma.Text, out id) && id != 0 ? (int?) id : null,

                // Caso seja o relatório de tutor, busca o questionário pós de pesquisa da turma informada na tela.
                // Como o id da turma foi setado acima, a variável id permanece com o id da turma e pode ser usado.
                IdQuestionario =
                    RelatorioTutor && id != 0
                        ? (questionario = new ManterQuestionarioAssociacao().ObterPesquisaPosTurma(id)) != null ? questionario.ID : 0
                        : int.TryParse(txtQuestionario.Text, out id) && id != 0 ? id : 0,

                IdsUf = ListBoxesUF.RecuperarIdsSelecionados<int>().ToList(),
                IdsNivelOcupacional = ListBoxesNivelOcupacional.RecuperarIdsSelecionados<int>().ToList(),
                IdsStatusMatricula = ListBoxesStatus.RecuperarIdsSelecionados<int>().ToList(),
                IdsCategorias = ucCategorias1.IdsCategoriasMarcadas.ToList(),
                IsRelatorioTutor = RelatorioTutor,
                IdProfessor = int.TryParse(cbxProfessor.SelectedValue, out id) && id != 0 ? (int?) id : null
            };
        }

        private void LimparCampos()
        {
            // Nâo limpar as soluções educacionais caso seja relatório consolidado,
            // pois este tipo de relatório obtém as SEs separadas da seleção de questionário.
            if (string.IsNullOrWhiteSpace(txtSolucaoEducacional.Text))
            {
                txtOferta.Text = "";
                ViewState["_Oferta"] = null;
            }

            if (string.IsNullOrWhiteSpace(txtOferta.Text))
            {
                txtTurma.Text = "";
                ViewState["_Turma"] = null;
            }
        }

        protected void rblTipoRelatorio_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            // Esconde todos os campos e os exibe de acordo com o tipo de relatório.
            divDemandas.Visible =
            divTipoQuestionario.Visible =
            divQuestionario.Visible =
            divRelatorioRespondente.Visible =
            divRelatorioEstatistico.Visible =
            divRelatorioConsolidado.Visible =
            divStatus.Visible = 
            divUf.Visible =
            divNivelOcupacional.Visible = false;

            // Limpar o evento e o postback da seleção da categoria, pois só serve para o consolidado.
            ucCategorias1.TreeNodeCheckChanged = null;

            ddlTipoQuestionario.Enabled = true;

            var tipoRelatorioSelecionado = ObterTipoRelatorioSelecionado();

            // Preencher os filtros personalizados de cada tipo de relatório.
            PreencherFiltros(tipoRelatorioSelecionado);

            switch (tipoRelatorioSelecionado)
            {
                case enumTipoRelatorioResposta.Respondente:
                    filtroCamposExibidos.Visible = false;

                    // Limpar e exibir drop de questionários.
                    txtQuestionario.Text = "";
                    LimparCampos();

                    // Só exibe os campos abaixo se não for relatório de tutor.
                    divDemandas.Visible =
                    divTipoQuestionario.Visible =
                    divQuestionario.Visible = !RelatorioTutor;

                    divStatus.Visible = true;
                    divUf.Visible = true;
                    divNivelOcupacional.Visible = true;

                    break;

                case enumTipoRelatorioResposta.Estatistico:
                    filtroCamposExibidos.Visible = true;

                    // O relatório consolidado só funciona para relatórios do tipo de Pesquisa.
                    ddlTipoQuestionario.Enabled = false;
                    ddlTipoQuestionario.SelectedValue = ((int) enumTipoQuestionario.Pesquisa).ToString();

                    LimparCampos();

                    // Limpar e exibir drop de questionários.
                    txtQuestionario.Text = "";

                    PreencherComboQuestionario();
                    
                    // Só exibe os campos abaixo se não for relatório de tutor.
                    divDemandas.Visible =
                    divTipoQuestionario.Visible =
                    divQuestionario.Visible = !RelatorioTutor;

                    divStatus.Visible = true;
                    divUf.Visible = true;
                    divNivelOcupacional.Visible = true;

                    break;

                case enumTipoRelatorioResposta.Consolidado:
                    ucCategorias1.TreeNodeCheckChanged = PreencherCombos;

                    filtroCamposExibidos.Visible = true;

                    // Exibir diretamente a lista de Soluções Educacionais, sem precisar da listagem de questionários.
                    using (var rel = new RelatorioQuestionario())
                    {
                        var lista = rel.ListaSolucaoEducacional();

                        ViewState["_SE"] = Helpers.Util.ObterListaAutocomplete(lista);

                        txtSolucaoEducacional.Text = "";

                        LimparCampos();
                    }

                    break;
            }

            // Forçar a recriação das categorias.
            ucCategorias1.PreencherCategorias(false);
        }

        protected void PreencherCombos(object sender, EventArgs e)
        {
            using (var rel = new RelatorioQuestionario())
            {
                ViewState["_Demandas"] = Helpers.Util.ObterListaAutocomplete(rel.ListaProcessos());

                switch (ObterTipoRelatorioSelecionado())
                {
                    case enumTipoRelatorioResposta.Respondente:
                    case enumTipoRelatorioResposta.Estatistico:

                        PreencherComboQuestionario();

                        ListBoxesStatus.PreencherItens(rel.ListaStatus(), "ID", "Nome");

                        if (ObterTipoRelatorioSelecionado() == enumTipoRelatorioResposta.Estatistico)
                            // Relatório estatístico sempre será de pesquisa.
                            ddlTipoQuestionario.SelectedValue = ((int) enumTipoQuestionario.Pesquisa).ToString();
                        else
                            WebFormHelper.PreencherLista(new ManterTipoQuestionario().ObterTodos().OrderBy(x => x.Nome).ToList(), ddlTipoQuestionario,
                                true);

                        txtQuestionario.Text = "";

                        LimparCampos();

                        break;
                    case enumTipoRelatorioResposta.Consolidado:
                        var listaSe = rel.ListaSolucaoEducacionalPorCategorias(ucCategorias1.IdsCategoriasMarcadas);

                        ViewState["_SE"] = Helpers.Util.ObterListaAutocomplete(listaSe);

                        txtSolucaoEducacional.Text = "";

                        LimparCampos();

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        protected void PreencherComboQuestionario()
        {
            using (var rel = new RelatorioQuestionario())
            {
                var listaQuestionarios = rel.ListaQuestionario(ObterFiltro()).OrderBy(x => x.Nome).ToList();

                if (string.IsNullOrWhiteSpace(txtQuestionario.Text))
                {
                    ViewState["_Questionario"] = Helpers.Util.ObterListaAutocomplete(listaQuestionarios);

                    txtQuestionario.Text = "";
                }

                var idTipoQuestionario = 0;
                if (!int.TryParse(ddlTipoQuestionario.SelectedValue, out idTipoQuestionario)) { 

                    IList<TipoQuestionario> tiposQuestionario = null;

                    if (listaQuestionarios.Any())
                    {
                        var questionarioIDs = listaQuestionarios.Select(x => x.ID).ToList();
                        tiposQuestionario = new ManterTipoQuestionario().ObterTodosIQueryable()
                            .Where(x => x.ListaQuestionario.Any(y => questionarioIDs.Contains(y.ID)))
                            .OrderBy(x => x.Nome)
                            .ToList();
                    }
                    else
                    {
                        tiposQuestionario = new ManterTipoQuestionario().ObterTodos().OrderBy(x => x.Nome).ToList();
                    }

                    WebFormHelper.PreencherLista(tiposQuestionario, ddlTipoQuestionario, true);
                }
                    
            }
        }

        protected void dgRelatorioConsolidado_OnSorting(object sender, GridViewSortEventArgs e)
        {
            WebFormHelper.OrdenarListaGrid(Cache["dsRespostas"] as IList<DTOSolucaoEducacionalFormaAquisicao>,
                dgRelatorioConsolidado, e.SortExpression, e.SortDirection, "dsRespostas");
        }

        protected void dgRelatorioConsolidado_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            WebFormHelper.PaginarGrid(
                Cache["dsRespostas"] as IList<DTOSolucaoEducacionalFormaAquisicao>, dgRelatorioConsolidado,
                e.NewPageIndex);
        }

        private enumTipoRelatorioResposta ObterTipoRelatorioSelecionado()
        {
            switch (rblTipoRelatorio.SelectedValue.ToLower())
            {
                case "respondente":
                    return enumTipoRelatorioResposta.Respondente;
                case "estatistico":
                    return enumTipoRelatorioResposta.Estatistico;
                case "consolidado":
                    return enumTipoRelatorioResposta.Consolidado;
                default:
                    throw new AcademicoException("Tipo de relatório inválido.");
            }
        }

        private bool ChecarExibirItem(string nome, RepeaterItemEventArgs e)
        {
            if (chkListaCamposVisiveisRespondente.Items.FindByValue(nome).Selected == false)
            {
                e.Item.FindControl(nome).Visible = false;
                return false;
            }

            return true;
        }

        protected void rptRespostas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            ChecarExibirItem("Questionario", e);
            ChecarExibirItem("Curso", e);
            ChecarExibirItem("Nome", e);
            ChecarExibirItem("NivelOcupacional", e);
            ChecarExibirItem("UF", e);
            ChecarExibirItem("Data", e);

            var participacao = (DTORelatorioQuestionarioParticipacao)e.Item.DataItem;

            var rptNotasTutor = (Repeater)e.Item.FindControl("rptNotasTutor");

            var listaQuestoes = participacao.Respostas.Select(x => x.Questao).OrderBy(x => x.IdEnunciado).ToList();
            var listaRespostas = participacao.Respostas.OrderBy(x => x.Questao.IdEnunciado).ThenBy(x => x.Questao.Id).ToList();

            var respostas = listaQuestoes.OrderBy(x => x.Id).Select(itemQuestao => new DTORelatoriQuestaoRespostas
            {
                Questao = itemQuestao,
                Respostas = listaRespostas.Where(itemRespostas => 
                    itemRespostas.Questao.Id == itemQuestao.Id
                    && itemRespostas.IdProfessor == itemQuestao.IdProfessor).ToList()
            }).ToList();
            
            rptNotasTutor.DataSource = respostas;
            rptNotasTutor.DataBind();
        }

        protected void rptNotasTutor_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var Questao = (DTORelatoriQuestaoRespostas)e.Item.DataItem;

            var rptNotas = (Repeater)e.Item.FindControl("rptNotas");
            // Obter as questões por aqui.
            rptNotas.DataSource = Questao.Respostas;
            rptNotas.DataBind();
        }

        protected void txtDemandas_OnTextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDemandas.Text))
            {
                LimparCampos();
                PreencherCombos(null, null);
            }
            else
            {
                var idProcesso = int.Parse(txtDemandas.Text);

                if (idProcesso == 0)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Atencao, "Demanda inválida selecionada.");
                }
                else
                {   
                    PreencherComboQuestionario();

                    txtSolucaoEducacional.Text = "";
                    LimparCampos();   
                }
            }
        }

        protected void rptPaginas_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var item = (DTOPaginacao)e.Item.DataItem;
                var link = (HtmlAnchor)e.Item.FindControl("linkPagina");
                var li = (HtmlGenericControl)e.Item.FindControl("liPagina");

                link.Attributes["data-pagina"] = item.Numero.ToString();

                li.Attributes["class"] = item.IsPaginaAtual ? "active" : "";
            }
        }

        protected void divPaginacaoRespondente_OnPreRender(object sender, EventArgs e)
        {
            var resultadoRespondente = (Cache["dsRelatorioRespondente"] as DTORelatorioQuestionarioRespondente);

            if (resultadoRespondente != null)
            {
                // Quantidade de páginas
                var qntPaginas = ObterQuanidadePaginas(resultadoRespondente);

                if (divPaginacaoRespondente.Visible)
                {
                    if (PaginaAtual == 1)
                    {
                        liPaginaAnterior.Attributes["class"] = "disabled";
                        // Remove evento do click do botão de página anterior.
                        btnPaginaAnterior.ServerClick -= AlterarPagina_ServerClick;
                    }
                    else
                    {
                        btnPaginaAnterior.Attributes["data-pagina"] = (PaginaAtual - 1).ToString();
                        liPaginaAnterior.Attributes["class"] = "mostrarload";
                    }

                    if (PaginaAtual >= qntPaginas)
                    {
                        liProximaPagina.Attributes["class"] = "disabled";
                        // Remove evento do click do botão de próxima página.
                        btnProximaPagina.ServerClick -= AlterarPagina_ServerClick;
                    }
                    else
                    {
                        btnProximaPagina.Attributes["data-pagina"] = (PaginaAtual + 1).ToString();
                        liProximaPagina.Attributes["class"] = "mostrarload";
                    }

                    // Gerar páginas.
                    var listaPaginas = new List<DTOPaginacao>();

                    for (var i = 1; i <= qntPaginas; i++)
                    {
                        listaPaginas.Add(new DTOPaginacao
                        {
                            Numero = i,
                            IsPaginaAtual = i == PaginaAtual
                        });
                    }

                    rptPaginas.DataSource = listaPaginas;
                    rptPaginas.DataBind();
                }
            }
        }

        protected void AlterarPagina_ServerClick(object sender, EventArgs e)
        {
            var pagina = int.Parse(((HtmlAnchor)sender).Attributes["data-pagina"]);

            AlterarPagina(pagina);
        }

        private void AlterarPagina(int pagina)
        {
            try
            {
                var resultadoRespondente = (Cache["dsRelatorioRespondente"] as DTORelatorioQuestionarioRespondente);

                if (resultadoRespondente != null)
                {
                    // Alterar a página por aqui. Simples assim.
                    PaginaAtual = pagina;
                    
                    // Quantidade de páginas
                    var qntPaginas = ObterQuanidadePaginas(resultadoRespondente);

                    if (PaginaAtual == 1)
                    {
                        liPaginaAnterior.Attributes["class"] = "disabled";
                        // Remove evento do click do botão de página anterior.
                        btnPaginaAnterior.ServerClick -= AlterarPagina_ServerClick;
                    }
                    else
                    {
                        btnPaginaAnterior.Attributes["data-pagina"] = (PaginaAtual - 1).ToString();
                        liPaginaAnterior.Attributes["class"] = "mostrarload";
                    }

                    if (PaginaAtual >= qntPaginas)
                    {
                        liProximaPagina.Attributes["class"] = "disabled";
                        // Remove evento do click do botão de próxima página.
                        btnProximaPagina.ServerClick -= AlterarPagina_ServerClick;
                    }
                    else
                    {
                        btnProximaPagina.Attributes["data-pagina"] = (PaginaAtual + 1).ToString();
                        liProximaPagina.Attributes["class"] = "mostrarload";
                    }

                    GerarDadosRespondentes(resultadoRespondente);
                }
                else
                    throw new Exception("A sessão do seu relatório foi perdida. Consulte novamente.");
            }
            catch (Exception ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private decimal ObterQuanidadePaginas(DTORelatorioQuestionarioRespondente resultadoRespondente)
        {
            var qntPaginas = decimal.Divide(resultadoRespondente.TotalRespostas, MaxPerPage);

            // Adiciona uma página para os resultados residuais caso a divisão possua decimais.
            if (qntPaginas - ((int)qntPaginas) > 0)
                qntPaginas = ((int)qntPaginas) + 1;

            return qntPaginas;
        }

        protected void ddlTipoQuestionario_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            using (var rel = new RelatorioQuestionario())
            {
                int idTipoQuestionario;

                if (int.TryParse(ddlTipoQuestionario.SelectedValue, out idTipoQuestionario))
                {
                    PreencherComboQuestionario();
                }
            }
        }
    }
}