<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="ListarFaq.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="Sebrae.Academico.WebForms.Cadastros.Faq.ListarFaq" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <div class="form-group">
            <asp:Label runat="server" Text="Buscar por nome:" AssociatedControlID="txtNome"></asp:Label>
            <asp:TextBox runat="server" ID="txtNome" CssClass="form-control" />
        </div>
        <div class="form-group">
            <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click" CssClass="btn btn-primary mostrarload" />
            <asp:Button ID="btnNovo" runat="server" Text="Criar novo" OnClick="btnNovo_Click" CssClass="btn btn-default mostrarload" />
        </div>
    </fieldset>

    <asp:Panel runat="server" ID="pnlFaq" Visible="false">
        <h4>Resultado da busca</h4>
        <asp:GridView runat="server" ID="dvgFaq" CssClass="table col-sm-12" GridLines="None" AutoGenerateColumns="false" EnableModelValidation="true" OnRowCommand="dvgFaq_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="Título">
                    <ItemTemplate>
                        <%# Eval("Nome") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Descrição">
                    <ItemTemplate>
                        <%# Eval("Descricao") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="false">
                    <ItemStyle Width="110px" HorizontalAlign="center" />
                    <ItemTemplate>
                        <asp:LinkButton ID="lkbVisualizar" runat="server" CausesValidation="False" CommandName="visualizar"
                            CommandArgument='<%# Eval("ID")%>' ToolTip="Visualizar">
                        <span class="btn btn-default btn-xs">
                            <span class="glyphicon glyphicon-search"></span>
                        </span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="lkbEditar" runat="server" CausesValidation="False" CommandName="editar" CommandArgument='<%# Eval("ID")%>' ToolTip="Editar">
                        <span class="btn btn-default btn-xs">
                            <span class="glyphicon glyphicon-pencil"></span>
                        </span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="lkbExcluir" runat="server" CausesValidation="False" CommandName="excluir" CommandArgument='<%# Eval("ID")%>' ToolTip="Excluir"
                            OnClientClick="return ConfirmarExclusao();">
                            <span class="btn btn-default btn-xs">
								<span class="glyphicon glyphicon-remove"></span>
							</span>
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <itemtemplate>Nenhum registro retornado para a solicitação efetuada.</itemtemplate>
            </EmptyDataTemplate>
        </asp:GridView>
    </asp:Panel>

    <!-- MODAL -->
    <asp:Panel ID="pnlModal" runat="server" Visible="false">
        <div aria-hidden="false" aria-labelledby="myModalLabel" role="dialog" tabindex="-1"
            id="myModal" class="modal fade in" style="display: block;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true" runat="server"
                            onserverclick="OcultarModal_Click" id="btnFecharModal">
                            &times;</button>
                        <h4 class="modal-title">Visualização</h4>
                    </div>
                    <div class="modal-body">
                        <fieldset>
                            <div class="form-group">
                                <asp:Label ID="lblNome" runat="server" Text="Nome" AssociatedControlID="txtNomeVisualizar" />
                                <asp:TextBox ID="txtNomeVisualizar" runat="server" CssClass="form-control" MaxLength="250"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label1" runat="server" Text="Descrição" AssociatedControlID="txtDescricaoVisualizar" />
                                <asp:TextBox ID="txtDescricaoVisualizar" runat="server" CssClass="form-control ckeditor" MaxLength="1024" TextMode="MultiLine"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="OcultarModal_Click" CssClass="btn btn-default" />
                            </div>
                        </fieldset>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <!-- FIM - MODAL -->
</asp:Content>
