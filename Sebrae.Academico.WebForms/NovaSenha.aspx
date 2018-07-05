<%@ Page Title="" Language="C#" MasterPageFile="~/Academico20.Master" AutoEventWireup="true"
    CodeBehind="NovaSenha.aspx.cs" Inherits="Sebrae.Academico.WebForms.NovaSenha" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="navbar navbar-fixed-top">
        <div class="col-sm-3 col-md-2 pull-right logon">
            <span class="hidden-xs">Usuário não logado</span></div>
    </div>
    <asp:Label ID="Label3" runat="server" Text="Alteração de Senha"></asp:Label></h3>
    CPF:
    <asp:TextBox ID="txtCPF" runat="server" MaxLength="11"></asp:TextBox>
    Nova Senha:
    <asp:TextBox ID="txtNovaSenha" runat="server" MaxLength="11" TextMode="Password"></asp:TextBox>
    Confirmar Nova Senha:
    <asp:TextBox ID="txtConfNovaSenha" runat="server" MaxLength="11" TextMode="Password"></asp:TextBox>
    <asp:Button ID="Enviar" runat="server" Text="Enviar" OnClick="btnSalvar_Click" />
    <script>
        $("body").addClass('login');
    </script>
</asp:Content>
