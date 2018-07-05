<%@ Page Title="" Language="C#" MasterPageFile="~/Academico20.Master" AutoEventWireup="true"
    CodeBehind="Home.aspx.cs" Inherits="Sebrae.Academico.WebForms.Home" %>

<%@ Register Src="UserControls/ucDashBoardRelatoriosMaisAcessados.ascx" TagName="ucDashBoardRelatoriosMaisAcessados"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDashboardFuncionalidadesMaisAcessadas.ascx" TagName="ucDashboardFuncionalidadesMaisAcessadas"
    TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/ucNotificacoes.ascx" TagName="ucNotificacoes" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="col-sm-7 col-md-7 col-lg-9">
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
                <div class="row box dynamic-graph" data-action="Graficos/TaxaDeAprovacaoNoAno.aspx">
                    <div class="clearfix text-center">
                        <h2 class="col-sm-12">Matrículas por UF</h2>
                        <img src="Graficos/TaxaDeAprovacaoNoAno.aspx" alt="Loading" class="force img-responsive" />
                        <div class="form action">
                            <input class="form-control half-size inline" type="text" name="ano" placeholder="Filtrar por ano" />
                            <asp:DropDownList CssClass="form-control half-size inline" ClientIDMode="Static" ID="ddlUF" runat="server"></asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12">
                <div class="row box dynamic-graph" data-action="Graficos/ParticipacaoProporcionalAoNumeroDeFuncionarios.aspx">
                    <div class="clearfix text-center">
                        <h2 class="col-sm-12">Matrículas por nível ocupacional</h2>
                        <img src="Graficos/ParticipacaoProporcionalAoNumeroDeFuncionarios.aspx" alt="Loading" class="force img-responsive" />
                        <div class="form action">
                            <input class="form-control" type="text" name="ano" placeholder="Filtrar por ano" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-md-6">
                <div class="row box dynamic-graph" data-action="Graficos/TurmasPorStatus.aspx">
                    <div class="clearfix text-center">
                        <h2 class="col-sm-12">Turmas por Status</h2>
                        <img src="Graficos/TurmasPorStatus.aspx" alt="Loading" class="force img-responsive" />
                        <div class="form action">
                            <input class="form-control" type="text" name="ano" placeholder="Filtrar por ano" />
                        </div>
                    </div>
                    <div class="panel-body">
                        <span class="legenda-square" style="background-color: #87ceeb"></span>&nbsp;- Total de Turmas no ano pesquisado.
                        <br />
                        <span class="legenda-square" style="background-color: #32cd32"></span>&nbsp;- Total de Turmas no ano por Status.
                        <br />
                        Caso o ano não seja informado, os dados serão de <strong><%= DateTime.Now.Year%></strong>.
                    </div>
                </div>
            </div>


            <div class="col-xs-12 col-md-6">
                <div class="row box dynamic-graph" data-action="Graficos/MatriculasPorStatus.aspx">
                    <div class="clearfix text-center">
                        <h2 class="col-sm-12">Matrículas por Status</h2>
                        <img src="Graficos/MatriculasPorStatus.aspx" alt="Loading" class="force img-responsive" />
                        <div class="form action">
                            <input class="form-control" type="text" name="ano" placeholder="Filtrar por ano" />
                        </div>
                    </div>
                    <div class="panel-body">
                        <span class="legenda-square" style="background-color: #87ceeb"></span>&nbsp;- Total de Matrículas no ano pesquisado.
                        <br />
                        <span class="legenda-square" style="background-color: #32cd32"></span>&nbsp;- Total de Matrículas no ano por Status.
                        <br />
                        Caso o ano não seja informado, os dados serão de <strong><%= DateTime.Now.Year%></strong>.
                    </div>
                </div>
            </div>
        </div>
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
                        <uc1:ucDashBoardRelatoriosMaisAcessados ID="ucDashBoardRelatoriosMaisAcessados1" runat="server" />
                    </div>
                </div>
                <div class="row box">
                    <div class="clearfix">
                        <h2 class="col-sm-12">Funcionalidades Mais Acessadas</h2>
                        <uc2:ucDashboardFuncionalidadesMaisAcessadas ID="ucDashboardFuncionalidadesMaisAcessadas1" runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script language="javascript" type="text/javascript">
        $("body").addClass('home');
    </script>
</asp:Content>
