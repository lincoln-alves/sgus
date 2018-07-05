<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CertificadosPorCargoETema.aspx.cs"
    Inherits="Sebrae.Academico.WebForms.Graficos.CertificadosPorCargoETema" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="/css/bootstrap.css" rel="stylesheet">
    <!-- Custom styles for this template -->
    <link href="/css/style-sgus20.css" rel="stylesheet">
    <script type="text/javascript" src="https://www.google.com/jsapi?autoload={
            'modules':[{
              'name':'visualization',
              'version':'1',
              'packages':['corechart', 'table']
            }]
          }">
    </script>
    <script type="text/javascript">
        google.setOnLoadCallback(drawCharts);

        function drawCharts() {
            for (var i in googleCharts) {
                googleCharts[i]();
            }
        }

        var googleCharts = new Array();
    </script>
</head>
<body style="margin: 0; padding: 0; height: 300px;">
    <form id="form1" runat="server">
    <script type="text/javascript">
                            (function(){
                                var titulo = 'Situação dos Alunos em Curso Online';
                                googleCharts.push(function drawChart() {
                                    var data = google.visualization.arrayToDataTable([
                                        <asp:Literal ID="literalTitulos" runat="server"></asp:Literal>,
                                        <asp:Repeater ID="rptRelatorio" onitemdatabound="rptRelatorio_ItemDataBound" runat="server">
                                             <ItemTemplate>
                                                <asp:Literal ID="literalDados" runat="server"></asp:Literal>
                                             </ItemTemplate>
                                        </asp:Repeater>
                                    ]);
                                    var options = {title: titulo, legend: { position: 'bottom' }};
                                    var chart = new google.visualization.Table(document.getElementById('chart'));
                                    chart.draw(data, options);
                                });
                            })()
    </script>
    
    <div id="chart" style="width: 100%; height: 100%;">
    </div>


    <asp:GridView runat="server" ID="grdRelatorio" AutoGenerateColumns="false" Visible="false" CssClass="table col-sm-12"
        OnDataBound="grdRelatorio_DataBound">
        <Columns>
            <asp:BoundField HeaderText="Espaço Ocupacional" DataField="EspacoOcupacional" />
            <asp:BoundField DataField="SolucaoEducacional1" />
            <asp:BoundField DataField="SolucaoEducacional2" />
            <asp:BoundField DataField="SolucaoEducacional3" />
            <asp:BoundField DataField="SolucaoEducacional4" />
            <asp:BoundField DataField="SolucaoEducacional5" />
            <asp:BoundField HeaderText="Certificações" DataField="Certificacoes" />
        </Columns>
    </asp:GridView>
    </form>
</body>
</html>
