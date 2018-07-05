<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucFormatoSaidaRelatorio.ascx.cs" Inherits="Sebrae.Academico.WebForms.UserControls.ucFormatoSaidaRelatorio" %>

<asp:Label ID="lblNome" runat="server" Text="Formato de Saída" AssociatedControlID="rblTipoSaida"/>
<asp:RadioButtonList ID="rblTipoSaida" runat="server" RepeatLayout="UnorderedList" CssClass="form-control file-types">
    <asp:ListItem Value="PDF" Selected="True"><i class="icon icon-adobe-pdf" title="PDF"></i></asp:ListItem>
    <asp:ListItem Value="EXCEL"><i class="icon icon-ms-excel" title="EXCEL"></i></asp:ListItem>
    <asp:ListItem Value="WORD"><i class="icon icon-ms-word" title="WORD"></i></asp:ListItem>
</asp:RadioButtonList>

