<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHelperTooltip.ascx.cs" Inherits="Sebrae.Academico.WebForms.UserControls.ucHelperTooltip" %>

<span class="glyphicon glyphicon-question-sign helpDialog" data-toggle="popover" data-placement="right" data-trigger="hover" data-html="true" runat="server" id="HelperTag" Visible="False"></span><asp:LinkButton ID="btnEditarHelperTag" runat="server" Visible="False" CssClass="mostrarload btn btn-default btn-xs helpertag-edit" ToolTip="Editar texto de ajuda" OnClick="btnEditarHelperTag_OnClick"><span class="glyphicon glyphicon-pencil"></span></asp:LinkButton>

<%= !string.IsNullOrWhiteSpace(Chave) ? "<script>$(function () { $('#" + HelperTag.ClientID  + "').popover({ template: '<div class=\"popover popover-helper " + CustomClass + "\" role=\"tooltip\"><div class=\"arrow\"></div><h3 class=\"popover-title\"></h3><div class=\"popover-content popover-helper\"></div></div>' }); });</script>" : "" %>
