<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="EdicaoPontoSebrae.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.PontoSebrae.EdicaoPontoSebrae" %>

<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <div class="form-group">
            <asp:Label ID="Label1" runat="server" Text="Trilha" AssociatedControlID="ddlTrilha" />
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip1" Chave="ddlTrilha" />
            <asp:DropDownList ID="ddlTrilha" CssClass="form-control mostrarload" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTrilha_OnSelectedIndexChanged"></asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Label ID="Label3" runat="server" Text="Nível" AssociatedControlID="ddlTrilhaNivel" />
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip2" Chave="ddlTrilhaNivel" />
            <asp:DropDownList ID="ddlTrilhaNivel" CssClass="form-control" runat="server"></asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Label ID="Label2" runat="server" Text="Nome" AssociatedControlID="txtNome" />
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip3" Chave="txtNome" />
            <asp:TextBox ID="txtNome" runat="server" CssClass="form-control" MaxLength="450"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label4" runat="server" Text="Nome Exibição" AssociatedControlID="txtNomeExibicao" />
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip4" Chave="txtNomeExibicao" />
            <asp:TextBox ID="txtNomeExibicao" runat="server" CssClass="form-control" MaxLength="450"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label5" runat="server" Text="Qt. Mínima de Pontos" AssociatedControlID="txtQtMinimaPontos" />
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip5" Chave="txtQtMinimaPontos" />
            <asp:TextBox ID="txtQtMinimaPontos" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label runat="server" Text="Ativo" AssociatedControlID="ddlAtivo"></asp:Label>
            <asp:DropDownList runat="server" ID="ddlAtivo" CssClass="form-control" OnSelectedIndexChanged="ddlAtivo_OnSelectedIndexChanged" AutoPostBack="True" />
        </div>
        <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click"
            CssClass="btn btn-primary mostrarload" />
        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click"
            CssClass="btn btn-default" />
    </fieldset>


    <!-- MODAL -->
    <asp:Panel ID="pnlModal" runat="server" Visible="false">
        <div aria-hidden="false" aria-labelledby="myModalLabel" data-toggle="modal" role="dialog" tabindex="-1" id="myModal" class="modal fade in" style="display: block;">
            <div class="modal-dialog modal-sm" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <%--<button type="button" class="close" data-dismiss="modal" aria-hidden="true" runat="server"
                                onserverclick="OcultarModal_Click" id="btnFecharModal">
                            &times;</button>--%>
                        <h4 class="modal-title align-center">Confirmar Desativação</h4>
                    </div>
                    <div class="modal-body">
                        <div class="panel-group">
                            <div class="alert alert-danger" role="alert">
                                Existem
                                <asp:Label runat="server" ID="totalItemTrilha"></asp:Label>
                                <em>Soluções Sebrae</em> vinculadas ao <em>Ponto Sebrae</em>
                            </div>
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <a role="button" data-toggle="collapse"
                                        href="#collapseSolucoes" aria-expanded="false" aria-controls="collapseExample">Soluções Vinculadas</a>
                                </div>
                                <div class="panel-body collapse" id="collapseSolucoes">
                                    <ul class="list-group">
                                        <asp:Repeater runat="server" ID="rptItemTrilha">
                                            <ItemTemplate>
                                                <li class="list-group-item">
                                                    <%# Eval("Nome") %>
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>
                                </div>
                            </div>

                            <p class="alert-status-demanda">Deseja continuar com a alteração?</p>

                        </div>
                    </div>
                    <div class="modal-footer center">
                        <button type="button" runat="server" onserverclick="btnCancelarAlteracao_OnClick" id="btnNao" class="btn btn-default">Não</button>
                        <button type="button" runat="server" onserverclick="btnAutorizarAlteracao_OnClick" id="btnSim" class="btn btn-primary">Sim</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->
    </asp:Panel>

    <script>
        validarForm();
    </script>
</asp:Content>
