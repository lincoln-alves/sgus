<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="Concluintes.aspx.cs" Inherits="Sebrae.Academico.WebForms.Relatorios.Concluintes.Concluintes" %>

<%@ Register Src="~/UserControls/ucTotalizadorRelatorio.ascx" TagPrefix="uc" TagName="ucTotalizadorRelatorio" %>
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
                            <asp:Label ID="lblTipoCurso" runat="server" AssociatedControlID="cbxTipoCurso" Text="Tipo de Curso" />
                            <asp:DropDownList ID="cbxTipoCurso" runat="server" CssClass="form-control" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label1" runat="server" AssociatedControlID="cbxUF" Text="UF" />
                            <asp:DropDownList ID="cbxUF" runat="server" CssClass="form-control" />
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
                            <asp:CheckBoxList ID="chkListaCamposVisiveis" runat="server" RepeatDirection="Vertical"
                                RepeatLayout="UnorderedList" ClientIDMode="Static">
                                <asp:ListItem Selected="True" Value="TipoCurso">Tipo de Curso</asp:ListItem>
                                <asp:ListItem Selected="True" Value="NomeCurso">Nome do Curso</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Concluintes">Concluintes</asp:ListItem>
                            </asp:CheckBoxList>
                        </div>
                    </fieldset>
                </div>
            </div>
        </div>
    </div>

    <asp:Button ID="btnPesquisar" runat="server" Text="Consultar" OnClick="btnPesquisar_Click"
        CssClass="btn btn-primary mostrarload" />

    <hr />

    <uc:ucTotalizadorRelatorio runat="server" ID="ucTotalizadorRelatorio" />

    <asp:GridView ID="dgRelatorio" runat="server" AutoGenerateColumns="False" AllowSorting="True"
        CssClass="table col-sm-12" GridLines="None" OnSorting="dgRelatorio_Sorting" AllowPaging="True"
        OnPageIndexChanging="dgRelatorio_PageIndexChanging" PageSize="100">
        <Columns>
            <asp:BoundField DataField="TipoCurso" HeaderText="Tipo de Curso" SortExpression="TipoCurso" />
            <asp:BoundField DataField="NomeCurso" HeaderText="Nome do Curso" SortExpression="NomeCurso" />
            <asp:BoundField DataField="Concluintes" HeaderText="Concluintes" SortExpression="Concluintes" />
        </Columns>
        <EmptyDataTemplate>
            <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
        </EmptyDataTemplate>
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
</asp:Content>
