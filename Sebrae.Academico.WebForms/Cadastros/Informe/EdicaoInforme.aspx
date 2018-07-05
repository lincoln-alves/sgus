<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="EdicaoInforme.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.Informe.EdicaoInforme" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <div class="row">
            <div class="col-xs-12 col-md-6">
                <div class="form-group">
                    <asp:Label ID="Label1" runat="server" Text="Número *" AssociatedControlID="txtNumero"></asp:Label>
                    <asp:TextBox ID="txtNumero" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="col-xs-12 col-md-6">
                <div class="form-group">
                    <asp:Label ID="Label2" runat="server" Text="Mês e ano *" AssociatedControlID="txtMesAno"></asp:Label>
                    <asp:TextBox ID="txtMesAno" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>

        <div class="panel panel-default">
            <div class="panel-body">
                <div class="form-group">
                    <asp:Label ID="Label7" runat="server" Text="Solução Educacional" AssociatedControlID="txtSolucao" />
                    <asp:TextBox ID="txtSolucao" runat="server" ClientIDMode="Static" OnTextChanged="txtSolucao_OnTextChanged" />
                </div>
                <div class="form-group">
                    <asp:Label ID="Label8" runat="server" Text="Oferta" AssociatedControlID="txtOferta" />
                    <asp:TextBox ID="txtOferta" runat="server" ClientIDMode="Static" OnTextChanged="txtOferta_OnTextChanged" data-mensagemVazia="Nenhuma informação disponivel" />
                </div>
                <div class="form-group">
                    <asp:Label ID="Label9" runat="server" Text="Turma" AssociatedControlID="txtTurma" />
                    <asp:TextBox ID="txtTurma" runat="server" ClientIDMode="Static" data-mensagemVazia="Nenhuma informação disponivel" />
                </div>
                <div class="form-group">
                    <button class="btn btn-default mostrarload" id="btnAdicionarTurma" runat="server" onserverclick="btnAdicionarTurma_OnClick">
                        <span class="glyphicon glyphicon-plus"></span>&nbsp;Adicionar mais <span runat="server" class="notification-blob" id="contadorTurmas">0</span>
                    </button>
                </div>
            </div>
            <div class="panel-body">

                <h4 runat="server" id="TituloTurmas" visible="False">Turmas selecionadas para envio do informe
                </h4>

                <asp:GridView ID="gvTurmas" runat="server" CssClass="table" GridLines="None" AutoGenerateColumns="false" DataKeyNames="ID" OnRowDataBound="gvTurmas_OnRowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="SE">
                            <ItemTemplate>
                                <%#Eval("Oferta.SolucaoEducacional.Nome") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Oferta">
                            <ItemTemplate>
                                <%#Eval("Oferta.Nome") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Turma">
                            <ItemTemplate>
                                <%#Eval("Nome") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemStyle Width="50px" HorizontalAlign="center" />
                            <ItemTemplate>
                                <button runat="server" turmaid='<%# Eval("ID")%>' class="mostrarload btn btn-sm btn-danger" onserverclick="RemoverTurma_OnServerClick">
                                    Remover
                                </button>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <itemtemplate>Nenhuma turma selecionada para envio.</itemtemplate>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </div>
        <hr />
        <div class="form-group">
            <div class="row">
                <div class="col-xs-12 col-sm-6 col-md-3">
                    <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click" CssClass="btn btn-primary btn-block mostrarload" />
                </div>
                <div class="col-xs-12 col-sm-6 col-md-3">
                    <a href="VisualizarInforme.aspx?Id=<%= Request["Id"]%>" class="btn btn-primary btn-block btn-default <%= (Request["Id"] == null ? "disabled" : "")%>" target="_blank" style="color: white;">
                        Visualizar
                    </a>
                </div>
                <div class="col-xs-12 col-sm-6 col-md-3">
                    <asp:Button ID="btnEnviar" Enabled="False" runat="server" Text="Enviar" OnClick="btnEnviar_OnClick" CssClass="btn btn-primary btn-block mostrarload" />
                </div>
                <div class="col-xs-12 col-sm-6 col-md-3">
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" CssClass="btn btn-default btn-block mostrarload" />
                </div>
            </div>
        </div>


        <script>
            // Encapsular jQuery, do jeito que o Senpai Bruno ensinou.
            (function ($){
                function validarMesData(value) {
                    var comp = value.split('/');

                    var m = parseInt(comp[0], 10);
                    var y = parseInt(comp[1], 10);

                    var date = new Date(y, m - 1, 1);

                    return {
                        result: (date.getFullYear() === y && date.getMonth() + 1 === m),
                        value: value
                    }
                }

                // Depois de carregar o DOM.
                $(function() {
                    $("#<%= txtMesAno.ClientID %>").mask("99/9999", {
                        autoclear: false
                    }).on('blur', function () {
                        var valor = $("#<%= txtMesAno.ClientID %>").val();

                        var data = validarMesData(valor);

                        if (!data.result) {
                            $("#<%= txtMesAno.ClientID %>").val('');
                        }
                    });
                });
                
                // Autocomplete starts here.

                var solucaoJsonList = <%= ViewState["_Solucao"] ?? "''" %>;
                var ofertaJsonList = <%= ViewState["_Oferta"] ?? "''" %>;
                var turmaJsonList = <%= ViewState["_Turma"] ?? "''" %>;

                AutoCompleteDefine(solucaoJsonList, '#txtSolucao', true, false, true);
                AutoCompleteDefine(ofertaJsonList, '#txtOferta', true, false, true);
                AutoCompleteDefine(turmaJsonList, '#txtTurma', false, false, true);

            } (window.jQuery)); // Passa jQuery para definir que o $ é realmente do jQuery.
        </script>
    </fieldset>
</asp:Content>
