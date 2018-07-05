<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="RelatorioAcessos.aspx.cs" Inherits="Sebrae.Academico.WebForms.Relatorios.RelatorioAcessos"
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
                        <uc:LupaUsuario ID="luUsuario" runat="server" IsNacional="true" />
                        <div class="form-group">
                            <asp:Label ID="Label5" runat="server" Text="Perfil" AssociatedControlID="cbxPefil"></asp:Label>
                            <asp:DropDownList ID="cbxPefil" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label1" runat="server" Text="UF" AssociatedControlID="cbxUF"></asp:Label>
                            <asp:DropDownList ID="cbxUF" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label2" runat="server" Text="Nível Ocupacional" AssociatedControlID="cbxNivelOcupacional"></asp:Label>
                            <asp:DropDownList ID="cbxNivelOcupacional" runat="server" CssClass="form-control">
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
                                <asp:ListItem Selected="True">Nome</asp:ListItem>
                                <asp:ListItem Selected="True">Email</asp:ListItem>
                                <asp:ListItem Selected="True">UF</asp:ListItem>
                                <asp:ListItem Selected="True" Value="NivelOcupacional">Nível Ocupacional</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Acesso">Acesso</asp:ListItem>
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
        OnPageIndexChanging="dgRelatorio_PageIndexChanging" PageSize="50">
        <Columns>
            <asp:BoundField DataField="Nome" HeaderText="Nome" SortExpression="Nome" />
            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
            <asp:BoundField DataField="UF" HeaderText="UF" SortExpression="UF" />
            <asp:BoundField DataField="NivelOcupacional" HeaderText="Nivel Ocupacional" SortExpression="NivelOcupacional" />
            <asp:BoundField DataField="Acesso" HeaderText="Data de Acesso" SortExpression="Acesso"
                DataFormatString="{0:dd/MM/yyyy}" />
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
            $("#<%= txtDataInicial.ClientID %>").mask("99/99/9999");
            $("#<%= txtDataFinal.ClientID %>").mask("99/99/9999");
        });
    </script>
</asp:Content>
