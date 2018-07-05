<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConcluintesPorPerfilETipo.aspx.cs" Inherits="Sebrae.Academico.WebForms.Graficos.ConcluintesPorPerfilETipo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
<body style="padding: 0px;">
    <form id="form1" runat="server">
    <div>
   
      <script type="text/javascript">
                            (function(){
                                googleCharts.push(function drawChart() {
                                    var data = google.visualization.arrayToDataTable([
                                        ['Tipo', 'Perfil', 'Concluíntes', '% Válido'],
                                        <asp:Repeater ID="rptRelatorio" onitemdatabound="rptRelatorio_ItemDataBound" runat="server">
                                             <ItemTemplate>
                                                <asp:Literal ID="literal" runat="server"></asp:Literal>
                                             </ItemTemplate>
                                        </asp:Repeater>
                                    ]);
                                    var options = {legend: { position: 'bottom' }};
                                    var chart = new google.visualization.Table(document.getElementById('chart'));
                                    chart.draw(data, options);
                                });
                            })()
    </script>
    
    <div id="chart" style="width: 100%; height: 100%;">
    </div>


    </div>
    </form>
</body>
</html>
