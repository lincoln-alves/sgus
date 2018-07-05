<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeBehind="ListarMetasIndividuais.aspx.cs"
    Inherits="Sebrae.Academico.WebForms.Cadastros.MetasIndividuais.ListarMetasIndividuais" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <div class="form-group">
            <asp:Label ID="Label1" runat="server" Text="Buscar por nome" AssociatedControlID="txtNome"></asp:Label>
            <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <uc:LupaUsuario ID="LupaUsuario" runat="server" Text="Buscar por usuário" />
        </div>
        <div class="form-group">
            <asp:Label ID="Label2" runat="server" Text="Buscar por data de validade inicial" AssociatedControlID="txtDataValidadeInicial"></asp:Label>
            <asp:TextBox ID="txtDataValidadeInicial" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label3" runat="server" Text="Buscar por data de validade final" AssociatedControlID="txtDataValidadeFinal"></asp:Label>
            <asp:TextBox ID="txtDataValidadeFinal" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click" CssClass="btn btn-primary mostrarload" />
            <asp:Button ID="btnNovo" runat="server" Text="Criar nova" OnClick="btnNovo_Click" CssClass="btn btn-default mostrarload" />
        </div>
    </fieldset>
    <hr />
    <asp:Panel ID="pnlMetaIndividual" runat="server" Visible="false">
        <asp:GridView ID="dgvMetaIndividual" runat="server" AutoGenerateColumns="False" CssClass="table col-sm-12"
            CellPadding="4" GridLines="None" DataKeyNames="ID" OnRowCommand="dgvMetaIndividual_RowCommand" AllowPaging="true" PageSize="50" OnPageIndexChanging="dgvMetaIndividual_PageIndexChanging">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="Nome" HeaderText="Nome" />
                <asp:TemplateField HeaderText="Usuário">
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("Usuario.Nome") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="DataValidade" HeaderText="Data de Validade" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="IDChaveExterna" HeaderText="IDChaveExterna" />
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:LinkButton ID="lkbEditar" runat="server" CausesValidation="False" CssClass="mostrarload"
                            CommandName="editar" CommandArgument='<%# Eval("ID")%>' ToolTip="Editar">
                                            <span class="btn btn-default btn-xs">
						                        <span class="glyphicon glyphicon-pencil"></span>
						                    </span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="lkbExcluir" runat="server" CausesValidation="False" CommandName="excluir"
                            CommandArgument='<%# Eval("ID")%>' ToolTip="Excluir" OnClientClick="return ConfirmarExclusao();">
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
    <script type="text/javascript">
        jQuery(function ($) {
            $("#<%= txtDataValidadeInicial.ClientID %>").mask("99/99/9999");
            $("#<%= txtDataValidadeFinal.ClientID %>").mask("99/99/9999");
        });
    </script>
</asp:Content>
