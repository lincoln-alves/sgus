<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeBehind="EdicaoMetasInstitucionais.aspx.cs"
    Inherits="Sebrae.Academico.WebForms.Cadastros.MetasInstitucionais.EdicaoMetasInstitucionais" %>

<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <div class="form-group">
            <asp:Label ID="Label1" runat="server" Text="Nome da meta" AssociatedControlID="txtNome"></asp:Label>
            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip1" Chave="txtNome" />
            <asp:TextBox ID="txtNome" runat="server" CssClass="form-control" MaxLength="250"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label2" runat="server" Text="Data de Inicio do Ciclo" AssociatedControlID="txtDataInicioCiclo"></asp:Label>
            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip2" Chave="txtDataInicioCiclo" />
            <asp:TextBox ID="txtDataInicioCiclo" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label3" runat="server" Text="Data de Fim do Ciclo" AssociatedControlID="txtDataFimCiclo"></asp:Label>
            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip3" Chave="txtDataFimCiclo" />
            <asp:TextBox ID="txtDataFimCiclo" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Button ID="Salvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click" CssClass="btn btn-primary mostrarload" />
            <asp:Button ID="Cancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" CssClass="btn btn-default mostrarload" />
        </div>
    </fieldset>
    <script type="text/javascript">
        jQuery(function ($) {
            $("#<%= txtDataInicioCiclo.ClientID %>").mask("99/99/9999");
            $("#<%= txtDataFimCiclo.ClientID %>").mask("99/99/9999");
        });
    </script>
</asp:Content>
