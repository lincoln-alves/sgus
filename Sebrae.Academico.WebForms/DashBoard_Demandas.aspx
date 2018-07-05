<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="DashBoard_Demandas.aspx.cs" Inherits="Sebrae.Academico.WebForms.DashBoard_Demandas" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<link rel="Stylesheet" href="/css/dashboard_demandas.css" />

    <div class="container">
        <div class="container-box">
            <asp:Repeater runat="server" ID="rptNucleos">
                <ItemTemplate>
                    <div class="col-xs-12 col-sm-demanda">
                        <div class="panel panel-default">
                            <div class="panel-heading heading-demandas">
                                <h2 class="nome-nucleo">
                                    <asp:Label runat="server" ID="lblNomeNucleo"><%# Eval("Nucleo") %></asp:Label>
                                </h2>
                            </div>

                            <div class="panel-body demanda">
                                <div class="box-content vencida">
                                    <asp:LinkButton runat="server" CommandArgument='<%# Eval("IdNucleo") %>' OnClick="CarregarEtapasNucleo_Click" CommandName="2">
                                        <p class="valor" runat="server">
                                            <asp:Label runat="server" ID="lblVencidas"><%# Eval("Vencidas") %></asp:Label>
                                        </p>
                                        <p class="status-demanda">Vencidas</p>
                                    </asp:LinkButton>

                                </div>
                            </div>

                            <div class="panel-body demanda">
                                <div class="box-content expirar">
                                    <asp:LinkButton runat="server" CommandArgument='<%# Eval("IdNucleo") %>' OnClick="CarregarEtapasNucleo_Click" CommandName="1">
                                        <p class="valor" runat="server">
                                            <asp:Label runat="server" ID="lblAExpirar"><%# Eval("AExpirar") %></asp:Label>
                                        </p>
                                        <p class="status-demanda">A expirar</p>
                                    </asp:LinkButton>
                                </div>
                            </div>

                            <div class="panel-body demanda">
                                <div class="box-content no-prazo">
                                    <asp:LinkButton runat="server" CommandArgument='<%# Eval("IdNucleo") %>' OnClick="CarregarEtapasNucleo_Click" CommandName="0">
                                        <p class="valor" runat="server">
                                            <asp:Label runat="server" ID="lblNoPrazo"><%# Eval("NoPrazo") %></asp:Label>
                                        </p>
                                        <p class="status-demanda">No Prazo</p>
                                    </asp:LinkButton>
                                </div>
                            </div>

                            <div class="panel-body demanda">
                                <div class="box-content encerrada">
                                    <asp:LinkButton runat="server" CommandArgument='<%# Eval("IdNucleo") %>' OnClick="CarregarEtapasNucleo_Click" CommandName="3">
                                        <p class="valor" runat="server">
                                            <asp:Label runat="server" ID="lblEncerradas"><%# Eval("Encerradas") %></asp:Label>
                                        </p>
                                        <p class="status-demanda">Encerradas</p>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>

    <!-- MODAL -->
    <asp:Panel ID="pnlModal" runat="server" Visible="false" ClientIDMode="Static">
        <div aria-hidden="false" aria-labelledby="myModalLabel" data-toggle="modal" role="dialog" tabindex="-1" id="myModal" class="modal fade in" style="display: block;">
            <div class="modal-dialog modal-sm" role="document">
                <div id="mHeader" runat="server" clientidmode="Static">
                    <div class="modal-content">
                        <div class="modal-header demandas">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true" runat="server" 
                                onserverclick="OcultarModal_Click" id="btnFecharModal">&times;</button>
                                <%--<h4 class="modal-title">Confirmação</h4>--%>
                            <div class="informacoes-nucleo">
                                <h4>Nucleo: </h4>
                                <asp:Label ID="lblNucleo" runat="server"></asp:Label>
                            </div>
                            <div class="informacoes-nucleo">
                                <h4>Status: </h4>
                                <asp:Label ID="lblStatus" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="modal-body">
                            <asp:Panel ID="pnlEtapas" runat="server">
                                <table class="table table-striped">
                                    <thead>
                                        <tr>
                                            <th>Nº</th>
                                            <th>Nome</th>
                                            <th>Demandante</th>
                                            <th>Responsável</th>
                                            <th>Prazo</th>
                                            <th>Detalhes</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptDemandas" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <td><%# Eval("IdProcessoResposta") %></td>
                                                    <td><%# Eval("NomeProcessoResposta") %></td>
                                                    <td><%# Eval("Demandante") %></td>
                                                    <td><%# Eval("ObterAnalistas") %></td>
                                                    <td><%# Eval("Prazo") %></td>
                                                    <td class="detalhes-menu">
                                                        <ul>
                                                            <li>
                                                                <a class="collapse-head lnk-demanda" role="button" data-toggle="collapse" href='#collapse-<%# Eval("IdProcessoResposta")%>' aria-expanded="false" aria-controls="collapseExample">[+]</a>
                                                            </li>
                                                            <li>
                                                                <asp:LinkButton Text="" runat="server" OnClick="btnPdf_Click" CssClass="lnk-detalhes glyphicon glyphicon-print " CommandArgument='<%#Eval("IdProcessoResposta")%>' />
                                                            </li>
                                                        </ul>
                                                    </td>
                                                </tr>
                                                <tr id="collapse-<%#Eval("IdProcessoResposta")%>" class="collapse">
                                                    <td colspan="6">
                                                        <table class="table table-striped">
                                                            <thead>
                                                                <tr id="head-detalhes">
                                                                    <th>Etapa
                                                                    </th>
                                                                    <th>Data Prevista
                                                                    </th>
                                                                    <th>Data Realizada
                                                                    </th>
                                                                    <th>Status
                                                                    </th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                <asp:Repeater runat="server" ID="rptEtapas" DataSource='<%# Eval("Etapas") %>'>
                                                                    <ItemTemplate>
                                                                        <tr>
                                                                            <td>
                                                                                <%# Eval("NomeEtapa") %>
                                                                            </td>
                                                                            <td>
                                                                                <%# Eval("Prazo") %>
                                                                            </td>
                                                                            <td>
                                                                                <%# Eval("DataPreenchimento") %>
                                                                            </td>
                                                                            <td>
                                                                                <%# Eval("ObterStatusFormatado") %>
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:Repeater>
                                                            </tbody>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>

                                <div class="col-sm-12 manualPagination">
                                    <ul id="list-rpt" class="list-unstyled list-inline">
                                        <asp:Repeater ID="rptDemandasPager" runat="server" ClientIDMode="Static">
                                            <ItemTemplate>
                                                <li>
                                                    <asp:LinkButton ID="lnkPage" data-dismiss="modal" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
                                                        CssClass='<%# Convert.ToBoolean(Eval("Enabled")) ? "current" : "mostrarload" %> lnk-demanda'
                                                        OnClick="lnkPage_Click" ClientIDMode="Static"></asp:LinkButton>
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>
                                </div>
                            </asp:Panel>
                        </div>
                        <asp:Panel ID="pnlSemResultado" runat="server">
                            <p class="sem-resultados">Não há demandas com esse status</p>
                        </asp:Panel>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->
    </asp:Panel>

    <script type="text/javascript">

        // TODO: Descobrir porque os eventos não funcionam no codebehind
        $(function () {
            $("#list-rpt a").click(function (e) {
                var url = $(e.target).attr("href");
                eval(url);
            });

            $(".lnk-detalhes").click(function (e) {
                var url = $(e.target).attr("href");

                if (url) {
                    eval(url);
                }
            });

            $(".collapse-head").click(function (e) {
                var classeHeader = $('#mHeader').attr('class');
                $("#head-detalhes").attr('class', classeHeader);
            });

            $("#myModal").on('shown', function () {
                alert("I want this to appear after the modal has opened!");
            });
        });
    </script>
</asp:Content>
