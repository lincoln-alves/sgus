<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="MonitoramentoTurma.aspx.cs" Inherits="Sebrae.Academico.WebForms.MonitoramentoTurma" %>

<%@ Import Namespace="Sebrae.Academico.Dominio.Enumeracao" %>
<%@ Import Namespace="Sebrae.Academico.Util.Classes" %>

<%@ Register Src="~/UserControls/ucCategorias.ascx" TagName="ucCategorias" TagPrefix="uc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <style>
        body, html {
            background-color: #b6b6b6;
        }

        .panel-body {
            padding-bottom: 0;
        }

        thead {
            color: #000 !important;
            background-color: #dfdfdf !important;
        }

        tbody {
            font-weight: 600;
        }

        .hiddencol {
            display: none;
        }
    </style>
    <div class="container">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <%--    <script src="~/js/jquery/jquery-1.10.2.min.js"></script>--%>
                <script src="/js/jquery/jquery.signalR-2.2.0.min.js"></script>
                <script src="http://localhost:7127/UI/hubs"></script>

                <script type="text/javascript">
                    $(function () {

                        $(function () {
                            // Recupera nome da conexão do sgus. (O nome é mapeado baseado no nome da classe).
                            var sgusHub = $.connection.uiHub;

                            // Funcao usada para atualizar notificacoes do minha pagina e do menu lateral
                            sgusHub.client.atualizarTurma = function (json) {
                                json = JSON.parse(json);
                                var tr = $("[data-turma='" + json.id + "']");
                                var alunos = parseInt($(tr).text());
                                alunos += parseInt(json.quantidade);
                                $(tr).text(alunos);
                            };

                            // Mapeia URL de conexão.
                            $.connection.hub.url = 'http://localhost:7127/UI';

                            $.connection.hub.start()
                                .done(function () { console.log('Now connected, connection ID=' + $.connection.hub.id); })
                                .fail(function () { console.log('Could not Connect!'); });
                        });
                    });
                </script>

                <div class="panel panel-default">
                    <div class="panel-heading">
                        <a data-toggle="collapse" data-target="#collapse-filtros" href="#">
                            <span>Filtros</span>
                        </a>
                    </div>
                    <div class="panel-body" id="collapse-filtros">
                        <section class="form-group">
                            <label>Categoria</label>
                            <uc1:ucCategorias runat="server" ID="ucCategoriasConteudo" class="form-control"  />
                         </section>

                        <section class="form-group">
                            <asp:Label ID="Label1" runat="server" Text="Data Início da Turma" AssociatedControlID="txtDataInicialTurma" data-help="<%$ Resources:Resource, dataFim%>" />
                            <asp:TextBox runat="server" ID="txtDataInicialTurma" CssClass="form-control"></asp:TextBox>
                        </section>

                        <section class="form-group">
                           <asp:Label ID="Label2" runat="server" Text="Data Fim da Turma" AssociatedControlID="txtDataFinalTurma" data-help="<%$ Resources:Resource, dataFim%>" />
                            <asp:TextBox runat="server" ID="txtDataFinalTurma" CssClass="form-control" ></asp:TextBox>
                        </section>

                        <section class="form-group">
                            <asp:label AssociatedControlID="chkStatus">Status</asp:label>
                            <asp:CheckBoxList runat="server" ID="chkStatus" />
                        </section>
                    </div>
                    <div class="panel-footer">
                        <asp:Button runat="server" ID="btnFiltrar" OnClick="btnFiltrar_OnClick" Text="Filtrar" CssClass="btn btn-default" />
                    </div>
                </div>

                <div class="panel panel-primary">
                    <div class="panel-heading">Total por Status</div>
                    <div class="panel-body">
                        <div class="row">
                            <asp:GridView runat="server" ID="grdTotalMatriculas" AutoGenerateColumns="False" CssClass="table col-sm-12" AllowPaging="True" PageSize="50">
                                <Columns>
                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <span><%# Eval("StatusFormatado") != "" ? Eval("StatusFormatado") : "Sem Status" %></span>
                                        </ItemTemplate>
                                        <ItemStyle Width="50%" />
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Total de Turmas" DataField="TotalTurmas" />
                                    <asp:BoundField HeaderText="Total de Alunos" DataField="TotalMatriculados" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>

                <div class="panel panel-primary">
                    <div class="panel-heading">Turmas</div>
                    <div class="panel-body">
                        <div class="row">
                            <asp:GridView runat="server" ID="grdTurma" AutoGenerateColumns="False" CssClass="table col-sm-12">
                                <Columns>
                                    <asp:BoundField HeaderText="Nome da Turma" DataField="NomeTurma" />
                                    <asp:BoundField HeaderText="Status" DataField="StatusFormatado" />
                                    <asp:TemplateField HeaderText="Total de Alunos">
                                        <ItemTemplate>
                                            <span data-turma="<%#Eval("ID_Turma") %>" class="qntAlunos" /><%#Eval("TotalMatriculados") %></span>
                                        </ItemTemplate>
                                        <ItemStyle Width="200px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <fieldset runat="server" id="componenteGeracaoRelatorio" clientidmode="Static" visible="true">
            <hr />
            <div class="form-group">
                <uc:SaidaRel ID="ucFormatoSaidaRelatorio" runat="server" />
            </div>
            <div class="form-group">
                <asp:Button ID="btnGerarRelatorio" runat="server" Text="Gerar Relatório" OnClick="btnGerarRelatorio_OnClick"
                    CssClass="btn btn-primary" />
            </div>
        </fieldset>
    </div>
    <script>
        jQuery(document).ready(function () {
            formatarMascara("#<%= txtDataInicialTurma.ClientID %>", "99/99/9999");
            formatarMascara("#<%= txtDataFinalTurma.ClientID %>", "99/99/9999");
        });

        function formatarMascara(id, formato) {
            $(id).mask(formato, {
                autoclear: false
            }).on('blur', function () {
                var valor = $(id).val();
                var dados = valor.split(' ');
                var data = validDate(dados[0]);
                if (!data.result) {
                    $(id).val('');
                } else {
                    dados[1] = dados[1].replace(/_/g, "0");
                    dados[1] = validHour(dados[1]);
                    $(id).val(dados.join(' '));
                }
            });
        }
    </script>
</asp:Content>
