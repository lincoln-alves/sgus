using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using classes = Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.WebForms.UserControls;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.WebForms.Cadastros.NotificacaoEnvio
{
    public partial class EdicaoNotificacaoEnvio : Page
    {
        private classes.NotificacaoEnvio _notificacaoEnvioEdicao;
        private ManterNotificacaoEnvio manterNotificacaoEnvio = new ManterNotificacaoEnvio();

        private int totalGerado
        {
            get
            {
                if (ViewState["totalGerado"] != null)
                {
                    return (int)ViewState["totalGerado"];
                }

                return 0;
            }
            set
            {
                ViewState["totalGerado"] = value;
            }
        }

        private int? idNotificacaoGerada
        {
            get
            {
                if (Session["NotificacaoEnvio"] != null)
                {
                    return (int)Session["NotificacaoEnvio"];
                }

                return null;
            }
            set
            {
                Session["NotificacaoEnvio"] = value;
            }
        }

        public void MostrarTab(HtmlAnchor link, HtmlGenericControl controle)
        {
            link.Attributes.Remove("class");
            controle.Attributes.Remove("class");
            controle.Attributes.Add("class", "panel-collapse in");
        }

        public void EsconderTab(HtmlAnchor link, HtmlGenericControl controle)
        {
            link.Attributes.Add("class", "collapsed");
            controle.Attributes.Remove("class");
            controle.Attributes.Add("class", "panel-collapse collapse");
        }

        protected void ddlSolucaoEducacional_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                //MostrarTab(this.lkbAlunos, this.collapseSelecionados);
                if (ddlSolucaoEducacional.SelectedIndex > 0)
                {
                    this.ObterOferta(int.Parse(ddlSolucaoEducacional.SelectedValue));
                }
            }
        }

        protected void ddlOferta_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                //MostrarTab(this.lkbAlunos, this.collapseSelecionados);
                if (ddlOferta.SelectedIndex > 0)
                {
                    this.ObterTurma(int.Parse(ddlOferta.SelectedValue));
                }
            }
        }

        private void ObterSolucaoEducacional()
        {
            var manterSolucaoEducacional = new ManterSolucaoEducacional();
            var listaSolucaoEducacional = manterSolucaoEducacional.ObterTodosSolucaoEducacional().ToList();
            listaSolucaoEducacional.Insert(0, new classes.SolucaoEducacional { Nome = "::Selecione::" });

            ddlSolucaoEducacional.DataSource = listaSolucaoEducacional;
            ddlSolucaoEducacional.DataValueField = "ID";
            ddlSolucaoEducacional.DataTextField = "Nome";
            ddlSolucaoEducacional.DataBind();
        }

        private void ObterOferta(int solucaoEducacionalId)
        {
            var manterSolucaoEducacional = new ManterOferta();

            var listaSolucaoEducacional = manterSolucaoEducacional.ObterOfertaPorSolucaoEducacional(new classes.SolucaoEducacional { ID = solucaoEducacionalId }).ToList();

            listaSolucaoEducacional.Insert(0, new classes.Oferta { Nome = "::Selecione::" });

            ddlOferta.DataSource = listaSolucaoEducacional;
            ddlOferta.DataValueField = "ID";
            ddlOferta.DataTextField = "Nome";
            ddlOferta.DataBind();

            // Na troca de solução educacional caso tenha itens no combo de turma zera a sua listagem
            if (ddlTurma.Items.Count > 0)
            {

                IList<classes.Turma> ListaTurma = new List<classes.Turma>();
                ListaTurma.Insert(0, new classes.Turma { Nome = "::Selecione::" });

                ddlTurma.DataSource = ListaTurma;
                ddlTurma.DataValueField = "ID";
                ddlTurma.DataTextField = "Nome";
                ddlTurma.DataBind();
            }
        }

        private void ObterTurma(int oferta)
        {
            var manterTurma = new ManterTurma();
            var lista = manterTurma.ObterTurmasPorOferta(oferta).ToList();
            lista.Insert(0, new classes.Turma { Nome = "::Selecione::" });

            ddlTurma.DataSource = lista;
            ddlTurma.DataValueField = "ID";
            ddlTurma.DataTextField = "Nome";
            ddlTurma.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                lkbAlunos.Attributes.Add("class", "collapsed");

                PreencherCombos();

                if (Request["Id"] != null)
                {
                    var idNotificacaoEnvio = int.Parse(Request["Id"]);

                    idNotificacaoGerada = idNotificacaoEnvio;

                    _notificacaoEnvioEdicao = manterNotificacaoEnvio.ObterNotificacaoEnvioPorID(idNotificacaoEnvio);

                    if (_notificacaoEnvioEdicao != null)
                        btnEnviarNotificacao.Enabled = true;
                }

                // Preenche o Status
                var listaStatus = Enum.GetValues(typeof(enumStatusMatricula)).Cast<enumStatusMatricula>().Select(x => new { nome = x.GetDescription(), valor = (int)x });
                WebFormHelper.PreencherListaCustomizado(listaStatus.ToList(), chkStatus, "valor", "nome");

                PreencherCampos(_notificacaoEnvioEdicao);

                // Preenche o combo de Soluções Educacionais
                ObterSolucaoEducacional();

                if (_notificacaoEnvioEdicao != null)
                {
                    var notPerm = _notificacaoEnvioEdicao.ListaPermissao.FirstOrDefault(x => x.Turma != null);

                    if (notPerm != null)
                    {
                        var solId = notPerm.Turma.Oferta.SolucaoEducacional.ID;
                        var offerId = notPerm.Turma.Oferta.ID;

                        ddlSolucaoEducacional.SelectedValue = solId.ToString();

                        // Preenche o combo de Oferta
                        ObterOferta(solId);
                        ddlOferta.SelectedValue = offerId.ToString();

                        // Preenche o combo de turmas
                        ObterTurma(offerId);
                        ddlTurma.SelectedValue = notPerm.Turma.ID.ToString();
                    }
                }
            }
            else
            {
                MostrarTab(lkbAlunos, collapseSelecionados);
            }
        }

        private void PreencherCombos()
        {
            try
            {
                this.ucPermissoes1.PreencherListas();
                //this.ucPermissoes1.SelecionarUFGestor(new BMUsuario().ObterUsuarioLogado().UF.ID);
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private void PreencherCampos(classes.NotificacaoEnvio notificacaoEnvio)
        {
            if (notificacaoEnvio != null)
            {
                txtTexto.Text = notificacaoEnvio.Texto;

                txtLink.Text = notificacaoEnvio.Link;

                PreencherListas(notificacaoEnvio);
            }
        }

        private void PreencherListas(classes.NotificacaoEnvio notificacaoEnvio)
        {
            PreencherListaUfs(notificacaoEnvio);
            PreencherListaNivelOcupacional(notificacaoEnvio);
            PreencherListaPerfil(notificacaoEnvio);
            PreencherListaUsuarios(notificacaoEnvio);
            PreencherListaStatus(notificacaoEnvio);
        }

        private void PreencherListaUsuarios(classes.NotificacaoEnvio notificacaoEnvio)
        {

            var usuarios =
                notificacaoEnvio.ListaPermissao.Where(x => x.Usuario != null)
                    .Select(x => new Usuario { ID = x.Usuario.ID, Nome = x.Usuario.Nome, CPF = x.Usuario.CPF })
                    .ToList();

            ucLupaMultiplosUsuarios.PreencherGridUsuarios(usuarios);
        }

        private void PreencherListaUfs(classes.NotificacaoEnvio notificacaoEnvio)
        {
            var permissoesUfIds = notificacaoEnvio.ListaPermissao.Where(x => x.Uf != null).Select(x => x.Uf.ID).ToList();

            ucPermissoes1.PreencherUfs(new ManterUf().ObterTodosUf().Select(x => new DTOUf
            {
                ID = x.ID,
                Nome = x.Sigla,
                IsSelecionado = permissoesUfIds.Contains(x.ID)
            }).ToList(), false);
        }

        private void PreencherListaStatus(classes.NotificacaoEnvio notificacaoEnvio)
        {
            var permissoesStatus = notificacaoEnvio.ListaPermissao.Where(x => x.Status != null);

            foreach (ListItem item in chkStatus.Items.Cast<ListItem>())
            {
                item.Selected = permissoesStatus.Any(x => x.Status.ID == int.Parse(item.Value));
            }
        }

        private void PreencherListaNivelOcupacional(classes.NotificacaoEnvio notificacaoEnvio)
        {
            var permissoesNiveisIds =
                notificacaoEnvio.ListaPermissao.Where(x => x.NivelOcupacional != null)
                    .Select(x => x.NivelOcupacional.ID)
                    .ToList();

            ucPermissoes1.PreencherNiveisOcupacionais(
                new ManterNivelOcupacional().ObterTodosNivelOcupacional().Select(x => new DTONivelOcupacional
                {
                    ID = x.ID,
                    Nome = x.Nome,
                    IsSelecionado = permissoesNiveisIds.Contains(x.ID)
                }).ToList());
        }

        private void PreencherListaPerfil(classes.NotificacaoEnvio notificacaoEnvio)
        {
            var permissoesPerfisIds =
                notificacaoEnvio.ListaPermissao.Where(x => x.Perfil != null).Select(x => x.Perfil.ID).ToList();

            ucPermissoes1.PreencherPerfis(
                new ManterPerfil().ObterTodosPerfis().Select(x => new DTOPerfil
                {
                    ID = x.ID,
                    Nome = x.Nome,
                    IsSelecionado = permissoesPerfisIds.Contains(x.ID)
                }).ToList());
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    _notificacaoEnvioEdicao = new classes.NotificacaoEnvio();

                    if (Request["Id"] == null)
                    {
                        _notificacaoEnvioEdicao = ObterObjetoNotificacaoEnvio();
                        manterNotificacaoEnvio.IncluirNotificacaoEnvio(_notificacaoEnvioEdicao);
                    }
                    else
                    {
                        _notificacaoEnvioEdicao = ObterObjetoNotificacaoEnvio();
                        manterNotificacaoEnvio.AlterarNotificacaoEnvio(_notificacaoEnvioEdicao);
                    }

                    idNotificacaoGerada = _notificacaoEnvioEdicao.ID;

                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados Gravados com Sucesso!", "EdicaoNotificacaoEnvio.aspx?Id=" + _notificacaoEnvioEdicao.ID + "&acao=Salvar");

                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
                catch (AlertException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Atencao, ex.Message);
                }
            }
        }

        private classes.NotificacaoEnvio ObterObjetoNotificacaoEnvio()
        {
            if (Request["Id"] != null)
            {
                _notificacaoEnvioEdicao = new ManterNotificacaoEnvio().ObterNotificacaoEnvioPorID((int.Parse(Request["Id"])));
            }
            else
            {
                _notificacaoEnvioEdicao = new classes.NotificacaoEnvio();
            }

            //Texto
            _notificacaoEnvioEdicao.Texto = txtTexto.Text.Trim();

            //Link
            _notificacaoEnvioEdicao.Link = txtLink.Text.Trim();

            AdicionarPermissao(_notificacaoEnvioEdicao);

            if (_notificacaoEnvioEdicao.Uf == null)
            {
                using (var bmusuario = new ManterUsuario())
                {
                    var usuario = bmusuario.ObterUsuarioLogado();
                    _notificacaoEnvioEdicao.Uf = new ManterUf().ObterUfPorID(usuario.UF.ID);
                }
            }

            return _notificacaoEnvioEdicao;
        }

        private void AdicionarPermissao(classes.NotificacaoEnvio notificacaoEnvioEdicao)
        {
            AdicionarOuRemoverPerfil(notificacaoEnvioEdicao);
            AdicionarOuRemoverUf(notificacaoEnvioEdicao);
            AdicionarOuRemoverNivelOcupacional(notificacaoEnvioEdicao);
            AdicionarOuRemoverTurma(notificacaoEnvioEdicao);
            AdicionarOuRemoverAlunos(notificacaoEnvioEdicao);
            AdicionarOuRemoverStatus(notificacaoEnvioEdicao);
        }

        private void AdicionarOuRemoverAlunos(classes.NotificacaoEnvio notificacaoEnvioEdicao)
        {

            IList<NotificacaoEnvioPermissao> listaNot = notificacaoEnvioEdicao.ListaPermissao.Where(x => x.Usuario != null).ToList();

            // Remove todos do Objeto sempre e aloca somente os selecionados no grid view
            notificacaoEnvioEdicao.RemoverUsuarios();

            GridView gridViewUsuarios = (GridView)this.ucLupaMultiplosUsuarios.FindControl("GridViewUsuariosSelecionados");

            if (gridViewUsuarios.Rows.Count > 0)
            {
                BMUsuario bmUsu = new BMUsuario();
                Usuario usuarioLogado = bmUsu.ObterUsuarioLogado();

                foreach (GridViewRow row in gridViewUsuarios.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chkRow = (row.Cells[0].FindControl("chkRow") as CheckBox);

                        // Se estiver selecionado
                        if (chkRow.Checked)
                        {
                            ManterUsuario mantUsuario = new ManterUsuario();
                            classes.Usuario user = mantUsuario.ObterPorCPF(row.Cells[2].Text);

                            notificacaoEnvioEdicao.AdicionarUsuario(user, usuarioLogado);
                        }
                    }
                }
            }

        }


        private void AdicionarOuRemoverTurma(classes.NotificacaoEnvio notificacaoEnvioEdicao)
        {
            IList<NotificacaoEnvioPermissao> listaNot = notificacaoEnvioEdicao.ListaPermissao.Where(x => x.Turma != null).ToList();

            // Já tinha alguma turma escolhida           
            if (ddlTurma.SelectedItem != null && ddlTurma.SelectedItem.Value != null && ddlTurma.SelectedItem.Value != "0")
            {

                // Se a seleção não conter o valor escolhido, se já estiver não precisa atualizar nada
                if (!listaNot.Any(x => x.Turma.ID.Equals(int.Parse(ddlTurma.SelectedItem.Value))))
                {

                    BMUsuario bmUsu = new BMUsuario();
                    Usuario usuarioLogado = bmUsu.ObterUsuarioLogado();

                    //classes.Turma tur = new classes.Turma();
                    //tur = new ManterTurma().ObterTurmaPorID(ddlTurma.SelectedItem.Value);
                    ManterTurma mantTurma = new ManterTurma();
                    classes.Turma tur = mantTurma.ObterTurmaPorID(Convert.ToInt32(ddlTurma.SelectedItem.Value));

                    notificacaoEnvioEdicao.AdicionarTurma(tur, usuarioLogado);
                    notificacaoEnvioEdicao.RemoverTurmasExceto(tur);

                }

            }// Se não selecinou nada mas tem algo no banco limpa
            else if (listaNot.Count() > 0)
            {
                notificacaoEnvioEdicao.RemoverTurmas();
            }

        }

        private void AdicionarOuRemoverStatus(classes.NotificacaoEnvio notificacaoEnvioEdicao)
        {
            var statusSelecionados = WebFormHelper.ObterValoresSelecionadosCheckBoxList(chkStatus);

            notificacaoEnvioEdicao.RemoverStatus();

            if (statusSelecionados.Any())
            {

                var bmUsu = new BMUsuario();
                var usuarioLogado = bmUsu.ObterUsuarioLogado();

                var manterStatusMatricula = new ManterStatusMatricula();
                var listaStatusMatricula = manterStatusMatricula.ObterTodosStatusMatricula();
                var listaStatusMatriculaSelecionadas = listaStatusMatricula.Where(x => statusSelecionados.Contains(x.ID));

                notificacaoEnvioEdicao.AdicionarStatus(listaStatusMatriculaSelecionadas, usuarioLogado);

            }
        }

        private void AdicionarOuRemoverNivelOcupacional(classes.NotificacaoEnvio solucaoEducacionalEdicao)
        {
            var niveisDto = ucPermissoes1.ObterNiveis();

            foreach (var nivelDto in niveisDto)
            {
                var nivel = new NivelOcupacional { ID = nivelDto.ID };

                if (nivelDto.IsSelecionado)
                    solucaoEducacionalEdicao.AdicionarNivelOcupacional(nivel);
                else
                    solucaoEducacionalEdicao.RemoverNivelOcupacional(nivel);
            }
        }

        private void AdicionarOuRemoverUf(classes.NotificacaoEnvio notificacaoEnvio)
        {
            try
            {
                var ufs = ucPermissoes1.ObterUfs();
                var usuarioLogado = new BMUsuario().ObterUsuarioLogado();

                foreach (var ufDto in ufs)
                {
                    var uf = new Uf { ID = ufDto.ID };

                    if (ufDto.IsSelecionado)
                        notificacaoEnvio.AdicionarUfs(uf, usuarioLogado);
                    else
                        notificacaoEnvio.RemoverUfs(uf);
                }
            }
            catch
            {
                throw new ExecutionEngineException("Erro ao salvar UFs das permissões.");
            }
        }


        private void AdicionarOuRemoverPerfil(classes.NotificacaoEnvio notificacaoEnvioEdicao)
        {
            var todosPerfis = this.ucPermissoes1.ObterTodosPerfis;

            if (todosPerfis != null && todosPerfis.Count > 0)
            {
                for (int i = 0; i < todosPerfis.Count; i++)
                {
                    var perfilSelecionado = new Perfil()
                    {
                        ID = int.Parse(todosPerfis[i].Value),
                        Nome = todosPerfis[i].Text
                    };

                    if (todosPerfis[i].Selected)
                    {
                        notificacaoEnvioEdicao.AdicionarPerfil(perfilSelecionado);
                    }
                    else
                    {
                        notificacaoEnvioEdicao.RemoverPerfil(perfilSelecionado);
                    }
                }
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //“Registro salvo para envio posterior”.
            if (Request["acao"] == "Salvar")
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Informacao, "Registro salvo para envio posterior", "ListarNotificacaoEnvio.aspx");
            }


            Response.Redirect("ListarNotificacaoEnvio.aspx");
        }

        protected void UserSelectedHandler(object sender, CompleteUserSelectionEvent e)
        {
            //if (LupaUsuario.SelectedUser != null)
            //{
            //    Usuario usuario = LupaUsuario.SelectedUser;
            //    this.IdUsuario = LupaUsuario.SelectedUser.ID;
            //    IList<LogAcesso> LogsDeAcesso = new ManterLogAcesso().ObterUltimosAcessoDosUsuario(usuario.ID);
            //    WebFormHelper.PreencherGrid(LogsDeAcesso, this.dgvLogAcessosDoUsuario);
            //    //this.PreencherPainelComInformacoesDoUsuario(usuario);
            //    this.IdUsuario = usuario.ID;
            //    this.CPFUsuario = usuario.CPF;
            //    //this.HabilitarBotaoNotificacoes();
            //    pnlGerenciador.Visible = true;
            //    this.ExibirPaineis();
            //    this.ExibirPanelDadosPessoais(usuario);
            //    this.btnHistorico.Visible = true;
            //}
            //else
            //{
            //    this.btnHistorico.Visible = false;
            //}
        }

        private void ExibirModal()
        {
            pnlModal.Visible = true;
        }

        private void OcultarModal()
        {
            pnlModal.Visible = false;
        }

        protected void OcultarModal_Click(object sender, EventArgs e)
        {
            OcultarModal();
        }

        protected void btnSim_Click(object sender, EventArgs e)
        {
             if (totalGerado == 0)
            {
                OcultarModal();
                return;
            }

            if (Request["Id"] != null)
            {
                OcultarModal();

                var idNotificacaoEnvio = int.Parse(Request["Id"]);

                _notificacaoEnvioEdicao = manterNotificacaoEnvio.ObterNotificacaoEnvioPorID(idNotificacaoEnvio);

                var manterNotificacao = new ManterNotificacao();
                try
                {
                    try
                    {
                        var usuarios = manterNotificacaoEnvio.CompilarUsuarios(_notificacaoEnvioEdicao);

                        foreach (var usuario in usuarios)
                        {
                            manterNotificacao.PublicarNotificacao(_notificacaoEnvioEdicao.Link, _notificacaoEnvioEdicao.Texto, usuario.ID, _notificacaoEnvioEdicao);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Notificação realizada com Sucesso!",
                        "ListarNotificacaoEnvio.aspx");
                }
                catch (Exception ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
            }
        }

        protected IEnumerable<NotificacaoEnvioPermissao> FiltrarPermissoes(IEnumerable<NotificacaoEnvioPermissao> permissoes)
        {
            return permissoes.Where(x => x.NivelOcupacional != null || x.Perfil != null
                                         || x.Uf != null || x.Usuario != null);
        }

        protected void btnEnviarNotificacao_Click(object sender, EventArgs e)
        {
            if (Request["Id"] != null)
            {
                var notificaoId = int.Parse(Request["Id"]);

                _notificacaoEnvioEdicao = manterNotificacaoEnvio.ObterNotificacaoEnvioPorID(notificaoId);

                var usuarios = manterNotificacaoEnvio.CompilarUsuarios(_notificacaoEnvioEdicao);

                totalGerado = usuarios.Count();

                AlterarMensagemModal(totalGerado);
            }

            rptFiltros.DataSource = _notificacaoEnvioEdicao.ListaPermissao;
            rptFiltros.DataBind();

            ExibirModal();
        }

        protected void AlterarMensagemModal(int total)
        {
            if (total == 0)
            {
                pMensagemTotal.InnerHtml =
                    "<strong>Dentro dos parâmetros utilizados, nenhum usuário receberá a mensagem. Deseja modificar?</strong>";
            }
            else
            {
                pMensagemTotal.InnerHtml =
                    "<strong>Este E-mail será enviado para <span ID='spnTotal'>" + total.ToString() + "</span> usuário(s). Confirma o envio?</strong>";
            }
        }

        protected void btnRemoverFiltro_Click(object sender, EventArgs e)
        {
            var btnRemover = (HtmlButton)sender;

            int id;

            if (int.TryParse(btnRemover.Attributes["data-id"], out id) && idNotificacaoGerada != null)
            {
                classes.NotificacaoEnvio notificacaoEnvio;

                using (var manter = new BMNotificacaoEnvio())
                {
                    notificacaoEnvio = manter.ObterPorID(idNotificacaoGerada.Value);

                    //// Busca por id novamente para ter o objeto novamente na sessão do nhibernate
                    notificacaoEnvio = manter.ObterPorID(notificacaoEnvio.ID);

                    var permissao = new ManterNotificacaoEnvioPermissao().ObterPorID(id);
                    notificacaoEnvio.ListaPermissao.Remove(permissao);

                    manter.Salvar(notificacaoEnvio);
                }

                PreencherCampos(notificacaoEnvio);

                rptFiltros.DataSource = FiltrarPermissoes(notificacaoEnvio.ListaPermissao); ;
                rptFiltros.DataBind();

                if (notificacaoEnvio.ListaPermissao.Count <= 0)
                {
                    OcultarModal();
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Você deve selecionar pelo menos um filtro");
                    return;
                }

                var total = manterNotificacaoEnvio.CompilarUsuarios(notificacaoEnvio).Count();

                AlterarMensagemModal(total);
            }
        }
    }
}