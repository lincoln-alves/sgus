using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using AutoMapper;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using classes = Sebrae.Academico.Dominio.Classes;
using System.Web.UI;
using System.Web.Script.Serialization;
using Sebrae.Academico.BM.Classes;

namespace Sebrae.Academico.WebForms.Cadastros
{
    public partial class EdicaoQuestionarioU : PageBase
    {
        public string QuestionarioSessaoJson { get; private set; }

        protected override object LoadPageStateFromPersistenceMedium()
        {
            try
            {
                Session["ViewState" + Request.Url.Fragment] = Request.Form["__VIEWSTATE"];
                return base.LoadPageStateFromPersistenceMedium();
            }
            catch (Exception x)
            {
                string vsString = Request.Form["__VIEWSTATE"];
            }

            pnlModalSessao.Visible = true;

            return string.Empty;
        }

        public int? QuestionarioSessao
        {
            get
            {
                return Request.Cookies["questionarioSessao"] != null && !string.IsNullOrEmpty(Request.Cookies["questionarioSessao"].Value) ?
                    int.Parse(Request.Cookies["questionarioSessao"].Value) : (int?)null;
            }
            set
            {
                Response.Cookies["questionarioSessao"].Value = value.ToString();
            }
        }

        public int? TipoQuestionarioSessao
        {
            get
            {
                return Request.Cookies["questionarioSessaoTipo"] != null && !string.IsNullOrEmpty(Request.Cookies["questionarioSessaoTipo"].Value) ?
                    int.Parse(Request.Cookies["questionarioSessaoTipo"].Value) : (int?)null;
            }
            set
            {
                Response.Cookies["questionarioSessaoTipo"].Value = value.ToString();
            }
        }

        public bool RestaurarSessao
        {
            get
            {
                // Verifica se o questionário que está na sessão é do mesmo tipo do questionário que está sendo editado
                return Request.Cookies["questionarioSessao"] != null && !string.IsNullOrEmpty(Request.Cookies["questionarioSessao"].Value) &&
                    Request.Cookies["questionarioSessaoTipo"] != null && !string.IsNullOrEmpty(Request.Cookies["questionarioSessaoTipo"].Value) &&
                    !string.IsNullOrEmpty(ddlTipoQuestionario.SelectedValue) && int.Parse(ddlTipoQuestionario.SelectedValue) == TipoQuestionarioSessao;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request["Session"]))
            {
                Response.Redirect(string.Format("EdicaoQuestionarioU.aspx?Session={0}", WebFormHelper.ObterStringAleatoria()));
            }

            if (!Page.IsPostBack)
            {
                try
                {
                    QuestionarioDaSessao = null;

                    CarregarPermissoes();

                    PreencherCombos();

                    if (Request["Id"] != null)
                    {
                        bool duplicar = !string.IsNullOrEmpty(Request["Duplicar"]);

                        // Esconde o botão de duplicar o questionário na edição para não confundir o usuário,
                        // caso a duplicagem esteja vindo da tela de listagem via Query String.
                        btnDuplicarQuestionario.Visible = Request["Duplicar"] == null;

                        var idQuestionario = int.Parse(Request["Id"]);

                        QuestionarioDaSessao = _manterQuestionario.ObterQuestionarioPorID(idQuestionario);

                        var usuario = new ManterUsuario().ObterUsuarioLogado();

                        // Validando permissão de edição do questionário
                        if (!duplicar && !QuestionarioDaSessao.TratarEdicaoQuestionario(usuario))
                        {
                            Response.Redirect("~/Cadastros/Questionario/ListarQuestionario.aspx");
                        }

                        PreencherCampos(QuestionarioDaSessao);
                        pnlOpcoesQuestionario.Visible = true;

                        if (QuestionarioDaSessao.TipoQuestionario == enumTipoQuestionario.Avulso)
                        {
                            divLinkQuestionario.Visible = true;
                            hpLinkQuestionario.NavigateUrl = (ConfigurationManager.AppSettings["url_portal"] ??
                                                              "http://uc.sebrae.com.br/") + "questionario/avulso/" +
                                                             QuestionarioDaSessao.ID;

                            hpLinkQuestionario.Text = (ConfigurationManager.AppSettings["url_portal"] ??
                                                       "http://uc.sebrae.com.br/") + "questionario/avulso/" +
                                                      QuestionarioDaSessao.ID;
                        }

                        VerificarRestauracao();
                    }
                    else
                    {
                        QuestionarioDaSessao = new classes.Questionario { ID = 0 };
                    }

                    FormatarTabela();
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
            }
        }

        private void VerificarRestauracao()
        {
            if (RestaurarSessao)
            {
                pnlModalSessao.Visible = true;
                return;
            }
        }

        private void RestaurarPermissoes(DTOQuestionario questionarioRestaurado)
        {
            // Restaurar lista de perfis.
            if (questionarioRestaurado.ListaPerfil.Any())
            {
                foreach (
                    var perfilIdDto in
                        questionarioRestaurado.ListaPerfil.Where(
                            perfilIdDto =>
                                !QuestionarioDaSessao.ListaQuestionarioPermissao.Any(
                                    x => x.Perfil != null && x.Perfil.ID == perfilIdDto)))
                {
                    QuestionarioDaSessao.ListaQuestionarioPermissao.Add(new classes.
                        QuestionarioPermissao
                    {
                        Perfil = new classes.Perfil
                        {
                            ID = perfilIdDto
                        }
                    });
                }

                var listaPerfil = QuestionarioDaSessao.ListaQuestionarioPermissao.Where(x => x.Perfil != null)
                            .Where(
                                permissaoPerfil =>
                                    questionarioRestaurado.ListaPerfil.All(
                                        perfilId => perfilId != permissaoPerfil.Perfil.ID)).ToList();

                foreach (var permissaoPerfil in listaPerfil)
                {
                    QuestionarioDaSessao.ListaQuestionarioPermissao.Remove(permissaoPerfil);
                }
            }
            else if (QuestionarioDaSessao.ListaQuestionarioPermissao.Any(x => x.Perfil != null))
            {
                // Limpar lista de perfil, se houver.
                QuestionarioDaSessao.ListaQuestionarioPermissao =
                    QuestionarioDaSessao.ListaQuestionarioPermissao.Where(x => x.Perfil != null)
                        .ToList();
            }

            // Restaurar lista de níveis ocupacionais.
            if (questionarioRestaurado.ListaNivelOcupacional.Any())
            {
                foreach (
                    var nivelOcupacionalIdDto in
                        questionarioRestaurado.ListaNivelOcupacional.Where(
                            nivelOcupacionalIdDto =>
                                !QuestionarioDaSessao.ListaQuestionarioPermissao.Any(
                                    x => x.NivelOcupacional != null && x.NivelOcupacional.ID == nivelOcupacionalIdDto)))
                {
                    QuestionarioDaSessao.ListaQuestionarioPermissao.Add(new classes.
                        QuestionarioPermissao
                    {
                        NivelOcupacional = new classes.NivelOcupacional
                        {
                            ID = nivelOcupacionalIdDto
                        }
                    });
                }

                var listaNivelOcupacional = QuestionarioDaSessao.ListaQuestionarioPermissao.Where(x => x.NivelOcupacional != null)
                            .Where(
                                permissaoNivelOcupacional =>
                                    questionarioRestaurado.ListaNivelOcupacional.All(
                                        nivelOcupacionalId =>
                                            nivelOcupacionalId != permissaoNivelOcupacional.NivelOcupacional.ID)).ToList();

                foreach (var permissaoNivelOcupacional in listaNivelOcupacional)
                {
                    QuestionarioDaSessao.ListaQuestionarioPermissao.Remove(permissaoNivelOcupacional);
                }
            }
            else if (QuestionarioDaSessao.ListaQuestionarioPermissao.Any(x => x.NivelOcupacional != null))
            {
                // Limpar lista de nível ocupacional, se houver.
                QuestionarioDaSessao.ListaQuestionarioPermissao =
                    QuestionarioDaSessao.ListaQuestionarioPermissao.Where(x => x.NivelOcupacional != null)
                        .ToList();
            }

            // Restaurar lista de Ufs.
            if (questionarioRestaurado.ListaUf.Any())
            {
                foreach (
                    var ufIdDto in
                        questionarioRestaurado.ListaUf.Where(
                            ufIdDto =>
                                !QuestionarioDaSessao.ListaQuestionarioPermissao.Any(
                                    x => x.Uf != null && x.Uf.ID == ufIdDto)))
                {
                    QuestionarioDaSessao.ListaQuestionarioPermissao.Add(new classes.
                        QuestionarioPermissao
                    {
                        Uf = new classes.Uf
                        {
                            ID = ufIdDto
                        }
                    });
                }

                var lista = QuestionarioDaSessao.ListaQuestionarioPermissao.Where(x => x.Uf != null)
                            .Where(
                                permissaoUf =>
                                    questionarioRestaurado.ListaUf.All(
                                        ufId => ufId != permissaoUf.Uf.ID)).ToList();

                foreach (var permissaoUf in lista)
                {
                    QuestionarioDaSessao.ListaQuestionarioPermissao.Remove(permissaoUf);
                }
            }
            else if (QuestionarioDaSessao.ListaQuestionarioPermissao.Any(x => x.Uf != null))
            {
                // Limpar lista de uf, se houver.
                QuestionarioDaSessao.ListaQuestionarioPermissao =
                    QuestionarioDaSessao.ListaQuestionarioPermissao.Where(x => x.Uf != null)
                        .ToList();
            }
        }

        private readonly ManterUsuario _manterUsuario = new ManterUsuario();
        private readonly ManterQuestionario _manterQuestionario = new ManterQuestionario();

        public classes.Questionario QuestionarioDaSessao
        {
            get
            {
                classes.Questionario questionario;

                if (Session["Questionario_" + Request["Session"]] != null)
                {
                    questionario = (classes.Questionario)Session["Questionario_" + Request["Session"]];
                }
                else
                {
                    questionario = new classes.Questionario();
                    Session["Questionario_" + Request["Session"]] = questionario;
                }

                return questionario;
            }
            set { Session["Questionario_" + Request["Session"]] = value; }
        }

        private classes.ItemQuestionario ItemQuestionarioBackup
        {
            get
            {
                classes.ItemQuestionario itemQuestionario;

                if (Session["ItemQuestionario_" + Request["Session"]] != null)
                {
                    itemQuestionario = (classes.ItemQuestionario)Session["ItemQuestionario_" + Request["Session"]];
                }
                else
                {
                    itemQuestionario = new classes.ItemQuestionario();
                    Session["ItemQuestionario_" + Request["Session"]] = itemQuestionario;
                }
                return itemQuestionario;
            }
            set { Session["ItemQuestionario_" + Request["Session"]] = value; }
        }

        protected override IList<enumPerfil> PerfisNecessariosParaAcessarAFuncionalidade
        {
            get { throw new NotImplementedException(); }
        }

        protected override enumFuncionalidade Funcionalidade
        {
            get { return enumFuncionalidade.Questionario; }
        }

        private void PreencherCombos()
        {
            WebFormHelper.PreencherComponenteComOpcoesSimNao(rblAtivo, inserirOpcaoSelecione: false, simOuNao: false);
            PreencherComboTipoQuestionario();
            PreencherComboTipoItemQuestionario();
            WebFormHelper.PreencherComponenteComOpcoesSimNao(rdbExibeFeedback);
            WebFormHelper.PreencherComponenteComOpcoesSimNao(rdbRespostaObrigatoria);

            if (Request["Id"] == null)
            {
                var usuario = _manterUsuario.ObterUsuarioLogado();
                ucCategorias1.PreencherCategorias(false, null, usuario);
            }

            ucPermissoes1.PreencherSomenteUfs(false);
        }

        private void PreencherComboEstiloItemQuestionario(classes.ItemQuestionario itemQuestionario, enumTipoItemQuestionario? tipoItemQuestionarioSelecionado)
        {
            var todosEstilos = itemQuestionario.TipoItemQuestionario.TodosEstilos;

            if (todosEstilos == null)
            {
                divEstilo.Visible = false;
            }
            else
            {
                divEstilo.Visible = true;

                var manterEstiloItemQuestonario = new ManterEstiloItemQuestionario();

                var listaEstiloItemQuestionario = todosEstilos == true
                    ? manterEstiloItemQuestonario.ObterTodosEstiloItemQuestionario()
                    : itemQuestionario.TipoItemQuestionario.ListaEstilosItemQuestionario;

                // Caso o estilo da questão não esteja na lista, adiciona.
                // Isso será útil para manter o valor em casos de edições.
                if (itemQuestionario.EstiloItemQuestionario != null &&
                    listaEstiloItemQuestionario.All(e => e.ID != itemQuestionario.EstiloItemQuestionario.ID))
                    listaEstiloItemQuestionario.Add(itemQuestionario.EstiloItemQuestionario);

                if (tipoItemQuestionarioSelecionado == enumTipoItemQuestionario.Objetiva || tipoItemQuestionarioSelecionado == enumTipoItemQuestionario.MultiplaEscolha)
                {
                    var estiloCheckButton = listaEstiloItemQuestionario.Where(e => e.ID == (int)enumEstiloItemQuestionario.Marcacao).FirstOrDefault();
                    if (estiloCheckButton != null)
                    {
                        listaEstiloItemQuestionario.Remove(estiloCheckButton);
                    }
                }

                WebFormHelper.PreencherLista(listaEstiloItemQuestionario.OrderBy(x => x.ID).ToList(), ddlEstiloItemQuestionario, false, true);
            }
        }

        private void PreencherComboTipoQuestionario()
        {
            var manterTipoQuestionario = new ManterTipoQuestionario();
            var listaTipoQuestionario = manterTipoQuestionario.ObterTodos();
            WebFormHelper.PreencherLista(listaTipoQuestionario, ddlTipoQuestionario, false, true);
        }

        private void PreencherComboTipoItemQuestionario()
        {
            var manterTipoItemQuestionario = new ManterTipoItemQuestionario();
            var listaTipoItemQuestionario = manterTipoItemQuestionario.ObterTodosTipoItemQuestionario();
            listaTipoItemQuestionario =
                listaTipoItemQuestionario.Where(x => x.ID != (int)enumTipoItemQuestionario.Diagnostico).ToList();
            WebFormHelper.PreencherLista(listaTipoItemQuestionario, ddlTipoItemQuestionario, false, true);
        }

        private void PreencherCampos(classes.Questionario questionarioEdicao)
        {
            if (questionarioEdicao == null) return;
            txtNome.Text = questionarioEdicao.Nome;
            WebFormHelper.SetarValorNaCombo(questionarioEdicao.TipoQuestionario.ID.ToString(), ddlTipoQuestionario);
            txtPrazoemMinutos.Text = questionarioEdicao.PrazoMinutos.ToString();
            if (questionarioEdicao.QtdQuestoesProva.HasValue)
            {
                txtQtdQuestoesDaProva.Text = questionarioEdicao.QtdQuestoesProva.Value.ToString();
            }

            if (!string.IsNullOrWhiteSpace(questionarioEdicao.TextoEnunciado))
            {
                txtTextoEnunciadoPre.Text = questionarioEdicao.TextoEnunciado;
            }

            WebFormHelper.SetarValorNoRadioButtonList(questionarioEdicao.Ativo, rblAtivo);

            ddlTipoQuestionario.Enabled = false;

            PreencherInformacoesDeItensDoQuestionario(questionarioEdicao);

            var tipo = (enumTipoQuestionario)questionarioEdicao.TipoQuestionario.ID;

            switch (tipo)
            {
                case enumTipoQuestionario.AvaliacaoProva:
                    divPrazoemMinutos.Visible = true;
                    divQtdQuestoesDaProva.Visible = true;
                    break;
                case enumTipoQuestionario.Pesquisa:
                case enumTipoQuestionario.Demanda:
                    divPrazoemMinutos.Visible = false;
                    divQtdQuestoesDaProva.Visible = false;
                    break;
                case enumTipoQuestionario.Avulso:
                    PreencherCamposDeQuestionarioAvulso();
                    break;
                case enumTipoQuestionario.Dinamico:
                case enumTipoQuestionario.Cancelamento:
                case enumTipoQuestionario.Abandono:
                    break;
                case enumTipoQuestionario.AtividadeTrilha:
                    Label4.Text = "Qtd de questões da atividade";
                    divNotaMinima.Visible = true;
                    txtNotaMinima.Text = questionarioEdicao.NotaMinima == 0 ? "7" : questionarioEdicao.NotaMinima.ToString();
                    break;
                default:
                    throw new AcademicoException("Tipo de questionário não encontrado");
            }

            var usuario = _manterUsuario.ObterUsuarioLogado();

            ucCategorias1.PreencherCategorias(false,
                questionarioEdicao.ListaCategoriaConteudo.Any()
                    ? questionarioEdicao.ListaCategoriaConteudo.Select(x => x.ID).ToList()
                    : null, usuario);

            CarregarPermissoes();
        }

        private void PreencherCamposDeQuestionarioAvulso()
        {
            divPrazoemMinutos.Visible = false;
            divQtdQuestoesDaProva.Visible = false;
            divFormaDeAquisicao.Visible = true;

            WebFormHelper.PreencherComponenteComOpcoesSimNao(rblNotificacao);
            WebFormHelper.SetarValorNoRadioButtonList(false, rblNotificacao);
            divNotificacao.Visible = true;

            CarregarFormaDeAquisicao();
            divQuestionarioPermissoes.Visible = true;
            ucPermissoes1.PreencherListas();
        }

        private void PreencherInformacoesDeItensDoQuestionario(classes.Questionario questionario)
        {
            if (questionario != null && questionario.ListaItemQuestionario != null &&
                questionario.ListaItemQuestionario.Count > 0)
            {
                WebFormHelper.PreencherGrid(questionario.ListaItemQuestionario.OrderBy(x => x.Ordem).ToList(),
                    dgvItensDoQuestionario);
            }
            else
            {
                IList<classes.ItemQuestionario> listaItemQuestionarioVazia = new List<classes.ItemQuestionario>();
                WebFormHelper.PreencherGrid(listaItemQuestionarioVazia, dgvItensDoQuestionario);
            }
        }

        protected void ddlTipoQuestionario_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlTipoItemQuestionario.Enabled = true;
                
                if (QuestionarioDaSessao.TipoQuestionario == null)
                    QuestionarioDaSessao.TipoQuestionario = new classes.TipoQuestionario();

                if (ddlTipoQuestionario.SelectedItem != null &&
                    !string.IsNullOrWhiteSpace(ddlTipoQuestionario.SelectedItem.Value))
                {
                    var tipo = (enumTipoQuestionario)int.Parse(ddlTipoQuestionario.SelectedItem.Value);

                    switch (tipo)
                    {
                        case enumTipoQuestionario.AvaliacaoProva:
                            divPrazoemMinutos.Visible = true;
                            divValorQuestao.Visible = true;
                            divGabarito.Visible = false;
                            divPrazoemMinutos.Visible = true;
                            divQtdQuestoesDaProva.Visible = true;
                            break;
                        case enumTipoQuestionario.Pesquisa:
                        case enumTipoQuestionario.Demanda:
                            //Esconde o checkbox de Resposta correta do usercontrol de Questionário
                            divRespostaCorreta.Visible = false;
                            divGabarito.Visible = false;
                            divValorQuestao.Visible = false;
                            divPrazoemMinutos.Visible = false;
                            divQtdQuestoesDaProva.Visible = false;
                            break;
                        case enumTipoQuestionario.Avulso:
                            //Esconde o checkbox de Resposta correta do usercontrol de Questionário
                            divRespostaCorreta.Visible = false;
                            divGabarito.Visible = false;
                            divValorQuestao.Visible = false;
                            divPrazoemMinutos.Visible = false;
                            divQtdQuestoesDaProva.Visible = false;
                            PreencherCamposDeQuestionarioAvulso();
                            break;
                        case enumTipoQuestionario.Dinamico:
                        case enumTipoQuestionario.Cancelamento:
                        case enumTipoQuestionario.Abandono:
                            break;
                        case enumTipoQuestionario.AtividadeTrilha:
                            divNotaMinima.Visible = true;
                            Label4.Text = "Qtd de questões da atividade";

                            if (Request["Id"] == null)
                            {
                                txtNotaMinima.Text = "7";
                            }
                            break;
                        default:
                            throw new AcademicoException("Tipo de questionário não encontrado");
                    }

                    QuestionarioDaSessao.TipoQuestionario.ID = int.Parse(ddlTipoQuestionario.SelectedItem.Value);
                }
                pnlOpcoesQuestionario.Visible = true;
                ddlTipoQuestionario.Enabled = false;
                dgvItensDoQuestionario.DataBind();

                VerificarRestauracao();
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private void CarregarPermissoes()
        {
            PreencherListBoxComFormaAquisicaoGravadosNoBanco();
            PreencherListaPerfil();
            PreencherNivelOcupacional();
            PreencherListaUf();
        }

        internal void PreencherListBoxComFormaAquisicaoGravadosNoBanco()
        {
            if (QuestionarioDaSessao.ListaQuestionarioPermissao == null ||
                QuestionarioDaSessao.ListaQuestionarioPermissao.Count <= 0) return;
            for (var i = 0; i < rblFormaDeAquisicao.Items.Count; i++)
            {
                rblFormaDeAquisicao.Items[i].Selected =
                    (QuestionarioDaSessao.ListaQuestionarioPermissao.Any(
                        x =>
                            x.FormaAquisicao != null &&
                            x.FormaAquisicao.ID == Convert.ToInt32(rblFormaDeAquisicao.Items[i].Value)));
            }
        }

        private void PreencherListaPerfil()
        {
            var listaPerfil =
                QuestionarioDaSessao.ListaQuestionarioPermissao.Where(x => x.Perfil != null)
                    .Select(x => new classes.Perfil { ID = x.Perfil.ID, Nome = x.Perfil.Nome })
                    .ToList();

            ucPermissoes1.PreencherPerfis(new ManterPerfil().ObterTodosPerfis().Select(x => new DTOPerfil
            {
                ID = x.ID,
                Nome = x.Nome,
                IsSelecionado = listaPerfil.Any(p => p.ID == x.ID)
            }).ToList(), "data-restaurar", "ListaPerfil");
        }

        private void PreencherNivelOcupacional()
        {
            var listaNivelOcupacional =
                QuestionarioDaSessao.ListaQuestionarioPermissao.Where(x => x.NivelOcupacional != null)
                    .Select(x => new classes.NivelOcupacional { ID = x.NivelOcupacional.ID, Nome = x.NivelOcupacional.Nome })
                    .ToList();

            ucPermissoes1.PreencherNiveisOcupacionais(
                new ManterNivelOcupacional().ObterTodosNivelOcupacional().Select(x => new DTONivelOcupacional
                {
                    ID = x.ID,
                    Nome = x.Nome,
                    IsSelecionado = listaNivelOcupacional.Any(p => p.ID == x.ID)
                }).ToList(), "data-restaurar", "ListaNivelOcupacional");
        }

        private void PreencherListaUf()
        {
            var listaUf =
               QuestionarioDaSessao.ListaQuestionarioPermissao.Where(x => x.Uf != null)
                   .Select(x => new classes.Uf { ID = x.Uf.ID, Nome = x.Uf.Nome })
                   .ToList();

            ucPermissoes1.PreencherUfs(
                new ManterUf().ObterTodosUf().Select(x => new DTOUf
                {
                    ID = x.ID,
                    Nome = x.Nome,
                    IsSelecionado = listaUf.Any(p => p.ID == x.ID)
                }).ToList(), false, "data-restaurar", "ListaUf");
        }

        private void CarregarFormaDeAquisicao()
        {
            if (rblFormaDeAquisicao.Items.Count != 0) return;

            var listaFormaAquisicao = new ManterFormaAquisicao().ObterTodasFormaAquisicao().OrderBy(x => x.Nome);

            foreach (var listItem in
                listaFormaAquisicao.Select(item => new ListItem
                {
                    Value = item.ID.ToString(),
                    Text = item.Nome,
                    Selected =
                        (QuestionarioDaSessao.ListaQuestionarioPermissao.Any(
                            x => x.FormaAquisicao != null && x.FormaAquisicao.ID == item.ID))
                }))
            {
                rblFormaDeAquisicao.Items.Add(listItem);
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            string msgRetorno;

            try
            {
                var listaPermissoes = new List<classes.QuestionarioPermissao>();

                var questionarioEdicao = ObterObjetoQuestionario(ref listaPermissoes);

                if (questionarioEdicao != null && questionarioEdicao.ListaItemQuestionario != null)

                if (Request["Id"] == null)
                {
                    _manterQuestionario.IncluirQuestionario(questionarioEdicao);
                    _manterQuestionario.IncluirQuestionarioPermissao(listaPermissoes, questionarioEdicao);
                }
                else
                {
                    if (Request["Duplicar"] != null)
                    {
                        _manterQuestionario.DuplicarQuestionario(questionarioEdicao,
                            _manterUsuario.ObterUsuarioLogado().IsGestor());
                    }
                    else
                        _manterQuestionario.AlterarQuestionario(questionarioEdicao);
                }

                msgRetorno = EmiteNotificacao(questionarioEdicao);

                LimparQuestionarioDaSessao();
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                return;
            }

            WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados gravados com sucesso !" + msgRetorno,
                "ListarQuestionario.aspx");
        }

        protected void btnAdicionarItemQuestionario_Click(object sender, EventArgs e)
        {
            try
            {
                ItemQuestionarioBackup = null;
                LimparCamposItemQuestionario();

                if (QuestionarioDaSessao.ListaItemQuestionario == null)
                    QuestionarioDaSessao.ListaItemQuestionario = new List<classes.ItemQuestionario>();

                hdAcaoItemQuestionario.Value = "INCLUIR";
                hdIndexOfItemQuestionario.Value = QuestionarioDaSessao.ListaItemQuestionario.Count.ToString();

                var novoItemQuestionario = new classes.ItemQuestionario
                {
                    Questionario = QuestionarioDaSessao,
                    TipoItemQuestionario = new classes.TipoItemQuestionario()
                };

                QuestionarioDaSessao.ListaItemQuestionario.Add(novoItemQuestionario);
                ExibirModal();
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private void FecharModalCancelando()
        {
            if (!string.IsNullOrEmpty(hdIndexOfItemQuestionario.Value))
            {
                try
                {
                    int itemQuestionarioEditado = int.Parse(hdIndexOfItemQuestionario.Value);
                    if (hdAcaoItemQuestionario.Value == "INCLUIR")
                        QuestionarioDaSessao.ListaItemQuestionario.RemoveAt(itemQuestionarioEditado);
                    else
                        QuestionarioDaSessao.ListaItemQuestionario[itemQuestionarioEditado] =
                            (classes.ItemQuestionario)ItemQuestionarioBackup.Clone();
                }
                catch
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Erro ao recuprar os dados da sessão");
                }
            }
            else
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Erro ao recuperar estado a solicitação");
            }

            OcultarModal();
            LimparCamposItemQuestionario();
        }

        private void LimparCamposItemQuestionario()
        {
            if (ddlEstiloItemQuestionario.SelectedIndex >= 0)
                ddlEstiloItemQuestionario.SelectedIndex = 0;

            txtQuestaoNome.Text = "";
            txtFeedback.Text = "";
            txtValorQuestao.Text = "";
            dgvItemQuestionarioOpcoes.DataSource = null;
            dgvItemQuestionarioOpcoes.DataBind();
            pnlItemQuestionario.Visible = false;
            btnAdicionarItemQuestionarioOpcao.Visible = true;

            ddlTipoItemQuestionario.SelectedIndex = 0;
            ddlTipoItemQuestionario.Enabled = true;

        }

        private void LimparQuestionarioDaSessao()
        {
            Session.Remove("QuestionarioEdit_" + Request["Session"]);
            Session.Remove("Questionario_" + Request["Session"]);
            Session.Remove("ItemQuestionario_" + Request["Session"]);

            QuestionarioSessao = null;
            TipoQuestionarioSessao = null;
        }

        private classes.Questionario ObterObjetoQuestionario(ref List<classes.QuestionarioPermissao> permissoes)
        {
            classes.Questionario questionarioEdicao;

            var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

            if (QuestionarioDaSessao.ID == 0)
            {
                questionarioEdicao = QuestionarioDaSessao;
                questionarioEdicao.TipoQuestionario = new classes.TipoQuestionario()
                {
                    ID = int.Parse(ddlTipoQuestionario.SelectedItem.Value),
                    Nome = ddlTipoQuestionario.SelectedItem.Text
                };

                if (usuarioLogado.IsGestor())
                    questionarioEdicao.Uf = new ManterUf().ObterUfPorID(usuarioLogado.UF.ID);
            }
            else
            {
                questionarioEdicao = (classes.Questionario)QuestionarioDaSessao.Clone();
            }

            if (txtQtdQuestoesDaProva.Visible &&
                (string.IsNullOrEmpty(txtQtdQuestoesDaProva.Text) || int.Parse(txtQtdQuestoesDaProva.Text) <= 0))
                throw new AcademicoException("Quantidade de questões é campo obrigatório.");

            questionarioEdicao.Nome = txtNome.Text.Trim();
            questionarioEdicao.PrazoMinutos = WebFormHelper.ConverterParaInteiroNull(txtPrazoemMinutos.Text);
            questionarioEdicao.QtdQuestoesProva = WebFormHelper.ConverterParaInteiroNull(txtQtdQuestoesDaProva.Text);

            questionarioEdicao.TextoEnunciado = txtTextoEnunciadoPre.Text.Trim();

            //Ativo
            if (rblAtivo.SelectedItem != null && !string.IsNullOrWhiteSpace(rblAtivo.SelectedItem.Value))
            {
                questionarioEdicao.Ativo = rblAtivo.SelectedItem.Value.Trim().ToUpper().Equals("S");
            }
            else
            {
                throw new AcademicoException("O campo \"ativo\" é obrigatório");
            }

            //if (questionarioEdicao.TipoQuestionario.ID == (int)enumTipoQuestionario.Avulso)
            // Adicionando inclusão de permissões para todos os tipos de questionários.
            ObterFormaAquisicao(questionarioEdicao);
            ObterPerfil(questionarioEdicao);
            ObterNivelOcupacional(questionarioEdicao);
            ObterUf(questionarioEdicao);

            if (questionarioEdicao.ID == 0)
            {
                permissoes = questionarioEdicao.ListaQuestionarioPermissao.ToList();
                questionarioEdicao.ListaQuestionarioPermissao.Clear();
            }

            if (!ucCategorias1.IdsCategoriasMarcadas.Any())
            {
                throw new AcademicoException("Selecione ao menos uma categoria de conteúdo.");
            }
            var manterCategoriaConteudo = new ManterCategoriaConteudo();
            questionarioEdicao.ListaCategoriaConteudo =
                ucCategorias1.IdsCategoriasMarcadas.Select(id => manterCategoriaConteudo.ObterCategoriaConteudoPorID(id))
                    .ToList();

            // Buscando uf do usuário logado para tratar problema de gerenciamento de conexão do nhibernate.
            if (questionarioEdicao.Uf == null)
            {
                var idUf = _manterUsuario.ObterUsuarioLogado().UF.ID;
                questionarioEdicao.Uf = new ManterUf().ObterUfPorID(idUf);
            }
            else if (Request["Duplicar"] != null)
            {
                if (usuarioLogado.IsAdministrador())
                {
                    questionarioEdicao.Uf = null;
                }

                if (usuarioLogado.IsGestor())
                {
                    // Buscando uf do usuário logado para tratar problema de gerenciamento de conexão do nhibernate.
                    questionarioEdicao.Uf = new ManterUf().ObterUfPorID(usuarioLogado.UF.ID);
                }
            }

            // Nota mínima
            if (questionarioEdicao.TipoQuestionario.ID == (int)enumTipoQuestionario.AtividadeTrilha)
            {
                if (string.IsNullOrEmpty(txtNotaMinima.Text))
                    throw new AcademicoException("O campo \"nota mínima\" e de preenchimento obrigatório");

                questionarioEdicao.NotaMinima = int.Parse(txtNotaMinima.Text);
            }

            return questionarioEdicao;
        }



        private void ObterNivelOcupacional(classes.Questionario questionarioEdicao)
        {
            var ckblstNivelOcupacional = (CheckBoxList)ucPermissoes1.FindControl("ckblstNivelOcupacional");

            for (var i = 0; i < ckblstNivelOcupacional.Items.Count; i++)
            {
                var q = new classes.QuestionarioPermissao
                {
                    NivelOcupacional = new classes.NivelOcupacional
                    {
                        ID = Convert.ToInt32(ckblstNivelOcupacional.Items[i].Value)
                    },
                    Questionario = new classes.Questionario { ID = questionarioEdicao.ID }
                };

                if (ckblstNivelOcupacional.Items[i].Selected)
                {
                    if (
                        !questionarioEdicao.ListaQuestionarioPermissao.Any(
                            x => x.NivelOcupacional != null && x.NivelOcupacional.ID == q.NivelOcupacional.ID))
                    {
                        questionarioEdicao.ListaQuestionarioPermissao.Add(q);
                    }
                }
                else
                {
                    var qp =
                        questionarioEdicao.ListaQuestionarioPermissao.FirstOrDefault(
                            x => (x.NivelOcupacional != null && x.NivelOcupacional.ID == q.NivelOcupacional.ID));
                    if (qp != null)
                    {
                        questionarioEdicao.ListaQuestionarioPermissao.Remove(qp);
                    }
                }
            }
        }

        private void ObterPerfil(classes.Questionario questionarioEdicao)
        {
            var ckblstPerfil = (CheckBoxList)ucPermissoes1.FindControl("ckblstPerfil");

            for (var i = 0; i < ckblstPerfil.Items.Count; i++)
            {
                var q = new classes.QuestionarioPermissao
                {
                    Perfil = new classes.Perfil { ID = Convert.ToInt32(ckblstPerfil.Items[i].Value) },
                    Questionario = new classes.Questionario { ID = questionarioEdicao.ID }
                };

                if (ckblstPerfil.Items[i].Selected)
                {
                    if (
                        !questionarioEdicao.ListaQuestionarioPermissao.Any(
                            x => x.Perfil != null && x.Perfil.ID == q.Perfil.ID))
                    {
                        questionarioEdicao.ListaQuestionarioPermissao.Add(q);
                    }
                }
                else
                {
                    var qp =
                        questionarioEdicao.ListaQuestionarioPermissao.FirstOrDefault(
                            x => (x.Perfil != null && x.Perfil.ID == q.Perfil.ID));
                    if (qp != null)
                    {
                        questionarioEdicao.ListaQuestionarioPermissao.Remove(qp);
                    }
                }
            }
        }

        private void ObterFormaAquisicao(classes.Questionario questionarioEdicao)
        {
            for (var i = 0; i < rblFormaDeAquisicao.Items.Count; i++)
            {
                var q = new classes.QuestionarioPermissao
                {
                    FormaAquisicao =
                        new classes.FormaAquisicao { ID = Convert.ToInt32(rblFormaDeAquisicao.Items[i].Value) },
                    Questionario = new classes.Questionario { ID = questionarioEdicao.ID }
                };

                if (rblFormaDeAquisicao.Items[i].Selected)
                {
                    if (
                        !questionarioEdicao.ListaQuestionarioPermissao.Any(
                            x => x.FormaAquisicao != null && x.FormaAquisicao.ID == q.FormaAquisicao.ID))
                    {
                        questionarioEdicao.ListaQuestionarioPermissao.Add(q);
                    }
                }
                else
                {
                    var qp =
                        questionarioEdicao.ListaQuestionarioPermissao.FirstOrDefault(
                            x => (x.FormaAquisicao != null && x.FormaAquisicao.ID == q.FormaAquisicao.ID));
                    if (qp != null)
                    {
                        questionarioEdicao.ListaQuestionarioPermissao.Remove(qp);
                    }
                }
            }
        }

        protected void dgvItensDoQuestionario_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            var indexAlteracao = row.DataItemIndex;

            //Re-ordena o questionario da sessao de acordo com a forma que os itens estão dispostos na tela.
            QuestionarioDaSessao.ListaItemQuestionario = QuestionarioDaSessao.ListaItemQuestionario.OrderBy(x => x.Ordem).ToList();

            if (e.CommandName.Equals("editar"))
            {
                try
                {
                    TratarEdicaoDeUmItemDoQuestionario(indexAlteracao);
                    ExibirModal();
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }

            }
            else if (e.CommandName.Equals("excluir"))
            {
                try
                {
                    TratarExclusaoDeUmItemDoQuestionario(indexAlteracao);
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
            }

            if (e.CommandName.Equals("duplicar"))
            {
                try
                {
                    DuplicarItemDoQuestionario(indexAlteracao);
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
            }



            if (e.CommandName.Equals("ordenar"))
            {
                if (!string.IsNullOrEmpty(hfOrdemItemQuestionario.Value))
                {
                    var ordenacao =
                        hfOrdemItemQuestionario.Value.Replace("dgvItensDoQuestionario%5B%5D=", "")
                            .Replace("&", ",")
                            .TrimStart(',')
                            .Split(',');

                    for (var i = 0; i < ordenacao.Count(); i++)
                    {
                        var idItemQuestionarioAtual = int.Parse(ordenacao[i]);

                        var itemQuestionario = QuestionarioDaSessao.ListaItemQuestionario[idItemQuestionarioAtual];

                        if (itemQuestionario != null) itemQuestionario.Ordem = i;
                    }
                }

                QuestionarioDaSessao.ListaItemQuestionario = QuestionarioDaSessao.ListaItemQuestionario.OrderBy(x => x.Ordem).ToList();

                WebFormHelper.PreencherGrid(QuestionarioDaSessao.ListaItemQuestionario, dgvItensDoQuestionario);
            }
        }

        protected void dgvItemQuestionarioOpcoes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            var indexAlteracao = row.DataItemIndex;

            if (e.CommandName.Equals("editar"))
            {
                try
                {
                    TratarEdicaoItemQuestionarioOpcao(indexAlteracao);
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
            }
            else if (e.CommandName.Equals("excluir"))
            {
                try
                {
                    TratarExclusaoItemQuestionarioOpcao(indexAlteracao);
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
            }

            if (e.CommandName.Equals("duplicar"))
            {
                try
                {
                    DuplicarItemQuestionarioOpcao(indexAlteracao);
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
            }

        }

        private void TratarEdicaoItemQuestionarioOpcao(int indexOf)
        {
            var itemQuestionario =
                QuestionarioDaSessao.ListaItemQuestionario[int.Parse(hdIndexOfItemQuestionario.Value)];

            // Se for de colunas relacionadas, multiplica o index por 2 porque só interessa pegar o item principal.
            var itemQuestionarioOpcao =
                itemQuestionario.ListaItemQuestionarioOpcoes[
                    indexOf *
                    (itemQuestionario.TipoItemQuestionario.ID == (int)enumTipoItemQuestionario.ColunasRelacionadas
                        ? 2
                        : 1)];

            btnEnviarItemQuestionarioOpcao.Visible =
                btnCancelarItemQuestionarioOpcao.Visible = true;


            switch ((enumTipoItemQuestionario)itemQuestionario.TipoItemQuestionario.ID)
            {
                case enumTipoItemQuestionario.Objetiva:
                    divRespostaCorreta.Visible = true;
                    divCampoItemQuestionarioOpcao.Visible = true;
                    break;
                case enumTipoItemQuestionario.MultiplaEscolha:
                    divCampoItemQuestionarioOpcao.Visible = true;
                    divRespostaCorreta.Visible =  itemQuestionario.Questionario.TipoQuestionario != null ?
                                                    itemQuestionario.Questionario.TipoQuestionario.ID == (int)enumTipoQuestionario.AvaliacaoProva ||
                                                    itemQuestionario.Questionario.TipoQuestionario.ID == (int)enumTipoQuestionario.AtividadeTrilha : false;
                    break;
                case enumTipoItemQuestionario.Discursiva:
                case enumTipoItemQuestionario.AgrupadorDeQuestoes:
                case enumTipoItemQuestionario.Diagnostico:
                    divCampoItemQuestionarioOpcao.Visible = true;
                    break;
                case enumTipoItemQuestionario.VerdadeiroOuFalso:
                    btnAdicionarItemQuestionarioOpcao.Visible = false;
                    break;
                case enumTipoItemQuestionario.ColunasRelacionadas:
                    divColunasRelacionadas.Visible = true;

                    txtResposta_coluna1.Text = itemQuestionarioOpcao.Nome;

                    // Buscar o texto da coluna 2 aonde a opção vinculada é a mesma que foi solicitada a edição.
                    // É uma busca inversa.
                    var itemQuestionarioOpcoesColuna2 = QuestionarioDaSessao.ListaItemQuestionario[
                        int.Parse(hdIndexOfItemQuestionario.Value)]
                        .ListaItemQuestionarioOpcoes.FirstOrDefault(
                            x =>
                                x.OpcaoVinculada != null &&
                                (itemQuestionarioOpcao.ID != 0
                                    ? x.OpcaoVinculada.ID == itemQuestionarioOpcao.ID
                                    : x.OpcaoVinculada.Nome == itemQuestionarioOpcao.Nome));
                    
                    //var itemQuestionarioOpcoesColuna2 = itemQuestionarioOpcao.OpcaoVinculada != null ? itemQuestionarioOpcao.OpcaoVinculada : null;

                    if (itemQuestionarioOpcoesColuna2 != null)
                        txtResposta_coluna2.Text = itemQuestionarioOpcoesColuna2.Nome;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            txtItemQuestionarioOpcao.Text = itemQuestionarioOpcao.Nome;
            rblRespostaCorreta.Checked = itemQuestionarioOpcao.RespostaCorreta;

            hdIndexOfItemQuestionarioOpcao.Value = indexOf.ToString();
            hdAcaoItemQuestionarioOpcao.Value = "EDITAR";
        }

        private void TratarExclusaoItemQuestionarioOpcao(int indexOf)
        {
            var itemQuestionario = QuestionarioDaSessao.ListaItemQuestionario[int.Parse(hdIndexOfItemQuestionario.Value)];

            switch ((enumTipoItemQuestionario)itemQuestionario.TipoItemQuestionario.ID)
            {
                case enumTipoItemQuestionario.Objetiva:
                case enumTipoItemQuestionario.Discursiva:
                case enumTipoItemQuestionario.MultiplaEscolha:
                case enumTipoItemQuestionario.AgrupadorDeQuestoes:
                case enumTipoItemQuestionario.Diagnostico:
                case enumTipoItemQuestionario.VerdadeiroOuFalso:
                    var item = itemQuestionario.ListaItemQuestionarioOpcoes[indexOf];
                    itemQuestionario.ListaItemQuestionarioOpcoes.Remove(item);
                    break;
                case enumTipoItemQuestionario.ColunasRelacionadas:
                    // Se for Colunas Relacionadas, remove também a opção oculta.
                    var coluna1 = itemQuestionario.ListaItemQuestionarioOpcoes[indexOf];
                    
                    var coluna2 = coluna1.OpcaoVinculada != null ? coluna1.OpcaoVinculada : null;

                    // Remover as bichonas.
                    itemQuestionario.ListaItemQuestionarioOpcoes.Remove(coluna1);

                    if(coluna2 != null) itemQuestionario.ListaItemQuestionarioOpcoes.Remove(coluna2);

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            ResetarItemQuestionarioOpcoes();
        }

        private void DuplicarItemQuestionarioOpcao(int indexOf)
        {
            //DUPLICAR
            var itemQuestionario = QuestionarioDaSessao.ListaItemQuestionario[int.Parse(hdIndexOfItemQuestionario.Value)];

            classes.ItemQuestionarioOpcoes itemQuestionarioOpcoes = new classes.ItemQuestionarioOpcoes();

            switch ((enumTipoItemQuestionario)itemQuestionario.TipoItemQuestionario.ID)
            {
                case enumTipoItemQuestionario.Objetiva:
                case enumTipoItemQuestionario.Discursiva:
                case enumTipoItemQuestionario.MultiplaEscolha:
                case enumTipoItemQuestionario.AgrupadorDeQuestoes:
                case enumTipoItemQuestionario.Diagnostico:
                case enumTipoItemQuestionario.VerdadeiroOuFalso:
                    itemQuestionarioOpcoes = itemQuestionario.ListaItemQuestionarioOpcoes[indexOf];
                    
                    break;
                case enumTipoItemQuestionario.ColunasRelacionadas:
                    
                    itemQuestionarioOpcoes = itemQuestionario.ListaItemQuestionarioOpcoes[indexOf * 2 + 1];

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if(itemQuestionarioOpcoes != null)
            {
                classes.ItemQuestionarioOpcoes opcaoVinculadaDuplicada = null;
                if (itemQuestionarioOpcoes.OpcaoVinculada != null)
                {
                    var opcaoVinculada = itemQuestionarioOpcoes.OpcaoVinculada;

                    opcaoVinculadaDuplicada = new classes.ItemQuestionarioOpcoes()
                    {
                        Nome = opcaoVinculada.Nome + " Copia",
                        ItemQuestionario = opcaoVinculada.ItemQuestionario,
                        RespostaCorreta = opcaoVinculada.RespostaCorreta,
                        TipoDiagnostico = opcaoVinculada.TipoDiagnostico,
                        OpcaoInt = opcaoVinculada.OpcaoInt
                    };

                    itemQuestionario.ListaItemQuestionarioOpcoes.Add(opcaoVinculadaDuplicada);
                }

                classes.ItemQuestionarioOpcoes itemQuestionarioOpcoesDuplicado = new classes.ItemQuestionarioOpcoes()
                {
                    Nome = itemQuestionarioOpcoes.Nome + " Copia",
                    ItemQuestionario = itemQuestionarioOpcoes.ItemQuestionario,
                    RespostaCorreta = itemQuestionarioOpcoes.RespostaCorreta,
                    TipoDiagnostico = itemQuestionarioOpcoes.TipoDiagnostico,
                    OpcaoInt = itemQuestionarioOpcoes.OpcaoInt,
                    OpcaoVinculada = opcaoVinculadaDuplicada
                };

                itemQuestionario.ListaItemQuestionarioOpcoes.Add(itemQuestionarioOpcoesDuplicado);
            }

            ResetarItemQuestionarioOpcoes();

            WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Resposta do Item do Questionário foi duplicado com suscesso!");
        }

        protected void btnAdicionarItemQuestionarioOpcao_Click(object sender, EventArgs e)
        {
            btnAdicionarItemQuestionarioOpcao.Visible = false;

            btnEnviarItemQuestionarioOpcao.Visible =
                btnCancelarItemQuestionarioOpcao.Visible = true;

            txtItemQuestionarioOpcao.Text = "";
            rblRespostaCorreta.Checked = false;

            var tipoItemQuestionario = (enumTipoItemQuestionario)(int.Parse(ddlTipoItemQuestionario.SelectedValue));

            switch (tipoItemQuestionario)
            {
                case enumTipoItemQuestionario.Objetiva:
                    divRespostaCorreta.Visible = true;
                    divCampoItemQuestionarioOpcao.Visible = true;
                    break;
                case enumTipoItemQuestionario.MultiplaEscolha:
                    divRespostaCorreta.Visible = false;
                    divCampoItemQuestionarioOpcao.Visible = true;
                    OcultarCollapseCabecalhoResposta();
                    break;
                case enumTipoItemQuestionario.Discursiva:
                case enumTipoItemQuestionario.AgrupadorDeQuestoes:
                case enumTipoItemQuestionario.Diagnostico:
                    divCampoItemQuestionarioOpcao.Visible = true;
                    break;
                case enumTipoItemQuestionario.VerdadeiroOuFalso:
                    // Só exibir os valores da resposta caso o campo V ou F.
                    divValorDaResposta.Visible = true;
                    break;
                case enumTipoItemQuestionario.ColunasRelacionadas:
                    divColunasRelacionadas.Visible = true;
                    break;
            }


            hdIndexOfItemQuestionarioOpcao.Value = "";
            hdAcaoItemQuestionarioOpcao.Value = "INCLUIR";
        }

        private void OcultarCollapseCabecalhoResposta()
        {
            var attrResposta = collapseOpcoesDeResposta.Attributes["class"];
            var classes = attrResposta.Split(' ').ToList();
            if (!classes.Exists(s => s.Equals("in")))
            {
                collapseOpcoesDeResposta.Attributes["class"] = collapseOpcoesDeResposta.Attributes["class"] + " in";
            }

            collapseDadosDaQuestao.Attributes["class"] = collapseDadosDaQuestao.Attributes["class"].Replace("in", "");
        }

        private void TratarEdicaoDeUmItemDoQuestionario(int indexItemQuestionario)
        {
            try
            {
                var itemQuestionario = QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario];

                if (itemQuestionario != null)
                {
                    hdAcaoItemQuestionario.Value = "EDITAR";
                    hdIndexOfItemQuestionario.Value = indexItemQuestionario.ToString();
                    CarregarDadosDoCadastroDeItensDoQuestionario(itemQuestionario);
                }
                else
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Erro ao recuperar o item do questionário");
                }
            }
            catch
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Erro ao recuperar o item do questionário");
            }
        }

        private void TratarExclusaoDeUmItemDoQuestionario(int idItemQuestionario)
        {
            var itemQuestionario = QuestionarioDaSessao.ListaItemQuestionario[idItemQuestionario];

            if (itemQuestionario != null)
            {
                QuestionarioDaSessao.ListaItemQuestionario.Remove(itemQuestionario);
                WebFormHelper.PreencherGrid(QuestionarioDaSessao.ListaItemQuestionario.OrderBy(x => x.Ordem).ToList(),
                    dgvItensDoQuestionario);
            }
            else
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Erro ao recuperar o item do questionário");
            }
        }


        private void DuplicarItemDoQuestionario(int idItemQuestionario)
        {
            var itemQuestionario = QuestionarioDaSessao.ListaItemQuestionario[idItemQuestionario];

            if (itemQuestionario != null)
            {
                var usuarioLogado = _manterUsuario.ObterUsuarioLogado();
                
                if (usuarioLogado.IsAdministrador() || usuarioLogado.IsGestor() || usuarioLogado.IsGestorContrato())
                {
                    //DUPLICAR
                    var nomeQuestaoDuplicado = itemQuestionario.Questao.Replace("</p>", " Copia</p>")+" ";

                    classes.ItemQuestionario itemQuestionarioDuplicado = new classes.ItemQuestionario()
                    {
                        Questao = nomeQuestaoDuplicado,
                        Questionario = itemQuestionario.Questionario,
                        QuestionarioEnunciado = itemQuestionario.QuestionarioEnunciado != null ? itemQuestionario.QuestionarioEnunciado : null,
                        EstiloItemQuestionario = itemQuestionario.EstiloItemQuestionario,
                        TipoItemQuestionario = itemQuestionario.TipoItemQuestionario,
                        ValorQuestao = itemQuestionario.ValorQuestao,
                        NomeGabarito = itemQuestionario.NomeGabarito,
                        Feedback = itemQuestionario.Feedback,
                        Comentario = itemQuestionario.Comentario,
                        Ordem = itemQuestionario.Ordem,
                        InAvaliaProfessor = itemQuestionario.InAvaliaProfessor,
                        ExibeFeedback = itemQuestionario.ExibeFeedback,
                        RespostaObrigatoria = itemQuestionario.RespostaObrigatoria,
                        Auditoria = new classes.Auditoria(usuarioLogado.CPF)
                    };

                    if (itemQuestionario.ListaItemQuestionarioOpcoes != null)
                    {
                        foreach(var itemQuestionarioOpcao in itemQuestionario.ListaItemQuestionarioOpcoes)
                        {
                            itemQuestionarioDuplicado.ListaItemQuestionarioOpcoes.Add(new classes.ItemQuestionarioOpcoes()
                            {
                                ItemQuestionario = itemQuestionarioDuplicado,
                                Nome = itemQuestionarioOpcao.Nome,
                                RespostaCorreta = itemQuestionarioOpcao.RespostaCorreta,
                                TipoDiagnostico = itemQuestionarioOpcao.TipoDiagnostico,
                                OpcaoInt = itemQuestionarioOpcao.OpcaoInt,
                                OpcaoVinculada = itemQuestionarioOpcao.OpcaoVinculada != null ? itemQuestionarioDuplicado.ListaItemQuestionarioOpcoes.LastOrDefault() : null
                            });
                        }
                    }

                    QuestionarioDaSessao.ListaItemQuestionario.Add(itemQuestionarioDuplicado);

                    LimparCamposItemQuestionario();
                    WebFormHelper.PreencherGrid(QuestionarioDaSessao.ListaItemQuestionario.OrderBy(x => x.Ordem).ToList(), dgvItensDoQuestionario);

                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Item do Questionário duplicado com suscesso!");
                }
            }
            else
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Erro ao recuperar o item do questionário");
            }
        }

        private void CarregarDadosDoCadastroDeItensDoQuestionario(classes.ItemQuestionario itemQuestionario)
        {
            // Caso a questão tenha enunciado, buscar pelo ID do enunciado, ou pelo nome da questão do enunciado.
            if (itemQuestionario.QuestionarioEnunciado != null)
            {
                if (itemQuestionario.QuestionarioEnunciado.ID >= 0)
                {
                    ddlQuestionarioEnunciado.SelectedValue = itemQuestionario.QuestionarioEnunciado.ID.ToString();
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(itemQuestionario.QuestionarioEnunciado.Questao) == false)
                    {
                        var enunciado =
                            QuestionarioDaSessao.ListaItemQuestionario.FirstOrDefault(
                                x =>
                                    x.TipoItemQuestionario.ID == (int)enumTipoItemQuestionario.AgrupadorDeQuestoes &&
                                    x.Questao == itemQuestionario.QuestionarioEnunciado.Questao);

                        if (enunciado != null)
                        {
                            ddlQuestionarioEnunciado.SelectedValue = enunciado.ID.ToString();
                        }
                    }
                }
            }

            txtQuestaoNome.Text = itemQuestionario.Questao;
            txtFeedback.Text = itemQuestionario.Feedback;
            WebFormHelper.SetarValorNoRadioButtonList(itemQuestionario.ExibeFeedback, rdbExibeFeedback);
            divFeedback.Visible = itemQuestionario.ExibeFeedback == true;
            txtValorQuestao.Text = itemQuestionario.ValorQuestao.ToString(CultureInfo.InvariantCulture);
            txtGabaritoQuestao.Text = itemQuestionario.NomeGabarito;
            ddlInAvaliaProfessor.SelectedValue = Convert.ToInt32(itemQuestionario.InAvaliaProfessor).ToString();

            ddlTipoItemQuestionario.SelectedValue = itemQuestionario.TipoItemQuestionario.ID.ToString();
            ddlTipoItemQuestionario.Enabled = false;
            pnlItemQuestionario.Visible = true;

            ValidarVisibilidadeCamposModal(itemQuestionario, true);

            if (itemQuestionario.EstiloItemQuestionario == null)
            {
                ddlEstiloItemQuestionario.SelectedValue = (new classes.EstiloItemQuestionario { ID = 1 }).ID.ToString();
            }
            else
            {                
                var selectdValue = ddlEstiloItemQuestionario.Items.FindByValue(itemQuestionario.EstiloItemQuestionario.ID.ToString());
                if (selectdValue != null)
                {
                    ddlEstiloItemQuestionario.SelectedValue = selectdValue.Value;
                }
                else
                {
                    ddlEstiloItemQuestionario.SelectedValue = (new classes.EstiloItemQuestionario { ID = 1 }).ID.ToString();
                }                

            }
            
            WebFormHelper.SetarValorNoRadioButtonList(itemQuestionario.RespostaObrigatoria, rdbRespostaObrigatoria);
            
            var opcoes =
                itemQuestionario.ListaItemQuestionarioOpcoes.Where(
                    x =>
                        x.ItemQuestionario.TipoItemQuestionario.ID != (int)enumTipoItemQuestionario.ColunasRelacionadas ||
                        x.OpcaoVinculada != null).ToList();

            WebFormHelper.PreencherGrid(opcoes, dgvItemQuestionarioOpcoes);

            ItemQuestionarioBackup = (classes.ItemQuestionario)itemQuestionario.Clone();
        }

        protected void ddlTipoItemQuestionario_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlItemQuestionario.Visible = true;
            ddlTipoItemQuestionario.Enabled = false;

            var indexOf = int.Parse(hdIndexOfItemQuestionario.Value);

            if (QuestionarioDaSessao.ListaItemQuestionario != null && QuestionarioDaSessao.ListaItemQuestionario[indexOf].TipoItemQuestionario == null)
                QuestionarioDaSessao.ListaItemQuestionario[indexOf].TipoItemQuestionario =
                    new classes.TipoItemQuestionario();

            QuestionarioDaSessao.ListaItemQuestionario[indexOf].TipoItemQuestionario =
                new ManterTipoItemQuestionario().ObterPorID(int.Parse(ddlTipoItemQuestionario.SelectedValue));

            var itemQuestionario = QuestionarioDaSessao.ListaItemQuestionario[indexOf];

            ValidarVisibilidadeCamposModal(itemQuestionario);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimparQuestionarioDaSessao();
            Response.Redirect("ListarQuestionario.aspx");
        }

        protected void btnCancelarItemQuestionarioOpcao_Click(object sender, EventArgs e)
        {
            ResetarItemQuestionarioOpcoes();
        }

        protected void ResetarItemQuestionarioOpcoes()
        {
            btnEnviarItemQuestionarioOpcao.Visible =
                btnCancelarItemQuestionarioOpcao.Visible =
                    divRespostaCorreta.Visible =
                        divCampoItemQuestionarioOpcao.Visible =
                            divValorDaResposta.Visible =
                                divColunasRelacionadas.Visible = false;

            rblRespostaCorreta.Checked = false;

            txtItemQuestionarioOpcao.Text = "";
            hdAcaoItemQuestionarioOpcao.Value = "";
            txtResposta_coluna1.Text = "";
            txtResposta_coluna2.Text = "";

            ddlEstiloItemQuestionario.ClearSelection();

            var itemQuestionarioEditado = QuestionarioDaSessao.ListaItemQuestionario[int.Parse(hdIndexOfItemQuestionario.Value)];

            // Se for tipo V ou F define a exibição do item botão de adicionar mais se existirem opções cadastradas.
            btnAdicionarItemQuestionarioOpcao.Visible =
                itemQuestionarioEditado.TipoItemQuestionario.ID != (int)enumTipoItemQuestionario.VerdadeiroOuFalso
                || !itemQuestionarioEditado.ListaItemQuestionarioOpcoes.Any();

            var opcoes = itemQuestionarioEditado.ListaItemQuestionarioOpcoes.Where(
                    x =>
                        x.ItemQuestionario.TipoItemQuestionario.ID != (int)enumTipoItemQuestionario.ColunasRelacionadas ||
                        x.OpcaoVinculada != null).ToList();

            WebFormHelper.PreencherGrid(opcoes, dgvItemQuestionarioOpcoes);
        }

        protected void btnEnviarItemQuestionarioOpcao_Click(object sender, EventArgs e)
        {
            try
            {
                // Obter o tipo da questão.
                var tipoItemQuestionario =
                    (enumTipoItemQuestionario)int.Parse(ddlTipoItemQuestionario.SelectedValue);

                int indexItemQuestionario;

                // Obter os Ids.
                int.TryParse(hdIndexOfItemQuestionario.Value, out indexItemQuestionario);

                if (hdAcaoItemQuestionarioOpcao.Value == "EDITAR")
                {
                    // Obter o index do bichão sendo editado.
                    var indexItemQuestionarioOpcaoEditado = int.Parse(hdIndexOfItemQuestionarioOpcao.Value);
                    var limparGrid = false;

                    switch (tipoItemQuestionario)
                    {
                        case enumTipoItemQuestionario.Objetiva:
                        case enumTipoItemQuestionario.Discursiva:
                        case enumTipoItemQuestionario.MultiplaEscolha:
                        case enumTipoItemQuestionario.AgrupadorDeQuestoes:
                        case enumTipoItemQuestionario.Diagnostico:

                            QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                .ListaItemQuestionarioOpcoes[
                                    indexItemQuestionarioOpcaoEditado].Nome = txtItemQuestionarioOpcao.Text;

                            // Caso o campo "Resposta Correta" esteja selecionado, faz a mágica para setar ele.
                            if (rblRespostaCorreta.Checked &&
                                tipoItemQuestionario != enumTipoItemQuestionario.MultiplaEscolha)
                                QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                    .ListaItemQuestionarioOpcoes
                                    .ToList().ForEach(x => x.RespostaCorreta = false);

                            QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                .ListaItemQuestionarioOpcoes[
                                    indexItemQuestionarioOpcaoEditado].RespostaCorreta = rblRespostaCorreta.Checked;

                            break;
                        case enumTipoItemQuestionario.VerdadeiroOuFalso:
                            throw new AcademicoException("Código obsoleto sendo utilizado.");

                            limparGrid = true;

                            // Esconde o botão de adicionar novas opções para não permitir mais envios.
                            btnAdicionarItemQuestionarioOpcao.Visible = false;
                            break;
                        case enumTipoItemQuestionario.ColunasRelacionadas:
                            // Obtém os itens originais antes da edição.
                            var nomeOriginal1 = QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                .ListaItemQuestionarioOpcoes.Where(x => x.OpcaoVinculada == null).ToList()
                                [indexItemQuestionarioOpcaoEditado].Nome;

                            var itemOriginal2 = QuestionarioDaSessao
                                .ListaItemQuestionario[indexItemQuestionario]
                                .ListaItemQuestionarioOpcoes.FirstOrDefault(
                                    x => x.OpcaoVinculada != null && x.OpcaoVinculada.Nome == nomeOriginal1);

                            if (itemOriginal2 != null)
                            {
                                var nomeOriginal2 = itemOriginal2.Nome;

                                // Pegar com o AutoMapper por causa do Virtual, que permite alterações em objetos da lista estando fora da lista.
                                // Incluir a coluna 1 e relacionar na coluna 2.
                                var coluna1 =
                                    Mapper.Map<classes.ItemQuestionarioOpcoes>(
                                        QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                            .ListaItemQuestionarioOpcoes.Where(x => x.OpcaoVinculada == null)
                                            .ToList()[indexItemQuestionarioOpcaoEditado]) ??
                                    new classes.ItemQuestionarioOpcoes
                                    {
                                        ItemQuestionario =
                                            QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                    };

                                coluna1.Nome = txtResposta_coluna1.Text;

                                // Obter a coluna2 com base na coluna1.
                                var coluna2 =
                                    Mapper.Map<classes.ItemQuestionarioOpcoes>(QuestionarioDaSessao
                                        .ListaItemQuestionario[indexItemQuestionario]
                                        .ListaItemQuestionarioOpcoes.FirstOrDefault(
                                            x =>
                                                x.OpcaoVinculada != null &&
                                                (coluna1.ID != 0
                                                    ? x.OpcaoVinculada.ID == coluna1.ID
                                                    : x.OpcaoVinculada.Nome == coluna1.Nome))) ??
                                    new classes.ItemQuestionarioOpcoes
                                    {
                                        Nome = txtResposta_coluna2.Text,
                                        OpcaoVinculada = coluna1,
                                        ItemQuestionario =
                                            QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                    };

                                coluna2.Nome = txtResposta_coluna2.Text;

                                // Remover as opções atuais para serem inseridas novamente.
                                QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                    .ListaItemQuestionarioOpcoes.RemoveAt(indexItemQuestionarioOpcaoEditado * 2);

                                QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                    .ListaItemQuestionarioOpcoes.RemoveAt(indexItemQuestionarioOpcaoEditado * 2);

                                var opcoes = QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                    .ListaItemQuestionarioOpcoes.ToList();

                                // Verificar se o nome das colunas já existem e não permitir que sejam cadastrados opções com nomes iguais para esse ItemTrilha.
                                if (opcoes.Any(x => x.OpcaoVinculada == null && x.Nome == coluna1.Nome))
                                {
                                    coluna1.Nome = nomeOriginal1;
                                    coluna2.Nome = nomeOriginal2;

                                    // Adiciona novamente as opções, pois não deve remover caso caia nesse erro.
                                    QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                        .ListaItemQuestionarioOpcoes.Insert(indexItemQuestionarioOpcaoEditado, coluna1);
                                    QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                        .ListaItemQuestionarioOpcoes.Insert(indexItemQuestionarioOpcaoEditado, coluna2);

                                    throw new AcademicoException(
                                        string.Format(
                                            "O nome \"{0}\" já existe nas opções da coluna 1 cadastradas e não pode ser utilizada.",
                                            coluna1.Nome));
                                }

                                if (opcoes.Any(x => x.OpcaoVinculada != null && x.Nome == coluna2.Nome))
                                {
                                    coluna1.Nome = nomeOriginal1;
                                    coluna2.Nome = nomeOriginal2;

                                    // Adiciona novamente as opções, pois não deve remover caso caia nesse erro.
                                    QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                        .ListaItemQuestionarioOpcoes.Insert(indexItemQuestionarioOpcaoEditado, coluna1);
                                    QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                        .ListaItemQuestionarioOpcoes.Insert(indexItemQuestionarioOpcaoEditado, coluna2);

                                    throw new AcademicoException(
                                        string.Format(
                                            "O nome \"{0}\" já existe nas opções da coluna 2 cadastradas e não pode ser utilizada.",
                                            coluna2.Nome));
                                }

                                // Adicionar as opções das colunas novamente.
                                QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                    .ListaItemQuestionarioOpcoes.Add(coluna1);
                                QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                    .ListaItemQuestionarioOpcoes.Add(coluna2);
                            }

                            limparGrid = true;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    if (limparGrid)
                    {
                        dgvItemQuestionarioOpcoes.DataSource = null;
                        dgvItemQuestionarioOpcoes.DataBind();
                    }

                    // Resetar conteúdo.
                    ResetarItemQuestionarioOpcoes();
                }
                else if (hdAcaoItemQuestionarioOpcao.Value == "INCLUIR")
                {
                    // Como é criação, aqui a gente cria o bichão.
                    if (QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario].ListaItemQuestionarioOpcoes ==
                        null)
                    {
                        QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario].ListaItemQuestionarioOpcoes =
                            new List<classes.ItemQuestionarioOpcoes>();
                    }

                    switch (tipoItemQuestionario)
                    {
                        case enumTipoItemQuestionario.Objetiva:
                        case enumTipoItemQuestionario.Discursiva:
                        case enumTipoItemQuestionario.MultiplaEscolha:
                        case enumTipoItemQuestionario.AgrupadorDeQuestoes:
                        case enumTipoItemQuestionario.Diagnostico:
                            var novoItem = new classes.ItemQuestionarioOpcoes
                            {
                                Nome = txtItemQuestionarioOpcao.Text,
                                RespostaCorreta = rblRespostaCorreta.Checked,
                                ItemQuestionario = QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                            };

                            if (novoItem.RespostaCorreta &&
                                tipoItemQuestionario != enumTipoItemQuestionario.MultiplaEscolha)
                                QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                    .ListaItemQuestionarioOpcoes
                                    .ToList().ForEach(x => x.RespostaCorreta = false);

                            var contemOpcao = QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                .ListaItemQuestionarioOpcoes
                                .Select(x => x.Nome)
                                .Contains(novoItem.Nome);

                            if (contemOpcao)
                            {
                                break;
                            }
                            QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                .ListaItemQuestionarioOpcoes.Add(
                                    novoItem);

                            break;
                        case enumTipoItemQuestionario.VerdadeiroOuFalso:
                            var valorSelecionado = rdbValorDaResposta.SelectedValue == "V";

                            var verdadeiro = new classes.ItemQuestionarioOpcoes
                            {
                                Nome = "V",
                                RespostaCorreta = valorSelecionado,
                                ItemQuestionario = QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                            };

                            var falso = new classes.ItemQuestionarioOpcoes
                            {
                                Nome = "F",
                                RespostaCorreta = valorSelecionado == false,
                                ItemQuestionario = QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                            };

                            // Adicionar as opções de Verdadeiro ou Falso
                            QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                .ListaItemQuestionarioOpcoes.Add(verdadeiro);
                            QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                .ListaItemQuestionarioOpcoes.Add(falso);

                            break;
                        case enumTipoItemQuestionario.ColunasRelacionadas:
                            // Incluir a coluna 1 e relacionar na coluna 2.
                            var coluna1 = new classes.ItemQuestionarioOpcoes
                            {
                                Nome = txtResposta_coluna1.Text,
                                ItemQuestionario = QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                            };

                            var coluna2 = new classes.ItemQuestionarioOpcoes
                            {
                                Nome = txtResposta_coluna2.Text,
                                OpcaoVinculada = coluna1,
                                ItemQuestionario = QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                            };

                            var opcoes = QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                .ListaItemQuestionarioOpcoes;

                            // Verificar se o nome das colunas já existem e não permitir que sejam cadastrados opções com nomes iguais para esse ItemTrilha.
                            if (opcoes.Any(x => x.OpcaoVinculada == null && x.Nome == coluna1.Nome))
                                throw new AcademicoException(
                                    string.Format(
                                        "O nome \"{0}\" já existe nas opções da coluna 1 cadastradas e não pode ser utilizada.",
                                        coluna1.Nome));

                            if (opcoes.Any(x => x.OpcaoVinculada != null && x.Nome == coluna2.Nome))
                                throw new AcademicoException(
                                    string.Format(
                                        "O nome \"{0}\" já existe nas opções da coluna 2 cadastradas e não pode ser utilizada.",
                                        coluna2.Nome));

                            // Adicionar as opções das colunas.
                            QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                .ListaItemQuestionarioOpcoes.Add(coluna1);
                            QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                .ListaItemQuestionarioOpcoes.Add(coluna2);

                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    ResetarItemQuestionarioOpcoes();
                }
                //else
                //{
                //    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Erro ao recuperar a operação");
                //}
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
            catch (Exception)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Erro ao recuperar informações da resposta");
            }
        }

        protected void btnEnviarItemQuestionario_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hdIndexOfItemQuestionario.Value))
            {
                try
                {

                    int indexItemQuestionario;
                    if(!int.TryParse(hdIndexOfItemQuestionario.Value, out indexItemQuestionario))
                        throw new AcademicoException("Erro ao recuperar os dados da sessão");

                    var itemQuestionario = QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario];

                    itemQuestionario.Ordem = indexItemQuestionario;

                    var tipoItemQuestionario = (enumTipoItemQuestionario)itemQuestionario.TipoItemQuestionario.ID;

                    if (txtQuestaoNome.Text.Length > 2000)
                    {
                        throw new AcademicoException("O enunciado deve ter no máximo 2000 caracteres");
                    }

                    if ((tipoItemQuestionario == enumTipoItemQuestionario.Objetiva ||
                         tipoItemQuestionario == enumTipoItemQuestionario.MultiplaEscolha ||
                         tipoItemQuestionario == enumTipoItemQuestionario.Diagnostico ||
                         tipoItemQuestionario == enumTipoItemQuestionario.VerdadeiroOuFalso ||
                         tipoItemQuestionario == enumTipoItemQuestionario.ColunasRelacionadas) &&
                        !itemQuestionario.ListaItemQuestionarioOpcoes.Any())
                    {
                        throw new AcademicoException("Não existem opções cadastradas para a questão");
                    }

                    // Removido tipo objetiva devido solicitacao do Robson BUG #1227
                    //tipoItemQuestionario == enumTipoItemQuestionario.Objetiva ||   
                    if ((tipoItemQuestionario == enumTipoItemQuestionario.Objetiva ||
                         tipoItemQuestionario == enumTipoItemQuestionario.Diagnostico) &&
                        !itemQuestionario.ListaItemQuestionarioOpcoes.Any(x => x.RespostaCorreta))
                    {
                        throw new AcademicoException("Deve existir pelo menos uma resposta correta");
                    }

                    //ddlTipoItemQuestionario
                    QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario].InAvaliaProfessor =
                        Convert.ToBoolean(int.Parse(ddlInAvaliaProfessor.SelectedValue));
                    QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario].Questao = txtQuestaoNome.Text;
                    QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario].Feedback = txtFeedback.Text;
                    QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario].ExibeFeedback = rdbExibeFeedback.SelectedValue == "S";
                    QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario].RespostaObrigatoria = rdbRespostaObrigatoria.SelectedValue == "S";

                    if (!string.IsNullOrEmpty(ddlEstiloItemQuestionario.SelectedValue) &&
                        int.Parse(ddlEstiloItemQuestionario.SelectedValue) > 0)
                    {
                        if (QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario].EstiloItemQuestionario ==
                            null)
                        {
                            QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario].EstiloItemQuestionario =
                                new classes.EstiloItemQuestionario();
                        }

                        QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario].EstiloItemQuestionario.ID =
                            int.Parse(ddlEstiloItemQuestionario.SelectedValue);
                    }
                    else
                    {
                        QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario].EstiloItemQuestionario = null;
                    }

                    QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario].NomeGabarito =
                        txtGabaritoQuestao.Text;

                    QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario].TipoItemQuestionario =
                        (new ManterTipoItemQuestionario()).ObterTipoOfertaPorID(
                            int.Parse(ddlTipoItemQuestionario.SelectedValue));

                    SetarEnunciado(indexItemQuestionario);

                    if (!txtValorQuestao.Visible)
                        txtValorQuestao.Text = "1";

                    //Valor da Questão

                    if (!string.IsNullOrWhiteSpace(txtValorQuestao.Text))
                    {
                        decimal qtdValorQuestao;
                        if (decimal.TryParse(txtValorQuestao.Text.Replace(",", "."), NumberStyles.Number,
                            CultureInfo.InvariantCulture, out qtdValorQuestao))
                        {
                            if (qtdValorQuestao < 1)
                                throw new AcademicoException("O valor mínimo da questão é deve ser 1.");

                            QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario].ValorQuestao =
                                qtdValorQuestao;
                        }
                    }
                    else
                    {
                        throw new AcademicoException("O valor da questão é obrigatório.");
                    }
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }
                catch
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro,
                        "Erro ao recuprar os dados da sessão para edição");
                }
            }
            //else
            //{
            //    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Erro ao recuperar estado a solicitação para edição");
            //}

            OcultarModal();
            LimparCamposItemQuestionario();
            WebFormHelper.PreencherGrid(QuestionarioDaSessao.ListaItemQuestionario.OrderBy(x => x.Ordem).ToList(),
                dgvItensDoQuestionario);
        }

        protected void btnCancelarItemQuestionario_Click(object sender, EventArgs e)
        {
            FecharModalCancelando();
        }

        #region "Controle Visibilidade"

        private void ExibirModal()
        {
            base.ExibirBackDrop();
            pnlModal.Visible = true;
            PreencherQuestionariosEnunciados();
        }


        private void PreencherQuestionariosEnunciados()
        {
            var listaItensQuestionario =
                QuestionarioDaSessao.ListaItemQuestionario.Where(
                    x => x.TipoItemQuestionario.ID == (int) enumTipoItemQuestionario.AgrupadorDeQuestoes).ToList();

            ddlQuestionarioEnunciado.DataValueField = "ID";
            ddlQuestionarioEnunciado.DataTextField = "Nome";

            var itens = new List<dynamic>
            {
                new
                {
                    ID = -1,
                    Nome = "- Nenhuma Questão -"
                }
            };
            
            itens.AddRange(listaItensQuestionario.Select(x => new { ID = x.ID, Nome = TratarHtml(x.Questao) }));

            ddlQuestionarioEnunciado.DataSource = itens;
            ddlQuestionarioEnunciado.DataBind();
        }


        private string TratarHtml(string questaoEnunciado)
        {
            var questao = Regex.Replace(questaoEnunciado ?? "", "<.*?>", "");
            return (questao.Length > 100) ? HttpUtility.HtmlDecode(questao.Substring(0, 100)) : HttpUtility.HtmlDecode(questao);
        }

        protected void OcultarModal_Click(object sender, EventArgs e)
        {
            FecharModalCancelando();
        }

        private void OcultarModal()
        {
            OcultarBackDrop();
            pnlModal.Visible = false;
        }

        private void ValidarVisibilidadeCamposModal(classes.ItemQuestionario itemQuestionario, bool exibirResposta = false)
        {
            enumTipoItemQuestionario? tipoItemQuestionarioSelecionado =
                (enumTipoItemQuestionario)int.Parse(ddlTipoItemQuestionario.SelectedValue);

            divCampoItemQuestionarioOpcao.Visible = false;
            divGabarito.Visible = false;
            divRespostaCorreta.Visible = false;
            divValorQuestao.Visible = false;
            btnEnviarItemQuestionarioOpcao.Visible = false;
            btnCancelarItemQuestionarioOpcao.Visible = false;
            pnlCamposItemQuestionarioOpcao.Visible = false;
            ddlEstiloItemQuestionario.Enabled = true;
            divAvaliaTutor.Visible = true;
            divColunasRelacionadas.Visible = false;
            divExibeFeedback.Visible = true;
            divFeedback.Visible = itemQuestionario.ExibeFeedback == true;
            divValorDaResposta.Visible = false;

            labelQuestaoNome.Text = tipoItemQuestionarioSelecionado == enumTipoItemQuestionario.AgrupadorDeQuestoes
                ? "Nome do agrupador"
                : "Enunciado / Pergunta";

            
            PreencherComboEstiloItemQuestionario(itemQuestionario, tipoItemQuestionarioSelecionado);
            DefinirVisibilidadeColunasOpcoes(tipoItemQuestionarioSelecionado.Value);

            switch (tipoItemQuestionarioSelecionado)
            {
                case enumTipoItemQuestionario.AgrupadorDeQuestoes:
                    divQuestaoEnunciado.Visible = false;
                    divAvaliaTutor.Visible = false;
                    divExibeFeedback.Visible = false;
                    divFeedback.Visible = false;
                    break;
                case enumTipoItemQuestionario.Discursiva:
                    divQuestaoEnunciado.Visible = true;
                    if (ddlTipoItemQuestionario.SelectedValue == ((int)enumTipoQuestionario.AvaliacaoProva).ToString())
                    {
                        divGabarito.Visible = true;
                        divValorQuestao.Visible = true;
                    }

                    break;
                case enumTipoItemQuestionario.Objetiva:
                    divQuestaoEnunciado.Visible = true;
                    pnlCamposItemQuestionarioOpcao.Visible = true;
                    ddlEstiloItemQuestionario.Visible = true;

                    if (exibirResposta)
                    {
                        divCampoItemQuestionarioOpcao.Visible = true;
                        divRespostaCorreta.Visible = true;
                    }
                    dgvItemQuestionarioOpcoes.DataBind();
                    if (ddlTipoItemQuestionario.SelectedValue == ((int)enumTipoQuestionario.AvaliacaoProva).ToString())
                    {
                        divValorQuestao.Visible = true;
                    }

                    break;
                case enumTipoItemQuestionario.MultiplaEscolha:
                    divQuestaoEnunciado.Visible = true;
                    pnlCamposItemQuestionarioOpcao.Visible = true;
                    if (exibirResposta)
                    {
                        divCampoItemQuestionarioOpcao.Visible = true;
                    }
                    dgvItemQuestionarioOpcoes.DataBind();
                    if (ddlTipoItemQuestionario.SelectedValue == ((int)enumTipoQuestionario.AvaliacaoProva).ToString())
                    {
                        divValorQuestao.Visible = true;
                    }

                    break;
                case enumTipoItemQuestionario.VerdadeiroOuFalso:
                    divQuestaoEnunciado.Visible = true;
                    // Esconder campos inúteis para V ou F
                    ddlEstiloItemQuestionario.Enabled =
                        divAvaliaTutor.Visible =
                            btnAdicionarItemQuestionarioOpcao.Visible = false;

                    divValorDaResposta.Visible = true;

                    if (itemQuestionario.ListaItemQuestionarioOpcoes.Any())
                    {
                        var opcaoCorreta =
                            itemQuestionario.ListaItemQuestionarioOpcoes.FirstOrDefault(x => x.RespostaCorreta);

                        if (opcaoCorreta != null)
                            rdbValorDaResposta.SelectedValue =
                                opcaoCorreta.Nome;
                    }

                    break;
                case enumTipoItemQuestionario.ColunasRelacionadas:
                    divQuestaoEnunciado.Visible = true;
                    divAvaliaTutor.Visible = false;

                    pnlCamposItemQuestionarioOpcao.Visible = true;

                    break;
            }
        }

        #endregion

        protected string EmiteNotificacao(classes.Questionario questionarioEdicao)
        {
            var msg = "";

            // Disponível somente se for um questionário avulso
            if (questionarioEdicao.TipoQuestionario.ID != (int)enumTipoQuestionario.Avulso)
                return msg;

            // Notifica os usuários
            if (rblNotificacao.SelectedItem == null || !rblNotificacao.SelectedItem.Value.Equals("S"))
                return msg;

            // Pega as permissões do questionário para criar as respectivas notificações
            var perfis =
                questionarioEdicao.ListaQuestionarioPermissao.Where(x => x.Perfil != null)
                    .Select(x => x.Perfil)
                    .ToList();

            var niveis =
                questionarioEdicao.ListaQuestionarioPermissao.Where(x => x.NivelOcupacional != null)
                    .Select(x => x.NivelOcupacional)
                    .ToList();

            var ufs = questionarioEdicao.ListaQuestionarioPermissao.Where(x => x.Uf != null).Select(x => x.Uf).ToList();

            var manterNot = new ManterNotificacao();

            var usuarios = new ManterUsuario().ObterPorUfsNiveisPerfis(ufs, niveis, perfis);

            manterNot.PublicarNotificacao("/questionario/avulso/" + questionarioEdicao.ID,
                "Nova Pesquisa: " + questionarioEdicao.Nome, usuarios);

            msg = "<br />Todas as notificações serão enviadas em até uma hora.";

            return msg;
        }

        protected void btnDuplicarQuestionario_Click(object sender, EventArgs e)
        {
            try
            {
                var questionario = QuestionarioDaSessao;

                var usuarioLogado = _manterUsuario.ObterUsuarioLogado();

                if (usuarioLogado.IsGestor())
                    questionario.Uf = new ManterUf().ObterUfPorID(usuarioLogado.UF.ID);

                _manterQuestionario.DuplicarQuestionario(questionario);

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Questionário duplicado com suscesso!",
                    "ListarQuestionario.aspx");
            }
            catch
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Ocorreu um erro ao tentar duplicar o questionário");
            }
        }

        protected void dgvMatriculaOferta_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;
            var itemQuestrionario = (classes.ItemQuestionario)e.Row.DataItem;

            var indexItemQuestionario = QuestionarioDaSessao.ListaItemQuestionario.IndexOf(itemQuestrionario);
            e.Row.Attributes.Add("id", indexItemQuestionario.ToString());

            var rdbValorDaResposta = (RadioButtonList)e.Row.FindControl("rdbValorDaResposta");

            rdbValorDaResposta.Attributes.Add("data-id", indexItemQuestionario.ToString());

            if (itemQuestrionario.TipoItemQuestionario != null && itemQuestrionario.TipoItemQuestionario.ID == (int)enumTipoItemQuestionario.VerdadeiroOuFalso)
            {
                rdbValorDaResposta.SelectedValue = itemQuestrionario.ListaItemQuestionarioOpcoes.Where(x => x.RespostaCorreta).FirstOrDefault().Nome;
                rdbValorDaResposta.Visible = true;
            }
            else
            {
                rdbValorDaResposta.Visible = false;
            }
        }

        private void DefinirVisibilidadeColunasOpcoes(enumTipoItemQuestionario tipoItemQuestionario)
        {
            switch (tipoItemQuestionario)
            {
                case enumTipoItemQuestionario.MultiplaEscolha:
                    dgvItemQuestionarioOpcoes.Columns[1].Visible = false;
                    dgvItemQuestionarioOpcoes.Columns[2].Visible = false;
                    break;
                case enumTipoItemQuestionario.ColunasRelacionadas:
                    dgvItemQuestionarioOpcoes.Columns[1].Visible = true;
                    dgvItemQuestionarioOpcoes.Columns[2].Visible = false;
                    break;
                default:
                    dgvItemQuestionarioOpcoes.Columns[1].Visible = false;
                    dgvItemQuestionarioOpcoes.Columns[2].Visible = true;
                    break;
            }
        }

        protected void rdbExibeFeedback_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            // Só exibe o feedback ao selecionar a opção "Sim".
            divFeedback.Visible = rdbExibeFeedback.SelectedValue == "S";
        }

        protected void dgvItemQuestionarioOpcoes_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            var opcao = (classes.ItemQuestionarioOpcoes)e.Row.DataItem;

            if (opcao != null)
            {
                var chkRespostaCorreta = (CheckBox)e.Row.FindControl("chkRespostaCorreta");

                chkRespostaCorreta.Attributes.Add("opcao_index", e.Row.RowIndex.ToString());

                if (opcao.RespostaCorreta)
                {
                    //chkRespostaCorreta.Enabled = false;
                    chkRespostaCorreta.Checked = true;

                    chkRespostaCorreta.CheckedChanged -= chkRespostaCorreta_OnCheckedChanged;
                }
                else
                    chkRespostaCorreta.CheckedChanged += chkRespostaCorreta_OnCheckedChanged;
            }
        }

        protected void chkRespostaCorreta_OnCheckedChanged(object sender, EventArgs e)
        {

            try
            {
                var chkRespostaCorreta = (CheckBox)sender;
                var opcaoIndex = int.Parse(chkRespostaCorreta.Attributes["opcao_index"]);
                var opcoes = QuestionarioDaSessao
                    .ListaItemQuestionario[int.Parse(hdIndexOfItemQuestionario.Value)]
                    .ListaItemQuestionarioOpcoes;

                for (var i = 0; i < opcoes.Count(); i++)
                {

                    opcoes[i].RespostaCorreta = i == opcaoIndex;
                }

                WebFormHelper.PreencherGrid(opcoes, dgvItemQuestionarioOpcoes);

            }
            catch (Exception ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, ex.Message);
            }

        }

        protected void rdbValorDaResposta_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var indexItemQuestionario = int.Parse(hdIndexOfItemQuestionario.Value);

            var valorSelecionado = rdbValorDaResposta.SelectedValue == "V";

            var verdadeiro =
                QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                    .ListaItemQuestionarioOpcoes.FirstOrDefault(x => x.Nome == "V") ??
                new classes.ItemQuestionarioOpcoes
                {
                    Nome = "V",
                    ItemQuestionario =
                        QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                };

            verdadeiro.RespostaCorreta = valorSelecionado;

            var falso =
                QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                    .ListaItemQuestionarioOpcoes.FirstOrDefault(x => x.Nome == "F") ??
                new classes.ItemQuestionarioOpcoes
                {
                    Nome = "F",
                    ItemQuestionario =
                        QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                };

            falso.RespostaCorreta = valorSelecionado == false;

            // Limpa a listagem e recria.
            QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                .ListaItemQuestionarioOpcoes.Clear();

            // Adicionar as opções de Verdadeiro ou Falso
            QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                .ListaItemQuestionarioOpcoes.Add(verdadeiro);
            QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                .ListaItemQuestionarioOpcoes.Add(falso);
        }

        protected void rdbValorDaRespostaLista_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var rdbValor = (RadioButtonList)sender;

            var indexItemQuestionario = int.Parse(rdbValor.Attributes["data-id"].ToString());

            if (QuestionarioDaSessao != null && QuestionarioDaSessao.ListaItemQuestionario.Count > 0)
            {
                //var questionarioEdicao = QuestionarioDaSessao.ListaItemQuestionario.FirstOrDefault(x => x.ID == indexItemQuestionario);
                var questionarioEdicao = QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario];

                if (questionarioEdicao != null)
                {
                    QuestionarioDaSessao.ListaItemQuestionario.Remove(questionarioEdicao);
                }

                var valorSelecionado = rdbValor.SelectedValue == "V";

                var verdadeiro =
                    (questionarioEdicao.ListaItemQuestionarioOpcoes.FirstOrDefault(x => x.Nome == "V")) ??
                    new classes.ItemQuestionarioOpcoes
                    {
                        Nome = "V",
                        ItemQuestionario = questionarioEdicao
                    };

                verdadeiro.RespostaCorreta = valorSelecionado;

                var falso =
                   (questionarioEdicao.ListaItemQuestionarioOpcoes.FirstOrDefault(x => x.Nome == "F")) ??
                    new classes.ItemQuestionarioOpcoes
                    {
                        Nome = "F",
                        ItemQuestionario = questionarioEdicao
                    };

                falso.RespostaCorreta = valorSelecionado == false;

                //// Limpa a listagem e recria.
                questionarioEdicao.ListaItemQuestionarioOpcoes = new List<classes.ItemQuestionarioOpcoes>()
            {
                verdadeiro,
                falso
            };

                //QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                //    .ListaItemQuestionarioOpcoes.Clear();

                // Adicionar as opções de Verdadeiro ou Falso
                QuestionarioDaSessao.ListaItemQuestionario.Add(questionarioEdicao);
            }
        }

        private void FormatarTabela()
        {
            //Manter a formatação (Header)
            if (dgvItemQuestionarioOpcoes.Rows.Count > 0)
            {
                dgvItemQuestionarioOpcoes.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            if (dgvItensDoQuestionario.Rows.Count > 0)
            {
                dgvItensDoQuestionario.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        /// <summary>
        /// Retorna o objeto de questionário com a últimas modificações do usuário
        /// </summary>
        /// <param name="permissoes"></param>
        /// <returns></returns>
        private classes.Questionario AtualizarQuestionarioEdicaoDaSessao(ref List<classes.QuestionarioPermissao> permissoes)
        {
            classes.Questionario questionarioEdicao;

            var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

            if (QuestionarioDaSessao.ID == 0)
            {
                questionarioEdicao = QuestionarioDaSessao;
                questionarioEdicao.TipoQuestionario = new classes.TipoQuestionario()
                {
                    ID = int.Parse(ddlTipoQuestionario.SelectedItem.Value),
                    Nome = ddlTipoQuestionario.SelectedItem.Text
                };

                if (usuarioLogado.IsGestor())
                    questionarioEdicao.Uf = new ManterUf().ObterUfPorID(usuarioLogado.UF.ID);
            }
            else
            {
                questionarioEdicao = (classes.Questionario)QuestionarioDaSessao.Clone();
            }

            questionarioEdicao.Nome = txtNome.Text.Trim();
            questionarioEdicao.PrazoMinutos = WebFormHelper.ConverterParaInteiroNull(txtPrazoemMinutos.Text);
            questionarioEdicao.QtdQuestoesProva = WebFormHelper.ConverterParaInteiroNull(txtQtdQuestoesDaProva.Text);

            questionarioEdicao.TextoEnunciado = txtTextoEnunciadoPre.Text.Trim();

            //Ativo
            if (rblAtivo.SelectedItem != null && !string.IsNullOrWhiteSpace(rblAtivo.SelectedItem.Value))
            {
                questionarioEdicao.Ativo = rblAtivo.SelectedItem.Value.Trim().ToUpper().Equals("S");
            }

            //if (questionarioEdicao.TipoQuestionario.ID == (int)enumTipoQuestionario.Avulso)
            // Adicionando inclusão de permissões para todos os tipos de questionários.
            ObterFormaAquisicao(questionarioEdicao);
            ObterPerfil(questionarioEdicao);
            ObterNivelOcupacional(questionarioEdicao);
            ObterUf(questionarioEdicao);

            if (questionarioEdicao.ID == 0)
            {
                permissoes = questionarioEdicao.ListaQuestionarioPermissao.ToList();
                questionarioEdicao.ListaQuestionarioPermissao.Clear();
            }

            var manterCategoriaConteudo = new ManterCategoriaConteudo();
            questionarioEdicao.ListaCategoriaConteudo =
                ucCategorias1.IdsCategoriasMarcadas.Select(id => manterCategoriaConteudo.ObterCategoriaConteudoPorID(id))
                    .ToList();

            // Buscando uf do usuário logado para tratar problema de gerenciamento de conexão do nhibernate.
            if (questionarioEdicao.Uf == null)
            {
                var idUf = _manterUsuario.ObterUsuarioLogado().UF.ID;
                questionarioEdicao.Uf = new ManterUf().ObterUfPorID(idUf);
            }
            else if (Request["Duplicar"] != null)
            {
                if (usuarioLogado.IsAdministrador())
                {
                    questionarioEdicao.Uf = null;
                }

                if (usuarioLogado.IsGestor())
                {
                    // Buscando uf do usuário logado para tratar problema de gerenciamento de conexão do nhibernate.
                    questionarioEdicao.Uf = new ManterUf().ObterUfPorID(usuarioLogado.UF.ID);
                }
            }

            // Nota mínima
            if (questionarioEdicao.TipoQuestionario.ID == (int)enumTipoQuestionario.AtividadeTrilha)
            {
                questionarioEdicao.NotaMinima = int.Parse(txtNotaMinima.Text);
            }

            return questionarioEdicao;
        }

        /// <summary>
        /// Retorna o objeto de questionário com as últimas modificações do item questionário do questionário 
        /// </summary>
        /// <returns></returns>
        private classes.Questionario AtualizarItemQuestionarioDaSessao()
        {
            if (!string.IsNullOrEmpty(hdIndexOfItemQuestionario.Value))
            {

                var indexItemQuestionario = int.Parse(hdIndexOfItemQuestionario.Value);

                var itemQuestionario = QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario];

                var tipoItemQuestionario = (enumTipoItemQuestionario)itemQuestionario.TipoItemQuestionario.ID;

                //ddlTipoItemQuestionario
                QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario].InAvaliaProfessor =
                    Convert.ToBoolean(int.Parse(ddlInAvaliaProfessor.SelectedValue));
                QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario].Questao = txtQuestaoNome.Text;
                QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario].Feedback = txtFeedback.Text;
                QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario].ExibeFeedback = rdbExibeFeedback.SelectedValue == "S";
                QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario].RespostaObrigatoria = rdbRespostaObrigatoria.SelectedValue == "S";

                if (!string.IsNullOrEmpty(ddlEstiloItemQuestionario.SelectedValue) &&
                    int.Parse(ddlEstiloItemQuestionario.SelectedValue) > 0)
                {
                    if (QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario].EstiloItemQuestionario ==
                        null)
                    {
                        QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario].EstiloItemQuestionario =
                            new classes.EstiloItemQuestionario();
                    }

                    QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario].EstiloItemQuestionario.ID =
                        int.Parse(ddlEstiloItemQuestionario.SelectedValue);
                }
                else
                {
                    QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario].EstiloItemQuestionario = null;
                }

                QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario].NomeGabarito =
                    txtGabaritoQuestao.Text;

                QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario].TipoItemQuestionario =
                    (new ManterTipoItemQuestionario()).ObterTipoOfertaPorID(
                        int.Parse(ddlTipoItemQuestionario.SelectedValue));

                SetarEnunciado(indexItemQuestionario);

                if (!txtValorQuestao.Visible)
                    txtValorQuestao.Text = "1";

                //Valor da Questão

                if (!string.IsNullOrWhiteSpace(txtValorQuestao.Text))
                {
                    decimal qtdValorQuestao;
                    if (decimal.TryParse(txtValorQuestao.Text.Replace(",", "."), NumberStyles.Number,
                        CultureInfo.InvariantCulture, out qtdValorQuestao))
                    {
                        QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario].ValorQuestao =
                            qtdValorQuestao;
                    }
                }
            }

            return QuestionarioDaSessao;
        }

        private void SetarEnunciado(int indexItemQuestionario)
        {
            var indexQuestionarioEnunciado = Convert.ToInt32(ddlQuestionarioEnunciado.SelectedIndex) - 1;
            var idQuestionarioEnunciado = Convert.ToInt32(ddlQuestionarioEnunciado.SelectedValue);

            if (indexQuestionarioEnunciado >= 0 || idQuestionarioEnunciado > 0)
            {
                // Se for edição, pegará do ID, se for criação, pegará do index.
                var enunciado = idQuestionarioEnunciado > 0
                    ? QuestionarioDaSessao.ListaItemQuestionario.FirstOrDefault(
                        x => x.ID == idQuestionarioEnunciado)
                    : QuestionarioDaSessao.ListaItemQuestionario.Where(
                        x => x.TipoItemQuestionario.ID == (int)enumTipoItemQuestionario.AgrupadorDeQuestoes)
                        .ToList()[indexQuestionarioEnunciado];

                if (enunciado != null)
                {
                    QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario].QuestionarioEnunciado = enunciado;
                }
            }
            else
            {
                QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario].QuestionarioEnunciado = null;
            }
        }

        private void ObterUf(classes.Questionario questionarioEdicao)
        {
            var rptUFs = (Repeater)ucPermissoes1.FindControl("rptUFs");

            for (var i = 0; i < rptUFs.Items.Count; i++)
            {
                var ckUf = (CheckBox)rptUFs.Items[i].FindControl("ckUF");

                var q = new classes.QuestionarioPermissao
                {
                    Uf = new classes.Uf { ID = Convert.ToInt32(ckUf.Attributes["ID_UF"]) },
                    Questionario = new classes.Questionario { ID = questionarioEdicao.ID }
                };


                if (ckUf.Checked)
                {
                    if (!questionarioEdicao.ListaQuestionarioPermissao.Any(x => x.Uf != null && x.Uf.ID == q.Uf.ID))
                    {
                        questionarioEdicao.ListaQuestionarioPermissao.Add(q);
                    }
                }
                else
                {
                    var qp =
                        questionarioEdicao.ListaQuestionarioPermissao.FirstOrDefault(
                            x => (x.Uf != null && x.Uf.ID == q.Uf.ID));
                    if (qp != null)
                    {
                        questionarioEdicao.ListaQuestionarioPermissao.Remove(qp);
                    }
                }
            }
        }

        // Atualiza questionário da sessão que será utilizado em caso de queda de sessão
        private void AtualizarQuestionarioSessao()
        {
            var manter = new ManterSessao();
            var questionario = QuestionarioSessaoJson;

            classes.Sessao sessao = QuestionarioSessao.HasValue ? manter.ObterObjetoPorHash(QuestionarioSessao.Value) : new classes.Sessao(QuestionarioSessaoJson);

            if (sessao != null)
            {
                QuestionarioSessao = sessao.Hash;
                sessao.Valor = QuestionarioSessaoJson;

                int tipoQuestionario;
                if (int.TryParse(ddlTipoQuestionario.SelectedValue, out tipoQuestionario))
                {
                    TipoQuestionarioSessao = tipoQuestionario;
                }

                manter.Salvar(sessao);
            }
        }

        private classes.Questionario AtualizarItemQuestionarioOpcaoDaSessao()
        {
            try
            {
                // Obter o tipo da questão.
                var tipoItemQuestionario =
                    (enumTipoItemQuestionario)int.Parse(ddlTipoItemQuestionario.SelectedValue);

                int indexItemQuestionario;

                // Obter os Ids.
                int.TryParse(hdIndexOfItemQuestionario.Value, out indexItemQuestionario);

                if (hdAcaoItemQuestionarioOpcao.Value == "EDITAR")
                {
                    // Obter o index do bichão sendo editado.
                    var indexItemQuestionarioOpcaoEditado = int.Parse(hdIndexOfItemQuestionarioOpcao.Value);
                    var limparGrid = false;

                    switch (tipoItemQuestionario)
                    {
                        case enumTipoItemQuestionario.Objetiva:
                        case enumTipoItemQuestionario.Discursiva:
                        case enumTipoItemQuestionario.MultiplaEscolha:
                        case enumTipoItemQuestionario.AgrupadorDeQuestoes:
                        case enumTipoItemQuestionario.Diagnostico:

                            QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                .ListaItemQuestionarioOpcoes[
                                    indexItemQuestionarioOpcaoEditado].Nome = txtItemQuestionarioOpcao.Text;

                            // Caso o campo "Resposta Correta" esteja selecionado, faz a mágica para setar ele.
                            if (rblRespostaCorreta.Checked &&
                                tipoItemQuestionario != enumTipoItemQuestionario.MultiplaEscolha)
                                QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                    .ListaItemQuestionarioOpcoes
                                    .ToList().ForEach(x => x.RespostaCorreta = false);

                            QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                .ListaItemQuestionarioOpcoes[
                                    indexItemQuestionarioOpcaoEditado].RespostaCorreta = rblRespostaCorreta.Checked;

                            break;
                        case enumTipoItemQuestionario.VerdadeiroOuFalso:
                            throw new AcademicoException("Código obsoleto sendo utilizado.");

                            limparGrid = true;

                            // Esconde o botão de adicionar novas opções para não permitir mais envios.
                            btnAdicionarItemQuestionarioOpcao.Visible = false;
                            break;
                        case enumTipoItemQuestionario.ColunasRelacionadas:
                            // Obtém os itens originais antes da edição.
                            var nomeOriginal1 = QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                .ListaItemQuestionarioOpcoes.Where(x => x.OpcaoVinculada == null).ToList()
                                [indexItemQuestionarioOpcaoEditado].Nome;

                            var itemOriginal2 = QuestionarioDaSessao
                                .ListaItemQuestionario[indexItemQuestionario]
                                .ListaItemQuestionarioOpcoes.FirstOrDefault(
                                    x => x.OpcaoVinculada != null && x.OpcaoVinculada.Nome == nomeOriginal1);

                            if (itemOriginal2 != null)
                            {
                                var nomeOriginal2 = itemOriginal2.Nome;

                                // Pegar com o AutoMapper por causa do Virtual, que permite alterações em objetos da lista estando fora da lista.
                                // Incluir a coluna 1 e relacionar na coluna 2.
                                var coluna1 =
                                    Mapper.Map<classes.ItemQuestionarioOpcoes>(
                                        QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                            .ListaItemQuestionarioOpcoes.Where(x => x.OpcaoVinculada == null)
                                            .ToList()[indexItemQuestionarioOpcaoEditado]) ??
                                    new classes.ItemQuestionarioOpcoes
                                    {
                                        ItemQuestionario =
                                            QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                    };

                                coluna1.Nome = txtResposta_coluna1.Text;

                                // Obter a coluna2 com base na coluna1.
                                var coluna2 =
                                    Mapper.Map<classes.ItemQuestionarioOpcoes>(QuestionarioDaSessao
                                        .ListaItemQuestionario[indexItemQuestionario]
                                        .ListaItemQuestionarioOpcoes.FirstOrDefault(
                                            x =>
                                                x.OpcaoVinculada != null &&
                                                (coluna1.ID != 0
                                                    ? x.OpcaoVinculada.ID == coluna1.ID
                                                    : x.OpcaoVinculada.Nome == coluna1.Nome))) ??
                                    new classes.ItemQuestionarioOpcoes
                                    {
                                        Nome = txtResposta_coluna2.Text,
                                        OpcaoVinculada = coluna1,
                                        ItemQuestionario =
                                            QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                    };

                                coluna2.Nome = txtResposta_coluna2.Text;

                                // Remover as opções atuais para serem inseridas novamente.
                                QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                    .ListaItemQuestionarioOpcoes.RemoveAt(indexItemQuestionarioOpcaoEditado * 2);

                                QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                    .ListaItemQuestionarioOpcoes.RemoveAt(indexItemQuestionarioOpcaoEditado * 2);

                                var opcoes = QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                    .ListaItemQuestionarioOpcoes.ToList();

                                // Verificar se o nome das colunas já existem e não permitir que sejam cadastrados opções com nomes iguais para esse ItemTrilha.
                                if (opcoes.Any(x => x.OpcaoVinculada == null && x.Nome == coluna1.Nome))
                                {
                                    coluna1.Nome = nomeOriginal1;
                                    coluna2.Nome = nomeOriginal2;

                                    // Adiciona novamente as opções, pois não deve remover caso caia nesse erro.
                                    QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                        .ListaItemQuestionarioOpcoes.Insert(indexItemQuestionarioOpcaoEditado, coluna1);
                                    QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                        .ListaItemQuestionarioOpcoes.Insert(indexItemQuestionarioOpcaoEditado, coluna2);

                                    throw new AcademicoException(
                                        string.Format(
                                            "O nome \"{0}\" já existe nas opções da coluna 1 cadastradas e não pode ser utilizada.",
                                            coluna1.Nome));
                                }

                                if (opcoes.Any(x => x.OpcaoVinculada != null && x.Nome == coluna2.Nome))
                                {
                                    coluna1.Nome = nomeOriginal1;
                                    coluna2.Nome = nomeOriginal2;

                                    // Adiciona novamente as opções, pois não deve remover caso caia nesse erro.
                                    QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                        .ListaItemQuestionarioOpcoes.Insert(indexItemQuestionarioOpcaoEditado, coluna1);
                                    QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                        .ListaItemQuestionarioOpcoes.Insert(indexItemQuestionarioOpcaoEditado, coluna2);

                                    throw new AcademicoException(
                                        string.Format(
                                            "O nome \"{0}\" já existe nas opções da coluna 2 cadastradas e não pode ser utilizada.",
                                            coluna2.Nome));
                                }

                                // Adicionar as opções das colunas novamente.
                                QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                    .ListaItemQuestionarioOpcoes.Add(coluna1);
                                QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                    .ListaItemQuestionarioOpcoes.Add(coluna2);
                            }

                            limparGrid = true;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    if (limparGrid)
                    {
                        dgvItemQuestionarioOpcoes.DataSource = null;
                        dgvItemQuestionarioOpcoes.DataBind();
                    }

                    // Resetar conteúdo.
                    ResetarItemQuestionarioOpcoes();
                }
                else if (hdAcaoItemQuestionarioOpcao.Value == "INCLUIR")
                {
                    // Como é criação, aqui a gente cria o bichão.
                    if (QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario].ListaItemQuestionarioOpcoes ==
                        null)
                    {
                        QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario].ListaItemQuestionarioOpcoes =
                            new List<classes.ItemQuestionarioOpcoes>();
                    }

                    switch (tipoItemQuestionario)
                    {
                        case enumTipoItemQuestionario.Objetiva:
                        case enumTipoItemQuestionario.Discursiva:
                        case enumTipoItemQuestionario.MultiplaEscolha:
                        case enumTipoItemQuestionario.AgrupadorDeQuestoes:
                        case enumTipoItemQuestionario.Diagnostico:
                            var novoItem = new classes.ItemQuestionarioOpcoes
                            {
                                Nome = txtItemQuestionarioOpcao.Text,
                                RespostaCorreta = rblRespostaCorreta.Checked,
                                ItemQuestionario = QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                            };

                            if (novoItem.RespostaCorreta &&
                                tipoItemQuestionario != enumTipoItemQuestionario.MultiplaEscolha)
                                QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                    .ListaItemQuestionarioOpcoes
                                    .ToList().ForEach(x => x.RespostaCorreta = false);

                            var existeAhQuestao = QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                 .ListaItemQuestionarioOpcoes.Select(item => item.Nome)
                                 .Contains(novoItem.Nome);

                            if(existeAhQuestao)
                            {
                                break;
                            }

                            QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                .ListaItemQuestionarioOpcoes.Add(
                                    novoItem);

                            break;
                        case enumTipoItemQuestionario.VerdadeiroOuFalso:
                            var valorSelecionado = rdbValorDaResposta.SelectedValue == "V";

                            var verdadeiro = new classes.ItemQuestionarioOpcoes
                            {
                                Nome = "V",
                                RespostaCorreta = valorSelecionado,
                                ItemQuestionario = QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                            };

                            var falso = new classes.ItemQuestionarioOpcoes
                            {
                                Nome = "F",
                                RespostaCorreta = valorSelecionado == false,
                                ItemQuestionario = QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                            };

                            // Adicionar as opções de Verdadeiro ou Falso
                            QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                .ListaItemQuestionarioOpcoes.Add(verdadeiro);
                            QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                .ListaItemQuestionarioOpcoes.Add(falso);

                            break;
                        case enumTipoItemQuestionario.ColunasRelacionadas:
                            // Incluir a coluna 1 e relacionar na coluna 2.
                            var coluna1 = new classes.ItemQuestionarioOpcoes
                            {
                                Nome = txtResposta_coluna1.Text,
                                ItemQuestionario = QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                            };

                            var coluna2 = new classes.ItemQuestionarioOpcoes
                            {
                                Nome = txtResposta_coluna2.Text,
                                OpcaoVinculada = coluna1,
                                ItemQuestionario = QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                            };

                            var opcoes = QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                .ListaItemQuestionarioOpcoes;

                            // Verificar se o nome das colunas já existem e não permitir que sejam cadastrados opções com nomes iguais para esse ItemTrilha.
                            if (opcoes.Any(x => x.OpcaoVinculada == null && x.Nome == coluna1.Nome))
                                throw new AcademicoException(
                                    string.Format(
                                        "O nome \"{0}\" já existe nas opções da coluna 1 cadastradas e não pode ser utilizada.",
                                        coluna1.Nome));

                            if (opcoes.Any(x => x.OpcaoVinculada != null && x.Nome == coluna2.Nome))
                                throw new AcademicoException(
                                    string.Format(
                                        "O nome \"{0}\" já existe nas opções da coluna 2 cadastradas e não pode ser utilizada.",
                                        coluna2.Nome));

                            // Adicionar as opções das colunas.
                            QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                .ListaItemQuestionarioOpcoes.Add(coluna1);
                            QuestionarioDaSessao.ListaItemQuestionario[indexItemQuestionario]
                                .ListaItemQuestionarioOpcoes.Add(coluna2);

                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    ResetarItemQuestionarioOpcoes();
                }
                //else
                //{
                //    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Erro ao recuperar a operação");
                //}
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
            catch (Exception)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Erro ao recuperar informações da resposta");
            }

            return QuestionarioDaSessao;
        }

        protected void btnRestaurarQuestionario_Click(object sender, EventArgs e)
        {
            var sessao = new ManterSessao().ObterObjetoPorHash(QuestionarioSessao.Value);

            var questionario = new JavaScriptSerializer().Deserialize<classes.Questionario>(sessao.Valor);
            QuestionarioDaSessao = questionario;
            QuestionarioSessaoJson = new ManterQuestionario().MapearJsonQuestionario(QuestionarioDaSessao);

            AtualizarQuestionarioSessao();

            PreencherCampos(QuestionarioDaSessao);

            pnlModalSessao.Visible = false;
        }

        protected void btn_CancelarRestaurarQuestionario_Click(object sender, EventArgs e)
        {
            if (QuestionarioSessao.HasValue)
                new ManterSessao().ExcluirPorHash(QuestionarioSessao.Value);

            QuestionarioSessao = null;
            TipoQuestionarioSessao = null;

            pnlModalSessao.Visible = false;
        }

        protected void OcultarModalSessao_Click(object sender, EventArgs e)
        {
            pnlModalSessao.Visible = false;
        }

        protected void lnkQuestionarioEdicao_Click(object sender, EventArgs e)
        {
            var permissoes = new List<classes.QuestionarioPermissao>();
            var questionario = AtualizarQuestionarioEdicaoDaSessao(ref permissoes);
            QuestionarioDaSessao = questionario;

            QuestionarioSessaoJson = new ManterQuestionario().MapearJsonQuestionario(QuestionarioDaSessao);
            AtualizarQuestionarioSessao();
        }


        protected void lnkItemQuestionarioEdicao_Click(object sender, EventArgs e)
        {
            var questionario = AtualizarItemQuestionarioDaSessao();
            QuestionarioDaSessao = questionario;

            QuestionarioSessaoJson = new ManterQuestionario().MapearJsonQuestionario(QuestionarioDaSessao);
            AtualizarQuestionarioSessao();
        }

        protected void lnkItemQuestionarioOpcaoEdicao_Click(object sender, EventArgs e)
        {
            var questionario = AtualizarItemQuestionarioOpcaoDaSessao();
            QuestionarioDaSessao = questionario;

            QuestionarioSessaoJson = new ManterQuestionario().MapearJsonQuestionario(QuestionarioDaSessao);
            AtualizarQuestionarioSessao();
        }
    }
}