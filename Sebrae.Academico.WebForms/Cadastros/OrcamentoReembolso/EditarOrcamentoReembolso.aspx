<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="EditarOrcamentoReembolso.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.EditarOrcamentoReembolso" %>
<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <fieldset>
            <div class="form-group">
                <asp:Label ID="lblNome" runat="server" Text="Ano" AssociatedControlID="txtAno"></asp:Label>
                <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip2" Chave="txtAno" />
                <asp:TextBox ID="txtAno" runat="server" MaxLength="250" CssClass="form-control"></asp:TextBox>
            </div>
                            
            <div class="form-group">
                <asp:Label ID="Label1" runat="server" Text="Orçamento" AssociatedControlID="txtOrcamento"></asp:Label>
                <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip1" Chave="txtOrcamento" />
                <asp:TextBox ID="txtOrcamento" runat="server" MaxLength="250" CssClass="form-control"></asp:TextBox>
            </div>
        </fieldset>
        <asp:Button ID="btnSalvar" CssClass="btn btn-primary" runat="server" Text="Salvar" OnClick="btnSalvar_Click" />
        <asp:Button ID="btnCancelar" CssClass="btn btn-default" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" />
    </div>
    <script type="text/javascript" src="/js/jquery.maskMoney.min.js"></script>
    <script type="text/javascript">
        (function ($) {
            if (<%= (Request["Id"] == null).ToString().ToLower() %>){
                $("#<%= txtAno.ClientID %>").mask("9999");
            }

            $('#<%= txtOrcamento.ClientID %>').maskMoney();

        })(jQuery);
    </script>
</asp:Content>