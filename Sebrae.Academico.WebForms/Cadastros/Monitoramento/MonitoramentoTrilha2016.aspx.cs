using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.Services;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.WebForms.UserControls;
using ManterItemTrilhaParticipacao = Sebrae.Academico.BP.ManterItemTrilhaParticipacao;

namespace Sebrae.Academico.WebForms.Cadastros.Monitoramento
{
    public partial class MonitoramentoTrilha2016 : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                WebFormHelper.PreencherLista(new BMTrilha().ObterTrilhas(), ddlTrilha, false, true);
                PreencherTabelas();
            }
        }

        protected override enumFuncionalidade Funcionalidade
        {
            get
            {
                return enumFuncionalidade.Trilha;
            }
        }

        protected override IList<enumPerfil> PerfisNecessariosParaAcessarAFuncionalidade
        {
            get
            {
                return new List<enumPerfil>();
            }
        }

        private void PreencherTabelas(Usuario usuario = null)
        {
            var uId = usuario != null
                ? usuario.ID
                : (ucLupaUsuario.SelectedUser != null ? (int?)ucLupaUsuario.SelectedUser.ID : null);

            PreencherSolucoesEducacionais(uId);
        }

        protected void ddlTrilha_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTrilha.SelectedIndex > 0)
            {
                var idTrilha = int.Parse(ddlTrilha.SelectedValue);

                ddlPontoSebrae.Items.Clear();
                ddlMissao.Items.Clear();

                WebFormHelper.PreencherLista(
                    new ManterTrilhaNivel().ObterPorTrilha(new Trilha { ID = idTrilha })
                        .OrderBy(x => x.Nome)
                        .ToList(), ddlTrilhaNivel, false, true);

                PreencherTabelas();
            }
        }

        protected void ddlTrilhaNivel_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTrilha.SelectedIndex <= 0 || ddlTrilhaNivel.SelectedIndex <= 0) return;

            var idTrilhaNivel = int.Parse(ddlTrilhaNivel.SelectedValue);

            ddlMissao.Items.Clear();

            var listaPontoSebrae =
                new ManterPontoSebrae().ObterPorTrilhaNivel(new Dominio.Classes.TrilhaNivel { ID = idTrilhaNivel });

            WebFormHelper.PreencherLista(listaPontoSebrae.ToList(), ddlPontoSebrae, false, true);

            PreencherTabelas();
        }

        protected void ddlPontoSebrae_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var listaMissoes =
                new ManterMissao().ObterPorPontoSebrae(new Dominio.Classes.PontoSebrae { ID = int.Parse(ddlPontoSebrae.SelectedValue) });

            WebFormHelper.PreencherLista(listaMissoes.ToList(), ddlMissao, false, true);

            PreencherTabelas();
        }

        protected void ddlMissao_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            PreencherTabelas();
        }

        protected void dataEnvioInicio_OnTextChanged(object sender, EventArgs e)
        {
            if (dataEnvioInicio.Text.Length != 10)
                dataEnvioInicio.Text = null;

            PreencherTabelas();
        }

        protected void dataEnvioFinal_OnTextChanged(object sender, EventArgs e)
        {
            if (dataEnvioFinal.Text.Length != 10)
                dataEnvioFinal.Text = null;

            PreencherTabelas();
        }

        private void PreencherSolucoesEducacionais(int? idUsuario = null, int page = 0)
        {
            var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

            int? idMonitor = null;

            if (usuarioLogado.IsMonitorTrilha() && !usuarioLogado.IsAdministrador())
                idMonitor = usuarioLogado.ID;

            var lista = ObterListaItemTrilhaParticipacao(idMonitor, idUsuario);

            var dataInicio = CommonHelper.TratarData(dataEnvioInicio.Text.Trim(), "Data de envio inicial");

            if (dataInicio.HasValue)
                lista = lista.Where(x => x.DataEnvio.Date >= dataInicio);

            var dataFim = CommonHelper.TratarData(dataEnvioFinal.Text.Trim(), "Data de envio Final");

            if (dataFim.HasValue)
                lista = lista.Where(x => x.DataEnvio.Date <= dataFim);

            if (idUsuario != null && idUsuario > 0)
                lista =
                    lista.Where(
                        x =>
                            x.UsuarioTrilha != null && x.UsuarioTrilha.Usuario != null &&
                            x.UsuarioTrilha.Usuario.ID == idUsuario);

            WebFormHelper.PaginarGrid(lista.ToList(), gvSolucoesEducacionais, page);
        }


        private IQueryable<ItemTrilhaParticipacao> ObterListaItemTrilhaParticipacao(int? idMonitor,
            int? idUsuario = null)
        {
            // Filtar pelo Status.
            var aprovadas = cblStatus.Items[0].Selected;
            var emRevisao = cblStatus.Items[1].Selected;
            var pendentes = cblStatus.Items[2].Selected;
            var suspensas = cblStatus.Items[3].Selected;

            var lista = new ManterItemTrilhaParticipacao().ObterSolucoesRecentes2016(aprovadas, emRevisao, pendentes,
                suspensas);

            if (lista.Any() && idMonitor.HasValue)
                lista =
                    lista.Where(
                        x =>
                            x.ItemTrilha.Missao.PontoSebrae.TrilhaNivel.Monitor != null &&
                            x.ItemTrilha.Missao.PontoSebrae.TrilhaNivel.Monitor.ID == idMonitor.Value);

            if (ddlMissao.SelectedIndex > 0)
                lista =
                    lista.Where(x => x.ItemTrilha.Missao.ID == int.Parse(ddlMissao.SelectedValue));
            else
            {
                if (ddlPontoSebrae.SelectedIndex > 0)
                    lista =
                        lista.Where(x => x.ItemTrilha.Missao.PontoSebrae.ID == int.Parse(ddlPontoSebrae.SelectedValue));
                else if (ddlTrilhaNivel.SelectedIndex > 0)
                    lista =
                        lista.Where(
                            x =>
                                x.ItemTrilha.Missao.PontoSebrae.TrilhaNivel.ID ==
                                int.Parse(ddlTrilhaNivel.SelectedValue));
                else if (ddlTrilha.SelectedIndex > 0)
                    lista =
                        lista.Where(
                            x =>
                                x.ItemTrilha.Missao.PontoSebrae.TrilhaNivel.Trilha.ID ==
                                int.Parse(ddlTrilha.SelectedValue));
            }

            if (idUsuario != null && idUsuario > 0)
                lista =
                    lista.Where(
                        x =>
                            x.UsuarioTrilha != null && x.UsuarioTrilha.Usuario != null &&
                            x.UsuarioTrilha.Usuario.ID == idUsuario);

            return lista.OrderByDescending(x => x.DataEnvio).AsQueryable();
        }

        private IQueryable<ItemTrilha> FiltrarPorStatus(IQueryable<ItemTrilha> lista)
        {
            // Filtar pelo Status.
            var aprovadas = cblStatus.Items[0].Selected;
            var emRevisao = cblStatus.Items[1].Selected;
            var pendentes = cblStatus.Items[2].Selected;

            lista = lista.Where(x => x.Aprovado == enumStatusSolucaoEducacionalSugerida.Aprovado ||
                                x.Aprovado == enumStatusSolucaoEducacionalSugerida.NaoAprovado ||
                                x.Aprovado == enumStatusSolucaoEducacionalSugerida.Pendente);
            return lista;
        }

        protected void cblStatus_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            PreencherTabelas();
        }

        protected void gvSolucoesEducacionais_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ValidarDataBoundPorSolucaoEducacional(sender, e);
        }

        protected void gvSolucoesEducacionais_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            ValidarRowCommandPorSolucaoEducacional(sender, e);
        }

        protected void ValidarDataBoundPorSolucaoEducacional(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {

            }
        }

        private void LimparObjetosItemTrilha()
        {
            _ItemTrilha = null;
        }

        protected void ValidarRowCommandPorSolucaoEducacional(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Page")
            {
                int idParticipacao = int.Parse(e.CommandArgument.ToString());
                MontarPainelSe(idParticipacao);
                base.ExibirBackDrop();
                pnlTrilhaParticipacao.Visible = true;
            }
        }

        private void MontarPainelSe(int idParticipacao)
        {
            LimparObjetos();

            _ItemTrilhaParticipacao = new BMItemTrilhaParticipacao().ObterPorId(idParticipacao);


            if (_ItemTrilhaParticipacao != null)
            {
                List<ItemTrilhaParticipacao> listaHistorico = new BMItemTrilhaParticipacao().ObterParticipacoesUsuarioTrilha(_ItemTrilhaParticipacao.ItemTrilha.ID, _ItemTrilhaParticipacao.UsuarioTrilha.ID);

                if (listaHistorico != null && listaHistorico.Count > 0)
                    listaHistorico = listaHistorico.Where(x => x.ID != idParticipacao).ToList();

                var itemTrilha = (new BMItemTrilha()).ObterPorID(_ItemTrilhaParticipacao.ItemTrilha.ID);
                var se = itemTrilha.SolucaoEducacionalAtividade;
                lblOrientacaoParticipacao.Text = string.IsNullOrEmpty(_ItemTrilhaParticipacao.Orientacao) ? (itemTrilha.Local ?? "Sem conteúdo.") : (_ItemTrilhaParticipacao.Orientacao ?? "Sem conteúdo.");
                lblObjetivo.Text = itemTrilha.Objetivo != null ? itemTrilha.Objetivo.Nome : se == null ? "Sem conteúdo." : se.Objetivo ?? "Sem conteúdo.";
                lblFormaAquisicao.Text = itemTrilha.FormaAquisicao != null ? itemTrilha.FormaAquisicao.Nome : "Sem conteúdo.";
                lblReferenciaBibliografica.Text = itemTrilha.ReferenciaBibliografica;
                if (!string.IsNullOrEmpty(itemTrilha.LinkConteudo))
                {
                    lblLinkConteudo.Text = "Clique para acessar.";
                    lblLinkConteudo.NavigateUrl = itemTrilha.LinkConteudo;
                }
                else
                {
                    lblLinkConteudo.Text = "Sem conteúdo.";
                    lblLinkConteudo.NavigateUrl = "#";
                }
                if (_ItemTrilhaParticipacao.ItemTrilha.FileServer != null && _ItemTrilhaParticipacao.ItemTrilha.FileServer.ID > 0)
                {
                    var file =
                        new ManterFileServer().ObterFileServerPorID(_ItemTrilhaParticipacao.ItemTrilha.FileServer.ID);

                    lblLinkAcessoConteudo.NavigateUrl = lblLinkAcessoConteudo.Text = ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.EnderecoSGUS).Registro + "/ExibirFileServer.ashx?Identificador=" + file.NomeDoArquivoNoServidor;
                }
                else
                {
                    lblLinkAcessoConteudo.Text = "Sem Anexo";
                    lblLinkAcessoConteudo.NavigateUrl = "#";
                }

                divObjetivoServer.Visible = true;
                divFormaAquisicao.Visible = true;
                divReferenca.Visible = true;
                divLinkConteudo.Visible = true;
                divLinkAcesso.Visible = true;

                var usuaroTrilha = new ManterUsuarioTrilha().ObterPorId(_ItemTrilhaParticipacao.UsuarioTrilha.ID);

                txtUsuarioModal.Text = usuaroTrilha.Usuario.Nome;
                txtTrilhaModal.Text = itemTrilha.Missao.PontoSebrae.TrilhaNivel.Trilha.Nome;
                txtNivelModal.Text = itemTrilha.Missao.PontoSebrae.TrilhaNivel.Nome;
                txtPontoSebraeModal.Text = itemTrilha.Missao.PontoSebrae.NomeExibicao;
                txtSolucaoEducacionalModal.Text = itemTrilha.Nome;
                txtParticipacaoModal.Text = _ItemTrilhaParticipacao.TextoParticipacao;
                txtObservacao.Text = string.Empty;
                txtAutorizado.Text = _ItemTrilhaParticipacao.AutorizadoFormatado;
                rblAutorizadoModal.SelectedIndex = -1;

                WebFormHelper.PreencherGrid(listaHistorico.OrderByDescending(x => x.DataAlteracao).ToList(), gvHistoricoParticipacaoModal);
                hplnkCaminhoArquivo.Visible = false;
                if (_ItemTrilhaParticipacao.FileServer != null && _ItemTrilhaParticipacao.FileServer.ID > 0)
                {
                    MontarLinkDownload(_ItemTrilhaParticipacao.FileServer);
                }

            }
        }

        private void MontarLinkDownload(FileServer fileServer)
        {
            string caminhoArquivoAshx = string.Format(@"/ExibirFileServer.ashx?Identificador={1}", HttpContext.Current.Request.Url, fileServer.NomeDoArquivoNoServidor);
            string urlCompleta = HttpContext.Current.Request.Url.ToString();
            urlCompleta = urlCompleta.Replace(HttpContext.Current.Request.RawUrl, string.Concat("/", caminhoArquivoAshx));
            hplnkCaminhoArquivo.Visible = true;
            hplnkCaminhoArquivo.Text = urlCompleta;
            hplnkCaminhoArquivo.NavigateUrl = caminhoArquivoAshx;
        }

        protected void btnEnviarObservacao_OnClick(object sender, EventArgs e)
        {
            if (rblAutorizadoModal.SelectedIndex == -1)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "O status é obrigatório");
            }
            else
            {
                AlterarItemTrilhaParticipacao();
                OcultarModal();
                PreencherTabelas();
            }

        }

        private void AlterarItemTrilhaParticipacao()
        {
            if (_ItemTrilhaParticipacao != null)
            {

                ItemTrilhaParticipacao itemNovo = new ItemTrilhaParticipacao();
                itemNovo.UsuarioTrilha = _ItemTrilhaParticipacao.UsuarioTrilha;
                itemNovo.ItemTrilha = _ItemTrilhaParticipacao.ItemTrilha;
                itemNovo.Monitor = new BMUsuario().ObterUsuarioLogado();
                itemNovo.TextoParticipacao = txtObservacao.Text;
                itemNovo.Visualizado = false;
                itemNovo.TipoParticipacao = enumTipoParticipacaoTrilha.InteracaoMonitor;
                itemNovo.DataEnvio = DateTime.Now;

                //Anexo
                if (fupldArquivoEnvio != null && fupldArquivoEnvio.PostedFile != null && fupldArquivoEnvio.PostedFile.ContentLength > 0)
                {
                    itemNovo.FileServer = ObterAnexo();
                }
                else
                {
                    itemNovo.FileServer = null;
                }

                try
                {
                    new ManterItemTrilhaParticipacao().CadastrarHistorico(itemNovo, chkEnviarEmail.Checked);
                }
                catch (EmailException)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Erro ao enviar o email. Dados salvos com sucesso!");
                }

                try
                {
                    bool? rbautorizado = null;
                    if (rblAutorizadoModal.SelectedValue == "S")
                        rbautorizado = true;
                    else if (rblAutorizadoModal.SelectedValue == "N")
                        rbautorizado = false;

                    if (rbautorizado != _ItemTrilhaParticipacao.Autorizado)
                    {
                        AlterarStatusItemTrilhaParticipacao(_ItemTrilhaParticipacao, rbautorizado);
                    }


                    // Pega uma nova referência já que a antiga de ItemTrilha não tem a autorização acima
                    itemNovo.ItemTrilha = new ManterItemTrilha().ObterItemTrilhaPorID(itemNovo.ItemTrilha.ID);

                    // Inclui caso seja a data de última participação do usuário, precisa ficar depois do salvamento da autorização
                    new ManterTrilhaTopicoTematicoParticipacao().IncluirUltimaParticipacao(itemNovo.UsuarioTrilha, itemNovo.ItemTrilha);

                    // Se foi aprovado, criar visualização da mensagem para o aluno.
                    if (itemNovo.ItemTrilha.ObterStatusParticipacoesItemTrilha(itemNovo.UsuarioTrilha) ==
                        enumStatusParticipacaoItemTrilha.Aprovado)
                    {
                        var mensagem = new ManterMensagemGuia().ObterPorId(enumMomento.DemaisConclusoesSolucaoSebrae);

                        TrilhaServices.RegistrarVisualizacao(itemNovo.UsuarioTrilha, mensagem, null, itemNovo.ItemTrilha, salvarData: false);
                    }

                    try
                    {
                        MontarPainelSe(_ItemTrilhaParticipacao.ID);
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados alterados com sucesso");
                    }
                    catch (Exception)
                    {
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Atencao, "A participação do aluno foi alterada com sucesso, porém não foi possível atualizar a tela. Atualize a tela manualmente.");
                    }
                }
                catch
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Erro ao atualizar a participação do usuário");
                }
            }
        }

        private void AlterarStatusItemTrilhaParticipacao(ItemTrilhaParticipacao _itemTrilhaParticipacao, bool? aprovado)
        {
            var itemTrilhaParticipacao = new BMItemTrilhaParticipacao().ObterPorId(_itemTrilhaParticipacao.ID);
            itemTrilhaParticipacao.Autorizado = aprovado;

            if (aprovado.HasValue)
            {
                itemTrilhaParticipacao.DataPrazoAvaliacao = null;
            }

            // Inclui as moedas da trilha caso o usuário seja aprovado.
            if (itemTrilhaParticipacao.Autorizado != null && itemTrilhaParticipacao.Autorizado.Value &&
                itemTrilhaParticipacao.ItemTrilha.Moedas != null)
            {
                new ManterUsuarioTrilhaMoedas().Incluir(itemTrilhaParticipacao.UsuarioTrilha, itemTrilhaParticipacao.ItemTrilha, null, 0, (int)itemTrilhaParticipacao.ItemTrilha.Moedas);
            }

            var bmItemTrilhaParticipacao = new BMItemTrilhaParticipacao();

            bmItemTrilhaParticipacao.LimparSessao();
            bmItemTrilhaParticipacao.Salvar(itemTrilhaParticipacao);

            new ManterItemTrilhaParticipacao().GerarNotificacaoItemTrilha(itemTrilhaParticipacao);

        }

        private FileServer ObterAnexo()
        {
            try
            {

                string caminhoDiretorioUpload = ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.RepositorioUpload).Registro;
                string nomeAleatorioDoArquivoParaUploadCriptografado = WebFormHelper.ObterStringAleatoria();
                string diretorioDeUploadComArquivo = string.Concat(caminhoDiretorioUpload, @"\", nomeAleatorioDoArquivoParaUploadCriptografado);
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

                FileServer retornoAnexo = new FileServer();
                retornoAnexo = new FileServer();
                retornoAnexo.NomeDoArquivoNoServidor = nomeAleatorioDoArquivoParaUploadCriptografado;
                retornoAnexo.NomeDoArquivoOriginal = fupldArquivoEnvio.FileName;
                retornoAnexo.TipoArquivo = fupldArquivoEnvio.PostedFile.ContentType;
                retornoAnexo.MediaServer = false;
                return retornoAnexo;

            }
            catch
            {
                //Todo: -> Logar erro
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Erro ao salvar o anexo");
                return null;
            }
        }

        protected void OcultarMatriculaTurma_Click(object sender, EventArgs e)
        {
            this.OcultarModal();
        }

        private void OcultarModal()
        {
            LimparObjetos();
            base.OcultarBackDrop();
            pnlTrilhaParticipacao.Visible = false;
        }

        private void LimparObjetos()
        {
            _ItemTrilhaParticipacao = null;
            _TrilhaAtividadeFormativaParticipacao = null;
        }

        protected ItemTrilha _ItemTrilha
        {
            get
            {
                if (Session["_ItemTrilha"] != null)
                {
                    return (ItemTrilha)Session["_ItemTrilha"];
                }

                return null;
            }
            set
            {
                Session["_ItemTrilha"] = value;
            }
        }

        protected ItemTrilhaParticipacao _ItemTrilhaParticipacao
        {
            get
            {
                if (Session["_ItemTrilhaParticipaca"] != null)
                {
                    return (ItemTrilhaParticipacao)Session["_ItemTrilhaParticipaca"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Session["_ItemTrilhaParticipaca"] = value;
            }
        }

        protected TrilhaAtividadeFormativaParticipacao _TrilhaAtividadeFormativaParticipacao
        {
            get
            {
                if (Session["_TrilhaAtividadeFormativaParticipacao"] != null)
                {
                    return (TrilhaAtividadeFormativaParticipacao)Session["_TrilhaAtividadeFormativaParticipacao"];
                }
                return null;
            }
            set
            {
                Session["_TrilhaAtividadeFormativaParticipacao"] = value;
            }
        }


        protected override void OnPreRenderComplete(EventArgs e)
        {
            //Manter a formatação (Header)
            if (gvSolucoesEducacionais.Rows.Count > 0 && gvSolucoesEducacionais.HeaderRow != null)
            {
                gvSolucoesEducacionais.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void gvHistoricoParticipacaoModal_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                HyperLink hplnkCaminhoArquivo = (HyperLink)e.Row.FindControl("hplnkCaminhoArquivo");

                if (hplnkCaminhoArquivo != null)
                {
                    try
                    {
                        ItemTrilhaParticipacao registro = (ItemTrilhaParticipacao)e.Row.DataItem;

                        if (registro != null && registro.FileServer != null)
                        {
                            string caminhoArquivoAshx = string.Format(@"/ExibirFileServer.ashx?Identificador={0}", registro.FileServer.NomeDoArquivoNoServidor);
                            string urlCompleta = HttpContext.Current.Request.Url.ToString();
                            //urlCompleta = urlCompleta.Replace(HttpContext.Current.Request.RawUrl, string.Concat("/", caminhoArquivoAshx));

                            hplnkCaminhoArquivo.Text = "Anexo";
                            hplnkCaminhoArquivo.NavigateUrl = caminhoArquivoAshx;
                        }
                        else
                        {
                            hplnkCaminhoArquivo.Visible = false;
                        }
                    }
                    catch
                    {
                        try
                        {
                            TrilhaAtividadeFormativaParticipacao registro = (TrilhaAtividadeFormativaParticipacao)e.Row.DataItem;

                            if (registro != null && registro.FileServer != null)
                            {
                                string caminhoArquivoAshx = string.Format(@"/ExibirFileServer.ashx?Identificador={0}", registro.FileServer.NomeDoArquivoNoServidor);
                                string urlCompleta = HttpContext.Current.Request.Url.ToString();
                                //urlCompleta = urlCompleta.Replace(HttpContext.Current.Request.RawUrl, string.Concat("/", caminhoArquivoAshx));

                                hplnkCaminhoArquivo.Text = "Anexo";
                                hplnkCaminhoArquivo.NavigateUrl = caminhoArquivoAshx;
                            }
                            else
                            {
                                hplnkCaminhoArquivo.Visible = false;
                            }
                        }
                        catch
                        {
                            hplnkCaminhoArquivo.Visible = false;
                        }
                    }
                }
            }

        }

        protected void gvSolucoesEducacionais_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            var uId = ucLupaUsuario.SelectedUser != null ? (int?)ucLupaUsuario.SelectedUser.ID : null;

            PreencherSolucoesEducacionais(uId, e.NewPageIndex);
        }

        public void ucLupaUsuario_UserSelected(object sender, CompleteUserSelectionEvent e)
        {
            var usuario = ucLupaUsuario.SelectedUser;

            if (usuario != null)
                PreencherTabelas(usuario);
        }

        protected void btnLimparUsuario_Click(object sender, EventArgs e)
        {
            ucLupaUsuario.LimparCampos();
            PreencherTabelas();
        }

        private DateTime DataValidacao()
        {
            return DateTime.Now;
        }
    }

}