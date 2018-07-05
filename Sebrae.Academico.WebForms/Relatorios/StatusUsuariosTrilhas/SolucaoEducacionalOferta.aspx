<%@ Page Title="" Language="C#" MasterPageFile="~/Relatorio.Master" AutoEventWireup="true"
    CodeBehind="SolucaoEducacionalOferta.aspx.cs" Inherits="Sebrae.Academico.WebForms.Relatorios.SolucaoEducacionalOferta"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        AutocompleteCombobox("cbxSolucaoEducacional", true);
    </script>
    <h3>
        Solução Educacional por Oferta</h3>
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
                            <asp:Label ID="lblNome" runat="server" Text="Tipo de Oferta" AssociatedControlID="cbxTipoOferta" />
                            <asp:DropDownList ID="cbxTipoOferta" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label1" runat="server" Text="Forma de Aquisição" AssociatedControlID="cbxFormaAquisicao" />
                            <asp:DropDownList ID="cbxFormaAquisicao" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbxFormaAquisicao_SelectedIndexChanged"
                                CssClass="form-control" >
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label2" runat="server" Text="Solução Educacional" AssociatedControlID="cbxSolucaoEducacional" />
                            <asp:DropDownList ID="cbxSolucaoEducacional" runat="server" ClientIDMode="Static" AutoPostBack="true" CssClass="form-control">
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
                            <asp:CheckBoxList ID="chkListaCamposVisiveis" runat="server" RepeatDirection="Vertical"
                                RepeatLayout="UnorderedList" ClientIDMode="Static">
                                <asp:ListItem Selected="True" Value="SolucaoEducacional">Solução Educacional</asp:ListItem>
                                <asp:ListItem Selected="True">Oferta</asp:ListItem>
                                <asp:ListItem Selected="True" Value="DataInicio">Data Inicio</asp:ListItem>
                                <asp:ListItem Selected="True" Value="DataFim">Data Fim</asp:ListItem>
                                <asp:ListItem Selected="True" Value="TipoOferta">Tipo de Oferta</asp:ListItem>
                                <asp:ListItem Selected="True" Value="DataInicioInscricoes">Data Inicio de Inscrições</asp:ListItem>
                                <asp:ListItem Selected="True" Value="DataFimInscricoes">Data Fim de Inscrições</asp:ListItem>
                                <asp:ListItem Selected="True" Value="MaxInscricoes">Máx. Inscrições</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Inscritos">Inscritos</asp:ListItem>
                                <asp:ListItem Selected="True" Value="FilaEspera">Fila de Espera</asp:ListItem>
                                <asp:ListItem Selected="True">Solicitado</asp:ListItem>
                            </asp:CheckBoxList>
                        </div>
                    </fieldset>
                </div>
            </div>
        </div>
    </div>
    <br />
    <asp:Button ID="btnConsultar" runat="server" Text="Consultar" OnClick="btnConsultar_Click"
        CssClass="btn btn-primary mostrarload" />
    <hr />
    <asp:GridView ID="dgRelatorio" runat="server" AutoGenerateColumns="False" AllowSorting="True"
        CssClass="table col-sm-12" GridLines="None" OnSorting="dgRelatorio_Sorting" AllowPaging="True"
        OnPageIndexChanging="dgRelatorio_PageIndexChanging" PageSize="100">
        <Columns>
            <asp:BoundField DataField="SolucaoEducacional" HeaderText="Solução Educacional" SortExpression="SolucaoEducacional" />
            <asp:BoundField DataField="Oferta" HeaderText="Oferta" SortExpression="Oferta" />
            <asp:BoundField DataField="DataInicio" HeaderText="Data Inicio" SortExpression="DataInicio"
                DataFormatString="{0:dd/MM/yyyy}" />
            <asp:BoundField DataField="DataFim" HeaderText="Data Fim" SortExpression="DataFim"
                DataFormatString="{0:dd/MM/yyyy}" />
            <asp:BoundField DataField="TipoOferta" HeaderText="Tipo de Oferta" SortExpression="TipoOferta" />
            <asp:BoundField DataField="DataInicioInscricoes" HeaderText="Data Inicio de Inscricoes"
                SortExpression="DataInicioInscricoes" DataFormatString="{0:dd/MM/yyyy}" />
            <asp:BoundField DataField="DataFimInscricoes" HeaderText="Data Fim de Inscricoes"
                SortExpression="DataFimInscricoes" DataFormatString="{0:dd/MM/yyyy}" />
            <asp:BoundField DataField="MaxInscricoes" HeaderText="Max. Inscrições" SortExpression="MaxInscricoes" />
            <asp:BoundField DataField="Inscritos" HeaderText="Inscritos" SortExpression="Inscritos" />
            <asp:BoundField DataField="FilaEspera" HeaderText="Fila de Espera" SortExpression="FilaEspera" />
            <asp:BoundField DataField="Solicitado" HeaderText="Solicitado" SortExpression="Solicitado" />
        </Columns>
        <EmptyDataTemplate>
            <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
        </EmptyDataTemplate>
        <PagerSettings Position="TopAndBottom" />
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
</asp:Content>
