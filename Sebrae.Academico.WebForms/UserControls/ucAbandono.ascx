<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAbandono.ascx.cs"
    Inherits="Sebrae.Academico.WebForms.UserControls.ucAbandono" %>
<asp:GridView ID="dgvAbandonos" runat="server" AutoGenerateColumns="false"
    GridLines="None" CssClass="table col-sm-12" OnRowDataBound="dgvAbandonos_RowDataBound">
    <Columns>
    <asp:TemplateField HeaderText="Solução Educacional">
            <ItemTemplate>
                <%#Eval("NomeSolucaoEducacional")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Status Matrícula">
            <ItemTemplate>
                <%#Eval("StatusMatriculaFormatado")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Turma">
            <ItemTemplate>
                <itemtemplate>
                <%#Eval("NomeTurma")%>
            </itemtemplate>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField ShowHeader="False">
            <ItemTemplate>
            </ItemTemplate>
        </asp:TemplateField>
        <%--<asp:TemplateField HeaderText="Data de Início">
            <ItemTemplate>
                <%#Eval("DataInicioAbandono")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Data Fim do Abandono">
            <ItemTemplate>
                <%#Eval("DataFimAbandono")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Origem">
            <ItemTemplate>
                <%#Eval("Origem")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Desconsiderado?">
            <ItemStyle Width="90px" HorizontalAlign="center" />
            <ItemTemplate>
                <asp:HiddenField runat="server" ID="hdfIdUsuarioAbandono" ClientIDMode="Static" />
                <asp:CheckBox ID="chkDesconsiderado" runat="server" CommandName="desconsiderar" CausesValidation="false"
                    Checked='<%# Eval("Desconsiderado") %>' AutoPostBack="true" ClientIDMode="Static"
                    OnCheckedChanged="chkDesconsiderado_CheckedChanged" CommandArgument='<%# Eval("ID")%>'
                    OnPreRender="chkDesconsiderado_PreRender" />
            </ItemTemplate>
        </asp:TemplateField>--%>
    </Columns>
    <EmptyDataTemplate>
        <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
    </EmptyDataTemplate>
</asp:GridView>
