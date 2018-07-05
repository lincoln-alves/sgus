<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeBehind="EdicaoHierarquiaAuxiliar.aspx.cs"
    Inherits="Sebrae.Academico.WebForms.Cadastros.HierarquiaAuxiliar.EdicaoHierarquiaAuxiliar" %>

<%@ Register Src="~/UserControls/ucLupaUsuario.ascx" TagName="ucLupaUsuario" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel runat="server" ID="pnlEdicaoFornecedor" DefaultButton="btnSalvar">
        <fieldset>
            <div class="form-group">
                <asp:Label ID="labelDdlCodUnidade" runat="server" Text="Diretoria" AssociatedControlID="ddlCodUnidade"></asp:Label>
                <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip1" Chave="ddlCodUnidade" />
                <asp:DropDownList ID="ddlCodUnidade" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>

            <uc:ucLupaUsuario ID="ucLupaUsuario" runat="server" Chave="usuario" />

            <div class="form-group">
                <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click"
                    CssClass="btn btn-primary mostrarload" />
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click"
                    CssClass="btn btn-default" />
            </div>
        </fieldset>
    </asp:Panel>       
</asp:Content>