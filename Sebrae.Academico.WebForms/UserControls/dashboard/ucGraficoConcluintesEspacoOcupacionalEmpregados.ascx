<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucGraficoConcluintesEspacoOcupacionalEmpregados.ascx.cs" Inherits="Sebrae.Academico.WebForms.UserControls.dashboard.ucGraficoConcluintesEspacoOcupacionalEmpregados" %>

<style>
    #barsChartInterno {
        width: inherit;
        height: inherit;
    }

    #barsLegendInterno ul{
        margin: 0px;
        padding: 0px;
        width: 49%;
        float: left
    }
    #barsLegendInterno li{
        list-style: none;
        padding-left: 10px; 
        font-size: 12px
    }
</style>

<canvas id="barsChartInterno" width="400" height="400"></canvas>
<div id="barsLegendInterno">
    <%=Legenda%>
</div>

<script type="text/javascript">
    gerarGraficoInterno();

    function gerarGraficoInterno(param) {
        var data;
        var detalhadoInterno = false;
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
            detalhadoInterno = true;
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

        var ctxInterno = document.getElementById("barsChartInterno").getContext("2d");
        var myChartInterno = new Chart(ctxInterno, {
            type: 'bar',
            data: data,
            options: {
                onClick: function(response) {
                    myChartInterno.destroy();
                    var param = undefined;
                    if (detalhadoInterno == false) {
                        var activePoints = myChartInterno.getElementsAtEvent(response);
                        if (activePoints.length > 0) {
                            var clickedElementindex = activePoints[0]["_index"];
                            var label = myChartInterno.data.labels[clickedElementindex];
                            var value = myChartInterno.data.datasets[0].data[clickedElementindex];
                            var cor = myChartInterno.data.datasets[0].backgroundColor[clickedElementindex];

                            if (value > 0) {
                                param = {
                                    'label': label,
                                    'value': value,
                                    'cor': cor
                                }
                            }
                        }
                    }
                    gerarGraficoInterno(param);
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