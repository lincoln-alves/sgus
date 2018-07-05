<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="~/Relatorios/AtividadeExtraCurricular/AtividadeExtraCurricular.aspx.cs" Inherits="Sebrae.Academico.WebForms.Relatorios.AtividadeExtraCurricular.AtividadeExtraCurricular"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                            <asp:Label ID="Label3" runat="server" Text="Data Términio Início" AssociatedControlID="txtDataTerIni"></asp:Label>
                            <asp:TextBox ID="txtDataTerIni" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label4" runat="server" Text="Data Término Final" AssociatedControlID="txtDataTerFim"></asp:Label>
                            <asp:TextBox ID="txtDataTerFim" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label1" runat="server" Text="Data de Atualização Início" AssociatedControlID="txtDataCadIni"></asp:Label>
                            <asp:TextBox ID="txtDataCadIni" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label2" runat="server" Text="Data de Atualização Final" AssociatedControlID="txtDataCadFim"></asp:Label>
                            <asp:TextBox ID="txtDataCadFim" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label5" runat="server" Text="Carga Horária" AssociatedControlID="txtCargaHoraria"></asp:Label>
                            <asp:TextBox ID="txtCargaHoraria" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
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
                                <asp:ListItem Selected="True" Value="NomeInstituicao">Instituição</asp:ListItem>
                                <asp:ListItem Selected="True" Value="SolucaoEducacional">Solução Educacional</asp:ListItem>
                                <asp:ListItem Selected="True" Value="DataInicio">Data Início</asp:ListItem>
                                <asp:ListItem Selected="True" Value="DataFinal">Data Final</asp:ListItem>
                                <asp:ListItem Selected="True" Value="CargaHoraria">Carga Horária</asp:ListItem>
                                <asp:ListItem Selected="True" Value="DescricaoSolucao">Descrição Solução</asp:ListItem>
                                <asp:ListItem Selected="True" Value="DataAtualizacao">Data Atualização</asp:ListItem>
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
    <asp:GridView ID="dgRelatorio" runat="server" AutoGenerateColumns="False" AllowSorting="True"
        CssClass="table col-sm-12" GridLines="None" OnSorting="dgRelatorio_Sorting" AllowPaging="True"
        OnPageIndexChanging="dgRelatorio_PageIndexChanging" PageSize="100">
        <Columns>
            <asp:BoundField DataField="NomeInstituicao" HeaderText="Instituição" SortExpression="NomeInstituicao" />
            <asp:BoundField DataField="SolucaoEducacional" HeaderText="Solução Educacional" SortExpression="SolucaoEducacional" />            
            <asp:BoundField DataField="DataInicio" HeaderText="Data Início" DataFormatString="{0:dd/MM/yyyy}" SortExpression="DataInicio" />
            <asp:BoundField DataField="DataFinal" HeaderText="Data Final" DataFormatString="{0:dd/MM/yyyy}" SortExpression="DataFinal" />
            <asp:BoundField DataField="CargaHoraria" HeaderText="Carga Horária" SortExpression="CargaHoraria" />
            <asp:BoundField DataField="DescricaoSolucao" HeaderText="Descrição Solução" SortExpression="DescricaoSolucao" />
            <asp:BoundField DataField="DataAtualizacao" HeaderText="Data Atualização" DataFormatString="{0:dd/MM/yyyy}" SortExpression="DataAtualizacao" />
            <asp:TemplateField>
                <ItemTemplate>
                        <asp:LinkButton ID="lkbInfo" runat="server" CausesValidation="False" CommandName="info"
                                        EnableViewState="true" CommandArgument='<%# Eval("idFileServer") %>' ToolTip="Ver arquivo"
                                        OnClick="lkbInfo_Click">
                            Ver arquivo
                        </asp:LinkButton>                
                </ItemTemplate>
            </asp:TemplateField>            
        </Columns>
        <EmptyDataTemplate>
            <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
        </EmptyDataTemplate>        
        <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
    </asp:GridView>
    <fieldset runat="server" id="componenteGeracaoRelatorio" clientidmode="Static" visible="false">
        <hr />
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
            $("#<%= txtDataTerIni.ClientID %>").mask("99/99/9999");
            $("#<%= txtDataTerFim.ClientID %>").mask("99/99/9999");
            $("#<%= txtDataCadIni.ClientID %>").mask("99/99/9999");
            $("#<%= txtDataCadFim.ClientID %>").mask("99/99/9999");
        });
    </script>
</asp:Content>
