<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="Solicitacoes.aspx.cs" Inherits="Sebrae.Academico.WebForms.Relatorios.Solicitacoes.Solicitacoes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:GridView ID="dgRelatorio" runat="server" AutoGenerateColumns="False" AllowSorting="True"
        CssClass="table col-sm-12" GridLines="None" OnSorting="dgRelatorio_Sorting" AllowPaging="True"
        OnPageIndexChanging="dgRelatorio_PageIndexChanging" PageSize="10">
        <Columns>
            <asp:BoundField DataField="NomeAmigavel" HeaderText="Nome" SortExpression="NomeAmigavel" />
            <asp:BoundField DataField="Saida" HeaderText="Extensão" SortExpression="Saida" />
            <asp:BoundField DataField="DataSolicitacao" HeaderText="Data da solicitação" SortExpression="DataSolicitacao" />
            <asp:BoundField DataField="DataGeracao" HeaderText="Data da geração" SortExpression="DataGeracao" />
            
            <%--<asp:TemplateField>
                <ItemTemplate>
                    <%# Eval("Falha") != null && bool.Parse(Eval("Falha").ToString()) ?  : "" %>
                </ItemTemplate>
                <ItemStyle Width="60px" />
            </asp:TemplateField>--%>

            <asp:TemplateField>
                <ItemTemplate>
                    
                    <%# Eval("Falha") != null && bool.Parse(Eval("Falha").ToString()) ? "<span class=\" label label-danger\" style=\"display: block; width: 100%; line-height: 16px;\">FALHA</span>" :
                        ("<a href=\""
                        + (Eval("Arquivo") != null ? "/MediaServer.ashx?Identificador=" + Eval("Arquivo.ID") : "#")
                        + "\""
                        + (Eval("DataGeracao") == null ? " disabled=\"disabled\" " : "")
                        + "class=\"btn btn-default btn-xs btn-block\"><span class=\"glyphicon glyphicon-download-alt\"></span>&nbsp;"
                        + (Eval("Arquivo") == null ? "Aguardando" : "Baixar")
                        + "</a>")
                    %>
                    <%--<a href='<%# Eval("Arquivo") != null ? "/MediaServer.ashx?Identificador=" + Eval("Arquivo.ID") : "#" %>' <%# Eval("DataGeracao") == null ? "disabled=disabled" : "" %> class="btn btn-default btn-xs">
                        <span class="glyphicon glyphicon-download-alt"></span>&nbsp;<%# Eval("Arquivo") == null ? "Aguardando" : "Baixar" %>
                    </a>--%>
                </ItemTemplate>
                <ItemStyle Width="60px" />
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
        </EmptyDataTemplate>
    </asp:GridView>

</asp:Content>
