<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="InscricoesPorStatusENivel.aspx.cs" Inherits="Sebrae.Academico.WebForms.Relatorios.InscricoesPorStatusENivel.InscricoesPorStatusENivel" %>

<%@ Register Src="~/UserControls/ucSeletorCheckboxes.ascx" TagName="ucCheckboxes" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucSeletorListBox.ascx" TagName="ucSeletorListBox"TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var _preloadedList = <%= ViewState["_SE"] ?? "''" %>;
        AutoCompleteDefine(_preloadedList, '#txtSolucaoEducacional', false, true, false);
    </script>
    <div class="panel-group" id="gruporelatorio">
        <div class="panel panel-default">
            <div class="panel-heading">
                <a data-toggle="collapse" data-parent="#gruporelatorio" href="#Filtros">Filtros
                </a>
            </div>
            <asp:Panel ID="Filtros" runat="server" ClientIDMode="Static" CssClass="panel-collapse collapse in">
                <div class="panel-body">
                    <fieldset>
                        <div class="form-group clearfix">
                            <uc1:ucCheckboxes runat="server" ID="CheckboxesStatus" Descricao="STATUS" />
                        </div>
                        <div class="form-group clearfix">
                            <uc1:ucCheckboxes runat="server" ID="CheckboxesNiveis" Descricao="NÍVEIS OCUPACIONAIS" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblUf" runat="server" Text="UF" AssociatedControlID="cbxUf" />
                            <asp:DropDownList ID="cbxUf" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label1" runat="server" Text="Solução Educacional" AssociatedControlID="txtSolucaoEducacional" />
                            <asp:TextBox ID="txtSolucaoEducacional" runat="server" ClientIDMode="Static" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label5" runat="server" Text="Data Início Matrícula" AssociatedControlID="txtDataInicio" />
                            <asp:TextBox ID="txtDataInicio" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label6" runat="server" Text="Data Fim Matrícula" AssociatedControlID="txtDataFim" />
                            <asp:TextBox ID="txtDataFim" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group" id="divUfResposanvel" runat="server">
                            <asp:Label ID="lblUfResposanvel" runat="server" Text="UF responsável" AssociatedControlID="ListBoxesUFResponsavel" />
                            <uc2:ucSeletorListBox runat="server" ID="ListBoxesUFResponsavel" />
                        </div>
                    </fieldset>
                </div>
            </asp:Panel>
        </div>
    </div>

    <br />
    <asp:Button ID="btnPesquisar" runat="server" Text="Consultar" OnClick="btnPesquisar_Click"
        CssClass="btn btn-primary mostrarload" />
    <hr />

    <asp:Label ID="lblQuantidadeEncontrada" runat="server" CssClass="form-control"></asp:Label>
    <br />

    <asp:GridView ID="dgRelatorio" runat="server" AutoGenerateColumns="False" AllowSorting="True"
        CssClass="table col-sm-12" GridLines="None" 
        OnSorting="dgRelatorio_Sorting" AllowPaging="True"
        OnPageIndexChanging="dgRelatorio_PageIndexChanging" PageSize="100" 
        ShowFooter="true" onrowdatabound="dgRelatorio_RowDataBound">
        <Columns>

            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
            <asp:BoundField DataField="AD" HeaderText="AD" SortExpression="AD" />
            <asp:BoundField DataField="ALI" HeaderText="ALI" SortExpression="ALI" />
            <asp:BoundField DataField="APA" HeaderText="APA" SortExpression="APA" />
            <asp:BoundField DataField="AnalistaTecnicoI" HeaderText="Analista Técnico I" SortExpression="AnalistaTecnicoI" />
            <asp:BoundField DataField="AnalistaTecnicoII" HeaderText="Analista Técnico II" SortExpression="AnalistaTecnicoII" />
            <asp:BoundField DataField="AnalistaTecnicoIII" HeaderText="Analista Técnico III" SortExpression="AnalistaTecnicoIII" />
            <asp:BoundField DataField="AOE" HeaderText="AOE" SortExpression="AOE" />
            <asp:BoundField DataField="Assessor" HeaderText="Assessor" SortExpression="Assessor" />
            <asp:BoundField DataField="AssistenteI" HeaderText="Assistente I" SortExpression="AssistenteI" />
            <asp:BoundField DataField="AssistenteII" HeaderText="Assistente II" SortExpression="AssistenteII" />
            <asp:BoundField DataField="AssistenteIII" HeaderText="Assistente III" SortExpression="AssistenteIII" />
            <asp:BoundField DataField="Conselheiro" HeaderText="Conselheiro" SortExpression="Conselheiro" />
            <asp:BoundField DataField="Credenciado" HeaderText="Credenciado" SortExpression="Credenciado" />
            <asp:BoundField DataField="Dirigente" HeaderText="Dirigente" SortExpression="Dirigente" />
            <asp:BoundField DataField="Estagiario" HeaderText="Estagiário" SortExpression="Estagiario" />
            <asp:BoundField DataField="Gerente" HeaderText="Gerente" SortExpression="Gerente" />
            <asp:BoundField DataField="MenorAprendiz" HeaderText="Menor Aprendiz" SortExpression="MenorAprendiz" />
            <asp:BoundField DataField="OrientadorALI" HeaderText="Orientador ALI" SortExpression="OrientadorALI" />
            <asp:BoundField DataField="Parceiro" HeaderText="Parceiro" SortExpression="Parceiro" />
            <asp:BoundField DataField="PreALI" HeaderText="Pré ALI" SortExpression="PreALI" />
            <asp:BoundField DataField="FuncionarioTemporario" HeaderText="Funcionário Temporário" SortExpression="Spot" />
            <asp:BoundField DataField="Trainee" HeaderText="Trainee" SortExpression="Trainee" />
            <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" />


            <%--<asp:BoundField DataField="UF" HeaderText="UF" SortExpression="UF" HeaderStyle-Width="125px" />
            <asp:BoundField DataField="NivelOcupacional" HeaderText="Nível Ocupacional" SortExpression="NivelOcupacional" HeaderStyle-Width="150px" />
            <asp:BoundField DataField="SolucaoEducacional" HeaderText="Solução Educacional" SortExpression="SolucaoEducacional" />
            <asp:BoundField DataField="TotalHoras" HeaderText="Total de Horas" SortExpression="TotalHoras" HeaderStyle-Width="120px" />--%>
        </Columns>
        <EmptyDataTemplate>
            <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
        </EmptyDataTemplate>
        <PagerSettings Mode="NumericFirstLast" PageButtonCount="20" Position="TopAndBottom" />
    </asp:GridView>

    <fieldset runat="server" id="componenteGeracaoRelatorio" clientidmode="Static" visible="false">
        <hr/>
        <div class="form-group">
            <uc:SaidaRel ID="ucFormatoSaidaRelatorio" runat="server" />
        </div>
        <div class="form-group">
            <asp:Button ID="btnGerarRelatorio" runat="server" Text="Gerar Relatório" OnClick="btnGerarRelatorio_Click"
                CssClass="btn btn-primary" />
        </div>
    </fieldset>




    <script type="text/javascript">
        jQuery(function ($) {
            $("#<%= txtDataInicio.ClientID %>").mask("99/99/9999");
            $("#<%= txtDataFim.ClientID %>").mask("99/99/9999");
        });
    </script>

</asp:Content>
