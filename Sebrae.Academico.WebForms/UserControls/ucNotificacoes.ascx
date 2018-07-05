<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucNotificacoes.ascx.cs"
    Inherits="Sebrae.Academico.WebForms.UserControls.ucNotificacoes" %>

<asp:GridView ID="dgvNotificacoes" runat="server" GridLines="None" CssClass="table col-sm-12" AutoGenerateColumns="false">   
    <Columns>
        <asp:TemplateField HeaderText="Foi Lida ?">
            <ItemTemplate>
                <%#Eval("Visualizado")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Data da Notificação">
            <ItemTemplate>
                <%#Eval("DataNotificacao")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Notificação">
            <ItemTemplate>
                <%#Eval("TextoNotificacao")%>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>

    <EmptyDataTemplate>
        <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
    </EmptyDataTemplate>
</asp:GridView>
