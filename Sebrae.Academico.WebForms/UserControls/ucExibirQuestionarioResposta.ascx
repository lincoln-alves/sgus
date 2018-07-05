<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucExibirQuestionarioResposta.ascx.cs"
    Inherits="Sebrae.Academico.WebForms.UserControls.ucExibirQuestionarioResposta" %>
<fieldset>
    <div class="form-group">
        <div id="dvDetalhesDaProva" runat="server">
        </div>
    </div>
</fieldset>
<div class="form-group">
    <asp:Button ID="btnFechar" Text="Enviar" runat="server" OnClick="btnFechar_Click" />
</div>
<%--<div class="form-group">
    <asp:Button ID="btnEnviar" runat="server" Text="Enviar" OnClick="btnEnviar_Click"
        CssClass="btn btn-default" Enabled="false" CausesValidation="true" />
</div>
--%>