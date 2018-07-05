<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RelatorioQuestionario.aspx.cs" Inherits="Sebrae.Academico.WebForms.Relatorios.Questionario.RelatorioQuestionario" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta charset="UTF-8">
</head>
<body>
    <form id="form1" runat="server">
        <div id="divRespondentes" runat="server" visible="false">
            <table border="1" cellpadding="0" cellspacing="0">
                <thead>
                    <tr>
                        <th runat="server" ID="Questionario" rowspan="2">Questionário</th>
                        <th runat="server" ID="Curso" rowspan="2">Curso</th>
                        <th runat="server" ID="Nome" rowspan="2">Nome</th>
                        <th runat="server" ID="NivelOcupacional" rowspan="2">Nível Ocupacional</th>
                        <th runat="server" ID="UF" rowspan="2">UF</th>
                        <th runat="server" ID="Data" rowspan="2">Data</th>

                        <asp:Repeater ID="rptCabecalho" runat="server" OnItemDataBound="rptPesquisa_OnItemDataBound">
                            <ItemTemplate>
                                <asp:Literal ID="thCabecalho" runat="server"></asp:Literal>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tr>
                    <tr>
                        <asp:Repeater ID="rptTopicos" runat="server">
                            <ItemTemplate>
                                <td><%# Eval("Nome")%><%# Eval("NomeProfessor") != null ? " - Tutor " + Eval("NomeProfessor") : ""%> </td>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptParticipacao" runat="server" OnItemDataBound="rptParticipacao_OnItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td runat="server" ID="Questionario"><%# Eval("Questionario")%></td>
                                <td runat="server" ID="Curso"><%# Eval("Curso")%></td>
                                <td runat="server" ID="Nome"><%# Eval("Nome")%></td>
                                <td runat="server" ID="NivelOcupacional"><%# Eval("NivelOcupacional")%></td>
                                <td runat="server" ID="UF"><%# Eval("UF")%></td>
                                <td runat="server" ID="Data"><%# Eval("Data") != null ? ((DateTime)Eval("Data")).ToString("G") : ""%></td>
                                <%--<td runat="server" ID="Tutor">
				                    <table border="1" cellpadding="0" cellspacing="0">
					                    <asp:Repeater ID="rptTutores" runat="server">
						                    <ItemTemplate>
							                    <tr>
								                    <td><%# Eval("Tutor") ?? "--"%></td>
							                    </tr>
						                    </ItemTemplate>
					                    </asp:Repeater>
				                    </table>
			                    </td>--%>
			                    <asp:Repeater ID="rptNotasTutor" runat="server" OnItemDataBound="rptNotasTutor_ItemDataBound">
				                    <ItemTemplate>
							            <asp:Repeater ID="rptNotas" runat="server">
								            <ItemTemplate> 
										            <td><%# Eval("NotaTexto") ?? "--"%></td>
								            </ItemTemplate>               
							            </asp:Repeater>
				                    </ItemTemplate>
			                    </asp:Repeater>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
        
        <div id="divEstatistico" runat="server" visible="false">
            <table border="1" cellpadding="0" cellspacing="0">
                <thead>
                    <tr>
                        <th>Categoria</th>
                        <th>Tópico Avaliado</th>
                        <th>Média</th>
                        <th>DP</th>
                        <th>Moda</th>
                        <th>Min</th>
                        <th>Max</th>
                        <th>QtdeItens</th>
                        <th>Média Final</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptEstatistico" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("Principal")%></td>
                                <td><%# Eval("Nome")%></td>
                                <td><%# Eval("Media")%></td>
                                <td><%# Eval("DP")%></td>
                                <td><%# Eval("Moda")%></td>
                                <td><%# Eval("Min")%></td>
                                <td><%# Eval("Max")%></td>
                                <td><%# Eval("QtdeItens")%></td>
                                <td><%# Eval("MediaFinal")%></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>

        <div id="divConsolidado" runat="server" visible="false">
            <table class="table col-sm-12" cellspacing="1" border="1">
                <thead>
                    <tr>
                        <th>Solução Educacional</th>
                        <th>Oferta</th>
                        <th>Turma</th>
                        <th>Data Início</th>
                        <th>Data Final</th>
                        <th>Quantidade de alunos na turma</th>
                        <th>Quantidade de alunos que respondeu o questionário</th>
                        <th>Quantidade de alunos que chegaram ao final do curso (Aprovado, Concluído, etc)</th>
                        <th>Percentual de alunos que responderam o questionário com relação ao total de alunos da turma</th>
                        <th>Percentual de alunos que responderam o questionário com relação aos que chegaram ao final do curso (Aprovado, Concluído, etc)</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptConsolidado" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("NM_SolucaoEducacional") %></td>
                                <td><%# Eval("NM_Oferta")%></td>
                                <td><%# Eval("NM_Turma")%></td>
                                <td><%# Eval("DT_Inicio")%></td>
                                <td><%# Eval("DT_Final")%></td>
                                <td><%# Eval("QtdeAlunosTurma")%></td>
                                <td><%# Eval("QtdeAlunosResponderamQuestionario") %></td>
                                <td><%# Eval("QtdeAlunosFinalizaram")%></td>
                                <td><%# Eval("PctAlunosQueResponderamQuestionario")%></td>
                                <td><%# Eval("PctAlunosFinalizaramQueResponderamQuestionario")%></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>