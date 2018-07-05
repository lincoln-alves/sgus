<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="UnificadoSolucaoEducacional.aspx.cs" Inherits="Sebrae.Academico.WebForms.Relatorios.UnificadoSolucaoEducacional.UnificadoSolucaoEducacional"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/UserControls/ucCategorias.ascx" TagName="ucCategorias" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucSeletorListBox.ascx" TagName="ucSeletorListBox" TagPrefix="uc2" %>

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
                            <asp:Label ID="Label1" runat="server" Text="Formas de aquisição" AssociatedControlID="ListBoxesFormaDeAquisicao" />
                            <uc2:ucSeletorListBox runat="server" ID="ListBoxesFormaDeAquisicao" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label4" runat="server" Text="Tipos de oferta" AssociatedControlID="ListBoxesTiposDeOferta" />
                            <uc2:ucSeletorListBox runat="server" ID="ListBoxesTiposDeOferta" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label5" runat="server" Text="Programas" AssociatedControlID="ListBoxesProgramas" />
                            <uc2:ucSeletorListBox runat="server" ID="ListBoxesProgramas" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label10" runat="server" Text="Categorias" AssociatedControlID="ucCategorias1" />
                            <uc1:ucCategorias ID="ucCategorias1" runat="server" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label6" runat="server" Text="Público alvo" AssociatedControlID="ListBoxesPublicosAlvo" />
                            <uc2:ucSeletorListBox runat="server" ID="ListBoxesPublicosAlvo" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label2" runat="server" Text="Nível Ocupacional" AssociatedControlID="ListBoxesNivelOcupacional" />
                            <uc2:ucSeletorListBox runat="server" ID="ListBoxesNivelOcupacional" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label7" runat="server" Text="Perfil de usuário" AssociatedControlID="ListBoxesPerfis" />
                            <uc2:ucSeletorListBox runat="server" ID="ListBoxesPerfis" />
                        </div>
                        <div class="form-group" id="dvUF" runat="server">
                            <asp:Label ID="Label3" runat="server" Text="UF" AssociatedControlID="ListBoxesUF" />
                            <uc2:ucSeletorListBox runat="server" ID="ListBoxesUF" />
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
                                <asp:ListItem Selected="True" Value="SE">Solução Educacional</asp:ListItem>
                                <asp:ListItem Selected="True" Value="FormaAquisicao">Forma de aquisição</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Categoria">Categoria</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Fornecedor">Fornecedor</asp:ListItem>
                                <asp:ListItem Selected="True" Value="CargaHoraria">Carga horária</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Ativo">Ativo</asp:ListItem>
                                <asp:ListItem Selected="True" Value="QntOfertas">QntOfertas</asp:ListItem>
                                <asp:ListItem Selected="True" Value="QntTurmas">QntTurmas</asp:ListItem>
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
    <asp:Literal ID="liTotalizador" runat="server"></asp:Literal>
    <hr />
    <asp:GridView ID="dgRelatorio" runat="server" AutoGenerateColumns="False" AllowSorting="True"
        CssClass="table col-sm-12" GridLines="None" OnSorting="dgRelatorio_Sorting" AllowPaging="True"
        OnPageIndexChanging="dgRelatorio_PageIndexChanging" PageSize="100">
        <Columns>
            <asp:BoundField DataField="SE" HeaderText="Solução Educacional" SortExpression="Nome" />
            <asp:BoundField DataField="FormaAquisicao" HeaderText="Forma de aquisição" SortExpression="Nome" />
            <asp:BoundField DataField="Categoria" HeaderText="Categoria" SortExpression="Nome" />
            <asp:BoundField DataField="Fornecedor" HeaderText="Fornecedor" SortExpression="Nome" />
            <asp:BoundField DataField="CargaHoraria" HeaderText="Carga horária" SortExpression="Nome" />
            <asp:BoundField DataField="Ativo" HeaderText="Ativo" SortExpression="Nome" />
            <asp:BoundField DataField="QntOfertas" HeaderText="QntOfertas" SortExpression="Nome" />
            <asp:BoundField DataField="QntTurmas" HeaderText="QntTurmas" SortExpression="Nome" />
            <asp:BoundField DataField="UFResponsavel" HeaderText="UF Responsável" SortExpression="UFResponsavel" />
        </Columns>
        <EmptyDataTemplate>
            <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
        </EmptyDataTemplate>
        <PagerSettings Mode="NumericFirstLast" />
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
    <%--<asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="ConsultaSolucaoEducacional"
        TypeName="Sebrae.Academico.BP.Relatorios.RelatorioUnificadoSolucaoEducacional">
        <SelectParameters>
            <asp:Parameter Name="pNome" Type="String" />
            <asp:Parameter Name="pCPF" Type="String" />
            <asp:Parameter Name="pNivelOcupacional" Type="Int32" />
            <asp:Parameter Name="pUf" Type="Int32" />
            <asp:Parameter Name="pDataInicialMatricula" Type="DateTime" />
            <asp:Parameter Name="pDataFinalMatricula" Type="DateTime" />
        </SelectParameters>
    </asp:ObjectDataSource>--%>
</asp:Content>
