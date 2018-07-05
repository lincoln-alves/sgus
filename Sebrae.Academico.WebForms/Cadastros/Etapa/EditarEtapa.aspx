<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="EditarEtapa.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.Etapa.EditarEtapa" MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/UserControls/ucPermissoes.ascx" TagName="ucPermissoes" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucLupaMultiplosUsuarios.ascx" TagName="ucLupaMultiplosUsuarios"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>
<%@ Register Src="~/UserControls/ucSeletorListBox.ascx" TagPrefix="uc1" TagName="ucListBox" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .hide-show {
            cursor: pointer;
        }

            .hide-show:hover {
                text-decoration: underline;
            }
    </style>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <script src="/js/jqueryUI/etapa.js" type="text/javascript"></script>
    <link href="/css/fakeTable.css" rel="stylesheet" type="text/css" />
    <fieldset>
        <div class="form-group">
            <asp:Label ID="Label1" runat="server" Text="Processo" AssociatedControlID="txtNome"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip1" Chave="nomeProcessoTelaEtapa" />
            <asp:TextBox ID="txtProcesso" runat="server" Enabled="false" MaxLength="150" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label2" runat="server" Text="Nome*" AssociatedControlID="txtNome"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip2" Chave="nomeEtapa" />
            <asp:TextBox ID="txtNome" runat="server" MaxLength="150" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label3" runat="server" Text="ESTA ETAPA DEPENDE DE APROVAÇÃO?" AssociatedControlID="rblRequerAtivacao"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip3" Chave="etapaDependeAprovacao" />
            <asp:RadioButtonList ID="rblRequerAtivacao" runat="server" RepeatDirection="Horizontal"
                CssClass="form-control" OnSelectedIndexChanged="rblRequerAtivacao_SelectedIndexChanged"
                AutoPostBack="true">
                <asp:ListItem Text="Sim" Value="1"></asp:ListItem>
                <asp:ListItem Text="Não" Value="0"></asp:ListItem>
            </asp:RadioButtonList>
        </div>

        <div id="divContainerNomes" runat="server" visible="False">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title" style="margin-bottom: 0">Botão de aprovação
                    </h3>
                </div>
                <div class="panel-body">
                    <div class="form-group" runat="server">
                        <asp:Label ID="Label17" runat="server" Text="Nome do botão de aprovação" AssociatedControlID="rblRequerAtivacao"></asp:Label>
                        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip4" Chave="etapaNomeFinalizacao" />
                        <asp:DropDownList ID="ddlNomeFinalizacaoEtapa" runat="server" CssClass="form-control mostrarload" AutoPostBack="True" OnSelectedIndexChanged="ddlNomeFinalizacaoEtapa_OnSelectedIndexChanged"></asp:DropDownList>
                    </div>

                    <div class="form-group" runat="server" id="divNomeFinalizacaoEtapaOutros" visible="False">
                        <asp:Label ID="Label18" runat="server" Text="Especifique o nome do botão" AssociatedControlID="txtNomeFinalizacaoOutro"></asp:Label>
                        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip5" Chave="nomeFinalizacao" />
                        <asp:TextBox ID="txtNomeFinalizacaoOutro" runat="server" MaxLength="50" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
            </div>

            <div class="form-group" runat="server">
                <asp:Label ID="LabelDropdownAjuste" runat="server" Text="Haverá retorno para Ajuste?" AssociatedControlID="rblBotaoAjuste"></asp:Label>
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltipAjuste" Chave="botaoAjuste" />
                <asp:RadioButtonList ID="rblBotaoAjuste" runat="server" RepeatDirection="Horizontal"
                    CssClass="form-control" OnSelectedIndexChanged="rblBotaoAjuste_SelectedIndexChanged"
                    AutoPostBack="true">
                    <asp:ListItem Text="Sim" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Não" Value="0"></asp:ListItem>
                </asp:RadioButtonList>
            </div>

            <div id="divContainerAjuste" runat="server" visible="False">

                <asp:Panel ID="pnlRetornarSelect" runat="server" Visible="false">
                    <div class="form-group">
                        <asp:Label ID="Label4" runat="server" Text="Etapa de Retorno" AssociatedControlID="ddlEtapaRetorno"></asp:Label>
                        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip30" Chave="etapaDeRetorno" />
                        <asp:DropDownList ID="ddlEtapaRetorno" runat="server" AutoPostBack="true" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </asp:Panel>

                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title" style="margin-bottom: 0">Botão de Ajuste
                        </h3>
                    </div>
                    <div class="panel-body">
                        <div class="form-group" runat="server">
                            <asp:Label ID="LabelAjuste" runat="server" Text="Nome do botão de ajuste" AssociatedControlID="rblBotaoAjuste"></asp:Label>
                            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltipAjusteDropDown" Chave="etapaNomeAjuste" />
                            <asp:DropDownList ID="ddlNomeBotaoAjuste" runat="server" CssClass="form-control mostrarload" AutoPostBack="True" OnSelectedIndexChanged="ddlNomeBotaoAjuste_SelectedIndexChanged"></asp:DropDownList>
                        </div>

                        <div class="form-group" runat="server" id="AjusteOutro" visible="False">
                            <asp:Label ID="Label24" runat="server" Text="Especifique o nome do botão" AssociatedControlID="txtNomeFinalizacaoOutro"></asp:Label>
                            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip32" Chave="nomeFinalizacao" />
                            <asp:TextBox ID="txtNomeAjuste" runat="server" MaxLength="50" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>

            <div class="form-group" runat="server">
                <asp:Label ID="Label5" runat="server" Text="Esta etapa pode ser reprovada?" AssociatedControlID="rblTeraReprovacao"></asp:Label>
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip8" Chave="botaoReprovacao" />
                <asp:RadioButtonList ID="rblTeraReprovacao" runat="server" RepeatDirection="Horizontal"
                    CssClass="form-control" OnSelectedIndexChanged="rblTeraReprovacao_SelectedIndexChanged"
                    AutoPostBack="true">
                    <asp:ListItem Text="Sim" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Não" Value="0"></asp:ListItem>
                </asp:RadioButtonList>
            </div>

            <div class="panel panel-default" id="pnlBotaoReprovacao" runat="server" visible="false">
                <div class="panel-heading">
                    <h3 class="panel-title" style="margin-bottom: 0">Botão de reprovação
                    </h3>
                </div>
                <div class="panel-body">
                    <div class="form-group" runat="server">
                        <asp:Label ID="Label19" runat="server" Text="Nome do botão de reprovação" AssociatedControlID="rblRequerAtivacao"></asp:Label>
                        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip6" Chave="etapaNomeReprovacao" />
                        <asp:DropDownList ID="ddlNomeReprovacaoEtapa" runat="server" CssClass="form-control mostrarload" AutoPostBack="True" OnSelectedIndexChanged="ddlNomeReprovacaoEtapa_OnSelectedIndexChanged"></asp:DropDownList>
                    </div>

                    <div class="form-group" runat="server" id="divNomeReprovacaoEtapaOutros" visible="False">
                        <asp:Label ID="Label20" runat="server" Text="Especifique o nome do botão" AssociatedControlID="txtNomeReprovacaoOutro"></asp:Label>
                        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip7" Chave="nomeReprovacao" />
                        <asp:TextBox ID="txtNomeReprovacaoOutro" runat="server" MaxLength="50" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>

        <div class="form-group">
            <asp:Label ID="Label30" runat="server" Text="Prazo para encaminhamento da demanda" AssociatedControlID="txtPrazoEncaminharDemanda"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip33" Chave="prazoEncaminhamento" />
            <asp:TextBox runat="server" ID="txtPrazoEncaminharDemanda" CssClass="form-control"></asp:TextBox>
        </div>

        <div class="form-group">
            <asp:Label ID="Label14" runat="server" Text="ESTA ETAPA PODERÁ SER IMPRESSA?" AssociatedControlID="rblRequerAtivacao"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip9" Chave="etapaVisivelImpressao" />
            <asp:RadioButtonList ID="rblVisivelImpressao" runat="server" RepeatDirection="Horizontal"
                CssClass="form-control">
                <asp:ListItem Text="Sim" Value="1" Selected="True"></asp:ListItem>
                <asp:ListItem Text="Não" Value="0"></asp:ListItem>
            </asp:RadioButtonList>
        </div>
        <asp:Panel runat="server" ID="pnlLinhaComUploadArquivo" Visible="true">
            <div class="form-group">
                <asp:Label ID="Label15" runat="server" Text="Anexo" AssociatedControlID="fupldArquivoEnvio"></asp:Label>
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip10" Chave="anexo" />
                <asp:FileUpload ID="fupldArquivoEnvio" runat="server" class="form-control" />
            </div>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlDownloadArquivo" Visible="false">
            <div class="form-group">
                <asp:HyperLink ID="lkbArquivo" runat="server">Baixar aquivo</asp:HyperLink>
            </div>
        </asp:Panel>


        <asp:Panel ID="pnlCampos" runat="server" Visible="false">
            <div class="fakeTh">
                Campos
            </div>
            <div class="ordenacaoCamposHidden">
                <asp:HiddenField ID="hdnOrdenacaoCampos" runat="server" />
            </div>
            <ul class="fakeTable CamposContainer">
                <asp:Repeater ID="rptCampos" runat="server" OnItemDataBound="Repeater_OnItemDataBound">
                    <HeaderTemplate>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <li guid="<%# DataBinder.Eval(Container, "DataItem.ID")%>">
                            <div class="ordem" style="display: inline; float: left; margin: 2px 10px; font-size: 11px; color: #237929;">
                                <%# Container.ItemIndex + 1 %>º
                            </div>
                            <%# DataBinder.Eval(Container, "DataItem.Nome")%>
                            <div style="display: inline; float: right">
                                <asp:LinkButton ID="btnAbrirAlternativas" runat="server" CommandArgument="" OnClick="btnAbrirAlternativas_Click">
                                    <span class="btn btn-default btn-xs">
                                    <span class="glyphicon glyphicon-th-list">
                                    </span>
                                    </span>
                                </asp:LinkButton>

                                <asp:LinkButton ID="Editar" runat="server" CommandArgument="" OnClick="EditarCampo_Click">
                                        <span class="btn btn-default btn-xs"><span class="glyphicon glyphicon-pencil"></span>
                                        </span>
                                </asp:LinkButton>
                                <asp:LinkButton ID="Duplicar" runat="server" CausesValidation="False" CommandName="duplicar" OnClick="DuplicarCampo_Click"
                                    ToolTip="Duplicar" OnClientClick="return confirm('Deseja realmente duplicar o registro?');">
                                            <span class="btn btn-default btn-xs">
									            <span class="glyphicon glyphicon-floppy-saved"></span>
								            </span>
                                </asp:LinkButton>
                                <asp:LinkButton ID="Excluir" runat="server" CommandArgument="" OnClick="ExcluirCampo_Click">
                                    <span class="btn btn-default btn-xs">
                                    <span class="glyphicon glyphicon-remove">
                                    </span>
                                </span>
                                </asp:LinkButton>
                            </div>
                        </li>
                    </ItemTemplate>
                    <FooterTemplate>
                        <li id="emptyCampo" runat="server" visible='<%# DataBinder.Eval(Container.Parent, "Items.Count").ToString() == "0" %>'>Sem Campos cadastrados </li>
                    </FooterTemplate>
                </asp:Repeater>
            </ul>
            <div class="form-group">
                <asp:Button ID="AddicionarCampo" runat="server" Text="Adicionar Campo" CssClass="btn btn-primary pull-right mostrarload"
                    OnClick="AddicionarCampo_Click" />
                <div style="clear: both;">
                </div>
            </div>
        </asp:Panel>
    </fieldset>
    <asp:Panel runat="server" ID="pnlPermissoesAnalise" Style="margin: 20px 0;">
        <p class="hide-show form-control" onclick="$('#divPermissaoAnalise').toggle();">
            Usuários que irão analisar esta etapa
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip11" Chave="tipoPermissaoAnalise" />
        </p>
        <div id="divPermissaoAnalise" style="margin-left: 20px;">
            <asp:RadioButtonList runat="server" ID="rblTipoPermissaoAnalise">
                <asp:ListItem Text="Colaborador(es)" Value="Colaborador" Selected="True"></asp:ListItem>
                <asp:ListItem Text="Solicitante" Value="Solicitante"></asp:ListItem>
                <asp:ListItem Text="Chefes Imediatos e Gerentes Adjuntos (Quando houver)" Value="ChefeImediato"></asp:ListItem>
                <asp:ListItem Text="Diretor Correspondente" Value="DiretorCorrespondente"></asp:ListItem>
                <asp:ListItem Text="Núcleos UC" Value="NucleosUC"></asp:ListItem>
            </asp:RadioButtonList>

            <div id="multiplosUsuariosAnalise">
                <uc1:ucLupaMultiplosUsuarios ID="ucMultiplosUsuariosAnalise" runat="server" Chave="multiplosUsuariosAnalise" />
            </div>

            <div id="suplente">
                <div>
                    <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip12" Chave="podeSerAprovadoAssessor" />
                    <asp:CheckBox ID="ckbPodeSerAprovadoChefeGabinete" Text="Pode ser aprovado por Chefe do Gabinete?" runat="server" />
                </div>
                <div>
                    <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip13" Chave="notificaDiretorAnalise" />
                    <asp:CheckBox ID="ckbNotificaDiretorAnalise" Text="Diretor deve ser notificado para analisar?" runat="server" />
                </div>
            </div>

            <div id="nucleosUC">
                <div class="form-group">
                    <asp:Label Text="Núcleos UC" runat="server" />
                    <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip35" Chave="usuariosNucleosUC" />
                    <uc1:ucListBox runat="server" ID="ucListBoxPermissoes" />
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlPermissoesNotificacao" Style="margin: 20px 0;">
        <p class="hide-show form-control" onclick="$('#divPermissaoNotificacao').toggle();">
            Usuários que receberão as notificações após conclusão desta etapa
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip14" Chave="tipoPermissaoNotificacao" />
        </p>
        <div id="divPermissaoNotificacao" style="margin-left: 20px;">
            <asp:CheckBoxList runat="server" ID="cblTipoPermissaoNotificacao">
                <asp:ListItem Text="Colaborador(es)" Value="Colaborador"></asp:ListItem>
                <asp:ListItem Text="Solicitante" Value="Solicitante"></asp:ListItem>
                <asp:ListItem Text="Chefes Imediatos e Gerentes Adjuntos (Quando houver)" Value="ChefeImediato"></asp:ListItem>
                <asp:ListItem Text="Diretor Correspondente" Value="DiretorCorrespondente"></asp:ListItem>
                <asp:ListItem Text="Núcleos UC" Value="NucleosUC"></asp:ListItem>
            </asp:CheckBoxList>

            <div id="multiplosUsuariosNotificacao">
                <uc1:ucLupaMultiplosUsuarios ID="ucMultiplosUsuariosNotificacao" runat="server" Chave="multiplosUsuariosNotificacao" />
            </div>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlPermissoesParticipacao" Style="margin: 20px 0;">
        <p class="hide-show form-control" onclick="$('#divPermissaoParticipacao').toggle();">
            Usuários que irão participar desta etapa
        </p>
        <div id="divPermissaoParticipacao" style="margin-left: 20px;">
            <uc1:ucPermissoes ID="ucPermissoes" runat="server" />
        </div>
    </asp:Panel>
    <div class="form-group">
        <uc:LupaUsuario ID="ucLupaUsuarioAssinatura" runat="server" Chave="usuarioAssinatura" />
        <center>
            <asp:Button ID="btnRemoverUsuario" runat="server" Text="Limpar usuário" OnClick="btnRemover_UsuarioAssinatura"
                CssClass="btn btn-default" />
        </center>
    </div>
    <div class="form-group">
        <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click"
            CssClass="btn btn-primary" />
        <asp:Button ID="btnCancelar" runat="server" Text="Voltar" CssClass="btn btn-default"
            OnClick="btnCancelar_Click" />
    </div>

    <!-- MODAL FORMULARIO CAMPO-->
    <asp:Panel ID="pnlCampoModal" runat="server" Visible="false">
        <div class="modal fade in" id="ModalMatriculaTurma" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
            aria-hidden="true" style="display: block;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button id="Button4" type="button" class="close" data-dismiss="modal" aria-hidden="true"
                            onserverclick="OcultarCampoModal_Click" runat="server">
                            &times;</button>
                        <h4 class="modal-title">Adicionar Campo</h4>
                    </div>
                    <div class="modal-body">
                        <fieldset>
                            <div class="form-group">
                                <asp:Label ID="Label6" runat="server" Text="Nome" AssociatedControlID="ddlNomeCampo"></asp:Label>
                                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip15" Chave="nomeCampoProcesso" />
                                <asp:DropDownList ID="ddlNomeCampo" runat="server" AutoPostBack="true" CssClass="form-control"
                                    OnSelectedIndexChanged="ddlNomeCampo_OnSelectIndexChanged">
                                </asp:DropDownList>
                                <br />
                                <asp:TextBox ID="txtNomeCampo" Visible="false" runat="server" MaxLength="2000" CssClass="form-control"></asp:TextBox>
                            </div>
                            <asp:Panel ID="pnlTamanhoCampo" runat="server">
                                <div class="form-group">
                                    <asp:Label ID="Label8" runat="server" Text="Tamanho" AssociatedControlID="txtTamanhoCampo"></asp:Label>
                                    <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip16" Chave="tamanhoCampoProcesso" />
                                    <asp:TextBox ID="txtTamanhoCampo" runat="server" MaxLength="150" CssClass="form-control"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <div class="form-group">
                                <asp:Label ID="Label7" runat="server" Text="Tipo de Campo" AssociatedControlID="ddlTipoCampo"></asp:Label>
                                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip17" Chave="tipoCampoProcesso" />
                                <asp:DropDownList ID="ddlTipoCampo" runat="server" AutoPostBack="true" CssClass="form-control"
                                    OnSelectedIndexChanged="ddlCampoDoUsuario_OnSelectIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label25" runat="server" Text="Ano de Orçamento para Reembolso" AssociatedControlID="ddlOrcamentoReembolso"></asp:Label>
                                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip36" Chave="campoOrcamentoReembolso" />
                                <asp:DropDownList ID="ddlOrcamentoReembolso" runat="server" AutoPostBack="true" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <asp:Panel ID="pnlQuestionario" runat="server" Visible="false">
                                <div class="form-group">
                                    <asp:Label ID="lblQuestionario" runat="server" Text="Questionário" AssociatedControlID="ddlQuestionario"></asp:Label>
                                    <asp:DropDownList ID="ddlQuestionario" runat="server" CssClass="form-control ays-ignore"></asp:DropDownList>
                                </div>
                            </asp:Panel>

                            <asp:Panel ID="pnlCampoDoUsuario" Visible="false" runat="server">
                                <div class="form-group">
                                    <div class="form-group">
                                        <asp:Label ID="Label13" runat="server" Text="Campo do Usuário" AssociatedControlID="ddlCampoDoUsuario"></asp:Label>
                                        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip18" Chave="campoDoUsuario" />
                                        <asp:DropDownList ID="ddlCampoDoUsuario" runat="server" AutoPostBack="true" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="pnlTipoDado" runat="server">
                                <div class="form-group">
                                    <asp:Label ID="Label9" runat="server" Text="Tipo de Dado" AssociatedControlID="ddlTipoDado"></asp:Label>
                                    <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip19" Chave="tipoDadoProcesso" />
                                    <asp:DropDownList ID="ddlTipoDado" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlTipoDado_OnSelectIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="pnlEtapaCampos" runat="server" Visible="false">
                                <div class="form-group">
                                    <asp:Label ID="Label23" runat="server" Text="Etapa" AssociatedControlID="ddlEtapa"></asp:Label>
                                    <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip31" Chave="etapa" />
                                    <asp:DropDownList ID="ddlEtapa" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlEtapa_OnSelectIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="LblCampos" runat="server" AssociatedControlID="ucListBoxesCamposEtapa" Text="Campos"></asp:Label>
                                    <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip20" Chave="cadastroProfessor" />
                                    <uc1:ucListBox runat="server" ID="ucListBoxesCamposEtapa" DescricaoDisponiveis="Selecione os campos da operação" DescricaoSelecionados="Selecionados" />
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="pnlLargura" runat="server">
                                <div class="form-group">
                                    <asp:Label ID="Label16" runat="server" Text="Largura do campo" AssociatedControlID="hdnLargura"></asp:Label>
                                    <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip21" Chave="larguraCampo" />
                                    <asp:HiddenField ID="hdnLargura" ClientIDMode="Static" runat="server" />

                                    <div class="row">
                                        <div class="demanda-largura-container col-sm-3 col-xs-6">
                                            <div class="list-group" style="margin-bottom: 0;">
                                                <a href="#" class="list-group-item demanda-largura-listitem" data-largura="4" onclick="SelecionarLargura(this)">
                                                    <h4 class="list-group-item-heading text-center demanda-largura-titulo">33%</h4>
                                                    <div class="panel panel-default demanda-largura-panel-container">
                                                        <div class="panel-body demanda-largura-panel-body">
                                                            <div class="row" style="margin: 0; padding: 2px 0;">
                                                                <div class="col-xs-4" style="padding-left: 4px; padding-right: 2px">
                                                                    <span style="display: block; background-color: #428bca; min-height: 20px;"></span>
                                                                </div>
                                                                <div class="col-xs-4" style="padding-left: 2px; padding-right: 2px">
                                                                    <span style="display: block; background-color: #5bc0de; min-height: 20px;"></span>
                                                                </div>
                                                                <div class="col-xs-4" style="padding-left: 2px; padding-right: 4px">
                                                                    <span style="display: block; background-color: #5bc0de; min-height: 20px;"></span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </a>
                                            </div>
                                        </div>

                                        <div class="demanda-largura-container col-sm-3 col-xs-6">
                                            <div class="list-group" style="margin-bottom: 0;">
                                                <a href="#" class="list-group-item demanda-largura-listitem" data-largura="6" onclick="SelecionarLargura(this)">
                                                    <h4 class="list-group-item-heading text-center demanda-largura-titulo">50%</h4>
                                                    <div class="panel panel-default demanda-largura-panel-container">
                                                        <div class="panel-body demanda-largura-panel-body">
                                                            <div class="row" style="margin: 0; padding: 2px 0;">
                                                                <div class="col-xs-6" style="padding-left: 4px; padding-right: 2px">
                                                                    <span style="display: block; background-color: #428bca; min-height: 20px;"></span>
                                                                </div>
                                                                <div class="col-xs-6" style="padding-left: 2px; padding-right: 4px">
                                                                    <span style="display: block; background-color: #5bc0de; min-height: 20px;"></span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </a>
                                            </div>
                                        </div>

                                        <div class="demanda-largura-container col-sm-3 col-xs-6">
                                            <div class="list-group" style="margin-bottom: 0;">
                                                <a href="#" class="list-group-item demanda-largura-listitem" data-largura="8" onclick="SelecionarLargura(this)">
                                                    <h4 class="list-group-item-heading text-center demanda-largura-titulo">66%</h4>
                                                    <div class="panel panel-default demanda-largura-panel-container">
                                                        <div class="panel-body demanda-largura-panel-body">
                                                            <div class="row" style="margin: 0; padding: 2px 0;">
                                                                <div class="col-xs-8" style="padding-left: 4px; padding-right: 2px">
                                                                    <span style="display: block; background-color: #428bca; min-height: 20px;"></span>
                                                                </div>
                                                                <div class="col-xs-4" style="padding-left: 2px; padding-right: 4px">
                                                                    <span style="display: block; background-color: #5bc0de; min-height: 20px;"></span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </a>
                                            </div>
                                        </div>

                                        <div class="demanda-largura-container col-sm-3 col-xs-6">
                                            <div class="list-group" style="margin-bottom: 0;">
                                                <a href="#" class="list-group-item demanda-largura-listitem" data-largura="12" onclick="SelecionarLargura(this)">
                                                    <h4 class="list-group-item-heading text-center demanda-largura-titulo">100%</h4>
                                                    <div class="panel panel-default demanda-largura-panel-container">
                                                        <div class="panel-body demanda-largura-panel-body">
                                                            <div class="row" style="margin: 0; padding: 2px 0;">
                                                                <div class="col-xs-12" style="padding-left: 4px; padding-right: 4px">
                                                                    <span style="display: block; background-color: #428bca; min-height: 20px;"></span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <div class="row">
                                <asp:Panel ID="pnlPermitirNulo" runat="server">
                                    <div class="col-md-4 col-xs-12">
                                        <div class="form-group">
                                            <asp:Label ID="Label10" runat="server" Text="Permitir nulo?" AssociatedControlID="rdbPermitirNulo"></asp:Label>
                                            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip22" Chave="permitirNuloCampo" />
                                            <asp:RadioButtonList ID="rdbPermitirNulo"
                                                runat="server" RepeatDirection="Horizontal" CssClass="form-control">
                                                <asp:ListItem Text="Sim" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Não" Selected="True" Value="0"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <div class="col-md-4 col-xs-12">
                                    <div class="form-group">
                                        <asp:Label ID="Label21" runat="server" Text="Exibir campo na impressão?" AssociatedControlID="rdbExibirImpressao"></asp:Label>
                                        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip23" Chave="exibirCampoNaImpressao" />
                                        <asp:RadioButtonList ID="rdbExibirImpressao"
                                            runat="server" RepeatDirection="Horizontal" CssClass="form-control" OnSelectedIndexChanged="rdbExibirImpressao_OnSelectedIndexChanged" AutoPostBack="True">
                                            <asp:ListItem Text="Sim" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Não" Value="0"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                                <div class="col-md-4 col-xs-12" runat="server" id="divExibirAjudaNaImpressao">
                                    <div class="form-group">
                                        <asp:Label ID="Label22" runat="server" Text="Exibir ajuda na impressão?" AssociatedControlID="rdbExibirAjudaImpressao"></asp:Label>
                                        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip24" Chave="exibirAjudaNaImpressao" />
                                        <asp:RadioButtonList ID="rdbExibirAjudaImpressao"
                                            runat="server" RepeatDirection="Horizontal" CssClass="form-control">
                                            <asp:ListItem Text="Sim" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Não" Value="0"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                            </div>


                            <asp:Repeater ID="rptMetaFields" runat="server">
                                <HeaderTemplate>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div class="form-group">
                                        <asp:Label ID="metaFieldLabel" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.MetaNome")%>' data-help='<%# DataBinder.Eval(Container, "DataItem.MetaDescription")%>' AssociatedControlID='metaField'></asp:Label>
                                        <asp:TextBox ID="metaField" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FirstMetaValue")%>' MaxLength="150" CssClass="form-control"></asp:TextBox>
                                        <asp:HiddenField ID="metaFieldId" Value='<%# DataBinder.Eval(Container, "DataItem.ID")%>' runat="server" />
                                        <asp:HiddenField ID="metaFieldValueId" Value='<%# DataBinder.Eval(Container, "DataItem.FirstMetaValueId")%>' runat="server" />
                                    </div>
                                </ItemTemplate>
                                <FooterTemplate>
                                </FooterTemplate>
                            </asp:Repeater>

                            <asp:Panel ID="pnlAjuda" runat="server">
                                <div class="form-group">
                                    <asp:Label ID="labelConteudoHtml" runat="server" Text="Conteúdo HTML" AssociatedControlID="txtAjuda" Visible="false" />
                                    <uc1:ucHelperTooltip runat="server" ID="ucHelperTooltipAjudaHtml" Chave="ajudaHtml" Visible="False" />
                                    <asp:Label ID="labelAjuda" runat="server" Text="Ajuda" AssociatedControlID="txtAjuda" />
                                    <uc1:ucHelperTooltip runat="server" ID="ucHelperTooltipAjuda" Chave="campoTextoAjuda" />
                                    <CKEditor:CKEditorControl ID="txtAjuda" Height="50" BasePath="/js/ckeditor/" runat="server"></CKEditor:CKEditorControl>
                                </div>
                            </asp:Panel>

                            <asp:Panel ID="pnlSomatorio" runat="server" Visible="false">
                                <div class="form form-inline">
                                    <div class="form-group">
                                        <asp:Label ID="lblSomatorio" runat="server">Selecione os campos da operação: </asp:Label>
                                        <uc1:ucHelperTooltip runat="server" ID="ucHelperTooltip25" Chave="somatorio" />
                                        <asp:CheckBoxList ID="chkLSomatorio" runat="server">
                                        </asp:CheckBoxList>
                                    </div>
                                </div>
                            </asp:Panel>

                            <asp:Panel runat="server" ID="pnlDivisao" Visible="false">
                                <div class="form-inline">
                                    <div class="form-group">
                                        <asp:Label runat="server">Campo Divisor:</asp:Label>
                                        <uc1:ucHelperTooltip runat="server" ID="ucHelperTooltip26" Chave="campoDivisor" />
                                        <asp:DropDownList runat="server" ID="ddlCampoDivisor" />
                                    </div>
                                    <div class="form-group">
                                        <div class="form-group">
                                            <asp:Label runat="server">Campo Dividendo:</asp:Label>
                                            <uc1:ucHelperTooltip runat="server" ID="ucHelperTooltip27" Chave="campoDividendo" />
                                            <asp:DropDownList runat="server" ID="ddlCampoDividendo" />
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <hr />
                            <div class="form-group">
                                <asp:Button ID="btnSalvarCampo" OnClick="btnSalvarCampo_OnClick" runat="server" Text="Salvar"
                                    CssClass="btn btn-primary" />
                                <asp:Button ID="btnCancelarCampo" OnClick="OcultarCampoModal_Click" runat="server"
                                    Text="Cancelar" CssClass="btn btn-default" />
                            </div>
                            <asp:HiddenField ID="CampoEdicaoId" runat="server" />
                        </fieldset>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>

    <!-- MODAL FORMULARIO ALTERNATIVAS-->
    <asp:Panel ID="pnlModalAlternativas" runat="server" Visible="false">
        <div class="modal fade in" id="Div1" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
            aria-hidden="true" style="display: block;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button id="Button2" type="button" class="close" data-dismiss="modal" aria-hidden="true"
                            onserverclick="OcultarAlternativaModal_Click" runat="server">
                            &times;</button>
                        <h4 class="modal-title">Alternativas do Campo</h4>
                    </div>
                    <div class="modal-body">
                        <fieldset>
                            <div class="form-group">
                                <asp:Label ID="Label12" runat="server" Text="Campo" AssociatedControlID="txtNomeAlternativaCampo"></asp:Label>
                                <uc1:ucHelperTooltip runat="server" ID="ucHelperTooltip28" Chave="nomeAlternativoCampo" />
                                <asp:TextBox ID="txtNomeAlternativaCampo" Enabled="false" runat="server" MaxLength="150"
                                    CssClass="form-control"></asp:TextBox>
                            </div>
                            <asp:HiddenField ID="hdnCampoAtivo" runat="server" />
                            <asp:Panel ID="pnlAlternativas" runat="server" Visible="true">
                                <div class="fakeTh">
                                    Alternativas
                                </div>
                                <div class="ordenacaoAlternativasHidden">
                                    <asp:HiddenField ID="hdnOrdenacaoAlternativas" runat="server" />
                                </div>
                                <ul class="fakeTable AlternativasContainer">
                                    <asp:Repeater ID="rptAlternativas" runat="server" OnItemDataBound="rptAlternativa_OnItemDataBound">
                                        <HeaderTemplate>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <li guid="<%# DataBinder.Eval(Container, "DataItem.ID")%>">
                                                <div class="ordem" style="display: inline; float: left; margin: 2px 10px; font-size: 11px; color: #237929;">
                                                    <%# Container.ItemIndex + 1 %>º
                                                </div>
                                                <%# DataBinder.Eval(Container, "DataItem.Nome")%>
                                                <div style="display: inline; float: right">
                                                    <asp:LinkButton ID="Editar" runat="server" CommandArgument="" OnClick="EditarAlternativa_Click">
                                                        <span class="btn btn-default btn-xs">
                                                            <span class="glyphicon glyphicon-pencil">
                                                            </span>
                                                        </span>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="Excluir" runat="server" CommandArgument="" OnClick="ExcluirAlternativa_Click"
                                                        OnClientClick="return ConfirmarExclusao();" CssClass="mostrarload">
                                                        <span class="btn btn-default btn-xs">
                                                            <span class="glyphicon glyphicon-remove">
                                                            </span>
                                                        </span>
                                                    </asp:LinkButton>
                                                </div>
                                            </li>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <li id="Li1" runat="server" visible='<%# DataBinder.Eval(Container.Parent, "Items.Count").ToString() == "0" %>'>Sem Alternativas cadastrados </li>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </ul>
                                <div class="form-group">
                                    <asp:Button ID="btnSalvarOrdenacao" runat="server" Text="Salvar Ordenação" CssClass="btn btn-primary pull-right"
                                        OnClick="btnSalvarOrdenacao_Click" />
                                    <div style="clear: both;">
                                    </div>
                                </div>
                                <br />
                                <h3 runat="server">
                                    <asp:Literal ID="ltAlternativaAcao" runat="server">
                                        Adicionar Alternativa
                                    </asp:Literal>
                                </h3>
                                <div class="form-group">
                                    <asp:Label ID="Label11" runat="server" Text="Nome" AssociatedControlID="txtNomeAlternativa"></asp:Label>
                                    <uc1:ucHelperTooltip runat="server" ID="ucHelperTooltip29" Chave="nomeAlternativa" />
                                    <asp:TextBox ID="txtNomeAlternativa" runat="server" MaxLength="150" CssClass="form-control"></asp:TextBox>
                                    <asp:HiddenField ID="hdnAlternativaIdEdicao" Value="0" runat="server" />
                                </div>
                                <div class="form-group">
                                    <asp:Label Text="Campo vinculado" runat="server" />
                                    <uc1:ucHelperTooltip runat="server" ID="ucHelperTooltip34" Chave="campoVinculado" />
                                    <asp:DropDownList ID="ddlCampoVinculado" runat="server"></asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <div class="pull-right">
                                        <asp:Button ID="btnSalvarAlternativa" runat="server" Text="Salvar Alternativa" CssClass="btn btn-primary "
                                            OnClick="SalvarAlternativa_Click" />
                                        <asp:Button ID="btnCancelarAlternativa" runat="server" Text="Voltar" CssClass="btn btn-default"
                                            OnClick="btnCancelarAlternativa_Click" />
                                        <div style="clear: both;">
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:HiddenField ID="hdnAlternativaId" runat="server" />
                        </fieldset>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <!-- MODAL EXCLUIR CAMPO-->
    <asp:Panel ID="pnlModalExcluirCampo" runat="server" Visible="false">
        <div class="modal fade in" id="ModalExcluirCampo" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
            aria-hidden="true" style="display: block;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button id="btnFecharModalExcluirCampo" type="button" class="close" data-dismiss="modal" aria-hidden="true"
                            onserverclick="btnFecharModalExcluirCampo_OnServerClick" runat="server">
                            &times;</button>
                        <h4 class="modal-title">Excluir Campo</h4>
                    </div>
                    <div class="modal-body">

                        <asp:HiddenField ID="hdnExcluirCampo_IdCampo" runat="server" Value="0" />


                        <div class="form-group">
                            <div class="alert alert-danger text-center" role="alert">
                                <h3 style="margin-top: 0; margin-bottom: 5px;">
                                    <strong>Atenção!</strong>
                                </h3>
                                Ao excluir este campo, você também excluirá todas as dependências listadas abaixo, podendo, inclusive, comprometer o fluxo de processos já respondidos por usuários.
                                <br />
                                <strong>Esta operação é irreversível
                                </strong>
                            </div>
                        </div>

                        <div class="form-group">
                            <h3>Dados do campo
                            </h3>
                        </div>

                        <div class="panel-group" id="accordion">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <a data-toggle="collapse" data-parent="#accordion" href="#collapse1">Dados do campo</a>
                                </div>
                                <div id="collapse1" class="panel-collapse collapse in">
                                    <div class="panel-body">
                                        <div class="row text-center">
                                            <div class="col-md-3 col-xs-12">
                                                <strong>Nome</strong>
                                                <br />
                                                <asp:Literal ID="ltrExcluirCampo_Nome" runat="server"></asp:Literal>
                                            </div>
                                            <div class="col-md-3 col-xs-3">
                                                <strong>Tipo</strong>
                                                <br />
                                                <asp:Literal ID="ltrExcluirCampo_Tipo" runat="server"></asp:Literal>
                                            </div>
                                            <div class="col-md-2 col-xs-3">
                                                <strong>Obrigatório</strong>
                                                <br />
                                                <asp:Literal ID="ltrExcluirCampo_Obrigatorio" runat="server"></asp:Literal>
                                            </div>
                                            <div class="col-md-2 col-xs-3">
                                                <strong>Tamanho</strong>
                                                <br />
                                                <asp:Literal ID="ltrExcluirCampo_Tamanho" runat="server"></asp:Literal>
                                            </div>
                                            <div class="col-md-2 col-xs-3">
                                                <strong>Largura</strong>
                                                <br />
                                                <asp:Literal ID="ltrExcluirCampo_Largura" runat="server"></asp:Literal>%
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <a data-toggle="collapse" data-parent="#accordion" href="#collapse3">Detalhes das dependências
                                        <label class="badge">
                                            <asp:Literal ID="ltrExcluirCampo_ContadorDependencias" runat="server"></asp:Literal></label>
                                    </a>
                                </div>
                                <div id="collapse3" class="panel-collapse collapse">
                                    <div class="panel-body">
                                        <asp:GridView ID="gdvExcluirCampo_Dependencias" runat="server" AutoGenerateColumns="False" GridLines="None" CssClass="table table-striped table-condensed" EmptyDataText="Não há dependências para este campo">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Demandante">
                                                    <ItemTemplate>
                                                        <small>
                                                            <%#Eval("EtapaResposta.ProcessoResposta.Usuario.Nome") %> (<%#Eval("EtapaResposta.ProcessoResposta.Usuario.CPF") %>)
                                                        </small>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Início">
                                                    <ItemTemplate>
                                                        <small>
                                                            <%# ((DateTime)Eval("EtapaResposta.ProcessoResposta.DataSolicitacao")).ToShortDateString() %>
                                                        </small>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Resposta">
                                                    <ItemTemplate>
                                                        <button type="button" class="btn btn-block btn-sm btn-link" data-trigger="hover" data-toggle="popover" data-html="true" data-content="<small><%#
    Eval("Campo") != null && Eval("Campo.TipoCampo") != null && int.Parse(Eval("Campo.TipoCampo").ToString()) == 9 ? "Dados do usuário" :
    (Eval("Campo") != null && Eval("Campo.TipoCampo") != null && int.Parse(Eval("Campo.TipoCampo").ToString()) == 10
        ? (Eval("Campo.Ajuda") == null || Eval("Campo.Ajuda").ToString() == "" ? "(Não informado)" : Eval("Campo.Ajuda"))
        : (Eval("Resposta") == null || Eval("Resposta").ToString() == "" ? "(Não informado)" : Eval("Resposta"))) %></small>">
                                                            <span class="glyphicon glyphicon-search"></span>&nbsp;&nbsp;Ver resposta
                                                        </button>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Usuário">
                                                    <ItemTemplate>
                                                        <small>
                                                            <%# Eval("EtapaResposta.Analista") != null ? Eval("EtapaResposta.Analista.Nome") : "N/D" %>
                                                        </small>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Situação da etapa">
                                                    <ItemTemplate>
                                                        <small>
                                                            <%#Eval("EtapaResposta.ObterSituacao") %>
                                                        </small>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <h3>Confirmação de exclusão
                        </h3>
                        <div class="form-group">
                            <div class="panel panel-default">
                                <div class="panel-body text-center">
                                    <asp:CheckBox ID="ckbConfirmacaoExclusaoCampo" ClientIDMode="Static" AutoPostBack="True" OnCheckedChanged="ckbConfirmacaoExclusaoCampo_OnCheckedChanged" runat="server" Text="&nbsp;&nbsp;Estou ciente que esta operação é irreversível e desejo remover permanentemente os dados citados acima." />
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Button ID="btnRemoverCampo" Enabled="False" CssClass="btn btn-danger btn-block" runat="server" Text="REMOVER CAMPO PERMANENTEMENTE" ClientIDMode="Static" OnClick="btnRemoverCampo_OnClick" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <script type="text/javascript">
        $(function () {
            $('[data-toggle="popover"]').popover();

            var larguraSelecionada = $('#hdnLargura').val();

            if (larguraSelecionada != undefined && larguraSelecionada !== '')
                $('[data-largura="' + larguraSelecionada + '"]').addClass('active');

            $('#<%= rblTipoPermissaoAnalise.ClientID %> input[type="radio"]').change(function () {
                var selecionado = $('#<%= rblTipoPermissaoAnalise.ClientID %>').find(':checked').val();

                if (selecionado == "Colaborador") {
                    $('#multiplosUsuariosAnalise').show();
                } else {
                    $('#multiplosUsuariosAnalise').hide();
                }

                if (selecionado == "DiretorCorrespondente") {
                    $('#suplente').show();
                } else {
                    $('#suplente').hide();
                }

                if (selecionado == "NucleosUC") {
                    $('#nucleosUC').show();
                } else {
                    $('#nucleosUC').hide();
                }
            });
            $('#<%= rblTipoPermissaoAnalise.ClientID %> input[type="radio"]').change();

            $('#<%= cblTipoPermissaoNotificacao.ClientID %> input[type="checkbox"]').change(function () {
                var colaboradorChecked = $('#<%= cblTipoPermissaoNotificacao.ClientID %>').find('input[value="Colaborador"]:checked').size();
                var nucleosSelecionado = $('#<%= cblTipoPermissaoNotificacao.ClientID %>').find('input[value="NucleosUC"]:checked').size();

                if (colaboradorChecked > 0) {
                    $('#multiplosUsuariosNotificacao').show();
                } else {
                    $('#multiplosUsuariosNotificacao').hide();
                }

                if (nucleosSelecionado > 0) {
                    $('#nucleosUCNotificacao').show();
                } else {
                    $('#nucleosUCNotificacao').hide();
                }
            });
            $('#<%= cblTipoPermissaoNotificacao.ClientID %> input[type="checkbox"]').change();
        });

        validarForm();

        function SelecionarLargura(element) {
            $.each(document.getElementsByClassName('demanda-largura-listitem'), function () {
                $(this).removeClass('active');
            });

            $(element).addClass('active');

            $('#hdnLargura').val(element.dataset.largura);
        }
    </script>
</asp:Content>
