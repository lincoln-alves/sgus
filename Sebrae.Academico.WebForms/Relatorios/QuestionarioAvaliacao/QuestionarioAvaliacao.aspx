<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="QuestionarioAvaliacao.aspx.cs" Inherits="Sebrae.Academico.WebForms.Relatorios.QuestionarioAvaliacao.QuestionarioAvaliacao"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        jQuery(function ($) {
            $("#<%= txtDataInicial.ClientID %>").mask("99/99/9999");
            $("#<%= txtDataFinal.ClientID %>").mask("99/99/9999");
        });
    </script>
    <h3>
        Questionário de Avaliação</h3>
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
                            <asp:Label ID="lblNomeQuestionario" runat="server" AssociatedControlID="cbxNomeQuestionario"
                                Text="Nome do Questionário" />
                            <asp:DropDownList ID="cbxNomeQuestionario" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblFormaAquisicao" runat="server" AssociatedControlID="cbxFormaAquisicao"
                                Text="Forma de Aquisição" />
                            <asp:DropDownList ID="cbxFormaAquisicao" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblSolucaoEducacional" runat="server" AssociatedControlID="cbxSolucaoEducacional"
                                Text="Solução Educacional" />
                            <asp:DropDownList ID="cbxSolucaoEducacional" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label3" runat="server" Text="Data Inicial" AssociatedControlID="txtDataInicial"></asp:Label>
                            <asp:TextBox ID="txtDataInicial" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label4" runat="server" Text="Data Final" AssociatedControlID="txtDataFinal"></asp:Label>
                            <asp:TextBox ID="txtDataFinal" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
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
                                <asp:ListItem Selected="True" Value="Questionario">Questionário</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Nome">Nome</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Email">Email</asp:ListItem>
                                <asp:ListItem Selected="True" Value="CPF">CPF</asp:ListItem>
                                <asp:ListItem Selected="True" Value="UF">UF</asp:ListItem>
                                <asp:ListItem Selected="True" Value="NivelOcupacional">Nível Ocupacional</asp:ListItem>
                                <asp:ListItem Selected="True" Value="DataAdmissao">Data da Admissão</asp:ListItem>
                            </asp:CheckBoxList>
                        </div>
                    </fieldset>
                </div>
            </div>
        </div>
    </div>
    <br />
    <asp:Button ID="btnPesquisar" runat="server" CssClass="btn btn-primary mostrarload"
        Text="Consultar" OnClick="btnPesquisar_Click" />
    <hr />
    <asp:GridView ID="dgRelatorio" runat="server" AutoGenerateColumns="False" AllowSorting="True"
        CssClass="table col-sm-12" GridLines="None" OnSorting="dgRelatorio_Sorting" AllowPaging="True"
        OnPageIndexChanging="dgRelatorio_PageIndexChanging" PageSize="100">
        <Columns>
            <asp:BoundField DataField="Questionario" HeaderText="Questionário" SortExpression="Questionario" />
            <asp:BoundField DataField="Nome" HeaderText="Nome" SortExpression="Nome" />
            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
            <asp:BoundField DataField="CPF" HeaderText="CPF" SortExpression="CPF" />
            <asp:BoundField DataField="UF" HeaderText="UF" SortExpression="UF" />
            <asp:BoundField DataField="NivelOcupacional" HeaderText="NivelOcupacional" SortExpression="NivelOcupacional" />
            <asp:BoundField DataField="DataAdmissao" HeaderText="DataAdmissao" SortExpression="DataAdmissao" />
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
</asp:Content>
