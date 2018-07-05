<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="Sincroniar.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.SincronizarPortal.Sincronizar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Button ID="btnSincronizarAreaTematica" runat="server" Text="Sincronizar Todas as Areas Tematicas" OnClick="btnSincronizarAreaTematica_Click" CssClass="btn btn-primary mostrarload" /><br/>
    <asp:Button ID="btnSincroniarSolucaoEducacional" runat="server" Text="Sincronizar Todas As Solucoes Educacionais" OnClick="btnSincronizarSE_Click" CssClass="btn btn-primary mostrarload" /><br/>
    <asp:Button ID="btnSincronizarOferta" runat="server" Text="Sincronizar Todas As Ofertas" OnClick="btnSincronizarOferta_Click" CssClass="btn btn-primary mostrarload" /><br/>
    <asp:Button ID="btnSincronizarTrilha" runat="server" Text="Sincronizar Todas As Trilhas" OnClick="btnSincronizarTrilha_Click" CssClass="btn btn-primary mostrarload" /><br/>
    <asp:Button ID="btnSincronizarPrograma" runat="server" Text="Sincronizar Todas os Programas" OnClick="btnSincronizarPrograma_Click" CssClass="btn btn-primary mostrarload" /><br/>
    <asp:Button ID="btnSincronizarCapacitacoes" runat="server" Text="Sincronizar Todas as Capacitações" OnClick="btnSincronizarCapacitacoes_Click" CssClass="btn btn-primary mostrarload" /><br/>
</asp:Content>
