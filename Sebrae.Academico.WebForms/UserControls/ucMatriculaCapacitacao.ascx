<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucMatriculaCapacitacao.ascx.cs" Inherits="Sebrae.Academico.WebForms.UserControls.ucMatriculaCapacitacao" %>

<%@ Register Src="~/UserControls/ucLupaUsuario.ascx" TagName="LupaUsuario" TagPrefix="uc1" %>

<fieldset>
    <div class="form-group">
        <uc1:LupaUsuario ID="LupaUsuario" runat="server" />
    </div>
    <div class="form-group">
        Status:
        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control"></asp:DropDownList>
    </div>
    <div class="form-group">
        Turma:
        <asp:DropDownList ID="ddlTurmaCapacitacao" Enabled="false" runat="server" CssClass="form-control"></asp:DropDownList>
    </div>
    <div class="form-group">
        <asp:Button ID="btnEnviar" runat="server" Text="Enviar" CausesValidation="false" OnClick="btnEnviar_Click"  CssClass="btn btn-default"/>
    </div>
</fieldset>
