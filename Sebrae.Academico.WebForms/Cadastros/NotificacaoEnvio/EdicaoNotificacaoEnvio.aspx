<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="EdicaoNotificacaoEnvio.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.NotificacaoEnvio.EdicaoNotificacaoEnvio" %>

<%@ Register Src="~/UserControls/ucPermissoes.ascx" TagName="ucPermissoes" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucLupaMultiplosUsuarios.ascx" TagName="ucLupaMultiplosUsuarios" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="divContainer">
        <div class="panel-group" id="accordion">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <a data-toggle="collapse" data-parent="#accordion" href="#collapse1">Dados </a>
                </div>
                <div id="collapse1" class="panel-collapse collapse in">
                    <div class="panel-body">
                        <fieldset>
                            <div class="form-group">
                                <asp:Label ID="Label2" runat="server" Text="Texto*" AssociatedControlID="txtTexto" />
                                <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip1" Chave="txtTexto" />
                                <span id="contNotificacao" class="pull-right">0</span>
                                <asp:TextBox ID="txtTexto" runat="server" CssClass="form-control" TextMode="multiline" Columns="50" Rows="5"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label1" runat="server" Text="Link*" AssociatedControlID="txtLink" />
                                <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip2" Chave="txtLink" />
                                <asp:TextBox ID="txtLink" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </fieldset>
                    </div>
                </div>
            </div>
            <div class="panel panel-default">
                <div class="panel-heading">
                    <a data-toggle="collapse" data-parent="#accordion" href="#collapseSelecionados" runat="server" id="lkbAlunos">Selecione os alunos</a>
                </div>
                <div id="collapseSelecionados" class="panel-collapse collapse" runat="server" clientidmode="Static">
                    <div class="panel-body">
                        <div class="tabbable tabContentPadding">
                            <ul class="nav nav-tabs" id="tabAlunos">
                                <li class="active"><a href="#tabPermissao" data-toggle="tabPermissao">Permissão</a></li>
                                <li><a href="#tabSolucoesEducacionais" data-toggle="tabSolucoesEducacionais">Solução educacional</a></li>
                                <li><a href="#tabUsuariosAvulsos" data-toggle="tabUsuariosAvulsos">Usuário Avulso</a></li>
                            </ul>
                            <div class="tab-content">
                                <div class="tab-pane active" id="tabPermissao">
                                    <fieldset>
                                        <div class="form-group">
                                            <label>Selecione Permissão de Acesso</label>
                                        </div>
                                    </fieldset>
                                    <uc1:ucPermissoes ID="ucPermissoes1" runat="server" />   
                                </div>
                                <div class="tab-pane" id="tabSolucoesEducacionais">                                    
                                    <fieldset>
                                        <div class="form-group">
                                            <label>Selecione por Turma</label>
                                        </div>
                                    </fieldset>
                                    <div class="form-group">
                                        <asp:Label ID="Label4" runat="server" Text="Solução educacional" AssociatedControlID="ddlSolucaoEducacional"></asp:Label>
                                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip4" Chave="ddlSolucaoEducacional" />
                                        <asp:DropDownList ID="ddlSolucaoEducacional" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlSolucaoEducacional_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label ID="Label3" runat="server" Text="Oferta" AssociatedControlID="ddlOferta"></asp:Label>
                                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip5" Chave="ddlOferta" />
                                        <asp:DropDownList ID="ddlOferta" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlOferta_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label ID="Label5" runat="server" Text="Turma" AssociatedControlID="ddlTurma"></asp:Label>
                                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip6" Chave="ddlTurma" />
                                        <asp:DropDownList ID="ddlTurma" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label AssociatedControlID="chkStatus" Text="Status" runat="server" data-help="Filtro de turmas por status"></asp:Label>
                                        <br /><label id="titleCampos" runat="server" clientidmode="Static"></label>
                                        <asp:CheckBoxList runat="server" ID="chkStatus" />
                                    </div>
                                </div>
                                <div class="tab-pane" id="tabUsuariosAvulsos">                                    
                                    <fieldset>
                                        <div class="form-group">
                                            <label>Selecione usuários avulso</label>
                                        </div>
                                    </fieldset>
                                    <uc1:ucLupaMultiplosUsuarios ID="ucLupaMultiplosUsuarios" runat="server" IsNacional="true" Chave="lupaUsuario" OnUserSelected="UserSelectedHandler" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <br />
    <div>
        <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click" CssClass="btn btn-primary mostrarload" />

        <asp:Button ID="btnEnviarNotificacao" runat="server" Enabled="false" OnClick="btnEnviarNotificacao_Click"
            UseSubmitBehavior="False" Text="Enviar" CssClass="btn btn-default mostrarload" />

        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" CssClass="btn btn-default" />
    </div>

    <!-- MODAL -->
    <asp:Panel ID="pnlModal" runat="server" Visible="false">
        <div aria-hidden="false" aria-labelledby="myModalLabel" data-toggle="modal" role="dialog" tabindex="-1" id="myModal" class="modal fade in" style="display: block;">
            <div class="modal-dialog modal-sm" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true" runat="server"
                            onserverclick="OcultarModal_Click" id="btnFecharModal">
                            &times;</button>
                        <h4 class="modal-title">Confirmação</h4>
                    </div>
                    <div class="modal-body">
                        <div class="panel-group" id="accordionTN">
                            <div class="row filtros">
                                <h4>Filtros selecionados</h4>
                                <asp:Repeater runat="server" ID="rptFiltros">
                                    <ItemTemplate>
                                        <%--Nomes dos status que são exibidos como filtros--%>
                                        <div class="col-sm-2 filtro-mail">
                                            <%# Eval("Uf.Nome") %>
                                            <%# Eval("NivelOcupacional.Nome") %>
                                            <%# Eval("Perfil.Nome") %>
                                            <%# Eval("Turma.Nome") %>
                                            <%# Eval("Usuario.Nome") %>
                                            <%# Eval("Status.Nome") %>
                                            &nbsp;<button type="button" runat="server" onserverclick="btnRemoverFiltro_Click" id="btnRemoverFiltro" class="btn-fechar-filtro" data-id='<%#Eval("ID") %>'>x</button>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                            <p id="pMensagemTotal" runat="server"></p>
                            <p>Obs: notificações são enviadas somente para usuários com situação igual a "ativo".</p>
                            <div>
                                <button type="button" runat="server" onserverclick="btnSim_Click" id="btnSim" class="btn btn-primary mostrarload">Sim</button>
                                <button type="button" runat="server" onserverclick="OcultarModal_Click" id="btnNao" class="btn btn-default">Não</button>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->
    </asp:Panel>

    <script type="text/javascript">
        $(function () {

            $('#tabAlunos a').click(function (e) {
                e.preventDefault();
                $(this).tab('show');
            });

            $(function () {
         
                $("ul.nav-tabs > li > a").on('shown.bs.tab', function (e) {
   
                    localStorage.setItem('lastTab', $(this).attr('href'));
                });

                // go to the latest tab, if it exists:
                var lastTab = localStorage.getItem('lastTab');
                if (lastTab) {
                    $('[href="' + lastTab + '"]').tab('show');
                }
            });

           
            $.markAll('titleCampos', '<%= chkStatus.ClientID %>', 'Marcar todos', 'Desmarcar todos');

            $(".markall").click(function(){
                if( verificarAlteracao() > 1 ){
                    $("#<%= btnEnviarNotificacao.ClientID %>").attr('disabled', 'disabled');
                    $("#divReenviar").hide();
                }else{
                    $("#<%= btnEnviarNotificacao.ClientID %>").removeAttr('disabled');
                    $("#divReenviar").show();
                }
            });

            $('#ContentPlaceHolder1_ContentPlaceHolder1_txtTexto').on('keyup', function (e) {
                var MaxLength = 1000;
                if ($(this).val().length >= MaxLength) {
                    var texto = $(this).val();

                    $(this).val(texto.substring(0, MaxLength));
                    e.preventDefault();
                }
            });

            $("#ContentPlaceHolder1_ContentPlaceHolder1_txtTexto").on('keyup', function () {
                var letras = $("#ContentPlaceHolder1_ContentPlaceHolder1_txtTexto").val();
                $("#contNotificacao").text(letras.length);

                e.preventDefault();

            });

            //Armazena informacao original nos elementos
            $('#divContainer input, #divContainer textarea').each(function(){
                switch($(this).attr('type')){
                    case 'checkbox':
                        $(this).data('original', $(this).is(':checked'));
                        break;
                    default:
                        $(this).data('original', $(this).val());
                        break;
                }
            });
                
            //Controla Visibilidade do BOtao
            $('#divContainer input, #divContainer textarea').change(function() {
                if( verificarAlteracao() > 1 ){
                    $("#<%= btnEnviarNotificacao.ClientID %>").attr('disabled', 'disabled');
                }else{
                    $("#<%= btnEnviarNotificacao.ClientID %>").removeAttr('disabled');
                }
            });

            //Verifica se houve alteracao
            verificarAlteracao = function() {
                var result = 0;
                $('#divContainer input, #divContainer textarea').each(function(){
                    switch($(this).attr('type')){
                        case 'checkbox':
                            if( $(this).is(':checked') != $(this).data('original') )
                                result++;
                            break;
                        default:
                            if( $(this).val() != $(this).data('original') )
                                result++;                                    
                            break;
                    }
                });

                return result;
            }
        });
    </script>
</asp:Content>
