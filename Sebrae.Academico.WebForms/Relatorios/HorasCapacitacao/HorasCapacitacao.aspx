<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" 
    CodeBehind="HorasCapacitacao.aspx.cs" Inherits="Sebrae.Academico.WebForms.Relatorios.HorasCapacitacao.HorasCapacitacao"
    MaintainScrollPositionOnPostback="true" %>

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
                        <div class="form-group">
                            <asp:Label ID="lblPerfil" runat="server" Text="Perfil" AssociatedControlID="cbxPerfil" />
                            <asp:DropDownList ID="cbxPerfil" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblUf" runat="server" Text="UF" AssociatedControlID="cbxUf" />
                            <asp:DropDownList ID="cbxUf" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label2" runat="server" Text="Nível Ocupacional" AssociatedControlID="cbxNivelOcupacional" />
                            <asp:DropDownList ID="cbxNivelOcupacional" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label3" runat="server" Text="Forma de Aquisição" AssociatedControlID="cbxFormaAquisicao" />
                            <asp:DropDownList ID="cbxFormaAquisicao" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbxFormaAquisicao_SelectedIndexChanged"
                                CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label1" runat="server" Text="Solução Educacional" AssociatedControlID="txtSolucaoEducacional" />
                            <asp:TextBox ID="txtSolucaoEducacional" runat="server" ClientIDMode="Static" data-mensagemVazia="Selecione uma forma de aquisição" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label4" runat="server" Text="Status da Matrícula" AssociatedControlID="cbxStatusMatricula" />
                            <asp:DropDownList ID="cbxStatusMatricula" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label5" runat="server" Text="Data Início" AssociatedControlID="txtDataInicio" />
                            <asp:TextBox ID="txtDataInicio" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label6" runat="server" Text="Data Fim" AssociatedControlID="txtDataFim" />
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
                                <asp:ListItem Selected="True">UF</asp:ListItem>
                                <asp:ListItem Selected="True" Value="NivelOcupacional">Nível Ocupacional</asp:ListItem>
                                <asp:ListItem Selected="True" Value="SolucaoEducacional">Solução Educacional</asp:ListItem>
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

    <asp:Label ID="lblQuantidadeEncontrada" runat="server" CssClass="form-control"></asp:Label>
    <br />

    <asp:GridView ID="dgRelatorio" runat="server" AutoGenerateColumns="False" AllowSorting="True"
        CssClass="table col-sm-12" GridLines="None" OnSorting="dgRelatorio_Sorting" AllowPaging="True"
        OnPageIndexChanging="dgRelatorio_PageIndexChanging" PageSize="100">
        <Columns>
            <asp:BoundField DataField="UF" HeaderText="UF" SortExpression="UF" HeaderStyle-Width="125px" />
            <asp:BoundField DataField="NivelOcupacional" HeaderText="Nível Ocupacional" SortExpression="NivelOcupacional" HeaderStyle-Width="150px" />
            <asp:BoundField DataField="SolucaoEducacional" HeaderText="Solução Educacional" SortExpression="SolucaoEducacional" />
            <asp:BoundField DataField="TotalHoras" HeaderText="Total de Horas" SortExpression="TotalHoras" HeaderStyle-Width="120px" />
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
