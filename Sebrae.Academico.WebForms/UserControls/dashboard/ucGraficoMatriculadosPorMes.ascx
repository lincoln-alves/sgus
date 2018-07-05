<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucGraficoMatriculadosPorMes.ascx.cs" Inherits="Sebrae.Academico.WebForms.UserControls.dashboard.ucGraficoMatriculadosPorMes" %>

<style>
    #chart_div_MatriculadosMes {
        width: inherit;
        height: inherit;
    }
</style>

<script type="text/javascript" src="https://www.google.com/jsapi?autoload={'modules':[{'name':'visualization','version':'1.1','packages':['corechart']}]}"></script>

<div id="container-matriculadosPorMes" style="background-color: white; margin-top: 20px;">
    <div id="chart_div_MatriculadosMes"></div>
</div>

<script type="text/javascript">
    $(document).ready(function () {


            var containerWidth = 0;

            var grafico = $("#container-matriculadosPorMes");

            if (grafico != undefined) {
                var body = $(grafico).parent();
                if (body != undefined) {
                    var container = $(body).parent();
                    if (container != undefined) {
                        containerWidth = $(container).height();
                    }
                }
            }
        
        // Define a altura do container, caso não seja mobile.
        if (!window.mobilecheck()) {
            $("#chart_div_MatriculadosMes").height(containerWidth - 100);
        }
    });

    google.setOnLoadCallback(drawChart);
    function drawChart() {
        var data = google.visualization.arrayToDataTable([
            ['Mês', 'Matriculas','Aprovados'],<%= PegaResultadoSp() %>
    ]);

        var options = {
            titleTextStyle: { color: '#0072bb' },
            legend: { position: 'none' },
            hAxis: { title: '', titleTextStyle: { color: '#333' } },
            vAxis: { minValue: 0 },
            backgroundColor: 'transparent',
            fontSize: 12
        };

        var chart = new google.visualization.AreaChart(document.getElementById('chart_div_MatriculadosMes'));
        chart.draw(data, options);
    }
</script>
