<%@ Page Title="" Language="C#" MasterPageFile="~/Cadastro.Master" AutoEventWireup="true"
    EnableViewState="true" EnableEventValidation="false" ValidateRequest="false"
    MaintainScrollPositionOnPostback="true" ViewStateMode="Enabled" CodeBehind="EdicaoQuestionario.aspx.cs"
    Inherits="Sebrae.Academico.WebForms.Cadastros.EdicaoQuestionario" %>

<%@ Register Src="~/UserControls/ucQuestoes.ascx" TagName="ucQuestoes" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h3>
        Edição de Questionário</h3>
    <div class="form-group">
        <asp:Label ID="Label2" runat="server" Text="Tipo de Questionário:" AssociatedControlID="ddlTipoQuestionario" />
        <asp:DropDownList ID="ddlTipoQuestionario" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoQuestionario_SelectedIndexChanged"
            CssClass="form-control mostrarload">
        </asp:DropDownList>
    </div>
    <asp:Panel ID="pnlOpcoesQuestionario" runat="server" Visible="false">
        <div class="panel-group" id="accordionGM">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <asp:LinkButton runat="server" ClientIDMode="Static" ID="lkbDadosQuestionario" OnClick="lkbDadosQuestionario_Click" CssClass="mostrarload"
                        Text="Dados Questionário"></asp:LinkButton>
                </div>
                <div id="collapseDados" class="panel-collapse collapse" runat="server" clientidmode="Static">
                    <div class="panel-body">
                        <div>
                            <div class="form-group">
                                <asp:Label ID="Label1" runat="server" Text="Nome do Questionário:" AssociatedControlID="txtNome" />
                                <asp:TextBox ID="txtNome" runat="server" CssClass="form-control" MaxLength="200"></asp:TextBox>
                            </div>
                            <div class="form-group" id="divPrazoemMinutos" runat="server">
                                <asp:Label ID="Label3" runat="server" Text="Prazo em Minutos:" AssociatedControlID="txtPrazoemMinutos" />
                                <asp:TextBox ID="txtPrazoemMinutos" runat="server" CssClass="form-control" MaxLength="3"
                                    onkeypress="return EhNumerico(event)"></asp:TextBox>
                            </div>
                            <div class="form-group" runat="server" id="divQtdQuestoesDaProva">
                                <asp:Label ID="Label4" runat="server" Text="Qtd de Questões da Prova:" AssociatedControlID="txtQtdQuestoesDaProva" />
                                <asp:TextBox ID="txtQtdQuestoesDaProva" runat="server" CssClass="form-control" MaxLength="3"
                                    onkeypress="return EhNumerico(event)"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label5" runat="server" Text="Enunciado:" AssociatedControlID="txtTextoEnunciadoPre" />
                                <CKEditor:CKEditorControl ID="txtTextoEnunciadoPre" BasePath="/js/ckeditor/" runat="server"></CKEditor:CKEditorControl>
                            </div>
                            <br />
                        </div>
                    </div>
                </div>
            </div>
            <div class="panel panel-default">
                <div class="panel-heading">
                    <asp:LinkButton ID="lkbQuestoes" runat="server" OnClick="lkbQuestoes_Click" CssClass="mostrarload"
                        ClientIDMode="Static" Text="Questões Cadastradas"></asp:LinkButton>
                </div>
                <div id="collapseQuestoes" class="panel-collapse collapse" runat="server"
                    clientidmode="Static">
                    <div class="panel-body">
                        <asp:GridView ID="dgvItensDoQuestionario" runat="server" OnRowCommand="dgvItensDoQuestionario_RowCommand"
                            CssClass="table col-sm-12" GridLines="None" AutoGenerateColumns="false">
                            <Columns>
                                <asp:TemplateField HeaderText="Tipo">
                                    <ItemTemplate>
                                        <%#Eval("TipoItemQuestionario.Nome")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Item do Questionário">
                                    <ItemTemplate>
                                        <%#Eval("Questao")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemStyle Width="90px" HorizontalAlign="center" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lkbEditar" runat="server" CausesValidation="False" CommandName="editar" CssClass="mostrarload"
                                            CommandArgument='<%#Eval("Id") + "|" + Eval("IdLogico")%>' Text="Editar">
                                                <span class="btn btn-default btn-xs">
								                    <span class="glyphicon glyphicon-pencil"></span>
							                    </span>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="lkbExcluir" runat="server" CausesValidation="False" CommandName="excluir"
                                            CommandArgument='<%#Eval("Id") + "|" + Eval("IdLogico")%>' Text="Excluir" OnClientClick="return confirm('Deseja Realmente Excluir este Item deste Questionário ?');">
                                                <span class="btn btn-default btn-xs">
								                    <span class="glyphicon glyphicon-remove"></span>
							                    </span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <itemtemplate>Nenhuma questão cadastrada</itemtemplate>
                            </EmptyDataTemplate>
                        </asp:GridView>
                        <asp:Button ID="btnAdicionarItem" runat="server" OnClick="btnAdicionarItem_Click"
                            UseSubmitBehavior="False" Text="Incluir Questão" CssClass="btn btn-default mostrarload" />
                    </div>
                </div>
            </div>
        </div>
        <br />
        <br />
        <div>
            <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click"
                CssClass="btn btn-default mostrarload" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click"
                OnClientClick="return confirm('Deseja Realmente Cancelar o Cadastro deste Questionário ? Todos os dados não salvos serão perdidos');"
                CssClass="btn btn-default" />
        </div>
        <!-- MODAL -->
        <asp:Panel ID="pnlModal" runat="server" Visible="false">
            <div aria-hidden="false" aria-labelledby="myModalLabel" role="dialog" tabindex="-1"
                id="myModal" class="modal fade in" style="display: block;">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true" runat="server"
                                onserverclick="OcultarModal_Click" id="btnFecharModal">
                                &times;</button>
                            <h4 class="modal-title">
                                Item do Questionário</h4>
                        </div>
                        <div class="modal-body">
                            <uc1:ucQuestoes ID="ucQuestoes1" runat="server" OnAdicionouItemNoQuestionario="ItemQuestionario_AdicionouItemAoQuestionario"
                                OnCancelouAdicaoDeItemNoQuestionario="ItemQuestionario_CancelouAdicaoDeItemNoQuestionario" />
                        </div>
                    </div>
                    <!-- /.modal-content -->
                </div>
                <!-- /.modal-dialog -->
            </div>
            <!-- /.modal -->
        </asp:Panel>
    </asp:Panel>
</asp:Content>
