<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeBehind="AlterarSenha.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.AlterarSenha" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h3>
        Alteração de Senha</h3>
    <fieldset>
        <div class="form-group">
            Nova Senha:
            <asp:TextBox ID="txtNovaSenha" runat="server" MaxLength="11" TextMode="Password"  CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            Confirmar Nova Senha:
            <asp:TextBox ID="txtConfNovaSenha" runat="server" MaxLength="11" TextMode="Password"  CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Button ID="btnAlterarSenha" runat="server" Text="Alterar Senha" OnClick="btnSalvar_Click" CssClass="btn btn-default mostrarload" />
        </div>
    </fieldset>
</asp:Content>
