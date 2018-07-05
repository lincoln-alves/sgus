<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="ImportarTag.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.Tag.ImportarTag" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form-group">
        <asp:Label ID="Label1" runat="server" Text="Arquivo csv de Tags:" AssociatedControlID="fupldArquivoTags" />
        <asp:FileUpload ID="fupldArquivoTags" runat="server" EnableViewState="true" ViewStateMode="Enabled" />
        <asp:Label ID="Label2" runat="server" Text="Selecione um arquivo .csv:"></asp:Label>
    </div>
    <div class="form-group">
        <asp:Button ID="btnEnviarArquivo" runat="server" Text="Enviar" CssClass="btn btn-default"
            OnClick="btnEnviarArquivo_Click" />
    </div>
</asp:Content>
