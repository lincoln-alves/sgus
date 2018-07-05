<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="RelatorioUsuarioMatriculadoSEFormaAquisicao.aspx.cs" Inherits="Sebrae.Academico.WebForms.Relatorios.UsuarioMatriculadoSEFormaAquisicao.RelatorioUsuarioMatriculadoSEFormaAquisicao"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/UserControls/ucSeletorListBox.ascx" TagName="ucSeletorListBox"TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                            <asp:Label ID="label5" runat="server" AssociatedControlID="cbxFormaAquisicao" Text="Tipo de Forma de Aquisição" />
                            <asp:DropDownList ID="cbxFormaAquisicao" runat="server" CssClass="form-control" OnSelectedIndexChanged="cbxFormaAquisicao_OnSelectedIndexChanged" AutoPostBack="True" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblPrograma" runat="server" AssociatedControlID="cbxPrograma" Text="Forma de Aquisição" />
                            <asp:DropDownList ID="cbxPrograma" runat="server" CssClass="form-control" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label1" runat="server" AssociatedControlID="cbxStatusMatricula" Text="Status da Matrícula" />
                            <asp:DropDownList ID="cbxStatusMatricula" runat="server" CssClass="form-control" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label2" runat="server" AssociatedControlID="txtDataInicioTurma" Text="Data Início da Turma" />
                            <asp:TextBox ID="txtDataInicioTurma" runat="server" MaxLength="10" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label3" runat="server" AssociatedControlID="txtDataFinalTurma" Text="Data Final da Turma" />
                            <asp:TextBox ID="txtDataFinalTurma" runat="server" MaxLength="10" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label4" runat="server" Text="UF" AssociatedControlID="cbxUF" />
                            <asp:DropDownList ID="cbxUF" runat="server" CssClass="form-control">
                            </asp:DropDownList>
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
                <a data-toggle="collapse" data-parent="#gruporelatorio" href="#Campos">Campos a Serem
                    Exibidos </a>
            </div>
            <div id="Campos" class="accordion-body collapse">
                <div class="accordion-inner">
                    <fieldset>
                        <div class="form-group">
                            <asp:CheckBoxList ID="chkListaCamposVisiveis" runat="server" ClientIDMode="Static"
                                RepeatDirection="Vertical" RepeatLayout="UnorderedList">
                                <asp:ListItem Selected="True" Value="FormaAquisicao">Forma de Aquisição</asp:ListItem>
                                <asp:ListItem Selected="True" Value="SolucaoEducacional">Solução Educacional</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Oferta">Oferta</asp:ListItem>
                                <asp:ListItem Selected="True" Value="DataInicio">Data Inicio</asp:ListItem>
                                <asp:ListItem Selected="True" Value="DataFim">Data Fim</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Nome">Nome</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Email">Email</asp:ListItem>
                                <asp:ListItem Selected="True" Value="CPF">CPF</asp:ListItem>
                                <asp:ListItem Selected="True" Value="NivelOcupacional">Nível Ocupacional</asp:ListItem>
                                <asp:ListItem Selected="True" Value="UF">UF</asp:ListItem>
                                <asp:ListItem Selected="True" Value="StatusMatricula">Status da Matrícula</asp:ListItem>
                                <asp:ListItem Selected="True" Value="UFResponsavel">UF Responsável</asp:ListItem>
                            </asp:CheckBoxList>
                        </div>
                    </fieldset>
                </div>
            </div>
        </div>
    </div>
    <br />
    <asp:Button ID="btnPesquisar" runat="server" CssClass="btn btn-primary mostrarload"
        OnClick="btnPesquisar_Click" Text="Consultar" />
    <hr />
    <asp:GridView ID="dgRelatorio" runat="server" AutoGenerateColumns="False" AllowSorting="True"
        CssClass="table col-sm-12" GridLines="None" OnSorting="dgRelatorio_Sorting" AllowPaging="True"
        OnPageIndexChanging="dgRelatorio_PageIndexChanging" PageSize="100">
        <Columns>
            <asp:BoundField DataField="FormaAquisicao" HeaderText="Forma de Aquisição" SortExpression="FormaAquisicao" />
            <asp:BoundField DataField="SolucaoEducacional" HeaderText="Solução Educacional" SortExpression="SolucaoEducacional" />
            <asp:BoundField DataField="Oferta" HeaderText="Oferta" SortExpression="Oferta" />
            <asp:BoundField DataField="DataInicio" HeaderText="Data Inicio" SortExpression="DataInicio" />
            <asp:BoundField DataField="DataFim" HeaderText="Data Fim" SortExpression="DataFim" />
            <asp:BoundField DataField="Nome" HeaderText="Nome" SortExpression="Nome" />
            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
            <asp:BoundField DataField="CPF" HeaderText="CPF" SortExpression="CPF" />
            <asp:BoundField DataField="NivelOcupacional" HeaderText="Nível Ocupacional" SortExpression="NivelOcupacional" />
            <asp:BoundField DataField="UF" HeaderText="UF" SortExpression="UF" />
            <asp:BoundField DataField="StatusMatricula" HeaderText="Status da Matricula" SortExpression="StatusMatricula" />
            <asp:BoundField DataField="UFResponsavel" HeaderText="UF Responsável" SortExpression="UFResponsavel" />
        </Columns>
        <EmptyDataTemplate>
            <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
        </EmptyDataTemplate>
        <PagerSettings Mode="NumericFirstLast" />
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
            $("#<%= txtDataInicioTurma.ClientID %>").mask("99/99/9999");
            $("#<%= txtDataFinalTurma.ClientID %>").mask("99/99/9999");
        });
    </script>
</asp:Content>
