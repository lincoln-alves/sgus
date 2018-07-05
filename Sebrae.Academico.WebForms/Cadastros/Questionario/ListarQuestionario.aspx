<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="ListarQuestionario.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.ListarQuestionario" MaintainScrollPositionOnPostback="true" %>
<%@ Register Src="~/UserControls/ucSeletorListBox.ascx" TagName="ucSeletorListBox"TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <div class="form-group">
            <asp:Label ID="Label2" runat="server" Text="Buscar por nome" AssociatedControlID="txtNome" />
            <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label1" runat="server" Text="UF" AssociatedControlID="ListBoxesUF" />
            <uc1:ucSeletorListBox runat="server" ID="ListBoxesUF" />
        </div>
        <div class="form-group">
            <asp:Label ID="Label3" runat="server" Text="Tipo de Questionário" AssociatedControlID="ListBoxesTipoQuestionario" />
            <uc1:ucSeletorListBox runat="server" ID="ListBoxesTipoQuestionario" />
        </div>
        <div class="form-group">
            <asp:Label ID="Label4" runat="server" Text="Status" AssociatedControlID="ddlStatus" />
            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                <asp:ListItem Value="0" Text="Selecionar Todos" />
                <asp:ListItem Value="1" Text="Ativo" />
                <asp:ListItem Value="2" Text="Inativo" />
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click"
                CssClass="btn btn-primary mostrarload" />
            <asp:Button ID="btnNovo" runat="server" Text="Criar novo" OnClick="btnNovo_Click" CssClass="btn btn-default mostrarload" />
        </div>
    </fieldset>
    <hr />
    <asp:Panel ID="pnlQuestionario" runat="server" Visible="false">
        <div>
            <h4>Resultado da Busca</h4>

            <asp:GridView ID="dgvQuestionarios" runat="server" OnRowCommand="dgvQuestionarios_RowCommand" AutoGenerateColumns="false"
                CssClass="table col-sm-12" GridLines="None" OnRowDataBound="dgvQuestionarios_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="Questionário">
                        <ItemTemplate>
                            <%#Eval("Nome") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tipo">
                        <ItemTemplate>
                            <%#Eval("TipoQuestionario.Nome")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="UF">
                        <ItemStyle Width="60px" HorizontalAlign="center" />
                        <ItemTemplate>
                            <%# Eval("Uf") != null ? Eval("Uf.Sigla") : "" %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False">
                        <ItemStyle Width="120px" HorizontalAlign="center" />
                        <ItemTemplate>
                            <asp:LinkButton ID="lkbEditar" runat="server" CausesValidation="False" CommandName="editar" CommandArgument='<%# Eval("ID")%>' CssClass="mostrarload btn btn-default btn-xs" ToolTip="Editar">
							    <span class="glyphicon glyphicon-pencil"></span>
                            </asp:LinkButton>
                            <asp:LinkButton ID="lkbExcluir" runat="server" CausesValidation="False" CommandName="excluir" CommandArgument='<%# Eval("ID")%>' CssClass="btn btn-default btn-xs" OnClientClick="return ConfirmarExclusao();" ToolTip="Excluir">
							    <span class="glyphicon glyphicon-remove"></span>
                            </asp:LinkButton>
                            <asp:LinkButton ID="lkbDuplicar" runat="server" CausesValidation="False" CommandName="duplicar" CommandArgument='<%# Eval("ID")%>' CssClass="mostrarload btn btn-default btn-xs" ToolTip="Duplicar">
							    <span class="glyphicon glyphicon-floppy-saved"></span>
                            </asp:LinkButton>
                            <asp:LinkButton ID="lkbVisualizar" runat="server" CausesValidation="False" CommandName="visualizar" CommandArgument='<%# Eval("ID")%>' CssClass="mostrarload btn btn-default btn-xs" ToolTip="Visualizar">
							    <span class="glyphicon glyphicon-search"></span>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
                </EmptyDataTemplate>
            </asp:GridView>
        </div>
    </asp:Panel>

    <asp:Panel runat="server" ID="pnlVisualizar" Visible="False">
        <div class="modal fade in" style="display: block;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" runat="server" class="close mostrarload" data-dismiss="modal" aria-label="Close" OnServerClick="FecharDetalhesQuestionario_Click"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Visualização do questionario
                            <asp:Literal ID="ltrQuestionario" runat="server"></asp:Literal></h4>
                    </div>
                    <div class="modal-body">
                        
                        <p>
                            <strong>
                                <asp:Literal ID="ltrNome" runat="server"></asp:Literal>
                            </strong>
                        </p>
                        
                        <p>
                            Categoria(s): <asp:Literal ID="ltrCategoria" runat="server"></asp:Literal>
                        </p>

                        <div runat="server" ID="divDetalhesQuestoes">
                            
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" runat="server" onserverclick="FecharDetalhesQuestionario_Click" class="mostrarload btn btn-primary">Fechar</button>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <script>
        $.removeCookie('questionarioSessao', { path: '/Cadastros/Questionario' });
    </script>
</asp:Content>
