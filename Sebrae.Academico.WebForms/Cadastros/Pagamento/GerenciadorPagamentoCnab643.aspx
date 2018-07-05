<%@ Page Title="" Language="C#" MasterPageFile="~/Gerenciador.master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeBehind="GerenciadorPagamentoCnab643.aspx.cs"
    Inherits="Sebrae.Academico.WebForms.Cadastros.Pagamento.GerenciadorPagamentoCnab643"
    EnableEventValidation="false" %>

<%@ Register Src="~/UserControls/ucInformarPagamento.ascx" TagName="ucInformarPagamento"
    TagPrefix="uc1" %>
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
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar Usuários" OnClick="btnBuscar_Click"
                    CssClass="btn btn-default mostrarload" />
            </div>
        </fieldset>
    </div>
    <hr />
    <asp:Panel ID="pnlInformacoesDePagamento" runat="server" Visible="false">
        <h4>
            Resultado da Busca</h4>
        <div class="panel-group" id="accordionGerenciador">
            <div class="panel-group" id="accordionGM">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <asp:LinkButton runat="server" ClientIDMode="Static" ID="lkbUsuarios" 
                            Text="Dados de Acesso"></asp:LinkButton>
                    </div>
                    <div id="divUsuarios" class="panel-collapse collapse" runat="server" clientidmode="Static">
                        <div class="panel-body">
                            <asp:GridView ID="dgvInformacoesDePagamento" runat="server" OnRowCommand="dgvInformacoesDePagamento_RowCommand"
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
                                                CommandArgument='<%# Eval("ConfiguracaoPagamento.id")%>' Text="H"></asp:LinkButton>
                                            |
                                            <asp:LinkButton ID="lkbInformarPagto" runat="server" CausesValidation="False" CommandName="infopagto"
                                                CommandArgument='<%# Eval("ConfiguracaoPagamento.id")%>' Text="IP"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <asp:LinkButton ID="lkbRetornoBancario" runat="server" 
                            ClientIDMode="Static" Text="Retono Bancário"></asp:LinkButton>
                    </div>
                    <div id="divRetornoBancario" class="panel-collapse collapse" runat="server" clientidmode="Static">
                        <div class="panel-body">
                            <fieldset>
                                <div class="form-group">
                                    <asp:Label ID="lblTipoArquivo" runat="server" Text="Escolha o Tipo de Arquivo:" AssociatedControlID="ddlTipoArquivo"></asp:Label>
                                    <asp:DropDownList ID="ddlTipoArquivo" runat="server" CssClass="form-control">
                                        <asp:ListItem Selected="True">CBR643</asp:ListItem>
                                        <asp:ListItem>RCB001</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="Label2" runat="server" Text="Entre com o arquivo texto:" AssociatedControlID="FileUpload1"></asp:Label>
                                    <asp:FileUpload ID="FileUpload1" runat="server" CssClass="form-control" />
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="Label3" runat="server" Text="Entre com a data do processamento:" AssociatedControlID="TxtData"></asp:Label>
                                    <asp:TextBox ID="TxtData" runat="server" MaxLength="10" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <asp:Button ID="botao" runat="server" Text="Processar" OnClick="botao_Click" CssClass="btn btn-default mostrarload" />
                                </div>
                            </fieldset>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlHistoricoPagamento" runat="server">
        <div class="modal fade in" id="ModalHistoricoPagamento" tabindex="-1" role="dialog"
            aria-labelledby="myModalLabel" aria-hidden="true" style="display: block;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button id="Button1" type="button" class="close" data-dismiss="modal" aria-hidden="true"
                            onserverclick="OcultarHistoricoPagamento_Click" runat="server">
                            &times;</button>
                        <h4 class="modal-title">
                            Histórico de Pagamento</h4>
                    </div>
                    <div class="modal-body">
                        <asp:GridView ID="dgvHistoricoDePagamento" runat="server" PageSize="1" AutoGenerateColumns="false"
                            CssClass="table col-sm-12" GridLines="None">
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
                        <asp:Button ID="btnEnviaBB" runat="server" OnClick="btnEnviaBB_Click" Text="Efetua Pagamento"
                            Visible="false" />
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->
    </asp:Panel>
    <asp:Panel ID="pnlUcInformarPagamento" runat="server">
        <div class="modal fade in" id="ModalInformarPagamento" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
            aria-hidden="true" style="display: block;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button id="Button2" type="button" class="close" data-dismiss="modal" aria-hidden="true"
                            onserverclick="OcultarInformarPagamento_Click" runat="server">
                            &times;</button>
                        <h4 class="modal-title">
                            Histórico de Pagamento</h4>
                    </div>
                    <div class="modal-body">
                        <uc1:ucInformarPagamento ID="ucInformarPagamento1" runat="server" />
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->
    </asp:Panel>
</asp:Content>
