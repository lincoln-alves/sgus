<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="GerenciarHierarquia.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.Hierarquia.GerenciarHierarquia" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <h1 class="text-center">
            <%= UfSelecionada.Nome %>
        </h1>
        <hr />
        <div class="panel panel-default">
            <div class="panel-body">
                
                <button runat="server" class="btn btn-danger mostrarload" type="submit" onserverclick="Voltar_OnServerClick"><span class="glyphicon glyphicon-chevron-left"></span>&nbsp;Voltar</button>
                
                <div class="pull-right" role="group" aria-label="...">
                    <button class="btn btn-success" style="margin-right:5px" type="button" id="btnAdicionarUnidade" runat="server" onserverclick="btnAdicionarDiretoria_OnClick">
                        <span class="glyphicon glyphicon-plus"></span>&nbsp;Adicionar Diretoria
                    </button>

                    <button class="btn btn-default" type="button" id="botaoExpandir" onclick="ExpandirTodos()"><span class="glyphicon glyphicon-plus"></span>&nbsp;Expandir todos</button>
                    <button class="btn btn-default" style="display:none" id="botaoOcultar" type="button" onclick="OcultarTodos()"><span class="glyphicon glyphicon-minus"></span>&nbsp;Ocultar todos</button>
                </div>                
            </div>
        </div>
        
        <asp:HiddenField ID="hfOrdemDiretoria" ClientIDMode="Static" runat="server" />
        <asp:LinkButton ID="lkbOrdenar" runat="server" Visible="true" onclick="btnOrdenarDiretoria_OnClick"></asp:LinkButton>
        <div class="row dropBox div-diretoria-style">
            <asp:Repeater ID="rptDiretoria" runat="server" OnItemDataBound="rptDiretoria_OnItemDataBound">
                <ItemTemplate>
                    <div class="col-xs-12 col-md-4 div-col-diretoria dragBox" id="<%#DataBinder.Eval(Container,"DataItem.ID")%>">
                        <div class="panel panel-default">
                            <asp:Panel CssClass="panel-heading text-center" runat="server" >
                                <h3 class="panel-title">
                                    <span class="pull-left"><i class="glyphicon glyphicon-move"></i></span>
                                    <strong>
                                        <asp:Literal ID="ltrCargoDiretoria" runat="server"></asp:Literal>
                                    </strong>
                                    <a href="#" class="pull-right clearfix" ID="btnEditarDiretoria" runat="server" onserverclick="btnEditarDiretoria_OnServerClick"><i class="glyphicon glyphicon-pencil"></i></a>
                                </h3>
                            </asp:Panel>
                            <asp:Panel runat="server" class="panel-body" id="divHeader">
                                <div class="panel panel-default">
                                    <div class="panel-heading text-center">
                                        <strong>
                                            Diretor
                                        </strong>
                                        <button runat="server" id="btnAdicionarDiretor" type="submit" class="btn btn-success btn-sm mostrarload pull-right" data-toggle="tooltip" data-placement="bottom" title="Incluir" onserverclick="btnAdicionarDiretor_OnServerClick">
                                            <span class="glyphicon glyphicon-plus"></span>
                                        </button>
                                    </div>
                                    <div class="panel-body">
                                        <asp:Repeater ID="rptDiretores" runat="server" OnItemDataBound="rptCargo_OnItemDataBound">
                                            <ItemTemplate>
                                                <p>
                                                    <button runat="server" id="btnRemoverUsuario" type="submit" class="btn btn-danger btn-sm mostrarload" data-toggle="tooltip" data-placement="bottom" title="Remover" onserverclick="btnRemoverUsuario_OnServerClick">
                                                        <span class="glyphicon glyphicon-remove"></span>
                                                    </button>
                                                    &nbsp;<%#DataBinder.Eval(Container,"DataItem.Usuario.Nome") %>
                                                </p>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <asp:Literal ID="ltrDiretorVazio" runat="server" Text="Nenhum" Visible="False"></asp:Literal>
                                    </div>
                                </div>
                                
                                <!-- INICIO -->
                                <div class="panel panel-info backgroundGabinte">
                                    <div class="panel-heading clickable text-center collapsed" role="button" data-toggle="collapse" href="#collapseGerencia-<%#DataBinder.Eval(Container,"DataItem.ID") %>">
                                        <asp:Literal ID="ltrChefeGabinete" runat="server"></asp:Literal>
                                    </div>
                                    <div id="collapseGerencia-<%#DataBinder.Eval(Container,"DataItem.ID") %>" class="panel-collapse collapse" role="tabpanel">
                                        <div class="panel-body">
                                            <div class="panel panel-default">
                                                <div class="panel-heading">
                                                    Chefe de Gabinete
                                                    <button runat="server" id="btnAdicionarChefeGabinete" type="submit" class="btn btn-success btn-sm mostrarload pull-right" data-toggle="tooltip" data-placement="bottom" title="Incluir" onserverclick="btnAdicionarChefeGabinete_OnServerClick">
                                                        <span class="glyphicon glyphicon-plus"></span>
                                                    </button>
                                                </div>
                                                <div class="panel-body">
                                                    <asp:Repeater ID="rptChefesGabinete" runat="server" OnItemDataBound="rptCargo_OnItemDataBound">
                                                        <ItemTemplate>
                                                            <p>
                                                                <button runat="server" id="btnRemoverUsuario" type="submit" class="btn btn-danger btn-sm mostrarload" data-toggle="tooltip" data-placement="bottom" title="Remover" onserverclick="btnRemoverUsuario_OnServerClick">
                                                                    <span class="glyphicon glyphicon-remove"></span>
                                                                </button>
                                                                &nbsp;<%#DataBinder.Eval(Container,"DataItem.Usuario.Nome") %>
                                                            </p>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                    <asp:Literal ID="ltrChefeGabineteVazio" runat="server" Text="Nenhum" Visible="False"></asp:Literal>
                                                </div>
                                            </div>
                                            <div class="panel panel-default">
                                                <div class="panel-heading">
                                                    Funcionários
                                                    <button runat="server" id="btnAdicionarFuncionario" type="submit" class="btn btn-success btn-sm mostrarload pull-right" data-toggle="tooltip" data-placement="bottom" title="Incluir" onserverclick="btnAdicionarFuncionario_OnServerClick">
                                                        <span class="glyphicon glyphicon-plus"></span>
                                                    </button>
                                                </div>
                                                <div class="panel-body">
                                                    <asp:Repeater ID="rptFuncionarios" runat="server" OnItemDataBound="rptCargo_OnItemDataBound">
                                                        <ItemTemplate>
                                                            <p>
                                                                <button runat="server" id="btnRemoverUsuario" type="submit" class="btn btn-danger btn-sm" data-toggle="tooltip" data-placement="bottom" title="Remover" onserverclick="btnRemoverUsuario_OnServerClick">
                                                                    <span class="glyphicon glyphicon-remove"></span>
                                                                </button>
                                                                &nbsp;<%#DataBinder.Eval(Container,"DataItem.Usuario.Nome") %>
                                                            </p>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                    <asp:Literal ID="ltrFuncionarioVazio" runat="server" Text="Nenhum" Visible="False"></asp:Literal>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <asp:Repeater ID="rptUnidade" runat="server" OnItemDataBound="rptUnidade_OnItemDataBound">
                                    <ItemTemplate>
                                        <div class="panel panel-info">
                                            <div class="panel-heading clickable text-center collapsed" role="button" data-toggle="collapse" href="#collapseGerencia-<%#DataBinder.Eval(Container,"DataItem.ID") %>" aria-expanded="false" aria-controls="collapseGerencia-<%#DataBinder.Eval(Container,"DataItem.ID") %>">
                                                <%#DataBinder.Eval(Container,"DataItem.Nome") %>
                                            </div>
                                            <div id="collapseGerencia-<%#DataBinder.Eval(Container,"DataItem.ID") %>" class="panel-collapse collapse" role="tabpanel">
                                                <div class="panel-body">
                                                    <div class="panel panel-default">
                                                        <div class="panel-heading">
                                                            Gerentes
                                                            <button runat="server" id="btnAdicionarGerente" type="submit" class="btn btn-success btn-sm mostrarload pull-right" data-toggle="tooltip" data-placement="bottom" title="Incluir" onserverclick="btnAdicionarGerente_OnServerClick">
                                                                <span class="glyphicon glyphicon-plus"></span>
                                                            </button>
                                                        </div>
                                                        <div class="panel-body">
                                                            <asp:Repeater ID="rptGerentes" runat="server" OnItemDataBound="rptCargo_OnItemDataBound">
                                                                <ItemTemplate>
                                                                    <p>
                                                                        <button runat="server" id="btnRemoverUsuario" type="submit" class="btn btn-danger btn-sm mostrarload" data-toggle="tooltip" data-placement="bottom" title="Remover" onserverclick="btnRemoverUsuario_OnServerClick">
                                                                            <span class="glyphicon glyphicon-remove"></span>
                                                                        </button>
                                                                        &nbsp;<%#DataBinder.Eval(Container,"DataItem.Usuario.Nome") %>
                                                                    </p>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                            <asp:Literal ID="ltrGerenteVazio" runat="server" Text="Nenhum" Visible="False"></asp:Literal>
                                                        </div>
                                                    </div>
                                                   
                                                    <div class="panel panel-default">
                                                        <div class="panel-heading">
                                                            Gerentes adjuntos
                                                            <button runat="server" id="btnAdicionarGerenteAdjunto" type="submit" class="btn btn-success btn-sm mostrarload pull-right" data-toggle="tooltip" data-placement="bottom" title="Incluir" onserverclick="btnAdicionarGerenteAdjunto_OnServerClick">
                                                                <span class="glyphicon glyphicon-plus"></span>
                                                            </button>
                                                        </div>
                                                        <div class="panel-body">
                                                            <asp:Repeater ID="rptGerentesAdjuntos" runat="server" OnItemDataBound="rptCargo_OnItemDataBound">
                                                                <ItemTemplate>
                                                                    <p>
                                                                        <button runat="server" id="btnRemoverUsuario" type="submit" class="btn btn-danger btn-sm mostrarload" data-toggle="tooltip" data-placement="bottom" title="Remover" onserverclick="btnRemoverUsuario_OnServerClick">
                                                                            <span class="glyphicon glyphicon-remove"></span>
                                                                        </button>
                                                                        &nbsp;<%#DataBinder.Eval(Container,"DataItem.Usuario.Nome") %>
                                                                    </p>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                            <asp:Literal ID="ltrGerenteAdjuntoVazio" runat="server" Text="Nenhum" Visible="False"></asp:Literal>
                                                        </div>
                                                    </div>
                                                    <div class="panel panel-default">
                                                        <div class="panel-heading">
                                                            Funcionários
                                                            <button runat="server" id="btnAdicionarFuncionario" type="submit" class="btn btn-success btn-sm mostrarload pull-right" data-toggle="tooltip" data-placement="bottom" title="Incluir" onserverclick="btnAdicionarFuncionario_OnServerClick">
                                                                <span class="glyphicon glyphicon-plus"></span>
                                                            </button>
                                                        </div>
                                                        <div class="panel-body">
                                                            <asp:Repeater ID="rptFuncionarios" runat="server" OnItemDataBound="rptCargo_OnItemDataBound">
                                                                <ItemTemplate>
                                                                    <p>
                                                                        <button runat="server" id="btnRemoverUsuario" type="submit" class="btn btn-danger btn-sm" data-toggle="tooltip" data-placement="bottom" title="Remover" onserverclick="btnRemoverUsuario_OnServerClick">
                                                                            <span class="glyphicon glyphicon-remove"></span>
                                                                        </button>
                                                                        &nbsp;<%#DataBinder.Eval(Container,"DataItem.Usuario.Nome") %>
                                                                    </p>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                            <asp:Literal ID="ltrFuncionarioVazio" runat="server" Text="Nenhum" Visible="False"></asp:Literal>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <!-- FIM -->
                            </asp:Panel>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <asp:Panel ID="pnlModal" runat="server" Visible="false">
            <div aria-hidden="false" aria-labelledby="myModalLabel" role="dialog" tabindex="-1"
                id="myModal" class="modal fade in" style="display: block;">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close mostrarload" data-dismiss="modal" aria-hidden="true" runat="server"
                                onserverclick="OcultarModal_Click" id="btnFecharModal">
                                &times;</button>
                            <h4 class="modal-title">
                                <asp:Literal ID="ltrTituloModal" runat="server"></asp:Literal>
                            </h4>
                        </div>
                        <div class="modal-body" style="min-height: 500px;">
                            <fieldset>
                                <asp:HiddenField ID="hdnCargoId" runat="server" />
                                <div class="form-group">
                                    <asp:Label ID="Label1" runat="server" AssociatedControlID="txtUfModal" Text="UF"></asp:Label>
                                    <asp:TextBox ID="txtUfModal" ClientIDMode="Static" runat="server" Enabled="False" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="lblCargoModal" runat="server" AssociatedControlID="txtCargoModal" Text="Cargo"></asp:Label>
                                    <asp:TextBox ID="txtCargoModal" ClientIDMode="Static" runat="server" Enabled="False" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <uc:LupaUsuario ID="LupaUsuario" runat="server" />
                                </div>
                            </fieldset>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_OnClick" CssClass="btn btn-primary pull-left mostrarload" />
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="OcultarModal_Click" CssClass="btn btn-default pull-right mostrarload" />
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
                
        <asp:Panel ID="pnlModalDiretoria" runat="server" Visible="false">
            <div aria-hidden="false" aria-labelledby="myModalLabel" role="dialog" tabindex="-1" id="modalDiretoria" class="modal fade in" style="display: block;">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close mostrarload" data-dismiss="modal" aria-hidden="true" runat="server" onserverclick="OcultarModalUnidade_Click" id="Button1">&times;</button>
                            <h4 class="modal-title">
                                <asp:Literal ID="ltrTituloModalUnidade" runat="server"></asp:Literal>
                            </h4>
                        </div>
                        <div class="modal-body" style="min-height: 500px;">
                            <fieldset>
                                <asp:HiddenField ID="idCargoPai" runat="server" Value="0" />
                                <asp:HiddenField ID="idCargo" runat="server" Value="0" />
                                <div class="form-group">
                                    <asp:Label ID="Label2" runat="server" AssociatedControlID="txtTitulo" Text="Título*"></asp:Label>
                                    <asp:TextBox ID="txtTitulo" ClientIDMode="Static" runat="server" MaxLength="255" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="Label3" runat="server" AssociatedControlID="txtSigla" Text="Sigla"></asp:Label>
                                    <asp:TextBox ID="txtSigla" ClientIDMode="Static" runat="server" MaxLength="100" CssClass="form-control"></asp:TextBox>
                                </div>
                                <asp:Panel ID="dvGabinete" runat="server" Visible="true" CssClass="form-group">
                                    <asp:Label ID="Label4" runat="server" AssociatedControlID="txtGabinete" Text="Gabinete*"></asp:Label>
                                    <asp:TextBox ID="txtGabinete" ClientIDMode="Static" runat="server" MaxLength="255" CssClass="form-control"></asp:TextBox>
                                </asp:Panel>
                                <asp:Panel ID="dvStatus" runat="server" CssClass="form-group" Visible="true">
                                    <asp:Label ID="Label5" runat="server" AssociatedControlID="rblStatus" Text="Status*"></asp:Label>
                                    <asp:RadioButtonList ID="rblStatus" runat="server" RepeatDirection="Horizontal" CssClass="form-control">
                                        <asp:ListItem Text="Ativo" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Inativo" Value="0"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </asp:Panel>
                                <asp:Panel ID="dvListaUnidades" runat="server" Visible="true">
                                    <div class="row">
                                        <div class="col-sm-1">&nbsp;</div>
                                        <div class="col-sm-10"><strong>Unidade</strong></div>
                                        <div class="col-sm-1"><strong>Ação</strong></div>
                                    </div>
                                     <div class="row">
                                        <div class="col-sm-12">
                                            <asp:HiddenField ID="hfOrdemUnidades" ClientIDMode="Static" runat="server" />
                                            <asp:LinkButton ID="lkbOrdemUnidades" runat="server" Visible="true" onclick="btnOrdenarUnidades_OnClick"></asp:LinkButton>
                                            <ul class="list-group unidadeDrop">
                                                <asp:Repeater ID="rptUnidadeDiretoria" runat="server" OnItemDataBound="rptUnidadeDiretoria_OnItemDataBound">
                                                    <ItemTemplate>
                                                        <li class="list-group-item clearfix" id="<%#DataBinder.Eval(Container,"DataItem.ID") %>">
                                                            <div class="col-sm-1"><i class="glyphicon glyphicon-move"></i></div>
                                                            <div class="col-sm-10"><%#DataBinder.Eval(Container,"DataItem.Nome") %></div>
                                                            <div class="col-sm-1 text-center">
                                                                <a href="#" id="btnEditarUnidade" runat="server" onserverclick="btnEditarUnidade_OnServerClick"><i class="glyphicon glyphicon-pencil"></i></a>
                                                                &nbsp;&nbsp;<a href="javascript:;" onclick="if(!confirm('Deseja Excluir Unidade')) return false;" id="btnExcluirUnidade" onserverclick="btnExcluirUnidade_OnServerClick" runat="server" ><i class="glyphicon glyphicon-trash"></i></a>
                                                            </div>
                                                        </li>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </ul>
                                        </div>
                                    </div>
                                    <button ID="Button4" runat="server" onserverclick="btnAdicionarUnidade_OnServerClick" class="btn btn-success mostrarload">
                                        <span class="glyphicon glyphicon-plus"></span>&nbsp;Adicionar Unidade
                                    </button>
                                </asp:Panel>
                            </fieldset>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnSalvarDiretoria" runat="server" Text="Salvar" OnClick="btnSalvarDiretoria_OnClick" Visible="true" Enabled="false" CssClass="btn btn-primary pull-left mostrarload" />
                            <asp:Button ID="btnSalvarUnidade" runat="server" Text="Salvar" OnClick="btnSalvarUnidade_OnClick" Visible="false" Enabled="false" CssClass="btn btn-primary pull-left mostrarload" />
                            <asp:Button ID="Button3" runat="server" Text="Cancelar" OnClick="OcultarModalUnidade_Click" CssClass="btn btn-default pull-right mostrarload" />
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>

        <asp:Panel ID="pnlAviso" runat="server" Visible="false">
            <div aria-hidden="false" aria-labelledby="myModalLabel" role="dialog" tabindex="-1"
                id="pnlAvisoDiv" class="modal fade in" style="display: block;">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close mostrarload" data-dismiss="modal" aria-hidden="true" runat="server"
                                onserverclick="OcultarModalVerificacao_Click" id="Button5">
                                &times;</button>
                            <h4 class="modal-title">
                                Atenção
                            </h4>
                        </div>
                        <div class="modal-body" style="min-height: 500px;">
                            <div runat="server" ID="divColaboradorEmOutrosCargos" Visible="False" class="form-group">
                                O Colaborador <asp:Literal ID="ltrNomeColaboradorEmOutrosCargos" runat="server"></asp:Literal> já está no(s) cargo(s) <strong><asp:Literal ID="ltrColaboradorEmOutrosCargos" runat="server"></asp:Literal></strong> e será movido.
                            </div>

                            <div runat="server" ID="divColaboradorEmOutrosCargosRepetiveis" class="form-group">
                                O Colaborador <asp:Literal ID="ltrNomeColaboradorEmOutrosCargosRepetiveis" runat="server"></asp:Literal> pertence a outra Diretoria e/ou Nível Hierárquico. Deseja duplicar ou mover?
                            </div>
                            
                            <div runat="server" ID="divNovasSolicitacoesDisponiveis" Visible="False" class="form-group">
                                A(s) solicitação(ões) <strong><asp:Literal ID="ltrNovasSolicitacoesDisponiveis" runat="server"></asp:Literal></strong> passa(m) a ter como responsável por aprovação o(as) novos chefes imediatos.
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnMover" runat="server" Text="Salvar" OnClick="btnMover_OnClick" CssClass="btn btn-primary pull-left mostrarload" />
                            <asp:Button ID="btnDuplicar" runat="server" Text="Duplicar" Visible="false" OnClick="btnDuplicar_OnClick" CssClass="btn btn-primary pull-left mostrarload" />
                            <asp:Button ID="Button7" runat="server" Text="Cancelar" OnClick="OcultarModalVerificacao_Click" CssClass="btn btn-default pull-right mostrarload" />
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>

        <asp:Panel ID="pnlAvisoExclusao" runat="server" Visible="false">
            <div aria-hidden="false" aria-labelledby="myModalLabel" role="dialog" tabindex="-1" id="pnlAvisoExclusaoDiv" class="modal fade in" style="display: block;">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close mostrarload" data-dismiss="modal" aria-hidden="true" runat="server"
                                onserverclick="OcultarModalExclusao_Click" id="Button8">
                                &times;</button>
                            <h4 class="modal-title">
                                Deseja remover <asp:Literal ID="ltrCargoRemover" runat="server"></asp:Literal>?
                            </h4>
                        </div>
                        <div class="modal-body" style="min-height: 500px;">
                            <asp:HiddenField ID="hdnUsuarioCargoIdRemover" runat="server" />
                            <div class="form-group">
                                A(s) solicitação(ões) <strong><asp:Literal ID="ltrSolicitacoesSemAnalistaExclusao" runat="server"></asp:Literal></strong> ficará(ão) sem responsável por aprovação de suas etapas. Deseja prosseguir?
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="Button9" runat="server" Text="Sim" OnClick="btnRemover_OnClick" CssClass="btn btn-danger pull-left mostrarload" />
                            <asp:Button ID="Button10" runat="server" Text="Não" OnClick="OcultarModalExclusao_Click" CssClass="btn btn-default pull-right mostrarload" />
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>

        <asp:Panel ID="pnlAvisoInativarDiretoria" runat="server" Visible="false">
            <div aria-hidden="false" aria-labelledby="myModalLabel" role="dialog" tabindex="-1" id="pnlAvisoExclusaoDiv" class="modal fade in" style="display: block;">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close mostrarload" data-dismiss="modal" aria-hidden="true" runat="server"
                                onserverclick="OcultarModalInativaDiretoria_Click" id="Button2">
                                &times;</button>
                            <h4 class="modal-title">
                                Deseja Inativar <asp:Literal ID="ltrDiretoriaInativar" runat="server"></asp:Literal>?
                            </h4>
                        </div>
                        <div class="modal-body" style="min-height: 500px;">
                            <asp:HiddenField ID="hdnCargoIdInativar" runat="server" />
                            <div class="form-group">
                                A(s) solicitação(ões) <strong><asp:Literal ID="ltrSolicitacoesSemAnalistaInativacaoDiretoria" runat="server"></asp:Literal></strong> ficará(ão) sem responsável por aprovação de suas etapas. Deseja prosseguir?
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="Button11" runat="server" Text="Sim" OnClick="ConfirmarInativacaoDiretoria_OnClick" CssClass="btn btn-danger pull-left mostrarload" />
                            <asp:Button ID="Button12" runat="server" Text="Não" OnClick="OcultarModalInativaDiretoria_Click" CssClass="btn btn-default pull-right mostrarload" />
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>

        <script type="text/javascript">

            $(function () {

                panelList = $('.dropBox');
                $('.dropBox').sortable({
                    handle: '.panel-heading',
                    update: function (event, ui) {
                        var data = panelList.sortable('toArray', { attribute: "id" });
                        $('#hfOrdemDiretoria').val(data);
                        __doPostBack('ctl00$ctl00$ContentPlaceHolder1$ContentPlaceHolder1$lkbOrdenar', '');
                    }
                });

                list = $('.unidadeDrop');
                list.sortable({
                    update: function (event, ui) {
                        var data = list.sortable('toArray', { attribute: "id" });
                        $('#hfOrdemUnidades').val(data);
                        __doPostBack('ctl00$ctl00$ContentPlaceHolder1$ContentPlaceHolder1$lkbOrdemUnidades', '');
                    }
                });
                
                $("#txtTitulo").on('keyup blur', function () {
                    if ($("#txtTitulo").val() != "" && ($("#dvGabinete").is(':not(:visible)') || $("#txtGabinete").val() != "")) {
                        $("#ContentPlaceHolder1_ContentPlaceHolder1_btnSalvarDiretoria").prop('disabled', false);
                        $("#ContentPlaceHolder1_ContentPlaceHolder1_btnSalvarUnidade").prop('disabled', false);
                    } else {
                        $("#ContentPlaceHolder1_ContentPlaceHolder1_btnSalvarDiretoria").prop('disabled', true);
                        $("#ContentPlaceHolder1_ContentPlaceHolder1_btnSalvarUnidade").prop('disabled', true);
                    }
                });

                $("#txtGabinete").on('keyup blur', function () {
                    if (($("#dvGabinete").is(':not(:visible)') || $("#txtGabinete").val() != "") && $("#txtTitulo").val() != "") {
                        $("#ContentPlaceHolder1_ContentPlaceHolder1_btnSalvarDiretoria").prop('disabled', false);
                        $("#ContentPlaceHolder1_ContentPlaceHolder1_btnSalvarUnidade").prop('disabled', false);
                    } else {
                        $("#ContentPlaceHolder1_ContentPlaceHolder1_btnSalvarDiretoria").prop('disabled', true);
                        $("#ContentPlaceHolder1_ContentPlaceHolder1_btnSalvarUnidade").prop('disabled', true);
                    }
                });

                $("div.disabled").find("*").prop('disabled', true);

                $('[data-toggle="tooltip"]').tooltip();
            });

            function ExpandirTodos() {
                $('div[data-toggle="collapse"].collapsed').click();
                $('#botaoExpandir').hide();
                $('#botaoOcultar').show();
            }

            function OcultarTodos() {
                $('div[data-toggle="collapse"]:not(.collapsed)').click();
                $('#botaoExpandir').show();
                $('#botaoOcultar').hide();
            }
                
        </script>
                
    </fieldset>
</asp:Content>
