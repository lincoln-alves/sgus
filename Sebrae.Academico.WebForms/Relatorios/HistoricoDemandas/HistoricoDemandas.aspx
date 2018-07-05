<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" 
    CodeBehind="HistoricoDemandas.aspx.cs" Inherits="Sebrae.Academico.WebForms.Relatorios.HistoricoDemandas.HistoricoDemandas" %>

<%@ Register Src="~/UserControls/ucSeletorListBox.ascx" TagName="ucSeletorListBox" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucLupaUsuario.ascx" TagName="ucLupaUsuario" TagPrefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-group" id="gruporelatorio">
        <div class="panel panel-default">
            <div class="panel-heading">
                <a data-toggle="collapse" data-parent="#gruporelatorio" href="#Filtros">Filtros
                </a>
            </div>
            <asp:Panel ID="Filtros" runat="server" ClientIDMode="Static" CssClass="panel-collapse collapse">
                <div class="panel-body">
                    <fieldset>
                        <div class="form-group">
                            <asp:Label ID="lblProcesso" runat="server" Text="Demanda" AssociatedControlID="cbxProcesso" />
                            <asp:DropDownList ID="cbxProcesso" runat="server" CssClass="form-control" OnSelectedIndexChanged="cbxProcesso_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </div>

                        <div class="form-group">
                            <asp:Label ID="Label2" runat="server" Text="Etapa da Demanda" AssociatedControlID="cbxDemanda" />
                            <asp:DropDownList ID="cbxDemanda" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>

                        <div class="form-group">
                            <uc1:ucLupaUsuario ID="ucLupaUsuario" IsNacional="true" Chave="lupaUsuario" runat="server" Text="Usuário Demandante" />
                        </div>

                        <div class="form-group">
                            <asp:Label ID="Label5" runat="server" Text="Data Início" AssociatedControlID="txtDataInicio" />
                            <asp:TextBox ID="txtDataInicio" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <asp:Label ID="Label6" runat="server" Text="Data Fim" AssociatedControlID="txtDataFim" />
                            <asp:TextBox ID="txtDataFim" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <asp:Label ID="Label1" runat="server" Text="Número da Demanda" AssociatedControlID="txtProcessoResposta" />
                            <asp:TextBox ID="txtProcessoResposta" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>

                        <div class="form-group" runat="server">
                            <asp:Label ID="labelUF" runat="server" Text="UF" AssociatedControlID="ListBoxesUF" />
                            <uc1:ucSeletorListBox runat="server" ID="ListBoxesUF" />
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
                                <asp:ListItem Selected="True" Value="IdProcessoResposta">Número</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Processo">Demanda</asp:ListItem>
                                <asp:ListItem Selected="True" Value="UsuarioDemandante">Usuário Demandante</asp:ListItem>
                                <asp:ListItem Selected="True" Value="DataAbertura">Data Abertura</asp:ListItem>
                                <asp:ListItem Selected="True" Value="EtapaConcluida">Etapa em Andamento</asp:ListItem>
                                <asp:ListItem Selected="True" Value="DataAlteracao">Data de Mudança de Etapa</asp:ListItem>
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
            <asp:BoundField DataField="IdProcessoResposta" HeaderText="Número" SortExpression="IdProcessoResposta" />
            <asp:BoundField DataField="Processo" HeaderText="Demanda" SortExpression="Processo" />
            <asp:BoundField DataField="UsuarioDemandante" HeaderText="Usuário Demandante" SortExpression="UsuarioDemandante" />
            <asp:BoundField DataField="DataAbertura" HeaderText="Data Abertura" SortExpression="DataAbertura" />
            <asp:BoundField DataField="EtapaConcluida" HeaderText="Etapa em Andamento" SortExpression="EtapaConcluida" />
            <asp:BoundField DataField="DataAlteracao" HeaderText="Data de Mudança de Etapa" SortExpression="DataAlteracao" />
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton ID="lkbInfo" runat="server" CausesValidation="False" CommandName="info" CssClass="mostrarload"
                        EnableViewState="true" CommandArgument='<%# Eval("IdProcessoResposta") %>' ToolTip="Ver informações"
                        OnClick="lkbInfo_Click">
                            Ver detalhes
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
        </EmptyDataTemplate>
        <PagerSettings Mode="NumericFirstLast" PageButtonCount="20" Position="TopAndBottom" />
    </asp:GridView>

    <script type="text/javascript">
        jQuery(function ($) {

            $("#Filtros").on('shown.bs.collapse', function () {
                $.cookie('activeFiltroGroup', 'true');
            });

            $("#Filtros").on('hidden.bs.collapse', function () {
                $.removeCookie('activeFiltroGroup');
            });

            $("#<%= txtDataInicio.ClientID %>").mask("99/99/9999");
            $("#<%= txtDataFim.ClientID %>").mask("99/99/9999");
        });
    </script>

</asp:Content>
