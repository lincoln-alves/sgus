<%@ Page Title="" Language="C#" MasterPageFile="~/Academico20.Master" AutoEventWireup="true"
    CodeBehind="HomeRelatoriosPaginaInicial.aspx.cs" Inherits="Sebrae.Academico.WebForms.HomeRelatoriosPaginaInicial" %>

<%@ Register Src="~/UserControls/ucDashBoardRelatoriosMaisAcessados.ascx" TagName="ucDashBoardRelatoriosMaisAcessados"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDashboardFuncionalidadesMaisAcessadas.ascx" TagName="ucDashboardFuncionalidadesMaisAcessadas"
    TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/ucNotificacoes.ascx" TagName="ucNotificacoes" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="js/home/Relatorios.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server"></asp:ScriptManager>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <%-- Links e campos usados para formar os relatórios sem postback --%>
            <asp:HiddenField runat="server" ID="hdConcluintesPorRegiao" />
            <asp:LinkButton runat="server" ID="lnkConcluintesPorRegiao" OnClick="lnkConcluintesPorRegiao_OnClick"></asp:LinkButton>
            <asp:HiddenField runat="server" ID="hdConcluintesPorSolucaoEducacional" />
            <asp:LinkButton runat="server" ID="lnkConcluintesPorSolucaoEducacional" OnClick="lnkConcluintesPorSolucaoEducacional_OnClick"></asp:LinkButton>
            <asp:HiddenField runat="server" ID="hdFaixaEtaria" />
            <asp:LinkButton runat="server" ID="lnkFaixaEtaria" OnClick="lnkFaixaEtaria_OnClick"></asp:LinkButton>
            <asp:HiddenField runat="server" ID="hdPerfilDoPublicoAtendimento" />
            <asp:LinkButton runat="server" ID="lnkPerfilDoPublicoAtendimento" OnClick="lnkPerfilDoPublicoAtendimento_OnClick"></asp:LinkButton>
            <asp:HiddenField runat="server" ID="hdLimitesPorSebraeDF"/>
            <asp:LinkButton runat="server" ID="lnkLimitesPorSebraeDF" OnClick="lnkLimitesPorSebraeDF_OnClick"></asp:LinkButton>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="col-sm-7 col-md-7 col-lg-9">
        <div class="row">
            <div class="col-md-6">
                <div class="row box dynamic-graph">
                    <div class="clearfix text-center">
                        <h2 class="col-sm-12">Público-alvo da UCSebrae</h2>
                        <img src="Graficos/PublicoAlvo.aspx" alt="Loading" class="force img-responsive" />
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="row box dynamic-graph">
                    <div class="clearfix text-center">
                        <h2 class="col-sm-12">Quantidade por situação nos cursos</h2>

                        <script type="text/javascript">
                            (function(){
                                var titulo = 'Situação dos Alunos em Curso Online';
                                googleCharts.push(function drawChart() {
                                    var data = google.visualization.arrayToDataTable([
                                        ['Situação', 'Porcentagem'],
                                        <asp:Repeater ID="rptSituacaoAlunos" onitemdatabound="rptSituacaoAlunos_ItemDataBound" runat="server">
                                             <ItemTemplate>
                                                <asp:Literal ID="LiteralSituacao" runat="server"></asp:Literal>
                                             </ItemTemplate>
                                        </asp:Repeater>
                                ]);
                                    var options = {title: titulo, legend: { position: 'bottom' }};
                                    var chart = new google.visualization.PieChart(document.getElementById('graf-situacao-alunos'));
                                    chart.draw(data, options);
                                });
                            })()
                        </script>
                        <div id="graf-situacao-alunos" style="width: 100%; height: 100%; padding: 0 15px;">
                        </div>



                        <iframe src="<%:ResolveUrl("~/Graficos/SituacaoCursos.aspx")%>" width="650px" height="250px"
                            frameborder="0" id="ifrmSituacaoCursos"></iframe>
                        <div class="form action">
                            <input type="text" name="anoSituacaoCursos" class="form-control" placeholder="Filtrar por ano" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6">
                <div class="row box dynamic-graph" data-action="Graficos/MatriculasPorAnoPorFormaDeAquisicao.aspx">
                    <div class="clearfix text-center">
                        <h2 class="col-sm-12">Matrículas forma de aquisição</h2>
                        <img src="Graficos/MatriculasPorAnoPorFormaDeAquisicao.aspx" alt="Loading" class="force img-responsive" />
                        <div class="form action">
                            <input type="text" name="ano" class="form-control" placeholder="Filtrar por ano" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="row box dynamic-graph" data-action="Graficos/ConcluintesPorFormaAquisicao.aspx">
                    <div class="clearfix text-center">
                        <h2 class="col-sm-12">% DE CONCLUINTES EM RELAÇÃO ÀS SOLUÇÕES</h2>
                        <img src="Graficos/ConcluintesPorFormaAquisicao.aspx" alt="Loading" class="force img-responsive" />
                        <div class="form action">
                            <input type="text" name="ano" class="form-control" placeholder="Filtrar por ano" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-6">
                <div class="row box dynamic-graph" data-action="Graficos/ParticipacaoProporcionalAoNumeroDeFuncionarios.aspx">
                    <div class="clearfix text-center">
                        <h2 class="col-sm-12">Matrículas por nível ocupacional</h2>
                        <img src="Graficos/ParticipacaoProporcionalAoNumeroDeFuncionarios.aspx" alt="Loading"
                            class="force img-responsive" />
                        <div class="form action">
                            <input class="form-control" type="text" name="ano" placeholder="Filtrar por ano" />
                            <!--<asp:DropDownList CssClass="form-control half-size inline" ClientIDMode="Static" ID="DropDownList1" runat="server"></asp:DropDownList>-->
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="row box dynamic-graph" data-action="Graficos/MatriculasPorMes.aspx">
                    <div class="clearfix text-center">
                        <h2 class="col-sm-12">Matrículas por mês</h2>
                        <img src="Graficos/MatriculasPorMes.aspx" alt="Loading" class="force img-responsive" />
                        <div class="form action">
                            <input type="text" name="ano" class="form-control" placeholder="Filtrar por ano" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6">
                <div class="row box dynamic-graph">
                    <div class="clearfix text-center">
                        <h2 class="col-sm-12">Cursos com maior numero de concluintes</h2>
                        <iframe src="<%:ResolveUrl("~/Graficos/CursosMaiorNumeroConcluintes.aspx")%>" width="650px"
                            height="250px" frameborder="0" id="ifrmConcluintes"></iframe>
                        <div class="form action">
                            <input type="text" name="anoConcluintes" class="form-control" placeholder="Filtrar por ano" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="row box dynamic-graph" data-action="Graficos/TaxaDeAprovacaoNoAno.aspx">
                    <div class="clearfix text-center">
                        <h2 class="col-sm-12">Matrículas por UF</h2>
                        <img src="Graficos/TaxaDeAprovacaoNoAno.aspx" alt="Loading" class="force img-responsive" />
                        <div class="form action">
                            <input class="form-control half-size inline" type="text" name="ano" placeholder="Filtrar por ano" />
                            <asp:DropDownList CssClass="form-control half-size inline" ClientIDMode="Static"
                                ID="ddlUF" runat="server">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6">
                <div class="row box dynamic-graph" data-action="Graficos/InscritosTotalUF.aspx">
                    <div class="clearfix text-center">
                        <h2 class="col-sm-12">Inscritos x Quadro total</h2>
                        <img src="Graficos/InscritosTotalUF.aspx" alt="Loading" class="force img-responsive" />
                        <div class="form action">
                            <input type="text" name="ano" class="form-control" placeholder="Filtrar por ano" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="row box dynamic-graph" data-action="Graficos/InscritosPorCategoria.aspx">
                    <div class="clearfix text-center">
                        <h2 class="col-sm-12">Inscritos por Eixo Temático</h2>
                        <img src="Graficos/InscritosPorCategoria.aspx" alt="Loading" class="force img-responsive" />
                        <div class="form action">
                            <input type="text" name="ano" class="form-control" placeholder="Filtrar por ano" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="row box">
                    <div class="clearfix text-center">
                        <h2 class="col-sm-12">Concluintes por Solução Educacional</h2>
                        <script type="text/javascript">
                            (function(){
                                var titulo = 'SOLUÇÕES EDUCACIONAIS COM MAIORES NÚMEROS DE CONCLUINTES';
                                googleCharts.push(function drawChart() {
                                    var data = google.visualization.arrayToDataTable([
                                        ['Soluções Educacionais', 'Concluintes'],
                                        <asp:Repeater ID="rptConcluintesPorSolucacaoEducacional" onitemdatabound="rptConcluintesPorSolucacaoEducacional_ItemDataBound" runat="server">
                                             <ItemTemplate>
                                                <asp:Literal ID="Literal" runat="server"></asp:Literal>
                                             </ItemTemplate>
                                        </asp:Repeater>
                                ]);
                                    var options = {title: titulo, legend: { position: 'bottom' }};
                                    var chart = new google.visualization.BarChart(document.getElementById('graf-concluintes'));
                                    chart.draw(data, options);
                                });
                            })()
                        </script>
                        <div id="graf-concluintes" style="width: 100%; height: 100%; padding: 0 15px;">
                        </div>
                        <div class="form action">
                            <input type="text" name="ano" class="form-control" placeholder="Filtrar por ano" runat="server" id="txtConcluintesPorSolucaoEducacional" onkeypress="concluintesPorSolucaoEducacional()" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="row box">
                    <div class="clearfix text-center">
                        <h2 class="col-sm-12">PERFIL DO PÚBLICO ATENDIDO</h2>
                        <script type="text/javascript">
                            (function(){
                                var titulo = 'INDIVÍDUOS CAPACITADOS';
                                
                                googleCharts.push(function drawChart() {
                                    var data = google.visualization.arrayToDataTable([
                                        ['PERFIL', 'QUANTIDADE'],
                                        <asp:Repeater ID="rptPerfilDoPUblicoAtendido" onitemdatabound="rptPerfilDoPUblicoAtendido_ItemDataBound" runat="server">
                                             <ItemTemplate>
                                                <asp:Literal ID="Literal" runat="server"></asp:Literal>
                                             </ItemTemplate>
                                        </asp:Repeater>
                                ]);

                                    var options = {title: titulo, legend: { position: 'bottom' }};
                                    var chart = new google.visualization.BarChart(document.getElementById('grafico-perfil-do-publico-concluinte'));
                                    chart.draw(data, options);
                                });

                            })()
                        </script>
                        <div id="grafico-perfil-do-publico-concluinte" style="width: 100%; height: 100%; padding: 0 15px;">
                        </div>
                        <div class="form action">
                            <input type="text" name="ano" class="form-control" placeholder="Filtrar por ano" runat="server" ID="txtPerfilDoPublicoAtendimento" onkeypress="perfilDoPublicoAtendido()" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="row box">
                    <div class="clearfix text-center">
                        <h2 class="col-sm-12">FAIXA ETÁRIA</h2>
                        <script type="text/javascript">
                            (function(){
                                var titulo = '% DE FAIXA ETÁRIA';
                                
                                googleCharts.push(function drawChart() {
                                    var data = google.visualization.arrayToDataTable([
                                        ['IDADE', 'PORCENTAGEM'],
                                        <asp:Repeater ID="rptFaixaEtaria" onitemdatabound="rptFaixaEtaria_ItemDataBound" runat="server">
                                             <ItemTemplate>
                                                <asp:Literal ID="Literal" runat="server"></asp:Literal>
                                             </ItemTemplate>
                                        </asp:Repeater>
                                ]);

                                    var options = {title: titulo, legend: { position: 'bottom' }};
                                    var chart = new google.visualization.BarChart(document.getElementById('grafico-faixa-etaria'));
                                    chart.draw(data, options);
                                });

                            })()
                        </script>
                        <div id="grafico-faixa-etaria" style="width: 100%; height: 100%; padding: 15px;">
                        </div>
                        <div class="form action">
                            <input type="text" name="ano" class="form-control" placeholder="Filtrar por ano" runat="server" id="txtFaixaEtaria" onkeypress="faixaEtaria()" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-6">
                <div class="row box">
                    <div class="clearfix text-center">
                        <h2 class="col-sm-12">CONCLUINTES POR REGIÃO</h2>
                        <script type="text/javascript">
                            (function(){
                                var titulo = '% DE CONCLUINTES POR REGIÃO';
                                
                                googleCharts.push(function drawChart() {
                                    var data = google.visualization.arrayToDataTable([
                                        ['REGIÃO', 'PORCENTAGEM'],
                                        <asp:Repeater ID="rptConcluintesPorRegiao" onitemdatabound="rptConcluintesPorRegiao_ItemDataBound" runat="server">
                                             <ItemTemplate>
                                                <asp:Literal ID="Literal" runat="server"></asp:Literal>
                                             </ItemTemplate>
                                        </asp:Repeater>
                                ]);

                                    var options = {title: titulo, legend: { position: 'bottom' }};
                                    var chart = new google.visualization.BarChart(document.getElementById('grafico-concluintes-por-regiao'));
                                    chart.draw(data, options);
                                });

                            })()
                        </script>
                        <div id="grafico-concluintes-por-regiao" style="width: 100%; height: 100%; padding: 0 15px;">
                        </div>

                        <div class="form action">
                            <input type="text" id="txtFiltrarPorAnoConcluintePorRegiao" class="form-control" placeholder="Filtrar por ano" onkeypress="concluintesPorRegiao()" runat="server" />
                        </div>

                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="row box">
                    <div class="clearfix text-center">
                        <h2 class="col-sm-12">LIMITES POR SEBRAE-DF</h2>
                        <script type="text/javascript">
                            (function(){
                                var titulo = '% DE LIMITES POR SEBRAE-DF';
                                
                                googleCharts.push(function drawChart() {
                                    var data = google.visualization.arrayToDataTable([
                                        ['UF', 'PORCENTAGEM'],
                                        <asp:Repeater ID="rptLimitesPorUF" onitemdatabound="rptLimitesPorUF_ItemDataBound" runat="server">
                                             <ItemTemplate>
                                                <asp:Literal ID="Literal" runat="server"></asp:Literal>
                                             </ItemTemplate>
                                        </asp:Repeater>
                                ]);

                                    var options = {title: titulo, legend: { position: 'bottom' }};
                                    var chart = new google.visualization.BarChart(document.getElementById('grafico-limites-por-uf'));
                                    chart.draw(data, options);
                                });

                            })()
                        </script>
                        <div id="grafico-limites-por-uf" style="width: 100%; height: 500px; padding: 0 15px;">
                        </div>
                        <div class="form action">
                            <input type="text" name="ano" class="form-control" placeholder="Filtrar por ano" runat="server" ID="txtLimitesPorSebraeDF" onkeypress="limitesPorSebraeDF()" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="row box">
                    <div class="clearfix text-center">
                        <h2 class="col-sm-12">TEMPO DE SEBRAE</h2>
                        <script type="text/javascript">
                            (function(){
                                var titulo = 'NÚMERO DE USUÁRIOS POR TEMPO DE SEBRAE';
                                
                                googleCharts.push(function drawChart() {
                                    var data = google.visualization.arrayToDataTable([
                                        ['TEMPO', 'QUANTIDADE'],
                                        <asp:Repeater ID="rptTempoDeSebrae" onitemdatabound="rptTempoDeSebrae_ItemDataBound" runat="server">
                                             <ItemTemplate>
                                                <asp:Literal ID="Literal" runat="server"></asp:Literal>
                                             </ItemTemplate>
                                        </asp:Repeater>
                                ]);

                                    var options = {title: titulo, legend: { position: 'bottom' }};
                                    var chart = new google.visualization.BarChart(document.getElementById('grafico-tempo-de-sebrae'));
                                    chart.draw(data, options);
                                });

                            })()
                        </script>
                        <div id="grafico-tempo-de-sebrae" style="width: 100%; height: 500px; padding: 0 15px;">
                        </div>
                        <div class="form action">
                            <input type="text" name="ano" class="form-control" placeholder="Filtrar por ano" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="row box">
                    <div class="clearfix text-center">
                        <h2 class="col-sm-12">NÚMERO DE CERTIFICAÇÕES x COLABORADOR</h2>
                        <script type="text/javascript">
                            (function(){
                                var titulo = 'NÚMERO DE CERTIFICAÇÕES POR COLABORADOR';
                                
                                googleCharts.push(function drawChart() {
                                    var data = google.visualization.arrayToDataTable([
                                        ['Nº DE CERTIFICAÇÕES', 'COLABORADORES CERTIFICADOS'],
                                        <asp:Repeater ID="rptCertificacoesPorColaborador" onitemdatabound="rptCertificacoesPorColaborador_ItemDataBound" runat="server">
                                             <ItemTemplate>
                                                <asp:Literal ID="Literal" runat="server"></asp:Literal>
                                             </ItemTemplate>
                                        </asp:Repeater>
                                ]);

                                    var options = {title: titulo, legend: { position: 'bottom' }};
                                    var chart = new google.visualization.Table(document.getElementById('grafico-certificacoes-por-colaborador'));
                                    chart.draw(data, options);
                                });

                            })()
                        </script>
                        <div id="grafico-certificacoes-por-colaborador" style="width: 100%; height: 500px; padding: 0 15px;">
                        </div>
                        <div class="form action">
                            <input type="text" name="ano" class="form-control" placeholder="Filtrar por ano" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="row box">
                    <div class="clearfix text-center">
                        <h2 class="col-sm-12">INSCRITOS x QUADRO TOTAL</h2>
                        <script type="text/javascript">
                            (function(){
                                var titulo = 'NÚMERO DE INSCRITOS POR QUADRO TOTAL DE UF';
                                
                                googleCharts.push(function drawChart() {
                                    var data = google.visualization.arrayToDataTable([
                                        ['UF', 'INSCRITOS', 'QUADRO TOTAL'],
                                        <asp:Repeater ID="rptInscritosPorQuadroTotal" onitemdatabound="numItemsInscritosPorQuadroTotal_ItemDataBound" runat="server">
                                             <ItemTemplate>
                                                <asp:Literal ID="Literal" runat="server"></asp:Literal>
                                             </ItemTemplate>
                                        </asp:Repeater>
                                ]);

                                    var options = {title: titulo, legend: { position: 'bottom' }};
                                    var chart = new google.visualization.BarChart(document.getElementById('grafico-inscritos-por-quadro-total'));
                                    chart.draw(data, options);
                                });

                            })()
                        </script>
                        <div id="grafico-inscritos-por-quadro-total" style="width: 100%; height: 500px; padding: 0 15px;">
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="row box">
                    <div class="clearfix text-center">
                        <h2 class="col-sm-12">QUANTIDADE DE TEMAS CERTIFICADOS</h2>
                        <script type="text/javascript">
                            (function(){
                                var titulo = 'NÚMERO DE CERTIFICADOS POR NÍVEL OPERACIONAL';
                                
                                googleCharts.push(function drawChart() {
                                    var data = google.visualization.arrayToDataTable([
                                        <asp:Repeater ID="rptCertificadosPorNivelOperacional" onitemdatabound="rptCertificadosPorNivelOperacional_ItemDataBound" runat="server">
                                             <ItemTemplate>
                                                <asp:Literal ID="Literal" runat="server"></asp:Literal>
                                             </ItemTemplate>
                                        </asp:Repeater>
                                ]);

                                    var options = {isStacked:true,title: titulo, legend: { position: 'bottom' }};
                                    var chart = new google.visualization.ColumnChart(document.getElementById('grafico-certificados-por-nivel-operacional'));
                                    chart.draw(data, options);
                                });

                            })()
                        </script>
                        <div id="grafico-certificados-por-nivel-operacional" style="width: 100%; height: 500px; padding: 0 15px;">
                        </div>
                        <div class="form action">
                            <input type="text" name="ano" class="form-control" placeholder="Filtrar por ano" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="row box">
                    <div class="clearfix text-center">
                        <h2 class="col-sm-12">CERTIFICADOS X NÃO CERTIFICADOS</h2>
                        <script type="text/javascript">
                            (function(){
                                var titulo = 'NÚMERO DE CERTIFICADOS E NÃO CERTIFICADOS';
                                
                                googleCharts.push(function drawChart() {
                                    var data = google.visualization.arrayToDataTable([
                                        ['', 'CERTIFICADOS', 'NÃO CERTIFICADOS'],
                                        <asp:Repeater ID="rptCertificadosPorNaoCertificados" onitemdatabound="rptCertificadosPorNaoCertificados_ItemDataBound" runat="server">
                                             <ItemTemplate>
                                                <asp:Literal ID="Literal" runat="server"></asp:Literal>
                                             </ItemTemplate>
                                        </asp:Repeater>
                                ]);

                                    var options = {title: titulo, legend: { position: 'bottom' }};
                                    var chart = new google.visualization.BarChart(document.getElementById('grafico-certificados-por-nao-certificados'));
                                    chart.draw(data, options);
                                });

                            })()
                        </script>
                        <div id="grafico-certificados-por-nao-certificados" style="width: 100%; height: 500px; padding: 0 15px;">
                        </div>
                        <div class="form action">
                            <input type="text" name="ano" class="form-control" placeholder="Filtrar por ano" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="row box">
                    <div class="clearfix text-center">
                        <h2 class="col-sm-12">GRAFICO EM RODA</h2>
                        <script type="text/javascript">
                            (function(){
                                var titulo = 'GRAFICO DE RODA EM DESENVOLVIMENTO';
                                
                                googleCharts.push(function drawChart() {
                                    var data = google.visualization.arrayToDataTable([
                                        <asp:Repeater ID="rptRodaEmPorcentagem" onitemdatabound="rptRodaEmPorcentagem_ItemDataBound" runat="server">
                                             <ItemTemplate>
                                                <asp:Literal ID="Literal" runat="server"></asp:Literal>
                                             </ItemTemplate>
                                        </asp:Repeater>
                                ]);

                                    var options = {title: titulo, legend: { position: 'bottom' }};
                                    var chart = new google.visualization.PieChart(document.getElementById('grafico-roda-em-porcentagem'));
                                    chart.draw(data, options);
                                });

                            })()
                        </script>
                        <div id="grafico-roda-em-porcentagem" style="width: 100%; height: 500px; padding: 0 15px;">
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="row box dynamic-graph">
                    <div class="clearfix text-center">
                        <h2 class="col-sm-12">Quantidade de certificações por cargo e tema</h2>
                        <iframe src="<%:ResolveUrl("~/Graficos/CertificadosPorCargoETema.aspx")%>" width="560px"
                            height="400px" frameborder="0" id="Iframe1"></iframe>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="row box dynamic-graph">
                    <div class="clearfix text-center">
                        <h2 class="col-sm-12">Participantes por Unidade do Sebrae-NA</h2>
                        <iframe src="<%:ResolveUrl("~/Graficos/ParticipantesPorUnidade.aspx")%>" width="560px"
                            height="400px" frameborder="0" id="Iframe2"></iframe>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="row box dynamic-graph">
                    <div class="clearfix text-center">
                        <h2 class="col-sm-12">Concluíntes por Solução e Eixo Temático</h2>
                        <iframe src="<%:ResolveUrl("~/Graficos/SolucoesEducacionaisPorCategoria.aspx")%>"
                            width="560px" height="400px" frameborder="0" id="Iframe3"></iframe>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="row box dynamic-graph">
                    <div class="clearfix text-center">
                        <h2 class="col-sm-12">Soluções e Concluíntes por Eixo Temático</h2>
                        <iframe src="<%:ResolveUrl("~/Graficos/SolucoesEConcluintesPorCategoria.aspx")%>"
                            width="560px" height="400px" frameborder="0" id="Iframe4"></iframe>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="row box dynamic-graph">
                    <div class="clearfix text-center">
                        <h2 class="col-sm-12">Concluíntes por Perfil e Tipo</h2>
                        <iframe src="<%:ResolveUrl("~/Graficos/ConcluintesPorPerfilETipo.aspx")%>" width="560px"
                            height="400px" frameborder="0" id="Iframe5"></iframe>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="row box dynamic-graph">
                    <div class="clearfix text-center">
                        <h2 class="col-sm-12">Cursos Oferecidos, Concluíntes e Horas de Treinamento</h2>
                        <iframe src="<%:ResolveUrl("~/Graficos/TotalizacoesCursosOferecidos.aspx")%>" width="560px"
                            height="400px" frameborder="0" id="Iframe6"></iframe>
                    </div>
                </div>
            </div>

            <div class="col-md-6">
                <div class="row box dynamic-graph">
                    <div class="clearfix text-center">
                        <h2 class="col-sm-12">Satisfação com as Soluções Educacionais</h2>
                        <iframe src="<%:ResolveUrl("~/Graficos/SatisfacaoSolucoesEducacionais.aspx")%>" width="560px" height="400px"
                            frameborder="0" id="Iframe7"></iframe>
                    </div>
                </div>
            </div>

            <div class="col-md-6">
                <div class="row box dynamic-graph">
                    <div class="clearfix text-center">
                        <h2 class="col-sm-12">Relatorio por UF / Perfil </h2>
                        <iframe src="<%:ResolveUrl("~/Graficos/RelatorioUFPerfil.aspx")%>" width="560px" height="400px"
                            frameborder="0" id="Iframe8"></iframe>
                    </div>
                </div>
            </div>


        </div>
        <script language="javascript" type="text/javascript">
            $("body").addClass('home');
        </script>
    </div>
    <div class="col-sm-5 col-md-5 col-lg-3">
        <div class="row">
            <div class="col-xs-12">
                <div class="row box">
                    <div class="clearfix">
                        <h2 class="col-sm-12">Notificações</h2>
                        <uc3:ucNotificacoes ID="ucNotificacoes1" runat="server" />
                    </div>
                </div>
                <div class="row box">
                    <div class="clearfix">
                        <h2 class="col-sm-12">Relatórios Mais Acessados</h2>
                        <uc1:ucDashBoardRelatoriosMaisAcessados ID="ucDashBoardRelatoriosMaisAcessados1"
                            runat="server" />
                    </div>
                </div>
                <div class="row box">
                    <div class="clearfix">
                        <h2 class="col-sm-12">Funcionalidades Mais Acessadas</h2>
                        <uc2:ucDashboardFuncionalidadesMaisAcessadas ID="ucDashboardFuncionalidadesMaisAcessadas1"
                            runat="server" />
                    </div>
                </div>
            </div>
        </div>
        <script>
            var urlSituacaoAno = '<%:ResolveUrl("~/Graficos/SituacaoCursos.aspx")%>';
            var urlConcluintes = '<%:ResolveUrl("~/Graficos/CursosMaiorNumeroConcluintes.aspx")%>';

            $("[name='anoSituacaoCursos']").keyup(function (event) {
                if (event.keyCode == 13) {
                    var ano = $("[name='anoSituacaoCursos']").val();
                    document.getElementById('ifrmSituacaoCursos').src = urlSituacaoAno + '?ano=' + ano;
                }
            });


            $("[name='anoConcluintes']").keyup(function (event) {
                if (event.keyCode == 13) {
                    var anoc = $("[name='anoConcluintes']").val();
                    document.getElementById('ifrmConcluintes').src = urlConcluintes + '?ano=' + anoc;
                }
            });
        </script>
    </div>
</asp:Content>
