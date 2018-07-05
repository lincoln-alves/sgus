<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="Dashboard.aspx.cs" Inherits="Sebrae.Academico.WebForms.Dashboard" %>

<%@ Register Src="~/UserControls/dashboard/ucGraficoMatriculadosUF.ascx" TagPrefix="uc" TagName="ucGraficoMatriculadosUF" %>
<%@ Register Src="~/UserControls/dashboard/ucGraficoBaseConcluintesMatriculados.ascx" TagPrefix="uc" TagName="ucGraficoBaseConcluintesMatriculados" %>
<%@ Register Src="~/UserControls/dashboard/ucGraficoMatriculadosPorMes.ascx" TagPrefix="uc" TagName="ucGraficoMatriculadosPorMes" %>
<%@ Register Src="~/UserControls/dashboard/ucGraficoIndiceSatisfacaoGeral.ascx" TagPrefix="uc" TagName="ucGraficoIndiceSatisfacaoGeral" %>
<%@ Register Src="~/UserControls/dashboard/ucGraficoTop5Cursos.ascx" TagPrefix="uc" TagName="ucGraficoTop5Cursos" %>
<%@ Register Src="~/UserControls/dashboard/ucGraficoConcluintesEspacoOcupacionalEmpregados.ascx" TagPrefix="uc" TagName="ucGraficoConcluintesEspacoOcupacionalEmpregados" %>
<%@ Register Src="~/UserControls/dashboard/ucGraficoConcluintesExternos.ascx" TagPrefix="uc" TagName="ucGraficoConcluintesExternos" %>
<%@ Register Src="~/UserControls/dashboard/ucGraficoIndiceSatisfacaoCredenciados.ascx" TagPrefix="uc" TagName="ucGraficoIndiceSatisfacaoCredenciados" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="/js/dashboard/RGraph.common.core.js"></script>
    <script type="text/javascript" src="/js/dashboard/RGraph.meter.js"></script>
    <script type="text/javascript" src="/js/home/Relatorios.js"></script>
    <script type="text/javascript" src="js/dashboard/Chart.min.js"></script>
    <script type="text/javascript" src="/js/window.mobilecheck.js"></script>
    <link href="/css/dashboard_graficos.css" rel="stylesheet" />

    <div>
        <div class="row">
            <div class="col-sm-12 col-md-4 col-lg-3 pull-right">
                <button class="btn btn-default" id="btnProximoMenu" onclick="DisableRedirection(); location.href = '/Painel.aspx'; return false;">Painel de Indicadores</button>
                <button class="btn btn-success" id="btnAtualizar">Atualizar</button>
            </div>

            <div class="col-sm-12 col-md-8 col-lg-5 pull-right">
                
                    <ul class="list-inline" style="padding-right: 0;">
                        <li style="-moz-min-width: 115px !important; -ms-min-width: 115px !important; -o-min-width: 115px !important; -webkit-min-width: 115px !important; min-width: 115px !important; margin: 0;">
                            <asp:TextBox ID="txtDataInicial" Width="110" CssClass="form-control data" runat="server"></asp:TextBox>
                        </li>
                        <li style="-moz-min-width: 115px !important; -ms-min-width: 115px !important; -o-min-width: 115px !important; -webkit-min-width: 115px !important; min-width: 115px !important; margin: 0;">
                            <asp:TextBox ID="txtDataFinal" Width="110" CssClass="form-control data" runat="server"></asp:TextBox>
                        </li>
                        <li style="-moz-min-width: 125px !important; -ms-min-width: 125px !important; -o-min-width: 125px !important; -webkit-min-width: 125px !important; min-width: 125px !important; margin: 0;">
                            <asp:DropDownList ID="cbxUf" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>
                        </li>
                        <li style="-moz-min-width: 80px !important; -ms-min-width: 80px !important; -o-min-width: 80px !important; -webkit-min-width: 80px !important; min-width: 80px !important; margin: 0;">
                            <asp:Button ID="btnFiltrar" ClientIDMode="Static" runat="server" Text="Fitrar" CssClass="btn btn-danger" OnClick="btnFiltrar_OnClick" />
                        </li>
                    </ul>
                
            </div>
        </div>
    </div>

    <asp:ScriptManager runat="server"></asp:ScriptManager>

    <div id="cortina" class="cortina">
    </div>

    <div id="dashboard-maincontainer" class="dashboard-bg">
        <div id="dashboard-proximoMenu-mobile" class="panel panel-default" style="display: none;">
            <div class="panel-body">
                <div class="form-group">
                    <asp:Button ID="btnProximoMenu" runat="server" Text="Fitrar" CssClass="btn btn-default btn-block" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-4 col-dashboard">
                <div class="panel panel-default panel-dashboard">
                    <div class="dashboard-title">
                        Matriculados por UF
                        <p>% em relação ao público alvo</p>
                    </div>
                    <div class="panel-body">
                        <uc:ucGraficoMatriculadosUF runat="server" ID="ucGraficoMatriculadosUF" />
                    </div>
                </div>
            </div>
            <div class="col-lg-4 col-dashboard">
                <div class="panel panel-default panel-dashboard">
                    <div class="dashboard-title">
                        TOP 5 Cursos
                    </div>
                    <div class="panel-body top5-body">
                        <uc:ucGraficoTop5Cursos runat="server" ID="ucGraficoTop5Cursos" />
                    </div>
                </div>
            </div>
            <div class="col-lg-2 col-dashboard">
                <div class="panel panel-default panel-dashboard">
                    <div class="dashboard-title">
                        Matrículas
                    </div>
                    <div class="panel-body">
                        <uc:ucGraficoBaseConcluintesMatriculados runat="server" ID="ucGraficoBaseConcluintesMatriculados" Tipo="1" />
                    </div>
                </div>
            </div>
            <div class="col-lg-2 col-dashboard">
                <div class="panel panel-default panel-dashboard">
                    <div class="dashboard-title">
                        Aprovações
                    </div>
                    <div class="panel-body">
                        <uc:ucGraficoBaseConcluintesMatriculados runat="server" ID="ucGraficoBaseConcluintesMatriculados1" Tipo="2" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-4 col-dashboard">
                <div class="panel panel-default panel-dashboard">
                    <div class="dashboard-title">
                        Matrículas por mês
                    </div>
                    <div>
                        <uc:ucGraficoMatriculadosPorMes ID="ucGraficoMatriculadosPorMes" runat="server" />
                    </div>
                </div>
            </div>
            <div class="col-lg-2 col-dashboard">
                <div class="panel panel-default panel-dashboard">
                    <div class="dashboard-title">
                        Aprovados
                        <br />
                        Internos
                    </div>
                    <div class="panel-body">
                        <uc:ucGraficoConcluintesEspacoOcupacionalEmpregados ID="ucGraficoConcluintesEspacoOcupacionalEmpregados" runat="server" />
                    </div>
                </div>
            </div>
            <div class="col-lg-2 col-dashboard">
                <div class="panel panel-default panel-dashboard">
                    <div class="dashboard-title">
                        Aprovados
                        <br />
                        Externos
                    </div>
                    <div class="panel-body">
                        <uc:ucGraficoConcluintesExternos ID="ucGraficoConcluintesExternos" runat="server" />
                    </div>
                </div>
            </div>
            <div class="col-lg-4 col-dashboard">
                <div class="row">
                    <div class="col-xs-12">
                        <div class="panel panel-default panel-dashboard" data-half="1">
                            <div class="dashboard-title">
                                Satisfação geral
                            </div>
                            <div class="panel-body">
                                <uc:ucGraficoIndiceSatisfacaoGeral runat="server" ID="ucGraficoIndiceSatisfacaoGeral" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-12">
                        <div class="panel panel-default panel-dashboard" data-half="1">
                            <div class="dashboard-title">
                                Satisfação na Formação de Multiplicadores
                            </div>
                            <div class="panel-body">
                                <uc:ucGraficoIndiceSatisfacaoCredenciados runat="server" ID="ucGraficoIndiceSatisfacaoCredenciados" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="js/jquery/jquery.mobile.custom.min.js"></script>
    <script type="text/javascript">
        $("body").addClass('home');

        // Não executa as definições de altura caso o usuário esteja acessando de um browser mobile.
        if (!window.mobilecheck()) {
            // Definir as alturas dos dashboards para 50% do espaço (metade do viewport menos a altura do navbar do topo)
            var defaultHeight = ($(document).height() / 2) - 50;

            var dashboards = $('.panel-dashboard');
            if (dashboards != undefined) {
                for (var i = 0; i < dashboards.length; i++) {
                    if (dashboards[i] != undefined) {
                        if (dashboards[i].dataset != undefined && dashboards[i].dataset.half == "1") {
                            $(dashboards[i]).height((defaultHeight / 2) - 5);
                        } else {
                            $(dashboards[i]).height(defaultHeight);
                        }
                    }
                }
            }
        }

        var reloadPage = true;

        function DisableRedirection() {
            reloadPage = false;
        }

        $(function () {

            setTimeout(function () {
                $("#cortina").fadeOut();
            }, 500);

            $("#btnAtualizar").click(function () {
                location.reload(true);
            });

        });

        if (window.mobilecheck()) {
            $(window).on("orientationchange", function () {
                $("#cortina").show();

            });

            // Exibir filtros no mobile.
            $('#dashboard-filtros-mobile').show();
            $('#dashboard-proximoMenu-mobile').show();
        }

        $('.data').mask("99/99/9999");

        function adcionarDatasNosInputs() {
            var datas = formtarDataParaImptsDaUrl();
            $('input[type="text"]').each(function (index, input) {
                $(input).val(datas[index]);
            });
        }

        function formtarDataParaImptsDaUrl() {
            return window.location.search
               .replace(/[?&if ]+/g, '')
               .split("=")
               .filter(function (data) { return data != "" })
        }

    </script>
</asp:Content>
