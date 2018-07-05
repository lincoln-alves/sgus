<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="InclusaoUsuario.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.InclusaoUsuario" %>

<%@ Register Src="../../UserControls/ucUsuario.ascx" TagName="ucUsuario" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="overflow: scroll;">
        <uc1:ucUsuario ID="ucUsuario1" runat="server" />
        <div class="form-group">
            Senha:
            <asp:TextBox ID="txtSenha" MaxLength="10" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <br />
        <asp:Button ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-primary" onclick="btnSalvar_Click1" />
    </div>
</asp:Content>
