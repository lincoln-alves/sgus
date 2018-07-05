<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucInformarPagamento.ascx.cs"
    Inherits="Sebrae.Academico.WebForms.UserControls.ucInformarPagamento" %>

<div>
    <fieldset>
        <div class="form-group">
            <asp:Label ID="lblConfiguracao" runat="server" Text="Configurações de Pagamento"
                AssociatedControlID="ddlConfigPagto" />
            <asp:DropDownList ID="ddlConfigPagto" runat="server" CssClass="form-control">
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Label ID="LblNomeTurma" runat="server" Text="Início Vigência" AssociatedControlID="txtDataInicioVigencia"></asp:Label>
            <asp:TextBox ID="txtDataInicioVigencia" runat="server" MaxLength="10" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Button ID="btnCalcular" Text="Calcular" runat="server" OnClick="btnCalcular_Click" CssClass="btn btn-default mostrarload" />
        </div>
        <div class="form-group">
            <asp:Label ID="LblChaveExterna" runat="server" Text="Fim Vigência" AssociatedControlID="txtFimVigencia"></asp:Label>
            <asp:TextBox ID="txtFimVigencia" runat="server" MaxLength="10" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="LblLocal" runat="server" Text="Valor" AssociatedControlID="txtValor"></asp:Label>
            <asp:TextBox ID="txtValor" runat="server" MaxLength="250" CssClass="form-control" onkeypress="return EhNumericoOuVirgula(event)"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="LblDtInicio" runat="server" Text="Data Início Renovação" AssociatedControlID="TxtDtInicioRenovacao"></asp:Label>
            <asp:TextBox ID="TxtDtInicioRenovacao" runat="server" MaxLength="10" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="LblDtFinal" runat="server" Text="Data Máxima Inadimplência " AssociatedControlID="txtDataMaxInadimplencia"></asp:Label>
            <asp:TextBox ID="txtDataMaxInadimplencia" runat="server" MaxLength="10" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="LblCodInformacao" runat="server" Text="Nosso Número" AssociatedControlID="txtCodInformacao"></asp:Label>
            <asp:TextBox ID="TxtCodInformacao" runat="server" MaxLength="23" CssClass="form-control"
                Enabled="false"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="LblInPago" runat="server" Text="Pago" AssociatedControlID="LblInPago"></asp:Label>
            <asp:DropDownList ID="DdlInPago" runat="server" CssClass="form-control">
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Label ID="LblFormaPagamento" runat="server" Text="Forma de Pagamento" AssociatedControlID="LblFormaPagamento"></asp:Label>
            <asp:DropDownList ID="DdlFormaPagamento" runat="server" CssClass="form-control">
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Label ID="Label1" runat="server" Text="Data de Vencimento" AssociatedControlID="TxtDtVencimento"></asp:Label>
            <asp:TextBox ID="TxtDtVencimento" runat="server" MaxLength="23" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Button ID="btnInformarPagamento" Text="Enviar" runat="server" OnClick="btnInformarPagamento_Click" CssClass="btn btn-primary mostrarload" />
        </div>
    </fieldset>
    <script type="text/javascript">

        jQuery(function ($) {
            $("#<%= txtDataInicioVigencia.ClientID %>").mask("99/99/9999");
            $("#<%= TxtDtInicioRenovacao.ClientID %>").mask("99/99/9999");
            $("#<%= txtDataMaxInadimplencia.ClientID %>").mask("99/99/9999");
            $("#<%= TxtDtVencimento.ClientID %>").mask("99/99/9999");
        });
    </script>
</div>
