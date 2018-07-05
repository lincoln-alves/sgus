<%@ Page Title="" Language="C#" MasterPageFile="~/Gerenciador.master" AutoEventWireup="true"
    CodeBehind="GerenciarPagamento.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.GerenciarPagamento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h3>
        Gerenciador de Pagamento</h3>
    <div>
        <fieldset>
            <div class="form-group">
                <asp:Label ID="lblConfiguracao" runat="server" Text="Configurações de Pagamento:"
                    AssociatedControlID="ddlConfigPagto" />
                <asp:DropDownList ID="ddlConfigPagto" runat="server" CssClass="form-control">
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:Label ID="lblUF" runat="server" Text="Uf:" AssociatedControlID="ddlUf" />
                <asp:DropDownList ID="ddlUf" runat="server" CssClass="form-control">
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:Label ID="lblNivelOcupacional" runat="server" Text="Nível Ocupacional:" AssociatedControlID="ddlNivelOcupacional" />
                <asp:DropDownList ID="ddlNivelOcupacional" runat="server" CssClass="form-control">
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:Label ID="lblperfil" runat="server" Text="Perfil:" AssociatedControlID="ddlPerfil" />
                <asp:DropDownList ID="ddlPerfil" runat="server" CssClass="form-control">
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:Label ID="lblNome" runat="server" Text="Nome" AssociatedControlID="txtNome" />
                <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click"
                    CssClass="btn btn-default" />
            </div>
        </fieldset>
    </div>
    <br />
    <asp:Panel ID="pnlInformacoesDePagamento" runat="server">
        <div>
            <p>
                <b>Resultado da Busca</b></p>
            <asp:Literal ID="litTable" runat="server" />
            <asp:GridView ID="dgvInformacoesDePagamento" runat="server" PageSize="1" OnRowCommand="dgvInformacoesDePagamento_RowCommand"
                OnRowDataBound="dgvInformacoesDePagamento_RowDataBound" AutoGenerateColumns="false"
                CssClass="table col-sm-12" GridLines="None">
                <Columns>
                    <asp:TemplateField HeaderText="Usuário">
                        <ItemTemplate>
                            <%#Eval("Usuario.Nome") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="UF">
                        <ItemTemplate>
                            <%#Eval("Usuario.UF.Nome")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Nível Ocupacional">
                        <ItemTemplate>
                            <%#Eval("Usuario.NivelOcupacional.Nome")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False">
                        <ItemStyle Width="90px" HorizontalAlign="center" />
                        <ItemTemplate>
                            <asp:LinkButton ID="lkbHistPagto" runat="server" CausesValidation="False" CommandName="histpagto"
                                CommandArgument='<%# Eval("ConfiguracaoPagamento.id")%>' Text="Histório Pagto"></asp:LinkButton>
                            |
                            <asp:LinkButton ID="lkbInformarPagto" runat="server" CausesValidation="False" CommandName="infopagto"
                                CommandArgument='<%# Eval("ConfiguracaoPagamento.id")%>' Text="Informar Pagto"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlHistoricoPagamento" runat="server">
        <div>
            <p>
                <asp:Literal ID="Literal1" runat="server" />
                <asp:GridView ID="dgvHistoricoDePagamento" runat="server" PageSize="1" AutoGenerateColumns="false"
                    CssClass="table col-sm-12" GridLines="None" OnSelectedIndexChanged="dgvHistoricoDePagamento_SelectedIndexChanged">
                    <Columns>
                        <asp:TemplateField HeaderText="Data Início Competência">
                            <ItemTemplate>
                                <%#Eval("DataInicioCompetencia")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Data Fim Competência">
                            <ItemTemplate>
                                <%#Eval("DataFimCompetencia")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Valor a Pagar">
                            <ItemTemplate>
                                <%#Eval("ValorAPagar")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Dias Validade">
                            <ItemTemplate>
                                <%#Eval("QuantidadeDiasValidade")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Dias Renovação">
                            <ItemTemplate>
                                <%#Eval("QuantidadeDiasRenovacao")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Dias Inadimplência">
                            <ItemTemplate>
                                <%#Eval("QuantidadeDiasInadimplencia")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Configuração Pagamento">
                            <ItemTemplate>
                                <%#Eval("nmConfiguracaoPagamento")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemStyle Width="90px" HorizontalAlign="center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:Button ID="btnVoltar" runat="server" OnClick="btnVoltar_Click" Text="Voltar"
                    Visible="false" />
        </div>
        <div>
            <table style="width: 100%;">
                <tr>
                    <td align="right">
                        <asp:Label ID="Label3" runat="server" Style="margin-left: 0px" Text="Entre com o arquivo texto:"
                            Width="258px"></asp:Label>
                    </td>
                    <td align="right">
                        <asp:FileUpload ID="FileUpload1" runat="server" />
                    </td>
                    <td align="right">
                        <asp:Label ID="Label4" runat="server" Style="margin-left: 0px" Text="Entre com a data do processamento:"
                            Width="258px"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtData2" runat="server"></asp:TextBox>
                        &nbsp;<asp:Label ID="Label5" runat="server" Text="Formato ddmmaa"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="4">
                        <asp:Button ID="botao" runat="server" Text="Processar" OnClick="botao_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Label ID="Label6" runat="server" Visible="False"></asp:Label>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>
        </div>
        <%--  %><asp:Button ID="btnVoltar" runat="server" onclick="btnVoltar_Click" Text="Voltar" /> --%>
    </asp:Panel>
    <asp:Panel ID="pnlArquivoRetorno" runat="server" Visible="true">
        <fieldset>
            <div class="form-group">
                <asp:Label ID="Label1" runat="server" Text="Arquivo de Retorno Bancário:" AssociatedControlID="fupldArquivoRetorno" />
                <asp:FileUpload ID="fupldArquivoRetorno" runat="server" />
                <br />
                <asp:Label ID="Label2" runat="server" Text="Entre com a data do processamento:"></asp:Label>
                <asp:TextBox ID="txtData" runat="server"></asp:TextBox>
                <asp:Label ID="lblFormato" runat="server" Text="Formato ddmmaaaa"></asp:Label>
            </div>
            <div class="form-group">
                <asp:Button ID="btnEnviarArquivo" runat="server" Text="Enviar" CssClass="btn btn-default"
                    OnClick="btnEnviarArquivo_Click" />
            </div>
            <div>
                <asp:Label ID="lblResultado" runat="server" Text=""></asp:Label>
            </div>
            <td align="right">
            </td>
            <asp:Button runat="server" ID="UploadButton" Text="Upload" OnClick="UploadButton_Click" />
            <br />
            <br />
            <asp:Label runat="server" ID="StatusLabel" Text="Upload status: " />
        </fieldset>
    </asp:Panel>
</asp:Content>
