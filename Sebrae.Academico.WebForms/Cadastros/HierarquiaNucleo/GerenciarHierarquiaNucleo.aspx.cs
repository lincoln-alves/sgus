using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.WebForms.Cadastros.HierarquiaNucleo
{
    public partial class GerenciarHierarquiaNucleo : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                PreencherHierarquiaNivel();
            }
        }

        private void PreencherHierarquiaNivel()
        {
            if (Request["Id"] == null)
            {
                Response.Redirect("ListarHierarquiaNucleo.aspx");
            }
            else
            {
                var ufId = int.Parse(Request["Id"]);

                var uf = new ManterUf().ObterUfPorID(ufId);

                if (uf == null)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "UF não existente.");
                    return;
                }

                UfSelecionada = uf;
                usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

                var hierarquiaCargo = new ManterHierarquiaNucleo().ObterPorUf(UfSelecionada);                

                if (!usuarioLogado.IsAdministrador())
                {
                    LupaUsuario.Uf = usuarioLogado.UF;
                }


                btnAdicionarNucleo.Visible = false;
                if (ObterPermissaoUsuario())
                {
                    btnAdicionarNucleo.Visible = true;
                }                    
                
                rptDiretoria.DataSource = hierarquiaCargo;
                rptDiretoria.DataBind();
            }
        }

        public Uf UfSelecionada
        {
            get { return (Uf)Session["UfSelecionada"]; }
            set { Session["UfSelecionada"] = value; }
        }

        public Usuario usuarioLogado { get; set; }

        public bool ObterPermissaoUsuario()
        {
            return usuarioLogado.IsAdministrador() || (usuarioLogado.IsGestor() && usuarioLogado.UF.ID == UfSelecionada.ID);
        }

        protected void rptHierarquiaNucleo_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var hierarquiaNucleo = (Dominio.Classes.HierarquiaNucleo)e.Item.DataItem;

                //Nucleos
                var ltrHierarquiaNucleo = (Literal)e.Item.FindControl("ltrHierarquiaNucleo");
                ltrHierarquiaNucleo.Text = hierarquiaNucleo.Nome.ToUpper();
                

                // Gestores
                var gestores = hierarquiaNucleo.ObterGestores();
                if (gestores.Any()) {
                    var rptGestores = (Repeater)e.Item.FindControl("rptGestores");

                    rptGestores.DataSource = gestores;
                    rptGestores.DataBind();
                } else {
                    ((Literal)e.Item.FindControl("ltrGestorVazio")).Visible = true;
                }

                // Botao de incluir Gestor.
                var btnAdicionarGestor = (HtmlButton)e.Item.FindControl("btnAdicionarGestor");
                btnAdicionarGestor.Attributes["data-hierarquiaNucleo"] = hierarquiaNucleo.ID.ToString();

                // Botão de editar núcleo.
                var btnEditarNucleo = (HtmlAnchor)e.Item.FindControl("btnEditarNucleo");
                btnEditarNucleo.Attributes["data-hierarquiaNucleo"] = hierarquiaNucleo.ID.ToString();                
                   

                // Funcionarios
                var funcionarios = hierarquiaNucleo.ObterFuncionariosHierarquiaNucleo();
                if (funcionarios.Any())
                {
                    var rptFuncionarios = (Repeater)e.Item.FindControl("rptFuncionarios");

                    rptFuncionarios.DataSource = funcionarios;
                    rptFuncionarios.DataBind();
                }
                else {
                    ((Literal)e.Item.FindControl("ltrFuncionarioVazio")).Visible = true;
                }
                // Botao de incluir Funcionario.
                var btnAdicionarFuncionario = (HtmlButton)e.Item.FindControl("btnAdicionarFuncionario");
                btnAdicionarFuncionario.Attributes["data-hierarquiaNucleo"] = hierarquiaNucleo.ID.ToString();

                var divDisabledNucleo = (Panel)e.Item.FindControl("divDisabledNucleo");
                if (!hierarquiaNucleo.Ativo)
                    divDisabledNucleo.CssClass = "panel-body disabled";

                btnAdicionarGestor.Visible = false;
                btnAdicionarFuncionario.Visible = false;
                btnEditarNucleo.Visible = false;

                if (ObterPermissaoUsuario()) {                    
                    btnAdicionarFuncionario.Visible = true;
                    btnEditarNucleo.Visible = true;

                    if (!gestores.Any()) {
                        btnAdicionarGestor.Visible = true;
                    }
                }
                
            }
        }

        protected void rptGestores_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var hierarquiaNucleoUsuario = (HierarquiaNucleoUsuario)e.Item.DataItem;

                var btnRemoverUsuario = (HtmlButton)e.Item.FindControl("btnRemoverUsuario");
                btnRemoverUsuario.Attributes["data-hierarquiaCargo"] = hierarquiaNucleoUsuario.ID.ToString();

                btnRemoverUsuario.Visible = false;
                if (ObterPermissaoUsuario())
                {
                    btnRemoverUsuario.Visible = true;
                }
                    
            }
        }

        protected void rptFuncionarios_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var hierarquiaNucleoUsuario = (HierarquiaNucleoUsuario)e.Item.DataItem;

                var btnRemoverUsuario = (HtmlButton)e.Item.FindControl("btnRemoverUsuario");
                btnRemoverUsuario.Attributes["data-hierarquiaCargo"] = hierarquiaNucleoUsuario.ID.ToString();

                btnRemoverUsuario.Visible = false;
                if (ObterPermissaoUsuario())
                    btnRemoverUsuario.Visible = true;
            }
        }

        protected void btnAdicionarGestor_OnServerClick(object sender, EventArgs e)
        {
            ltrTituloModal.Text = "Cadastrar Gestor";
            hdnIsGestor.Value = "1";

            AbrirCadastro(sender);
        }

        protected void btnAdicionarFuncionario_OnServerClick(object sender, EventArgs e)
        {
            ltrTituloModal.Text = "Cadastrar Funcionário";
            hdnIsGestor.Value = "0";

            AbrirCadastro(sender);
        }


        private void AbrirCadastro(object sender)
        {
            var button = (HtmlButton) sender;

            var hierarquiaNucleo = int.Parse(button.Attributes["data-hierarquiaNucleo"]);
            var isGestor = int.Parse(button.Attributes["data-gestor"]);

            ExibirModal(hierarquiaNucleo, isGestor);
        }

        protected void btnRemoverUsuario_OnServerClick(object sender, EventArgs e)
        {
            var button = (HtmlButton)sender;

            var idHierarquiaNucleoUsuario = int.Parse(button.Attributes["data-hierarquiaCargo"]);

            try
            {
                (new ManterHierarquiaNucleoUsuario()).Remover(idHierarquiaNucleoUsuario);

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Usuário removido da Hierarquia de Núcleo.");
            }
            catch (Exception)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro,"Erro ao remover usuário da Hierarquia de Núcleo.");
            }
            finally
            {
                PreencherHierarquiaNivel();
            }
        }

        private void ExibirModal(int idHierarquiaNucleo, int isGestor = 0)
        {
            var hierarquiaNucleo = new ManterHierarquiaNucleo().ObterPorId(idHierarquiaNucleo);

            if (hierarquiaNucleo == null)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Erro ao obter Hierarquia Núcleo");
                return;
            }

            if (Master != null && Master.Master != null)
            {
                var pnlbackdrop = ((Panel)Master.Master.FindControl("pnlbackdrop"));

                if (pnlbackdrop != null)
                {
                    // Preencher campos do Modal.
                    hdnIdHierarquiaNucleo.Value = hierarquiaNucleo.ID.ToString();
                    txtUfModal.Text = UfSelecionada.Nome + " - " + UfSelecionada.Sigla;

                    txtHierarquiaNucleoModal.Text = hierarquiaNucleo.Nome;

                    pnlbackdrop.Visible = true;
                    pnlModal.Visible = true;
                }
            }
        }

        private void LimparModal()
        {
            txtHierarquiaNucleoModal.Text = "";
            hdnIdHierarquiaNucleo.Value = "";
            hdnIsGestor.Value = "";
        }

        protected void OcultarModal_Click(object sender, EventArgs e)
        {
            LupaUsuario.LimparCampos();

            if (Master != null && Master.Master != null)
            {
                LimparModal();

                var pnlbackdrop = ((Panel)Master.Master.FindControl("pnlbackdrop"));

                if (pnlbackdrop != null)
                {
                    pnlModal.Visible = false;
                    pnlbackdrop.Visible = false;
                }
            }

            PreencherHierarquiaNivel();
        }

        protected void btnSalvar_OnClick(object sender, EventArgs e)
        {
            var usuarioSelecionado = LupaUsuario.SelectedUser;

            if (usuarioSelecionado == null)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Usuário é obrigatório");
                return;
            }

            var idHierarquiaNucleo = int.Parse(hdnIdHierarquiaNucleo.Value);
            var hierarquiaNucleo = (new ManterHierarquiaNucleo()).ObterPorId(idHierarquiaNucleo);

            var isGestor = int.Parse(hdnIsGestor.Value) == 1 ? true : false;

            IQueryable<HierarquiaNucleoUsuario> hierarquiaNucleoUsuarioList = new ManterHierarquiaNucleoUsuario().ObterUsuariosNucleo(usuarioSelecionado.ID);

            if (hierarquiaNucleoUsuarioList.Count() > 0)
            {
                var usuarioPertenceMesmoNucleo = hierarquiaNucleoUsuarioList.Where(x => x.HierarquiaNucleo.ID == idHierarquiaNucleo);
                IdUsuarioSelecionado.Value = usuarioSelecionado.ID.ToString();
                IdHierarquiaNucleoSelcionado.Value = hierarquiaNucleo.ID.ToString();
                IsGestorSelecionado.Value = isGestor ? "1" : "0";

                if (isGestor)
                {
                    btnDuplicar.Visible = true;
                    TxtMgsConfirmacao.Text = "Colaborador selecionado pertence a outro Núcleo, deseja Duplicar ou Mover o colaborador?";
                    if (usuarioPertenceMesmoNucleo.Any())
                    {
                        TxtMgsConfirmacao.Text = "Colaborador selecionado pertence a esse Núcleo, deseja Mover o colaborador?";
                        btnDuplicar.Visible = false;
                    }
                }
                else
                {
                    var isFuncionarioNucleo = hierarquiaNucleoUsuarioList.Where(x=> x.HierarquiaNucleo.ID == hierarquiaNucleo.ID && x.IsGestor == false).ToList();

                    if (isFuncionarioNucleo.Count > 0) {
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "O Funcionário selecionado já é funcionário do desse núcleo");
                        return;
                    }

                    var nomesNucleosPertencentes = hierarquiaNucleoUsuarioList.Select(x=> (x.IsGestor ? "Gestor" : "Funcionário") + " de " + x.HierarquiaNucleo.Nome);
                    var msgBase = usuarioSelecionado.Nome + " já faz parte do(s) seguinte(s) núcleo(s) : <br>";
                    string msg = String.Join("<br>", nomesNucleosPertencentes);
                    TxtMgsConfirmacao.Text = msgBase + msg;
                    btnSalvarFuncionario.Visible = true;
                    btnDuplicar.Visible = false;
                    btnMover.Visible = false;
                }

                OcultarModal_Click(null, null);
                ExibirModalUsuarioConfirmacao();
            }
            else
            {
                salvarUsuarioNucleo(usuarioSelecionado, hierarquiaNucleo, UfSelecionada, isGestor);
            }
            
        }


        protected void btnDuplicar_OnClick(object sender, EventArgs e)
        {
            var idHierarquiaNucleo = int.Parse(IdHierarquiaNucleoSelcionado.Value);
            var hierarquiaNucleo = (new ManterHierarquiaNucleo()).ObterPorId(idHierarquiaNucleo);

            var idUsuario = int.Parse(IdUsuarioSelecionado.Value);
            var usuarioSelecionado = (new ManterUsuario()).ObterPorID(idUsuario);

            var isGestor = int.Parse(IsGestorSelecionado.Value) == 1 ? true : false;

            salvarUsuarioNucleo(usuarioSelecionado, hierarquiaNucleo, UfSelecionada, isGestor);

        }


        protected List<EtapaResposta> ObterSolicitacoesUsuario(int idUsuario) {
            
            var usuarioSelecionado = (new ManterUsuario()).ObterPorID(idUsuario);

            IQueryable<HierarquiaNucleoUsuario> listaHierarquiaNucleoUsuario = new ManterHierarquiaNucleoUsuario().ObterUsuariosNucleo(usuarioSelecionado.ID);

            List<int> idsEtapas = new List<int>();

            foreach (HierarquiaNucleoUsuario hierarquiaNucleoUsuario in listaHierarquiaNucleoUsuario)
            {
                var etapasPermissaoNucleo = new ManterEtapaPermissaoNucleo().ObterPorHierarquiaNucleoUsario(hierarquiaNucleoUsuario.ID);

                if (etapasPermissaoNucleo.Any())
                {

                    foreach (EtapaPermissaoNucleo etapaPermissaoNucleo in etapasPermissaoNucleo)
                    {
                        idsEtapas.Add(etapaPermissaoNucleo.Etapa.ID);

                    }
                }
            }

            if (idsEtapas.Count > 0)
            {
                List<EtapaResposta> demandas_usuario = new ManterEtapaResposta().ObterTodos().Where(x => idsEtapas.Contains(x.Etapa.ID) && x.Status == (int)enumStatusEtapaResposta.Aguardando).ToList();
                return demandas_usuario;
            }

            return null;
        }


        protected void btnConfirmacaoMover_onClick(object sender, EventArgs e) {

            var idUsuario = int.Parse(IdUsuarioSelecionado.Value);

            var solicitacoes = ObterSolicitacoesUsuario(idUsuario);

            string msg;
            if (solicitacoes != null)
            {             

                var solicitacoesAguardando = solicitacoes.Where(x=> x.ProcessoResposta.Status == (int)enumStatusEtapaResposta.Aguardando).ToList();
                var idsSolicitacoes = solicitacoesAguardando.Select(x => $"#{x.ProcessoResposta.ID}").ToList();
                msg = String.Join(", ", idsSolicitacoes);

                msg = "A(s) solicitação(ões) " + msg + " ficará(ão) sem responsáveis por aprovação de suas etapas. Deseja prosseguir?";
            }
            else
            {
                msg = "Não existe nenhuma solicitação aguardando análise do usuário selecionado, Deseja prosseguir?"; 

            }            

            ExibirModalConfirmacaoMover(msg);

        }
       

        protected void btnMover_OnClick(object sender, EventArgs e)
        {
            try
            {
                var manterHierarquiaNucleoUsuario = new ManterHierarquiaNucleoUsuario();

                var idHierarquiaNucleo = int.Parse(IdHierarquiaNucleoSelcionado.Value);
                var hierarquiaNucleo = new ManterHierarquiaNucleo().ObterPorId(idHierarquiaNucleo);

                var idUsuario = int.Parse(IdUsuarioSelecionado.Value);
                var usuarioSelecionado = new ManterUsuario().ObterPorID(idUsuario);

                var isGestor = int.Parse(IsGestorSelecionado.Value) == 1 ? true : false;

                IQueryable<HierarquiaNucleoUsuario> listaHierarquiaNucleoUsuario = manterHierarquiaNucleoUsuario.ObterUsuariosNucleo(usuarioSelecionado.ID);

                if (listaHierarquiaNucleoUsuario.Any())
                {
                    foreach (HierarquiaNucleoUsuario hierarquiaNucleoUsuario in listaHierarquiaNucleoUsuario)
                    {
                        var etapasPermissaoNucleo = new ManterEtapaPermissaoNucleo().ObterPorHierarquiaNucleoUsario(hierarquiaNucleoUsuario.ID);
                        if (etapasPermissaoNucleo.Any())
                        {
                            foreach (var etapaPermissaoNucleo in etapasPermissaoNucleo)
                            {
                                new ManterEtapaRespostaPermissao().ExcluirTodosFkEtapaPermissaoNucleo(etapaPermissaoNucleo.ID);
                            }
                        }

                        manterHierarquiaNucleoUsuario.ExcluirTodosFkHieraquiaNucleoUsuario(hierarquiaNucleoUsuario.ID);

                        manterHierarquiaNucleoUsuario.Excluir(hierarquiaNucleoUsuario.ID);
                    }
                }

                salvarUsuarioNucleo(usuarioSelecionado, hierarquiaNucleo, UfSelecionada, isGestor);
                var usuarioHierarquiaNucleoInserido = manterHierarquiaNucleoUsuario.ObterPorUsuarioEHieraquiaNucleo(usuarioSelecionado.ID, hierarquiaNucleo.ID);

                List<int> idsUsuariosNucleo = hierarquiaNucleo.HierarquiaNucleoUsuarios.Select(x => x.ID).ToList();

                if (idsUsuariosNucleo.Any())
                {
                    var etapasHierarquiaNucleo = new ManterEtapaPermissaoNucleo().ObterPorHierarquiaNucleoUsarioIn(idsUsuariosNucleo).Select(x => x.Etapa).Distinct().ToList();
                    if (etapasHierarquiaNucleo.Any())
                    {
                        foreach (var etapa in etapasHierarquiaNucleo)
                        {

                            var etapaPermissaoNucleoAdd = new EtapaPermissaoNucleo()
                            {
                                Etapa = etapa,
                                HierarquiaNucleoUsuario = usuarioHierarquiaNucleoInserido
                            };

                            new ManterEtapaPermissaoNucleo().Salvar(etapaPermissaoNucleoAdd);
                        }
                    }
                }
            }
            catch (Exception)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Erro ao mover o usuário de Hierarquia de Núcleo.");
            }
        }

        protected void salvarUsuarioNucleo(Usuario usuarioSelecionado, Dominio.Classes.HierarquiaNucleo hierarquiaNucleo, Uf UfSelecionada, bool isGestor)
        {
            try
            {
                var manterHierarquiaNucleoUsuario = new ManterHierarquiaNucleoUsuario();
                manterHierarquiaNucleoUsuario.Salvar(usuarioSelecionado, hierarquiaNucleo, UfSelecionada, isGestor);

                //ADICIONA USUARIO DO NUCLEO AS PERMISSOES DAS ETAPAS QUE POSSUI O NUCLEO
                var hierarquiaNucleoUsuario = manterHierarquiaNucleoUsuario.ObterTodosIQueryable()
                    .FirstOrDefault(x => x.Usuario.ID == usuarioSelecionado.ID && x.HierarquiaNucleo.ID == hierarquiaNucleo.ID);

                var etapasHierarquia = new ManterEtapaPermissaoNucleo()
                    .ObterTodosIQueryable()
                    .Where(x => x.HierarquiaNucleoUsuario != null &&
                    x.HierarquiaNucleoUsuario.HierarquiaNucleo != null &&
                    x.HierarquiaNucleoUsuario.HierarquiaNucleo.ID == hierarquiaNucleo.ID)
                    .Select(x => x.Etapa)
                    .Distinct().ToList();

                if (hierarquiaNucleoUsuario != null && etapasHierarquia.Any())
                {
                    var manterEtapaPermissaoNucleo = new ManterEtapaPermissaoNucleo();
                    foreach (var etapa in etapasHierarquia)
                    {
                        var permissao = new EtapaPermissaoNucleo()
                        {
                            Etapa = etapa,
                            HierarquiaNucleoUsuario = hierarquiaNucleoUsuario
                        };
                        manterEtapaPermissaoNucleo.Salvar(permissao);
                    }
                    
                }

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Usuário incluído com sucesso.");

                OcultarModal_Click(null, null);
                OcultarModalUsuarioConfirmacao_Click(null, null);
                OcultarModaConfirmacaoMover_Click(null, null);

                if (isGestor) {

                    List<int> idsUsuariosNucleo = hierarquiaNucleo.HierarquiaNucleoUsuarios.Select(x => x.ID).ToList();

                    if (idsUsuariosNucleo.Count > 0) {

                        List<int> idsEtapasHierarquiaNucleo = new ManterEtapaPermissaoNucleo().ObterPorHierarquiaNucleoUsarioIn(idsUsuariosNucleo).Select(x => x.Etapa.ID).Distinct().ToList();

                        var etapasResposta = new ManterEtapaResposta().ObterTodos().Where(x => idsEtapasHierarquiaNucleo.Contains(x.Etapa.ID)).ToList();

                        var etapasRespostaAguaradando = etapasResposta.Where(x=> x.ProcessoResposta.Status == (int)enumStatusEtapaResposta.Aguardando).Distinct().ToList();

                        var idsSolicitacoes = etapasRespostaAguaradando.Select(x => $"#{x.ProcessoResposta.ID}").ToList();

                        string msg;
                        msg = String.Join(", ", idsSolicitacoes);

                        msg = "A(s) solicitação(ões) " + msg + " passa(m) a ter como responsável por aprovação o(a) " + usuarioSelecionado.Nome;
                        ExibirModalNovoResponsavel(msg);
                    }
                       
                }
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
            catch (Exception ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Erro ao incluir o usuário na Hierarquia de Núcleo.");
            }
        }

        protected void Voltar_OnServerClick(object sender, EventArgs e)
        {
            Response.Redirect("ListarHierarquiaNucleo.aspx");
        }

        protected void btnAdicionarNucleo_OnClick(object sender, EventArgs e)
        {
            idHierarquiaNucleo.Value = "0";
            ExibirModalNucleo("Adicionar Núcleo");
                      
        }

        private void ExibirModalNucleo(string titulo)
        {
            if (Master != null && Master.Master != null)
            {
                ltrTituloModalNucleo.Text = titulo;

                var pnlbackdrop = ((Panel)Master.Master.FindControl("pnlbackdrop"));

                if (pnlbackdrop != null)
                {
                    pnlbackdrop.Visible = true;
                    pnlModalNucleo.Visible = true;
                }
            }
        }


        private void ExibirModalUsuarioConfirmacao()
        {
            if (Master != null && Master.Master != null)
            {               

                var pnlbackdrop = ((Panel)Master.Master.FindControl("pnlbackdrop"));

                if (pnlbackdrop != null)
                {
                    pnlbackdrop.Visible = true;
                    pnlModalGestorConfirmacao.Visible = true;
                }
            }
        }

        protected void OcultarModalUsuarioConfirmacao_Click(object sender, EventArgs e)
        {
            if (Master != null && Master.Master != null)
            {
                LimparModalNucleo();

                var pnlbackdrop = ((Panel)Master.Master.FindControl("pnlbackdrop"));

                if (pnlbackdrop != null)
                {
                    pnlbackdrop.Visible = false;
                    pnlModalGestorConfirmacao.Visible = false;
                }
            }

            PreencherHierarquiaNivel();
        }


        private void ExibirModalMsg(string msg)
        {
            if (Master != null && Master.Master != null)    {
                

                var pnlbackdrop = ((Panel)Master.Master.FindControl("pnlbackdrop"));

                if (pnlbackdrop != null)
                {
                    txtMsgInativacao.Text = "A(s) solicitação(ões) " + msg + " ficará(ão) sem responsáveis por aprovação de suas etapas. Deseja prosseguir?";
                    pnlbackdrop.Visible = true;
                    pnlModalMsg.Visible = true;
                }
            }
        }


        private void ExibirModalConfirmacaoMover(string msg)
        {
            if (Master != null && Master.Master != null)
            {

                var pnlbackdrop = ((Panel)Master.Master.FindControl("pnlbackdrop"));

                if (pnlbackdrop != null)
                {                    
                    txtConfirmacaoMover.Text = msg;
                    pnlbackdrop.Visible = true;
                    pnlModalConfirmacaoMover.Visible = true;
                }
            }
        }

        private void ExibirModalNovoResponsavel(string msg)
        {
            if (Master != null && Master.Master != null)
            {

                var pnlbackdrop = ((Panel)Master.Master.FindControl("pnlbackdrop"));

                if (pnlbackdrop != null)
                {
                    txtNovoResponsavel.Text = msg;
                    pnlbackdrop.Visible = true;
                    pnlModalMsgSolicitacoesResponsavel.Visible = true;
                }
            }
        }

        protected void OcultarModalNovoResponsavel_Click(object sender, EventArgs e)
        {
            if (Master != null && Master.Master != null)
            {
                LimparModalNucleo();

                var pnlbackdrop = ((Panel)Master.Master.FindControl("pnlbackdrop"));

                if (pnlbackdrop != null)
                {
                    pnlbackdrop.Visible = false;
                    pnlModalMsgSolicitacoesResponsavel.Visible = false;
                }
            }

            PreencherHierarquiaNivel();
        }

        protected void OcultarModaConfirmacaoMover_Click(object sender, EventArgs e)
        {
            if (Master != null && Master.Master != null)
            {
                LimparModalNucleo();

                var pnlbackdrop = ((Panel)Master.Master.FindControl("pnlbackdrop"));

                if (pnlbackdrop != null)
                {
                    pnlModalNucleo.Visible = false;
                    pnlModalConfirmacaoMover.Visible = false;
                }
            }

            PreencherHierarquiaNivel();
        }


        protected void OcultarModalNucleo_Click(object sender, EventArgs e)
        {
            if (Master != null && Master.Master != null)
            {
                LimparModalNucleo();

                var pnlbackdrop = ((Panel)Master.Master.FindControl("pnlbackdrop"));

                if (pnlbackdrop != null)
                {
                    pnlModalNucleo.Visible = false;
                    pnlbackdrop.Visible = false;
                }
            }

            PreencherHierarquiaNivel();
        }


        protected void OcultarModalMsg_Click(object sender, EventArgs e)
        {
            if (Master != null && Master.Master != null)
            {
                LimparModalNucleo();

                var pnlbackdrop = ((Panel)Master.Master.FindControl("pnlbackdrop"));

                if (pnlbackdrop != null)
                {
                    pnlModalMsg.Visible = false;
                    pnlbackdrop.Visible = false;
                }
            }

            PreencherHierarquiaNivel();
        }


        protected void btnSalvarNucleo_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtTitulo.Text))
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Nome do Núcleo Obrigatório");
                    return;
                }

                Dominio.Classes.HierarquiaNucleo hierarquiaNucleo = new Dominio.Classes.HierarquiaNucleo();
                List<EtapaResposta> etapasRespostas = new List<EtapaResposta>();
                

                var HierarquiaNucleoId = int.Parse(idHierarquiaNucleo.Value);

                if (HierarquiaNucleoId > 0)
                {
                    hierarquiaNucleo = new ManterHierarquiaNucleo().ObterPorId(HierarquiaNucleoId);
                    if (rblStatus.SelectedValue == "0" && hierarquiaNucleo.Ativo)
                    {
                        etapasRespostas = new ManterEtapaResposta().ObterEtapasRespostaAnalistas(hierarquiaNucleo.HierarquiaNucleoUsuarios.ToList());
                        if (etapasRespostas.Count == 0)
                            hierarquiaNucleo.Ativo = false;

                    }
                    else
                    {
                        hierarquiaNucleo.Ativo = rblStatus.SelectedValue == "1" ? true : false;
                    }
                }
                else
                {                    
                    hierarquiaNucleo.Uf = UfSelecionada;
                    hierarquiaNucleo.Ativo = rblStatus.SelectedValue == "1" ? true : false;
                }
                
                hierarquiaNucleo.Nome = txtTitulo.Text;                

                new ManterHierarquiaNucleo().Salvar(hierarquiaNucleo);            


                OcultarModalNucleo_Click(null, null);

                if (etapasRespostas.Count > 0)
                {
                    idHieraquiaNucleoInativacao.Value = hierarquiaNucleo.ID.ToString();

                   // var ids = etapasRespostas.Select(x => x.ProcessoResposta.ID);
                    var ids = etapasRespostas.Select(x => $"#{x.ProcessoResposta.ID}").ToList();
                    string msg = String.Join(", ", ids);
                    ExibirModalMsg(msg);
                }
                else {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Núcleo salvo com sucesso");
                }

            }
            catch (Exception ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Erro ao salvar Núcleo");
                return;
            }
        }


        protected void btnEditarNucleo_OnServerClick(object sender, EventArgs e)
        {
            var link = (HtmlAnchor)sender;
            var ID = int.Parse(link.Attributes["data-hierarquiaNucleo"]);
            var hierarquiaNucleo = new ManterHierarquiaNucleo().ObterPorId(ID);

            txtTitulo.Text = hierarquiaNucleo.Nome;
            rblStatus.SelectedValue = hierarquiaNucleo.Ativo ? "1" : "0";
            idHierarquiaNucleo.Value = hierarquiaNucleo.ID.ToString();
            btnSalvarNucleo.Enabled = true;

            ExibirModalNucleo("Alterar Núcleo");
                    
        }

        protected void btnInativarNucleo_OnClick(object sender, EventArgs e)
        {
            
            var ID = int.Parse(idHieraquiaNucleoInativacao.Value);
            var hierarquiaNucleo = new ManterHierarquiaNucleo().ObterPorId(ID);
            hierarquiaNucleo.Ativo = false;
            new ManterHierarquiaNucleo().Salvar(hierarquiaNucleo);

            OcultarModalMsg_Click(null, null);
            WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Núcleo inativado com sucesso");

        }

        private void LimparModalNucleo()
        {            
            txtTitulo.Text = "";           
            btnSalvarNucleo.Enabled = false;
            rblStatus.SelectedValue = "1";
        }


    }
}