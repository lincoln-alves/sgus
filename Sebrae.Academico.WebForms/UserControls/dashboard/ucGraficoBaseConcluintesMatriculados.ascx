<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucGraficoBaseConcluintesMatriculados.ascx.cs" Inherits="Sebrae.Academico.WebForms.UserControls.dashboard.ucGraficoBaseConcluintesMatriculados" %>

<%
    var id = Guid.NewGuid().ToString().Replace("-", "");
%>
<div id="container-<%=id %>">
    <div id="centerText-<%=id %>" class="centerText-donut text-center">
        <div id="porcentagem-<%=id %>" style="font-size: 40px">
            <asp:Literal ID="ltnPorcentagem" runat="server"></asp:Literal>%
        </div>
        <div id="label-<%=id %>" style="font-weight: 600; margin-top: 0;">
            internos
        </div>
    </div>
    <div id="chart-<%=id %>" style="margin: auto;"></div>
</div>

<div id="retorno-container-<%=id %>" style="position: relative;">
    <div style="font-weight: 600; margin: 0; color: #ffad00;" class="text-center resultado-<%=id %>">
        <asp:Literal ID="ltnQntExterno" runat="server"></asp:Literal>
    </div>
    <h5 style="font-weight: 600; margin: 0; color: #ffad00" class="text-center">
        externos
    </h5>

    <div style="font-weight: 600; margin: 0; color: #0072bb" class="text-center resultado-<%=id %>">
        <asp:Literal ID="ltnQntInterno" runat="server"></asp:Literal>
    </div>
    <h5 style="font-weight: 600; margin: 0; color: #0072bb" class="text-center">
        internos
    </h5>

    <div style="font-weight: 600; margin: 0; color: #68bd00" class="text-center resultado-<%=id %>">
        <asp:Literal ID="ltnQntTotal" runat="server"></asp:Literal>
    </div>
    <h5 style="font-weight: 600; margin: 0; color: #68bd00" class="text-center">
        total
    </h5>
</div>

<script type="text/javascript" src="https://www.google.com/jsapi"></script>
<script type="text/javascript">
    $(document).ready(function () {

        var containerWidth = $('#container-<%=id %>').width();
        var bodyWidth = 0;

        var grafico = $("#container-<%=id %>");

        if (grafico != undefined) {
            var body = $(grafico).parent();
            if (body != undefined) {

                bodyWidth = $(body).width();

                var container = $(body).parent();
                if (container != undefined) {
                    containerWidth = ($(container).height() - 75) / 2;
                }
            }
        }

        // Setar largura e altura responsiva no load.
        if (!window.mobilecheck()) {
            $("#chart-<%=id %>").height(containerWidth - 30);
            $("#chart-<%=id %>").width(containerWidth - 30);
            $('#container-<%=id %>').height(containerWidth - 30);
        }

        google.load("visualization", "1", { packages: ["corechart"] });
        google.setOnLoadCallback(drawChart);
        function drawChart() {
            var data = google.visualization.arrayToDataTable([
                ['Origem', 'Inscritos'],
                <asp:Literal ID="ltnDataSource" runat="server"></asp:Literal>
            ]);

            var options = {
                pieHole: 0.7,
                pieSliceText: 'none',
                legend: 'none',
                slices: {
                    0: { color: '#ffad00' },
                    1: { color: '#0072bb' },

                },
                backgroundColor: 'transparent',
                chartArea: { 'width': '100%', 'height': '100%' },
                tooltip: { trigger: 'none' },
                enableInteractivity: false
            };

            var chart = new google.visualization.PieChart(document.getElementById('chart-<%=id %>'));
            chart.draw(data, options);

            if (!window.mobilecheck()) {
                // Atualizar o tamanho do texto da informação interna do chart.
                var fontSize = containerWidth * 0.18;
                $("#porcentagem-<%=id %>").css('font-size', fontSize > 45 ? 45 : fontSize);

                var fontSizeLabel = containerWidth * 0.07;
                $("#label-<%=id %>").css('font-size', fontSizeLabel > 18 ? 18 : fontSizeLabel);

                var resultados = $(".resultado-<%=id %>");
                if (resultados != undefined) {
                    for (var i = 0; i < resultados.length; i++) {
                        $(resultados[i]).css('font-size', fontSizeLabel * 2.5);
                    }
                }
            }

            // Atualizar posição da informação interna do chart.
            $("#centerText-<%=id %>").css("margin-top", Math.round((($("#chart-<%=id %>").height() / 2) - ($("#centerText-<%=id %>").height() / 2)), 0) + "px");
            $("#centerText-<%=id %>").css("margin-left", Math.round(((bodyWidth / 2) - ($("#centerText-<%=id %>").width() / 2)), 0) + "px");
        }
    });
</script>
