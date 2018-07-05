<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="EdicaoTag.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.EdicaoTag" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <div class="form-group">
            <asp:Label ID="Label2" runat="server" Text="Tag Pai" AssociatedControlID="ddlTagPai" data-help="<%$ Resources:Resource, categoriaPai %>"></asp:Label>
            <asp:DropDownList ID="ddlTagPai" runat="server" CssClass="form-control">
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Label ID="Label1" runat="server" Text="Nome*" AssociatedControlID="txtNome" data-help="<%$ Resources:Resource, nomeExibicao %>"></asp:Label>
            <asp:TextBox ID="txtNome" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:CheckBox ID="chkSinonimo" runat="server" Text="Sinônimo" CssClass="form-control" data-help="<%$ Resources:Resource, tagSinonimo %>" />
        </div>
        <div class="form-group">
            <asp:Button ID="Button1" runat="server" Text="Salvar" OnClick="btnSalvar_Click" CssClass="btn btn-primary mostrarload" />
            <asp:Button ID="Button2" runat="server" Text="Cancelar" OnClick="btnCancelar_Click"
                CssClass="btn btn-default mostrarload" />
        </div>
    </fieldset>
</asp:Content>
