<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="ObterSenhaEmergencia.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.ObterSenhaEmergencia" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="text-align: center; vertical-align: text-bottom; width: 100%; height: 100%;">
        <div class="form-group">
            <asp:Button ID="btnGerar" runat="server" Text="Gerar Senha de Emergência" OnClick="btnGerar_Click" CssClass="btn btn-primary" />
        </div>
        <div class="form-group">
            Senha: <b>
                <asp:Label ID="lblSenhaEmergencia" runat="server" Text="Label" Visible="False"></asp:Label></b>
        </div>
        <div class="form-group">
            Validade: <b>
                <asp:Label ID="lblDataExpiracao" runat="server" Text="Label" Visible="False"></asp:Label></b>
        </div>
    </div>
</asp:Content>