<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucNacionalizarRelatorio.ascx.cs" Inherits="Sebrae.Academico.WebForms.UserControls.ucNacionalizarRelatorio" %>

<div class="form-group" runat="server" id="divNacionalizarRelatorio">
    <asp:Label ID="Label1" runat="server" Text="UF do Gestor" AssociatedControlID="ckbNacionalizacao" data-help="<%$ Resources:Resource, filtroNacionalizacaoRelatorio %>"></asp:Label>
    <asp:CheckBox ID="ckbNacionalizacao" CssClass="form-control" runat="server" OnCheckedChanged="ckbNacionalizacao_OnCheckedChanged" Text="Filtrar relatório com dados da UF"/>
</div>