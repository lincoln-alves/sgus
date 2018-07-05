<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"    
    CodeBehind="Painel.aspx.cs" Inherits="Sebrae.Academico.WebForms.Painel" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="/js/dashboard/RGraph.common.core.js"></script>
    <script type="text/javascript" src="/js/dashboard/RGraph.meter.js"></script>
    <script type="text/javascript" src="/js/home/Relatorios.js"></script>
    <script type="text/javascript" src="js/dashboard/Chart.min.js"></script>
    <script type="text/javascript" src="/js/window.mobilecheck.js"></script>
    
    <div class="row" style="margin: 10px 0;">
        <div class="btn-group pull-right">
            <button class="btn btn-default" id="btnProximoMenu" onclick="location.href = '/Dashboard.aspx'; return false;">Monitoramento de indicadores</button>
            <button class="btn btn-success" id="btnAtualizar">Atualizar</button>
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
            <div class="col-md-6 col-dashboard">
                <div class="panel panel-default panel-dashboard">
                    <div class="panel-body">
                        <table class="table table-stripped table-hover">
                            <thead>
                                <tr>
                                    <th>HORAS DE TREINAMENTO (aprovado)
                                    </th>
                                    <th class="text-right coluna-valores">
                                        <span class="ano0"></span>(h)
                                    </th>
                                    <th class="text-right coluna-valores">
                                        <span class="ano1"></span>(h)
                                    </th>
                                    <th class="text-right coluna-valores">
                                        <span class="ano2"></span>(h)
                                    </th>
                                    <th class="text-right coluna-valores">
                                        <span class="ano3"></span>(h)
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>Em Formação de Multiplicadores
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr0100" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr0101" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr0102" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr0103" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Capacitação do Sebrae Nacional (<i>per capta</i>)
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr0200" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr0201" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr0202" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr0203" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Capacitação do Sistema Sebrae (<i>per capta</i>)
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr0300" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr0301" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr0302" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr0303" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Capacitação do Sistema Sebrae (total)
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr0400" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr0401" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr0402" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr0403" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <div class="col-md-6 col-dashboard">
                <div class="panel panel-default panel-dashboard">
                    <div class="panel-body">
                        <table class="table table-stripped table-hover">
                            <thead>
                                <tr>
                                    <th>GESTÃO DO CONHECIMENTO
                                    </th>
                                    <th class="text-right coluna-valores">
                                        <span class="ano0"></span>
                                    </th>
                                    <th class="text-right coluna-valores">
                                        <span class="ano1"></span>
                                    </th>
                                    <th class="text-right coluna-valores">
                                        <span class="ano2"></span>
                                    </th>
                                    <th class="text-right coluna-valores">
                                        <span class="ano3"></span>
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>Ações de Gestão do Conhecimento registradas no PADI
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr1100" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr1101" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr1102" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr1103" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Produção de conteúdos na Plataforma Saber (crescimento)
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr1200" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr1201" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr1202" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr1203" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-dashboard">
                <div class="panel panel-default panel-dashboard">
                    <div class="panel-body">
                        <table class="table table-stripped table-hover">
                            <thead>
                                <tr>
                                    <th>PARTICIPAÇÃO NAS CAPACITAÇÕES
                                    </th>
                                    <th class="text-right coluna-valores">
                                        <span class="ano0"></span>
                                    </th>
                                    <th class="text-right coluna-valores">
                                        <span class="ano1"></span>
                                    </th>
                                    <th class="text-right coluna-valores">
                                        <span class="ano2"></span>
                                    </th>
                                    <th class="text-right coluna-valores">
                                        <span class="ano3"></span>
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>Aprovadas nas capacitações (<i>per capta</i>)
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr0500" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr0501" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr0502" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr0503" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Aprovações em relação aos inscritos
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr0600" runat="server"></asp:Literal>
                                        %
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr0601" runat="server"></asp:Literal>
                                        %
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr0602" runat="server"></asp:Literal>
                                        %
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr0603" runat="server"></asp:Literal>
                                        %
                                    </td>
                                </tr>
                                <tr>
                                    <td>Participação dos colaboradores nas ações de desenvolvimento (NA)

                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr0700" runat="server"></asp:Literal>
                                        %
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr0701" runat="server"></asp:Literal>
                                        %
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr0702" runat="server"></asp:Literal>
                                        %
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr0703" runat="server"></asp:Literal>
                                        %
                                    </td>
                                </tr>
                                <tr>
                                    <td>Aprovação em ações de Formação de Multiplicadores
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr0800" runat="server"></asp:Literal>
                                        %
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr0801" runat="server"></asp:Literal>
                                        %
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr0802" runat="server"></asp:Literal>
                                        %
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr0803" runat="server"></asp:Literal>
                                        %
                                    </td>
                                </tr>
                                <tr>
                                    <td>Colaboradores que pactuam meta de desenvolvimento (PADI)
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr2100" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr2101" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr2102" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr2103" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <div class="col-md-6 col-dashboard">
                <div class="panel panel-default panel-dashboard">
                    <div class="panel-body">
                        <table class="table table-stripped table-hover">
                            <thead>
                                <tr>
                                    <th>CERTIFICAÇÃO
                                    </th>
                                    <th class="text-right coluna-valores">
                                        <span class="ano0"></span>
                                    </th>
                                    <th class="text-right coluna-valores">
                                        <span class="ano1"></span>
                                    </th>
                                    <th class="text-right coluna-valores">
                                        <span class="ano2"></span>
                                    </th>
                                    <th class="text-right coluna-valores">
                                        <span class="ano3"></span>
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>Colaboradores certificados do Sistema Sebrae (em relação ao universo, exceto SP)
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr1300" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr1301" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr1302" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr1303" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Colaboradores certificados do Sistema Sebrae (em relação aos inscritos)
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr1400" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr1401" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr1402" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr1403" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Colaboradores certificados do Sebrae-NA (em relação aos inscritos)
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr1500" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr1501" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr1502" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr1503" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-dashboard">
                <div class="panel panel-default panel-dashboard">
                    <div class="panel-body">
                        <table class="table table-stripped table-hover">
                            <thead>
                                <tr>
                                    <th>EFICÁCIA E SATISFAÇÃO
                                    </th>
                                    <th class="text-right coluna-valores">
                                        <span class="ano0"></span>
                                    </th>
                                    <th class="text-right coluna-valores">
                                        <span class="ano1"></span>
                                    </th>
                                    <th class="text-right coluna-valores">
                                        <span class="ano2"></span>
                                    </th>
                                    <th class="text-right coluna-valores">
                                        <span class="ano3"></span>
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>Satisfação em relação aos programas educacionais da UC (Portfólio UC)
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr0900" runat="server"></asp:Literal>
                                        %
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr0901" runat="server"></asp:Literal>
                                        %
                                    </td>
                                    <td class="text-right  coluna-valores">
                                        <asp:Literal ID="ltr0902" runat="server"></asp:Literal>
                                        %
                                    </td>
                                    <td class="text-right  coluna-valores">
                                        <asp:Literal ID="ltr0903" runat="server"></asp:Literal>
                                        %
                                    </td>
                                </tr>
                                <tr>
                                    <td>Satisfação nas ações de Formação de Multiplicadores
                                    </td>
                                    <td class="text-right  coluna-valores">
                                        <asp:Literal ID="ltr1000" runat="server"></asp:Literal>
                                        %
                                    </td>
                                    <td class="text-right  coluna-valores">
                                        <asp:Literal ID="ltr1001" runat="server"></asp:Literal>
                                        %
                                    </td>
                                    <td class="text-right  coluna-valores">
                                        <asp:Literal ID="ltr1002" runat="server"></asp:Literal>
                                        %
                                    </td>
                                    <td class="text-right  coluna-valores">
                                        <asp:Literal ID="ltr1003" runat="server"></asp:Literal>
                                        %
                                    </td>
                                </tr>
                                <tr>
                                    <td>Eficácia dos programas educacionais (Portfólio UC)
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr1900" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr1901" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr1902" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr1903" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Eficácia dos programas acadêmicos
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr2000" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr2001" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr2002" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr2003" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <div class="col-md-6 col-dashboard">
                <div class="panel panel-default panel-dashboard">
                    <div class="panel-body">
                        <table class="table table-stripped table-hover">
                            <thead>
                                <tr>
                                    <th>INVESTIMENTO
                                    </th>
                                    <th class="text-right  coluna-valores">
                                        <span class="ano0"></span>
                                    </th>
                                    <th class="text-right  coluna-valores">
                                        <span class="ano1"></span>
                                    </th>
                                    <th class="text-right  coluna-valores">
                                        <span class="ano2"></span>
                                    </th>
                                    <th class="text-right  coluna-valores">
                                        <span class="ano3"></span>
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>Capacitação Metodológica em ações de Formação de Multiplicadores
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr1600" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr1601" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr1602" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr1603" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Em relação à folha de pagamento do Sistema Sebrae
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr1700" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr1701" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr1702" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr1703" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Execução %
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr1800" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr1801" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr1802" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-right coluna-valores">
                                        <asp:Literal ID="ltr1803" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script language="javascript" type="text/javascript">
        $(function () {
            $("body").addClass('home');

            // Definir as alturas de cada célula de acordo com seu container.
            var tabelas = $("table");

            // Definir as alturas dos containers para 33% do espaço (um terço do viewport menos a altura do navbar do topo)
            if (!window.mobilecheck()) {
                var defaultHeight = ($(document).height() / 3) - 40;

                var dashboards = $('.panel-dashboard');
                if (dashboards != undefined) {
                    for (var i = 0; i < dashboards.length; i++) {

                        if (dashboards[i] != undefined) {
                            $(dashboards[i]).height(defaultHeight);
                        }
                    }
                }

                if (tabelas != undefined) {
                    for (var t = 0; t < tabelas.length; t++) {

                        var tab = tabelas[t];

                        var tbody = tab.getElementsByTagName('tbody')[0];

                        if (tbody != undefined) {

                            var rows = tbody.getElementsByTagName('tr');

                            if (rows != undefined) {

                                var rowsCt = rows.length;

                                // Variável de fator da altura da célula.
                                var cellFactor = (defaultHeight - 60) / rowsCt;

                                for (var r = 0; r < rows.length; r++) {
                                    var row = rows[r];

                                    var cols = row.getElementsByTagName('td');

                                    if (cols != undefined) {
                                        for (var c = 0; c < cols.length; c++) {
                                            var col = cols[c];

                                            $(col).css("height", cellFactor + "px");
                                            $(col).css("padding-top", ((cellFactor / 2) - 10) + "px");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            } else {
                for (var x = 0; x < tabelas.length; x++) {
                    var container = $(tabelas[x]).parent();
                    container.addClass('table-responsive');
                    container.css("padding-bottom", "50px");
                }

                $('#dashboard-proximoMenu-mobile').show();
            }



            // Setar 3 últimos anos.
            var anoAtual = new Date().getFullYear();

            var anos1 = $(".ano0");

            if (anos1 != undefined) {
                for (var a = 0; a < anos1.length; a++) {
                    var ano1 = anos1[a];

                    if (ano1 != undefined) {
                        $(ano1).text(anoAtual - 3);
                    }
                }
            }

            var anos1 = $(".ano1");

            if (anos1 != undefined) {
                for (var a = 0; a < anos1.length; a++) {
                    var ano1 = anos1[a];

                    if (ano1 != undefined) {
                        $(ano1).text(anoAtual - 2);
                    }
                }
            }

            var anos2 = $(".ano2");

            if (anos2 != undefined) {
                for (var b = 0; b < anos2.length; b++) {
                    var ano2 = anos2[b];

                    if (ano2 != undefined) {
                        $(ano2).text(anoAtual - 1);
                    }
                }
            }

            var anos3 = $(".ano3");

            if (anos3 != undefined) {
                for (var c = 0; c < anos3.length; c++) {
                    var ano3 = anos3[c];

                    if (ano3 != undefined) {
                        $(ano3).text(anoAtual);
                    }
                }
            }

            setTimeout(function () {
                $("#cortina").fadeOut();
            }, 500);
        });
    </script>
</asp:Content>
