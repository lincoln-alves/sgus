<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="EdicaoTrilhaNivel.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.TrilhaNivel.EdicaoTrilhaNivel" %>

<%@ Register Src="~/UserControls/ucPermissoes.ascx" TagName="ucPermissoes" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var _preloadedListTrilha = <%= ViewState["_Trilhas"] ?? "''" %>;
        AutoCompleteDefine(_preloadedListTrilha, '#txtTrilha', true, true, false);
    </script>

    <fieldset>
        <asp:HiddenField ID="hdAcaoTrilhaNivel" runat="server" />
        <asp:HiddenField ID="hdTrilhaNivelID" runat="server" />
        <asp:HiddenField ID="hdMapUrl" runat="server" />

        <div class="form-group">
            <asp:Label ID="Label4" runat="server" Text="Trilha" AssociatedControlID="txtTrilha"></asp:Label>
            <asp:TextBox ID="txtTrilha" ClientIDMode="Static" runat="server" OnTextChanged="txtTrilha_TextChanged"></asp:TextBox>
        </div>
        <asp:Panel ID="pnlItemTrilha" Visible="false" runat="server">
            <div class="form-group">
                <asp:Label ID="Label1" runat="server" Text="Nome:" AssociatedControlID="txtNomeNivel" />
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip15" Chave="txtNomeNivel" />
                <asp:TextBox ID="txtNomeNivel" runat="server" CssClass="form-control" MaxLength="250"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="Label22" runat="server" Text="Descrição:" AssociatedControlID="txtDescricaoTrilhaNivel" />
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip25" Chave="txtDescricaoTrilhaNivel" />
                <asp:TextBox ID="txtDescricaoTrilhaNivel" runat="server" TextMode="MultiLine" CssClass="form-control" />
            </div>
            <div class="form-group">
                <asp:Label ID="lblCargaHoraria" runat="server" Text="Carga Horaria" AssociatedControlID="txtCargaHoraria" />
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip23" Chave="txtCargaHoraria" />
                <asp:TextBox ID="txtCargaHoraria" runat="server" CssClass="form-control" MaxLength="4"
                    onkeypress="return EhNumerico(event)" />
            </div>
            <div class="form-group">
                <asp:Label ID="Label31" runat="server" Text="Tipo de Mapa:" AssociatedControlID="ddlTipoMapa" />
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip34" Chave="ddlTipoMapa" />
                <asp:DropDownList ID="ddlTipoMapa" runat="server" CssClass="form-control" onchange="changeMap();" />
            </div>
            <div class="form-group">
                <asp:Label ID="Label18" runat="server" Text="Ordem:" AssociatedControlID="ddlOrdem" />
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip29" Chave="ddlOrdem" />
                <asp:DropDownList ID="ddlOrdem" runat="server" CssClass="form-control">
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <uc:LupaUsuario ID="ucLupaUsuarioMonitor" runat="server" />
            </div>
            <div class="form-group">
                <asp:Label ID="Label11" runat="server" Text="Prova:" AssociatedControlID="ddlQuestionarioProva" />
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip19" Chave="ddlQuestionarioProva" />
                <asp:DropDownList ID="ddlQuestionarioProva" runat="server" CssClass="form-control" />
            </div>
            <div class="form-group">
                <asp:Label ID="Label17" runat="server" Text="Nota Mínima da Prova Final:" AssociatedControlID="txtValorNotaMinima" />
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip28" Chave="txtValorNotaMinima" />
                <asp:TextBox ID="txtValorNotaMinima" runat="server" CssClass="form-control" MaxLength="4"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="Label28" runat="server" Text="Quantidade de Moedas de Ouro para Prova Final:" AssociatedControlID="txtQuantidadeMoedasProvaFinal" />
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip4" Chave="txtQuantidadeMoedasProvaFinal" />
                <asp:TextBox runat="server" ID="txtQuantidadeMoedasProvaFinal" CssClass="form-control" />
            </div>
            <div class="form-group">
                <asp:Label ID="Label15" runat="server" Text="Prazo para Conclusão do Nível (Dias)" AssociatedControlID="txtPrazo" />
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip22" Chave="txtPrazo" />
                <asp:TextBox ID="txtPrazo" runat="server" CssClass="form-control" MaxLength="4" onkeypress="return EhNumerico(event)" />
            </div>
            <div class="form-group">
                <asp:Label runat="server" ID="Label30" Text="Prazo Limite de Cancelamento" AssociatedControlID="txtLimiteCancelamento"></asp:Label>
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip27" Chave="txtLimiteCancelamento" />
                <asp:TextBox runat="server" ID="txtLimiteCancelamento" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="Label26" runat="server" Text="Termo de Aceite:" AssociatedControlID="ddlTermoAceite" />
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip31" Chave="ddlTermoAceite" />
                <asp:DropDownList ID="ddlTermoAceite" runat="server" CssClass="form-control" />
            </div>
            <div class="form-group">
                <asp:Label ID="Label10" runat="server" Text="Entrevista:" AssociatedControlID="ddlQuestionarioPos" />
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip18" Chave="ddlQuestionarioPos" />
                <asp:DropDownList ID="ddlQuestionarioPos" runat="server" CssClass="form-control" />
            </div>
            <div class="form-group">
                <asp:Label ID="Label13" runat="server" Text="Template do Certificado:" AssociatedControlID="ddlCertificadoTemplate" />
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip20" Chave="ddlCertificadoTemplate" />
                <asp:DropDownList ID="ddlCertificadoTemplate" runat="server" CssClass="form-control" />
            </div>
            <div class="form-group" id="porcentagenTrofeus">
                <asp:Label ID="Label32" runat="server" Text="Porcentagem de Parâmetros de Troféus" AssociatedControlID="txtPorcentagensTrofeus" />
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip30" Chave="txtPorcentagensTrofeus" />
                <asp:HiddenField runat="server" ID="txtPorcentagensTrofeus" Value="33,66" />
            </div>
            <div class="form-group">
                <asp:Label ID="Label12" runat="server" Text="Câmbio de Moedas:" AssociatedControlID="txtCambioMoedas" />
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip13" Chave="txtCambioMoedas" />
                <asp:TextBox runat="server" ID="txtCambioMoedas" CssClass="form-control" />
            </div>
            <div class="form-group">
                <asp:Label ID="Label16" runat="server" Text="Quantidade de Moedas de Prata por Curtidas:" AssociatedControlID="txtQuantidadeMoedasPorCurtida" />
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip32" Chave="txtQuantidadeMoedasPorCurtida" />
                <asp:TextBox runat="server" ID="txtQuantidadeMoedasPorCurtida" CssClass="form-control" />
            </div>
            <div class="form-group">
                <asp:Label ID="Label29" runat="server" Text="Quantidade de Moedas de Prata por Descurtidas:" AssociatedControlID="txtQuantidadeMoedasPorDescurtida" />
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip33" Chave="txtQuantidadeMoedasPorDescurtida" />
                <asp:TextBox runat="server" ID="txtQuantidadeMoedasPorDescurtida" CssClass="form-control" />
            </div>
            <div class="panel panel-default">
                <div class="panel-heading">
                    <a data-toggle="collapse" data-parent="#accordionTN" href="#collapse11">Permissões
                    </a>
                </div>
                <div id="collapse11" class="panel-collapse collapse">
                    <div class="panel-body">
                        <uc1:ucPermissoes ID="ucPermissoesNivel" runat="server" />
                    </div>
                </div>
            </div>
            <div class="form-group">
                <asp:Label ID="Label8" runat="server" Text="Inscrições Abertas:" AssociatedControlID="ddlAceitaNovasMatriculas" />
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip16" Chave="ddlAceitaNovasMatriculas" />
                <asp:DropDownList ID="ddlAceitaNovasMatriculas" runat="server" CssClass="form-control" />
            </div>
            <div class="form-group">
                <asp:Label ID="Label14" runat="server" Text="Pré-Requisito:" AssociatedControlID="ddlPreRequisito" />
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip21" Chave="ddlPreRequisito" />
                <asp:DropDownList ID="ddlPreRequisito" runat="server" CssClass="form-control" />
            </div>


            <asp:Panel ID="MapaPreview" runat="server" Style="height: 380px;">
                <asp:HtmlIframe runat="server" ID="MapaFrame" style="width: 100%; border: none; overflow: hidden; height: 100%;"></asp:HtmlIframe>
            </asp:Panel>

        </asp:Panel>
    </fieldset>

    <hr />
    <div>
        <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click"
            CausesValidation="true" CssClass="btn btn-primary mostrarload" />

        <asp:Button ID="btnCancelarTrilha" runat="server" Text="Cancelar" OnClick="btnCancelarTrilha_Click"
            CausesValidation="true" CssClass="btn btn-default" />
    </div>

    <script type="text/javascript">
        validarForm();
        // Adicionando limite de 200 caracteres para o campo descrição
        $("#<%= txtDescricaoTrilhaNivel.ClientID %>").attr("maxlength", "250");

        $(function () {
            if ($("#porcentagenTrofeus").length > 0) {
                var rangeValue = $("#<%= txtPorcentagensTrofeus.ClientID %>").val().split(",").map(Number);

                var rangeTrofeus = new RangeBar({
                    values: [rangeValue], // array of value pairs; each pair is the min and max of the range it creates
                    label: function (a) {
                        return "Prata " + Math.round(a[0]) + "% à " + Math.round(a[1]) + "%";
                    },
                    readonly: false, // whether this bar is read-only
                    min: 0, // value at start of bar
                    max: 100, // value at end of bar
                    valueFormat: function (a) { return a; }, // formats a value on the bar for output
                    valueParse: function (a) { return a; }, // parses an output value for the bar
                    snap: 1, // clamps range ends to multiples of this value (in bar units)
                    minSize: 20, // smallest allowed range (in bar units)
                    maxRanges: 1, // maximum number of ranges allowed on the bar
                    indicator: null, // pass a function(RangeBar, Indicator, Function?) Value to calculate where to put a current indicator, calling the function whenever you want the position to be recalculated
                    allowDelete: false, // set to true to enable double-middle-click-to-delete
                    deleteTimeout: 5000, // maximum time in ms between middle clicks
                    vertical: false, // if true the rangebar is aligned vertically, and given the class elessar-vertical
                    bounds: null, // a function that provides an upper or lower bound when a range is being dragged. call with the range that is being moved, should return an object with an upper or lower key
                    htmlLabel: false, // if true, range labels are written as html
                    allowSwap: false, // swap ranges when dragging past
                    barClass: 'progress',
                    rangeClass: 'progress-bar barra-prata'
                }).on('changing', function (ev, ranges, element) {
                    $("#<%= txtPorcentagensTrofeus.ClientID %>").val(ranges[0]);
                    calcBars(ranges[0]);
                });

                $("#porcentagenTrofeus").append(rangeTrofeus.$el);
                $(rangeTrofeus.$el).append("<div class=\"elessar-labels\"> <div class=\"elessar-label barra-bronze\">Bronze 0% à 33%</div> <div class=\"elessar-label barra-ouro\">Ouro 66% à 100%</div> </di>");

                calcBars(rangeValue);
            }
        });

        function calcBars(range) {
            $(".barra-bronze").css("width", Math.round(range[0]) + "%").html("Bronze 0% à " + Math.round(range[0] - 1) + "%");
            $(".barra-ouro").css("left", Math.round(range[1]) + "%").css("width", Math.round(100 - range[1]) + "%").html("Ouro " + Math.round(range[1] + 1) + "% à 100%");
        }

        function changeMap() {
            var nivelId = $("#<%= hdTrilhaNivelID.ClientID %>").val();
            var mapaId = $("#<%= ddlTipoMapa.ClientID %>").val();
            var url = $("#<%= hdMapUrl.ClientID %>").val();
            if (nivelId != undefined) {
                $("#<%= MapaFrame.ClientID %>").attr('src', url + mapaId + '/' + nivelId);
            } else {
                $("#<%= MapaFrame.ClientID %>").attr('src', url + mapaId);
            }
        }

    </script>
</asp:Content>
