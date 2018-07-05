<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ParticipacaoTrilha.aspx.cs"
    Inherits="Sebrae.Academico.WebForms.Relatorios.ParticipacaoTrilha" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="Label1" runat="server" Text="Trilha"></asp:Label>
        <asp:DropDownList ID="ddlTrilha" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTrilhaNivel_SelectedIndexChanged">
        </asp:DropDownList>
        <asp:Label ID="Label2" runat="server" Text="Trilha Nível"></asp:Label>
        <asp:DropDownList ID="ddlTrilhaNivel" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTrilhaNivel_SelectedIndexChanged">
        </asp:DropDownList>
    </div>
    <div>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt"
            InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"
            Height="" Width="">
            <LocalReport ReportPath="Relatorios\rptParticipacaoTrilha.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="dsParticipacaoItem" Name="dsParticipacaoTrilha" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
        <asp:SqlDataSource ID="dsParticipacaoItem" runat="server" ConnectionString="<%$ ConnectionStrings:cnxSebraeAcademico %>"
            SelectCommand="SELECT * FROM [vwParticipacaoTrilha]"></asp:SqlDataSource>
    </div>
    </form>
</body>
</html>
