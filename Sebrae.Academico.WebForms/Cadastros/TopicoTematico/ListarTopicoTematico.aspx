<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="ListarTopicoTematico.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.TopicoTematico.ListarTopicoTematico"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <fieldset>
            <div class="form-group">
                <asp:Label ID="lblNome" runat="server" Text="Buscar por nome" AssociatedControlID="txtNome" />
                <asp:TextBox ID="txtNome" runat="server" CssClass="form-control" MaxLength="250"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click"
                    CssClass="btn btn-primary mostrarload" />
                <asp:Button ID="btnAdicionarTopicoTematico" runat="server" Text="Criar novo" OnClick="btnNovo_Click"
                    CssClass="btn btn-default mostrarload" />
            </div>
        </fieldset>
    </div>
    <hr />
    <asp:Panel ID="pnlTopicoTematico" runat="server" Visible="false">
        <div>
            <h4>
                Resultado da Busca</h4>
            <asp:GridView ID="dgvTopicoTematico" runat="server" CssClass="table col-sm-12" GridLines="None"
                OnRowCommand="dgvTopicoTematico_RowCommand" AutoGenerateColumns="false" EnableModelValidation="True">
                <Columns>
                    <asp:TemplateField HeaderText="Ponto Sebrae">
                        <ItemTemplate>
                            <%#Eval("Nome") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Descrição do Texto de Envio">
                        <ItemTemplate>
                            <%#Eval("DescricaoTextoEnvio")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Quantidade Mínima">
                        <ItemTemplate>
                            <%#Eval("QtdMinimaPontosAtivFormativa")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False">
                        <ItemStyle Width="90px" HorizontalAlign="center" />
                        <ItemTemplate>
                            <asp:LinkButton ID="lkbEditar" runat="server" CausesValidation="False" CommandName="editar"
                                CommandArgument='<%# Eval("ID")%>' ToolTip="Editar" CssClass="mostrarload">
                                <span class="btn btn-default btn-xs">
								    <span class="glyphicon glyphicon-pencil"></span>
							    </span>
                            </asp:LinkButton>
                            <asp:LinkButton ID="lkbExcluir" runat="server" CausesValidation="False" CommandName="excluir"
                                CommandArgument='<%# Eval("ID")%>' ToolTip="Excluir" OnClientClick="return ConfirmarExclusao();">
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
        </div>
    </asp:Panel>
</asp:Content>
