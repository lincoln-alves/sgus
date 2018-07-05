<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeBehind="EdicaoMetasIndividuais.aspx.cs"
    Inherits="Sebrae.Academico.WebForms.Cadastros.MetasIndividuais.EdicaoMetasIndividuais" %>

<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <div class="form-group">
            <asp:Label ID="Label1" runat="server" Text="Nome da meta" AssociatedControlID="txtNome"></asp:Label>
            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip1" Chave="txtNome" />
            <asp:TextBox ID="txtNome" runat="server" CssClass="form-control" MaxLength="250"></asp:TextBox>
        </div>
        <div class="form-group">
            <uc:LupaUsuario ID="LupaUsuario" runat="server" Chave="lupaUsuario" />
        </div>
        <div class="form-group">
            <asp:Label ID="Label2" runat="server" Text="Data de Validade" AssociatedControlID="txtDataValidade"></asp:Label>
            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip2" Chave="txtDataValidade" />
            <asp:TextBox ID="txtDataValidade" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label3" runat="server" Text="ID Chave Externa"></asp:Label>
            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip3" Chave="txtIDChaveExterna" />
            <asp:TextBox ID="txtIDChaveExterna" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click"
                CssClass="btn btn-primary mostrarload" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click"
                CssClass="btn btn-default" />
        </div>
    </fieldset>
    <script type="text/javascript">
        jQuery(function ($) {
            $("#<%= txtDataValidade.ClientID %>").mask("99/99/9999");
        });
    </script>
</asp:Content>
