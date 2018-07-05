using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.BM.Views;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.BP;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.WebForms.UserControls;

namespace Sebrae.Academico.WebForms.Cadastros.Monitoramento
{
    public partial class MonitoramentoTrilha : PageBase
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

                ddlTopicoTematico.Items.Clear();

                WebFormHelper.PreencherLista(
                    new ManterTrilhaNivel().ObterPorTrilha(new ManterTrilha().ObterTrilhaPorId(idTrilha))
                        .OrderBy(x => x.Nome)
                        .ToList(), ddlTrilhaNivel, false, true);

                PreencherTabelas();
            }
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

        protected void ddlTrilhaNivel_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTrilha.SelectedIndex <= 0 || ddlTrilhaNivel.SelectedIndex <= 0) return;

            int idTrilha = int.Parse(ddlTrilha.SelectedValue), idTrilhaNivel = int.Parse(ddlTrilhaNivel.SelectedValue);

            var viewTrilhaFiltro = new ViewTrilha
            {
                TrilhaOrigem = idTrilha == 0 ? null : (new BMTrilha()).ObterPorId(idTrilha),
                TrilhaNivelOrigem = idTrilhaNivel == 0 ? null : (new BMTrilhaNivel()).ObterPorID(idTrilhaNivel),
            };

            var lstView =
                new BMViewTrilha().ObterViewTrilhaPorFiltro(viewTrilhaFiltro)
                    .OrderBy(x => x.TrilhaOrigem.ID)
                    .ThenBy(x => x.TrilhaNivelOrigem.ID)
                    .ThenBy(x => x.TopicoTematico.ID);

            var lstviewTopicos =
                lstView.Where(
                    x =>
                        x.TrilhaOrigem.ID == idTrilha && x.TrilhaNivelOrigem.ID == idTrilhaNivel &&
                        x.UsuarioOrigem == null).Select(x => x.TopicoTematico).Distinct();

            var listaTopicos =
                lstviewTopicos.Select(x => new TrilhaTopicoTematico { ID = x.ID, Nome = x.NomeExibicao });

            WebFormHelper.PreencherLista(listaTopicos.ToList(), ddlTopicoTematico, false, true);

            PreencherTabelas();
        }

        protected void ddlTopicoTematico_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            PreencherTabelas();
        }

        private void PreencherSolucoesEducacionais(int? idUsuario = null, int page = 0, int? idMonitor = null)
        {
            var lista = ObterListaItemTrilhaParticipacao(false, idMonitor, idUsuario);

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
                            x.UsuarioTrilha.Usuario.ID == idUsuario && x.ItemTrilha.Tipo.ID == (int)enumTipoItemTrilha.Discursiva);

            WebFormHelper.PaginarGrid(lista.ToList(), gvSolucoesEducacionais, page);
        }

        private IQueryable<ItemTrilhaParticipacao> ObterListaItemTrilhaParticipacao(bool sugerida, int? idMonitor,
            int? idUsuario = null)
        {
            // Filtar pelo Status.
            var aprovadas = cblStatus.Items[0].Selected;
            var emRevisao = cblStatus.Items[1].Selected;
            var pendentes = cblStatus.Items[2].Selected;
            var suspensas = cblStatus.Items[3].Selected;

            var lista = new ManterItemTrilhaParticipacao().ObterSolucoesRecentes(sugerida, aprovadas, emRevisao,
                pendentes, suspensas);

            if (lista.Any() && idMonitor.HasValue)
                lista =
                    lista.Where(
                        x =>
                            x.ItemTrilha.Missao.PontoSebrae.TrilhaNivel.Monitor != null &&
                            x.ItemTrilha.Missao.PontoSebrae.TrilhaNivel.Monitor.ID == idMonitor.Value);

            if (ddlTrilha.SelectedIndex > 0)
                lista =
                    lista.Where(
                        x => x.ItemTrilha.Missao.PontoSebrae.TrilhaNivel.Trilha.ID == int.Parse(ddlTrilha.SelectedValue));

            if (ddlTrilhaNivel.SelectedIndex > 0)
                lista =
                    lista.Where(
                        x => x.ItemTrilha.Missao.PontoSebrae.TrilhaNivel.ID == int.Parse(ddlTrilhaNivel.SelectedValue));

            if (ddlTopicoTematico.SelectedIndex > 0)
                lista =
                    lista.Where(x => x.ItemTrilha.TrilhaTopicoTematico.ID == int.Parse(ddlTopicoTematico.SelectedValue));

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

        private void MontarPainelSeAprovacao(int idItemTrilha)
        {
            LimparObjetosItemTrilha();

            _ItemTrilha = new BMItemTrilha().ObterPorID(idItemTrilha);

            if (_ItemTrilha == null) return;
            lblTipoItemTrilha.Text = _ItemTrilha.FormaAquisicao.Nome;
            lblTituloItemTrilha.Text = _ItemTrilha.Nome;
            lblOjetivoItemTrilha.Text = _ItemTrilha.Objetivo.NomeExibicao;
            lblLinkAcessoItemTrilha.Text = _ItemTrilha.LinkConteudo;
            lblLinkAcessoItemTrilha.NavigateUrl = _ItemTrilha.LinkConteudo;
            lblReferenciaBibliograficaItemTrilha.Text = _ItemTrilha.ReferenciaBibliografica;
            lblLocalItemTrilha.Text = _ItemTrilha.Local;
            txtObservacaoItemTrilha.Text = _ItemTrilha.Observacao;

            divCargaHoraria.Visible = false;

            if (_ItemTrilha.CargaHoraria > 0)
            {
                divCargaHoraria.Visible = true;
                txtCargHoraria.Text = _ItemTrilha.CargaHoraria.ToString();
            }
        }

        void AtualizarItemTrilhaAprovacao(enumStatusSolucaoEducacionalSugerida status)
        {
            var manterItemTrilha = (new ManterItemTrilha());
            var itemTrilha = manterItemTrilha.ObterItemTrilhaPorID(_ItemTrilha.ID);
            itemTrilha.Aprovado = status;
            itemTrilha.Observacao = txtObservacao.Text;
            manterItemTrilha.AlterarItemTrilha(itemTrilha);

            var idUsuario = !string.IsNullOrWhiteSpace(ucLupaUsuario.SelectedUserId) ? int.Parse(ucLupaUsuario.SelectedUserId) : 0;
            OcultarModaltemTrilhaAprovacao();
        }

        protected void btnAprovar_OnClick(object sender, EventArgs e)
        {
            try
            {
                var idItemTrilha = _ItemTrilha.ID;

                AtualizarItemTrilhaAprovacao(enumStatusSolucaoEducacionalSugerida.Aprovado);

                AtualizaCargaHoraria(idItemTrilha);

                LimparModalParaAprovacao();
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private void LimparModalParaAprovacao()
        {
            txtCargHoraria.Text = "";
            txtObservacaoItemTrilha.Text = "";
        }

        private void AtualizaCargaHoraria(int idItemTrilha)
        {
            _ItemTrilha = new BMItemTrilha().ObterPorID(idItemTrilha);

            if (!string.IsNullOrWhiteSpace(txtCargHoraria.Text) && _ItemTrilha.CargaHoraria >= 0 && txtCargHoraria.Text != _ItemTrilha.CargaHoraria.ToString())
            {
                int cargaHoraria;

                if (int.TryParse(txtCargHoraria.Text, out cargaHoraria))
                {
                    _ItemTrilha.CargaHoraria = int.Parse(txtCargHoraria.Text);

                    new ManterItemTrilha().AlterarItemTrilha(_ItemTrilha);

                    var manterNotificacao = new ManterNotificacao();

                    var nivel = _ItemTrilha.Missao.PontoSebrae.TrilhaNivel;

                    var link = string.Format("/trilhas/trilha/{0}/nivel/{1}",
                        nivel.Trilha.ID, nivel.ID);

                    var textoPublicacao = "Carga horária não aprovada.";
                    var usuarioLogado = new BMUsuario().ObterUsuarioLogado();

                    var notificacaoEnvio = new Sebrae.Academico.Dominio.Classes.NotificacaoEnvio
                    {
                        Texto = textoPublicacao,
                        Link = link
                    };

                    new ManterNotificacaoEnvio().AlterarNotificacaoEnvio(notificacaoEnvio);

                    manterNotificacao.PublicarNotificacao(link, textoPublicacao, usuarioLogado.ID, notificacaoEnvio);
                }
                else
                {
                    throw new AcademicoException("Carga horária informada é inválida.");
                }
            }
        }

        protected void btnReprovar_OnClick(object sender, EventArgs e)
        {
            AtualizarItemTrilhaAprovacao(enumStatusSolucaoEducacionalSugerida.NaoAprovado);
        }

        protected void OcultarItemTrilha_Click(object sender, EventArgs e)
        {
            OcultarModaltemTrilhaAprovacao();
        }

        private void LimparObjetosItemTrilha()
        {
            _ItemTrilha = null;
        }

        private void OcultarModaltemTrilhaAprovacao()
        {
            LimparObjetosItemTrilha();
            base.OcultarBackDrop();
            pnlItemTrilha.Visible = false;
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

                if (listaHistorico != null && listaHistorico.Count > 0) listaHistorico = listaHistorico.Where(x => x.ID != idParticipacao).ToList();
                var itemTrilha = (new BMItemTrilha()).ObterPorID(_ItemTrilhaParticipacao.ItemTrilha.ID);
                var se = itemTrilha.SolucaoEducacional;
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
                    lblLinkAcessoConteudo.NavigateUrl = lblLinkAcessoConteudo.Text = ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.EnderecoSGUS).Registro + "/ExibirFileServer.ashx?Identificador=" + _ItemTrilhaParticipacao.ItemTrilha.FileServer.NomeDoArquivoNoServidor;
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

                var nivel = _ItemTrilhaParticipacao.ItemTrilha.Missao.PontoSebrae.TrilhaNivel;

                txtUsuarioModal.Text = _ItemTrilhaParticipacao.UsuarioTrilha.Usuario.Nome;
                txtTrilhaModal.Text = nivel.Trilha.Nome;
                txtNivelModal.Text = nivel.Nome;
                txtTopicoTematicoModal.Text = _ItemTrilhaParticipacao.ItemTrilha.Missao.PontoSebrae.NomeExibicao;
                txtSolucaoEducacionalModal.Text = _ItemTrilhaParticipacao.ItemTrilha.Nome;
                txtParticipacaoModal.Text = _ItemTrilhaParticipacao.TextoParticipacao;
                txtObservacao.Text = string.Empty;
                txtOrientacao.Text = string.Empty;
                txtAutorizado.Text = _ItemTrilhaParticipacao.AutorizadoFormatado;
                rblAutorizadoModal.SelectedIndex = -1;
                divOrientacao.Visible = false;

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
                AlterarTrilhaAtividadeFormativaParticipacaoHistorico();
                this.OcultarModal();
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
                itemNovo.Orientacao = string.IsNullOrWhiteSpace(txtOrientacao.Text) ? null : txtOrientacao.Text;
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

                    MontarPainelSe(_ItemTrilhaParticipacao.ID);
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados alterados com sucesso");
                }
                catch
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Erro ao atualizar a participação do usuário");
                }
            }
        }

        private void AlterarTrilhaAtividadeFormativaParticipacaoHistorico()
        {
            if (_TrilhaAtividadeFormativaParticipacao != null)
            {
                TrilhaAtividadeFormativaParticipacao sprintNovo = new TrilhaAtividadeFormativaParticipacao();
                sprintNovo.UsuarioTrilha = _TrilhaAtividadeFormativaParticipacao.UsuarioTrilha;
                sprintNovo.TrilhaTopicoTematico = _TrilhaAtividadeFormativaParticipacao.TrilhaTopicoTematico;
                sprintNovo.Monitor = new BMUsuario().ObterUsuarioLogado();
                sprintNovo.TextoParticipacao = txtObservacao.Text;
                sprintNovo.TipoParticipacao = enumTipoParticipacaoTrilha.InteracaoMonitor;
                sprintNovo.Visualizado = false;
                sprintNovo.DataEnvio = DateTime.Now;


                //Anenxo
                if (fupldArquivoEnvio != null && fupldArquivoEnvio.PostedFile != null && fupldArquivoEnvio.PostedFile.ContentLength > 0)
                {
                    sprintNovo.FileServer = ObterAnexo();
                }
                else
                {
                    sprintNovo.FileServer = null;
                }

                try
                {
                    new ManterTrilhaAtividadeFormativaParticipacao().CadastrarHistorico(sprintNovo, chkEnviarEmail.Checked);
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

                    if (rbautorizado != _TrilhaAtividadeFormativaParticipacao.Autorizado)
                    {
                        AlterarStatusTrilhaAtividadeFormativaParticipacao(_TrilhaAtividadeFormativaParticipacao, rbautorizado);
                    }

                    MontarPainelSprint(_TrilhaAtividadeFormativaParticipacao.ID);
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados alterados com sucesso");
                }
                catch
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Erro ao atualizar a participação do usuário");
                }
            }
        }

        private void AlterarStatusTrilhaAtividadeFormativaParticipacao(TrilhaAtividadeFormativaParticipacao _TrilhaAtividadeFormativaParticipacao, bool? aprovado)
        {
            var trilhaAtividadeFormativaParticipacao = new BMTrilhaAtividadeFormativaParticipacao().ObterPorID(_TrilhaAtividadeFormativaParticipacao.ID);
            trilhaAtividadeFormativaParticipacao.Autorizado = aprovado;
            if (aprovado.HasValue)
            {
                trilhaAtividadeFormativaParticipacao.DataPrazoAvaliacao = null;
            }
            new BMTrilhaAtividadeFormativaParticipacao().Alterar(trilhaAtividadeFormativaParticipacao);
            new ManterTrilhaAtividadeFormativaParticipacao().GerarNotificacaoItemTrilha(trilhaAtividadeFormativaParticipacao);
        }

        private void AlterarStatusItemTrilhaParticipacao(Dominio.Classes.ItemTrilhaParticipacao _itemTrilhaParticipacao, bool? aprovado)
        {
            var itemTrilhaParticipacao = new BMItemTrilhaParticipacao().ObterPorId(_itemTrilhaParticipacao.ID);
            itemTrilhaParticipacao.Autorizado = aprovado;
            itemTrilhaParticipacao.ItemTrilha.Aprovado = aprovado == true ? enumStatusSolucaoEducacionalSugerida.Aprovado : enumStatusSolucaoEducacionalSugerida.Aprovado;

            if (aprovado.HasValue)
            {
                itemTrilhaParticipacao.DataPrazoAvaliacao = null;
            }

            new BMItemTrilhaParticipacao().Salvar(itemTrilhaParticipacao);
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



        protected void gvSprints_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {

            }
        }

        protected void gvSprints_RowCommand(object sender, GridViewCommandEventArgs e)

        {
            if (e.CommandName != "Page")
            {
                int idParticipacao = int.Parse(e.CommandArgument.ToString());
                MontarPainelSprint(idParticipacao);
                base.ExibirBackDrop();
                pnlTrilhaParticipacao.Visible = true;
            }
        }

        private void MontarPainelSprint(int idParticipacao)
        {
            LimparObjetos();

            _TrilhaAtividadeFormativaParticipacao = new BMTrilhaAtividadeFormativaParticipacao().ObterPorID(idParticipacao);

            if (_TrilhaAtividadeFormativaParticipacao != null)
            {
                dvSolucaoEducacionalModal.Visible = false;


                List<TrilhaAtividadeFormativaParticipacao> listaHistorico = new BMTrilhaAtividadeFormativaParticipacao().ObterParticipacoesUsuarioTrilha(_TrilhaAtividadeFormativaParticipacao.TrilhaTopicoTematico.ID, _TrilhaAtividadeFormativaParticipacao.UsuarioTrilha.ID);

                if (listaHistorico != null && listaHistorico.Count > 0)
                    listaHistorico = listaHistorico.Where(x => x.ID != idParticipacao).ToList();

                lblOrientacaoParticipacao.Text = _TrilhaAtividadeFormativaParticipacao.TrilhaTopicoTematico.DescricaoTextoEnvio;

                divObjetivoServer.Visible = false;
                divFormaAquisicao.Visible = false;
                divReferenca.Visible = false;
                divLinkConteudo.Visible = false;
                divLinkAcesso.Visible = false;

                txtUsuarioModal.Text = _TrilhaAtividadeFormativaParticipacao.UsuarioTrilha.Usuario.Nome;
                txtTrilhaModal.Text = _TrilhaAtividadeFormativaParticipacao.UsuarioTrilha.TrilhaNivel.Trilha.Nome;
                txtNivelModal.Text = _TrilhaAtividadeFormativaParticipacao.UsuarioTrilha.TrilhaNivel.Nome;
                txtTopicoTematicoModal.Text = _TrilhaAtividadeFormativaParticipacao.TrilhaTopicoTematico.NomeExibicao;
                txtParticipacaoModal.Text = _TrilhaAtividadeFormativaParticipacao.TextoParticipacao;
                txtAutorizado.Text = _TrilhaAtividadeFormativaParticipacao.AutorizadoFormatado;
                rblAutorizadoModal.SelectedIndex = -1;
                txtObservacao.Text = string.Empty;

                WebFormHelper.PreencherGrid(listaHistorico.OrderByDescending(x => x.DataAlteracao).ToList(), gvHistoricoParticipacaoModal);
                hplnkCaminhoArquivo.Visible = false;
                if (_TrilhaAtividadeFormativaParticipacao.FileServer != null && _TrilhaAtividadeFormativaParticipacao.FileServer.ID > 0)
                {
                    MontarLinkDownload(_TrilhaAtividadeFormativaParticipacao.FileServer);
                }
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

        protected Dominio.Classes.ItemTrilha _ItemTrilha
        {
            get
            {
                if (Session["_ItemTrilha"] != null)
                {
                    return (Dominio.Classes.ItemTrilha)Session["_ItemTrilha"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Session["_ItemTrilha"] = value;
            }
        }

        protected Dominio.Classes.ItemTrilhaParticipacao _ItemTrilhaParticipacao
        {
            get
            {
                if (Session["_ItemTrilhaParticipaca"] != null)
                {
                    return (Dominio.Classes.ItemTrilhaParticipacao)Session["_ItemTrilhaParticipaca"];
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
                else
                {
                    return null;
                }
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

        protected void rblAutorizadoModal_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblAutorizadoModal.SelectedValue == "N")
            {
                divOrientacao.Visible = true;
            }
            else
            {
                txtOrientacao.Text = "";
            }
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