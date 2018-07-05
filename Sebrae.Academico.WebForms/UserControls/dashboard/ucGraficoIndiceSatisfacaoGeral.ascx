<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucGraficoIndiceSatisfacaoGeral.ascx.cs"
    Inherits="Sebrae.Academico.WebForms.UserControls.ucGraficoIndiceSatisfacaoGeral" %>

<asp:Panel ID="pnlAviso" runat="server" Visible="false">
    <p style="color: red; text-align: center;"><b>Não foi possível recuperar as informações. <br/>Nossa equipe de suporte já está verificando o ocorrido. <br/>Tente novamente em alguns instantes.</b></p>
    
    <div class="hidden">
        <asp:Literal ID="ltrAvisoErro" runat="server"></asp:Literal>
    </div>
</asp:Panel>
<asp:Panel ID="pnlGrafico" runat="server" Visible="true">
    <canvas id="cvs-geral" data-toggle="tooltip" data-placement="top" title="<%= ObterValorFormatado() %>">[No canvas support]</canvas>
    <script type="text/javascript">
        (function ($) {
            var grafico = $("#cvs-geral");

            if (grafico != undefined) {
                var body = $(grafico).parent();
                if (body != undefined) {
                    var container = $(body).parent();
                    if (container != undefined) {

                        // Definir altura do gráfico.
                        if (!window.mobilecheck()) {
                            grafico.attr("height", $(container).height() + "px");
                        } else {
                            grafico.attr("width", $(container).width() + "px");
                        }

                        // Centralizar gráfico.
                        grafico.css("margin-left", (($(container).width() / 2) - (grafico.width() / 2)) + "px");
                    }
                }
            }

            $(document).ready(function() {

                if (!$('#cvs-geral').is(':visible'))return;
                $('#cvs-geral').tooltip();
                new RGraph.Meter({
                    id: 'cvs-geral',
                    min: 0,
                    max: 10,
                    value: <%= ObterValor() %>,
                    options: {
                        backgroundColor: 'transparent',
                        //linewidthSegments: 5,
                        textSize: 0,
                        segmentRadiusStart: (window.mobilecheck() ? 75 : 100),
                        border: 0,
                        tickmarksSmallNum: 0,
                        tickmarksBigNum: 0,
                        gutterTop:0,
                        colorsRanges: [
                            [0, 4.9, '#ed1c24'],
                            [5, 6.9, '#f3ec1a'],
                            [7, 7.9, '#94ca54'],
                            [8, 8.9, '#1fb151'],
                            [9, 9.4, '#95daf8'],
                            [9.5, 10, '#64c7f1']
                        ]
                    }
                }).on('beforedraw', function(obj) {
                    RGraph.clear(obj.canvas, 'transparent');
                }).draw();
            });
        })(jQuery);
    </script>
</asp:Panel>
