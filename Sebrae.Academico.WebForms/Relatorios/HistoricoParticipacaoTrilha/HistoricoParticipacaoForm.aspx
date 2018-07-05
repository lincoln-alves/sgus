<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HistoricoParticipacaoForm.aspx.cs" Inherits="Sebrae.Academico.WebForms.Relatorios.HistoricoParticipacaoTrilha.HistoricoParticipacaoForm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta charset="UTF-8">
</head>
<body>
    <form id="form1" runat="server">
        <table border="1" cellpadding="0" cellspacing="0">
            <tbody>
                <asp:Repeater ID="rptUsuariosTrilha" runat="server" OnItemDataBound="rptUsuariosTrilha_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <th bgcolor="#d5d6d6" class="FiltroNome">Nome</th>
                            <th bgcolor="#d5d6d6" class="FiltroCPF" runat="server" id="Th1">CPF</th>
                            <th bgcolor="#d5d6d6" class="FiltroNivelOcupacional">Nível Ocupacional</th>
                            <th bgcolor="#d5d6d6" class="FiltroUF">UF</th>
                            <th bgcolor="#d5d6d6" class="FiltroStatusMatricula">Status Matricula</th>
                            <th bgcolor="#d5d6d6" class="FiltroTrilha">Trilha</th>
                            <th bgcolor="#d5d6d6" class="FiltroTrilhaNivel">Trilha Nível</th>
                            <th bgcolor="#d5d6d6" class="filtroDataAlteracaoStatus" runat="server" id="Th2">Data de alteração do Status do Participante</th>
                            <th bgcolor="#d5d6d6" class="filtroDataInclusao" runat="server" id="Th3">Data de Inclusão na Trilha</th>
                        </tr>
                        <tr>
                            <td class="FiltroNome"><%#Eval("Usuario.Nome")%></td>
                            <td class="FiltroCPF" runat="server" id="CPF"><%# Eval("Usuario.CPF")%></td>
                            <td class="FiltroNivelOcupacional"><%# Eval("NivelOcupacional.Nome")%></td>
                            <td class="FiltroUF"><%# Eval("Uf.Nome")%></td>
                            <td class="FiltroStatusMatricula"><%#Eval("StatusMatriculaFormatado")%></td>
                            <td class="FiltroTrilha"><%#Eval("TrilhaNivel.Trilha.Nome")%></td>
                            <td class="FiltroTrilhaNivel"><%#Eval("TrilhaNivel.Nome")%></td>
                            <td class="FiltroDataAlteracaoStatus" runat="server" id="DataAlteracaoStatusParticipacao"><%#Eval("DataAlteracao")%></td>
                            <td class="filtroDataInicio" runat="server" id="DataInclusaoTrilha"><%#Eval("DataInicio")%></td>
                        </tr>

                        <%--Soluções da trilha--%>
                        <tr>
                            <th bgcolor="#999" colspan="9">Soluções da trilha</th>
                        </tr>
                        <tr>
                            <th colspan="3">SOLUÇÕES SEBRAE</th>
                            <th colspan="3">CARGA HORÁRIA</th>
                            <th colspan="3">QUANTIDADE DE MOEDAS</th>
                        </tr>
                        <asp:Repeater ID="rptPontosSebraeUsuario" runat="server" OnItemDataBound="rptPontosSebraeUsuario_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <th colspan="3" class="nome"><%#Eval("Nome") %></th>
                                    <td colspan="3" class="CargaHoraria"><%#Eval("CargaHoraria") %>h</td>
                                    <td colspan="3" class="Objetivo"><%#Eval("Moedas") %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr>
                            <th colspan="2">Total Objeto/Solução</th>
                            <td runat="server" class="Moedas" id="rptPontosSebraeUsuarioObjetivos"></td>
                            <th colspan="2">Total de Horas</th>
                            <td runat="server" class="MoedasProvaFinal" id="rptPontosSebraeUsuarioTotalHoras"></td>
                            <th colspan="2">Total Moedas</th>
                            <td runat="server" class="Total" id="rptPontosSebraeUsuarioMoedas"></td>
                        </tr>
                        <%--Soluções da trilha--%>

                        <%--curços online--%>
                        <tr>
                            <th bgcolor="#999" colspan="9">Cursos online UCSebrae</th>
                        </tr>
                        <tr>
                            <th colspan="3">SOLUÇÕES EDUCACIONAL</th>
                            <th colspan="2">CARGA HORÁRIA</th>
                            <th colspan="2">QUANTIDADE DE MOEDAS</th>
                            <th colspan="2">PERÍODO DE REALIZAÇÃOS</th>
                        </tr>
                        <asp:Repeater ID="rptsolucaoesDaTrilhaOnline" runat="server" OnItemDataBound="rptsolucaoesDaTrilhaOnline_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <th colspan="3" class="nome"><%#Eval("Nome") %></th>
                                    <td colspan="2" class="CargaHoraria"><%#Eval("CargaHoraria") %> h</td>
                                    <td colspan="2" class="Moedas"><%#Eval("Moedas") %></td>
                                    <td colspan="2" class="Periodo"><%#Eval("DataInicio") %>  <%#Eval("DataFim") != null ? "A " +Eval("DataFim") : "" %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr>
                            <th colspan="2">Total Objeto/Solução</th>
                            <td runat="server" class="Moedas" id="rptsolucaoesDaTrilhaOnlineObjetivo"></td>
                            <th colspan="2">Total de Horas</th>
                            <td runat="server" class="Total" id="rptsolucaoesDaTrilhaOnlineTotal"></td>
                            <th colspan="2">Total Moedas</th>
                            <td runat="server" class="MoedasProvaFinal" id="rptsolucaoesDaTrilhaOnlineMoedasProvaFinal"></td>
                        </tr>
                        <%--curços online--%>


                        <%--soluções do trilheiro--%>
                        <tr>
                            <th bgcolor="#999" colspan="9">Soluções do Trilheiro</th>
                        </tr>
                        <tr>
                            <th colspan="3">SOLUÇÕES TRILHEIRO</th>
                            <th colspan="2">TOTAL DE MOEDAS DE PRATA</th>
                            <th colspan="2">TOTAL DE MOEDAS DE OURO</th>
                            <th colspan="2">TOTAL DE HORAS</th>
                        </tr>
                        <asp:Repeater ID="rptsolucaoesDoTrilheiro" runat="server" OnItemDataBound="rptsolucaoesDoTrilheiro_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td colspan="3"><%#Eval("Nome") %></td>
                                    <td colspan="2"><%#Eval("MoedasPratas") %></td>
                                    <td colspan="2"><%#Eval("MoedasOuro") %></td>
                                    <td colspan="2"><%#Eval("CargaHoraria") %>h</td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <%--soluções do trilheiro--%>

                        <%--Desempenho geral--%>
                        <tr>
                            <th bgcolor="#999" colspan="9">Desempenho Geral</th>
                        </tr>
                        <tr>
                            <th>QUANTIDADE DE MOEDAS</th>
                            <th>QUANTIDADE DE MEDALHAS</th>
                            <th>TROFÉUS ALCANÇADOS</th>
                            <th colspan="2">TOTAL DE HORAS CERTIFICADAS NA TRILHA/NÍVEL</th>
                            <th colspan="2">TOTAL DE HORAS REGISTRADAS NA TRILHA/NÍVEL</th>
                            <th colspan="2">TOTAL OBJETO/SOLUÇÃO</th>
                        </tr>
                        <asp:Repeater ID="rptsolucaoesDesempenhoGeral" runat="server" OnItemDataBound="rptsolucaoesDesempenhoGeral_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td><%#Eval("Moedas") %></td>
                                    <td><%#Eval("Medalhas") %></td>
                                    <td><%#Eval("Trofeus") %></td>
                                    <td colspan="2"><%#Eval("HorasCertificadas") %>h</td>
                                    <td colspan="2"><%#Eval("HorasRegistradas") %>h</td>
                                    <td colspan="2"><%#Eval("Solucoes") %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <%--Desempenho geral--%>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>

    </form>
</body>
</html>
