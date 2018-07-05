using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.DTO.Services;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Util.Classes;
using classes = Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.WebForms.Cadastros.Matricula
{
    public partial class AvaliarTurma : Page
    {
        public int QuantidadeQuestoes
        {
            get
            {
                if (ViewState["QuantidadeQuestoes"] != null)
                    return (int)ViewState["QuantidadeQuestoes"];

                return 0;
            }
            set
            {
                ViewState["QuantidadeQuestoes"] = value;
            }
        }

        public bool BloquearCampos
        {
            get
            {
                if (ViewState["BloquearCampos"] != null)
                    return (bool)ViewState["BloquearCampos"];

                return false;
            }
            set
            {
                ViewState["BloquearCampos"] = value;
            }
        }


        public List<classes.Questao> Questoes { get; set; }

        public classes.Avaliacao AvaliacaoSalva { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int turmaId;
                classes.Turma turma;

                // Já verifica se o Id existe, está correto e existe no banco numa lapada só.
                // Aqui é assim, sou dessas.
                if (Request["Id"] != null && (int.TryParse(Request["Id"], out turmaId)) &&
                    (turma = new ManterTurma().ObterTurmaPorID(turmaId)) != null)
                {
                    var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

                    if (usuarioLogado.IsAdministrador())
                    {
                        Response.Redirect("GerenciamentoMatricula.aspx?oferta=" + turma.Oferta.ID, true);
                        return;
                    }

                    if (turma.ConsultorEducacional == null)
                    {
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro,
                            "A turma selecionada não possui Consultor Educacional.",
                            "GerenciamentoMatricula.aspx?oferta=" + turma.Oferta.ID);
                        return;
                    }

                    var avaliacao = turma.Avaliacoes.FirstOrDefault();

                    if (avaliacao != null)
                    {
                        switch (avaliacao.Status)
                        {
                            case enumStatusAvaliacao.AguardandoResposta:
                                // Gestor só pode visualizar essa tela caso a avaliação exista e já tenha sido respondida pelo Consultor Educacional.
                                if (usuarioLogado.IsGestor())
                                {
                                    ExibirMensagemAguardandoAvaliacao(turmaId);
                                    return;
                                }

                                if (turma.ConsultorEducacional.ID != usuarioLogado.ID)
                                {
                                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro,
                                        "Apenas o Consultor Educacional da turma pode avaliar a turma.",
                                        "GerenciamentoMatricula.aspx?oferta=" + turma.Oferta.ID);
                                }

                                break;
                            case enumStatusAvaliacao.AguardandoGestor:
                                if (usuarioLogado.IsGestor())
                                {
                                    if (usuarioLogado.UF.ID == turma.ConsultorEducacional.UF.ID)
                                    {
                                        btnEnviarAvaliacao.Visible = false;
                                        btnSalvar.Visible = false;

                                        btnAprovar.Visible = true;
                                        btnReprovar.Visible = true;
                                        
                                        BloquearCampos = true;
                                        break;
                                    }

                                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro,
                                        "Apenas um gestor de " + turma.ConsultorEducacional.UF.Nome +
                                        " pode validar a avaliação desta turma.",
                                        "GerenciamentoMatricula.aspx?oferta=" + turma.Oferta.ID);
                                    return;
                                }

                                if (usuarioLogado.IsConsultorEducacional())
                                {
                                    BloquearCampos = true;

                                    // Bloqueia os campos, pois está aguardando análise do gestor.
                                    btnEnviarAvaliacao.Enabled = false;
                                    btnSalvar.Enabled = false;
                                    btnEnviarAvaliacao.Text = "Aguardando avaliação do gestor";

                                    break;
                                }

                                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro,
                                    "Apenas o Consultor Educacional da turma pode avaliar a turma.",
                                    "GerenciamentoMatricula.aspx?oferta=" + turma.Oferta.ID);

                                return;
                            case enumStatusAvaliacao.Aprovada:
                                BloquearCampos = true;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    else
                    {
                        if (turma.ConsultorEducacional.ID != usuarioLogado.ID)
                        {
                            WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro,
                                "Apenas o Consultor Educacional da turma pode avaliar a turma.",
                                "GerenciamentoMatricula.aspx?oferta=" + turma.Oferta.ID);
                            return;
                        }

                        if (usuarioLogado.IsGestor())
                        {
                            ExibirMensagemAguardandoAvaliacao(turmaId);
                            return;
                        }
                    }

                    ltrSolucaoEducacional.Text = turma.Oferta.SolucaoEducacional.Nome;

                    var manterAvaliacao = new ManterAvaliacao();

                    var questoes = manterAvaliacao.ObterQuestoes().ToList();

                    QuantidadeQuestoes = questoes.Count();

                    Questoes = questoes;

                    // Chamar o método ObterStatusDisponiveis() porque ele tem um comportamento semelhante
                    // ao de um Singleton, e será usado posteriormente.
                    ObterStatusDisponiveis(turma.Oferta.SolucaoEducacional.CategoriaConteudo, true);

                    rptQuestoes.DataSource = questoes;
                    rptQuestoes.DataBind();

                    rptMatriculas.DataSource = turma.ListaMatriculas.OrderBy(x => x.MatriculaOferta.Usuario.Nome);
                    rptMatriculas.DataBind();
                }
                else
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Oferta inválida para avaliação. Tente novamente.",
                        "GerenciamentoMatricula.aspx");
                }
            }
        }

        public List<DTOStatusMatricula> ObterStatusDisponiveis(classes.CategoriaConteudo categoria = null, bool recarregar = false)
        {
            if (!recarregar && ViewState["StatusDisponiveis"] != null)
                return (List<DTOStatusMatricula>)ViewState["StatusDisponiveis"];

            if (categoria != null)
            {
                var manterStatusMatricula = new ManterStatusMatricula();

                var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

                var matriculas =
                    manterStatusMatricula.ObterStatusMatriculaPorCategoriaConteudo(categoria)
                        .Where(x => x.PermiteVisualizacao(usuarioLogado))
                        .Select(x => new DTOStatusMatricula
                        {
                            ID = x.ID,
                            Nome = x.Nome
                        }).ToList();

                ViewState["StatusDisponiveis"] = matriculas;

                return matriculas;
            }

            return null;
        }

        private static void ExibirMensagemAguardandoAvaliacao(int ofertaId)
        {
            WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro,
                "Esta turma ainda não foi avaliada. Aguarde o aviso do Consultor Educacional via e-mail.",
                "GerenciamentoMatricula.aspx?oferta=" + ofertaId);
        }

        protected void rptQuestoes_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                var questao = (classes.Questao)e.Item.DataItem;

                var ltrTitulo = (Literal)e.Item.FindControl("ltrTitulo");
                var ltrQuestao = (Literal)e.Item.FindControl("ltrQuestao");

                ltrTitulo.Text = questao.Titulo;
                ltrQuestao.Text = questao.Pergunta;
            }
        }

        protected void rptMatriculas_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                var matricula = (classes.MatriculaTurma)e.Item.DataItem;

                var hdnIdMatriculaTurma = (HiddenField)e.Item.FindControl("hdnIdMatriculaTurma");
                var ltrNome = (Literal)e.Item.FindControl("ltrNome");
                var ltrUf = (Literal)e.Item.FindControl("ltrUf");

                hdnIdMatriculaTurma.Value = matricula.ID.ToString();
                ltrNome.Text = matricula.MatriculaOferta.Usuario.Nome;
                ltrUf.Text = matricula.MatriculaOferta.UF.Sigla;

                var rptQuestaoResposta = (Repeater)e.Item.FindControl("rptQuestaoResposta");

                var avaliacao = matricula.Turma.Avaliacoes.FirstOrDefault();

                rptQuestaoResposta.DataSource = ObterQuestoesResposta(matricula, avaliacao);
                rptQuestaoResposta.DataBind();
            }
        }

        /// <summary>
        /// Obtém a listagem de questão resposta. Caso não tenha, cria os objetos agora. Caso possua parcialmente, obtém somente os preenchidos e cria o resto.
        /// </summary>
        /// <returns></returns>
        private IList<classes.QuestaoResposta> ObterQuestoesResposta(classes.MatriculaTurma matricula,
            classes.Avaliacao avaliacao)
        {
            var retorno = new List<classes.QuestaoResposta>();

            for (var i = 0; i < QuantidadeQuestoes; i++)
            {
                var questao = Questoes[i];

                classes.QuestaoResposta resposta;

                // Obtém ou cria uma nova resposta utilizando mágica.
                if (avaliacao != null)
                {
                    resposta = avaliacao.Respostas.FirstOrDefault(x => x.MatriculaTurma.ID == matricula.ID && x.Questao.ID == questao.ID) ??
                               new classes.QuestaoResposta
                               {
                                   Questao = questao,
                                   MatriculaTurma = matricula
                               };
                }
                else
                {
                    resposta = new classes.QuestaoResposta
                    {
                        Questao = questao,
                        MatriculaTurma = matricula
                    };
                }

                retorno.Add(resposta);
            }

            return retorno;
        }

        protected void rptQuestaoResposta_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var questaoResposta = (classes.QuestaoResposta)e.Item.DataItem;

            var hdnIdQuestaoResposta = (HiddenField)e.Item.FindControl("hdnIdQuestaoResposta");
            var hdnIdQuestao = (HiddenField)e.Item.FindControl("hdnIdQuestao");

            hdnIdQuestaoResposta.Value = questaoResposta.ID.ToString();
            hdnIdQuestao.Value = questaoResposta.Questao.ID.ToString();

            if (questaoResposta.Questao.Tipo == enumTipoQuestao.Dominio)
            {
                var ltrLabel = (Literal)e.Item.FindControl("ltrLabel");
                ltrLabel.Visible = true;
                ltrLabel.Text = "Domínio";

                var ddlDominio = (DropDownList)e.Item.FindControl("ddlDominio");
                ddlDominio.Visible = true;

                var dominiosExistentes = Enum.GetValues(typeof(enumDominio)).Cast<enumDominio>().Select(x => new classes.StatusMatricula
                {
                    ID = (int)x,
                    Nome = x.GetDescription()
                }).ToList();

                WebFormHelper.PreencherLista(dominiosExistentes, ddlDominio, pInsereOpcaoSelecione: true);

                // Selecionar categoria existente.
                if (questaoResposta.Dominio.HasValue)
                    ddlDominio.SelectedValue = ((int)questaoResposta.Dominio).ToString();

                if (BloquearCampos)
                    ddlDominio.Enabled = false;
            }

            if (questaoResposta.Questao.Tipo == enumTipoQuestao.Dissertativa)
            {
                var txtDissertativo = (TextBox)e.Item.FindControl("txtDissertativo");
                txtDissertativo.Visible = true;
                txtDissertativo.Text = questaoResposta.Comentario;

                if (BloquearCampos)
                    txtDissertativo.Enabled = false;
            }

            if (questaoResposta.Questao.Tipo == enumTipoQuestao.Resultado)
            {
                var ddlStatus = (DropDownList)e.Item.FindControl("ddlStatus");
                ddlStatus.Visible = true;

                var status = ObterStatusDisponiveis();

                if (questaoResposta.StatusMatricula != null &&
                    status.All(x => x.ID != questaoResposta.StatusMatricula.ID))
                {
                    status.Add(new DTOStatusMatricula
                    {
                        ID = questaoResposta.StatusMatricula.ID,
                        Nome = questaoResposta.StatusMatricula.Nome
                    });
                }

                WebFormHelper.PreencherLista(status, ddlStatus, pInsereOpcaoSelecione: true);

                if (questaoResposta.StatusMatricula != null)
                    ddlStatus.SelectedValue = questaoResposta.StatusMatricula.ID.ToString();

                if (BloquearCampos)
                    ddlStatus.Enabled = false;
            }
        }

        protected void btnSalvar_OnClick(object sender, EventArgs e)
        {
            var manterAvaliacao = new ManterAvaliacao();

            // Esse método já salva a avaliação.
            var avaliacao = ObterObjetoAvaliacao(manterAvaliacao);

            // Persistir a avaliação salva caso precise usar no envio.
            AvaliacaoSalva = avaliacao;
        }

        private classes.Avaliacao ObterObjetoAvaliacao(ManterAvaliacao manterAvaliacao = null)
        {
            manterAvaliacao = manterAvaliacao ?? new ManterAvaliacao();

            var questoes = manterAvaliacao.ObterQuestoes().ToList();

            var turma = ObterTurma();

            var avaliacao = turma.Avaliacoes.FirstOrDefault() ?? new classes.Avaliacao
            {
                Turma = turma
            };

            var respostas = new List<classes.QuestaoResposta>();

            // Salvar de acordo com os dados preenchidos na tela e recarregar a tela.
            foreach (RepeaterItem repeaterMatriculasItem in rptMatriculas.Items)
            {
                int matriculaId;

                if (int.TryParse(((HiddenField)repeaterMatriculasItem.FindControl("hdnIdMatriculaTurma")).Value, out matriculaId))
                {
                    var rptQuestaoResposta = (Repeater)repeaterMatriculasItem.FindControl("rptQuestaoResposta");

                    foreach (RepeaterItem repeaterQuestaoRespostaItem in rptQuestaoResposta.Items)
                    {
                        //var repeaterQuestaoRespostaItem = rptQuestaoResposta.Controls[j];

                        int idQuestaoResposta;

                        int.TryParse(
                            ((HiddenField)repeaterQuestaoRespostaItem.FindControl("hdnIdQuestaoResposta")).Value,
                            out idQuestaoResposta);

                        var questaoResposta = idQuestaoResposta == 0
                            ? new classes.QuestaoResposta()
                            : questoes.SelectMany(x => x.Respostas).FirstOrDefault(x => x.ID == idQuestaoResposta) ??
                              new classes.QuestaoResposta();

                        var matriculaTurma = turma.ListaMatriculas.FirstOrDefault(x => x.ID == matriculaId);

                        questaoResposta.MatriculaTurma = matriculaTurma != null ? matriculaTurma : null;

                        int idQuestao;

                        int.TryParse(
                            ((HiddenField)repeaterQuestaoRespostaItem.FindControl("hdnIdQuestao")).Value,
                            out idQuestao);

                        questaoResposta.Questao = questoes.FirstOrDefault(x => x.ID == idQuestao);

                        if (questaoResposta.Questao == null)
                            throw new AcademicoException("Erro na montagem das respostas da questão. Contacte o suporte técnico ou atualize a tela e tente novamente.");

                        switch (questaoResposta.Questao.Tipo)
                        {
                            case enumTipoQuestao.Dominio:
                                enumDominio? dominio;

                                try
                                {
                                    var dominioId =
                                        int.Parse(
                                            ((DropDownList)repeaterQuestaoRespostaItem.FindControl("ddlDominio"))
                                                .SelectedValue);

                                    dominio = dominioId == 0 ? null : (enumDominio?)dominioId;
                                }
                                catch (Exception)
                                {
                                    dominio = null;
                                }

                                questaoResposta.Dominio = dominio;

                                break;
                            case enumTipoQuestao.Dissertativa:
                                var dissercao =
                                    ((TextBox)repeaterQuestaoRespostaItem.FindControl("txtDissertativo")).Text;

                                questaoResposta.Comentario = string.IsNullOrWhiteSpace(dissercao) ? null : dissercao;

                                break;
                            case enumTipoQuestao.Resultado:

                                int statusId;

                                int.TryParse(((DropDownList)repeaterQuestaoRespostaItem.FindControl("ddlStatus")).SelectedValue, out statusId);

                                var status = statusId == 0
                                    ? null
                                    : ObterStatusDisponiveis().FirstOrDefault(x => x.ID == statusId);

                                questaoResposta.StatusMatricula = status == null ? null : new classes.StatusMatricula { ID = status.ID };

                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        manterAvaliacao.Salvar(questaoResposta);

                        respostas.Add(questaoResposta);
                    }
                }
                else
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Houve um erro ao tentar salvar as matrículas. Tente novamente ou contacte o suporte técnico.");
                }
            }

            // Salvar a avaliação antes caso esteja criando uma nova. Está aqui no método ObterObjetoAvaliacao() porque sempre usa a mesma avaliação da Oferta.
            if (avaliacao.ID == 0)
                manterAvaliacao.Salvar(avaliacao);

            avaliacao.Respostas = respostas;

            // Salvar as associações da avaliação com as respostas.
            manterAvaliacao.Salvar(avaliacao);

            return avaliacao;
        }

        protected void btnEnviarAvaliacao_OnClick(object sender, EventArgs e)
        {
            if (BloquearCampos)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Não é possível alterar esta avaliação, pois ela está bloqueada.");
                return;
            }

            // Salvar os dados da tela antes de enviar, só para se certificar.
            btnSalvar_OnClick(null, null);

            // O evento de salvar persiste a avaliação salva na variável AvaliacaoSalva.
            var avaliacao = AvaliacaoSalva;

            var manterAvaliacao = new ManterAvaliacao();

            var qtQuestoes = manterAvaliacao.ObterQuestoes().Count();

            var qtMatriculas = ObterTurma().ListaMatriculas.Count();

            var manterUsuario = new ManterUsuario();

            var usuarioLogado = manterUsuario.ObterUsuarioLogado();

            if (avaliacao.Status == enumStatusAvaliacao.AguardandoResposta)
            {
                if (avaliacao.IsRespondido(qtQuestoes, qtMatriculas) && usuarioLogado.IsConsultorEducacional())
                {
                    var template = new ManterTemplate().ObterTemplatePorID((int)enumTemplate.AvaliacaoGestor);

                    if (template == null)
                    {
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro,
                            "Não foi possível enviar e-mail ao gestor. Tente novamente.");
                        return;
                    }

                    // Obter todos os gestores da UF do usuário logado.
                    var usuarioFiltro = new classes.Usuario
                    {
                        ListaPerfil =
                            new List<classes.UsuarioPerfil>
                            {
                                new classes.UsuarioPerfil {Perfil = new classes.Perfil {ID = (int) enumPerfil.GestorUC}}
                            },
                        UF = new classes.Uf { ID = usuarioLogado.UF.ID }
                    };

                    var gestores = manterUsuario.ObterPorFiltro(usuarioFiltro)
                        .Where(x => x.Email != null && x.Email != "")
                        // Selecionar somente os campos que interessam.
                        .Select(x => new classes.Usuario { Email = x.Email, Nome = x.Nome })
                        .ToList();

                    // Caso não haja gestores para atualizar, notifica o usuário e impede a alteração.
                    if (!gestores.Any())
                    {
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro,
                            "Não há gestor na UF " + usuarioLogado.UF.Sigla + " para validar a avaliação dos alunos.");
                        return;
                    }

                    var manterEmail = new ManterEmail();

                    var textoTemplate = template.TextoTemplate;


                    // Reza a lenda que cada oferta desses cursos só possui uma turma. Vamos torcer para isso ser verdade.
                    var turma = avaliacao.Turma;

                    textoTemplate =
                        textoTemplate
                            // Substituir o nome da solução duas vezes porque tem duas tags de solução.
                            .Replace("#SOLUCAO", turma.Oferta.SolucaoEducacional.Nome)
                            .Replace("#SOLUCAO", turma.Oferta.SolucaoEducacional.Nome)
                            .Replace("#OFERTA", turma.Oferta.Nome)
                            .Replace("#TURMA", turma != null ? turma.Nome : "")
                            .Replace("#CONSULTOR",
                                turma != null && turma.ConsultorEducacional != null
                                    ? turma.ConsultorEducacional.Nome
                                    : "");

                    // Executa o envio de e-mails via Threading para deixar o fluxo mais rápido.
                    var thread = new Thread(() =>
                    {
                        foreach (var gestor in gestores)
                        {
                            try
                            {
                                var templateFinal = textoTemplate.Replace("#GESTOR", gestor.Nome);

                                manterEmail.EnviarEmail(gestor.Email, template.Assunto ?? template.DescricaoTemplate, templateFinal);
                            }
                            catch (Exception)
                            {
                                // Ignored.
                            }
                        }
                    })
                    {
                        IsBackground = true
                    };

                    thread.Start();

                    avaliacao.Status = enumStatusAvaliacao.AguardandoGestor;

                    manterAvaliacao.Salvar(avaliacao);

                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso,
                        "Avaliação enviada ao gestor e aguardando análise.",
                        "GerenciamentoMatricula.aspx?oferta=" + avaliacao.Turma.Oferta.ID);
                }
                else
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Você precisa responder todas as questões antes de enviar sua avaliação ao Gestor.");
                }
            }
        }

        private classes.Turma ObterTurma()
        {
            return new ManterTurma().ObterTurmaPorID(int.Parse(Request["Id"]));
        }

        protected void btnVoltar_OnClick(object sender, EventArgs e)
        {
            try
            {
                var ofertaId = ObterTurma().Oferta.ID;

                Response.Redirect("GerenciamentoMatricula.aspx?oferta=" + ofertaId, false);
            }
            catch (Exception)
            {
                Response.Redirect("GerenciamentoMatricula.aspx", true);
            }
        }

        protected void btnAprovar_OnClick(object sender, EventArgs e)
        {
            ValidarAvaliacao(true);
        }

        protected void btnReprovar_OnClick(object sender, EventArgs e)
        {
            ValidarAvaliacao(false);
        }

        private void ValidarAvaliacao(bool aprovar)
        {
            try
            {
                var turma = ObterTurma();
                
                var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

                if (UsuarioPodeValidarAvaliacao(turma, usuarioLogado))
                {
                    var avaliacao = turma.Avaliacoes.FirstOrDefault();

                    if (avaliacao != null)
                    {
                        var manterMatriculaOferta = new ManterMatriculaOferta();
                        var manterAvaliacao = new ManterAvaliacao();

                        var emailsAlunos = new List<string>();

                        // Alterar status das matrículas.
                        foreach (var respostaId in avaliacao.Respostas.Where(x => x.Questao.Tipo == enumTipoQuestao.Resultado).Select(x => x.ID))
                        {
                            // Busca a resposta denovo do banco pois o NHibernate tá pirando com o Lazy.
                            var resposta = manterAvaliacao.ObterQuestaoRespostaPorId(respostaId);

                            var matricula = resposta.MatriculaTurma.MatriculaOferta;

                            // Adicionar emails para envio posterior.
                            if(aprovar && !string.IsNullOrWhiteSpace(matricula.Usuario.Email))
                                emailsAlunos.Add(matricula.Usuario.Email);

                            if (resposta.StatusMatricula != null)
                            {
                                matricula.StatusMatricula = (enumStatusMatricula)resposta.StatusMatricula.ID;

                                manterMatriculaOferta.Salvar(matricula);
                            }
                        }

                        // Mandar e-mail aos alunos caso a avaliação seja aprovada.
                        if (aprovar)
                        {
                            var manterEmail = new ManterEmail();

                            var template = new ManterTemplate().ObterTemplatePorID((int)enumTemplate.ResultadoAvaliacao);

                            var thread = new Thread(() =>
                            {
                                foreach (var email in emailsAlunos)
                                {
                                    manterEmail.EnviarEmail(email, template.Assunto ?? template.DescricaoTemplate, template.TextoTemplate);
                                }
                            })
                            {
                                IsBackground = true
                            };

                            thread.Start();
                        }

                        // Finalizar status da avaliação e setar o usuário que analisou.
                        avaliacao.Status = aprovar ? enumStatusAvaliacao.Aprovada : enumStatusAvaliacao.AguardandoResposta;
                        avaliacao.Analista = usuarioLogado;

                        manterAvaliacao.Salvar(avaliacao);

                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso,
                            "Avaliação " + (!aprovar ? "não" : "") + " aprovada com sucesso",
                            "GerenciamentoMatricula.aspx?oferta=" + turma.Oferta.ID);
                        return;
                    }
                }
            }
            catch (Exception)
            {
                // ignored.
            }

            WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro,
                "Houve um erro na validação da avaliação. Tente novamente.");
        }

        private static bool UsuarioPodeValidarAvaliacao(classes.Turma turma, classes.Usuario usuarioLogado)
        {
            return usuarioLogado.IsGestor() && usuarioLogado.UF.ID == turma.ConsultorEducacional.UF.ID;
        }
    }
}