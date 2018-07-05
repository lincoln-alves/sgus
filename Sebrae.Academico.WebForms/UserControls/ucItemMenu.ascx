<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucItemMenu.ascx.cs"
    Inherits="Sebrae.Academico.WebForms.UserControls.WebUserControl1" %>

<li class="menu-item">   
    <asp:hyperlink ID="hypLinkMenuNome" runat="server" CssClass="menu-nome" title="Abrir"/>
    <span class="actions visible-md visible-lg">
        <asp:hyperlink ID="hypLinkMenuRelatar" runat="server" title="Gerar relatório" CssClass="menu-relatar glyphicon glyphicon-file"/>
        <asp:hyperlink ID="hypLinkMenuAdicionar" runat="server" title="Adicionar" CssClass="menu-adicionar glyphicon glyphicon-plus"/>
        <asp:hyperlink ID="hypLinkMenuListar" runat="server" title="Buscar" CssClass="menu-adicionar glyphicon glyphicon-search"/>
        <asp:hyperlink ID="hypLinkMenuConfigurar" runat="server" title="Configurar" CssClass="menu-configurar glyphicon glyphicon-cog"/>
        <asp:hyperlink ID="hypLinkMenuGerenciar" runat="server" title="Gerenciar" CssClass="menu-gerenciar glyphicon glyphicon-wrench"/>
    </span>
</li>
