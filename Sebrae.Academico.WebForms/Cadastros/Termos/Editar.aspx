<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="Editar.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.Termos.Editar" %>

<%@ Register Src="~/UserControls/ucCategorias.ascx" TagName="ucCategorias" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <div class="form-group">
            <asp:Label ID="Label2" runat="server" Text="Nome" AssociatedControlID="txtNome"></asp:Label>
            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip1" Chave="nome" />
            <asp:TextBox ID="txtNome" runat="server" CssClass="form-control" MaxLength="250"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label1" runat="server" Text="Termos do Aceite" AssociatedControlID="txtTermo"></asp:Label>
            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip2" Chave="txtTermo" />
            <CKEditor:CKEditorControl ID="txtTermo" BasePath="/js/ckeditor/" runat="server"></CKEditor:CKEditorControl>
        </div>
        <div class="form-group">
            <asp:Label ID="Label6" runat="server" Text="Políticas de Consequência" AssociatedControlID="txtPoliticaConseguencia"></asp:Label>
            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip3" Chave="txtPoliticaConseguencia" />
            <CKEditor:CKEditorControl ID="txtPoliticaConseguencia" BasePath="/js/ckeditor/" runat="server"></CKEditor:CKEditorControl>
        </div>
        <div class="form-group">
            <asp:Label ID="Label8" runat="server" Text="CATEGORIAS" AssociatedControlID="ucCategorias1" />
            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip4" Chave="ucCategorias1" />
            <uc1:ucCategorias ID="ucCategorias1" runat="server" />
        </div>
        <div class="form-group">
            <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click" CssClass="btn btn-primary" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" CssClass="btn btn-default" />
        </div>
    </fieldset>
</asp:Content>
