<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.Master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeBehind="ListarInformes.aspx.cs"
    Inherits="Sebrae.Academico.WebForms.Cadastros.Informe.ListarInformes" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <fieldset>
            <div class="form-group">
                <asp:Label ID="Label2" runat="server" Text="Buscar por nome" AssociatedControlID="txtNome"></asp:Label>
                <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_OnClick"
                    CssClass="btn btn-primary mostrarload" />
                <asp:Button ID="btnNovo" runat="server" Text="Criar nova" OnClick="btnNovo_OnClick" CssClass="btn btn-default mostrarload" />
            </div>
        </fieldset>
    </div>
    <hr />
    <asp:GridView ID="dgvInformes" runat="server" AutoGenerateColumns="false"
        CssClass="table col-sm-12" GridLines="None" OnRowCommand="dgvInformes_OnRowCommand">
        <Columns>
            <asp:TemplateField HeaderText="Número do informe">
                <ItemTemplate>
                    <%# Eval("Numero") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Data de envio">
                <ItemTemplate>
                    <%# Eval("UltimoEnvio") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <ItemStyle Width="90px" HorizontalAlign="center" />
                <ItemTemplate>
                    <asp:LinkButton ID="lkbEditar" runat="server" CausesValidation="False" CommandName="editar" ToolTip="Editar"
                        CommandArgument='<%# Eval("ID")%>' CssClass="mostrarload btn btn-default btn-xs">
									<span class="glyphicon glyphicon-pencil"></span>
                    </asp:LinkButton>
                    <asp:LinkButton ID="lkbExcluir" runat="server" CausesValidation="False" CommandName="excluir" ToolTip="Excluir"
                        CommandArgument='<%# Eval("ID")%>' OnClientClick="return ConfirmarExclusao();" CssClass="btn btn-default btn-xs">
									<span class="glyphicon glyphicon-remove"></span>
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
        </EmptyDataTemplate>
    </asp:GridView>
</asp:Content>
