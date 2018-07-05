<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="Detalhes.aspx.cs" Inherits="Sebrae.Academico.WebForms.Relatorios.HistoricoDemandas.Detalhes" %>

<%@ Import Namespace="Sebrae.Academico.Dominio.Enumeracao" %>

<script runat="server">
    protected string GetStatus(object id)
    {
        return GetDescription((enumStatusEtapaResposta)id);
    }

</script>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .campo-formulario {
            width: 200px;
            float: left;
            margin: 8px 20px;
        }

        .invisible {
            display: none;
        }

        .visible {
            display: block !important;
        }
    </style>
    <div class="btn-group pull-right">
        <button class="btn btn-default" type="button" runat="server" onserverclick="btnPdf_Click" name="btnPDF" title="PDF">
            <i class="glyphicon glyphicon-print"></i>
        </button>

        <button class="btn btn-default" type="button" name="btnModeView" title="Modo de Visualização do Processo: Completo">
            <i class="glyphicon glyphicon-sort-by-attributes"></i>
        </button>
    </div>

    <h2>Demanda:
        <asp:Label ID="lblProcesso" runat="server"></asp:Label>
    </h2>
     <p>
        <b>Número da demanda:</b>
        <asp:Label ID="lblNumeroDemanda" runat="server"></asp:Label>
    </p>
    <p>
        <b>Demandado por:</b>
        <asp:Label ID="lblDemandante" runat="server"></asp:Label>
    </p>

    <asp:Panel runat="server" ID="pnlJustificativa" Visible="false">
        <asp:Label runat="server" ID="lblStatusCancelamento"><strong>Status: </strong>Cancelado</asp:Label><br />
        <fieldset style="border: 1px solid #c0c0c0; padding: 16px; margin: 8px 0px;">
            <legend style="font-size: 14px; width: auto; border-width: 0; padding: 5px; margin-bottom: 0px;">Justificativa</legend>
            <asp:Label runat="server" ID="lblJustificativaCancelamento"></asp:Label>
        </fieldset>
    </asp:Panel>

    <hr />

    <div>
        <ul class="nav nav-tabs invisible" role="tablist" id="tabEtapas">
            <asp:Repeater ID="rptEtapasTab" runat="server">
                <ItemTemplate>
                    <li role="presentation" class="<%# Eval("Situacao.Nome").ToString() == "Aguardando" ? "active" : "" %>"
                        title="<%# Eval("Situacao.Nome").ToString() %>">
                        <a href="#<%# Eval("ID")%>" aria-controls="<%# Eval("ID")%>" role="tab" data-toggle="tab">
                            <%# (Container.ItemIndex + 1) %>
                            <i class="glyphicon glyphicon-<%# Eval("Situacao.Nome").ToString() == "Aguardando" ? "time" : "ok" %>"></i>
                        </a>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>

        <div id="conteudoAba" class="">

            <asp:Repeater ID="rptEtapas" runat="server" OnItemDataBound="rptEtapas_OnItemDataBound">

                <ItemTemplate>
                    <div role="tabpanel" class="tab-pane <%# Eval("Situacao.Nome").ToString() == "Aguardando" ? "active" : "" %>" id="<%# Eval("ID")%>">
                        <h3>Etapa: <%# Eval("Nome")%></h3>

                        <div class="row text-center">
                            <div class="col-xs-3">
                                <strong>Requer Aprovação</strong>
                                <br />
                                <asp:Label runat="server" ID="Label1"><%# bool.Parse(Eval("RequerAprovacao").ToString()) ? "Sim" : "Não" %></asp:Label>
                            </div>
                            <div class="col-xs-2">
                                <strong>Status</strong>
                                <br />
                                <asp:Label runat="server" ID="Label2"><%# GetStatus(Eval("Status")) %></asp:Label>
                            </div>

                            <div class="col-xs-2 pull-right">
                                <strong><%# Eval("EtapaEncaminhamentoUsuario.StatusEncaminhamento") != null ? "Status" : "" %></strong>
                                <br />
                                <asp:Label runat="server" ID="Label3"><%# Eval("EtapaEncaminhamentoUsuario.Status") %></asp:Label>
                            </div>
                            <div class="col-xs-3 pull-right">
                                <strong>Etapa Encaminhada</strong>
                                <br />
                                <asp:Label runat="server" ID="Label4"><%# Eval("EtapaEncaminhamentoUsuario.ID_EtapaEncamihamentoUsuario") != null ? "Sim" : "Não" %></asp:Label>
                            </div>                            
                        </div>

                        <br />

                        <fieldset style="border: 1px solid #c0c0c0; padding: 16px; margin: 8px 0px;">
                            <legend style="font-size: 14px; width: auto; border-width: 0; padding: 5px; margin-bottom: 0px;">Formulário:</legend>

                            <div runat="server" id="divDataPreenchimento" class="form-group" visible="False">
                                <strong>DATA DE PREENCHIMENTO:</strong>
                                <br />
                                <asp:Label runat="server" ID="lblDataPreenchimento"><%# Eval("DataPreenchimento")%> </asp:Label>
                            </div>

                            <div class="row">
                                <asp:Repeater ID="rptFormulario" runat="server" OnItemDataBound="rptFormulario_OnItemDataBound">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQuebrarLinha" Visible="False" runat="server"></asp:Label>
                                        <div runat="server" id="divAbrirCampo" clientidmode="Static">
                                            <div class="form-group">
                                                <strong>
                                                    <asp:Label ID="lblNomeCampo" runat="server"></asp:Label></strong>
                                                <br />
                                                <asp:Label ID="lblResposta" runat="server"></asp:Label>
                                                <asp:CheckBoxList ID="chkListaAlternativas" CssClass="checkbox_tree_lg" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server"></asp:CheckBoxList>
                                            </div>

                                            <div class="form-group">
                                                <asp:Repeater ID="rptRespostasQuestionario" runat="server">
                                                    <ItemTemplate>
                                                        <div class="col-md-6 col-xs-12">
                                                            <div class="form-group">
                                                                <asp:Label ID="lblQuestionario" runat="server" CssClass="questionario enunciado">
                                                                <%# Eval("Questao") %>
                                                                </asp:Label>
                                                                <br />
                                                                <asp:Label ID="Label3" runat="server">
                                                                <%# Eval("Resposta") %>
                                                                </asp:Label>
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </div>
                                        </div>

                                        <asp:Label ID="lblFecharCampo" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:Repeater>

                            </div>

                            <div class="clearfix"></div>

                            <h4 class="text-center">
                                <%# Eval("ObterAnalistas")%>
                            </h4>

                            <asp:Panel runat="server" ID="pnlJustificativaRodape" Visible="false">
                                <fieldset style="border: 1px solid #c0c0c0; padding: 16px; margin: 8px 0px;">
                                    <legend style="font-size: 14px; width: auto; border-width: 0; padding: 5px; margin-bottom: 0px;">Justificativa</legend>
                                    <asp:Label runat="server" ID="lblJustificativaCancelamentoRodape"></asp:Label>
                                </fieldset>
                            </asp:Panel>

                        </fieldset>

                        <hr />
                        <br />
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>

    <input type="button" value="Voltar" class="btn btn-default" onclick="history.go(-1);" />

    <script type="text/javascript">
        $(function () {
            $('button[name=btnModeView]').click(function () {
                if ($('#tabEtapas').hasClass("invisible")) {
                    $('#tabEtapas').removeClass("invisible");
                    $('#conteudoAba').addClass('tab-content');
                    $(this).children().removeClass("glyphicon-sort-by-attributes").addClass("glyphicon-align-justify");
                    $(this).attr('title', 'Modo de Visualização do Processo: Etapa');
                } else {
                    $('#tabEtapas').addClass("invisible");
                    $('#conteudoAba').removeClass('tab-content');
                    $(this).children().removeClass("glyphicon-align-justify").addClass("glyphicon-sort-by-attributes");
                    $(this).attr('title', 'Modo de Visualização do Processo: Completo');
                }
            });
        });
    </script>

</asp:Content>
