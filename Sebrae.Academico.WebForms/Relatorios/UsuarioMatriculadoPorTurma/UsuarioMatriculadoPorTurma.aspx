<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="UsuarioMatriculadoPorTurma.aspx.cs" Inherits="Sebrae.Academico.WebForms.Relatorios.UsuarioMatriculadoPorTurma"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/UserControls/ucTotalizadorRelatorio.ascx" TagPrefix="uc" TagName="ucTotalizadorRelatorio" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var _preloadedList = <%= ViewState["_SE"] ?? "''" %>;
        AutoCompleteDefine(_preloadedList, '#txtSolucaoEducacional', true, true, false);

        var _preloadedListOferta = <%= ViewState["_Oferta"] ?? "''" %>;
        AutoCompleteDefine(_preloadedListOferta, '#txtOferta', true, true, false);

        var _preloadedListTurma = <%= ViewState["_Turma"] ?? "''" %>;
        AutoCompleteDefine(_preloadedListTurma, '#txtTurma', false, true, false);
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
                            <asp:Label ID="lblFormaAquisicao" runat="server" Text="Forma de Aquisição" AssociatedControlID="cbxFormaAquisicao" />
                            <asp:DropDownList ID="cbxFormaAquisicao" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbxFormaAquisicao_SelectedIndexChanged" CssClass="form-control" ClientIDMode="Static" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblSolucaoEducacional" runat="server" Text="Solução Educacional"
                                AssociatedControlID="txtSolucaoEducacional" />
                            <asp:TextBox ID="txtSolucaoEducacional" runat="server" ClientIDMode="Static" OnTextChanged="txtSolucaoEducacional_OnValueChanged" data-mensagemVazia="Selecione uma forma de aquisição" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblOferta" runat="server" Text="Oferta" AssociatedControlID="txtOferta" />
                            <asp:TextBox ID="txtOferta" runat="server" ClientIDMode="Static" OnTextChanged="txtOferta_OnTextChanged" data-mensagemVazia="Selecione uma Solução Educacional" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblTurma" runat="server" Text="Turma" AssociatedControlID="txtTurma" />
                            <asp:TextBox ID="txtTurma" runat="server" ClientIDMode="Static" data-mensagemVazia="Selecione uma Oferta" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label2" runat="server" Text="UF" AssociatedControlID="cbxUF" />
                            <asp:DropDownList ID="cbxUF" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </fieldset>
                </div>
            </asp:Panel>
        </div>
        <div class="panel panel-default">
            <div class="panel-heading">
                <a data-toggle="collapse" data-parent="#gruporelatorio" href="#Campos">Campos a Serem
                    Exibidos </a>
            </div>
            <div id="Campos" class="accordion-body collapse">
                <div class="accordion-inner">
                    <fieldset>
                        <div class="form-group">
                            <asp:CheckBoxList ID="chkListaCamposVisiveis" runat="server" ClientIDMode="Static"
                                RepeatDirection="Vertical" RepeatLayout="UnorderedList">
                                <asp:ListItem Selected="True" Value="Nome">Nome</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Email">Email</asp:ListItem>
                                <asp:ListItem Selected="True" Value="CPF">CPF</asp:ListItem>
                                <asp:ListItem Selected="True" Value="UF">UF</asp:ListItem>
                                <asp:ListItem Selected="True" Value="NivelOcupacional">Nível Ocupacional</asp:ListItem>
                                <asp:ListItem Selected="True" Value="SolucaoEducacional">Solução Educacional</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Oferta">Oferta</asp:ListItem>
                                <asp:ListItem Selected="True" Value="CargaHoraria">Garga Horária</asp:ListItem>
                                <asp:ListItem Selected="True" Value="TemMaterial">Tem Material?</asp:ListItem>
                                <asp:ListItem Selected="True" Value="FormaAquisicao">Forma de Aquisição</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Turma">Turma</asp:ListItem>
                                <asp:ListItem Selected="True" Value="TipoTutoria">Tipo de Tutoria</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Professor">Professor</asp:ListItem>
                                <asp:ListItem Selected="True" Value="DataInicio">Data Inicio</asp:ListItem>
                                <asp:ListItem Selected="True" Value="DataFinal">Data Final</asp:ListItem>
                                <asp:ListItem Selected="True" Value="StatusMatricula">Situação</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Fornecedor">Fornecedor</asp:ListItem>
                                <asp:ListItem Selected="True" Value="UFResponsavel">UF Responsável</asp:ListItem>
                            </asp:CheckBoxList>
                        </div>
                    </fieldset>
                </div>
            </div>
        </div>
    </div>
    <br />
    <asp:Button ID="btnPesquisar" runat="server" Text="Consultar" CssClass="btn btn-primary mostrarload"
        OnClick="btnPesquisar_Click" />
    <hr />

    <uc:ucTotalizadorRelatorio runat="server" ID="ucTotalizadorRelatorio" />

    <asp:GridView ID="dgRelatorio" runat="server" AutoGenerateColumns="False" AllowSorting="True"
        CssClass="table col-sm-12" GridLines="None" OnSorting="dgRelatorio_Sorting" AllowPaging="True"
        OnPageIndexChanging="dgRelatorio_PageIndexChanging" PageSize="100">
        <Columns>
            <asp:BoundField DataField="Nome" HeaderText="Nome" SortExpression="Nome" />
            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
            <asp:BoundField DataField="CPF" HeaderText="CPF" SortExpression="CPF" />
            <asp:BoundField DataField="UF" HeaderText="UF" SortExpression="UF" />
            <asp:BoundField DataField="NivelOcupacional" HeaderText="Nível Ocupacional" SortExpression="NiveOcupacional" />
            <asp:BoundField DataField="SolucaoEducacional" HeaderText="Solução Educacional" SortExpression="SolucaoEducacional" />
            <asp:BoundField DataField="Oferta" HeaderText="Oferta" SortExpression="Oferta" />
            <asp:BoundField DataField="CargaHoraria" HeaderText="Carga Horaria" SortExpression="CargaHoraria" />
            <asp:CheckBoxField DataField="TemMaterial" HeaderText="Tem Material?" SortExpression="TemMaterial" />
            <asp:BoundField DataField="FormaAquisicao" HeaderText="Forma de Aquisição" SortExpression="FormaAquisicao" />
            <asp:BoundField DataField="Turma" HeaderText="Turma" SortExpression="Turma" />
            <asp:BoundField DataField="TipoTutoria" HeaderText="Tipo de Tutoria" SortExpression="TipoTutoria" />
            <asp:BoundField DataField="Professor" HeaderText="Professor" SortExpression="Professor" />
            <asp:BoundField DataField="DataInicio" HeaderText="Data Inicio" SortExpression="DataInicio"
                DataFormatString="{0:dd/MM/yyyy}" />
            <asp:BoundField DataField="DataFinal" HeaderText="Data Final" SortExpression="DataFinal"
                DataFormatString="{0:dd/MM/yyyy}" />
            <asp:BoundField DataField="StatusMatricula" HeaderText="Situação" SortExpression="StatusMatricula" />
            <asp:BoundField DataField="Fornecedor" HeaderText="Fornecedor" SortExpression="Fornecedor" />
            <asp:BoundField DataField="UFResponsavel" HeaderText="UF Responsável" SortExpression="UFResponsavel" />
        </Columns>
        <EmptyDataTemplate>
            <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
        </EmptyDataTemplate>
        <PagerSettings Mode="NumericFirstLast" />
    </asp:GridView>
    <fieldset runat="server" id="componenteGeracaoRelatorio" clientidmode="Static" visible="false">
        <div class="form-group">
            <uc:SaidaRel ID="ucFormatoSaidaRelatorio" runat="server" />
        </div>
        <div class="form-group">
            <asp:Button ID="btnGerarRelatorio" runat="server" Text="Gerar Relatório" CssClass="btn btn-primary"
                OnClick="btnGerarRelatorio_Click" />
        </div>
    </fieldset>
</asp:Content>
