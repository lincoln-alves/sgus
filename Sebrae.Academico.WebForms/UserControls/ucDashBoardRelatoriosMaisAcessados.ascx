 <%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDashBoardRelatoriosMaisAcessados.ascx.cs"
    Inherits="Sebrae.Academico.WebForms.ucDashBoardRelatoriosMaisAcessados" %>
        
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:GridView ID="dgvInformacoesDeRelatoriosMaisAcessados" runat="server" PageSize="1"
    OnRowDataBound="dgvInformacoesDeRelatoriosMaisAcessados_RowDataBound" CssClass="table col-sm-12"
    GridLines="None" AutoGenerateColumns="false">
    <Columns>
        <asp:TemplateField ShowHeader="False">
            <ItemStyle Width="90px" HorizontalAlign="center" />
            <ItemTemplate>
                <asp:HyperLink ID="hlnkRelatorio" runat="server" ClientIDMode="Static"></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <EmptyDataTemplate>
        <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
    </EmptyDataTemplate>
</asp:GridView>
