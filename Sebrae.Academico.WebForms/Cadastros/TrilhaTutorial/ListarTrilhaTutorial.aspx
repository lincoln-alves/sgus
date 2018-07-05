<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="ListarTrilhaTutorial.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.ListarTrilhaTutorial" %>
<%@ Import Namespace="Sebrae.Academico.Util.Classes" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <fieldset>
            <div class="form-group">
                <asp:Label ID="lblNome" runat="server" Text="Buscar por nome" AssociatedControlID="txtNome" />                
                <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="LblCategoria" runat="server" Text="Categoria" AssociatedControlID="ddlCategoria"></asp:Label>                
                <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="form-control">                                    
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click" CssClass="btn btn-primary mostrarload" />
                <asp:Button ID="btnNovo" runat="server" Text="Criar novo" OnClick="btnNovo_Click" CssClass="btn btn-default mostrarload" />
            </div>
        </fieldset>
    </div>
    <hr />

    <asp:Panel ID="pnlEtapas" runat="server" Visible="false">
        <div class="fakeTh">            
            <div class="row">
                <div class="col-xs-6">
                    Nome
                </div>                  
                <div class="col-xs-3">
                    Categoria
                </div>
                <div class="col-xs-3">
                </div>
            </div>
        </div>        
        <div class="ordenacaoHidden">
            <asp:HiddenField ID="hdnOrdenacao" runat="server" />
        </div>
        <ul class="fakeTable TrilhaTutorialContainer">
            <asp:Repeater ID="rptTrilhaTutorial" runat="server" OnItemDataBound="Repeater_OnItemDataBound">
                <HeaderTemplate>                    
                </HeaderTemplate>
                <ItemTemplate>
                    <li class="row reorder" guid="<%# DataBinder.Eval(Container, "DataItem.ID")%>" category="<%# (int) Eval("Categoria") %>">
                        <div class="col-xs-6">
                            <%# DataBinder.Eval(Container, "DataItem.Nome") %>
                        </div>                  
                        <div class="col-xs-3">
                            <%#( (Sebrae.Academico.Dominio.Enumeracao.enumCategoriaTrilhaTutorial) Eval("Categoria") ).GetDescription() %>
                        </div>
                        <div class="col-xs-3">
                            <div style="display: inline; float: right">
                                <asp:LinkButton ID="Editar" runat="server" CommandArgument="" OnClick="Editar_Click">
                                            <span class="btn btn-default btn-xs">
                                            <span class="glyphicon glyphicon-pencil">
                                            </span>
                                            </span>
                                </asp:LinkButton>
                                <asp:LinkButton ID="Excluir" runat="server" CommandArgument="" OnClick="Excluir_Click"
                                    OnClientClick="return ConfirmarExclusao();">
                                            <span class="btn btn-default btn-xs">
                                            <span class="glyphicon glyphicon-remove">
                                            </span>
                                            </span>
                                </asp:LinkButton>
                            </div>
                        </div>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    <li id="emptyCampo" runat="server" visible='<%# DataBinder.Eval(Container.Parent, "Items.Count").ToString() == "0" %>'>Nenhum Tutorial foi encontrado.</li>
                </FooterTemplate>
            </asp:Repeater>
        </ul>
        <div>
            <fieldset>
                <div class="form-group">                
                    <asp:Button ID="Button2" runat="server" Text="Salvar Ordem" OnClick="btnSalvaOrdem_Click" CssClass="btn-salva-ordem btn btn-primary btn-default mostrarload" />
                </div>
            </fieldset>
        </div>
    </asp:Panel>
    <link href="/css/fakeTable.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/js/jqueryUI/reordenaTrilhaTutorial.js"></script>
</asp:Content>
