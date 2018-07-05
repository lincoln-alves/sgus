<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.Master" AutoEventWireup="true" CodeBehind="Paginas.aspx.cs" Inherits="Sebrae.Academico.WebForms.Paginas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        tr {
            -webkit-transition: background-color 1000ms linear;
            -moz-transition: background-color 1000ms linear;
            -o-transition: background-color 1000ms linear;
            -ms-transition: background-color 1000ms linear;
            transition: background-color 1000ms linear;
        }
    </style>

    <div style="margin: 0 10px">
        <h3>Gerenciamento de páginas
        </h3>
        <div class="panel panel-default">
            <div class="panel-body">
                <div class="form-group">
                    <button id="btnCadastrarMenu" onserverclick="btnCadastrarMenu_OnServerClick" runat="server" class="btn btn-default pull-left" data-toggle="modal" data-target="#modalCadastrarPagina" style="margin-right: 10px;"><span class="glyphicon glyphicon-plus"></span>&nbsp;&nbsp;Menu</button>

                    <div class="pull-right" style="width: 190px;">
                        <div class="pull-right">
                            <asp:DropDownList ID="ddlFiltroProfundidade" CssClass="form-control" runat="server" OnSelectedIndexChanged="ddlFiltroProfundidade_OnSelectedIndexChanged" AutoPostBack="True" Style="max-width: 140px;">
                                <asp:ListItem Text="-- Todos --" Value="0" />
                                <asp:ListItem Text="Menu" Value="1" />
                                <asp:ListItem Text="Agrupador" Value="2" />
                                <asp:ListItem Text="Página" Value="3" />
                                <asp:ListItem Text="Cadastro" Value="4" />
                                <asp:ListItem Text="Subcadastro" Value="5" />
                            </asp:DropDownList>
                        </div>
                        <div class="pull-left">
                            <p class="form-control-static">
                                <strong>Filtro:
                                </strong>
                            </p>
                        </div>
                    </div>

                </div>
            </div>
        </div>
        <div class="panel panel-default">
            <asp:GridView ID="gdvPaginas" OnRowCreated="gdvPaginas_OnRowCreated" runat="server" AutoGenerateColumns="False" CssClass="table" DataKeyNames="ID" EnableTheming="True" ShowHeader="False" BorderStyle="None" OnRowDataBound="gdvPaginas_OnRowDataBound">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <%--<%# "<span style=\"min-width: 100px; display: inline-block;\"><strong>" + Eval("Left") + " - " + Eval("Right") + "</strong></span>"%>--%>
                            <%# int.Parse(Eval("Profundidade").ToString()) >= 3 ? "<span style=\"position: relative;\"><span class=\"paginas-identacao\" style=\"margin-left: 30px\"></span></span>" : "" %>

                            <span <%# int.Parse(Eval("Profundidade").ToString()) == 2 ? "class=\"text-bold\"" : "" %> style="padding-left: <%# (int.Parse(Eval("Profundidade").ToString()) * 10) + (int.Parse(Eval("Profundidade").ToString()) == 2 ? 20 : 0) + (int.Parse(Eval("Profundidade").ToString()) == 4 ? 10 : 0)%>px">

                                <%# int.Parse(Eval("Profundidade").ToString()) == 1 ? "<strong>" : ""%>
                                <%#Eval("CaminhoRelativo") != null ? "<a href=" + Eval("CaminhoRelativo") + " target='blank'>" : ""%>
                                <%#Eval("IconeMenu") != null ?  "<span style=\"opacity: 0.5; margin-right: 5px;\" class=\" glyphicon glyphicon-" + Eval("IconeMenu") + "\"></span>" : ""%><%# int.Parse(Eval("Profundidade").ToString()) < 4 ? Eval("Nome") : ((int.Parse(Eval("Profundidade").ToString()) == 4 ? "Cadastro/edição" : "Subcadastro") + (Eval("Titulo") != null ? " de " + Eval("Titulo") : "" ))%>
                                <%#Eval("CaminhoRelativo") != null ? "</a>" : ""%>
                                <%# int.Parse(Eval("Profundidade").ToString()) == 1 ? "</strong>" : ""%>
                            </span>
                        </ItemTemplate>
                        <ItemStyle BorderStyle="None" />
                    </asp:TemplateField>

                    <asp:TemplateField>
                        <ItemTemplate>
                            <%# Eval("PaginaInicial") != null ? (bool.Parse(Eval("PaginaInicial").ToString()) ? "<span class=\"label label-danger\" style=\"line-height: 20px;\">Página inicial</span>" : "") : ""%>
                        </ItemTemplate>
                        <ItemStyle Width="30px" BorderStyle="None" />
                    </asp:TemplateField>

                    <asp:TemplateField>
                        <ItemTemplate>
                            <span style="line-height: 15px; width: 100px; position: absolute; margin-right: 15px;" class="label label-<%# 
                            int.Parse(Eval("Profundidade").ToString()) == 1 ? "primary" : int.Parse(Eval("Profundidade").ToString()) == 2 ? "success" : int.Parse(Eval("Profundidade").ToString()) == 3 ? "info" : int.Parse(Eval("Profundidade").ToString()) == 4 ? "warning" : int.Parse(Eval("Profundidade").ToString()) == 5 ? "danger" : "" %>">
                                <%# Eval("_ObterTipoPagina").ToString()%>
                            </span>
                        </ItemTemplate>
                        <ItemStyle BorderStyle="None" Width="120px" />
                    </asp:TemplateField>

                    <asp:TemplateField>
                        <ItemTemplate>
                            <%# int.Parse(Eval("_ObterQntFiltrosPerfil").ToString()) > 0 ? "<span class='badge' data-toggle='tooltip' data-title='" + Eval("_ObterPerfis") + "' data-placement='bottom'>" + (bool.Parse(Eval("TodosPerfis").ToString()) ? "" : Eval("_ObterQntFiltrosPerfil").ToString()) + "</span>" : ""%>
                        </ItemTemplate>
                        <ItemStyle Width="40px" CssClass="paginas-permissao" />
                    </asp:TemplateField>

                    <asp:TemplateField>
                        <ItemTemplate>
                            <%# int.Parse(Eval("_ObterQntFiltrosUf").ToString()) > 0 ? "<span class='badge' data-toggle='tooltip' data-title='" + Eval("_ObterUfs") + "' data-placement='bottom'>" + (bool.Parse(Eval("TodasUfs").ToString()) ? "" : Eval("_ObterQntFiltrosUf").ToString()) + "</span>" : ""%>
                        </ItemTemplate>
                        <ItemStyle Width="40px" CssClass="paginas-permissao" />
                    </asp:TemplateField>

                    <asp:TemplateField>
                        <ItemTemplate>
                            <%# int.Parse(Eval("_ObterQntFiltrosNivelOcupacional").ToString()) > 0 ? "<span class='badge' data-toggle='tooltip' data-title='" + Eval("_ObterNiveisOcupacionais") + "' data-placement='bottom'>" + (bool.Parse(Eval("TodosNiveisOcupacionais").ToString()) ? "" : Eval("_ObterQntFiltrosNivelOcupacional").ToString()) + "</span>" : ""%>
                        </ItemTemplate>
                        <ItemStyle Width="40px" CssClass="paginas-permissao" />
                    </asp:TemplateField>

                    <asp:TemplateField>
                        <ItemTemplate>
                            <%--<button runat="server" id="btnEditar" OnClientClick="return false" OnServerClick="btnEditar_OnServerClick" data-id='<%#Eval("ID")%>' data-profundidade='<%#Eval("Profundidade")%>' class="btn btn-default btn-xs" data-toggle="tooltip" data-title="Editar" data-placement="bottom">
                                <span class="glyphicon glyphicon-pencil"></span>
                            </button>--%>
                            <%--<asp:Button ID="btnEditar" OnClientClick="__doPostBack('btnEditar', '')" OnClick="btnEditar_OnServerClick" runat="server" Text="Button" />--%>
                            <button type="button" id="btnEditar" data-toggle="tooltip" data-title="Editar" data-placement="bottom" class="btn btn-default btn-xs" onclick="$('#<%#hdnEdicaoId.ClientID%>').val('<%#Eval("ID") %>');__doPostBack('<%#hdnEdicaoId.ClientID%>', '')">
                                <span class="glyphicon glyphicon-pencil"></span>
                            </button>
                        </ItemTemplate>
                        <ItemStyle Width="20px" />
                        <ItemStyle BorderStyle="None" />
                    </asp:TemplateField>

                    <asp:TemplateField>
                        <ItemTemplate>
                            <%-- O código abaixo está sujo por causa de limitações do WebForms. --%>
                            <%-- Inserir um <span> com tooltip ao redor do botão desabilitado e configura a mensagem com o motivo da desabilitação. --%>

                            <%--<%# (bool.Parse(Eval("_PossuiFilho").ToString()) || ((Eval("PaginaInicial") != null && bool.Parse(Eval("PaginaInicial").ToString())))) ? "<div data-toggle='tooltip' data-title='" + (bool.Parse(Eval("_PossuiFilho").ToString()) ? "Esta página possui filhos e não pode ser excluída" : (bool.Parse(Eval("PaginaInicial").ToString()) ? "Esta é a página inicial e não pode ser excluída" : "")) + "' data-container='body' data-placement='left'>" : "" %>
                            <button id="btnExcluir" type="button" onclick="$('#<%#hdnExcluir.ClientID%>').val('<%#Eval("ID") %>');__doPostBack('<%#hdnExcluir.ClientID%>', '') " class="btn btn-danger btn-xs" data-toggle="tooltip" data-title="Excluir" data-placement="bottom" <%#((bool.Parse(Eval("_PossuiFilho").ToString()) || (Eval("PaginaInicial") != null && bool.Parse(Eval("PaginaInicial").ToString()))) ? "disabled" : "")%>>
                                <span class="glyphicon glyphicon-remove" style="color: white"></span>
                            </button>--%>

                             <%# (bool.Parse(Eval("_PossuiFilho").ToString()) || ((Eval("PaginaInicial") != null && bool.Parse(Eval("PaginaInicial").ToString())))) ? "<div data-toggle='tooltip' data-title='" + (bool.Parse(Eval("_PossuiFilho").ToString()) ? "Esta página possui filhos e não pode ser excluída" : (bool.Parse(Eval("PaginaInicial").ToString()) ? "Esta é a página inicial e não pode ser excluída" : "")) + "' data-container='body' data-placement='left'>" : "" %>
                            <button type="button" id="btnExcluir" data-toggle="tooltip" data-title="Excluir" data-placement="bottom" class="btn btn-danger btn-xs" onclick="$('#<%#hdnExcluir.ClientID%>').val('<%#Eval("ID") %>');__doPostBack('<%#hdnExcluir.ClientID%>', '') " <%#((bool.Parse(Eval("_PossuiFilho").ToString()) || (Eval("PaginaInicial") != null && bool.Parse(Eval("PaginaInicial").ToString()))) ? "disabled" : "")%>>
                                <span class="glyphicon glyphicon-remove" style="color: white"></span>
                            </button>

                            <%-- Fechar o <span> do tooltip aberto acima. --%>
                            <%# (bool.Parse(Eval("_PossuiFilho").ToString()) || (Eval("PaginaInicial") != null && bool.Parse(Eval("PaginaInicial").ToString()))) ? "</div>" : "" %>
                        </ItemTemplate>
                        <ItemStyle Width="10px" />
                        <ItemStyle BorderStyle="None" />
                    </asp:TemplateField>

                    <asp:TemplateField>
                        <ItemTemplate>
                            <%# (int.Parse(Eval("Profundidade").ToString()) > 4)
                                || ((int.Parse(Eval("Profundidade").ToString()) == 3) && bool.Parse(Eval("_PossuiFilho").ToString()))
                                ? "<div data-toggle='tooltip' data-title='" 
                                    + (((int.Parse(Eval("Profundidade").ToString()) == 3) && bool.Parse(Eval("_PossuiFilho").ToString())) ? "Esta página já possui filhos" : ((int.Parse(Eval("Profundidade").ToString()) > 4) 
                                    ? "Não é possível adicionais mais níveis"
                                        : ""))
                                    + "' data-container='body' data-placement='left'>" : "" %>
                            <%--<button runat="server" id="btnCriarFilho" onserverclick="btnCriarFilho_OnServerClick" data-id='<%#Eval("ID")%>' data-profundidade='<%#Eval("Profundidade")%>' class="btn btn-primary btn-xs" data-toggle="tooltip" data-title="Filho" data-placement="bottom" disabled='<%#(int.Parse(Eval("Profundidade").ToString()) > 4) || ((int.Parse(Eval("Profundidade").ToString()) == 3) && bool.Parse(Eval("_PossuiFilho").ToString())) || (Eval("PaginaInicial") != null && bool.Parse(Eval("PaginaInicial").ToString()))%>'>
                                <span class="glyphicon glyphicon-plus" style="color: white"></span>
                            </button>--%>
                            <button type="button" id="btnCriarFilho" data-toggle="tooltip" data-title="Filho" data-placement="bottom" class="btn btn-primary btn-xs" data-profundidade='<%#Eval("Profundidade")%>' onclick="$('#<%#hdnProfundidade.ClientID%>').val('<%#Eval("Profundidade") %>');$('#<%#hdnCriarFilho.ClientID%>').val('<%#Eval("ID") %>');__doPostBack('<%#hdnCriarFilho.ClientID%>', '')" <%#((int.Parse(Eval("Profundidade").ToString()) > 4) || ((int.Parse(Eval("Profundidade").ToString()) == 3) && bool.Parse(Eval("_PossuiFilho").ToString()))) ? "disabled" : ""%>>
                                <span class="glyphicon glyphicon-plus" style="color: white"></span>
                            </button>
                            <%#(int.Parse(Eval("Profundidade").ToString()) > 4) || ((int.Parse(Eval("Profundidade").ToString()) == 3) && bool.Parse(Eval("_PossuiFilho").ToString())) || (Eval("PaginaInicial") != null && bool.Parse(Eval("PaginaInicial").ToString())) 
                                ? "</div>" : ""%>
                        </ItemTemplate>
                        <ItemStyle Width="20px" />
                        <ItemStyle BorderStyle="None" />
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle BorderColor="#DFDFDF" BorderStyle="None" />
                <RowStyle BorderColor="#DFDFDF" BorderStyle="None" />
            </asp:GridView>
        </div>

        <asp:HiddenField ID="hdnEdicaoId" runat="server" OnValueChanged="btnEditar_OnServerClick" />

        <asp:HiddenField ID="hdnExcluir" runat="server" OnValueChanged="btnExcluir_OnClick" />

        <asp:HiddenField ID="hdnCriarFilho" runat="server" OnValueChanged="btnCriarFilho_OnServerClick" />

        <asp:HiddenField ID="hdnProfundidade" runat="server"/>
        
        <asp:HiddenField ID="hdnMoverPagina" OnValueChanged="hdnMoverPagina_OnValueChanged" runat="server" />

        <asp:Panel ID="pnlCadastrarPagina" runat="server" Visible="False">
            <div class="modal fade in" id="modalCadastrarPagina" tabindex="-1" role="dialog" aria-labelledby="Cadastrar página" aria-hidden="true" style="display: block;">
                <div class="modal-dialog modal-lg" style="width: 85%" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button runat="server" type="button" class="close mostrarload" data-dismiss="modal" aria-label="Fechar" onserverclick="btnFecharModal_OnServerClick"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title" id="modalCadastrarPaginaTitulo" runat="server"></h4>
                        </div>
                        <div id="modalCadastrarPaginaBody" runat="server" class="modal-body">

                            <asp:HiddenField ID="hdnIdPaginaPai" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnIdPagina" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnTipoPagina" runat="server" Value="0" />

                            <div class="form-group" runat="server" id="divPaginaInicial" visible="False">
                                <div class="row">
                                    <div class="col-md-4 col-xs-12">
                                        <div class="panel panel-default" style="margin-bottom: 0">
                                            <div class="panel-body">
                                                <asp:CheckBox ID="ckbPaginaInicial" runat="server" Text="&nbsp;Página inicial" OnCheckedChanged="ckbPaginaInicial_OnCheckedChanged" AutoPostBack="True" CssClass="mostrarload" />
                                                &nbsp;&nbsp;<span class="glyphicon glyphicon-home"></span>
                                            </div>
                                        </div>
                                    </div>
                                    <div runat="server" id="divEstilo" class="col-md-4 col-xs-12" visible="False">
                                        <asp:Label ID="Label6" runat="server" Text="Estilo*" AssociatedControlID="ddlEstilo" />
                                        <asp:DropDownList ID="ddlEstilo" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="-- Selecione --" Value="" />
                                            <asp:ListItem Text="Azul" Value="home" />
                                            <asp:ListItem Text="Vermelho" Value="relatorios" />
                                            <asp:ListItem Text="Verde" Value="cadastro" />
                                            <asp:ListItem Text="Amarelo" Value="gerenciador" />
                                            <asp:ListItem Text="Roxo" Value="configuracoes" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group" runat="server" id="divNome">
                                <asp:Label ID="labelNome" runat="server" Text="Nome*" AssociatedControlID="txtNome" />
                                <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group" runat="server" id="divTitulo">
                                <asp:Label ID="labelTitulo" runat="server" Text="Título*" AssociatedControlID="txtTitulo" />
                                <asp:TextBox ID="txtTitulo" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group" runat="server" id="divCaminhoRelativo">
                                <asp:Label ID="Label3" runat="server" Text="Caminho relativo*" AssociatedControlID="txtCaminhoRelativo" />
                                <asp:TextBox ID="txtCaminhoRelativo" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label1" runat="server" Text="Descrição" AssociatedControlID="txtDescricao" />
                                <CKEditor:CKEditorControl ID="txtDescricao" BasePath="/js/ckeditor/" runat="server" Height="80px"></CKEditor:CKEditorControl>
                            </div>
                            <div class="form-group" runat="server" id="divDescricaoAdministrador" visible="false">
                                <asp:Label ID="Label2" runat="server" Text="Conteúdo Administrador" AssociatedControlID="txtDescricaoAdministrador" />
                                <CKEditor:CKEditorControl ID="txtDescricaoAdministrador" BasePath="/js/ckeditor/" runat="server" Height="80px"></CKEditor:CKEditorControl>
                            </div>
                            <div class="form-group" runat="server" id="divDescricaoGestor" visible="false">
                                <asp:Label ID="Label4" runat="server" Text="Conteúdo Gestor" AssociatedControlID="txtDescricaoGestor" />
                                <CKEditor:CKEditorControl ID="txtDescricaoGestor" BasePath="/js/ckeditor/" runat="server" Height="80px"></CKEditor:CKEditorControl>
                            </div>
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-md-4 col-xs-12" runat="server" id="divIconeMenu" visible="False">
                                        <div class="form-group">
                                            <asp:Label ID="labelIconeMenu" runat="server" Text="Ícone do Menu*" AssociatedControlID="hddIcone" />
                                            <asp:HiddenField ID="hddIcone" runat="server" Value="" ClientIDMode="Static" />
                                            <div class="dropdown">
                                                <button class="btn btn-default dropdown-toggle dropdown-toggle-icons" type="button" id="btnClassFont" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                                                    <span class="nome">- Selecione -</span>
                                                    <span class="caret caret-black"></span>
                                                </button>
                                                <ul class="dropdown-menu dropdown-menu-icons scrollable-menu" aria-labelledby="btnClassFont">
                                                    <li><a href="javascript:void(0)" data-icon=""><span class="glyphicon"></span>- Nenhum -</a></li>
                                                    <li><a href="javascript:void(0)" data-icon="plus"><span class="glyphicon glyphicon-plus"></span>plus</a></li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-xs-12" runat="server" id="divChaveVerificadora" visible="False">
                                        <div class="form-group">
                                            <asp:Label ID="labelChaveVerificadora" runat="server" Text="Chave verificadora*" AssociatedControlID="txtChaveVerificadora" data-help="<%$ Resources:Resource, paginaChaveVerificadora %>" />
                                            <asp:TextBox ID="txtChaveVerificadora" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-xs-12" runat="server" id="divIconePaginas" visible="False">
                                        <div class="form-group">
                                            <asp:Label ID="Label5" runat="server" Text="Ícone padrão*" AssociatedControlID="txtIconePaginas" />
                                            <asp:TextBox ID="txtIconePaginas" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-md-4 col-xs-12">
                                        <div class="panel panel-default">
                                            <div class="panel-body">
                                                <asp:CheckBox ID="ckbTodosPerfis" runat="server" Text="&nbsp;Todos os perfis" Checked="True" AutoPostBack="True" CssClass="mostrarload" OnCheckedChanged="ckbTodosPerfis_OnCheckedChanged" />
                                            </div>
                                            <div runat="server" id="divPerfis" class="panel-body" visible="False">
                                                <asp:CheckBoxList ID="ckblPerfis" runat="server"></asp:CheckBoxList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-xs-12">
                                        <div class="panel panel-default">
                                            <div class="panel-body">
                                                <div class="form-group">
                                                    <asp:CheckBox ID="ckbConsiderarNacionalizacaoUf" runat="server" Text="&nbsp;Considerar Nacionalização das UFs" Checked="True" data-help="<%$ Resources:Resource, considerarNacionalizacaoUf %>" />
                                                </div>
                                                <div class="form-group">
                                                    <asp:CheckBox ID="ckbTodasUfs" runat="server" Text="&nbsp;Todas as UFs" Checked="True" AutoPostBack="True" CssClass="mostrarload" OnCheckedChanged="ckbTodasUfs_OnCheckedChanged" />
                                                </div>
                                            </div>
                                            <div runat="server" id="divUfs" class="panel-body" visible="False">
                                                <asp:CheckBoxList ID="ckblUfs" runat="server"></asp:CheckBoxList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-xs-12">
                                        <div class="panel panel-default">
                                            <div class="panel-body">
                                                <asp:CheckBox ID="ckbTodosNiveisOcupacionais" runat="server" Text="&nbsp;Todos os níveis ocup." Checked="True" AutoPostBack="True" CssClass="mostrarload" OnCheckedChanged="ckbTodosNiveisOcupacionais_OnCheckedChanged" />
                                            </div>
                                            <div runat="server" id="divNiveisOcupacionais" class="panel-body" visible="False">
                                                <asp:CheckBoxList ID="cbklNiveisOcupacionais" runat="server"></asp:CheckBoxList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default mostrarload" runat="server" id="btnFecharModal" onserverclick="btnFecharModal_OnServerClick">Fechar</button>
                            <button type="button" class="btn btn-primary mostrarload" runat="server" id="btnSalvarModal" onserverclick="btnSalvarModal_OnServerClick">Salvar</button>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <script type="text/javascript">
            (function ($) {
                if (!$) {
                    console.log('no Jquery!');
                    return;
                }
                $(document).ready(function () {
                    var btn = $('#btnClassFont');
                    var hdClassFont = $('#hddIcone');
                    $('.dropdown-menu-icons a').on('click', function () {
                        var item = $(this);
                        btn.html(item.html() + ' <span class="caret caret-black"></span>');
                        hdClassFont.val(item.attr('data-icon'));
                    });
                    if (hdClassFont.val() !== "") {
                        btn.html('<span class="icone ' + hdClassFont.val() + ' icone-fonte-20"></span> ' + hdClassFont.val() + ' <span class="caret caret-black"></span>');
                    }
                });
            })(jQuery);
        </script>
        <script>
            (function ($) {
                $.fn.goTo = function (offset) {
                    $('html, body').animate({
                        scrollTop: ($(this).offset().top - offset) + 'px'
                    }, 'fast');
                    return this; // for chaining...
                }
            })(jQuery);

            $('[data-toggle="tooltip"]').tooltip();

            function getParameterByName(name) {
                name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
                var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                    results = regex.exec(location.search);
                return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
            }
            
            function SetarIdsPais() {
                var tabela = document.getElementById('<%= gdvPaginas.ClientID %>');

                var ultimoAgrupador = 0;

                for (var i = 0; i < tabela.rows.length; i++) {
                    var row = tabela.rows[i];

                    if (row.dataset.profundidade != undefined) {
                        var prof = parseInt(row.dataset.profundidade);
                        var id = parseInt(row.dataset.id);

                        if (prof === 2)
                            ultimoAgrupador = id;

                        if (prof === 3) {
                            $(row).attr('data-pai', ultimoAgrupador);
                        }
                    }
                }
            }

            $(function () {

                var id = getParameterByName('id');

                // Remove o valor de ID da queryString para evitar scrollToElements indesejados.
                window.history.pushState('page2', null, '/Paginas.aspx');

                if (id != undefined && id !== "") {
                    var row = $('#pagina-' + id);
                    row.goTo(70);

                    row.css("border-color", "rgb(245, 180, 87)");
                    row.css("background-color", "rgb(245, 180, 87)");

                    var rows = row.parent()[0].rows;

                    var ct = 0;

                    // Verifica a cor original do fundo de acordo com a quantidade de rows
                    if (rows != undefined) {

                        // Índice começa com 1 por causa do cabeçalho da tabela.
                        for (var i = 1; i < rows.length; i++) {
                            if (rows[i].children[0] != undefined) {
                                var loopRow = rows[i];

                                if (loopRow != undefined) {

                                    ct++;

                                    if (loopRow.id === 'pagina-' + id) {
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    setTimeout(function () {
                        // Seta a cor correta do fundo de acordo com o stripe da tabela
                        if (ct % 2 === 0) {
                            row.css("background-color", "#eee");
                        } else {
                            row.css("background-color", "#fff");
                        }

                        row.css("border-color", "rgb(223, 223, 223)");
                    }, 2000);
                }

                // Setar o id do pai via Javascript para evitar mais consultas no banco.
                SetarIdsPais();

                var indiceOriginal;

                $("#<%= gdvPaginas.ClientID %>").tableDnD({
                    onDrop: function (table, row) {

                        var newIndex = [].slice.call(table.rows).indexOf(row);

                        if (indiceOriginal === newIndex)
                            alert('Nenhum movimento detectado');
                        else {
                            // Pega o id do pai para onde está sendo movido. Caso o pai não possua filhos, pega o id
                            // do próprio pai para comparar.
                            var paiMovimento =
                                table.rows[newIndex - 1].dataset.pai != undefined
                                    ? table.rows[newIndex - 1].dataset.pai
                                    : (table.rows[newIndex + 1].dataset.pai != undefined
                                        ? table.rows[newIndex + 1].dataset.pai
                                        : table.rows[newIndex - 1].dataset.id);

                            // Se estiver movendo para outro pai, não permite o movimento.
                            if (paiMovimento != undefined && paiMovimento !== row.dataset.pai) {
                                $(row).addClass('danger');
                                $(row).addClass('pagina-draggable');
                            } else {
                                window.MostarLoading();

                                // Salvar em um hidden e dar postback.
                                $(row).removeClass('pagina-draggable');

                                // Obter todos os filhos
                                var filhos = $('[data-pai="' + row.dataset.pai + '"]');

                                // Obter o índice da página dentro do pai.
                                var subIndex = 0;
                                for (var p = 0; p < filhos.length; p++) {
                                    if (filhos[p].dataset.id === row.dataset.id) {
                                        break;
                                    }

                                    subIndex++;
                                }

                                // Setar o ID da página e o novo índice no hidden, separados por vírgula.
                                $('#<%= hdnMoverPagina.ClientID %>').val(row.dataset.id + ',' + subIndex);

                                __doPostBack('<%= hdnMoverPagina.ClientID %>');
                            }
                        }
                    },
                    onDragClass: "pagina-draggable",
                    onDragStart: function (table, row) {
                        // Incluir CSS que desabilita páginas menores para não atrabalhar na visualização.
                        $('tr:not([data-pai="' + row.dataset.pai + '"])').addClass('pagina-deny');

                        // Salvar o índice original antes do drag.
                        indiceOriginal = [].slice.call(table.rows).indexOf(row);
                    },
                    onDragStop: function () {
                        // Remover CSS que desabilita páginas menores caso o usuário não tenha arrastado nada.
                        $('.pagina-deny').removeClass('pagina-deny');
                    },
                    onAllowDrop: function (draggedRow, row) {
                        if (draggedRow[0].dataset.pai === (row.dataset.pai !== undefined ? row.dataset.pai : row.dataset.id)) {
                            $(draggedRow).removeClass('danger');
                        } else {
                            $(draggedRow).addClass('danger');
                        }

                        return true;
                    }
                });
            });
        </script>
    </div>
</asp:Content>
