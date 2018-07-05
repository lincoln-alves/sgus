<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="GerenciarHierarquiaNucleo.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.HierarquiaNucleo.GerenciarHierarquiaNucleo" %>

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
                    <button class="btn btn-default" type="button" id="botaoExpandir" onclick="ExpandirTodos()"><span class="glyphicon glyphicon-plus"></span>&nbsp;Expandir todos</button>
                    <button class="btn btn-default" style="display:none" id="botaoOcultar" type="button" onclick="OcultarTodos()"><span class="glyphicon glyphicon-minus"></span>&nbsp;Ocultar todos</button>
                </div>

                <button class="btn btn-success pull-right" style="margin-right:5px" type="button" id="btnAdicionarNucleo" runat="server" onserverclick="btnAdicionarNucleo_OnClick">
                    <span class="glyphicon glyphicon-plus"></span>&nbsp;Adicionar Núcleo
                </button>

            </div>
        </div>
        <div class="row div-nucleo-style">
            <asp:Repeater ID="rptDiretoria" runat="server" OnItemDataBound="rptHierarquiaNucleo_OnItemDataBound">
                <ItemTemplate>
                    <div class="div-hierarquia-nucleo col-xs-12 col-md-3 div-col-nucleo">
                        <div class="panel panel-default">
                            <div class="panel-heading text-center">
                                <h3 class="panel-title">
                                    <strong>
                                        <asp:Literal ID="ltrHierarquiaNucleo" runat="server"></asp:Literal>
                                    </strong>
                                    <a href="#" class="pull-right clearfix" ID="btnEditarNucleo" runat="server" onserverclick="btnEditarNucleo_OnServerClick"><i class="glyphicon glyphicon-pencil"></i></a>
                                </h3>
                            </div>
                            <asp:Panel ID="divDisabledNucleo" runat="server" CssClass="panel-body">
                                <div class="panel panel-default" >
                                    <div class="panel-heading text-center clickable collapsed" data-toggle="collapse"  href="#collapseGestor-<%#DataBinder.Eval(Container,"DataItem.ID") %>" aria-expanded="false" aria-controls="collapseGestor-<%#DataBinder.Eval(Container,"DataItem.ID") %>">
                                        <strong >
                                            Gestor
                                        </strong>
                                        <button runat="server" id="btnAdicionarGestor" type="submit" data-hierarquiaNucleo="" data-Gestor="1" class="btn btn-success btn-sm mostrarload pull-right btn-disabled-collapsed" data-toggle="tooltip" data-placement="bottom" title="Incluir" onserverclick="btnAdicionarGestor_OnServerClick">
                                            <span class="glyphicon glyphicon-plus"></span>
                                        </button>
                                    </div>
                                    <div id="collapseGestor-<%#DataBinder.Eval(Container,"DataItem.ID") %>" class="panel-collapse collapse" role="tabpanel">
                                        <div class="panel-body">
                                            <asp:Repeater ID="rptGestores" runat="server" OnItemDataBound="rptGestores_OnItemDataBound">
                                                <ItemTemplate>
                                                    <p>
                                                        <button runat="server" id="btnRemoverUsuario" type="submit" data-hierarquiaNucleoUsuario="" class="btn btn-danger btn-sm mostrarload" data-toggle="tooltip" data-placement="bottom" title="Remover" onserverclick="btnRemoverUsuario_OnServerClick">
                                                            <span class="glyphicon glyphicon-remove"></span>
                                                        </button>
                                                        &nbsp;<%#DataBinder.Eval(Container,"DataItem.Usuario.Nome") %>
                                                    </p>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                            <asp:Literal ID="ltrGestorVazio" runat="server" Text="Nenhum" Visible="False"></asp:Literal>
                                        </div>
                                    </div>                                        
                                </div>
                                
                                <div class="panel panel-default">
                                    <div class="panel-heading text-center clickable collapsed" data-toggle="collapse"  href="#collapseFuncionario-<%#DataBinder.Eval(Container,"DataItem.ID") %>" aria-expanded="false" aria-controls="collapseFuncionario-<%#DataBinder.Eval(Container,"DataItem.ID") %>">
                                        <strong>
                                            Funcionários
                                        </strong>
                                        <button runat="server" id="btnAdicionarFuncionario" type="submit" data-hierarquiaNucleo="" data-Gestor="0" class="btn btn-success btn-sm mostrarload pull-right btn-disabled-collapsed" data-toggle="tooltip" data-placement="bottom" title="Incluir" onserverclick="btnAdicionarFuncionario_OnServerClick">
                                            <span class="glyphicon glyphicon-plus"></span>
                                        </button>
                                    </div>
                                    <div id="collapseFuncionario-<%#DataBinder.Eval(Container,"DataItem.ID") %>" class="panel-collapse collapse" role="tabpanel">
                                        <div class="panel-body">
                                           <asp:Repeater ID="rptFuncionarios" runat="server" OnItemDataBound="rptFuncionarios_OnItemDataBound">
                                                <ItemTemplate>
                                                    <p>
                                                        <button runat="server" id="btnRemoverUsuario" type="submit" data-hierarquiaNucleoUsuario="" class="btn btn-danger btn-sm mostrarload" data-toggle="tooltip" data-placement="bottom" title="Remover" onserverclick="btnRemoverUsuario_OnServerClick">
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
                                <asp:HiddenField ID="hdnIdHierarquiaNucleo" runat="server" />
                                <asp:HiddenField ID="hdnIsGestor" runat="server" />
                                <div class="form-group">
                                    <asp:Label ID="Label1" runat="server" AssociatedControlID="txtUfModal" Text="UF"></asp:Label>
                                    <asp:TextBox ID="txtUfModal" ClientIDMode="Static" runat="server" Enabled="False" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="lblHierarquiaNucleoModa" runat="server" AssociatedControlID="txtHierarquiaNucleoModal" Text="Núcleo"></asp:Label>
                                    <asp:TextBox ID="txtHierarquiaNucleoModal" ClientIDMode="Static" runat="server" Enabled="False" CssClass="form-control"></asp:TextBox>
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


        <asp:Panel ID="pnlModalNucleo" runat="server" Visible="false">
            <div aria-hidden="false" aria-labelledby="myModalLabel" role="dialog" tabindex="-1" id="modalDiretoria" class="modal fade in" style="display: block;">
                <div class="modal-dialog" style="width: 40%;">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close mostrarload" data-dismiss="modal" aria-hidden="true" runat="server" onserverclick="OcultarModalNucleo_Click" id="Button1">&times;</button>
                            <h4 class="modal-title">
                                <asp:Literal ID="ltrTituloModalNucleo" runat="server"></asp:Literal>
                            </h4>
                        </div>
                        <div class="modal-body">
                            <fieldset>  
                                <asp:HiddenField ID="idHierarquiaNucleo" runat="server" Value="0" />                              
                                <div class="form-group">
                                    <asp:Label ID="Label2" runat="server" AssociatedControlID="txtTitulo" Text="Título*"></asp:Label>
                                    <asp:TextBox ID="txtTitulo" ClientIDMode="Static" runat="server" MaxLength="255" CssClass="form-control"></asp:TextBox>
                                </div>
                                <asp:Panel ID="dvStatus" runat="server" CssClass="form-group" Visible="true">
                                    <asp:Label ID="Label5" runat="server" AssociatedControlID="rblStatus" Text="Status*"></asp:Label>
                                    <asp:RadioButtonList ID="rblStatus" runat="server" RepeatDirection="Horizontal" CssClass="form-control">
                                        <asp:ListItem Text="Ativo" Selected="True" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Inativo" Value="0"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </asp:Panel>
                                
                            </fieldset>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnSalvarNucleo" runat="server" Text="Salvar" OnClick="btnSalvarNucleo_OnClick"  Enabled="false" CssClass="btn btn-primary pull-left mostrarload" />
                            <asp:Button ID="Button3" runat="server" OnClick="OcultarModalNucleo_Click" Text="Cancelar"  CssClass="btn btn-default pull-right mostrarload" />
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>


        <asp:Panel ID="pnlModalGestorConfirmacao" runat="server" Visible="false">
            <div aria-hidden="false" aria-labelledby="myModalLabel" role="dialog" tabindex="-1" id="modalGestorConfirmacao" class="modal fade in" style="display: block;">
                <div class="modal-dialog" style="width: 40%;">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close mostrarload" data-dismiss="modal" aria-hidden="true" runat="server" OnClick="OcultarModalUsuarioConfirmacao_Click"  id="Button6">&times;</button>
                            <h4 class="modal-title">
                               Confirmação
                            </h4>
                        </div>
                        <div class="modal-body">
                            <fieldset>  
                                <asp:HiddenField ID="IdUsuarioSelecionado" runat="server" Value="0" />
                                <asp:HiddenField ID="IdHierarquiaNucleoSelcionado" runat="server" Value="0" />   
                                <asp:HiddenField ID="IsGestorSelecionado" runat="server" Value="0" />                                    
                                <div class="form-group">
                                     <strong><asp:Literal ID="TxtMgsConfirmacao" runat="server"></asp:Literal></strong>                                
                                </div>  
                            </fieldset>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnDuplicar" runat="server" Text="Duplicar" OnClick="btnDuplicar_OnClick"   CssClass="btn btn-primary pull-right mostrarload btn-margin" />
                            <asp:Button ID="btnMover" runat="server" Text="Mover" OnClick="btnConfirmacaoMover_onClick"   CssClass="btn btn-primary pull-right mostrarload btn-margin" />
                            <asp:Button ID="btnSalvarFuncionario" runat="server" Text="Salvar" Visible="false" OnClick="btnDuplicar_OnClick"   CssClass="btn btn-primary pull-right mostrarload btn-margin" />
                            <asp:Button ID="Button8" runat="server" OnClick="OcultarModalUsuarioConfirmacao_Click" Text="Cancelar"  CssClass="btn btn-default pull-right mostrarload btn-margin"  />
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>


        <asp:Panel ID="pnlModalMsg" runat="server" Visible="false">
            <div aria-hidden="false" aria-labelledby="myModalLabel" role="dialog" tabindex="-1" id="modalMsg" class="modal fade in" style="display: block;">
                <div class="modal-dialog" style="width: 40%;">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close mostrarload"  onserverclick="OcultarModalMsg_Click" data-dismiss="modal" aria-hidden="true" runat="server" id="Button2">&times;</button>
                            <h4 class="modal-title">
                                Inativar Núcleo
                            </h4>
                        </div>
                        <div class="modal-body">
                            <fieldset>  
                                <asp:HiddenField ID="idHieraquiaNucleoInativacao" runat="server" Value="0" />   
                                <strong><asp:Literal  ID="txtMsgInativacao" runat="server"></asp:Literal></strong>
                            </fieldset>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="Button4" runat="server" Text="Sim"   OnClick="btnInativarNucleo_OnClick" CssClass="btn btn-primary pull-left mostrarload" />
                            <asp:Button ID="Button5" runat="server"  Text="Não" OnClick="OcultarModalMsg_Click"  CssClass="btn btn-default pull-right mostrarload" />
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>

        <asp:Panel ID="pnlModalConfirmacaoMover" runat="server" Visible="false">
            <div aria-hidden="false" aria-labelledby="myModalLabel" role="dialog" tabindex="-1" id="modalMsg" class="modal fade in" style="display: block;">
                <div class="modal-dialog" style="width: 40%;">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close mostrarload"  onserverclick="OcultarModaConfirmacaoMover_Click" data-dismiss="modal" aria-hidden="true" runat="server" id="Button7">&times;</button>
                            <h4 class="modal-title">
                                Mover Gestor
                            </h4>
                        </div>
                        <div class="modal-body">
                            <fieldset>                                  
                                <strong><asp:Literal  ID="txtConfirmacaoMover" runat="server"></asp:Literal></strong>
                            </fieldset>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="Button9" runat="server" Text="Mover"   OnClick="btnMover_OnClick" CssClass="btn btn-primary pull-left mostrarload" />
                            <asp:Button ID="Button10" runat="server"  Text="Cancelar" OnClick="OcultarModaConfirmacaoMover_Click"  CssClass="btn btn-default pull-right mostrarload" />
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>

        <asp:Panel ID="pnlModalMsgSolicitacoesResponsavel" runat="server" Visible="false">
            <div aria-hidden="false" aria-labelledby="myModalLabel" role="dialog" tabindex="-1" id="modalMsg" class="modal fade in" style="display: block;">
                <div class="modal-dialog" style="width: 40%;">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close mostrarload"  OnClick="OcultarModalNovoResponsavel_Click"  data-dismiss="modal" aria-hidden="true" runat="server" id="Button11">&times;</button>
                            <h4 class="modal-title">
                               Novo responsável
                            </h4>
                        </div>
                        <div class="modal-body">
                            <fieldset>                                  
                                <strong><asp:Literal  ID="txtNovoResponsavel" runat="server"></asp:Literal></strong>
                            </fieldset>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="Button12" runat="server" Text="OK" OnClick="OcultarModalNovoResponsavel_Click" CssClass="btn btn-primary pull-left mostrarload" />                            
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>




        <script type="text/javascript">
            $(function () {

                $("#txtTitulo").on('keyup blur', function () {
                    if ($("#txtTitulo").val() != "") {
                        $("#ContentPlaceHolder1_ContentPlaceHolder1_btnSalvarNucleo").prop('disabled', false);
                    } else {
                        $("#ContentPlaceHolder1_ContentPlaceHolder1_btnSalvarNucleo").prop('disabled', true);
                    }
                });

                $('[data-toggle="tooltip"]').tooltip();               

                $('.btn-disabled-collapsed').click(function (e) {
                    e.stopPropagation();
                });
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
