<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="ListarNotificacaoEnvio.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.NotificacaoEnvio.ListarNotificacaoEnvio" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <div class="form-group">
            <asp:Label ID="Label2" runat="server" Text="Buscar por texto" AssociatedControlID="txtNome"></asp:Label>
            <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label1" runat="server" Text="Buscar por link" AssociatedControlID="txtLink"></asp:Label>
            <asp:TextBox ID="txtLink" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click"
                CssClass="btn btn-primary mostrarload" />
            &nbsp;<asp:Button ID="btnNovo" runat="server" Text="Criar nova" OnClick="btnNovo_Click"
                CssClass="btn btn-default mostrarload" />
        </div>
    </fieldset>
    <asp:Panel ID="pnlNotificacaoEnvio" runat="server" Visible="false">
        <hr />
        <h4>Resultado da Busca</h4>
        <asp:Literal ID="litTable" runat="server" />
        <asp:GridView ID="dgvNotificacaoEnvio" runat="server" CssClass="table col-sm-12" GridLines="None"
            OnRowCommand="dgvNotificacaoEnvio_RowCommand" OnRowCreated="dgvNotificacaoEnvio_RowCreated" AutoGenerateColumns="false">
            <Columns>
                <asp:TemplateField HeaderText="Texto">
                    <ItemTemplate>
                        <%#Eval("Texto") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Link">
                    <ItemTemplate>
                        <%#Eval("Link") %>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Uf">
                    <ItemTemplate>
                        <%#Eval("Uf.Nome") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <ItemStyle Width="90px" HorizontalAlign="center" />
                    <ItemTemplate>
                         <asp:LinkButton ID="lkbRelatorio" runat="server" CausesValidation="False" CommandName="relatorio" ToolTip="Relatório de Envio"
                            CommandArgument='<%# Eval("ID")%>' CssClass="mostrarload">
                            <span class="btn btn-default btn-xs">
								<span class="glyphicon glyphicon-list-alt mostrarload"></span>
							</span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="lkbEditar" runat="server" CausesValidation="False" CommandName="editar" ToolTip="Editar"
                            CommandArgument='<%# Eval("ID")%>' CssClass="mostrarload">
                            <span class="btn btn-default btn-xs">
								<span class="glyphicon glyphicon-pencil mostrarload"></span>
							</span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="lkbExcluir" runat="server" CausesValidation="False" CommandName="excluir" ToolTip="Excluir"
                            CommandArgument='<%# Eval("ID")%>' OnClientClick="return ConfirmarExclusao();">
                            <span class="btn btn-default btn-xs">
								<span class="glyphicon glyphicon-remove"></span>
							</span>
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
            </EmptyDataTemplate>
        </asp:GridView>
    </asp:Panel>
</asp:Content>
