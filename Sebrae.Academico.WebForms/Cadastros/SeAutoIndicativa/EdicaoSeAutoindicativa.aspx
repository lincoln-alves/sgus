<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EdicaoSeAutoindicativa.aspx.cs"
    MasterPageFile="~/Pagina.master" Inherits="Sebrae.Academico.WebForms.Cadastros.EdicaoSeAutoindicativa" %>

<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <div class="form-group">
            <div class="form-group">
                <asp:Label ID="Label23" runat="server" Text="Tipo"></asp:Label>
                <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip1" Chave="lblTipoItemTrilha" /><br />
                <asp:Label ID="lblTipoItemTrilha" runat="server" Text=""></asp:Label>
            </div>
            <div class="form-group">
                <asp:Label ID="Label24" runat="server" Text="Título"></asp:Label>
                <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip2" Chave="txtTituloItemTrilha" /><br />
                <asp:TextBox ID="txtTituloItemTrilha" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="Label26" runat="server" Text="Objetivo"></asp:Label>
                <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip3" Chave="lblObjetivoItemTrilha" /><br />
                <asp:Label ID="lblObjetivoItemTrilha" runat="server" Text=""></asp:Label>
            </div>
            <div class="form-group">
                <asp:Label ID="Label27" runat="server" Text="Link de Acesso"></asp:Label>
                <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip4" Chave="txtLinkAcessoItemTrilha" /><br />
                <asp:TextBox ID="txtLinkAcessoItemTrilha" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="Label28" runat="server" Text="Referência bibliográfica"></asp:Label>
                <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip5" Chave="txtReferenciaBibliograficaItemTrilha" /><br />
                <asp:TextBox ID="txtReferenciaBibliograficaItemTrilha" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="Label29" runat="server" Text="Local"></asp:Label>
                <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip6" Chave="txtLocalItemTrilha" /><br />
                <asp:TextBox ID="txtLocalItemTrilha" runat="server" CssClass="form-control"></asp:TextBox>
            </div>

            <asp:Button runat="server" Text="Aprovar" CssClass="btn" OnClick="btnAprovar_OnClick" OnClientClick="return ValidarAprovacao('Aprovar esta Solução Educacional?');" />
            <asp:Button runat="server" Text="Não Aprovar" CssClass="btn" OnClick="btnReprovar_OnClick" OnClientClick="return ValidarAprovacao('Não Aprovar esta Solução Educacional?');" />
        </div>
    </fieldset>
</asp:Content>
