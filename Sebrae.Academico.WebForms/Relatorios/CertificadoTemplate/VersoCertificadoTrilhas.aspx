<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VersoCertificadoTrilhas.aspx.cs" Inherits="Sebrae.Academico.WebForms.Relatorios.CertificadoTemplate.VersoCertificadoTrilhas" %>

<form id="form1" runat="server">
    <start>
        <div>
            <div style="font-size: 0.5cm; text-align: center;">
                <strong>
                    <asp:Literal runat="server" ID="ltrNomeNivel"></asp:Literal>
                </strong>
            </div>
            <asp:Repeater ID="rptLojas" runat="server" OnItemDataBound="rptLojas_OnItemDataBound">
                <ItemTemplate>
                    <div>
                        <strong>Etapa da trilha:</strong>&nbsp;<%# Eval("NomeExibicao")%>
                    </div>
                    <asp:Repeater ID="rptObjetivos" runat="server" OnItemDataBound="rptObjetivos_OnItemDataBound">
                        <ItemTemplate>
                            <div style="padding-left: 1cm">
                                <strong>Objetivo:</strong>&nbsp;<%# Eval("Nome")%>
                            </div>
                            <div style="padding-left: 2cm">
                                <asp:Repeater ID="rptSolucoesSebrae" runat="server">
                                    <ItemTemplate>
                                        <div>
                                            <strong>Solução Sebrae:</strong>&nbsp;<%# Eval("Nome")%>. <strong>Forma de aquisição:</strong>&nbsp;<%# Eval("FormaAquisicao.Nome")%>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </start>
</form>