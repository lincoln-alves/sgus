<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="EnvioInforme.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.Informe.EnvioInforme" %>

<%@ Register Src="~/UserControls/ucLupaUsuario.ascx" TagName="ucLupaUsuario" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/ucPermissoesRefatorado.ascx" TagName="ucPermissoesRefatorado" TagPrefix="uc" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc:ucLupaUsuario runat="server" ID="ucLupaUsuario" />
    <asp:Button ID="btnRemoverUsuario" runat="server" Text="Remover usuário" CssClass="btn btn-danger mostrarload" OnClick="btnRemoverUsuario_OnClick" />

    <hr />

    <uc:ucPermissoesRefatorado runat="server" ID="ucPermissoes" />

    <hr />
    <div class="form-group">
        <asp:Button ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-primary mostrarload" OnClick="btnSalvar_OnClick" />
        <asp:Button ID="btnEnviar" runat="server" Text="Enviar" CssClass="btn btn-primary mostrarload" OnClick="btnEnviar_OnClick" />
        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-default mostrarload" OnClick="btnCancelar_OnClick" />
    </div>

</asp:Content>
