<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="ListarDemandas.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.Demanda.ListarDemandas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <div class="form-group">
            <asp:Label ID="Label2" runat="server" Text="Buscar por nome" AssociatedControlID="txtNome"></asp:Label>
            <asp:TextBox ID="txtNome" runat="server" class="form-control"></asp:TextBox>
        </div>
        <div class="form-group" runat="server">
            <asp:Label ID="Label1" runat="server" Text="UF" AssociatedControlID="cbxUF" />
            <asp:DropDownList ID="cbxUF" runat="server" CssClass="form-control">
            </asp:DropDownList>
        </div>
        <div class="form-group" runat="server">
            <asp:Label ID="labelStatus" runat="server" Text="Status" AssociatedControlID="cbxStatus" />
            <asp:DropDownList ID="cbxStatus" runat="server" CssClass="form-control">
                <asp:ListItem Text="-- Todos --" Value="todos" />
                <asp:ListItem Text="Ativo" Value="1" />
                <asp:ListItem Text="Inativo" Value="0" />
            </asp:DropDownList>
        </div>

         <div class="form-group" runat="server">
            <asp:Label ID="label3" runat="server" Text="Tipo" AssociatedControlID="cbxTipo" />
            <asp:DropDownList ID="cbxTipo" runat="server" CssClass="form-control">
                <asp:ListItem Text="-- Todos --" Value="todos" />
                <asp:ListItem Text="Reembolso" Value="1" />
                <asp:ListItem Text="Outros" Value="2" />
            </asp:DropDownList>
        </div>

        <div class="form-group">
            <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click"
                CssClass="btn btn-primary mostrarload" />
            <asp:Button ID="btnNovo" runat="server" Text="Cadastrar novo" OnClick="btnNovo_Click" CssClass="btn btn-default mostrarload" />
        </div>
    </fieldset>
    <hr />
    <asp:Panel ID="pnlProcesso" runat="server" Visible="false">
        <h4>Resultado da Busca</h4>
        <asp:GridView ID="dgvProcesso" runat="server" CssClass="table col-sm-12" OnRowCommand="dgvProcesso_RowCommand"
            AutoGenerateColumns="false" EnableModelValidation="True"
            OnSelectedIndexChanged="dgvProcesso_SelectedIndexChanged">
            <Columns>
                <asp:TemplateField HeaderText="Demanda">
                    <ItemTemplate>
                        <%#Eval("Nome") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tipo">
                    <ItemTemplate>
                        <%# Eval("Tipo") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ativa">
                    <ItemTemplate>
                        <%# ((bool)Eval("Ativo")) ? "Sim" : "Não" %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="UF">
                    <ItemTemplate>
                        <%# (Eval("UF") != null) ? Eval("UF.Sigla") : "" %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <ItemStyle Width="90px" HorizontalAlign="center" />
                    <ItemTemplate>
                        <asp:LinkButton ID="lkbEditar" runat="server" CausesValidation="False" CommandName="editar"
                            CssClass="mostrarload" CommandArgument='<%# Eval("ID")%>' ToolTip="Editar">
                                <span class="btn btn-default btn-xs">
									<span class="glyphicon glyphicon-pencil"></span>
								</span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="lkbDuplicar" runat="server" CausesValidation="False" CommandName="duplicar"
                            ToolTip="Duplicar" CommandArgument='<%# Eval("ID")%>' OnClientClick="return confirm('Deseja realmente duplicar o registro?');">
                                <span class="btn btn-default btn-xs">
									<span class="glyphicon glyphicon-floppy-saved"></span>
								</span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="lkbExcluir" runat="server" CausesValidation="False" CommandName="excluir"
                            ToolTip="Excluir" CommandArgument='<%# Eval("ID")%>' OnClientClick="return ConfirmarExclusao();">
                                <span class="btn btn-default btn-xs">
									<span class="glyphicon glyphicon-remove"></span>
								</span>
                        </asp:LinkButton>

                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
            </EmptyDataTemplate>
        </asp:GridView>

    </asp:Panel>
</asp:Content>
