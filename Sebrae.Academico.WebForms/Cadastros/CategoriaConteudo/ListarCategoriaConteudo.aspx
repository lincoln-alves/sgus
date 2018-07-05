<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeBehind="ListarCategoriaConteudo.aspx.cs"
    Inherits="Sebrae.Academico.WebForms.Cadastros.ListarCategoriaConteudo" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <div class="form-group">
            <asp:Label ID="lblNome" runat="server" Text="Buscar por nome" AssociatedControlID="txtNome" />
            <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click" CssClass="btn btn-primary mostrarload" />
            <asp:Button ID="Button2" runat="server" Text="Criar nova" OnClick="btnNovo_Click" CssClass="btn btn-default mostrarload" />
            <%--<asp:Button ID="btnResolverHerancaSigla" runat="server" Visible="False" Text="Resolver Herança de Sigla" OnClick="btnResolverHerancaSigla_Click" CssClass="btn btn-default mostrarload" />--%>
        </div>
    </fieldset>
    <hr />
    <asp:Panel ID="pnlCategoriaConteudo" runat="server" Visible="false">
        <div>
            <h4>Resultado da Busca</h4>
            <asp:Literal ID="litTable" runat="server" />
            <asp:GridView ID="dgvCategoriaConteudo" runat="server" OnRowCommand="dgvFormaAquisicao_RowCommand"
                CssClass="table col-sm-12" GridLines="None" AutoGenerateColumns="false" EnableModelValidation="True">
                <Columns>
                    <asp:TemplateField HeaderText="Categoria">
                        <ItemTemplate>
                            <%#Eval("Nome") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False">
                        <ItemStyle Width="50px" HorizontalAlign="center" VerticalAlign="Middle" />
                        <ItemTemplate>
                            <%# (Boolean.Parse(Eval("PossuiFiltroCategorias").ToString())) ? "<span data-toggle=\"tooltip\" data-placement=\"left\" data-title=\"Esta categoria possui filtro de Status. Todas as matrículas nas Soluções Educacionais desta categoria poderão ter seus Status filtrados.\" class=\"isTooltip label label-success\">FILTRO</span>" : "" %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False">
                        <ItemStyle Width="90px" HorizontalAlign="center" />
                        <ItemTemplate>
                            <asp:LinkButton ID="lkbEditar" runat="server" CausesValidation="False" CommandName="editar"
                                ToolTip="Editar" CommandArgument='<%# Eval("ID")%>' CssClass="mostrarload">
                                <span class="btn btn-default btn-xs">
								    <span class="glyphicon glyphicon-pencil"></span>
							    </span> 

                            </asp:LinkButton>
                            <asp:LinkButton ID="lkbExcluir" runat="server" CausesValidation="False" CommandName="excluir"
                                CommandArgument='<%# Eval("ID")%>' ToolTip="Excluir" OnClientClick="return ConfirmarExclusao();">
                                <span class="btn btn-default btn-xs">
								    <span class="glyphicon glyphicon-remove">
                                </span>
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
        <script>
            $(function () {
                $('[data-toggle="tooltip"]').tooltip()
            })
        </script>
    </asp:Panel>
</asp:Content>
