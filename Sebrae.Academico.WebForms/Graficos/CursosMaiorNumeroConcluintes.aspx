<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CursosMaiorNumeroConcluintes.aspx.cs" Inherits="Sebrae.Academico.WebForms.Graficos.CursosMaiorNumeroConcluintes" ValidateRequest="false" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
</head>
<body style="margin:0;padding:0; height:300px;">
<form id="form1" runat="server">
<asp:scriptmanager ID="Scriptmanager1" runat="server"></asp:scriptmanager>
<rsweb:ReportViewer ID="rpt1" runat="server" ShowFindControls="false" Width="622px" ShowBackButton="false" ShowPageNavigationControls="false" ShowRefreshButton="false">
</rsweb:ReportViewer>
<p>*Exibindo apenas os 5 primeiros com mais concluintes de cada curso</p>
</form>
</body>
</html>
