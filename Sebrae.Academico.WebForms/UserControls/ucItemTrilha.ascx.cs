using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.BP.Classes.ConheciGame;

namespace Sebrae.Academico.WebForms.UserControls
{
    public partial class ucItemTrilha : UserControl
    {
        private ItemTrilha _itemTrilhaEdicao;

        private enumOperacao Operacao
        {
            get
            {
                enumOperacao? operacao;

                if (Session["ItemTrilhaEdit"] != null)
                {
                    operacao = enumOperacao.Edicao;
                }
                else
                {
                    operacao = enumOperacao.Consulta;
                }

                return operacao.Value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                PreencherCombos();

                if (Session["ItemTrilhaEdit"] != null)
                {
                    var idItemTrilha = int.Parse(Session["ItemTrilhaEdit"].ToString());
                    _itemTrilhaEdicao = new ManterItemTrilha().ObterItemTrilhaPorID(idItemTrilha);

                    divArquivoEnvio.Visible = true;
                    divDownloadArquivo.Visible = true;


                    PreencherCampos(_itemTrilhaEdicao);
                }
            }
        }

        #region "Métodos Privados"

        private void BaixarArquivo()
        {
            int idItemTrilha = 0;

            if (ViewState["idItemTrilha"] != null)
            {
                //Obtém o Id do Item Trilha do viewstate
                idItemTrilha = (int)ViewState["idItemTrilha"];
            }

            if (idItemTrilha > 0)
            {

                ItemTrilha itemTrilha = new ManterItemTrilha().ObterItemTrilhaPorID(idItemTrilha);

                if (itemTrilha != null)
                {
                    string caminhoFisicoDoDiretorioDeUpload =
                        ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.RepositorioUpload).Registro;
                    string caminhoLogicoDoArquivo = string.Concat(caminhoFisicoDoDiretorioDeUpload,
                        "\\" + itemTrilha.FileServer.NomeDoArquivoNoServidor);

                    if (!File.Exists(caminhoLogicoDoArquivo))
                        throw new FileNotFoundException("Arquivo não encontrado no servidor!");

                    Response.ContentType = itemTrilha.FileServer.TipoArquivo;
                    Response.AddHeader("content-disposition",
                        String.Format("attachment; filename={0}", itemTrilha.FileServer.NomeDoArquivoOriginal));
                    HttpContext.Current.Response.WriteFile(caminhoLogicoDoArquivo);
                    Response.End();
                }
            }
        }

        protected void lkbArquivo_Click(object sender, EventArgs e)
        {
            try
            {
                BaixarArquivo();
            }
            catch (FileNotFoundException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        public ItemTrilha ObterObjetoItemTrilha(int? idItemTrilha)
        {
            _itemTrilhaEdicao = idItemTrilha.HasValue
                ? new ManterItemTrilha().ObterItemTrilhaPorID(idItemTrilha.Value)
                : new ItemTrilha();

            //Nome
            _itemTrilhaEdicao.Nome = txtNome.Text.Trim();

            int cargaHoraria;
            if (!string.IsNullOrEmpty(txtCargaHoraria.Text))
            {
                if (int.TryParse(txtCargaHoraria.Text, out cargaHoraria))
                {
                    _itemTrilhaEdicao.CargaHoraria = cargaHoraria;
                }
                else
                {
                    throw new AcademicoException("O campo Carga horaria deve ser númerico e em minutos.");
                }
            }

            // Missão
            // Se for edição, o índice 0 do DropDown de Missões será uma opção válida, e não uma opção inválida.
            if (idItemTrilha.HasValue
                ? ddlMissao.SelectedIndex < 0
                : ddlMissao.SelectedIndex <= 0)
                throw new AcademicoException("Selecione uma missão.");

            _itemTrilhaEdicao.Missao = new ManterMissao().ObterPorID(int.Parse(ddlMissao.SelectedValue));

            //Tipo de Solução
            if((enumTipoItemTrilha)int.Parse(ddlTipo.SelectedValue) == enumTipoItemTrilha.Solucoes )
            { 
                if( string.IsNullOrEmpty(ddlSolucao.SelectedValue) )
                {
                    throw new AcademicoException("Selecione uma solução educacional.");
                }
            }


            //Ativo ?
            if (ddlStatus.SelectedItem != null && !string.IsNullOrWhiteSpace(ddlStatus.SelectedItem.Value))
            {
                var valorInformadoParaStatus = ddlStatus.SelectedItem.Value;

                if (valorInformadoParaStatus.ToUpper().Equals("S"))
                    _itemTrilhaEdicao.Ativo = true;
                else if (valorInformadoParaStatus.ToUpper().Equals("N"))
                    _itemTrilhaEdicao.Ativo = false;
            }
            else
            {
                throw new AcademicoException("Informe se o Item da Trilha está Ativo Sim ou Não.");
            }

            // Moedas
            if (string.IsNullOrWhiteSpace(txtMoedas.Text))
                throw new AcademicoException("Campo \"Quantidade de Moedas\" é obrigatório");

            int moedas;
            if (int.TryParse(txtMoedas.Text, out moedas))
                _itemTrilhaEdicao.Moedas = moedas;
            else
                throw new AcademicoException("Valor inválido para o campo \"Quantidade de Moedas\"");

            //Forma Aquisição
            if (ddlFormaAquisicao.SelectedItem != null &&
                !string.IsNullOrWhiteSpace(ddlFormaAquisicao.SelectedItem.Value))
                _itemTrilhaEdicao.FormaAquisicao =
                    new BMFormaAquisicao().ObterPorID(int.Parse(ddlFormaAquisicao.SelectedItem.Value));

            // Tipo de item trilha
            if (ddlTipo.SelectedItem != null && !string.IsNullOrWhiteSpace(ddlTipo.SelectedItem.Value))
                _itemTrilhaEdicao.Tipo = new BMTipoItemTrilha().ObterPorId(int.Parse(ddlTipo.SelectedItem.Value));

            // Subitens do tipo
            switch ((enumTipoItemTrilha)_itemTrilhaEdicao.Tipo.ID)
            {
                // Discursivas ou jogos não precisam de referências.
                case enumTipoItemTrilha.Jogo:
                    _itemTrilhaEdicao.Questionario = null;
                    _itemTrilhaEdicao.SolucaoEducacionalAtividade = null;
                    _itemTrilhaEdicao.FaseJogo = (enumFaseJogo)int.Parse(ddlFase.SelectedValue);
                    break;
                case enumTipoItemTrilha.Discursiva:
                    ValidarCampoLinkConteudo();
                    ValdarCampoOrientacoes();
                    ValidarReferenciaBibliografica();
                    ValidarArquivoDeEnvio(txtNomeArquivo.Text);
                    ValidarPermissaoDeReenvio();

                    _itemTrilhaEdicao.Questionario = null;
                    _itemTrilhaEdicao.SolucaoEducacionalAtividade = null;
                    _itemTrilhaEdicao.FaseJogo = 0;
                    break;
                case enumTipoItemTrilha.Atividade:
                    ValidarCampoLinkConteudo();
                    ValdarCampoOrientacoes();
                    ValidarReferenciaBibliografica();
                    ValidarArquivoDeEnvio(txtNomeArquivo.Text);

                    // Verificar Questionário.
                    int questionarioId;
                    if (int.TryParse(ddlQuestionario.SelectedValue, out questionarioId))
                    {
                        var questionario = new ManterQuestionario().ObterQuestionarioPorID(questionarioId);

                        if (questionario != null)
                        {
                            _itemTrilhaEdicao.Questionario = questionario;
                            _itemTrilhaEdicao.SolucaoEducacionalAtividade = null;
                            break;
                        }
                    }
                    _itemTrilhaEdicao.FaseJogo = 0;

                    throw new AcademicoException("Questionário obrigatório para o tipo \"Atividade\"");
                case enumTipoItemTrilha.Solucoes:
                    // Verificar Solução Educacional.
                    int seId;
                    if (int.TryParse(ddlSolucao.SelectedValue, out seId))
                    {
                        var se = new ManterSolucaoEducacional().ObterSolucaoEducacionalPorId(seId);

                        if (se != null)
                        {
                            _itemTrilhaEdicao.SolucaoEducacionalAtividade = se;
                            _itemTrilhaEdicao.Questionario = null;
                            break;
                        }
                    }
                    _itemTrilhaEdicao.FaseJogo = 0;

                    throw new AcademicoException("Solução Educacional obrigatória para o tipo \"Soluções Educacionais\"");

                case enumTipoItemTrilha.ConheciGame:
                    int idTema;
                    int quantidadeAcertos;
                    if (int.TryParse(ddlTema.SelectedValue, out idTema) && idTema > 0)
                    {
                        _itemTrilhaEdicao.ID_TemaConheciGame = idTema;

                        if (!int.TryParse(txtAcertosTema.Text, out quantidadeAcertos) || quantidadeAcertos <= 0 || quantidadeAcertos > 30)
                        {
                            throw new AcademicoException("A quantidade de acertos deve ser maior que 0 e menor que 30");
                        }

                        _itemTrilhaEdicao.QuantidadeAcertosTema = !string.IsNullOrWhiteSpace(txtAcertosTema.Text) ? int.Parse(txtAcertosTema.Text) : 0;
                    }
                    else
                    {
                        throw new AcademicoException("Selecione um tema");
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            ValidarArquivoDeEnvio(txtNomeArquivo.Text);

            return _itemTrilhaEdicao;
        }

        private void ValidarPermissaoDeReenvio()
        {
            if (ddlPermiteReenvio.SelectedItem == null || string.IsNullOrWhiteSpace(ddlPermiteReenvio.SelectedItem.Value))
            {             
                throw new AcademicoException("Informe se é permitido o reenvio do arquivo.");
            }
            else
            {
                var valorInformadoParaStatus = ddlPermiteReenvio.SelectedItem.Value;

                if (valorInformadoParaStatus.ToUpper().Equals("S"))
                    _itemTrilhaEdicao.PermiteReenvioArquivo = true;
                else if (valorInformadoParaStatus.ToUpper().Equals("N"))
                    _itemTrilhaEdicao.PermiteReenvioArquivo = false;                
            }
        }

        private void ValidarArquivoDeEnvio(string nomeArquivo = "")
        {
            //Arquivo de Envio
            if (fupldArquivoEnvio != null && fupldArquivoEnvio.PostedFile != null &&
                fupldArquivoEnvio.PostedFile.ContentLength > 0)
            {
                try
                {

                    string nomeArquivoFinal = fupldArquivoEnvio.FileName;

                    if (!string.IsNullOrEmpty(nomeArquivo))
                    {

                        nomeArquivoFinal = nomeArquivo + "." + fupldArquivoEnvio.FileName.Split('.').Last();
                    }

                    var caminhoDiretorioUpload =
                        ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.RepositorioUpload).Registro;
                    var nomeAleatorioDoArquivoParaUploadCriptografado = WebFormHelper.ObterStringAleatoria();
                    var diretorioDeUploadComArquivo = string.Concat(caminhoDiretorioUpload, @"\",
                        nomeAleatorioDoArquivoParaUploadCriptografado);

                    try
                    {
                        //Salva o arquivo no caminho especificado
                        fupldArquivoEnvio.PostedFile.SaveAs(diretorioDeUploadComArquivo);
                    }
                    catch
                    {
                        //Todo: -> Logar o Erro
                        throw new AcademicoException("Ocorreu um erro ao Salvar o arquivo");
                    }
                    
                    _itemTrilhaEdicao.FileServer = new FileServer
                    {
                        NomeDoArquivoNoServidor = nomeAleatorioDoArquivoParaUploadCriptografado,
                        NomeDoArquivoOriginal = nomeArquivoFinal,
                        TipoArquivo = fupldArquivoEnvio.PostedFile.ContentType,
                        MediaServer = false
                    };
                }
                catch (AcademicoException ex)
                {
                    throw ex;
                }
                catch
                {
                    //Todo: -> Logar erro
                    throw new AcademicoException("Ocorreu um Erro ao Salvar o arquivo");
                }
            }
        }

        private void ValidarReferenciaBibliografica()
        {
            //Referência Bibliográfica
            _itemTrilhaEdicao.ReferenciaBibliografica = txtReferenciaBibliografica.Text.Trim();
        }

        private void ValdarCampoOrientacoes()
        {
            if (string.IsNullOrWhiteSpace(txtOrientacoes.Text))
                throw new AcademicoException("Campo \"Orientações para participação\" é obrigatório.");

            if (txtOrientacoes.Text.Length > 2000)
                throw new AcademicoException(
                    "Limite para o campo \"Orientações para participação\" é de 2000 caracteres.");

            _itemTrilhaEdicao.Local = txtOrientacoes.Text;
        }

        private void ValidarCampoLinkConteudo()
        {
            if (!string.IsNullOrEmpty(txtLinkAcesso.Text) && txtLinkAcesso.Text.StartsWith("www"))
            {
                txtLinkAcesso.Text = txtLinkAcesso.Text.Insert(0, "http://");
            }

            _itemTrilhaEdicao.LinkConteudo = txtLinkAcesso.Text;
        }


        private void PreencherComboTrilhas()
        {
            var listaTrilhas = new ManterTrilha().ObterTodasTrilhas();

            WebFormHelper.PreencherLista(listaTrilhas, ddlTrilha, false, true);
        }

        private void PreencherComboTrilhaNivel(Trilha trilha)
        {
            var listaTrilhaNivel = new ManterTrilhaNivel().ObterPorTrilha(trilha);

            if (listaTrilhaNivel != null && listaTrilhaNivel.Count > 0)
                WebFormHelper.PreencherLista(listaTrilhaNivel, ddlTrilhaNivel, false, true);
            else
                ddlTrilhaNivel.Items.Clear();
        }

        private void PreencherComboPontoSebrae(TrilhaNivel trilhaNivel)
        {
            var listaPontoSebrae = new ManterPontoSebrae().ObterPorTrilhaNivelAtivos(trilhaNivel);

            if (listaPontoSebrae != null && listaPontoSebrae.Any())
            {
                WebFormHelper.PreencherLista(listaPontoSebrae, ddlPontoSebrae, false, true);
            }
            else
            {
                ddlPontoSebrae.Items.Clear();
            }
        }

        private void PreencherComboMissao(PontoSebrae pontoSebrae)
        {
            var listaMissao = new ManterMissao().ObterPorPontoSebrae(pontoSebrae);

            if (listaMissao != null && listaMissao.Any())
                WebFormHelper.PreencherLista(listaMissao, ddlMissao, false, true);
            else
                ddlTrilhaNivel.Items.Clear();
        }

        private void PreencherCombos()
        {
            try
            {
                PreencherComboTrilhas();
                PreencherComboTipo();
                PreencherComboTrilhaFormaAquisicao();
                PreencherComboStatus();
                PreencherComboReenviarArquivo();
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }
        private void PreencherComboReenviarArquivo()
        {
            WebFormHelper.PreencherComponenteComOpcoesSimNao(ddlPermiteReenvio, true);
        }

        private void PreencherComboStatus()
        {
            WebFormHelper.PreencherComponenteComOpcoesSimNao(ddlStatus, true);
        }

        private void PreencherComboTipo()
        {
            var tiposItensTrilha = new BMTipoItemTrilha().ObterTodos();

            WebFormHelper.PreencherLista(tiposItensTrilha, ddlTipo, false,
                _itemTrilhaEdicao == null || _itemTrilhaEdicao.ID == 0);
        }

        private void PreencherComboTrilhaFormaAquisicao()
        {
            ManterFormaAquisicao manterFormaAquisicao = new ManterFormaAquisicao();
            IList<FormaAquisicao> ListaFormaAquisicao = manterFormaAquisicao.ObterTodasFormaAquisicao();

            if (Operacao.Equals(enumOperacao.Consulta))
            {
                WebFormHelper.PreencherLista(ListaFormaAquisicao, this.ddlFormaAquisicao, true, false);
            }
            else if (Operacao.Equals(enumOperacao.Edicao))
            {
                WebFormHelper.PreencherLista(ListaFormaAquisicao, this.ddlFormaAquisicao, false, true);
            }
        }

        #endregion

        private void PreencherCampos(ItemTrilha itemTrilhaEdicao)
        {
            if (itemTrilhaEdicao != null)
            {
                WebFormHelper.SetarValorNaCombo(itemTrilhaEdicao.Missao.PontoSebrae.TrilhaNivel.Trilha.ID.ToString(), ddlTrilha);

                ViewState.Add("idItemTrilha", itemTrilhaEdicao.ID);

                var listaTrilhaNivel =
                    new BMTrilhaNivel().ObterPorFiltro(new TrilhaNivel { Trilha = _itemTrilhaEdicao.Missao.PontoSebrae.TrilhaNivel.Trilha });
                listaTrilhaNivel.Add(itemTrilhaEdicao.Missao.PontoSebrae.TrilhaNivel);

                WebFormHelper.PreencherLista(listaTrilhaNivel, ddlTrilhaNivel, false, true);

                WebFormHelper.SetarValorNaCombo(itemTrilhaEdicao.Missao.PontoSebrae.TrilhaNivel.ID.ToString(), ddlTrilhaNivel);

                if (itemTrilhaEdicao.Tipo != null)
                {
                    divFormaAquisicao.Visible = true;

                    WebFormHelper.SetarValorNaCombo(itemTrilhaEdicao.Tipo.ID.ToString(), ddlTipo);

                    switch ((enumTipoItemTrilha)itemTrilhaEdicao.Tipo.ID)
                    {
                        case enumTipoItemTrilha.Jogo:
                            divFaseJogo.Visible = true;
                            divOrientacoes.Visible = true;

                            PreencherFasesJogo();

                            WebFormHelper.SetarValorNaCombo(((int)itemTrilhaEdicao.FaseJogo).ToString(), ddlFase);
                            break;
                        case enumTipoItemTrilha.Discursiva:
                            pnlArtigoOnline.Visible = true;
                            break;
                        case enumTipoItemTrilha.Atividade:
                            divQuestionario.Visible = true;

                            var questionarios = new ManterQuestionario().ObterQuestionariosItemTrilha().ToList();

                            if (itemTrilhaEdicao.Questionario != null &&
                                questionarios.All(x => x.ID != itemTrilhaEdicao.Questionario.ID))
                            {
                                questionarios.Add(itemTrilhaEdicao.Questionario);
                            }

                            WebFormHelper.PreencherLista(questionarios.OrderBy(x => x.Nome).ToList(), ddlQuestionario,
                                false, true);

                            if (itemTrilhaEdicao.Questionario != null)
                                WebFormHelper.SetarValorNaCombo(itemTrilhaEdicao.Questionario.ID.ToString(),
                                    ddlQuestionario);
                            break;
                        case enumTipoItemTrilha.Solucoes:
                            divSolucao.Visible = true;
                            ddlFormaAquisicao.Enabled = false;

                            WebFormHelper.PreencherLista(
                                new ManterSolucaoEducacional().ObterTodosSolucaoEducacional().ToList(), ddlSolucao);

                            if (itemTrilhaEdicao.SolucaoEducacionalAtividade != null)
                                WebFormHelper.SetarValorNaCombo(
                                    itemTrilhaEdicao.SolucaoEducacionalAtividade.ID.ToString(), ddlSolucao);
                            break;
                        case enumTipoItemTrilha.ConheciGame:
                            var conteudos = new ManterConteudo().ObterTodos().Where(x => x.Ativo).ToList();
                            WebFormHelper.PreencherListaCustomizado(conteudos, ddlConteudo, "ID", "Nome", false, true);

                            var manterTema = new ManterTema();

                            var temas = manterTema.ObterTodos().ToList();
                            WebFormHelper.PreencherListaCustomizado(temas, ddlTema, "ID", "Nome", false, true);

                            var temaSelecionado = manterTema.ObterPorID(itemTrilhaEdicao.ID_TemaConheciGame);

                            if (temaSelecionado != null && temaSelecionado.Conteudo != null)
                            {
                                pnlConheciGame.Visible = true;
                                pnlTemaConheciGame.Visible = true;

                                WebFormHelper.SetarValorNaCombo(temaSelecionado.Conteudo.ID.ToString(), ddlConteudo);
                                WebFormHelper.SetarValorNaCombo(temaSelecionado.ID.ToString(), ddlTema);

                                txtAcertosTema.Text = itemTrilhaEdicao.QuantidadeAcertosTema.ToString();
                            }

                            break;
                        
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    ddlTipo.Enabled = !itemTrilhaEdicao.ListaItemTrilhaParticipacao.Any();
                }

                // Trilha Nível
                WebFormHelper.PreencherLista(itemTrilhaEdicao.Missao.PontoSebrae.TrilhaNivel.Trilha.ListaTrilhaNivel, ddlTrilhaNivel);
                WebFormHelper.SetarValorNaCombo(itemTrilhaEdicao.Missao.PontoSebrae.TrilhaNivel.ID.ToString(), ddlTrilhaNivel);

                // Ponto Sebrae
                WebFormHelper.PreencherLista(itemTrilhaEdicao.Missao.PontoSebrae.TrilhaNivel.ListaPontoSebrae, ddlPontoSebrae);
                WebFormHelper.SetarValorNaCombo(itemTrilhaEdicao.Missao.PontoSebrae.ID.ToString(), ddlPontoSebrae);

                // Missão
                WebFormHelper.PreencherLista(itemTrilhaEdicao.Missao.PontoSebrae.ListaMissoes, ddlMissao);
                WebFormHelper.SetarValorNaCombo(itemTrilhaEdicao.Missao.ID.ToString(), ddlMissao);


                if (itemTrilhaEdicao.FormaAquisicao != null && itemTrilhaEdicao.FormaAquisicao.ID > 0)
                {
                    WebFormHelper.SetarValorNaCombo(itemTrilhaEdicao.FormaAquisicao.ID.ToString(), ddlFormaAquisicao);
                }

                if (_itemTrilhaEdicao.Ativo.HasValue)
                {
                    WebFormHelper.SetarValorNaCombo(itemTrilhaEdicao.Ativo.Value ? "S" : "N", ddlStatus);
                }

                txtCargaHoraria.Text = itemTrilhaEdicao.CargaHoraria > 0 ? itemTrilhaEdicao.CargaHoraria.ToString() : string.Empty;

                txtNome.Text = itemTrilhaEdicao.Nome;
                txtMoedas.Text = itemTrilhaEdicao.Moedas.ToString();

                txtOrientacoes.Text = _itemTrilhaEdicao.Local;
                txtLinkAcesso.Text = _itemTrilhaEdicao.LinkConteudo;
                txtReferenciaBibliografica.Text = _itemTrilhaEdicao.ReferenciaBibliografica;

                //Arquivo de Envio 
                if (_itemTrilhaEdicao.FileServer != null &&
                    !string.IsNullOrWhiteSpace(_itemTrilhaEdicao.FileServer.NomeDoArquivoOriginal))
                {
                    lkbArquivo.Text = string.Concat("Abrir arquivo ", _itemTrilhaEdicao.FileServer.NomeDoArquivoOriginal);
                }
            }
        }

        public void InformarAcaoDeEdicao()
        {
            spanAcao.InnerText = "Edição de Item Trilha";
            h3Acao.Visible = true;
        }

        public void InformarAcaoDeConsulta()
        {
            Session["ItemTrilhaEdit"] = null;
            spanAcao.InnerText = "Filtro de Item Trilha";
            h3Acao.Visible = true;
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Session["ItemTrilhaEdit"] = null;
            Response.Redirect("EdicaoItemTrilha.aspx");
        }

        protected void ddlTrilha_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlPontoSebrae.Items.Clear();
            ddlMissao.Items.Clear();

            if (ddlTrilha.SelectedIndex > 0)
            {
                //Busca os níveis associados à trilha
                PreencherComboTrilhaNivel(new Trilha { ID = int.Parse(ddlTrilha.SelectedValue) });
            }
        }

        protected void ddlTrilhaNivel_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            ddlMissao.ClearSelection();

            if (ddlTrilhaNivel.SelectedIndex > 0)
            {
                //Busca os níveis associados à trilha
                PreencherComboPontoSebrae(new TrilhaNivel { ID = int.Parse(ddlTrilhaNivel.SelectedValue) });
            }
        }

        protected void ddlPontoSebrae_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPontoSebrae.SelectedIndex > 0)
            {
                //Busca os níveis associados à trilha
                PreencherComboMissao(new PontoSebrae { ID = int.Parse(ddlPontoSebrae.SelectedValue) });
            }
        }

        protected void ddlTipo_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            OcultarTodasDivs();

            divFaseJogo.Visible = divSolucao.Visible = divQuestionario.Visible = false;

            divFormaAquisicao.Visible = true;
            pnlArtigoOnline.Visible = false;
            ddlFormaAquisicao.Enabled = true;
            ddlFormaAquisicao.SelectedIndex = 0;
            txtCargaHoraria.Enabled = (enumTipoItemTrilha)int.Parse(ddlTipo.SelectedValue) !=  enumTipoItemTrilha.Solucoes;
            switch ((enumTipoItemTrilha)int.Parse(ddlTipo.SelectedValue))
            {
                case enumTipoItemTrilha.Discursiva:
                    divOrientacoes.Visible = true;
                    divLinkAcesso.Visible = true;
                    divReferenciaBibliografica.Visible = true;
                    divArquivoEnvio.Visible = true;
                    divNomeArquivo.Visible = true;
                    pnlArtigoOnline.Visible = true;

                    break;
                case enumTipoItemTrilha.Jogo:
                    divFaseJogo.Visible = true;
                    divOrientacoes.Visible = true;

                    PreencherFasesJogo();
                    break;
                case enumTipoItemTrilha.Solucoes:
                    divSolucao.Visible = true;
                    ddlFormaAquisicao.Enabled = false;

                    PreencherSolucoes();
                    break;
                case enumTipoItemTrilha.Atividade:
                    divQuestionario.Visible = true;
                    divOrientacoes.Visible = true;
                    divLinkAcesso.Visible = true;
                    divReferenciaBibliografica.Visible = true;
                    divArquivoEnvio.Visible = true;
                    divNomeArquivo.Visible = true;

                    PreencherQuestionarios();
                    break;
                case enumTipoItemTrilha.ConheciGame:
                    var formaDeAquisicao = new ManterFormaAquisicao().ObterTodosIQueryable().Where(x => x.Nome.Contains("Jogo Online"));
                    WebFormHelper.PreencherLista(formaDeAquisicao, ddlFormaAquisicao);

                    PreencherConteudoConheciGame();

                    pnlConheciGame.Visible = true;
                    break;
            }
        }

        private void PreencherConteudoConheciGame()
        {
            var todos = new ManterConteudo().ObterTodos().Where(x => x.Ativo).ToList();
            WebFormHelper.PreencherListaCustomizado(todos, ddlConteudo, "ID", "Nome", false, true);
        }

        private void PreencherSolucoes()
        {
            WebFormHelper.PreencherLista(new ManterSolucaoEducacional().ObterTodosSolucaoEducacional().ToList(),
                ddlSolucao, false, true);
        }

        private void PreencherFasesJogo()
        {
            var fases =
                Enum.GetValues(typeof(enumFaseJogo))
                    .Cast<enumFaseJogo>()
                    .Select(x => new { Nome = x.GetDescription(), ID = (int)x })
                    .ToList();

            WebFormHelper.PreencherLista(fases, ddlFase, false, true);
        }

        private void PreencherQuestionarios()
        {
            WebFormHelper.PreencherLista(new ManterQuestionario().ObterQuestionariosItemTrilha().ToList(),
                ddlQuestionario, false, true);
        }

        private void OcultarTodasDivs()
        {
            divFormaAquisicao.Visible = false;
            divSolucao.Visible = false;
            divFaseJogo.Visible = false;
            divQuestionario.Visible = false;
            divOrientacoes.Visible = false;
            divLinkAcesso.Visible = false;
            divReferenciaBibliografica.Visible = false;
            divArquivoEnvio.Visible = false;
        }

        protected void ddlSolucao_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var solucao =
                new ManterSolucaoEducacional().ObterSolucaoEducacionalPorId(int.Parse(ddlSolucao.SelectedValue));

            if (solucao == null)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Solução Educacional inválida");
                return;
            }
            txtCargaHoraria.Text = solucao.CargaHoraria.ToString();

            WebFormHelper.SetarValorNaCombo(solucao.FormaAquisicao.ID.ToString(), ddlFormaAquisicao, true);
        }

        protected void ddlConteudo_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idSelecionado;
            if (int.TryParse(ddlConteudo.SelectedValue, out idSelecionado))
            {
                var temas = new ManterTema().ObterPorConteudo(idSelecionado).ToList();
                WebFormHelper.PreencherListaCustomizado(temas, ddlTema, "ID", "Nome", false, true);

                pnlTemaConheciGame.Visible = true;
            }
        }
    }
}