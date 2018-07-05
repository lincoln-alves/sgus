using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Linq;
using System.Web.UI;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.UserControls
{
    /// <summary>
    /// User Control utilizado para exibir informações de um nível de uma trilha.
    /// </summary>
    public partial class ucTrilhaNivel : System.Web.UI.UserControl
    {

        /// <summary>
        /// Propriedade utilizada para manipular as informações 
        /// de uma trilha que está na memória (ou seja, que ainda não foi salva).
        /// Obs: As informações de uma trilha (níveis, permissões dos níveis) 
        /// são persistidas na sessão para serem gravadas no banco de dados, somente após o
        /// usuário clicar no botão Salvar. (Na tela de Cadastro de Trilhas, no caso no arquivo EdicaoTrilha.aspx.cs)
        /// </summary>
        public Trilha TrilhaDaSessao
        {
            get
            {
                if (Session["Trilha"] != null)
                {
                    return (Trilha)Session["Trilha"];
                }

                return null;
            }
            set
            {
                Session["Trilha"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PreencherCombos();
            }
        }

        //TODO -> #revisado - INICIO

        #region "Atributos Privados"

        /// <summary>
        /// Propriedade que indica se uma trilha (que está na sessão) possui ou não níveis.
        /// </summary>
        private bool TemNiveisNaTrilha
        {
            get
            {

                bool temNiveisNaTrilha = false;

                if (this.TrilhaDaSessao.ListaTrilhaNivel != null && this.TrilhaDaSessao.ListaTrilhaNivel.Count > 0)
                {
                    temNiveisNaTrilha = true;
                }

                return temNiveisNaTrilha;

            }
        }

        //TODO -> #revisado - FIM

        #endregion


        /// <summary>
        /// Preenche todos os campos desta tela.
        /// </summary>
        private void PreencherCombos()
        {
            PreencherComboQuestionario();
            PreencherComboTemplateCertificado();
            PreencherComboInscricoesAbertas();
            PreencherComboOrdem();
        }

        private void PreencherComboOrdem()
        {
            for (int i = 1; i <= 3; i++)
            {
                this.ddlOrdem.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
        }

        private void PreencherComboInscricoesAbertas()
        {
            WebFormHelper.PreencherComponenteComOpcoesSimNao(ddlAceitaNovasMatriculas, true);
        }


        private void PreencherComboTemplateCertificado()
        {
            var listaCertificadoTemplate = new ManterCertificadoTemplate().ObterTodosCertificadoTemplate().ToList();
            WebFormHelper.PreencherLista(listaCertificadoTemplate, ddlCertificadoTemplate, false, true);
        }

        private void PreencherComboQuestionario()
        {

            try
            {
                IList<Questionario> ListaQuestionariosDePesquisa = new ManterQuestionario().ObterQuestionariosDePesquisa();
                IList<Questionario> ListaQuestionariosDeAvaliacaoProva = new ManterQuestionario().ObterQuestionariosDeAvaliacaoProva();

                WebFormHelper.PreencherLista(ListaQuestionariosDePesquisa, ddlQuestionarioPre, false, true);
                WebFormHelper.PreencherLista(ListaQuestionariosDePesquisa, ddlQuestionarioPos, false, true);
                WebFormHelper.PreencherLista(ListaQuestionariosDeAvaliacaoProva, ddlQuestionarioProva, false, true);

            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }

        }

        public TrilhaNivel ObterObjetoTrilhaNivel()
        {

            switch (this.AcaoDaTela)
            {
                //caso esteja editando um nível
                case (int)enumAcaoTelaTrilhaNivel.EdicaoDeUmNivel:

                    this.TrilhaNivelAux = ObterNivelDaTrilha();
                    this.TrilhaNivelAux.StatusRegistro = enumStatusRegistro.Alterado;
                    break;

                //Caso esteja cadastrando um novo item
                case (int)enumAcaoTelaTrilhaNivel.NovoRegistroDeNivel:

                    if (this.TrilhaDaSessao == null || (this.TrilhaDaSessao != null &&
                       (this.TrilhaDaSessao.ListaTrilhaNivel == null || this.TrilhaDaSessao.ListaTrilhaNivel.Count == 0)))
                    {
                        this.TrilhaNivelAux = new TrilhaNivel() { IdLogico = 1 };
                        this.IdLogicoDoNivelTrilha = this.TrilhaNivelAux.IdLogico;
                        this.IdNivelTrilha = 0;
                    }
                    else
                    {
                        this.TrilhaNivelAux = new TrilhaNivel();
                        this.IdLogicoDoNivelTrilha = this.TrilhaDaSessao.ListaTrilhaNivel.Max(x => x.IdLogico) + 1;
                        this.TrilhaNivelAux.IdLogico = this.IdLogicoDoNivelTrilha;
                    }

                    break;

            }

            this.ObterInformacoesDaTelaDeNivel();

            //trilhaNivel.Trilha = this.TrilhaDaSessao;
            this.TrilhaNivelAux.Trilha = this.TrilhaDaSessao;

            this.AdicionarPermissaoATrilhaNivel(this.TrilhaNivelAux);
            this.AdicionarQuestionarioATrilhaNivel(this.TrilhaNivelAux);

            return this.TrilhaNivelAux;

            //return trilhaNivel;

        }

        private TrilhaNivel ObterNivelDaTrilha()
        {

            TrilhaNivel trilhaNivel = null;

            //Se IdNivelTrilha > 0, então é edição de um nível pelo ID (obtido do banco)
            //if (this.IdItemQuestionario > 0)
            if (this.IdNivelTrilha > 0)
            {
                //Obtém o item do questionário
                trilhaNivel = new ManterTrilhaNivel().ObterTrilhaNivelPorID(this.IdNivelTrilha);

                /* Marca o atributo para indicar que estamos alterando as informações 
                   do nível da trilha */
                trilhaNivel.StatusRegistro = enumStatusRegistro.Alterado;
            }
            else
            {

                //Edição de um nível da trilha pelo id lógico
                //Obtém o TrilhaNivel através de seu IdLogico (IdLogicoDoItemQuestionario)
                //IList<ItemQuestionario> ListaItens = this.QuestionarioDaSessao.ListaItemQuestionario.Where(x => x.IdLogico == this.IdLogicoDoItemQuestionario).ToList();
                IList<TrilhaNivel> ListaNiveis = this.TrilhaDaSessao.ListaTrilhaNivel.Where(x => x.IdLogico == this.IdLogicoDoNivelTrilha).ToList();

                if (ListaNiveis != null && ListaNiveis.Count > 0)
                {
                    trilhaNivel = ListaNiveis[0];

                    ///* Marca o atributo para indicar que estamos alterando as informações 
                    //do item do Questionário */
                    //itemQuestionario.StatusRegistro = enumStatusRegistro.Alterado;
                }
                else
                {
                    int ultimoIdInserido = this.TrilhaDaSessao.ListaTrilhaNivel.Max(x => x.IdLogico);
                    trilhaNivel = new TrilhaNivel() { IdLogico = ultimoIdInserido + 1 };
                    this.IdLogicoDoNivelTrilha = trilhaNivel.IdLogico;
                    //this.IdLogicoDoItemQuestionario = trilhaNivel.IdLogico;
                }

            }

            int limiteCancelamento = 0;
            if (int.TryParse(txtLimiteCancelamento.Text, out limiteCancelamento))
            {
                trilhaNivel.LimiteCancelamento = limiteCancelamento;
            }

            if (this.AcaoDaTela.Equals((int)enumAcaoTelaTrilhaNivel.NovoRegistroDeNivel))
            {
                trilhaNivel = new TrilhaNivel() { IdLogico = 1 };
            }

            return trilhaNivel;

        }

        private void AdicionarPermissaoATrilhaNivel(TrilhaNivel trilhaNivelEdicao)
        {
            if (trilhaNivelEdicao != null)
            {
                this.AdicionarOuRemoverPerfilATrilhaNivel(trilhaNivelEdicao);
                this.AdicionarOuRemoverUfATrilhaNivel(trilhaNivelEdicao);
                this.AdicionarOuRemoverNivelOcupacionalATrilhaNivel(trilhaNivelEdicao);
            }
        }

        private void AdicionarQuestionarioATrilhaNivel(TrilhaNivel trilhaNivel)
        {
            int idQuestionario = 0;
            if (int.TryParse(this.ddlQuestionarioProva.SelectedItem.Value, out idQuestionario))
                this.TratarQuestionario(trilhaNivel, idQuestionario, false, enumTipoQuestionarioAssociacao.Prova);
            else
                TratarRemocao(trilhaNivel, false, enumTipoQuestionarioAssociacao.Prova);

            if (int.TryParse(this.ddlQuestionarioPos.SelectedItem.Value, out idQuestionario))
                this.TratarQuestionario(trilhaNivel, idQuestionario, false, enumTipoQuestionarioAssociacao.Pos);
            else
                TratarRemocao(trilhaNivel, false, enumTipoQuestionarioAssociacao.Pos);

            if (int.TryParse(this.ddlQuestionarioPre.SelectedItem.Value, out idQuestionario))
                this.TratarQuestionario(trilhaNivel, idQuestionario, false, enumTipoQuestionarioAssociacao.Pre);
            else
                TratarRemocao(trilhaNivel, false, enumTipoQuestionarioAssociacao.Pre);
        }

        private void TratarQuestionario(TrilhaNivel trilhaNivel, int idQuestionario, bool evolutivo, enumTipoQuestionarioAssociacao tipoQuestionarioAssociacao)
        {
            Questionario questionario = new ManterQuestionario().ObterQuestionarioPorID(idQuestionario);
            QuestionarioAssociacao questionarioAssociacaoEditar = trilhaNivel.ListaQuestionarioAssociacao.FirstOrDefault(x => x.TrilhaNivel.ID == trilhaNivel.ID && x.Evolutivo == evolutivo && x.TipoQuestionarioAssociacao.ID == (int)tipoQuestionarioAssociacao);

            if (questionarioAssociacaoEditar == null)
            {
                QuestionarioAssociacao questionarioAssociacaoAdicionar = new QuestionarioAssociacao();
                questionarioAssociacaoAdicionar.TipoQuestionarioAssociacao = new ManterTipoQuestionarioAssociacao().ObterTipoQuestionarioAssociacaoPorID((int)tipoQuestionarioAssociacao);
                questionarioAssociacaoAdicionar.Evolutivo = evolutivo;
                questionarioAssociacaoAdicionar.TrilhaNivel = trilhaNivel;
                questionarioAssociacaoAdicionar.Questionario = questionario;
                questionarioAssociacaoAdicionar.Obrigatorio = true;
                trilhaNivel.ListaQuestionarioAssociacao.Add(questionarioAssociacaoAdicionar);
                //trilhaNivel.AdicionarQuestionarioAssociacao(questionarioAssociacaoAdicionar);
            }
            else
            {
                questionarioAssociacaoEditar.Questionario = questionario;

            }
        }

        private void TratarRemocao(TrilhaNivel trilhaNivel, bool evolutivo, enumTipoQuestionarioAssociacao tipoQuestionarioAssociacao)
        {
            QuestionarioAssociacao questionarioAssociacaoRemover = trilhaNivel.ListaQuestionarioAssociacao.Where(x => x.TrilhaNivel == trilhaNivel && x.Evolutivo == evolutivo && x.TipoQuestionarioAssociacao.ID == (int)tipoQuestionarioAssociacao).FirstOrDefault();
            if (questionarioAssociacaoRemover != null)
                trilhaNivel.ListaQuestionarioAssociacao.Remove(questionarioAssociacaoRemover);
        }


        private void AdicionarOuRemoverNivelOcupacionalATrilhaNivel(TrilhaNivel trilhaNivelEdicao)
        {
            var todosNiveisOcupacionaisDaTrilhaNivel = this.ucPermissoesNivel.ObterTodosNiveisOcupacionais;  //.ObterPerfisSelecionados;

            if (todosNiveisOcupacionaisDaTrilhaNivel != null && todosNiveisOcupacionaisDaTrilhaNivel.Count > 0)
            {

                NivelOcupacional nivelOcupacionalSelecionado = null;

                for (int i = 0; i < todosNiveisOcupacionaisDaTrilhaNivel.Count; i++)
                {

                    nivelOcupacionalSelecionado = new NivelOcupacional()
                    {
                        ID = int.Parse(todosNiveisOcupacionaisDaTrilhaNivel[i].Value),
                        Nome = todosNiveisOcupacionaisDaTrilhaNivel[i].Text
                    };

                    if (todosNiveisOcupacionaisDaTrilhaNivel[i].Selected)
                    {
                        trilhaNivelEdicao.AdicionarNivelOcupacional(nivelOcupacionalSelecionado);
                    }
                    else
                    {
                        trilhaNivelEdicao.RemoverNivelOcupacional(nivelOcupacionalSelecionado);
                    }
                }
            }
        }

        public void LimparCampos()
        {
            this.txtNomeNivel.Text = "";
            this.ddlQuestionarioPre.ClearSelection();
            this.ddlQuestionarioPos.ClearSelection();
            this.ddlPreRequisito.ClearSelection();
            this.ddlCertificadoTemplate.ClearSelection();
            this.ddlQuestionarioProva.ClearSelection();
            this.txtPrazo.Text = "";
            //this.txtTextoTermoDeAceite.Text = "";
            this.txtValorNotaMinima.Text = "";
            this.ddlOrdem.ClearSelection();
            //this.chkPreObrigatorio.Checked = false;
            this.ucPermissoesNivel.LimparCampos();
            this.ddlAceitaNovasMatriculas.ClearSelection();
            this.txtLimiteCancelamento.Text = "";

        }

        public void PreencherPreRequisito(Trilha trilha)
        {
            if (trilha != null && trilha.ListaTrilhaNivel != null && trilha.ListaTrilhaNivel.Count > 0)
            {
                WebFormHelper.PreencherLista(trilha.ListaTrilhaNivel, ddlPreRequisito, false, true);
            }

        }

        public void PreencherListas()
        {
            PreencherListarDePermissoes();
        }

        private void PreencherListarDePermissoes()
        {
            this.ucPermissoesNivel.PreencherListas();
        }


        private void ZerarIds()
        {
            //Id da Trilha Nivel
            this.IdNivelTrilha = 0;

            //Id lógico do Nível da Trilha
            this.IdLogicoDoNivelTrilha = 0;
        }

        public void PreencherCamposDoNivel(TrilhaNivel pTrilhaNivel)
        {

            this.TrilhaNivelAux = pTrilhaNivel;

            if (pTrilhaNivel != null)
            {
                this.PrepararTelaParaEdicaoDeUmNivel(pTrilhaNivel);
            }
            else
            {
                this.PrepararTelaParaInclusaoDeNovoNivel();
            }
        }

        private void PrepararTelaParaEdicaoDeUmNivel(TrilhaNivel pTrilhaNivel)
        {
            this.PreencherComboQuestionario();

            //Id da Trilha Nivel
            this.IdNivelTrilha = pTrilhaNivel.ID;

            //Id lógico do Nível da Trilha
            this.IdLogicoDoNivelTrilha = pTrilhaNivel.IdLogico;

            //Nome
            this.txtNomeNivel.Text = pTrilhaNivel.Nome;

            //Inscrições Abertas ?
            if (pTrilhaNivel.AceitaNovasMatriculas.HasValue)
            {
                this.ddlAceitaNovasMatriculas.ClearSelection();
                bool inscricaoAberta = pTrilhaNivel.AceitaNovasMatriculas.Value;

                if (inscricaoAberta)
                    WebFormHelper.SetarValorNaCombo("S", ddlAceitaNovasMatriculas);
                else
                    WebFormHelper.SetarValorNaCombo("N", ddlAceitaNovasMatriculas);

            }

            //Prazo
            this.txtPrazo.Text = pTrilhaNivel.QuantidadeDiasPrazo.ToString();

            //Prazo Monitor
            this.txtPrazoMonitor.Text = pTrilhaNivel.PrazoMonitorDiasUteis.ToString();

            //Avisar monitor
            this.chbAvisarMonitor.Checked = pTrilhaNivel.AvisarMonitor;

            //Pré-Requisito
            if (pTrilhaNivel.PreRequisito != null && !pTrilhaNivel.PreRequisito.ID.Equals(0))
            {
                //WebFormHelper.PreencherLista(this.TrilhaDaSessao.ListaTrilhaNivel, this.ddlPreRequisito, false, true);
                //WebFormHelper.SetarValorNaCombo(pTrilhaNivel.PreRequisito.ID.ToString(), this.ddlPreRequisito, false);
                IList<TrilhaNivel> trilhaNivelfiltrada = this.TrilhaDaSessao.ListaTrilhaNivel.Where(x => x.ID != pTrilhaNivel.ID).ToList();
                WebFormHelper.PreencherLista(trilhaNivelfiltrada, this.ddlPreRequisito, false, true);
                WebFormHelper.SetarValorNaCombo(pTrilhaNivel.PreRequisito.ID.ToString(), this.ddlPreRequisito, false);
            }
            else
            {
                //Senão tiver pré-requisito, retira da lista de pre-requisitos possíveis o nível que está sendo editado
                if (pTrilhaNivel.ID > 0)
                {
                    IList<TrilhaNivel> listaTrilhaNivelfiltrada = this.TrilhaDaSessao.ListaTrilhaNivel.Where(x => x.ID != pTrilhaNivel.ID).ToList();
                    WebFormHelper.PreencherLista(listaTrilhaNivelfiltrada, this.ddlPreRequisito, false, true);
                    //WebFormHelper.SetarValorNaCombo(pTrilhaNivel.PreRequisito.ID.ToString(), this.ddlPreRequisito, false);
                }
                else
                {
                    //Edição de um nível da trilha, com id logico (nivel que ainda não foi salvo no banco)
                    IList<TrilhaNivel> listaTrilhaNivelfiltrada = this.TrilhaDaSessao.ListaTrilhaNivel.Where(x => x.IdLogico != pTrilhaNivel.IdLogico).ToList();
                    WebFormHelper.PreencherLista(listaTrilhaNivelfiltrada, this.ddlPreRequisito, false, true);

                    if (listaTrilhaNivelfiltrada != null && pTrilhaNivel.PreRequisito != null)
                    {
                        WebFormHelper.SetarValorNaComboPorTexto(pTrilhaNivel.PreRequisito.Nome.ToString(), this.ddlPreRequisito, false);
                    }
                }
            }

            //Template do Certificado
            if (pTrilhaNivel.CertificadoTemplate != null && !pTrilhaNivel.CertificadoTemplate.ID.Equals(0))
            {
                PreencherComboTemplateCertificado();
                WebFormHelper.SetarValorNaCombo(pTrilhaNivel.CertificadoTemplate.ID.ToString(), this.ddlCertificadoTemplate, false);
            }

            //Texto do Termo de Aceite
            //this.txtTextoTermoDeAceite.Text = pTrilhaNivel.TermoAceite;

            //Valor da Nota Minima
            if (pTrilhaNivel.NotaMinima.HasValue)
                this.txtValorNotaMinima.Text = pTrilhaNivel.NotaMinima.Value.ToString();

            //Ordem
            WebFormHelper.SetarValorNaCombo(pTrilhaNivel.ValorOrdem.ToString(), this.ddlOrdem, false);

            //Prazo (Dias)
            this.txtPrazo.Text = pTrilhaNivel.QuantidadeDiasPrazo.ToString();

            //Carga Horária
            this.txtCargaHoraria.Text = pTrilhaNivel.CargaHoraria.ToString();

            this.SetarAcaoDaTela(enumAcaoTelaTrilhaNivel.EdicaoDeUmNivel);

            //Preenche os campos referentes às Permissões e aos questionários do Nível da trilha
            this.PreencherCamposDasPermissoesEAssociacaoDoQuestionarioDaTrilhaNivel(pTrilhaNivel);

            txtLimiteCancelamento.Text = pTrilhaNivel.LimiteCancelamento.ToString();
        }

        private void PrepararTelaParaInclusaoDeNovoNivel()
        {
            this.ZerarIds();
            this.SetarAcaoDaTela(enumAcaoTelaTrilhaNivel.NovoRegistroDeNivel);
        }

        #region "Métodos Privados"

        private void SetarAcaoDaTela(enumAcaoTelaTrilhaNivel acaoTelaTrilha)
        {
            this.AcaoDaTela = (int)acaoTelaTrilha;
        }

        #endregion

        /// <summary>
        /// Propriedade que guarda o nível da trilha, temporariamente, que estará sendo editada 
        /// na tela.
        /// </summary>
        public TrilhaNivel TrilhaNivelAux
        {
            get
            {
                if (Session["ViewStateTrilhaNivel"] != null)
                {
                    return (TrilhaNivel)Session["ViewStateTrilhaNivel"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Session["ViewStateTrilhaNivel"] = value;
            }
        }


        /// <summary>
        /// Propriedade que indica a ação realizada pelo usuário em uma tela.
        /// </summary>
        public int AcaoDaTela
        {
            get
            {
                if (ViewState["ViewStateAcaoDaTelaDeTrilha"] != null)
                {
                    return (int)ViewState["ViewStateAcaoDaTelaDeTrilha"];
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                ViewState["ViewStateAcaoDaTelaDeTrilha"] = value;
            }

        }

        #region "Enumeração"

        /// <summary>
        /// Enumeração referente às ações feitas na tela de cadastro de trilha.
        /// </summary>
        private enum enumAcaoTelaTrilhaNivel
        {
            NovoRegistroDeNivel = 1,
            EdicaoDeUmNivel = 2,
        }


        #endregion

        #region "Operações Relacionadas à Trilha Nível"

        private void AdicionarOuRemoverUfATrilhaNivel(TrilhaNivel trilhaNivelEdicao)
        {
            var todosUfsDaTrilhaNivel = this.ucPermissoesNivel.ObterTodosUfs;

            if (todosUfsDaTrilhaNivel != null && todosUfsDaTrilhaNivel.Count > 0)
            {

                Uf ufSelecionado = null;

                for (int i = 0; i < todosUfsDaTrilhaNivel.Count; i++)
                {

                    ufSelecionado = new Uf()
                    {
                        ID = int.Parse(todosUfsDaTrilhaNivel[i].Value),
                        Nome = todosUfsDaTrilhaNivel[i].Text
                    };

                    if (todosUfsDaTrilhaNivel[i].Selected)
                    {
                        trilhaNivelEdicao.AdicionarUfs(ufSelecionado);
                    }
                    else
                    {
                        trilhaNivelEdicao.RemoverUf(ufSelecionado);
                    }
                }
            }
        }
        private void AdicionarOuRemoverPerfilATrilhaNivel(TrilhaNivel trilhaNivelEdicao)
        {
            var todosPerfisDaTrilhaNivel = this.ucPermissoesNivel.ObterTodosPerfis;

            if (todosPerfisDaTrilhaNivel != null && todosPerfisDaTrilhaNivel.Count > 0)
            {
                //trilhaNivelEdicao.ListaPermissao.Clear();
                for (int i = 0; i < todosPerfisDaTrilhaNivel.Count; i++)
                {
                    var perfilSelecionado = new Perfil()
                    {
                        ID = int.Parse(todosPerfisDaTrilhaNivel[i].Value),
                        Nome = todosPerfisDaTrilhaNivel[i].Text
                    };

                    if (todosPerfisDaTrilhaNivel[i].Selected)
                    {
                        trilhaNivelEdicao.AdicionarPerfil(perfilSelecionado);
                    }
                    else
                    {
                        trilhaNivelEdicao.RemoverPerfil(perfilSelecionado);
                    }
                }
            }
            else
            {
                if (trilhaNivelEdicao.ListaPermissao != null)
                {
                    TrilhaNivelPermissao trilhaNivelPermissao = new TrilhaNivelPermissao() { TrilhaNivel = trilhaNivelEdicao };
                    trilhaNivelEdicao.ListaPermissao.Add(trilhaNivelPermissao);
                }
            }

        }

        #endregion

        #region "Métodos para Preencher Listas"

        private void PreencherCamposDasPermissoesEAssociacaoDoQuestionarioDaTrilhaNivel(TrilhaNivel trilhaNivel)
        {
            this.PreencherListaUfsDaTrilhaNivel(trilhaNivel);
            this.PreencherListaNivelOcupacional(trilhaNivel);
            this.PreencherListaPerfil(trilhaNivel);

            this.PreencherCamposDaAssociacaoDoQuestionario(trilhaNivel);
        }

        private void PreencherDadosQuestionarioPre(TrilhaNivel trilhaNivel)
        {
            QuestionarioAssociacao questionarioAssociacaoPre = trilhaNivel.ListaQuestionarioAssociacao.FirstOrDefault(x => x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pre &&
                                                                          !x.Evolutivo); //.ToList()[0];
            if (questionarioAssociacaoPre != null)
            {
                ddlQuestionarioPre.ClearSelection();
                WebFormHelper.SetarValorNaCombo(questionarioAssociacaoPre.Questionario.ID.ToString(), this.ddlQuestionarioPre);
                //this.IdQuestionarioPre = questionarioAssociacaoPre.Questionario.ID;
                //chkPreObrigatorio.Checked = questionarioAssociacaoPre.Obrigatorio;
            }
        }

        private void PreencherDadosQuestionarioPos(TrilhaNivel trilhaNivel)
        {
            QuestionarioAssociacao questionarioAssociacaoPos = trilhaNivel.ListaQuestionarioAssociacao.FirstOrDefault(x => x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pos &&
                                                                                   !x.Evolutivo); //.ToList()[0];

            if (questionarioAssociacaoPos != null)
            {
                ddlQuestionarioPos.ClearSelection();
                WebFormHelper.SetarValorNaCombo(questionarioAssociacaoPos.Questionario.ID.ToString(), this.ddlQuestionarioPos);
            }
        }

        private void PreencherCamposDaAssociacaoDoQuestionario(TrilhaNivel trilhaNivel)
        {
            if (trilhaNivel.ListaQuestionarioAssociacao != null && trilhaNivel.ListaQuestionarioAssociacao.Count > 0)
            {

                foreach (var item in trilhaNivel.ListaQuestionarioAssociacao)
                {

                    if (item.Evolutivo)
                    {
                        //Marca o checkbox
                        this.chkDinamicoPrePos.Checked = true;
                    }
                    else
                    {

                        switch (item.TipoQuestionarioAssociacao.ID)
                        {
                            case (int)enumTipoQuestionarioAssociacao.Pre:
                                this.PreencherDadosQuestionarioPre(trilhaNivel);
                                break;

                            case (int)enumTipoQuestionarioAssociacao.Pos:
                                this.PreencherDadosQuestionarioPos(trilhaNivel);
                                break;

                            case (int)enumTipoQuestionarioAssociacao.Prova:
                                this.PreencherDadosDoQuestionarioProva(trilhaNivel);
                                break;

                        }
                    }

                    //if (item.TipoQuestionarioAssociacao.ID == (int) enumTipoQuestionarioAssociacao.Pre)
                    //{
                    //    this.PreencherDadosQuestionarioPre(trilhaNivel);
                    //    return;
                    //}
                    //if (item.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pos)
                    //{
                    //    this.PreencherDadosQuestionarioPre(trilhaNivel);
                    //    return;
                    //}
                    //if (item.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Prova)
                    //{
                    //    this.PreencherDadosDoQuestionarioProva(trilhaNivel);
                    //    return;
                    //}
                }
            }
        }

        private void PreencherDadosDoQuestionarioProva(TrilhaNivel trilhaNivel)
        {
            //Questionário Prova
            this.ddlQuestionarioProva.ClearSelection();
            QuestionarioAssociacao questionarioAssociacaoProva = trilhaNivel.ListaQuestionarioAssociacao.Where(x => x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Prova).ToList()[0];
            WebFormHelper.SetarValorNaCombo(questionarioAssociacaoProva.Questionario.ID.ToString(), this.ddlQuestionarioProva);
        }

        private void PreencherListaUfsDaTrilhaNivel(TrilhaNivel trilhaNivel)
        {

            //Obtém a lista de ufs da trilha nível
            IList<Uf> ListaUFs = trilhaNivel.ListaPermissao.Where(x => x.Uf != null)
                      .Select(x => new Uf() { ID = x.Uf.ID, Nome = x.Uf.Nome }).ToList<Uf>();

            this.ucPermissoesNivel.PreencherListBoxComUfsGravadasNoBanco(ListaUFs);
        }

        private void PreencherListaPerfil(TrilhaNivel trilhaNivel)
        {
            //Obtém a lista de Perfis gravados no banco
            IList<Perfil> ListaPerfil = trilhaNivel.ListaPermissao.Where(x => x.Perfil != null)
                    .Select(x => new Perfil() { ID = x.Perfil.ID, Nome = x.Perfil.Nome }).ToList<Perfil>();

            bool temPerfilPublico = false;

            if (ListaPerfil.Count == 0)
            {
                temPerfilPublico = trilhaNivel.ListaPermissao.Where(x => x.Perfil == null &&
                    x.NivelOcupacional == null && x.Uf == null).Any();
            }

            this.ucPermissoesNivel.PreencherListBoxComPerfisGravadosNoBanco(ListaPerfil, temPerfilPublico);
        }

        private void PreencherListaNivelOcupacional(TrilhaNivel trilhaNivel)
        {
            //Obtém a lista de niveis ocupacionais gravadas no banco
            IList<NivelOcupacional> ListaNivelOcupacional = trilhaNivel.ListaPermissao.Where(x => x.NivelOcupacional != null)
                    .Select(x => new NivelOcupacional() { ID = x.NivelOcupacional.ID, Nome = x.NivelOcupacional.Nome }).ToList<NivelOcupacional>();

            this.ucPermissoesNivel.PreencherListBoxComNiveisOcupacionaisGravadosNoBanco(ListaNivelOcupacional);

        }

        #endregion

        #region "Eventos referentes ao cancelamento de um cadastro de um nível de uma trilha"

        public delegate void AdicaoDeNivelCancelada(object sender, AdicionarTrilhaEventArgs e);
        public event AdicaoDeNivelCancelada CancelouAdicaoDeNivel;

        #endregion

        #region "Eventos referentes à adição de nível a uma trilha"

        public delegate void AdicaoDeNivelRealizada(object sender, AdicionarTrilhaNivelEventArgs e);
        public event AdicaoDeNivelRealizada AdicionouNivel;

        #endregion

        private void TratarCancelamento()
        {
            if (CancelouAdicaoDeNivel != null)
            {

                if (!this.AcaoDaTela.Equals((int)enumAcaoTelaTrilhaNivel.EdicaoDeUmNivel))
                //   && !this.AcaoDaTela.Equals((int)enumAcaoTelaTrilhaNivel.EdicaoDeUmaResposta))
                {
                    this.ExcluirNivelDaTrilhaDaListaDaSessao();
                }

                //Dispara o evento de cancelamento de adição de um nível item da trilha.
                CancelouAdicaoDeNivel(this, new AdicionarTrilhaEventArgs(this.TrilhaDaSessao));

                //this.LimparInformacoesDosQuestionarios();
            }

        }

        private void ExcluirNivelDaTrilhaDaListaDaSessao()
        {
            if (this.TrilhaNivelAux != null)
            {
                // REMOVER AUTO RELACIONAMENTO PARA NÂO APAGAR MAIS DE UMA E NÂO OCASIONAR ERROS AO EDITAR UMA FILHA
                foreach (var item in TrilhaDaSessao.ListaTrilhaNivel)
                {
                    if (item.PreRequisito == this.TrilhaNivelAux)
                    {
                        item.PreRequisito = null;
                    }
                }

                if (this.TrilhaNivelAux.ID > 0)
                {
                    this.TrilhaDaSessao.ListaTrilhaNivel.Remove(this.TrilhaNivelAux);
                }
                else
                {
                    this.TrilhaNivelAux.RemoverPeloIdLogico = true;
                    this.TrilhaDaSessao.ListaTrilhaNivel.Remove(this.TrilhaNivelAux);
                }
            }
        }

        protected void btnCancelarEdicao_Click(object sender, EventArgs e)
        {
            this.TratarCancelamento();
        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {

            try
            {
                this.ObterObjetoTrilhaNivel();
                this.AdicionarNivelATrilha();

            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                return;
            }


            if (AdicionouNivel != null)
            {
                //Dispara o evento, passando a lista de níveis da trilha, atualizada.
                AdicionouNivel(this, new AdicionarTrilhaNivelEventArgs((IList<TrilhaNivel>)this.TrilhaDaSessao.ListaTrilhaNivel));

                this.IdLogicoDoNivelTrilha = 0;
            }
        }

        private void ObterInformacoesDaTelaDeNivel()
        {
            //Nome
            this.TrilhaNivelAux.Nome = txtNomeNivel.Text.Trim();

            //Inscrições Abertas ?
            if (ddlAceitaNovasMatriculas.SelectedItem != null && !string.IsNullOrWhiteSpace(ddlAceitaNovasMatriculas.SelectedItem.Value))
            {
                string valorInformadoParaInscricaoAberta = ddlAceitaNovasMatriculas.SelectedItem.Value;

                if (valorInformadoParaInscricaoAberta.ToString().ToUpper().Equals("S"))
                    this.TrilhaNivelAux.AceitaNovasMatriculas = true;
                else if (valorInformadoParaInscricaoAberta.ToString().ToUpper().Equals("N"))
                    this.TrilhaNivelAux.AceitaNovasMatriculas = false;
            }
            else
            {
                this.TrilhaNivelAux.AceitaNovasMatriculas = false;
            }

            //Certificado
            if (ddlCertificadoTemplate.SelectedItem != null && !string.IsNullOrWhiteSpace(ddlCertificadoTemplate.SelectedItem.Value))
            {
                this.TrilhaNivelAux.CertificadoTemplate = new CertificadoTemplate()
                {
                    ID = int.Parse(ddlCertificadoTemplate.SelectedItem.Value),
                    Descricao = ddlCertificadoTemplate.SelectedItem.Text
                };
            }
            else
            {
                this.TrilhaNivelAux.CertificadoTemplate = null;
            }

            //Pré-Requisito
            if (ddlPreRequisito.SelectedItem != null && !string.IsNullOrWhiteSpace(ddlPreRequisito.SelectedItem.Value))
            {
                TrilhaNivel trilhaNivelPreRequisito = null;

                if (ddlPreRequisito.SelectedItem.Text.ToLower().Equals("-- Selecione-- "))
                {
                    this.TrilhaNivelAux.PreRequisito = null;
                }
                else
                {
                    //Nivel 
                    int idPreRequisito = int.Parse(ddlPreRequisito.SelectedItem.Value);

                    //Pre-Requisito gravado no banco
                    if (idPreRequisito > 0)
                    {
                        //Busca o nível da trilha no banco, para verificar se é um nivel ja salvo
                        trilhaNivelPreRequisito = new ManterTrilhaNivel().ObterTrilhaNivelPorID(int.Parse(ddlPreRequisito.SelectedItem.Value));

                        //Se encontrou, é um Pre-Requisito (um nivel de um atrilha) que já foi salvo.
                        if (trilhaNivelPreRequisito != null)
                        {
                            this.TrilhaNivelAux.PreRequisito = trilhaNivelPreRequisito;
                        }
                        else
                        {
                            //É um Pre-Requisito (um nivel de uma trilha) que ainda não foi salvo.
                            this.TrilhaNivelAux.PreRequisito = new TrilhaNivel()
                            {
                                ID = int.Parse(ddlPreRequisito.SelectedItem.Value),
                                Descricao = ddlPreRequisito.SelectedItem.Text
                            };
                        }
                    }
                    else
                    {
                        //Pré-requisito em memória (ainda não foi salvo no banco)
                        trilhaNivelPreRequisito = this.TrilhaDaSessao.ListaTrilhaNivel.FirstOrDefault(x => x.Nome == ddlPreRequisito.SelectedItem.Text);

                        if (trilhaNivelPreRequisito != null)
                        {
                            this.TrilhaNivelAux.PreRequisito = trilhaNivelPreRequisito;
                        }
                    }
                }

            }
            else
            {
                //Retira o Pré-Requisito do nível da trilha, pois a opção selecione foi escolhida.
                this.TrilhaNivelAux.PreRequisito = null;
            }


            //Prazo
            int qtdPrazo = 0;
            if (!int.TryParse(this.txtPrazo.Text.Trim(), out qtdPrazo))
                throw new AcademicoException("Valor Inválido para o Campo Prazo.");
            else
            {
                this.TrilhaNivelAux.QuantidadeDiasPrazo = qtdPrazo;
            }

            //Prazo monitor
            byte qtdPrazoMonitor = 0;
            if (!byte.TryParse(this.txtPrazoMonitor.Text.Trim(), out qtdPrazoMonitor))
                throw new AcademicoException("Valor Inválido para o campo prazo de avaliação do monitor.");
            else
            {
                this.TrilhaNivelAux.PrazoMonitorDiasUteis = qtdPrazoMonitor;
            }

            //Avisar Monitor
            this.TrilhaNivelAux.AvisarMonitor = chbAvisarMonitor.Checked;

            //Texto do Termo de Aceite
            //if (!string.IsNullOrWhiteSpace(txtTextoTermoDeAceite.Text))
            //{
            //    this.TrilhaNivelAux.TermoAceite = txtTextoTermoDeAceite.Text.Trim();
            //}
            //else
            //{
            //    this.TrilhaNivelAux.TermoAceite = null;
            //}

            //Valor da Ordem
            this.TrilhaNivelAux.ValorOrdem = byte.Parse(this.ddlOrdem.SelectedItem.Value);

            //Valor da Nota Minima
            decimal valorNotaMinima = 0;
            if (!decimal.TryParse(this.txtValorNotaMinima.Text.Trim(), out valorNotaMinima))
                throw new AcademicoException("Valor Inválido para o Campo Valor da Nota Mínima.");
            else
            {
                if (valorNotaMinima > 10 || valorNotaMinima < 0)
                {
                    throw new AcademicoException("O valor da Nota Mínima deve estar entre 0 e 10.");
                }

                this.TrilhaNivelAux.NotaMinima = valorNotaMinima;
            }

            //Carga Horária
            int qtdCargaHoraria = 0;
            if (!int.TryParse(this.txtCargaHoraria.Text.Trim(), out qtdCargaHoraria))
                throw new AcademicoException("Valor Inválido para a Carga Horária.");
            else
            {
                this.TrilhaNivelAux.CargaHoraria = qtdCargaHoraria;
            }

            int limiteCancelamento = 0;
            if (int.TryParse(txtLimiteCancelamento.Text, out limiteCancelamento))
            {
                this.TrilhaNivelAux.LimiteCancelamento = limiteCancelamento;
            }
        }

        private void AdicionarNivelATrilha()
        {

            //if (this.TrilhaDaSessao.ListaTrilhaNivel.Where(x => !string.IsNullOrWhiteSpace(trilhaNivel.Nome)
            //    && x.Nome.Trim().ToLower().Equals(trilhaNivel.Nome.Trim().ToLower()) && x.StatusRegistro.Equals(enumStatusRegistro.Novo)).Any())

            if (this.TrilhaDaSessao.ListaTrilhaNivel.Where(x => !string.IsNullOrWhiteSpace(this.TrilhaNivelAux.Nome)
             && x.Nome.Trim().ToLower().Equals(this.TrilhaNivelAux.Nome.Trim().ToLower()) && this.TrilhaNivelAux.StatusRegistro.Equals(enumStatusRegistro.Novo)).Any())
            {
                throw new AcademicoException(string.Format("Não é possível adicionar este Nível '{0}', pois já existe um Nível com este Nome nesta Trilha", this.TrilhaNivelAux.Nome));
            }

            //Verifica se o Id do Nivel existe na lista de Ids dos Niveis da Trilha que estão na sessão
            if (!this.TrilhaDaSessao.ListaTrilhaNivel.Any(x => x.ID.Equals(this.TrilhaNivelAux.ID)))
            {
                this.AtualizarListaDeNiveisDaSessao(this.TrilhaNivelAux);
            }
            else
            {
                //Se encontrar o nível da trilha pelo id fisico, entao remove e adiciona de novo
                //todo: complemento ->  && x.ID > 0))
                if (this.TrilhaDaSessao.ListaTrilhaNivel.Any(x => x.ID.Equals(this.TrilhaNivelAux.ID) && x.ID > 0))
                {
                    this.VerificarCamposObrigatorios(this.TrilhaNivelAux);
                    this.TrilhaNivelAux.RemoverPeloIdLogico = false;
                    this.TrilhaDaSessao.ListaTrilhaNivel.Remove(this.TrilhaNivelAux);
                    this.TrilhaDaSessao.ListaTrilhaNivel.Add(this.TrilhaNivelAux);
                    return;
                }
                else
                {
                    //Busca o nivel da trilha pelo id logico
                    if (!this.TrilhaDaSessao.ListaTrilhaNivel.Any(x => x.IdLogico.Equals(this.TrilhaNivelAux.IdLogico)))
                    {
                        this.AtualizarListaDeNiveisDaSessao(this.TrilhaNivelAux);
                    }
                    else
                    {
                        //Verifica os campos obrigatórios do nível da trilha antes de adicionar na sessão.
                        this.VerificarCamposObrigatorios(this.TrilhaNivelAux);
                        this.TrilhaNivelAux.RemoverPeloIdLogico = true;
                        this.TrilhaDaSessao.ListaTrilhaNivel.Remove(this.TrilhaNivelAux);
                        this.TrilhaDaSessao.ListaTrilhaNivel.Add(this.TrilhaNivelAux);
                    }
                }
            }
        }

        private void AtualizarListaDeNiveisDaSessao(TrilhaNivel pTrilhaNivel)
        {
            //Verifica os campos obrigatórios do nível da trilha antes de adicionar na sessão.
            this.VerificarCamposObrigatorios(pTrilhaNivel);
            this.TrilhaDaSessao.ListaTrilhaNivel.Add(pTrilhaNivel);
        }


        private void LimparInformacoesDosQuestionarios()
        {
            //this.IdQuestionarioPre = 0;
            //this.chkPreObrigatorio.Checked = false;
        }

        /// <summary>
        /// Verifica se os campos obrigatórios (do objeto trilhaNivel) foram informados.
        /// </summary>
        /// <param name="trilhaNivel">Objeto da classe trilhaNivel que contém informações de um nível de uma trilha</param>
        private void VerificarCamposObrigatorios(TrilhaNivel trilhaNivel)
        {
            try
            {
                if (trilhaNivel != null)
                {
                    //Nome do Nível da Trilha
                    if (string.IsNullOrWhiteSpace(trilhaNivel.Nome))
                    {
                        throw new AcademicoException("Nome do Nível. Campo Obrigatório");
                    }

                    //Ordem
                    if (trilhaNivel.ValorOrdem == 0)
                    {
                        throw new AcademicoException("Ordem. Campo Obrigatório");
                    }

                    //Carga Horária
                    if (trilhaNivel.CargaHoraria == 0)
                    {
                        throw new AcademicoException("Carga Horária. Campo Obrigatório");
                    }
                }


            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// ID Lógico do Nível da Trilha. O ID Lógico do Nível da Trilha é persistido (armazenado) no viewstate da página.
        /// </summary>
        public int IdLogicoDoNivelTrilha
        {
            get
            {
                if (ViewState["ViewStateIdLogicoDoNivelTrilha"] != null)
                {
                    return int.Parse(ViewState["ViewStateIdLogicoDoNivelTrilha"].ToString());
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                ViewState["ViewStateIdLogicoDoNivelTrilha"] = value;
            }

        }

        /// <summary>
        /// ID do Nível da Trilha. O ID do Nível da Trilha é persistido (armazenado) no viewstate da página.
        /// </summary>
        public int IdNivelTrilha
        {
            get
            {
                if (ViewState["ViewStateIdNivelTrilha"] != null)
                {
                    return (int)ViewState["ViewStateIdNivelTrilha"];
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                ViewState["ViewStateIdNivelTrilha"] = value;
            }

        }

    }

    /// <summary>
    /// Classe que contém informações do evento disparada após a adição de uma trilha.
    /// </summary>
    public class AdicionarTrilhaEventArgs : EventArgs
    {
        public Trilha TrilhaSelecionada { get; set; }

        public AdicionarTrilhaEventArgs(Trilha trilha)
        {
            TrilhaSelecionada = trilha;
        }
    }

    /// <summary>
    /// Classe que contém informações do evento disparada após a adição de uma trilha.
    /// </summary>
    public class AdicionarTrilhaNivelEventArgs : EventArgs
    {
        public IList<TrilhaNivel> listaTrilhaNivel { get; set; }

        public AdicionarTrilhaNivelEventArgs(IList<TrilhaNivel> listaNivel)
        {
            listaTrilhaNivel = listaNivel;
        }
    }


}