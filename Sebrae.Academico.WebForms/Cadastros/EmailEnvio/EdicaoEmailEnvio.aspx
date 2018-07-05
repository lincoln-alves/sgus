<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="EdicaoEmailEnvio.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.EmailEnvio.EdicaoEmailEnvio" MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/UserControls/ucPermissoes.ascx" TagName="ucPermissoes" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucLupaMultiplosUsuarios.ascx" TagName="ucLupaMultiplosUsuarios" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var _preloadedList = <%= ViewState["_SE"] ?? "''" %>;
        AutoCompleteDefine(_preloadedList, '#txtSolucaoEducacional', true, false, true);

        var _preloadedListOfertas = <%= ViewState["_Oferta"] ?? "''" %>;
        AutoCompleteDefine(_preloadedListOfertas, '#txtOferta', true, false, true);

        var _preloadedListTurmas = <%= ViewState["_Turma"] ?? "''" %>;
        AutoCompleteDefine(_preloadedListTurmas, '#txtTurma', false, false, true);
    </script>

     <style>
        .font-filtros {
           font-weight: bold;
           font-size: 18px;
        }
        .font-filtro-todos {
            font-size:15px !important
        }
         .marginTopFiltros {
            margin-top: 50px;
         }
    </style>

   
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
                                <asp:Label ID="Label1" runat="server" Text="Assunto*" AssociatedControlID="txtAssunto" />
                                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip2" Chave="txtAssunto" />
                                <asp:TextBox ID="txtAssunto" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label6" runat="server" Text="TEXTO*" AssociatedControlID="txtMensagem" />
                                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip1" Chave="txtMensagem" />
                                <label class="pull-right" id="caracteres"></label>
                                <CKEditor:CKEditorControl ID="txtMensagem" runat="server" ExtraPlugins="autocomplete"></CKEditor:CKEditorControl>
                                <script type="text/javascript">
                                    var Suggestions = [
                                        { id: 'NOME_CURSO', label: '#NOME_CURSO' },
                                        { id: 'NOME_ALUNO', label: '#NOME_ALUNO' },
                                        { id: 'CPF', label: '#CPF' },
                                        { id: 'UF', label: '#UF' },
                                        { id: 'EMAIL', label: '#EMAIL' },
                                        { id: 'SENHA', label: '#SENHA' },
                                        { id: 'LINK_CONFIRMAR_INSCRICAO', label: '#LINK_CONFIRMAR_INSCRICAO' },
                                        { id: 'URL_PORTAL', label: '#URL_PORTAL' },
                                        { id: 'URL_SGUS', label: '#URL_SGUS' },
                                        { id: 'DATA_INSCRICAO', label: '#DATA_INSCRICAO' },
                                        { id: 'DATA_TERMINO', label: '#DATA_TERMINO' }
                                    ];

                                    CKEDITOR.on('instanceReady', function (evt) {

                                        //Adicionando caracteres do ckeditor no texto.
                                        var editorContent = $(evt.editor.getData());
                                        var plainEditorContent = editorContent.text().trim();
                                        $("#caracteres").text(plainEditorContent.length);
                                    
                                        // Contando a quantidade de caracteres do ckEditor.
                                        evt.editor.on('key', function(e) {
                                            editorContent = $(evt.editor.getData());
                                            plainEditorContent = editorContent.text().trim();
                                       
                                            // Validando limite máximo de caracteres
                                            if(plainEditorContent.length > 1000) {
                                                evt.editor.setData(plainEditorContent.substring(0, 1000));
                                                $("#caracteres").text(plainEditorContent.length);
                                            }

                                            $("#caracteres").text(plainEditorContent.length);
                                        });

                                        CKEDITOR.instances.ContentPlaceHolder1_ContentPlaceHolder1_txtMensagem.execCommand('reloadSuggestionBox');
                                    });
                                </script>
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
                                            <label>Selecione Permissão</label>
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
                                        <asp:Label ID="Label4" runat="server" Text="Solução educacional" AssociatedControlID="txtSolucaoEducacional"></asp:Label>
                                        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip3" Chave="txtSolucaoEducacional" />
                                        <asp:TextBox ID="txtSolucaoEducacional" ClientIDMode="Static" runat="server" OnTextChanged="txtSolucaoEducacional_OnTextChanged"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label ID="Label3" runat="server" Text="Oferta" AssociatedControlID="txtOferta"></asp:Label>
                                        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip4" Chave="txtOferta" />
                                        <asp:TextBox ID="txtOferta" ClientIDMode="Static" runat="server" OnTextChanged="txtOferta_OnTextChanged" data-mensagemVazia="Selecione uma Solução Educacional"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label ID="Label5" runat="server" Text="Turma" AssociatedControlID="txtTurma"></asp:Label>
                                        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip5" Chave="txtTurma" />
                                        <asp:TextBox ID="txtTurma" ClientIDMode="Static" runat="server" data-mensagemVazia="Selecione uma Oferta"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label AssociatedControlID="chkStatus" Text="Status" runat="server" data-help="Filtro de turmas por status"></asp:Label>
                                        <br />
                                        <label id="titleCampos" runat="server" clientidmode="Static"></label>
                                        <asp:CheckBoxList runat="server" ID="chkStatus" />
                                    </div>
                                </div>
                                <div class="tab-pane" id="tabUsuariosAvulsos">
                                    <fieldset>
                                        <div class="form-group">
                                            <label>Selecione usuários avulso</label>
                                        </div>
                                    </fieldset>
                                    <uc1:ucLupaMultiplosUsuarios ID="ucLupaMultiplosUsuarios" IsNacional="true" Chave="lupaUsuario" runat="server" OnUserSelected="UserSelectedHandler" />
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
        <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click"
            CssClass="btn btn-primary mostrarload" />

        <asp:Button ID="btnEnviarEmail" runat="server" Enabled="false" OnClick="btnEnviarEmail_Click"
            UseSubmitBehavior="False" Text="Enviar" CssClass="btn btn-default mostrarload" />

        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click"
            CssClass="btn btn-default" />
        <div id="divReenviar" style="display: inline">
            <asp:CheckBox Text="Reenviar E-mails?" ID="chkReenviarEmails" Visible="false" runat="server" />
        </div>
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
                        <div class="panel-group">
                            <div class="filtros">
                                <h4>Filtros selecionados</h4> 
                                    <div class="row">
                                        <asp:Panel ID="divPerfil" runat="server" Visible="true" CssClass="col-xs-4"> 
                                            <h5 class="col-sm-12 font-filtros">
                                                <button type="button" runat="server" onserverclick="btnRemoverFiltroTodos_Click" data-filtro='perfil'  class="btn btn-danger btn-xs font-filtro-todos">X</button>
                                                Perfil
                                            </h5>
                                            <asp:Repeater runat="server" ID="rptFiltrosPerfil">
                                                <ItemTemplate>
                                                    <%--Nomes dos status que são exibidos como filtros--%>
                                                    <div class="col-sm-12 filtro-mail" style="margin-bottom:2px">      
                                                        <button type="button" runat="server" onserverclick="btnRemoverFiltro_Click" id="btnRemoverFiltro" class="btn btn-danger btn-xs" data-id='<%#Eval("ID") %>'>x</button>                                  
                                                        <span><%# Eval("Perfil.Nome") %></span>   
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </asp:Panel> 
                                        <asp:Panel ID="divNivelOcupacional" runat="server" Visible="true" CssClass="col-xs-8">                                            
                                            <h5 class="col-sm-12 font-filtros">
                                                <button type="button" runat="server" onserverclick="btnRemoverFiltroTodos_Click" data-filtro='nivel-ocupacional'  class="btn btn-danger btn-xs font-filtro-todos">X</button>
                                                Nível Ocupacional
                                            </h5>
                                            <asp:Repeater runat="server" ID="rptFiltrosNivelOcupacional">
                                                <ItemTemplate>
                                                    <%--Nomes dos status que são exibidos como filtros--%>
                                                    <div class="col-sm-4 filtro-mail" style="margin-bottom:2px">   
                                                        <button type="button" runat="server" onserverclick="btnRemoverFiltro_Click" id="btnRemoverFiltro" class="btn btn-danger btn-xs" data-id='<%#Eval("ID") %>'>x</button>                                        
                                                        <span><%# Eval("NivelOcupacional.Nome") %> </span>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </asp:Panel>
                                    </div>                                    
                                    <asp:Panel ID="divUf" runat="server" Visible="true" CssClass="row marginTopFiltros"> 
                                        <div class="col-xs-10">                                             
                                            <h5 class="col-sm-12 font-filtros">
                                                <button type="button" runat="server" onserverclick="btnRemoverFiltroTodos_Click" data-filtro='uf'  class="btn btn-danger btn-xs font-filtro-todos">X</button>
                                                UF
                                            </h5>            
                                            <asp:Repeater runat="server" ID="rptFiltrosUf">
                                                <ItemTemplate>
                                                    <%--Nomes dos status que são exibidos como filtros--%>
                                                    <div class="col-sm-1 filtro-mail" style="margin-bottom:2px; width:10% !important">    
                                                        <button type="button" runat="server" onserverclick="btnRemoverFiltro_Click" id="btnRemoverFiltro" class="btn btn-danger btn-xs" data-id='<%#Eval("ID") %>'>x</button>                                      
                                                        <span><%# Eval("Uf.Sigla") %> </span>   
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </asp:Panel>                                    
                                    <asp:Panel ID="divTurma" runat="server" Visible="true" CssClass="row marginTopFiltros">
                                        <div class="col-xs-12">
                                            <asp:Repeater runat="server" ID="rptFiltrosTurma">
                                                <ItemTemplate>
                                                    <%--Nomes dos status que são exibidos como filtros--%>                                                    
                                                    <div class="col-sm-12 font-filtros">
                                                        <h5>
                                                            <button type="button" runat="server" onserverclick="btnRemoverFiltroTodos_Click" data-filtro='turma'  class="btn btn-danger btn-xs font-filtro-todos">X</button>
                                                            <span style="font-weight: bold; font-size: 18px;">Turma - </span><span><%# Eval("Turma.Nome") %> </span> 
                                                        </h5> 
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </asp:Panel>
                                    <div class="row">
                                        <asp:Panel ID="divStatus" runat="server" Visible="true" CssClass="col-xs-12">   
                                            <asp:Repeater runat="server" ID="rptFiltrosStatus">
                                                <ItemTemplate>
                                                    <%--Nomes dos status que são exibidos como filtros--%>
                                                    <div class="col-sm-4 filtro-mail" style="margin-bottom:2px">    
                                                        <button type="button" runat="server" onserverclick="btnRemoverFiltro_Click" id="btnRemoverFiltro" class="btn btn-danger btn-xs" data-id='<%#Eval("ID") %>'>x</button>                                      
                                                        <span><%# Eval("Status.Nome") %> </span>   
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </asp:Panel>
                                    </div>        
                                    <asp:Panel ID="divUsuario" runat="server" Visible="true" CssClass="row marginTopFiltros">  
                                        <div class="col-xs-12">                                            
                                            <h5 class="col-sm-12 font-filtros">
                                                <button type="button" runat="server" onserverclick="btnRemoverFiltroTodos_Click" data-filtro='usuario'  class="btn btn-danger btn-xs font-filtro-todos">X</button>
                                                Usuário
                                            </h5>            
                                            <asp:Repeater runat="server" ID="rptFiltrosUsuario">
                                                <ItemTemplate>
                                                    <%--Nomes dos status que são exibidos como filtros--%>
                                                    <div class="col-sm-4 filtro-mail" style="margin-bottom:2px">    
                                                        <button type="button" runat="server" onserverclick="btnRemoverFiltro_Click" id="btnRemoverFiltro" class="btn btn-danger btn-xs" data-id='<%#Eval("ID") %>'>x</button>                                      
                                                        <span><%# Eval("Usuario.Nome") %> </span>   
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </asp:Panel>                                    
                                </div>                                                               
                            </div>
                            <div class="row filtros-result">
                                <p id="pMensagemTotal" runat="server"><strong>Este E-mail será enviado para <span id="spnTotal" runat="server">0</span> usuário(s). Confirma o envio?</strong></p>
                                <p>Obs: e-mails são enviados somente para usuários que possuam situação igual a "ativo".</p>
                                <div>
                                    <button type="button" runat="server" onserverclick="btnSim_Click" id="btnSim" class="btn btn-primary mostrarload">Sim</button>
                                    <button type="button" runat="server" onserverclick="OcultarModal_Click" id="btnNao" class="btn btn-default">Não</button>
                                </div>
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
        (function() {
            $(document).ready(function() {

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
                        $("#<%= btnEnviarEmail.ClientID %>").attr('disabled', 'disabled');
                        $("#divReenviar").hide();
                    }else{
                        $("#<%= btnEnviarEmail.ClientID %>").removeAttr('disabled');
                        $("#divReenviar").show();
                    }
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
                
                //Controla Visibilidade do Botao
                $('#divContainer input, #divContainer textarea').change(function() {
                    if( verificarAlteracao() > 1 ){
                        $("#<%= btnEnviarEmail.ClientID %>").attr('disabled', 'disabled');
                        $("#divReenviar").hide();
                    }else{
                        $("#<%= btnEnviarEmail.ClientID %>").removeAttr('disabled');
                        $("#divReenviar").show();
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
        })(jQuery);
    </script>
</asp:Content>

