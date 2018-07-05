<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="EdicaoTurma.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.Turma.EdicaoTurma1" %>

<%@ Register Src="~/UserControls/ucTurma.ascx" TagName="ucTurma" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:ucTurma ID="ucTurma1" runat="server" />
    <asp:Button ID="Button1" runat="server" OnClick="btnSalvar_Click" Text="Salvar" 
    CssClass="btn btn-primary mostrarload" />
        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click"
        CssClass="btn btn-default mostrarload" />
</asp:Content>
