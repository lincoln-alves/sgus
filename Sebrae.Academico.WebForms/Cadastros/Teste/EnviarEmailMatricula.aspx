<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" EnableViewState="true" EnableEventValidation="false"
    ValidateRequest="false" CodeBehind="EnviarEmailMatricula.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.Teste.EnviarEmailMatricula" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h3>Gerenciamento de Matrícula</h3>
    <div>

    <asp:TextBox ID="txtNomeSolucaoEducacional" runat="server"></asp:TextBox>
    
    <fieldset>
        <div class="form-group">
            <asp:Button ID="btnEnviarEmail" runat="server" Text="Enviar E-mail" OnClick="btnEnviarEmail_Click" ToolTip="Envia e-mail para os alunos matriculados na Oferta" CssClass="btn btn-default mostrarload"  OnClientClick="return confirm('Todos os alunos receberão o email de alerta padrão, deseja continua?');" />
        </div>
    </fieldset>
    </div>
</asp:Content>