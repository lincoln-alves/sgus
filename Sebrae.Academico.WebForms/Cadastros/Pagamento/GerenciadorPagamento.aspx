<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeBehind="GerenciadorPagamento.aspx.cs"
    Inherits="Sebrae.Academico.WebForms.Cadastros.Pagamento.GerenciadorPagamento"
    EnableEventValidation="false" %>

<%@ Register Src="~/UserControls/ucInformarPagamento.ascx" TagName="ucInformarPagamento"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <div class="form-group">
            <asp:Label ID="lblConfiguracao" runat="server" Text="Filtrar por configurações de pagamento"
                AssociatedControlID="ddlConfigPagto" />
            <asp:DropDownList ID="ddlConfigPagto" runat="server" CssClass="form-control">
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Label ID="lblUF" runat="server" Text="Filtrar por UF" AssociatedControlID="ddlUf" />
            <asp:DropDownList ID="ddlUf" runat="server" CssClass="form-control">
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Label ID="lblNivelOcupacional" runat="server" Text="Filtrar por nível ocupacional" AssociatedControlID="ddlNivelOcupacional" />
            <asp:DropDownList ID="ddlNivelOcupacional" runat="server" CssClass="form-control">
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Label ID="lblperfil" runat="server" Text="Filtrar por perfil" AssociatedControlID="ddlPerfil" />
            <asp:DropDownList ID="ddlPerfil" runat="server" CssClass="form-control">
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Label ID="lblNome" runat="server" Text="Filtrar por nome do usuário" AssociatedControlID="txtNome" />
            <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar Usuários" OnClick="btnBuscar_Click"
                CssClass="btn btn-primary mostrarload" />
        </div>
    </fieldset>
    <hr />
    <asp:Panel ID="pnlInformacoesDePagamento" runat="server" Visible="false">
        <h4>Resultado da Busca</h4>
        <div class="panel-group" id="accordionGerenciador">
            <div class="panel-group" id="accordionGM">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <asp:LinkButton runat="server" ClientIDMode="Static" ID="lkbUsuarios" OnClick="lkbUsuario_Click"
                            Text="Usuários"></asp:LinkButton>
                    </div>
                    <div id="divUsuarios" class="panel-collapse collapse" runat="server" clientidmode="Static">
                        <div class="panel-body">
                            <asp:GridView ID="dgvInformacoesDePagamento" runat="server" OnRowCommand="dgvInformacoesDePagamento_RowCommand"
                                AutoGenerateColumns="false" OnPageIndexChanging="dgvInformacoesDePagamento_PageIndexChanging"
                                PageSize="100" CssClass="table col-sm-12" GridLines="None" AllowPaging="true">
                                <Columns>
                                    <asp:TemplateField HeaderText="Usuário">
                                        <ItemTemplate>
                                            <%#Eval("NomeUsuario")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="UF">
                                        <ItemTemplate>
                                            <%#Eval("UfDoUsuarioPorExtenso")%>
                                            - (<%#Eval("SiglaUfDoUsuario")%>)
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Nível Ocupacional">
                                        <ItemTemplate>
                                            <%#Eval("NomeNivelOcupacional")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemStyle Width="90px" HorizontalAlign="center" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lkbHistPagto" runat="server" CausesValidation="False" CommandName="histpagto" CssClass="mostrarload" CommandArgument='<%#Eval("IdUsuario")%>' ToolTip="Histórico Pagamento">
                                                <span class="btn btn-default btn-xs">
								                    <span class="glyphicon glyphicon-list"></span>
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
                    </div>
                </div>
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <asp:LinkButton ID="lkbRetornoBancario" runat="server" ClientIDMode="Static" OnClick="lkbRetornoBancario_Click"
                            Text="Retono Bancário"></asp:LinkButton>
                    </div>
                    <div id="divRetornoBancario" class="panel-collapse collapse" runat="server" clientidmode="Static">
                        <div class="panel-body">
                            <fieldset>
                                <div class="form-group">
                                    <asp:Label ID="lblTipoArquivo" runat="server" Text="Escolha o Tipo de Arquivo" AssociatedControlID="ddlTipoArquivo"></asp:Label>
                                    <asp:DropDownList ID="ddlTipoArquivo" runat="server" CssClass="form-control">
                                        <asp:ListItem Selected="True">CBR643</asp:ListItem>
                                        <asp:ListItem>RCB001</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="Label2" runat="server" Text="Entre com o arquivo texto" AssociatedControlID="FileUpload1"></asp:Label>
                                    <asp:FileUpload ID="FileUpload1" runat="server" CssClass="form-control" />
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="Label3" runat="server" Text="Entre com a data do processamento" AssociatedControlID="TxtData"></asp:Label>
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
                        <h4 class="modal-title">Histórico de Pagamento</h4>
                    </div>
                    <div class="modal-body">
                        <asp:GridView ID="dgvHistoricoDePagamento" runat="server" PageSize="1" AutoGenerateColumns="false"
                            CssClass="table col-sm-12" GridLines="None" OnRowDataBound="dgvHistoricoDePagamento_RowDataBound"
                            OnRowCommand="dgvHistoricoDePagamento_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="Data Início">
                                    <ItemTemplate>
                                        <%#Eval("DataInicioVigencia", "{0:dd/MM/yyyy}")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Data Fim Competência">
                                    <ItemTemplate>
                                        <%#Eval("DataFimVigencia", "{0:dd/MM/yyyy}")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Valor">
                                    <ItemTemplate>
                                        <%#Eval("ValorPagamento")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Forma de Pagamento">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFormaPagamento" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Configuração Pagamento">
                                    <ItemTemplate>
                                        <%#Eval("ConfiguracaoPagamento.Nome")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Pago">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPago" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemStyle Width="90px" HorizontalAlign="center" />
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemStyle Width="160px" HorizontalAlign="center" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lkbEditarHistPagto" runat="server" CausesValidation="False" CommandName="editar"
                                            CommandArgument='<%#Eval("Id")%>' Text="Editar">
                                            <span class="btn btn-default btn-xs">
								                <span class="glyphicon glyphicon-pencil"></span>
							                </span>
                                        </asp:LinkButton>
                                        <asp:Button ID="btnEnviaBB" runat="server" CausesValidation="False" CommandName="linksitebb"
                                            CommandArgument='<%#Eval("Id")%>' Text="Acessar o site do BB" CssClass="btn btn-default btn-xs" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <itemtemplate>Nenhum pagamento encontrado</itemtemplate>
                            </EmptyDataTemplate>
                        </asp:GridView>
                        <%--<asp:Button ID="btnEnviaBB" runat="server" OnClick="btnEnviaBB_Click" Text="Efetua Pagamento"
                            Visible="false" />--%>
                        <asp:Button ID="btnGerarPagamento" runat="server" Text="Gerar Pagamento" OnClick="btnGerarPagamento_Click" CssClass="btn btn-primary" />
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->
    </asp:Panel>
    <asp:Panel ID="pnlUcInformarPagamento" runat="server">
        <div class="modal fade in" id="ModalInformarPagamento" tabindex="-1" role="dialog"
            aria-labelledby="myModalLabel" aria-hidden="true" style="display: block;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button id="Button2" type="button" class="close" data-dismiss="modal" aria-hidden="true"
                            onserverclick="OcultarInformarPagamento_Click" runat="server">
                            &times;</button>
                        <h4 class="modal-title">Histórico de Pagamento</h4>
                    </div>
                    <div class="modal-body">
                        <uc1:ucInformarPagamento ID="ucInformarPagamento1" runat="server" OnInformouPagamento="InformarPagamento_InformouPagamento" />
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->
    </asp:Panel>
    <%--<form id="formInfPagamento" enctype="application/x-www-form-urlencoded" action="https://www16.bancodobrasil.com.br/site/mpag/"
    target="_blank" name="formInfPagamento">--%>


    <%--</form>--%>
</asp:Content>
