<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SolicitacaoDemandasForm.aspx.cs" Inherits="Sebrae.Academico.WebForms.Relatorios.Demandas.SolicitacaoDemandasForm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta charset="UTF-8">

    <asp:PlaceHolder runat="server">
        <%: System.Web.Optimization.Scripts.Render("~/bundles/master") %>
    </asp:PlaceHolder>
</head>
<body>
    <form id="form1" runat="server">
         <table border="1">
            <thead>
                <tr id="cabecalho">
                    <asp:Repeater ID="rptCabecalho" runat="server">
                        <ItemTemplate>
                            <th runat="server" visible='<%# ExibirCampo(Eval("Nome")) %>' ><%# Eval("Nome") %></th>
                        </ItemTemplate>
                    </asp:Repeater>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rptRelatorio" runat="server" OnItemDataBound="rptRelatorio_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td data-head="Número do Processo" runat="server" visible='<%# ExibirCampo("Número do Processo") %>'><%#Eval("NumeroProcesso") %></td>
                            <td data-head="Usuário Demandante" runat="server" visible='<%# ExibirCampo("Usuário Demandante") %>'><%#Eval("UsuarioDemandante") %></td>
                            <td data-head="Demanda" runat="server" visible='<%# ExibirCampo("Demanda") %>'><%#Eval("Demanda") %></td>
                            <td data-head="Data de Abertura" runat="server" visible='<%# ExibirCampo("Data Abertura") %>'><%#Eval("DataAbertura") %></td>
                            <td data-head="Etapa Atual" runat="server" visible='<%# ExibirCampo("Etapa Atual") %>'><%#Eval("EtapaAtual") %></td>
                            <td data-head="Status" runat="server" visible='<%# ExibirCampo("Status") %>'><%#Eval("Status") %></td>
                            <asp:Repeater runat="server" ID="rptCampos">
                                <ItemTemplate>
                                    <td runat="server" visible='<%# ExibirCampo(Eval("Campo.Nome")) %>'><%#Eval("Resposta") %></td>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </form>
</body>
</html>
