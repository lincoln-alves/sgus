<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="HistoricoTurma.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.Turma.HistoricoTurma" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h3 id="nomeTurma" class="text-center" runat="server"></h3>

    <div class="form-group">
        <h4>Responsáveis
        </h4>
        <asp:GridView ID="dgvResponsaveis" runat="server" CssClass="table" GridLines="None" AutoGenerateColumns="false" AllowPaging="True">
            <Columns>
                <asp:TemplateField HeaderText="Responsável">
                    <ItemTemplate>
                        <%#Eval("Responsavel.Nome")%> (<%#Eval("Responsavel.CPF")%>)
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <Columns>
                <asp:TemplateField HeaderText="Alterado por">
                    <ItemTemplate>
                        <%#Eval("UsuarioAlteracao.Nome")%> (<%#Eval("UsuarioAlteracao.CPF")%>)
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <Columns>
                <asp:TemplateField HeaderText="Data da alteração">
                    <ItemTemplate>
                        <%# Convert.ToDateTime(Eval("Data")).ToString("dd/MM/yyyy HH:mm:ss") %>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
            </EmptyDataTemplate>
        </asp:GridView>
    </div>
    <div class="form-group">
        <h4>Consultores Educacionais
        </h4>
        <asp:GridView ID="dgvConsultores" runat="server" CssClass="table" GridLines="None" AutoGenerateColumns="false" AllowPaging="True">
             <Columns>
                <asp:TemplateField HeaderText="Consultor">
                    <ItemTemplate>
                        <%#Eval("ConsultorEducacional.Nome")%> (<%#Eval("ConsultorEducacional.CPF")%>)
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <Columns>
                <asp:TemplateField HeaderText="Alterado por">
                    <ItemTemplate>
                        <%#Eval("UsuarioAlteracao.Nome")%> (<%#Eval("UsuarioAlteracao.CPF")%>)
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <Columns>
                <asp:TemplateField HeaderText="Data da alteração">
                    <ItemTemplate>
                        <%# Convert.ToDateTime(Eval("Data")).ToString("dd/MM/yyyy HH:mm:ss") %>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
            </EmptyDataTemplate>
        </asp:GridView>
    </div>

    <asp:Button ID="btnVoltar" runat="server" Text="Voltar" OnClick="btnVoltar_OnClick" CssClass="btn btn-default mostrarload" />
</asp:Content>
