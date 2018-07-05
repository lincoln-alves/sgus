<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="ListarConfiguracaoPagamento.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.ListarConfiguracaoPagamento"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <div class="form-group">
            <asp:Label ID="lblNome" runat="server" Text="Buscar por tipo de pagamento" AssociatedControlID="cbxTipoPagamento" />
            <asp:DropDownList ID="cbxTipoPagamento" runat="server" CssClass="form-control">
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Button ID="btnPesquisa" runat="server" Text="Pesquisar" OnClick="btnPesquisa_Click" CssClass="btn btn-primary mostrarload" />
            <asp:Button ID="btnNovo" runat="server" Text="Criar novo" OnClick="btnNovo_Click" CssClass="btn btn-default mostrarload" />
        </div>
    </fieldset>
    <hr />
    <asp:GridView ID="dgvConfigPagamento" runat="server" AutoGenerateColumns="False" CssClass="table col-sm-12"
        GridLines="None" OnRowCommand="dgvConfigPagamento_RowCommand">
        <Columns>
            <asp:BoundField DataField="Nome" HeaderText="Nome" />
            <asp:BoundField DataField="DataInicioCompetencia" HeaderText="Inicio Competência"
                DataFormatString="{0:dd/MM/yyyy}" />
            <asp:BoundField DataField="DataFimCompetencia" HeaderText="Fim Competência" DataFormatString="{0:dd/MM/yyyy}" />
            <asp:BoundField DataField="ValorAPagar" HeaderText="Valor a Pagar" />
            <asp:CheckBoxField DataField="Ativo" HeaderText="Ativo" />
            <asp:CheckBoxField DataField="BloqueiaAcesso" HeaderText="Bloqueia Acesso" />
            <asp:CheckBoxField DataField="Recursiva" HeaderText="Recursiva" />
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
</asp:Content>