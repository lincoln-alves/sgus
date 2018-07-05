<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SituacaoCursos.aspx.cs" Inherits="Sebrae.Academico.WebForms.Graficos.SituacaoCursos"  ValidateRequest="false"%>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<body style="margin:0;padding:0; height:300px;">
<form id="form1" runat="server">
<asp:scriptmanager runat="server"></asp:scriptmanager>
<rsweb:ReportViewer ID="rpt1" runat="server" ShowFindControls="false" Width="622px" ShowBackButton="false" ShowPageNavigationControls="false" ShowRefreshButton="false">
</rsweb:ReportViewer>
</form>
</body>