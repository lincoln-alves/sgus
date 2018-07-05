using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.WebForms.UserControls;
using classes = Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Util.Classes;
using System.Data;

namespace Sebrae.Academico.WebForms.Cadastros.EmailEnvio
{
    public partial class EdicaoEmailEnvio : Page
    {
        #region Atributos
        private classes.EmailEnvio _emailEnvioEdicao;
        private ManterEmailEnvio _manterEmailEnvio = new ManterEmailEnvio();

        private int? idEmailGerado
        {
            get
            {
                if (Session["EmailEnvio"] != null)
                {
                    return (int)Session["EmailEnvio"];
                }

                return null;
            }
            set
            {
                Session["EmailEnvio"] = value;
            }
        }

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
        #endregion

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

        protected void txtSolucaoEducacional_OnTextChanged(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                var idSe = string.IsNullOrWhiteSpace(txtSolucaoEducacional.Text) ? 0 : int.Parse(txtSolucaoEducacional.Text);

                ObterOferta(idSe);
            }
        }

        protected void txtOferta_OnTextChanged(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                var idOferta = string.IsNullOrWhiteSpace(txtOferta.Text) ? 0 : int.Parse(txtOferta.Text);

                ObterTurma(idOferta);
            }
        }

        private void ObterSolucaoEducacional()
        {
            ViewState["_SE"] = Helpers.Util.ObterListaAutocomplete(new ManterSolucaoEducacional().ObterTodosPorGestor());
        }

        private void ObterOferta(int solucaoEducacionalId)
        {
            var ofertas = new ManterOferta().ObterOfertasPorGestor(solucaoEducacionalId);

            txtOferta.Text = "";
            ViewState["_Oferta"] = solucaoEducacionalId == 0 ? null : Helpers.Util.ObterListaAutocomplete(ofertas);

            txtTurma.Text = "";
            ViewState["_Turma"] = null;
        }

        private void ObterTurma(int oferta)
        {
            var lista = new ManterTurma().ObterTurmasPorOferta(oferta);

            ViewState["_Turma"] = Helpers.Util.ObterListaAutocomplete(lista);
            txtTurma.Text = "";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                PreencherCombos();

                if (Request["Id"] != null)
                {
                    var idEmailEnvio = int.Parse(Request["Id"]);

                    idEmailGerado = idEmailEnvio;

                    _emailEnvioEdicao = _manterEmailEnvio.ObterEmailEnvioPorID(idEmailEnvio);

                    if (_emailEnvioEdicao != null && _emailEnvioEdicao.ListaEmailsGerados.Any())
                        btnEnviarEmail.Enabled = true;
                }

                // Preenche o Status
                var listaStatus =
                    Enum.GetValues(typeof(enumStatusMatricula))
                        .Cast<enumStatusMatricula>()
                        .Select(x => new { nome = x.GetDescription(), valor = (int)x });

                WebFormHelper.PreencherListaCustomizado(listaStatus.ToList(), chkStatus, "valor", "nome");

                PreencherCampos(_emailEnvioEdicao);

                // Preenche o combo de Soluções Educacionais
                ObterSolucaoEducacional();

                if (_emailEnvioEdicao != null)
                {
                    var notPerm = _emailEnvioEdicao.ListaPermissao.FirstOrDefault(x => x.Turma != null);

                    if (notPerm != null)
                    {
                        var solId = notPerm.Turma.Oferta.SolucaoEducacional.ID;
                        var offerId = notPerm.Turma.Oferta.ID;

                        txtSolucaoEducacional.Text = solId.ToString();

                        // Preenche o combo de Oferta
                        ObterOferta(solId);
                        txtOferta.Text = offerId.ToString();

                        // Preenche o combo de turmas
                        ObterTurma(offerId);
                        txtTurma.Text = notPerm.Turma.ID.ToString();
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
                ucPermissoes1.PreencherListas();
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private void PreencherCampos(classes.EmailEnvio emailEnvio)
        {
            if (emailEnvio != null)
            {
                txtMensagem.Text = emailEnvio.Texto;
                txtAssunto.Text = emailEnvio.Assunto;
                PreencherListas(emailEnvio);
            }
        }

        private void PreencherListas(classes.EmailEnvio emailEnvio)
        {
            PreencherListaUfs(emailEnvio);
            PreencherListaNivelOcupacional(emailEnvio);
            PreencherListaPerfil(emailEnvio);
            PreencherListaUsuarios(emailEnvio);
            PreencherListaStatuss(emailEnvio);
        }

        private void PreencherListaUsuarios(classes.EmailEnvio emailEnvio)
        {

            IList<Usuario> ListaUsuarios = emailEnvio.ListaPermissao.Where(x => x.Usuario != null).Select(x => new Usuario() { ID = x.Usuario.ID, Nome = x.Usuario.Nome, CPF = x.Usuario.CPF }).ToList<Usuario>();

            this.ucLupaMultiplosUsuarios.PreencherGridUsuarios(ListaUsuarios);
        }

        private void PreencherListaStatuss(classes.EmailEnvio emailEnvio)
        {
            var permissoesStatus = emailEnvio.ListaPermissao.Where(x => x.Status != null);

            foreach (ListItem item in chkStatus.Items.Cast<ListItem>())
            {
                item.Selected = permissoesStatus.Any(x => x.Status.ID == int.Parse(item.Value));
            }
        }

        private void PreencherListaUfs(classes.EmailEnvio emailEnvio)
        {
            var listaUfs = emailEnvio.ListaPermissao.Where(x => x.Uf != null)
                      .Select(x => x.Uf.ID).ToList();

            this.ucPermissoes1.SelecionarUfs(listaUfs, false);
        }

        private void PreencherListaNivelOcupacional(classes.EmailEnvio solucaoEducacional)
        {
            IList<NivelOcupacional> ListaNivelOcupacional = solucaoEducacional.ListaPermissao.Where(x => x.NivelOcupacional != null)
                    .Select(x => new NivelOcupacional() { ID = x.NivelOcupacional.ID, Nome = x.NivelOcupacional.Nome }).ToList<NivelOcupacional>();

            this.ucPermissoes1.PreencherListBoxComNiveisOcupacionaisGravadosNoBanco(ListaNivelOcupacional);

        }

        private void PreencherListaPerfil(classes.EmailEnvio solucaoEducacional)
        {
            IList<classes.Perfil> ListaPerfil = solucaoEducacional.ListaPermissao.Where(x => x.Perfil != null)
                    .Select(x => new classes.Perfil() { ID = x.Perfil.ID, Nome = x.Perfil.Nome }).ToList<classes.Perfil>();

            bool temPerfilPublico = false;

            if (ListaPerfil.Count == 0)
            {
                temPerfilPublico = solucaoEducacional.ListaPermissao.Where(x => x.Perfil == null &&
                    x.NivelOcupacional == null && x.Uf == null).Any();
            }

            this.ucPermissoes1.PreencherListBoxComPerfisGravadosNoBanco(ListaPerfil, temPerfilPublico);
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    _emailEnvioEdicao = new classes.EmailEnvio();

                    if (Request["Id"] == null)
                    {
                        _manterEmailEnvio = new ManterEmailEnvio();
                        _emailEnvioEdicao = ObterObjetoEmailEnvio();
                        _emailEnvioEdicao.Processado = false;
                        _emailEnvioEdicao.DataGeracao = DateTime.Now;
                        _manterEmailEnvio.IncluirEmailEnvio(_emailEnvioEdicao);
                    }
                    else
                    {
                        _emailEnvioEdicao = ObterObjetoEmailEnvio();
                        _manterEmailEnvio.AlterarEmailEnvio(_emailEnvioEdicao, true);
                    }

                    GerarEmails(_emailEnvioEdicao);

                    // Usado para enviar os emails
                    idEmailGerado = _emailEnvioEdicao.ID;

                    //Session.Remove("SolucaoEducacionalEdit");
                    Session.Remove("userSelected");

                    esconderFiltros(_emailEnvioEdicao);

                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados Gravados com Sucesso !");
                    btnEnviarEmail.Enabled = true;
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

        protected void GerarEmails(classes.EmailEnvio emailEnvio)
        {
            var ckblstPerfil = (CheckBoxList) this.ucPermissoes1.FindControl("ckblstPerfil");
            var perfisSelecionados = WebFormHelper.ObterValoresSelecionadosCheckBoxList(ckblstPerfil);

            var ckblstNivelOcupacional = (CheckBoxList) this.ucPermissoes1.FindControl("ckblstNivelOcupacional");
            var niveisOcupacionaisSelecionados =
                WebFormHelper.ObterValoresSelecionadosCheckBoxList(ckblstNivelOcupacional);

            var rptUFs = (Repeater) ucPermissoes1.FindControl("rptUFs");
            var ufsSelecionadas = WebFormHelper.ObterValoresSelecionadosRepeaterCheckBox(rptUFs, "ckUf", "ID_UF");

            var statusSelecionados = WebFormHelper.ObterValoresSelecionadosCheckBoxList(chkStatus);

            var selectedUsers =
                emailEnvio.ListaPermissao.Where(x => x.Usuario != null)
                    .Select(x => new Usuario() {ID = x.Usuario.ID})
                    .ToList();

            var turma = string.IsNullOrWhiteSpace(txtTurma.Text)
                ? null
                : new classes.Turma {ID = int.Parse(txtTurma.Text)};

            var manter = new ManterEmail();

            manter.FormatarEmailEnvioFormulario(emailEnvio, perfisSelecionados, niveisOcupacionaisSelecionados,
                ufsSelecionadas, statusSelecionados, turma, selectedUsers);
        }

        private classes.EmailEnvio ObterObjetoEmailEnvio()
        {
            _emailEnvioEdicao = Request["Id"] != null
                ? new ManterEmailEnvio().ObterEmailEnvioPorID((int.Parse(Request["Id"])))
                : new classes.EmailEnvio();

            //Texto
            _emailEnvioEdicao.Texto = txtMensagem.Text.Trim();

            //Assunto
            _emailEnvioEdicao.Assunto = txtAssunto.Text.Trim();

            if (_emailEnvioEdicao.Uf == null)
            {
                Usuario usuario;

                using (var manterUsuario = new ManterUsuario())
                {
                    usuario = manterUsuario.ObterUsuarioLogado();
                }

                _emailEnvioEdicao.Uf = new ManterUf().ObterUfPorID(usuario.UF.ID);
            }

            AdicionarPermissao(_emailEnvioEdicao);

            return _emailEnvioEdicao;
        }

        private void AdicionarPermissao(classes.EmailEnvio emailEnvioEdicao)
        {
            AdicionarOuRemoverPerfil(emailEnvioEdicao);
            AdicionarOuRemoverUf(emailEnvioEdicao);
            AdicionarOuRemoverNivelOcupacional(emailEnvioEdicao);
            AdicionarOuRemoverTurma(emailEnvioEdicao);
            AdicionarOuRemoverAlunos(emailEnvioEdicao);
            AdicionarOuRemoverStatus(emailEnvioEdicao);
        }

        private void AdicionarOuRemoverAlunos(classes.EmailEnvio emailEnvioEdicao)
        {

            IList<EmailEnvioPermissao> listaNot = emailEnvioEdicao.ListaPermissao.Where(x => x.Usuario != null).ToList();

            // Remove todos do Objeto sempre e aloca somente os selecionados no grid view
            emailEnvioEdicao.removerUsuarios();

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

                            emailEnvioEdicao.adicionarUsuario(user, usuarioLogado);
                        }
                    }
                }
            }
        }


        private void AdicionarOuRemoverTurma(classes.EmailEnvio emailEnvioEdicao)
        {
            IList<EmailEnvioPermissao> listaNot = emailEnvioEdicao.ListaPermissao.Where(x => x.Turma != null).ToList();

            // Já tinha alguma turma escolhida           
            if (txtTurma.Text != "")
            {
                // Se a seleção não conter o valor escolhido, se já estiver não precisa atualizar nada
                if (!listaNot.Any(x => x.Turma.ID.Equals(int.Parse(txtTurma.Text))))
                {

                    BMUsuario bmUsu = new BMUsuario();
                    Usuario usuarioLogado = bmUsu.ObterUsuarioLogado();

                    //classes.Turma tur = new classes.Turma();
                    //tur = new ManterTurma().ObterTurmaPorID(ddlTurma.SelectedItem.Value);
                    ManterTurma mantTurma = new ManterTurma();
                    classes.Turma tur = mantTurma.ObterTurmaPorID(int.Parse(txtTurma.Text));

                    emailEnvioEdicao.adicionarTurma(tur, usuarioLogado);
                    emailEnvioEdicao.removerTurmasExceto(tur);

                }

            }// Se não selecinou nada mas tem algo no banco limpa
            else if (listaNot.Count() > 0)
            {
                emailEnvioEdicao.removerTurmas();
            }
        }

        private void AdicionarOuRemoverStatus(classes.EmailEnvio emailEnvioEdicao)
        {

            int[] statusSelecionados = WebFormHelper.ObterValoresSelecionadosCheckBoxList(chkStatus);

            emailEnvioEdicao.removerStatus(emailEnvioEdicao);

            // Já tinha alguma turma escolhida           
            if (statusSelecionados.Any())
            {

                // Se a seleção não conter o valor escolhido, se já estiver não precisa atualizar nada
                BMUsuario bmUsu = new BMUsuario();
                Usuario usuarioLogado = bmUsu.ObterUsuarioLogado();

                var manterStatusMatricula = new ManterStatusMatricula();
                var listaStatusMatricula = manterStatusMatricula.ObterTodosStatusMatricula();
                var listaStatusMatriculaSelecionadas = listaStatusMatricula.Where(x => statusSelecionados.Contains(x.ID));

                emailEnvioEdicao.adicionarStatus(listaStatusMatriculaSelecionadas, usuarioLogado);

            }// Se não selecinou nada mas tem algo no banco limpa
        }

        private void AdicionarOuRemoverNivelOcupacional(classes.EmailEnvio solucaoEducacionalEdicao)
        {
            var todosNiveisOcupacionais = this.ucPermissoes1.ObterTodosNiveisOcupacionais;  //.ObterPerfisSelecionados;

            if (todosNiveisOcupacionais != null && todosNiveisOcupacionais.Count > 0)
            {

                BMUsuario bmUsu = new BMUsuario();
                Usuario usuarioLogado = bmUsu.ObterUsuarioLogado();

                NivelOcupacional nivelOcupacionalSelecionado = null;

                for (int i = 0; i < todosNiveisOcupacionais.Count; i++)
                {
                    if (string.IsNullOrEmpty(todosNiveisOcupacionais[i].Value))
                        continue;

                    nivelOcupacionalSelecionado = new NivelOcupacional()
                    {
                        ID = int.Parse(todosNiveisOcupacionais[i].Value),
                        Nome = todosNiveisOcupacionais[i].Text
                    };

                    if (todosNiveisOcupacionais[i].Selected)
                    {
                        solucaoEducacionalEdicao.AdicionarNivelOcupacional(nivelOcupacionalSelecionado, usuarioLogado);
                    }
                    else
                    {
                        solucaoEducacionalEdicao.RemoverNivelOcupacional(nivelOcupacionalSelecionado);
                    }
                }
            }
        }

        private void AdicionarOuRemoverUf(classes.EmailEnvio solucaoEducacionalEdicao = null)
        {
            try
            {
                BMUsuario bmUsu = new BMUsuario();
                Usuario usuarioLogado = bmUsu.ObterUsuarioLogado();

                Repeater rptUFs = (Repeater)ucPermissoes1.FindControl("rptUFs");
                for (int i = 0; i < rptUFs.Items.Count; i++)
                {
                    CheckBox ckUF = (CheckBox)rptUFs.Items[i].FindControl("ckUF");
                    var lblUF = (Literal)rptUFs.Items[i].FindControl("lblUF");

                    int idUf = int.Parse(ckUF.Attributes["ID_UF"]);
                    var ufSelecionado = new Uf()
                    {
                        ID = idUf,
                        Nome = lblUF.Text,
                    };

                    if (ckUF.Checked)
                    {
                        solucaoEducacionalEdicao.AdicionarUfs(ufSelecionado, usuarioLogado);
                    }
                    else
                    {
                        solucaoEducacionalEdicao.RemoverUfs(ufSelecionado);
                    }
                }
            }
            catch (Exception)
            {
                throw new AcademicoException("Você deve informar a quantidade de vagas do estado");
            }
        }


        private void AdicionarOuRemoverPerfil(classes.EmailEnvio emailEnvioEdicao)
        {
            var todosPerfis = this.ucPermissoes1.ObterTodosPerfis;

            if (todosPerfis != null && todosPerfis.Count > 0)
            {
                for (int i = 0; i < todosPerfis.Count; i++)
                {
                    if (string.IsNullOrEmpty(todosPerfis[i].Value))
                        continue;

                    var perfilSelecionado = new Perfil()
                    {
                        ID = int.Parse(todosPerfis[i].Value),
                        Nome = todosPerfis[i].Text
                    };

                    if (todosPerfis[i].Selected)
                    {
                        emailEnvioEdicao.AdicionarPerfil(perfilSelecionado);
                    }
                    else
                    {
                        emailEnvioEdicao.RemoverPerfil(perfilSelecionado);
                    }
                }
            }
        }

        protected void btnEnviarEmail_Click(object sender, EventArgs e)
        {
            int idEmailEnvio = (Request["Id"] != null) ? int.Parse(Request["Id"]) : (int)idEmailGerado;

            _emailEnvioEdicao = _manterEmailEnvio.ObterEmailEnvioPorID(idEmailEnvio);

            totalGerado = _emailEnvioEdicao.ListaEmailsGerados.Count(x => x.Enviado != true);

            AlterarMensagemModal(totalGerado);            
           
            rptFiltrosUf.DataSource = FiltrarPermissoesUf(_emailEnvioEdicao.ListaPermissao);
            rptFiltrosUf.DataBind();            
           
            rptFiltrosNivelOcupacional.DataSource = FiltrarPermissoesNivelOcupacional(_emailEnvioEdicao.ListaPermissao);
            rptFiltrosNivelOcupacional.DataBind();            
           
            rptFiltrosPerfil.DataSource = FiltrarPermissoesPerfil(_emailEnvioEdicao.ListaPermissao);
            rptFiltrosPerfil.DataBind();

            rptFiltrosUsuario.DataSource = FiltrarPermissoesUsuario(_emailEnvioEdicao.ListaPermissao);
            rptFiltrosUsuario.DataBind();

            rptFiltrosStatus.DataSource = FiltrarPermissoesStatus(_emailEnvioEdicao.ListaPermissao);
            rptFiltrosStatus.DataBind();

            rptFiltrosTurma.DataSource = FiltrarPermissoesTurma(_emailEnvioEdicao.ListaPermissao);
            rptFiltrosTurma.DataBind();

            esconderFiltros(_emailEnvioEdicao);
            

            ExibirModal();
        }

        protected IEnumerable<EmailEnvioPermissao> FiltrarPermissoes(IEnumerable<EmailEnvioPermissao> permissoes)
        {
            return permissoes.Where(x => x.NivelOcupacional != null || x.Perfil != null
                                         || x.Uf != null || x.Usuario != null);
        }

        protected IEnumerable<EmailEnvioPermissao> FiltrarPermissoesUsuario(IEnumerable<EmailEnvioPermissao> permissoes)
        {
            return permissoes.Where(x => x.Usuario != null);
        }
        protected IEnumerable<EmailEnvioPermissao> FiltrarPermissoesUf(IEnumerable<EmailEnvioPermissao> permissoes)
        {
            return permissoes.Where(x => x.Uf != null);
        }
        protected IEnumerable<EmailEnvioPermissao> FiltrarPermissoesNivelOcupacional(IEnumerable<EmailEnvioPermissao> permissoes)
        {
            return permissoes.Where(x => x.NivelOcupacional != null);
        }
        protected IEnumerable<EmailEnvioPermissao> FiltrarPermissoesPerfil(IEnumerable<EmailEnvioPermissao> permissoes)
        {
            return permissoes.Where(x => x.Perfil != null);
        }
        protected IEnumerable<EmailEnvioPermissao> FiltrarPermissoesTurma(IEnumerable<EmailEnvioPermissao> permissoes)
        {
            return permissoes.Where(x => x.Turma != null);
        }
        protected IEnumerable<EmailEnvioPermissao> FiltrarPermissoesStatus(IEnumerable<EmailEnvioPermissao> permissoes)
        {
            return permissoes.Where(x => x.Status != null);
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

            if (Request["Id"] != null || (idEmailGerado != null && idEmailGerado > 0))
            {
                OcultarModal();
                int idEmailEnvio = (Request["Id"] != null) ? int.Parse(Request["Id"]) : (int)idEmailGerado;

                _emailEnvioEdicao = _manterEmailEnvio.ObterEmailEnvioPorID(idEmailEnvio);

                var ignorarEnviados = chkReenviarEmails.Checked;
                if (_emailEnvioEdicao != null)
                {
                    if ((_emailEnvioEdicao.ListaEmailsGerados != null && _manterEmailEnvio.ObterEmailsParaEnvio(_emailEnvioEdicao, ignorarEnviados).Any()))
                    {
                        int emailsComErro;
                        int emailsEnviados;

                        EnviarEmail(_emailEnvioEdicao, ignorarEnviados, out emailsEnviados, out emailsComErro);

                        _manterEmailEnvio.AlterarEmailEnvio(_emailEnvioEdicao);

                        if (emailsEnviados > 0 && emailsComErro == 0)
                        {
                            WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, emailsEnviados + " emails enviados com sucesso!");
                        }
                        else if (emailsEnviados == 0 && emailsComErro == 0)
                        {
                            WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Não existem emails pendente de envio!");
                        }
                        else
                        {
                            WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, emailsEnviados + " emails enviados com sucesso, " + emailsComErro.ToString() + " com erro!");
                        }
                    }
                    else
                    {
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Não existem emails para serem enviados! Caso queira reenviar os e-mails, Favor selecionar a opção Reencaminhar E-mails.");
                        chkReenviarEmails.Visible = true;
                    }
                }
                else
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Erro ao recuperar os dados!");
                }
            }
            else
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Favor salvar antes!");
            }
        }

        private void EnviarEmail(classes.EmailEnvio emailEnvio, bool ignorarEnviados, out int quantidadeEnviados, out int emailsComErro, bool validar = false)
        {
            emailsComErro = 0;
            quantidadeEnviados = 0;
            ManterEmail manterEmail = new ManterEmail();

            if (!validar)
            {
                foreach (var registro in _manterEmailEnvio.ObterEmailsParaEnvio(emailEnvio, ignorarEnviados))
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(registro.Usuario.Email))
                        {
                            manterEmail.EnviarEmail(registro.Usuario.Email, emailEnvio.Assunto, emailEnvio.Texto);
                            registro.Enviado = true;
                            registro.DataEnvio = DateTime.Now;
                            quantidadeEnviados++;
                        }
                    }
                    catch
                    {
                        emailsComErro++;
                    }
                }
            }
            else
            {
                foreach (var registro in _emailEnvioEdicao.ListaEmailsGerados)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(registro.Usuario.Email))
                        {
                            manterEmail.EnviarEmail(registro.Usuario.Email, _emailEnvioEdicao.Assunto, _emailEnvioEdicao.Texto);
                            registro.Enviado = true;
                            registro.DataEnvio = DateTime.Now;
                            quantidadeEnviados++;
                        }
                    }
                    catch
                    {
                        emailsComErro++;
                    }
                }
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Session.Remove("userSelected");

            if (idEmailGerado != null && idEmailGerado > 0)
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Informacao, "Registro salvo para envio posterior", "ListarEmailEnvio.aspx");

            Session["EmailEnvio"] = null;

            Response.Redirect("ListarEmailEnvio.aspx");
        }

        protected void UserSelectedHandler(object sender, CompleteUserSelectionEvent e)
        {
        }

        protected void btnRemoverFiltro_Click(object sender, EventArgs e)
        {
            var btnRemover = (HtmlButton)sender;

            int id;

            if (int.TryParse(btnRemover.Attributes["data-id"], out id) && idEmailGerado != null)
            {
                classes.EmailEnvio email;

                using (var manter = new ManterEmailEnvio())
                {
                    email = manter.ObterEmailEnvioPorID(idEmailGerado.Value);
                    email = manter.RemoverEmailsGerados(email);

                    // Busca por id novamente para ter o objeto novamente na sessão do nhibernate
                    email = manter.ObterEmailEnvioPorID(email.ID);

                    var permissao = new ManterEmailEnvioPermissao().ObterPorID(id);
                    email.ListaPermissao.Remove(permissao);

                    manter.AlterarEmailEnvio(email, true);

                    GerarEmails(email);
                }

                rptFiltrosPerfil.DataSource = FiltrarPermissoesPerfil(email.ListaPermissao);
                rptFiltrosPerfil.DataBind();

                rptFiltrosNivelOcupacional.DataSource = FiltrarPermissoesNivelOcupacional(email.ListaPermissao);
                rptFiltrosNivelOcupacional.DataBind();

                rptFiltrosUf.DataSource = FiltrarPermissoesUf(email.ListaPermissao);
                rptFiltrosUf.DataBind();

                rptFiltrosUsuario.DataSource = FiltrarPermissoesUsuario(email.ListaPermissao);
                rptFiltrosUsuario.DataBind();

                rptFiltrosTurma.DataSource = FiltrarPermissoesTurma(email.ListaPermissao);
                rptFiltrosTurma.DataBind();

                rptFiltrosStatus.DataSource = FiltrarPermissoesStatus(email.ListaPermissao);
                rptFiltrosStatus.DataBind();

                esconderFiltros(email);

                if (email.ListaPermissao.Count <= 0)
                {
                    OcultarModal();
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Você deve selecionar pelo menos um filtro");
                    return;
                }

                ucPermissoes1.LimparCampos();
                PreencherCampos(email);

                totalGerado = new ManterEmail().ObterPorEmailEnvio(email).Select(x => new { x.ID }).Count();
                
                AlterarMensagemModal(totalGerado);
            }
        }

        protected void btnRemoverFiltroTodos_Click(object sender, EventArgs e)
        {
            var btnRemover = (HtmlButton)sender;

            string filtro = btnRemover.Attributes["data-filtro"];

            var manter = new BMEmailEnvio();

            if (idEmailGerado != null )
            {
                var email = manter.ObterPorID(idEmailGerado.Value);

                // Busca por id novamente para ter o objeto novamente na sessão do nhibernate

                var query = new ManterEmailEnvioPermissao().ObterTodosIQueryable().Where(x => x.EmailEnvio.ID == email.ID);

                if(filtro == "nivel-ocupacional")
                {
                    query = query.Where(x => x.NivelOcupacional != null);
                }

                if (filtro == "perfil")
                {
                    query = query.Where(x => x.Perfil != null);
                }

                if (filtro == "uf")
                {
                    query = query.Where(x => x.Uf != null);
                }

                if (filtro == "usuario")
                {
                    query = query.Where(x => x.Usuario != null);
                }

                if (filtro == "turma")
                {
                    query = query.Where(x => x.Turma != null || x.Status != null);
                }               

                var listaPermissoes = query.ToList();
                foreach(EmailEnvioPermissao permissao in listaPermissoes)
                {
                    email.ListaPermissao.Remove(permissao);                    
                }

                manter.Salvar(email);

                rptFiltrosPerfil.DataSource = FiltrarPermissoesPerfil(email.ListaPermissao);
                rptFiltrosPerfil.DataBind();

                rptFiltrosNivelOcupacional.DataSource = FiltrarPermissoesNivelOcupacional(email.ListaPermissao);
                rptFiltrosNivelOcupacional.DataBind();

                rptFiltrosUf.DataSource = FiltrarPermissoesUf(email.ListaPermissao);
                rptFiltrosUf.DataBind();

                rptFiltrosUsuario.DataSource = FiltrarPermissoesUsuario(email.ListaPermissao);
                rptFiltrosUsuario.DataBind();

                rptFiltrosTurma.DataSource = FiltrarPermissoesTurma(email.ListaPermissao);
                rptFiltrosTurma.DataBind();

                rptFiltrosStatus.DataSource = FiltrarPermissoesStatus(email.ListaPermissao);
                rptFiltrosStatus.DataBind();

                esconderFiltros(email);

                if (email.ListaPermissao.Count <= 0)
                {
                    OcultarModal();
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Você deve selecionar pelo menos um filtro");
                    return;
                }

                ucPermissoes1.LimparCampos();
                PreencherCampos(email);

                totalGerado = _manterEmailEnvio.ObterUsuariosParaEnvio(email).Count();

                AlterarMensagemModal(totalGerado);
            }
        }


        protected void esconderFiltros(classes.EmailEnvio _emailEnvioEdicao) {

            divUf.Visible = true;
            divNivelOcupacional.Visible = true;
            divPerfil.Visible = true;
            divUsuario.Visible = true;
            divTurma.Visible = true;
            divStatus.Visible = true;

            var filtrosUf = FiltrarPermissoesUf(_emailEnvioEdicao.ListaPermissao);
            if (filtrosUf.Count() == 0)
            {
                divUf.Visible = false;
            }            

            var filtrosNivelOcupacional = FiltrarPermissoesNivelOcupacional(_emailEnvioEdicao.ListaPermissao);
            if (filtrosNivelOcupacional.Count() == 0)
            {
                divNivelOcupacional.Visible = false;
            }            

            var filtrosPerfil = FiltrarPermissoesPerfil(_emailEnvioEdicao.ListaPermissao);
            if (filtrosPerfil.Count() == 0)
            {
                divPerfil.Visible = false;
            }

            var filtrosUsuario = FiltrarPermissoesUsuario(_emailEnvioEdicao.ListaPermissao);
            if (filtrosUsuario.Count() == 0)
            {
                divUsuario.Visible = false;
            }

            var filtrosTurma = FiltrarPermissoesTurma(_emailEnvioEdicao.ListaPermissao);
            if (filtrosTurma.Count() == 0)
            {
                divTurma.Visible = false;
                divStatus.Visible = false;
            }

            var filtrosStatus = FiltrarPermissoesStatus(_emailEnvioEdicao.ListaPermissao);
            if (filtrosStatus.Count() == 0)
            {
                divStatus.Visible = false;
            }

        }
       
    }
}