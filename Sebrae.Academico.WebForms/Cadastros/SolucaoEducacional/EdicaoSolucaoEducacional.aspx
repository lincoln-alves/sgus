<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EdicaoSolucaoEducacional.aspx.cs"
    MasterPageFile="~/Pagina.master" Inherits="Sebrae.Academico.WebForms.Cadastros.EdicaoSolucaoEducacional" %>

<%@ Register Src="~/UserControls/ucPermissoes.ascx" TagName="ucPermissoes" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucCategorias.ascx" TagName="ucCategorias" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucSeletorListBox.ascx" TagName="ucSeletorListBox" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
        var _preloadedList = <%= ViewState["_SE"] %>;
        AutoCompleteDefine(_preloadedList, '#txtSolucaoEducacional', true, false, true);

        _preloadedList = <%= ViewState["_CATEGORIAMOODLE"] %>;
        AutoCompleteDefine(_preloadedList, '#txtCategoriaMoodle', true, false, true);
    </script>

    <asp:ValidationSummary ID="valSumSolucaoEducacional" runat="server" ShowMessageBox="true" />

    <div class="panel-group" id="accordion">
        <div class="panel panel-default">
            <div class="panel-heading">
                <a data-toggle="collapse" data-parent="#accordion" href="#collapseDados">Dados </a>
            </div>
            <div id="collapseDados" runat="server" class="panel-collapse collapse in" clientidmode="Static">
                <div class="panel-body">
                    <fieldset>
                        <div class="form-group" runat="server" clientidmode="Static" id="divTxtCodigo">
                            <asp:Label ID="Label4" runat="server" Text="Código" AssociatedControlID="txtNome" />
                            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip1" Chave="codigoSequencialSolucao" />
                            <asp:TextBox ID="txtCodigo" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>

                        <asp:Panel runat="server" ID="pnlNode">
                            <div class="form-group" runat="server" clientidmode="Static" id="divIdNode">
                                <asp:Label ID="Label7" runat="server" Text="Drupal Node ID" AssociatedControlID="txtIdNode" />
                                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip2" Chave="idDrupal" />
                                <asp:TextBox ID="txtIdNode" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                        </asp:Panel>

                        <div class="form-group">
                            <asp:Label ID="Label2" runat="server" Text="Nome*" AssociatedControlID="txtNome" />
                            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip12" Chave="nomeSolucao" />
                            <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label14" runat="server" Text="Carga Horaria" AssociatedControlID="txtCargaHoraria" />
                            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip21" Chave="cargaHoraria" />
                            <asp:TextBox ID="txtCargaHoraria" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>


                        <div class="form-group" id="divAreaTematica" runat="server">
                            <asp:Label ID="lblAreaTematica" runat="server" AssociatedControlID="lblAreaTematica" Text="Área Temática"></asp:Label>
                            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip10" Chave="areaTematica" />
                            <uc2:ucSeletorListBox runat="server" ID="listBoxesAreaTematica" DescricaoDisponiveis="Lista de Áreas Temáticas" DescricaoSelecionados="Selecionados" />
                        </div>

                        <div class="form-group">
                            <asp:Label ID="Label13" runat="server" Text="Fornecedor*" AssociatedControlID="ddlFornecedor" />
                            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip20" Chave="cadastroFornecedor" />
                            <asp:DropDownList AutoPostBack="true" ID="ddlFornecedor" runat="server" CssClass="form-control mostrarload"
                                OnTextChanged="ddlFornecedor_TextChanged">
                            </asp:DropDownList>
                        </div>

                        <asp:Panel ID="pnlEventosCredenciamento" Visible="false" runat="server">
                            <div class="form-group">
                                <asp:Label runat="server" Text="Eventos" AssociatedControlID="ddlEventos"></asp:Label>
                                <uc1:ucHelperTooltip runat="server" ID="ucHelpTooltip10" Chave="cadastroEvento" />
                                <asp:DropDownList runat="server" ID="ddlEventos" CssClass="form-control mostrarload" AutoPostBack="true"></asp:DropDownList>
                            </div>
                        </asp:Panel>

                        <div class="form-group" id="divChaveExterna" runat="server">
                            <asp:Label ID="Label10" runat="server" Text="ID da Chave Externa" AssociatedControlID="txtIDChaveExterna" />
                            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip3" Chave="chaveExterna" />
                            <asp:TextBox ID="txtIDChaveExterna" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                        </div>
                        <div class="form-group" id="divddlCategoriaMoodle" runat="server">
                            <asp:Label ID="Label5" runat="server" Text="Categoria no Moodle*" AssociatedControlID="txtCategoriaMoodle" />
                            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip4" Chave="cadastroFormaAprendizagem" />
                            <asp:TextBox ID="txtCategoriaMoodle" runat="server" CssClass="form-control" ClientIDMode="Static" OnTextChanged="ddlCategoriaMoodle_TextChanged"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label3" runat="server" Text="Forma de Aquisição*" AssociatedControlID="ddlFormaAquisicao" />
                            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip5" Chave="cadastroFormaAquisicao" />
                            <asp:DropDownList ID="ddlFormaAquisicao" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        <asp:Panel runat="server" ID="pnlCategoria">
                            <div class="form-group" id="divCategoriaMoodle" runat="server">
                                <asp:Label ID="lblCategoria" runat="server" Text="CATEGORIA" AssociatedControlID="ucCategorias1" />
                                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip13" Chave="categoria" />
                                <uc1:ucCategorias ID="ucCategorias1" runat="server" />
                            </div>
                        </asp:Panel>
                        <div class="form-group">
                            <asp:Label ID="Label6" runat="server" Text="Terá ciclo de oferta?" AssociatedControlID="rblTeraOfertasContinuas" />
                            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip14" Chave="ofertasContinuas" />
                            <asp:RadioButtonList ID="rblTeraOfertasContinuas" runat="server" RepeatDirection="Horizontal" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="rblTeraOfertasContinuas_OnSelectedIndexChanged" />
                        </div>
                        <div runat="server" id="divDatasInicioFim" visible="False">
                            <div class="form-group">
                                <asp:Label ID="Label16" runat="server" Text="Data de Início do ciclo" AssociatedControlID="txtDtInicio" />
                                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip6" Chave="dataInicio" />
                                <asp:TextBox ID="txtDtInicio" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label17" runat="server" Text="Data de Fim do ciclo" AssociatedControlID="txtDtFim" />
                                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip7" Chave="dataFim" />
                                <asp:TextBox ID="txtDtFim" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label9" runat="server" Text="Termo de Aceite e Política de Consequência*" AssociatedControlID="ddlTermoAceite" />
                            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip8" Chave="termoAceite" />
                            <asp:DropDownList ID="ddlTermoAceite" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>

                        <div class="form-group">
                            <asp:Label ID="Label12" runat="server" Text="Texto de Apresentação Portal" AssociatedControlID="txtTextoApresentacao" />
                            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip9" Chave="textoApresentacao" />
                            <CKEditor:CKEditorControl ID="txtTextoApresentacao" BasePath="/js/ckeditor/" runat="server"></CKEditor:CKEditorControl>
                        </div>


                        <asp:Panel runat="server" ID="pnlSolucaoObrigatoria">
                            <div class="form-group">
                                <asp:Label ID="Label20" runat="server" Text="Marcar como solução obrigatória?" AssociatedControlID="rblMarcarComoSolucaoObrigatoria" />
                                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip11" Chave="solucaoObrigatoria" />
                                <asp:RadioButtonList ID="rblMarcarComoSolucaoObrigatoria" runat="server" RepeatDirection="Horizontal" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="rblMarcarComoSolucaoObrigatoria_OnSelectedIndexChanged">
                                </asp:RadioButtonList>
                            </div>
                        </asp:Panel>

                        <div class="form-group clearfix" id="divNiveisOcupacionaisObrigatorios" runat="server"
                            visible="false">
                            <label id="titleNiveisOcupacionais" runat="server" clientidmode="Static">
                                Nível Ocupacional -
                            </label>
                            <asp:CheckBoxList ID="cblNivelOcupacionalObrigatorio" runat="server" RepeatDirection="Vertical"
                                RepeatLayout="UnorderedList">
                            </asp:CheckBoxList>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label15" runat="server" Text="Ativo?" AssociatedControlID="rblAtivo" />
                            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip15" Chave="ativo" />
                            <asp:RadioButtonList ID="rblAtivo" runat="server" RepeatDirection="Horizontal" CssClass="form-control" />
                        </div>
                        <asp:Panel runat="server" ID="visibilidadeIntegracaoSAS" Visible="false">
                            <div class="form-group">
                                <asp:Label ID="Label11" runat="server" Text="Auto Cadastrar no SAS?" AssociatedControlID="rblAtivo" />
                                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip18" Chave="ativo" />
                                <asp:RadioButtonList ID="rblIntergracaoSAS" runat="server" RepeatDirection="Horizontal" CssClass="form-control" />
                            </div>
                        </asp:Panel>

                        <div class="form-group" id="unidadeDemandante" runat="server">
                            <asp:Label ID="Label1" runat="server" Text="Unidade do Demandante" AssociatedControlID="listBoxesUnidadeDemandante" />
                            <uc1:ucHelperTooltip runat="server" ID="ucHelpTooltip" Chave="unidadeDemandante" />
                            <uc2:ucSeletorListBox runat="server" ID="listBoxesUnidadeDemandante" DescricaoDisponiveis="Unidades do Demandante" DescricaoSelecionados="Selecionados" MostrarSelecaoTodos="True" />
                        </div>



                        <div class="form-group" id="produtoSebrae" runat="server">
                            <asp:Label ID="lblProdutoSebrae" runat="server" AssociatedControlID="lblAreaTematica" Text="Produto Sebrae"></asp:Label>
                            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip19" Chave="produtoSebrae" />
                            <uc2:ucSeletorListBox runat="server" ID="listBoxesProdutoSebrae" DescricaoDisponiveis="Lista de Produtos Sebrae" DescricaoSelecionados="Selecionados" MostrarSelecaoTodos="True" />
                        </div>

                        <div runat="server" id="divSincronizarPortal" class="form-group">
                            <asp:CheckBox Text=" Sincronizar com o portal" ID="chkSincronizar" runat="server" Checked="true" Visible="false" />
                        </div>
                    </fieldset>
                </div>
            </div>
        </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            <a data-toggle="collapse" data-parent="#accordion" href="#collapsePreRequisito">Pré
                        Requisito </a>
        </div>
        <div id="collapsePreRequisito" class="panel-collapse collapse" runat="server" clientidmode="Static">
            <div class="panel-body">
                <div class="form-group">
                    <uc1:ucCategorias ID="ucCategoriasPreRequisito" runat="server" />
                </div>
                <div class="form-group">
                    <asp:Label ID="Label8" runat="server" Text="Solução Educacional" AssociatedControlID="txtSolucaoEducacional" />
                    <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip16" Chave="prerequisito" />
                    <asp:TextBox ID="txtSolucaoEducacional" runat="server" CssClass="form-control" ClientIDMode="Static" OnTextChanged="txtSolucaoEducacional_OnTextChanged"></asp:TextBox>
                </div>
                <asp:Panel ID="pnlSolucaoEducacionalPreRequisito" runat="server" Visible="false">
                    <div class="panel-body">
                        <h4>Itens Selecionados</h4>
                        <asp:GridView ID="gvSolucaoEducacionalPreRequisito" runat="server" CssClass="table col-sm-12"
                            GridLines="None" AutoGenerateColumns="false" EnableModelValidation="True" DataKeyNames="ID">
                            <Columns>
                                <asp:TemplateField HeaderText="Solução Educacional">
                                    <ItemTemplate>
                                        <%#Eval("Nome") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbSolucaoEducacionalPreRequisito" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
    <div class="panel panel-default" runat="server" visible="False">
        <div class="panel-heading">
            <a data-toggle="collapse" data-parent="#accordion" href="#collapse2">Tags </a>
        </div>
        <div id="collapse2" class="panel-collapse collapse">
            <div class="panel-body">
                <uc:Tags ID="ucTags1" runat="server" />
            </div>
        </div>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            <a data-toggle="collapse" data-parent="#accordion" href="#collapse3">Permissões
            </a>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip17" Chave="permissoes" />
        </div>
        <div id="collapse3" class="panel-collapse collapse">
            <div class="panel-body">
                <uc1:ucPermissoes ID="ucPermissoes1" runat="server" />
            </div>
        </div>
    </div>
    <br />
    <div>
        <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click"
            CssClass="btn btn-primary mostrarload" />
        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click"
            CssClass="btn btn-default" />
    </div>
    </div>

    <script type="text/javascript">
        function pad2(number) {
            return (number < 10 ? '0' : '') + number;
        }
        function validHour(value) {
            var comp = value.split(':');
            var h = parseInt(comp[0], 10);
            var m = parseInt(comp[1], 10);
            var s = parseInt(comp[2], 10);
            if (h > 23) h = 23;
            if (m > 59) m = 59;
            if (s > 59) s = 59;
            return pad2(h) + ':' + pad2(m) + ':' + pad2(s);
        }
        function validDate(value) {
            var comp = value.split('/');
            var d = parseInt(comp[0], 10);
            var m = parseInt(comp[1], 10);
            var y = parseInt(comp[2], 10);
            var date = new Date(y, m - 1, d);
            return {
                result: (date.getFullYear() === y && date.getMonth() + 1 === m && date.getDate() === d),
                value: value
            }
        }
        jQuery(document).ready(function () {

            $.markAll('titleNiveisOcupacionais', '<%= cblNivelOcupacionalObrigatorio.ClientID %>', 'Marcar todos', 'Desmarcar todos');

            $("#<%= txtDtInicio.ClientID %>").mask("99/99/9999 99:99:99", {
                autoclear: false
            }).on('blur', function () {
                var valor = $("#<%= txtDtInicio.ClientID %>").val();
                var dados = valor.split(' ');
                var data = validDate(dados[0]);
                if (!data.result) {
                    $("#<%= txtDtInicio.ClientID %>").val('');
                } else {
                    dados[1] = dados[1].replace(/_/g, "0");
                    dados[1] = validHour(dados[1]);
                    $("#<%= txtDtInicio.ClientID %>").val(dados.join(' '));
                }
            });

            $("#<%= txtDtFim.ClientID %>").mask("99/99/9999 99:99:99", {
                autoclear: false
            }).on('blur', function () {
                var valor = $("#<%= txtDtFim.ClientID %>").val();
                var dados = valor.split(' ');
                var data = validDate(dados[0]);
                if (!data.result) {
                    $("#<%= txtDtFim.ClientID %>").val('');
                } else {
                    dados[1] = dados[1].replace(/_/g, "9");
                    dados[1] = validHour(dados[1]);
                    $("#<%= txtDtFim.ClientID %>").val(dados.join(' '));
                }
            });

        });
    </script>
</asp:Content>
