<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucModalAlternativas.ascx.cs"
    Inherits="Sebrae.Academico.WebForms.UserControls.ucModalAlternativas" %>
<asp:Panel ID="pnlAlternativas" runat="server" Visible="false">
    <div class="fakeTh">
        Alternativas
    </div>
    <div class="ordenacaoAlternativasHidden">
        <asp:HiddenField ID="hdnOrdenacaoAlternativas" runat="server" />
    </div>
    <ul class="fakeTable AlternativasContainer">
        <asp:Repeater ID="rptAlternativas" runat="server" OnItemDataBound="Repeater_OnItemDataBound">
            <HeaderTemplate>
            </HeaderTemplate>
            <ItemTemplate>
                <li guid="<%# DataBinder.Eval(Container, "DataItem.ID")%>">
                    <div class="ordem" style="display: inline; float: left; margin: 2px 10px; font-size: 11px;
                        color: #237929;">
                        <%# Container.ItemIndex + 1 %>º
                    </div>
                    <%# DataBinder.Eval(Container, "DataItem.Nome")%>
                    <div style="display: inline; float: right">
                        <asp:LinkButton ID="Editar" runat="server" CommandArgument="" OnClick="EditarAlternativa_Click">
                                <span class="btn btn-default btn-xs">
                                    <span class="glyphicon glyphicon-pencil">
                                    </span>
                                </span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="Excluir" runat="server" CommandArgument="" OnClick="ExcluirAlternativa_Click"
                            OnClientClick="return ConfirmarExclusao();" CssClass="mostrarload">
                                <span class="btn btn-default btn-xs">
                                    <span class="glyphicon glyphicon-remove">
                                    </span>
                                </span>
                        </asp:LinkButton>
                    </div>
                </li>
            </ItemTemplate>
            <FooterTemplate>
                <li id="emptyCampo" runat="server" visible='<%# DataBinder.Eval(Container.Parent, "Items.Count").ToString() == "0" %>'>
                    Sem Alternativas cadastrados </li>
            </FooterTemplate>
        </asp:Repeater>
    </ul>
    <div class="form-group">
        <asp:Button ID="btnAdicionarAlternativa" runat="server" Text="Adicionar Alternativa"
            CssClass="btn btn-primary pull-right" OnClick="btnAdicionarAlternativa_Click" />
        <div style="clear: both;">
        </div>
    </div>
</asp:Panel>
