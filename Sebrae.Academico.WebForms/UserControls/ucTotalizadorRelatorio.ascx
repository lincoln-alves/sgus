<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucTotalizadorRelatorio.ascx.cs" Inherits="Sebrae.Academico.WebForms.UserControls.ucTotalizadorRelatorio" %>
<%@ Import Namespace="System.Net" %>

<div class="panel-group" ID="grupototalizador" runat="server" Visible="False">
    <style>
        #doublescroll {
            overflow: auto;
            overflow-y: hidden;
        }

            #scroll-container {
                margin: 0;
                padding: 1em;
            }
    </style>

    <div class="panel panel-default">
        <div class="panel-heading">
            <a data-toggle="collapse" data-parent="#grupototalizador" href="#Totalizador">
                <%= NomeTotalizador ?? "Totalizador" %>
            </a>
        </div>
        <div id="Totalizador" class="accordion-body in">
            <div id="doublescroll">
                <div id="scroll-container" class="table-responsive">
                    <table id="tabela_totalizador" class="table">
                        <tbody>
                            <asp:Repeater ID="rptTotalizador" runat="server">
                                <ItemTemplate>
                                    <tr />
                                        <td <%# ((bool)Eval("IsAgrupado") || (bool)Eval("DadoIsLista")) ? "" : "colspan=\"2\"" %>>
                                            <%# WebUtility.HtmlDecode(Eval("Descricao").ToString()) %>
                                        </td>
                                        <td class="text-right" <%# ((bool)Eval("IsAgrupado") || (bool)Eval("DadoIsLista")) ? "colspan=\"2\"" : "style=\"width: 300px;\"" %>>
                                            <%# WebUtility.HtmlDecode(Eval("DadoFormatado").ToString()) %>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
    
    <script>
        $(function () {
            function doubleScroll(element) {
                var scrollbar = document.createElement('div');
                scrollbar.id = 'scroll-top';
                scrollbar.appendChild(document.createElement('div'));
                scrollbar.style.overflow = 'auto';
                scrollbar.style.overflowY = 'hidden';
                scrollbar.firstChild.style.width = $('#tabela_totalizador').width() + (parseInt($('#scroll-container').css('padding')) * 2) + 'px';
                scrollbar.firstChild.style.paddingTop = '0';
                scrollbar.firstChild.style.height = '0';
                scrollbar.firstChild.appendChild(document.createTextNode('\xA0'));

                element.parentNode.insertBefore(scrollbar, element);

                $('#scroll-top').scroll(function () {
                    $('#scroll-container')
                        .scrollLeft($('#scroll-top').scrollLeft());
                });

                $('#scroll-container').scroll(function () {
                    $('#scroll-top')
                        .scrollLeft($('#scroll-container').scrollLeft());
                });

                // Esconder o totalizadore após a criação do scroll do topo.
                $('#Totalizador').collapse('hide');
            }

            doubleScroll(document.getElementById('doublescroll'));
        });
    </script>
</div>
