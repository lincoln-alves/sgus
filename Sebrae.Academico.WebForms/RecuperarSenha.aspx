<%@ Page Title="" Language="C#" MasterPageFile="~/Academico20.Master" AutoEventWireup="true"
    CodeBehind="RecuperarSenha.aspx.cs" Inherits="Sebrae.Academico.WebForms.RecuperarSenha" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="navbar navbar-fixed-top">
        <div class="col-sm-3 col-md-2 pull-right logon">
            <span class="hidden-xs">Usuário não logado</span></div>
    </div>
    CPF:
    <asp:TextBox ID="txtCPF" runat="server" MaxLength="11"></asp:TextBox>
    <asp:Button ID="Enviar" runat="server" Text="Enviar" OnClick="btnSalvar_Click" />
    <script>
        $("body").addClass('login');
    </script>
</asp:Content>
