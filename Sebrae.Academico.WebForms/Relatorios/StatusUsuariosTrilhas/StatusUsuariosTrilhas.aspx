<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="StatusUsuariosTrilhas.aspx.cs" Inherits="Sebrae.Academico.WebForms.Relatorios.StatusUsuariosTrilhas.StatusUsuariosTrilhas"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/UserControls/ucTotalizadorRelatorio.ascx" TagPrefix="uc" TagName="ucTotalizadorRelatorio" %>
<%@ Register Src="~/UserControls/ucLupaUsuario.ascx" TagPrefix="uc" TagName="ucLupaUsuario" %>
<%@ Register Src="~/UserControls/ucSeletorListBox.ascx" TagPrefix="uc" TagName="ucSeletorListBox" %>
<%@ Register Src="~/UserControls/ucFormatoSaidaRelatorio.ascx" TagPrefix="uc" TagName="SaidaRel" %>

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
                            <uc:LupaUsuario ID="cbxNomeLupa" runat="server" IsNacional="True" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblTrilhas" runat="server" Text="Trilhas" AssociatedControlID="cbxTrilha" />
                            <asp:DropDownList ID="cbxTrilha" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="cbxTrilha_SelectedIndexChanged" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label3" runat="server" Text="Nível da trilha" AssociatedControlID="cbxNivelTrilha" />
                            <asp:DropDownList ID="cbxNivelTrilha" runat="server" CssClass="form-control">
                                <asp:ListItem Selected="True" Text="-- Todos ou Selecione uma Trilha --" Value="">
                                </asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label4" runat="server" Text="Nível ocupacional" AssociatedControlID="cbxNivelOcupacional" />
                            <asp:DropDownList ID="cbxNivelOcupacional" runat="server" CssClass="form-control" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label2" runat="server" Text="UF" AssociatedControlID="cbxUF" />
                            <asp:DropDownList ID="cbxUF" runat="server" CssClass="form-control" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label5" runat="server" Text="Status da matrícula" AssociatedControlID="ucListBoxStatus" />
                            <uc:ucSeletorListBox runat="server" ID="ucListBoxStatus" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label6" runat="server" Text="Data de Inscrição" AssociatedControlID="cbxDataInicio" />
                            <asp:TextBox ID="cbxDataInicio" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label7" runat="server" Text="Data de Conclusão" AssociatedControlID="cbxDataDeConclusao" />
                            <asp:TextBox ID="cbxDataDeConclusao" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label8" runat="server" Text="Data Limite" AssociatedControlID="cbxDataLimite" />
                            <asp:TextBox ID="cbxDataLimite" runat="server" CssClass="form-control"></asp:TextBox>
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
                                <asp:ListItem Selected="True" Value="Trilha">Trilha</asp:ListItem>
                                <asp:ListItem Selected="True" Value="NivelTrilha">Nível da trilha</asp:ListItem>
                                <asp:ListItem Selected="True" Value="NivelOcupacional">Nível ocupacional</asp:ListItem>
                                <asp:ListItem Selected="True" Value="UF">UF</asp:ListItem>
                                <asp:ListItem Selected="True" Value="StatusMatricula">Status da matrícula</asp:ListItem>
                                <asp:ListItem Selected="True" Value="DataInicio">Data de Inscrição</asp:ListItem>
                                <asp:ListItem Selected="True" Value="DataFim">Data Conclusão</asp:ListItem>
                                <asp:ListItem Selected="True" Value="DataLimite">Data de limite</asp:ListItem>
                                <asp:ListItem Selected="True" Value="CPF">CPF</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Email">Email</asp:ListItem>
                                <asp:ListItem Selected="True" Value="NotaProvaFinal">Nota da Prova Final</asp:ListItem>
                                <asp:ListItem Selected="True" Value="SprintsRealizados">Sprint Realizados</asp:ListItem>
                                <asp:ListItem Selected="True" Value="DataAlteracaoStatus">Data de alteracao do status</asp:ListItem>
                                <asp:ListItem Selected="True" Value="SolucoesAutoindicativas">Soluções do Trilheiro Indicadas</asp:ListItem>
                                <asp:ListItem Selected="True" Value="SolucoesRealizadas">Quantidade de Soluções Sebrae</asp:ListItem>
                                <asp:ListItem Selected="True" Value="TotalMoedasOuro">Total de Moedas de Ouro</asp:ListItem>
                                <asp:ListItem Selected="True" Value="TotalMoedasPrata">Total de Moedas de Pratas</asp:ListItem>
                            </asp:CheckBoxList>
                        </div>
                    </fieldset>
                </div>
            </div>
        </div>
    </div>
    <br />
    <asp:Button ID="btnPesquisar" runat="server" Text="Consultar" CssClass="btn btn-primary mostrarload" OnClick="btnPesquisar_Click" />
    <hr />

    <uc:ucTotalizadorRelatorio runat="server" ID="ucTotalizadorRelatorio" />

    <div class="table-responsive">
        <asp:GridView ID="dgRelatorio" runat="server" AutoGenerateColumns="False" AllowSorting="True"
            CssClass="table col-sm-12" GridLines="None" OnSorting="dgRelatorio_Sorting" AllowPaging="True"
            OnPageIndexChanging="dgRelatorio_PageIndexChanging" PageSize="20">
            <Columns>
                <asp:BoundField DataField="NomeUsuario" HeaderText="Nome" SortExpression="NomeUsuario" />
                <asp:BoundField DataField="Trilha" HeaderText="Trilha" SortExpression="Trilha" />
                <asp:BoundField DataField="NivelTrilha" HeaderText="Nível da trilha" SortExpression="NivelTrilha" />
                <asp:BoundField DataField="NivelOcupacional" HeaderText="Nível ocupacional" SortExpression="NivelOcupacional" />
                <asp:BoundField DataField="UF" HeaderText="UF" SortExpression="UF" />
                <asp:BoundField DataField="StatusMatricula" HeaderText="Status da matrícula" SortExpression="StatusMatricula" />
                <asp:BoundField DataField="DataInicio" HeaderText="Data de Inscrição" DataFormatString="{0:dd/MM/yyyy}" SortExpression="DataInicio" />
                <asp:BoundField DataField="DataFim" HeaderText="Data de Conclusão" DataFormatString="{0:dd/MM/yyyy}" SortExpression="DataFim" />
                <asp:BoundField DataField="DataLimite" HeaderText="Data limite" DataFormatString="{0:dd/MM/yyyy}" SortExpression="DataLimite" />
                <asp:BoundField DataField="CPF" HeaderText="CPF" SortExpression="CPF" />
                <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
                <asp:BoundField DataField="NotaProvaFinal" HeaderText="Nota da Prova Final" SortExpression="NotaProvaFinal" />
                <asp:BoundField DataField="SprintsRealizados" HeaderText="Sprint Realizados" SortExpression="SprintsRealizados" />
                <asp:BoundField DataField="DataAlteracaoStatus" HeaderText="Data de alteração do status" DataFormatString="{0:dd/MM/yyyy}" SortExpression="DataAlteracaoStatus" />
                <asp:BoundField DataField="SolucoesAutoindicativas" HeaderText="Soluções do Trilheiro Indicadas" SortExpression="SolucoesAutoindicativas" />
                <asp:BoundField DataField="SolucoesRealizadas" HeaderText="Quantidade de Soluções Sebrae" SortExpression="SolucoesRealizadas" />
                <asp:BoundField DataField="TotalMoedasOuro" HeaderText="Total de Moedas de Ouro" SortExpression="TotalMoedasOuro" />
                <asp:BoundField DataField="TotalMoedasPrata" HeaderText="Total de Moedas de Prata" SortExpression="TotalMoedasPrata" />
            </Columns>
            <EmptyDataTemplate>
                <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
            </EmptyDataTemplate>
        </asp:GridView>
    </div>
    <fieldset runat="server" id="componenteGeracaoRelatorio" clientidmode="Static" visible="false">
        <div class="form-group">
            <uc:SaidaRel ID="ucFormatoSaidaRelatorio" runat="server" />
        </div>
        <div class="form-group">
            <asp:Button ID="btnGerarRelatorio" runat="server" Text="Gerar Relatório" CssClass="btn btn-primary"
                OnClick="btnGerarRelatorio_Click" />
        </div>
    </fieldset>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="ConsultaStatusUsuariosTrilhas"
        TypeName="Sebrae.Academico.BP.Relatorios.RelatorioStatusUsuariosTrilhas">
        <SelectParameters>
            <asp:Parameter Name="NomeUsuario" Type="String" />
            <asp:Parameter Name="Trilha" Type="String" />
            <asp:Parameter Name="NivelTrilha" Type="String" />
            <asp:Parameter Name="NivelOcupacional" Type="String" />
            <asp:Parameter Name="UF" Type="String" />
            <asp:Parameter Name="StatusMatricula" Type="String" />
            <asp:Parameter Name="DataInicio" Type="DateTime" />
            <asp:Parameter Name="DataFim" Type="DateTime" />
            <asp:Parameter Name="DataLimite" Type="DateTime" />
            <asp:Parameter Name="CPF" Type="String" />
            <asp:Parameter Name="Email" Type="String" />
            <asp:Parameter Name="NotaProvaFinal" Type="Int32" />
            <asp:Parameter Name="SprintsRealizados" Type="Int32" />
            <asp:Parameter Name="SolucoesAutoindicativas" Type="Int32" />
            <asp:Parameter Name="SolucoesRealizadas" Type="Int32" />
            <asp:Parameter Name="TotalMoedasOuro" Type="Int32" />
            <asp:Parameter Name="TotalMoedasPrata" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <script type="text/javascript">
        jQuery(function ($) {
            $("#<%= cbxDataInicio.ClientID %>").mask("99/99/9999");
            $("#<%= cbxDataDeConclusao.ClientID %>").mask("99/99/9999");
            $("#<%= cbxDataLimite.ClientID %>").mask("99/99/9999");

            $("#ContentPlaceHolder1_ContentPlaceHolder1_cbxNomeLupa_txtNomeUsuarioSelecionado").click(function (e) {
                $("#Filtros").collapse("show");
            });
        });
    </script>
</asp:Content>
