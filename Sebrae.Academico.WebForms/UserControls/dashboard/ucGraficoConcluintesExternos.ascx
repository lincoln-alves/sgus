<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucGraficoConcluintesExternos.ascx.cs" Inherits="Sebrae.Academico.WebForms.UserControls.dashboard.ucGraficoConcluintesExternos" %>

<style>
    #barsChartExterno {
        width: inherit;
        height: inherit;
    }

    #barsLegendExterno ul{
        margin: 0px;
        padding: 0px;
        width: 49%;
        float: left
    }
    #barsLegendExterno li{
        list-style: none;
        padding-left: 10px; 
        font-size: 12px
    }
</style>
<canvas id="barsChartExterno" width="400" height="400"></canvas>
<div id="barsLegendExterno">
    <%=Legenda%>
</div>

<script type="text/javascript">
    gerarGraficoExterno();

    function gerarGraficoExterno(param) {
        var data;
        var detalhadoExterno = false;
        if (param == undefined) {
            data = {
                labels: <%=Titulos%>,
                datasets: [{
                    label: "Quantidade",
                    data: <%=Valores%>,
                    backgroundColor: <%=CoresGrafico%>
                }]
            }
        } else {
            detalhadoExterno = true;
            data = {
                labels: [param.label],
                datasets: [{
                    label: "Quantidade",
                    data: [param.value],
                    backgroundColor: [
                      param.cor
                    ]
                }]
            }
        }

        var ctxExterno = document.getElementById("barsChartExterno").getContext("2d");
        var myChartExterno = new Chart(ctxExterno, {
            type: 'bar',
            data: data,
            options: {
                onClick: function(response) {
                    myChartExterno.destroy();
                    var param = undefined;
                    if (detalhadoExterno == false) {
                        var activePoints = myChartExterno.getElementsAtEvent(response);
                        if (activePoints.length > 0) {
                            var clickedElementindex = activePoints[0]["_index"];
                            var label = myChartExterno.data.labels[clickedElementindex];
                            var value = myChartExterno.data.datasets[0].data[clickedElementindex];
                            var cor = myChartExterno.data.datasets[0].backgroundColor[clickedElementindex];

                            if (value > 0) {
                                param = {
                                    'label': label,
                                    'value': value,
                                    'cor': cor
                                }
                            }
                        }
                    }
                    gerarGraficoExterno(param);
                },
                legend: {
                    display: false
                },
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true
                        }
                    }]
                },
            }
        });
    }
</script>