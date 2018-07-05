<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucFaleConosco.ascx.cs"
    Inherits="Sebrae.Academico.WebForms.UserControls.ucFaleConosco" %>
<asp:GridView ID="dgvFaleConosco" runat="server" PageSize="1" CssClass="table col-sm-12"
    GridLines="None" AutoGenerateColumns="false">
    <Columns>
        <asp:TemplateField HeaderText="Data Solicitação">
            <ItemTemplate>
                <%#Eval("DataSolicitacao")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Assunto">
            <ItemTemplate>
                <%#Eval("Assunto")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField ShowHeader="False">
            <ItemStyle Width="90px" HorizontalAlign="center" />
            <ItemTemplate>
                <asp:LinkButton ID="lkbVisualizarMsg" runat="server" CausesValidation="False" CommandName="visualizar"
                    CommandArgument='<%# Eval("ID")%>' Text="Editar">
                    <span class="btn btn-default btn-xs">
						<span class="glyphicon glyphicon-pencil"></span>
					</span>
                </asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <EmptyDataTemplate>
        <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
    </EmptyDataTemplate>
</asp:GridView>
