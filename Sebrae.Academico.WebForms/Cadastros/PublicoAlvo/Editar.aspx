<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="Editar.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.PublicoAlvo.Editar" %>
<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <div class="form-group">
            <asp:Label ID="lblNome" runat="server" Text="Nome" AssociatedControlID="txtNome" />
            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip1" Chave="txtNome" />
            <asp:TextBox ID="txtNome" runat="server" CssClass="form-control" MaxLength="250"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click" CssClass="btn btn-primary" />
        </div>
    </fieldset>
</asp:Content>
