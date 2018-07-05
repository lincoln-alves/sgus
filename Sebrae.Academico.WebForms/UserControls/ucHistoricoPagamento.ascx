<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHistoricoPagamento.ascx.cs"
    Inherits="Sebrae.Academico.WebForms.UserControls.ucHistoricoPagamento" %>
<asp:GridView ID="dgvInformacoesDeHistoricoDePagamento" runat="server" PageSize="1"
    CssClass="table col-sm-12" GridLines="None" OnRowCommand="dgvInformacoesDeHistoricoDePagamento_RowCommand"
    OnRowDataBound="dgvInformacoesDeHistoricoDePagamento_RowDataBound" AutoGenerateColumns="false">
    <Columns>
        <asp:TemplateField HeaderText="Início da Vigência">
            <ItemTemplate>
                <%#Eval("DataInicioVigencia", "{0:dd/MM/yyyy}")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Nome da Configuração Pagamento">
            <ItemTemplate>
                <%#Eval("ConfiguracaoPagamento.Nome")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Fim da Vigência">
            <ItemTemplate>
                <%#Eval("DataFimVigencia", "{0:dd/MM/yyyy}")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Valor">
            <ItemTemplate>
                <%#Eval("ValorPagamento")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Data Pagamento">
            <ItemTemplate>
                <%#Eval("DataPagamento")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Pago ?">
            <ItemStyle Width="90px" HorizontalAlign="center" />
            <ItemTemplate>
                <asp:CheckBox ID="chkInPago" runat="server" ClientIDMode="Static" Enabled="false" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField ShowHeader="False">
            <ItemStyle Width="90px" HorizontalAlign="center" />
            <ItemTemplate>
                <asp:LinkButton ID="lkbEditarHistPagto" runat="server" CausesValidation="False" CommandName="editar"
                    CommandArgument='<%# Eval("ID")%>' Text="Editar">
                    <span class="btn btn-default btn-xs">
						<span class="glyphicon glyphicon-pencil"></span>
					</span>
                </asp:LinkButton>
                <asp:LinkButton ID="lkblinkSiteBB" runat="server" CausesValidation="False" CommandName="linksitebb"
                    CommandArgument='<%# Eval("ID")%>' Text="Acessar o site do BB">
                    <span class="btn btn-default btn-xs">
						<span class="glyphicon glyphicon-usd"></span>
					</span>
                </asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <EmptyDataTemplate>
        <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
    </EmptyDataTemplate>
</asp:GridView>
