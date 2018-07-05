<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EdicaoSolucaoSebrae.aspx.cs"
    MaintainScrollPositionOnPostback="true" ValidateRequest="false" EnableEventValidation="false"
    MasterPageFile="~/Pagina.master" Inherits="Sebrae.Academico.WebForms.Cadastros.SolucaoSebrae.EdicaoSolucaoSebrae" %>

<%@ Register Src="~/UserControls/ucItemTrilha.ascx" TagName="ucItemTrilha" TagPrefix="uc" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc:ucItemTrilha ID="ucItemTrilha1" runat="server" />
    <asp:Button ID="btnSalvar" CssClass="btn btn-primary" runat="server" Text="Salvar"
        OnClick="btnSalvar_Click" />
    <asp:Button ID="btnCancelar" CssClass="btn btn-default" runat="server" Text="Cancelar"
        OnClick="btnCancelar_Click" />

    <script type="text/javascript">
        validarForm();
    </script>
</asp:Content>
