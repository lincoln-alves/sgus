<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucTutorias.ascx.cs" Inherits="Sebrae.Academico.WebForms.UserControls.ucTutorias" %>

<asp:GridView runat="server" AutoGenerateColumns="false" ID="dgvTutorias"  CssClass="table col-sm-12" GridLines="None">
    <Columns>
        <asp:TemplateField HeaderText="Turma">
            <ItemTemplate>
                <%#Eval("Turma.Nome")%>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

<asp:Label runat="server" ID="txtSemTurmas">Não existem turmas vinculadas</asp:Label>
