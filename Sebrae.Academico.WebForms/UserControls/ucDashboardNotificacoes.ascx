<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDashboardNotificacoes.ascx.cs"
    Inherits="Sebrae.Academico.WebForms.UserControls.ucDashboardNotificacoes" %>
<div>
    Notificações<br />
    <asp:GridView ID="dgvNotificacoes" runat="server" PageSize="1" CssClass="table col-sm-12"
        GridLines="None" AutoGenerateColumns="false">
        <Columns>
            <asp:TemplateField HeaderText="Nome">
                <ItemTemplate>
                    <%#Eval("TextoNotificacao")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Data da Notificação">
                <ItemTemplate>
                    <%#Eval("DataNotificacao", "{0:dd/MM/yyyy}")%>
                </ItemTemplate>
            </asp:TemplateField>
            <%--  <asp:TemplateField ShowHeader="False">
                                            <ItemStyle Width="90px" HorizontalAlign="center" />
                                            <ItemTemplate>
                                                <asp:HyperLink ID="hlnkFuncionalidade" runat="server" ClientIDMode="Static"></asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
        </Columns>
        <EmptyDataTemplate>
            <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
        </EmptyDataTemplate>
    </asp:GridView>
</div>
