<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="MonitoramentoTurma.aspx.cs" Inherits="Sebrae.Academico.WebForms.Relatorios.MonitoramentoTurma.MonitoramentoTurma" %>

<%@ Register Src="~/UserControls/ucCategorias.ascx" TagName="ucCategorias" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucSeletorListBox.ascx" TagName="ucSeletorListBox"TagPrefix="uc2" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="scriptManager" runat="server"></asp:ScriptManager>

    <style>
        input.form-control {
            width: 100% !important;
        }
    </style>
     <script type="text/javascript">
         var _preloadedListSE = <%= ViewState["_SE"] ?? "''" %>;
         if(_preloadedListSE != '')AutoCompleteDefine(_preloadedListSE, '#txtSolucaoEducacional', true, true, false);
        
         var _preloadedListOferta = <%= ViewState["_Oferta"] ?? "''" %>;
         if(_preloadedListOferta != '')AutoCompleteDefine(_preloadedListOferta, '#txtOferta', true, false, true);

         var _preloadedListTurma = <%= ViewState["_Turma"] ?? "''" %>;
         if(_preloadedListTurma != '')AutoCompleteDefine(_preloadedListTurma, '#txtTurma', false, false, true);

     </script>
    <div class="">
        <div class="panel panel-default">
            <div class="panel-heading">
                <a data-toggle="collapse" data-target="#collapse-filtros" href="#">
                    <span>Filtros</span>
                </a>
            </div>
            <div class="panel-body" id="collapse-filtros">
                <div class="form-group">
                    <asp:Label ID="Label6" runat="server" Text="CATEGORIAS" data-help="Filtra turmas por categorias de conteúdo"></asp:Label>
                    <uc1:ucCategorias runat="server" ID="ucCategoriasConteudo" CssClass="form-control" />
                </div>

                <div class="form-group">
                    <asp:Label ID="Label1" runat="server" Text="DATA DE INÍCIO DA TURMA" AssociatedControlID="txtDataInicialTurma" data-help="<%$ Resources:Resource, dataFim%>" />
                    <asp:TextBox runat="server" ID="txtDataInicialTurma" CssClass="form-control"></asp:TextBox>
                </div>

                <div class="form-group">
                    <asp:Label ID="Label2" runat="server" Text="DATA FIM DA TURMA" AssociatedControlID="txtDataFinalTurma" data-help="<%$ Resources:Resource, dataFim%>" />
                    <asp:TextBox runat="server" ID="txtDataFinalTurma" CssClass="form-control"></asp:TextBox>
                </div>

                <div class="form-group">
                    <asp:Label AssociatedControlID="chkStatus" Text="STATUS" runat="server" data-help="Filtro de turmas por status"></asp:Label>
                    <br /><label id="titleCampos" runat="server" clientidmode="Static"></label>
                    <asp:CheckBoxList runat="server" ID="chkStatus" />
                </div>

                <div class="form-group" id="divUfResposanvel" runat="server">
                    <asp:Label ID="lblUfResposanvel" runat="server" Text="UF responsável" AssociatedControlID="ListBoxesUFResponsavel" />
                    <uc2:ucSeletorListBox runat="server" ID="ListBoxesUFResponsavel" />
                </div>
            </div>
        </div>

        <br />
        <asp:Button runat="server" ID="btnFiltrar" OnClick="btnFiltrar_OnClick" Text="Consultar" CssClass="btn btn-primary mostrarload" />
        <hr />

        <asp:Panel runat="server" Visible="False" ID="pnl1">
            <div class="row">
                <asp:GridView runat="server" ID="grdTotalMatriculas" AutoGenerateColumns="False" CssClass="table col-sm-12">
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
                    <EmptyDataTemplate>
            <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
        </EmptyDataTemplate>
                </asp:GridView>
            </div>
            <div class="row">
                <asp:GridView runat="server" ID="grdTurma" AutoGenerateColumns="False" CssClass="table col-sm-12" GridLines="None" AllowPaging="True" PageSize="30" OnPageIndexChanging="grdTurma_PageIndexChanging">
                    <Columns>
                        <asp:BoundField HeaderText="Solução Educacional" DataField="SolucaoEducacional" />
                        <asp:BoundField HeaderText="Nome da Turma" DataField="NomeTurma" />
                        <asp:BoundField HeaderText="Status" DataField="StatusFormatado" />
                        <asp:TemplateField HeaderText="Total de Alunos">
                            <ItemTemplate>
                                <span data-turma="<%#Eval("ID_Turma") %>" class="qntAlunos" /><%#Eval("TotalMatriculados") %></span>
                            </ItemTemplate>
                            <ItemStyle Width="200px" />
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
            <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
        </EmptyDataTemplate>
                </asp:GridView>

                <fieldset runat="server" id="componenteGeracaoRelatorio" clientidmode="Static">
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
        </asp:Panel>
    </div>
    <script>
        (function() {
            $(document).ready(function() {
                $("#<%= txtDataInicialTurma.ClientID %>").mask("99/99/9999", {
                    autoclear: false
                });
                $("#<%= txtDataFinalTurma.ClientID %>").mask("99/99/9999", {
                    autoclear: false
                });

                $.markAll('titleCampos', '<%= chkStatus.ClientID %>', 'Marcar todos', 'Desmarcar todos');
            });
        })(jQuery);
    </script>
</asp:Content>
