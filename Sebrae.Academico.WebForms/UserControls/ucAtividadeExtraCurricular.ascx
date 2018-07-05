<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAtividadeExtraCurricular.ascx.cs"
    Inherits="Sebrae.Academico.WebForms.UserControls.ucAtividadeExtraCurricular" %>
<asp:GridView ID="dgvAtividadeExtraCurricular" runat="server" PageSize="1" CssClass="table col-sm-12"
    GridLines="None" OnRowDataBound="dgvAtividadeExtraCurricular_RowDataBound" AutoGenerateColumns="false">
    <Columns>
        <asp:TemplateField HeaderText="Curso">
            <ItemTemplate>
                <%#Eval("SolucaoEducacionalExtraCurricular")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Atividade">
            <ItemTemplate>
                <%#Eval("TextoAtividade")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Instituição">
            <ItemTemplate>
                <%#Eval("Instituicao")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Data de Início">
            <ItemTemplate>
                <%#Eval("DataInicioAtividade")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Data Fim">
            <ItemTemplate>
                <%#Eval("DataFimAtividade")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Carga Horária">
            <ItemTemplate>
                <%#Eval("CargaHoraria")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField ShowHeader="False">
            <ItemTemplate>
                <asp:HyperLink ID="hlnkDownload" runat="server" ClientIDMode="Static" Text="Baixar Arquivo"></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <EmptyDataTemplate>
        <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
    </EmptyDataTemplate>
</asp:GridView>
