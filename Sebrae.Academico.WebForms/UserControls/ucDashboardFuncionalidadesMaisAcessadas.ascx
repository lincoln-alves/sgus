<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDashboardFuncionalidadesMaisAcessadas.ascx.cs"
    Inherits="Sebrae.Academico.WebForms.UserControls.ucDashboardFuncionalidadesMaisAcessadas" %>

<asp:GridView ID="dgvFuncionalidadesMaisAcessadas" runat="server" PageSize="1" CssClass="table col-sm-12"
    OnRowDataBound="dgvFuncionalidadesMaisAcessadas_RowDataBound" GridLines="None"
    AutoGenerateColumns="false">
    <Columns>
        <asp:TemplateField ShowHeader="False">
            <ItemTemplate>
                <asp:HyperLink ID="hlnkFuncionalidade" runat="server" ClientIDMode="Static"></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <EmptyDataTemplate>
        <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
    </EmptyDataTemplate>
</asp:GridView>
