<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="VerificarCertificado.aspx.cs"
    Inherits="Sebrae.Academico.WebForms.Cadastros.Certificado.ValidarCertificado" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <div class="form-group">
            <asp:Label ID="lblCodigo" runat="server" Text="Código do certificado" AssociatedControlID="txtCodigo"></asp:Label>
            <asp:TextBox ID="txtCodigo" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Button ID="btnPesquisar" runat="server" Text="Verificar" OnClick="btnVerificar_Click"
                CssClass="btn btn-primary mostrarload" />
        </div>
    </fieldset>
    <hr />
    <asp:Panel ID="pnlResultadoVerificacao" runat="server" Visible="false">
        <h4>Resultado da Busca</h4>
        <div>
            <div>
                <asp:Label ID="lblResultado" runat="server"></asp:Label>
            </div>
            <br />
            <div>
                Nome:
                <asp:Label ID="lblNome" runat="server"></asp:Label>
            </div>
            <div>
                Curso: 
                <asp:Label ID="lblCurso" runat="server"></asp:Label>
            </div>
            <div>
                Data de geração: 
                <asp:Label ID="lblDataGeracao" runat="server"></asp:Label>
            </div>
            <div>
                Tipo:
                <asp:Label ID="lblTipo" runat="server"></asp:Label>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlNaoEncontrado" runat="server" Visible="false">
        <h4>Resultado da Busca</h4>
        <div>
            <div>
                <asp:Label ID="lblNaoEncontrado" runat="server"></asp:Label>
            </div>
        </div>
    </asp:Panel>

</asp:Content>
