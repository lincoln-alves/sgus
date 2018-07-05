<%@ Page Title="" Language="C#" MasterPageFile="~/Academico20.Master" AutoEventWireup="true"
    CodeBehind="ListarUsuario.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.ListarUsuário" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table>
            <tr>
                <td colspan="2">
                    <h3>
                        <asp:Label ID="Label3" runat="server" Text="Filtro de Usuário"></asp:Label></h3>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Nome:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNome" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="UF:"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="cbxFiltroUF" DataTextField="Sigla" DataValueField="ID" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label4" runat="server" Text="CPF:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtCPF" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click" />
                    <asp:Button ID="btnCadastrarUsuario" runat="server" Text="Novo" OnClick="btnCadastrarUsuario_Click" />
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
    <br />
    <asp:GridView ID="dgvUsuario" runat="server" AutoGenerateColumns="False" CssClass="table col-sm-12"
        OnRowCommand="dgvUsuario_RowCommand">
        <Columns>
            <asp:BoundField DataField="Nome" HeaderText="Nome" />
            <asp:TemplateField HeaderText="UF">
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("Uf.Nome") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Uf") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="CPF" HeaderText="CPF" />
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:LinkButton ID="lkbEditar" runat="server" CausesValidation="False" CommandName="editar"
                        CssClass="btn btn-default btn-xs" CommandArgument='<%# Eval("ID")%>' Text="Editar"><span class="glyphicon glyphicon-pencil"></span></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <script>
        $("body").addClass('relatorios');
    </script>
</asp:Content>
