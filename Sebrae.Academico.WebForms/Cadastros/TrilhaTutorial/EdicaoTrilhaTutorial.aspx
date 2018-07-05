<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="EdicaoTrilhaTutorial.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.EdicaoTrilhaTutorial" %>
<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <div class="form-group">
            <asp:Label ID="Label2" runat="server" Text="Nome" AssociatedControlID="txtNome" />
            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip6" Chave="trilhaTutorialNome" />
            <asp:TextBox ID="txtNome" runat="server" CssClass="form-control" MaxLength="450"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="LblCategoria" runat="server" Text="Categoria" AssociatedControlID="ddlCategoria" ></asp:Label>
            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip2" Chave="trilhaTutorialCategoria" />
            <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="form-control">                
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Label ID="Label3" runat="server" Text="Conteúdo" AssociatedControlID="txtConteudo"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip1" Chave="trilhaTutorialConteudo" />
            <CKEditor:CKEditorControl ID="txtConteudo" BasePath="/js/ckeditor/" runat="server"></CKEditor:CKEditorControl>
        </div>
        <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click"
            CssClass="btn btn-primary mostrarload" />
        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click"
            CssClass="btn btn-default" />
    </fieldset>

</asp:Content>
