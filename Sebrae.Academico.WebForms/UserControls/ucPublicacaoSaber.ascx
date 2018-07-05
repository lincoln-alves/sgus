<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPublicacaoSaber.ascx.cs"
    Inherits="Sebrae.Academico.WebForms.UserControls.ucPublicacaoSaber" %>
<asp:GridView ID="dgvPublicacaoDoSaber" runat="server" AutoGenerateColumns="false"
    CssClass="table col-sm-12" GridLines="None">
    <Columns>
        <asp:TemplateField HeaderText="Publicação">
            <ItemTemplate>
                <%#Eval("PublicacaoSaber.TextoTitulo")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Publicação">
            <ItemTemplate>
                <%#Eval("PublicacaoSaber.DataPublicacao")%>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <EmptyDataTemplate>
        <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
    </EmptyDataTemplate>
</asp:GridView>
