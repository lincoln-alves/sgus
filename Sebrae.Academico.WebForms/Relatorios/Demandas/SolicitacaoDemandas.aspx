<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="SolicitacaoDemandas.aspx.cs" Inherits="Sebrae.Academico.WebForms.Relatorios.Demandas.SolicitacaoDemandas" %>


<%@ Register Src="~/UserControls/ucSeletorListBox.ascx" TagName="ucSeletorListBox" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucLupaUsuario.ascx" TagName="ucLupaUsuario" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucTotalizadorRelatorio.ascx" TagPrefix="uc" TagName="ucTotalizadorRelatorio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-group" id="gruporelatorio">
        <div class="panel panel-default">
            <div class="panel-heading">
                <a data-toggle="collapse" data-parent="#gruporelatorio" id="btnFiltro" href="#Filtros">Filtros</a>
            </div>
            <asp:Panel ID="Filtros" runat="server" ClientIDMode="Static" CssClass="panel-collapse collapse">
                <div class="panel-body">
                    <fieldset>
                        <div class="form-group">
                            <uc1:ucLupaUsuario ID="ucLupaUsuario" IsNacional="true" Chave="lupaUsuario" runat="server" Text="Usuário Demandante" />
                        </div>

                        <div class="form-group">
                            <asp:Label ID="Label1" runat="server" Text="Número da Demanda" AssociatedControlID="txtProcessoResposta" />
                            <asp:TextBox ID="txtProcessoResposta" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <asp:Label ID="Label5" runat="server" Text="Data Abertura" AssociatedControlID="txtDataAbertura" />
                            <asp:TextBox ID="txtDataAbertura" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <asp:Label ID="lblProcesso" runat="server" Text="Demanda" AssociatedControlID="cbxProcesso" />
                            <asp:DropDownList ID="cbxProcesso" runat="server" CssClass="form-control" OnSelectedIndexChanged="cbxProcesso_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </div>

                        <div class="form-group">
                            <asp:Label ID="Label2" runat="server" Text="Etapa da Demanda" AssociatedControlID="cbxDemanda" />
                            <asp:DropDownList ID="cbxDemanda" runat="server" CssClass="form-control" OnSelectedIndexChanged="cbxDemanda_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </div>

                        <div class="form-group" runat="server">
                            <asp:Label ID="labelUF" runat="server" Text="Status" AssociatedControlID="cbxStatus" />
                            <asp:DropDownList ID="cbxStatus" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
                        </div>

                        <div class="form-group">
                            <asp:Label ID="Label3" runat="server" Text="Data de Início do Período" AssociatedControlID="txtDataInicio" />
                            <asp:TextBox ID="txtDataInicio" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <asp:Label ID="Label6" runat="server" Text="Data de Fim do Período" AssociatedControlID="txtDataFim" />
                            <asp:TextBox ID="txtDataFim" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>

                        <div class="form-group" runat="server">
                            <asp:Label ID="label4" runat="server" Text="Unidade" AssociatedControlID="listUnidades" />
                            <uc1:ucSeletorListBox runat="server" ID="listUnidades" />
                        </div>
                    </fieldset>
                </div>
            </asp:Panel>
        </div>
        <div class="panel panel-default">
            <div class="panel-heading">
                <a data-toggle="collapse" data-parent="#gruporelatorio" href="#Campos" title="As horas são agrupadas de acordo com os campos exibidos">Campos a Serem
                    Exibidos</a>
            </div>
            <div id="Campos" class="accordion-body collapse">
                <div class="accordion-inner">
                    <fieldset>
                        <div class="form-group">
                            <asp:CheckBoxList ID="chkListaCamposVisiveis" runat="server" RepeatDirection="Vertical"
                                RepeatLayout="UnorderedList" ClientIDMode="Static">
                            </asp:CheckBoxList>
                        </div>
                    </fieldset>
                </div>
            </div>
        </div>
    </div>

    <br />
    <asp:Button ID="btnPesquisar" runat="server" Text="Consultar" OnClick="btnPesquisar_Click"
        CssClass="btn btn-primary mostrarload" />
    <hr />

    <asp:Panel CssClass="panel-group" ID="divTotalizadores" Visible="false" runat="server" ClientIDMode="Static">
        <div class="panel panel-default">
            <div class="panel-heading">
                <a data-toggle="collapse" data-parent="#grupototalizador" href="#Totais">Totalizador</a>
            </div>
            <asp:Panel ID="Totais" runat="server" ClientIDMode="Static" CssClass="panel-collapse collapse">
                <div class="panel-body">
                    <fieldset>
                        <div class="form-control">
                            <asp:Label ID="lblQuantidadeEncontrada" runat="server"></asp:Label>
                        </div>
                        <br />
                        <div class="form-control">
                            <asp:Label ID="lblValorTotalPrevisto" runat="server"></asp:Label>
                        </div>
                        <br />
                        <div class="form-control">
                            <asp:Label ID="lblValorTotalExecutado" runat="server"></asp:Label>
                        </div>
                    </fieldset>
                </div>
            </asp:Panel>
        </div>
    </asp:Panel>
    <br />

    <asp:Panel ID="dvResultado" runat="server" ClientIDMode="Static" CssClass="table-responsive" Visible="false">

        <table class="table table-stripped">
            <thead>
                <tr id="cabecalho">
                    <asp:Repeater ID="rptCabecalho" runat="server">
                        <ItemTemplate>
                            <th runat="server" visible='<%# ExibirCampo(Eval("Nome")) %>' ><%# Eval("Nome") %></th>
                        </ItemTemplate>
                    </asp:Repeater>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rptRelatorio" runat="server" OnItemDataBound="rptRelatorio_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td data-head="Número do Processo" runat="server" visible='<%# ExibirCampo("Número do Processo") %>'><%#Eval("NumeroProcesso") %></td>
                            <td data-head="Usuário Demandante" runat="server" visible='<%# ExibirCampo("Usuário Demandante") %>'><%#Eval("UsuarioDemandante") %></td>
                            <td data-head="Demanda" runat="server" visible='<%# ExibirCampo("Demanda") %>'><%#Eval("Demanda") %></td>
                            <td data-head="Data de Abertura" runat="server" visible='<%# ExibirCampo("Data Abertura") %>'><%#Eval("DataAbertura") %></td>
                            <td data-head="Etapa Atual" runat="server" visible='<%# ExibirCampo("Etapa Atual") %>'><%#Eval("EtapaAtual") %></td>
                            <td data-head="Status" runat="server" visible='<%# ExibirCampo("Status") %>'><%#Eval("Status") %></td>
                            <asp:Repeater runat="server" ID="rptCampos">
                                <ItemTemplate>
                                    <td runat="server" visible='<%# ExibirCampo(Eval("Campo.Nome")) %>'><%#Eval("Resposta") %></td>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </asp:Panel>

    <fieldset runat="server" id="componenteGeracaoRelatorio" clientidmode="Static" visible="false">
        <hr />
        <hr />
        <div class="form-group">
            <asp:Label ID="Label7" runat="server" Text="Formato de Saída" AssociatedControlID="rblTipoSaida" />
            <asp:RadioButtonList ID="rblTipoSaida" runat="server" RepeatLayout="UnorderedList" CssClass="form-control file-types">
                <asp:ListItem Value="2" Selected="True"><i class="icon icon-ms-excel" title="EXCEL"></i></asp:ListItem>
            </asp:RadioButtonList>
        </div>
        <div class="form-group">
            <asp:Button ID="btnGerarRelatorio" runat="server" Text="Gerar Relatório" OnClick="btnGerarRelatorio_Click"
                CssClass="btn btn-primary" />
        </div>
    </fieldset>

    <script type="text/javascript" src="/js/jquery.numeric.js"></script>
    <script type="text/javascript">
        jQuery(function ($) {

            preencherCabecalho();

            $("#Filtros").on('shown.bs.collapse', function () {
                $.cookie('activeFiltroGroup', 'true');
            });

            $("#Filtros").on('hidden.bs.collapse', function () {
                $.removeCookie('activeFiltroGroup');
            });

            $("#<%= txtProcessoResposta.ClientID %>").numeric();


            $("#<%= txtDataAbertura.ClientID %>").mask("99/99/9999");
            $("#<%= txtDataInicio.ClientID %>").mask("99/99/9999");
            $("#<%= txtDataFim.ClientID %>").mask("99/99/9999");

        });

        function preencherCabecalho() {
            //var tr = $('#table-relatorio tr')[1];
            //var tdsCabecalho = $(tr).find('td');

            //tdsCabecalho.each(function () {
            //    var visible = $(this).attr('visible');
            //    if (visible != undefined && visible == 'False') {
            //        return;
            //    }

            //    nome = $(this).data('head');
            //    $('#cabecalho').append('<th>' + nome + '</th>')
            //})

            //var trs = $('#table-relatorio tr');
            //trs.each(function () {
            //    var tds = $(this).find('td');
            //    tds.each(function () {
            //        var visible = $(this).attr('visible');

            //        if (visible != undefined && visible == 'False') {
            //            $(this).hide();
            //            return;
            //        }
            //    })
            //})
        }
    </script>

</asp:Content>
