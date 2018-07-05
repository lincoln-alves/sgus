<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucLupaUsuario.ascx.cs"
    Inherits="Sebrae.Academico.WebForms.UserControls.ucLupaUsuario" %>
<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>

<asp:Panel ID="pnlModalLupaBase" runat="server" Visible="true">
    <div>
        <input id="hdIdusuario" type="hidden" runat="server" />
        <div id="pnlSelecao" style="vertical-align: middle;" class="formLupa">
            <fieldset>
                <div class="form-group">
                    <asp:Label ID="lblUsuario" runat="server" Text="Usuário" AssociatedControlID="txtNomeUsuarioSelecionado" />
                    <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip2" Chave="usuarioLupa" />
                    <asp:LinkButton ID="LinkButton1" runat="server" OnClick="btnAbrirLupa_Click" CssClass="mostrarload">
                        <asp:TextBox runat="server" ID="txtNomeUsuarioSelecionado" ReadOnly="True" CssClass="form-control"
                            Style="cursor: pointer; margin-top: 0;" placeholder="Clique para buscar" />
                    </asp:LinkButton>
                </div>
            </fieldset>
        </div>
    </div>
</asp:Panel>
<!-- Modal -->
<asp:Panel ID="pnlModalLupa" runat="server" Visible="false">
    <div aria-hidden="false" aria-labelledby="myModalLabel" role="dialog" tabindex="-1"
        id="ModalLupa" class="modal fade in" style="display: block;">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true" onserverclick="OcultarModal_Click"
                        runat="server">
                        &times;</button>
                    <h4 class="modal-title">Buscar Usuário</h4>
                </div>
                <div class="modal-body">
                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnPesquisar">
                        <fieldset>
                            <div class="form-group">
                                <asp:Label ID="lblNome" runat="server" Text="Nome" AssociatedControlID="txtNome" />
                                <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lblIf" runat="server" Text="UF" AssociatedControlID="ddlUF" />
                                <asp:DropDownList ID="ddlUF" runat="server" DataTextField="Nome" DataValueField="ID"
                                    CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lblCpf" runat="server" Text="CPF" AssociatedControlID="txtCPF" />
                                <asp:TextBox ID="txtCPF" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lblNivel" runat="server" Text="Nível Ocupacional" AssociatedControlID="ddlNivelOcupacional" />
                                <asp:DropDownList ID="ddlNivelOcupacional" runat="server" DataTextField="Nome" DataValueField="ID"
                                    CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <br />
                                <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click"
                                    CssClass="btn btn-primary mostrarload" />
                            </div>
                        </fieldset>
                    </asp:Panel>
                    <br />
                    <asp:Panel ScrollBars="Auto" runat="server" Direction="LeftToRight">
                        <asp:GridView ID="dgPesquisaUsuario" runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
                            CssClass="table col-sm-12" GridLines="None" OnSelectedIndexChanged="dgPesquisaUsuario_SelectedIndexChanged">
                            <Columns>
                                <asp:CommandField SelectText="Selecionar" ShowSelectButton="True" ButtonType="Link"
                                    ControlStyle-CssClass="mostrarload" />
                                <asp:BoundField HeaderText="Nome" DataField="Nome" SortExpression="Nome">
                                    <ItemStyle />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="UF" SortExpression="Uf.Nome">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" Text='<%#Eval("Uf.Nome")%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Nível Ocupacional" SortExpression="NivelOcupacional.Nome">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" Text='<%#Eval("NivelOcupacional.Nome")%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="CPF" DataField="CPF" SortExpression="CPF" />
                            </Columns>
                            <EmptyDataTemplate>
                                <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
</asp:Panel>
