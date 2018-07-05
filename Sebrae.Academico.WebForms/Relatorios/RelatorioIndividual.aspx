<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RelatorioIndividual.aspx.cs" Inherits="Sebrae.Academico.WebForms.Relatorios.RelatorioIndividual" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">



<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Styles/style_SGUS.css" rel="stylesheet" type="text/css" />
</head>
<body>
   <form id="form1" runat="server">
    <div class="formLupa">
        <ul>
            <li><label for="txtUsuarioSelect">
                <asp:Label ID="Label1" runat="server" Text="Selecione o Usuário"></asp:Label> </label>
                <uc:LupaUsuario runat="server" ID="txtUsuarioSelect" ClientIDMode="Static" OnUserSelected="UserSelectedHandler"   />
            </li>
            <%--<li>
                <asp:Button ID="btnPesquisar" runat="server" Text="Gerar Relatório" 
                    onclick="btnPesquisar_Click" />
            
            </li>--%>
        </ul>
    </div>
    <hr />
    <br/>
    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="931px" 
        Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" 
        WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" 
       HyperlinkTarget="_blank">
        <LocalReport ReportPath="Relatorios\rptRelatorioIndividual.rdlc">
            <DataSources>
                <rsweb:ReportDataSource DataSourceId="SqlDataSource1" 
                    Name="dsRelatorioIndividual" />
            </DataSources>
        </LocalReport>
    </rsweb:ReportViewer>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:cnxSebraeAcademico %>" 
        SelectCommand="sp_relatorio_individual" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter DefaultValue="0" Name="pID_Usuario" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    

    </form>
</body>
</html>
