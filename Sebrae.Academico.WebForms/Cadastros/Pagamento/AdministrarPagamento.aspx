<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="AdministrarPagamento.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.Pagamento.AdministrarPagamento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h3>
        Gerenciador de Pagamento</h3>
    <div>
        <fieldset>
            <div class="form-group">
                <asp:Label ID="lblNome" runat="server" Text="Filtrar por nome"
                    AssociatedControlID="txtNome" />
                <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="Label1" runat="server" Text="Filtrar por CPF"
                    AssociatedControlID="txtCPF" />
                <asp:TextBox ID="txtCPF" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar Dados de Pagamento" OnClick="btnBuscar_Click"
                    CssClass="btn btn-primary mostrarload" />
            </div>
            <div class="form-group" id="dvDdlUsuario" runat="server" visible="false">
                <asp:Label ID="Label2" runat="server" Text="Usuários Encontrados"
                    AssociatedControlID="ddlUsuario" />
                <asp:DropDownList ID="ddlUsuario" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlUsuario_OnSelectedIndexChanged"></asp:DropDownList>
            </div>
            <div class="form-group" id="dvPagamento" runat="server" visible="false">
                <asp:Button ID="btnGerarDadosPagamento" runat="server" Text="Gerar Dados de Pagamento" OnClick="btnGerarDadosPagamento_Click"
                    CssClass="btn btn-primary mostrarload" />
            </div>
    <hr />
    <asp:Panel ID="pnlInformacoesDePagamento" runat="server" Visible="false">
        <h4>
            Resultado da Busca</h4>
            <div class="panel-body">
                <asp:GridView ID="dgvDados" runat="server" 
                    AutoGenerateColumns="false" OnRowCommand="dgvDados_RowCommand"
                    PageSize="100" CssClass="table col-sm-12" GridLines="None" AllowPaging="true">
                    <Columns>
                        <asp:TemplateField HeaderText="Usuário">
                            <ItemTemplate>
                                <%#Eval("Usuario.Nome")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CPF">
                            <ItemTemplate>
                                <%#Eval("Usuario.CPF")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Início da Vigência">
                            <ItemTemplate>
                                <%#Eval("DataInicioVigencia")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fim da Vigência">
                            <ItemTemplate>
                                <%#Eval("DataFimVigencia")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Valor">
                            <ItemTemplate>
                                <%#Eval("ValorPagamento")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Início da Renovação">
                            <ItemTemplate>
                                <%#Eval("DataInicioRenovacao")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Data Máxima da Inadimplência">
                            <ItemTemplate>
                                <%#Eval("DataMaxInadimplencia")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Data Máxima da Inadimplência">
                            <ItemTemplate>
                                <%#Eval("DataMaxInadimplencia")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nosso Número">
                            <ItemTemplate>
                                <%#Eval("NossoNumero")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Pagamento Efetuado">
                            <ItemTemplate>
                                <%# (Convert.ToBoolean(Eval("PagamentoEfetuado")) ? "Sim" : "Não") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Data Aceite TermoAdesao">
                            <ItemTemplate>
                                <%#Eval("DataAceiteTermoAdesao")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Data de Vencimento">
                            <ItemTemplate>
                                <%#Eval("DataVencimento")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Data de Vencimento">
                            <ItemTemplate>
                                <asp:LinkButton ID="lkbHistPagto" runat="server" CausesValidation="False" CommandName="histpagto" CssClass="mostrarload" CommandArgument='<%#Eval("Id")%>' ToolTip="Confirmar Pagamento" >
                                    <span class="btn btn-default btn-xs">
								        <span class="glyphicon glyphicon-pencil"></span>
							        </span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </asp:Panel>
        </fieldset>
    </div>
</asp:Content>
