<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeBehind="EdicaoOferta.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.EdicaoOferta" %>

<%@ Register Src="~/UserControls/ucOferta.ascx" TagName="Oferta" TagPrefix="uc" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <uc:Oferta ID="ucOferta1" runat="server" />
        <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click"
            CssClass="btn btn-primary mostrarload" />
        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click"
            CssClass="btn btn-default" />
    </div>
</asp:Content>
