<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeBehind="Agilizador.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.Ferramentas.Agilizador" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h3>
        Instrução SQL</h3>
    <div>
        <fieldset>
            <div class="form-group">
                Código:
                <asp:TextBox ID="txtSenha" runat="server"></asp:TextBox>
                <br />
                <br />
                <asp:TextBox ID="txtInstrucao" runat="server" Width="657px" TextMode="MultiLine"
                    Height="380"></asp:TextBox>
                <br />
                <br />
                <asp:Button ID="BtnProcessar" runat="server" ToolTip="Clique Aqui para processar a instrução SQL"
                    Text="Processar" OnClick="BtnProcessar_Click" />
            </div>
            <br />
            <asp:GridView ID="dgvResultado" runat="server" AutoGenerateColumns="true">
            </asp:GridView>
            <asp:Label ID="lblMensagem" runat="server"></asp:Label>
        </fieldset>
    </div>
</asp:Content>
