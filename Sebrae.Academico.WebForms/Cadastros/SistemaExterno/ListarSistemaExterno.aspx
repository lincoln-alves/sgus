<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="ListarSistemaExterno.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.ListarSistemaExterno"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <div class="form-group">
            <asp:Label ID="lblNome" runat="server" Text="Buscar por nome" AssociatedControlID="txtNome" />
            <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click"
                CssClass="btn btn-primary mostrarload" />
            <asp:Button ID="btnNovo" runat="server" Text="Criar novo" OnClick="btnNovo_Click" CssClass="btn btn-default mostrarload" />
        </div>
    </fieldset>
    <hr />
    <asp:Panel ID="pnlSistemaExterno" runat="server" Visible="false">
        <p>
            <b>Resultado da Busca</b>
        </p>
        <asp:Literal ID="litTable" runat="server" />
        <asp:GridView ID="dgvSistemaExterno" runat="server" CssClass="table col-sm-12" GridLines="None"
            OnRowCommand="dgvSistemaExterno_RowCommand" AutoGenerateColumns="false" EnableModelValidation="True">
            <Columns>
                <asp:TemplateField HeaderText="ID">
                    <ItemTemplate>
                        <%#Eval("ID") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Nome">
                    <ItemTemplate>
                        <%#Eval("Nome") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Link">
                    <ItemTemplate>
                        <%#Eval("LinkSistemaExterno") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <ItemStyle Width="90px" HorizontalAlign="center" />
                    <ItemTemplate>
                        <asp:LinkButton ID="lkbEditar" runat="server" CausesValidation="False" CommandName="editar" ToolTip="Editar"
                            CommandArgument='<%# Eval("ID")%>' CssClass="mostrarload">
                                <span class="btn btn-default btn-xs">
								    <span class="glyphicon glyphicon-pencil"></span>
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
