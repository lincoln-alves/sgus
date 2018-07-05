<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="EdicaoObjetivo.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.EdicaoObjetivo" %>
<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <div class="form-group">
            <asp:Label ID="Label2" runat="server" Text="Objetivo" AssociatedControlID="txtObjetivo" />
            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip6" Chave="objetivoTexto" />
            <asp:TextBox ID="txtObjetivo" runat="server" CssClass="form-control" MaxLength="450"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label1" runat="server" Text="Chave Externa" AssociatedControlID="txtChaveExterna"/>
            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip1" Chave="objetivoChaveExterna" />
            <asp:TextBox ID="txtChaveExterna" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
        </div>
        <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click"
            CssClass="btn btn-primary mostrarload" />
        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click"
            CssClass="btn btn-default" />
    </fieldset>

    <script>
        validarForm();
    </script>
</asp:Content>
