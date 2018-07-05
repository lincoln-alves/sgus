<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="Listar.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.MonitoramentoIndicadores.Listar" %>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <fieldset>
            <div class="form-group">
                <asp:Label ID="Label2" runat="server" Text="Buscar por Ano" AssociatedControlID="txtAno"></asp:Label>
                <asp:TextBox ID="txtAno" runat="server" MaxLength="250" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click" CssClass="btn btn-primary mostrarload" />
                <asp:Button ID="btnNovo" runat="server" Text="Criar novo" OnClick="btnNovo_Click" CssClass="btn btn-default mostrarload" />
            </div>
        </fieldset>
    </div>

    <asp:Panel ID="pnlMonitoramentoIndicador" runat="server" Visible="false">
        <hr />
        <h4>Resultado da Busca</h4>

        <asp:GridView ID="dgvMonitoramentoIndicador" OnRowCommand="dgvMonitoramentoIndicador_RowCommand" CssClass="table col-sm-12" runat="server" AutoGenerateColumns="false" EnableModelValidation="True" GridLines="None">
            <Columns>
                <asp:TemplateField HeaderText="Ano">
                    <ItemTemplate>
                        <%#Eval("Ano") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <ItemStyle Width="90px" HorizontalAlign="center"/>
                    <ItemTemplate>
                        <asp:LinkButton ID="lkbEditar" runat="server" CausesValidation="False" CommandName="editar" CommandArgument='<%# Eval("ID")%>' ToolTip="Editar">
                            <span class="btn btn-default btn-xs">
								<span class="glyphicon glyphicon-pencil"></span>
							</span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="lkbExcluir" runat="server" CausesValidation="False" CommandName="excluir" CommandArgument='<%# Eval("ID")%>' ToolTip="Excluir" OnClientClick="return ConfirmarExclusao();">
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
