<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="HistoricoDemandasRelatorio.aspx.cs" Inherits="Sebrae.Academico.WebForms.Relatorios.HistoricoDemandas.HistoricoDemandasRelatorio" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="Sebrae.Academico.Dominio.Enumeracao" %>

<script runat="server">
    protected string GetStatus(object id)
    {
        return Enum.GetName(typeof(enumStatusEtapaResposta), id);
    }
   

</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta charset="UTF-8">
</head>
<body>
    <form id="form1" runat="server">

        <asp:Repeater ID="rptEtapas" runat="server" OnItemDataBound="rptEtapas_OnItemDataBound">

            <ItemTemplate>
                <div style="<%# Container.ItemIndex > 0 ? "page-break-before:always;": ""  %>">
                    <div>
                        <strong>
                            <asp:Label ID="lblTitle" runat="server"></asp:Label>
                        </strong>
                        <p>Número da demanda:
                            <asp:Label ID="lblNumeroDemanda" runat="server"></asp:Label></p>
                        <p>Demandado por:
                            <asp:Label ID="lblDemandadoPor" runat="server"></asp:Label></p>
                        <p>Data de Solicitação:
                            <asp:Label ID="lblDataSolicitacao" runat="server"></asp:Label></p>
                    </div>
                    <table role="tabpanel" id="<%# Eval("ID")%>" style="border: 2px solid; margin: 5px; width: 100%" cellpadding="5px" >
                        <tr>
                            <td colspan="2">
                                <h3 style="padding: 5px"><%# Container.ItemIndex + 1 %> - Etapa - <%# Eval("Nome") %> </h3>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:50%; vertical-align:top">
                                <p style="padding: 5px">Requer Aprovação: <%# bool.Parse(Eval("RequerAprovacao").ToString()) ? "Sim" : "Não" %></p>
                                <p style="padding: 5px">Etapa Encaminhada: <%# Eval("EtapaEncaminhamentoUsuario.ID_EtapaEncamihamentoUsuario") != null ? "Sim" : "Não" %></p>
                                
                                <p style="padding: 5px">Concluido por :<%# Eval("Analista.Nome") %></p>
                                <p style="padding: 5px">Unidade : <%# Eval("Analista.Unidade") %> </p>
                                <p style="padding: 5px">Espaço Ocupacional : <%# Eval("Analista.NomeNivel") %> </p>
                                <p style="padding: 5px">CPF: <%# Eval("Analista.Cpf") %></p>
                                <p style="padding: 5px">E-mail: <%# Eval("Analista.Email") %></p>
                            </td>
                            <td style="width:50%; vertical-align:top">
                                <p style="padding: 5px">Status : <%# GetStatus(Eval("Status")) %></p>
                                <p style="padding: 5px"><%# Eval("EtapaEncaminhamentoUsuario.StatusEncaminhamento") != null ? "Status :" : "" %> <%# Eval("EtapaEncaminhamentoUsuario.Status") %></p>
                                <p style="padding: 5px">Data de Preenchimento: <%# Eval("DataPreenchimento") %></p>
                            </td>
                        </tr>
                        <asp:Repeater ID="rptResposta" runat="server" OnItemDataBound="rptResposta_OnItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td colspan="2">
                                        <p style="padding: 5px; margin-top: 10px; line-height: 18px">
                                            <asp:Label ID="lblQuestionario" runat="server" CssClass="questionario enunciado">
                                                <%# Eval("Questao") %>
                                            </asp:Label>
                                            <br />
                                            <asp:Label ID="Label3" runat="server">
                                              <%# Eval("Resposta") %>
                                            </asp:Label>
                                        </p>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </form>
</body>
</html>
