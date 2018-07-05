<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GerenciamentoAluno.aspx.cs"
    MaintainScrollPositionOnPostback="true" MasterPageFile="~/Pagina.master"
    Inherits="Sebrae.Academico.WebForms.Cadastros.Painel.GerenciamentoAluno" %>

<%@ Register Src="~/UserControls/ucNotificacoes.ascx" TagName="ucNotificacoes" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/ucTrilhas.ascx" TagName="ucTrilhas" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/ucCursos.ascx" TagName="ucCursos" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/ucHistoricoPagamento.ascx" TagName="ucHistoricoPagamento"
    TagPrefix="uc" %>
<%@ Register Src="~/UserControls/ucFaleConosco.ascx" TagName="ucFaleConosco" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/ucAbandono.ascx" TagName="ucAbandono" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/ucPublicacaoSaber.ascx" TagName="ucPublicacaoSaber"
    TagPrefix="uc" %>
<%@ Register Src="~/UserControls/ucAtividadeExtraCurricular.ascx" TagName="ucAtividadeExtraCurricular"
    TagPrefix="uc" %>
<%@ Register Src="~/UserControls/ucUsuario.ascx" TagName="ucUsuario" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/ucInformarPagamento.ascx" TagName="ucInformarPagamento"
    TagPrefix="uc" %>
<%@ Register Src="~/UserControls/ucCategorias.ascx" TagName="ucCategorias" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/ucTutorias.ascx" TagName="ucTutorias" TagPrefix="uc" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc:LupaUsuario ID="LupaUsuario" runat="server" OnUserSelected="UserSelectedHandler" />
    <hr />
    <div>
        <asp:Panel ID="pnlGerenciador" runat="server" Visible="false">
            <div class="panel-group" id="accordionGerenciador">
                <div class="panel-group" id="accordionGM">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <a href="#divDadosCadastrais" data-toggle="collapse" data-parent="#accordionGM">Dados Pessoais</a>
                        </div>
                        <div id="divDadosCadastrais" class="panel-collapse collapse" runat="server" clientidmode="Static">
                            <div class="panel-body">
                                <uc:ucUsuario ID="ucUsuario1" runat="server" />
                            </div>

                            <div class="panel-body">
                                <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click"
                                    CssClass="btn btn-default mostrarload" />
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-default" runat="server" id="divAreas" visible="False">
                        <div class="panel-heading">
                            <a href="#divAreasAccordion" data-toggle="collapse" data-parent="#accordionGM">Áreas SGC</a>
                        </div>
                        <div id="divAreasAccordion" class="panel-collapse collapse">
                            <div class="panel-body">
                                <div class="row">
                                    <asp:Repeater ID="rptAreas" runat="server" OnItemDataBound="rptAreas_OnItemDataBound">
                                        <ItemTemplate>
                                            <div class="col-xs-12 col-sm-6 col-md-4">
                                                <div class="form-group">
                                                    <p>
                                                        <strong>ÁREA
                                                            <asp:Literal ID="literalArea" runat="server"></asp:Literal>
                                                        </strong>
                                                    </p>
                                                    <p style="margin-left: 20px" class="text-muted">
                                                        Subáreas
                                                    </p>
                                                    <asp:Repeater ID="rptSubareas" runat="server" OnItemDataBound="rptSubareas_OnItemDataBound">
                                                        <ItemTemplate>
                                                            <p style="margin-left: 20px;">
                                                                <asp:Literal ID="literalSubarea" runat="server"></asp:Literal>
                                                            </p>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <a href="#divUfs" data-toggle="collapse" data-parent="#accordionGM">Categoria de Conteúdo</a>
                        </div>
                        <div id="divUfs" class="panel-collapse collapse" runat="server" clientidmode="Static">
                            <div class="panel-body">
                                <uc:ucCategorias ID="ucCategorias" runat="server" />
                            </div>

                            <div class="panel-body">
                                <asp:Button ID="btnSalvarCategorias" runat="server" Text="Salvar" OnClick="btnSalvar_Click"
                                    CssClass="btn btn-default mostrarload" />
                            </div>
                        </div>
                    </div>
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <a href="#divRegistroAcessos" data-toggle="collapse" data-parent="#accordionGM">Registro de Acessos</a>
                        </div>
                        <div id="divRegistroAcessos" class="panel-collapse collapse" runat="server" clientidmode="Static">
                            <div class="panel-body">
                                <asp:GridView ID="dgvLogAcessosDoUsuario" runat="server" AutoGenerateColumns="false"
                                    GridLines="None" CssClass="table col-sm-12">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Data do Acesso">
                                            <ItemTemplate>
                                                <%#Eval("DataAcesso")%>
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
                            <a href="#divNotificacoes" data-toggle="collapse" data-parent="#accordionGM">Notificações</a>
                        </div>
                        <div id="divNotificacoes" class="panel-collapse collapse" runat="server" clientidmode="Static">
                            <div class="panel-body">
                                <uc:ucNotificacoes ID="ucNotificacoes1" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <a href="#divTrilhas" data-toggle="collapse" data-parent="#accordionGM">Trilhas</a>
                        </div>
                        <div id="divTrilhas" class="panel-collapse collapse" runat="server" clientidmode="Static">
                            <div class="panel-body">
                                <uc:ucTrilhas ID="ucTrilhas1" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <a href="#divCursos" data-toggle="collapse" data-parent="#accordionGM">Soluções Educacionais</a>
                        </div>
                        <div id="divCursos" class="panel-collapse collapse" runat="server" clientidmode="Static">
                            <div class="panel-body">
                                <uc:ucCursos ID="ucCursos1" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <a href="#divAtividadeExtraCurricular" data-toggle="collapse" data-parent="#accordionGM">Atividades Extra Curricular</a>
                        </div>
                        <div id="divAtividadeExtraCurricular" class="panel-collapse collapse" runat="server"
                            clientidmode="Static">
                            <div class="panel-body">
                                <uc:ucAtividadeExtraCurricular ID="ucAtividadeExtraCurricular1" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <a href="#divPagamentos" data-toggle="collapse" data-parent="#accordionGM">Pagamentos</a>
                        </div>
                        <div id="divPagamentos" class="panel-collapse collapse" runat="server" clientidmode="Static">
                            <div class="panel-body">
                                <uc:ucHistoricoPagamento ID="ucHistoricoPagamento1" runat="server" OnVisualizouDetalheDoPagamento="VisualizarDetalheDoPagamento_VisualizouDetalheDoPagamento" />
                            </div>
                        </div>
                    </div>
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <a href="#divAbandono" data-toggle="collapse" data-parent="#accordionGM">Abandono</a>
                        </div>
                        <div id="divAbandono" class="panel-collapse collapse" runat="server" clientidmode="Static">
                            <div class="panel-body">
                                <uc:ucAbandono ID="ucAbandono1" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <a href="#divPublicacoes" data-toggle="collapse" data-parent="#accordionGM">Publicações do Saber</a>
                        </div>
                        <div id="divPublicacoes" class="panel-collapse collapse" runat="server" clientidmode="Static">
                            <div class="panel-body">
                                <uc:ucPublicacaoSaber ID="ucPublicacaoSaber1" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <a href="#divFaleConosco" data-toggle="collapse" data-parent="#accordionGM">Fale Conosco</a>
                        </div>
                        <div id="divFaleConosco" class="panel-collapse collapse" runat="server" clientidmode="Static">
                            <div class="panel-body">
                                <uc:ucFaleConosco ID="ucFaleConosco1" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="panel panel-default" id="pnlTutorias" runat="server">
                        <div class="panel-heading">
                            <a href="#divTutorias" data-toggle="collapse" data-parent="#accordionGM">Tutorias</a>
                        </div>
                        <div id="divTutorias" class="panel-collapse collapse" runat="server" clientidmode="Static">
                            <div class="panel-body">
                                <uc:ucTutorias ID="ucTutorias" runat="server" />
                            </div>
                        </div>
                    </div>

                </div>
                <asp:Button ID="btnHistorico" runat="server" OnClick="btnHistorico_Click" Text="Histórico acadêmico"
                    CssClass="btn btn-default" />
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlUcInformarPagamento" runat="server">
            <div class="modal fade in" id="ModalInformarPagamento" tabindex="-1" role="dialog"
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
                            <uc:ucInformarPagamento ID="ucInformarPagamento1" runat="server" OnInformouPagamento="InformarPagamento_InformouPagamento" />
                        </div>
                    </div>
                    <!-- /.modal-content -->
                </div>
                <!-- /.modal-dialog -->
            </div>
            <!-- /.modal -->
        </asp:Panel>
    </div>
</asp:Content>
