<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true"
    CodeBehind="SelecionarUfs.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.Nacionalizacao.SelecionarUfs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <asp:Table ID="myTable" runat="server" CssClass="table table-hover table-striped">
            <asp:TableRow TableSection="TableHeader">
                <asp:TableHeaderCell>UF</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="150"></asp:TableHeaderCell>
            </asp:TableRow>
        </asp:Table>
    </fieldset>
</asp:Content>
