<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeBehind="ListarMediaServer.aspx.cs"
    Inherits="Sebrae.Academico.WebForms.Cadastros.MediaServer.ListarMediaServer" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <div class="form-group">
            <asp:Label ID="Label1" runat="server" Text="Buscar por código" AssociatedControlID="txtID"></asp:Label>
            <asp:TextBox ID="txtID" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label2" runat="server" Text="Buscar por nome" AssociatedControlID="txtNome"></asp:Label>
            <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click" CssClass="btn btn-primary mostrarload" />
            &nbsp;<asp:Button ID="btnNovo" runat="server" Text="Criar novo" OnClick="btnNovo_Click" CssClass="btn btn-default mostrarload" />
        </div>
    </fieldset>

    <hr />
    <asp:Panel ID="pnlFileServer" runat="server" Visible="false">

        <h4>Resultado da Busca</h4>
        <asp:Literal ID="litTable" runat="server" />
        <asp:GridView ID="dgvFileServer" runat="server" AutoGenerateColumns="false" CssClass="table col-sm-12" EnableModelValidation="True" OnRowCommand="dgvFileServer_RowCommand" OnRowDataBound="dgvFileServer_RowDataBound" GridLines="None">
            <Columns>
                <asp:TemplateField HeaderText="Nome" HeaderStyle-HorizontalAlign="center">
                    <ItemTemplate>
                        <asp:Label ID="lblNome" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Url Reduzida" HeaderStyle-HorizontalAlign="center">
                    <ItemTemplate>
                        <asp:HyperLink ID="hplnkCaminhoReduzido" runat="server"></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="UF" HeaderStyle-HorizontalAlign="center">
                    <ItemTemplate>
                        <%#Eval("Uf.Nome") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <ItemStyle Width="90px" HorizontalAlign="center" />
                    <ItemTemplate>
                        <asp:LinkButton ID="lkbEditar" id_uf='<%#Eval("Uf.ID")%>' runat="server" CausesValidation="False"
                            CommandName="editar" CommandArgument='<%# Eval("ID")%>' ToolTip="Editar" CssClass="mostrarload" OnPreRender="lkbBotoesAcesso_PreRender">
                                        <span class="btn btn-default btn-xs">
						                    <span class="glyphicon glyphicon-pencil"></span>
						                </span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="lkbExcluir" id_uf='<%#Eval("Uf.ID")%>' runat="server" CausesValidation="False"
                            CommandName="excluir" CommandArgument='<%# Eval("ID")%>' ToolTip="Excluir" OnPreRender="lkbBotoesAcesso_PreRender" OnClientClick="return ConfirmarExclusao();">
                                        <span class="btn btn-default btn-xs">
						                    <span class="glyphicon glyphicon-remove"></span>
						                </span>
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>
</asp:Content>
