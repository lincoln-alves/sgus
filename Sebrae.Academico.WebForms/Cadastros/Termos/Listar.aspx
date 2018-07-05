<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="Listar.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.Termos.Listar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <div class="form-group">
            <asp:Label ID="Label2" runat="server" Text="Nome" AssociatedControlID="txtNome"></asp:Label>
            <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click" CssClass="btn btn-primary mostrarload" />
            <asp:Button ID="btnNovo" runat="server" Text="Criar novo" OnClick="btnNovo_Click" CssClass="btn btn-default mostrarload" />
        </div>
    </fieldset>
    <hr />
    <asp:Panel ID="pnlPrograma" runat="server" Visible="false">
        <h4>Resultado da Busca</h4>
        <asp:Literal ID="litTable" runat="server" />
        <asp:GridView ID="dgvTermoAceite" runat="server" OnRowCommand="dgvTermoAceite_RowCommand" OnRowDataBound="dgvTermoAceite_OnRowDataBound" CssClass="table col-sm-12" AutoGenerateColumns="false" GridLines="None">
            <Columns>
                <asp:TemplateField HeaderText="Termos de Aceite e Políticas de Consequências">
                    <ItemTemplate>
                        <%#Eval("Nome") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="UF">
                    <ItemStyle Width="60px" HorizontalAlign="center" />
                    <ItemTemplate>
                        <%# Eval("Uf") != null ? Eval("Uf.Sigla") : "" %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <ItemStyle Width="120px" HorizontalAlign="center" />
                    <ItemTemplate>
                        <asp:LinkButton ID="lkbEditar" runat="server" CausesValidation="False" CommandName="editar" CommandArgument='<%# Eval("ID")%>' CssClass="mostrarload btn btn-default btn-xs" ToolTip="Editar">
							<span class="glyphicon glyphicon-pencil"></span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="lkbExcluir" runat="server" CausesValidation="False" CommandName="excluir" CommandArgument='<%# Eval("ID")%>' CssClass="btn btn-default btn-xs" OnClientClick="return ConfirmarExclusao();" ToolTip="Excluir">
							<span class="glyphicon glyphicon-remove"></span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="lkbDuplicar" runat="server" CausesValidation="False" CommandName="duplicar" CommandArgument='<%# Eval("ID")%>' CssClass="mostrarload btn btn-default btn-xs" ToolTip="Duplicar">
							<span class="glyphicon glyphicon-floppy-saved"></span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="lkbVisualizar" runat="server" CausesValidation="False" CommandName="visualizar" CommandArgument='<%# Eval("ID")%>' CssClass="mostrarload btn btn-default btn-xs" ToolTip="Visualizar">
							<span class="glyphicon glyphicon-search"></span>
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>

    <asp:Panel runat="server" ID="pnlVisualizar" Visible="False">
        <div class="modal fade in" style="display: block;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close" runat="server" OnServerClick="Fechar_OnServerClick"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Visualização do termo de aceite
                            <asp:Literal ID="ltrNome" runat="server"></asp:Literal></h4>
                    </div>
                    <div class="modal-body">

                        <div class="form-group">
                            <p>
                                <strong>Texto</strong>
                            </p>
                            <div class="well">
                                <asp:Literal ID="ltrTexto" runat="server"></asp:Literal>
                            </div>
                        </div>

                        <div class="form-group">
                            <p>
                                <strong>Política de consequência</strong>
                            </p>
                            <div class="well">
                                <asp:Literal ID="ltrPolitica" runat="server"></asp:Literal>
                            </div>
                        </div>

                        <div runat="server" id="divUF" class="form-group" visible="False">
                            <p>
                                <strong>UF</strong>
                            </p>
                            <div class="well">
                                <asp:Literal ID="ltrUf" runat="server"></asp:Literal>
                            </div>
                        </div>

                        <div class="form-group">
                            <p>
                                <strong>Categorias</strong>
                            </p>
                            <p>
                                <asp:Repeater ID="rptCategorias" runat="server" OnItemDataBound="rptCategorias_OnItemDataBound">
                                    <ItemTemplate>
                                        <p>
                                            <asp:Literal ID="ltrCategoria" runat="server"></asp:Literal>
                                        </p>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </p>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" runat="server" onserverclick="Fechar_OnServerClick" class="btn btn-primary">Fechar</button>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
