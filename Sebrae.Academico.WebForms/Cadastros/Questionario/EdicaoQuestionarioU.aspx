<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    EnableViewState="true" EnableEventValidation="false" ValidateRequest="false"
    MaintainScrollPositionOnPostback="true" ViewStateMode="Enabled" CodeBehind="EdicaoQuestionarioU.aspx.cs"
    Inherits="Sebrae.Academico.WebForms.Cadastros.EdicaoQuestionarioU" %>

<%@ Register Src="~/UserControls/ucPermissoes.ascx" TagName="ucPermissoes" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucCategorias.ascx" TagName="ucCategorias" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="/js/jquery/jquery.tablednd.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#dgvItensDoQuestionario").tableDnD({
                onDrop: function (table, row) {
                    var teste = $.tableDnD.serialize();
                    $('#hfOrdemItemQuestionario').val(teste);
                    __doPostBack("ctl00$ctl00$ContentPlaceHolder1$ContentPlaceHolder1$dgvItensDoQuestionario$ctl02$lkbOrdenar");
                }
            });
        });
    </script>
    <fieldset>
        <asp:ScriptManager ID="SM1" runat="server" />
        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:LinkButton Text="teste" runat="server" ID="lnkQuestionarioEdicao" ClientIDMode="Static" OnClick="lnkQuestionarioEdicao_Click" CssClass="hidden-field "></asp:LinkButton>

                <asp:LinkButton Text="link item questionário" ID="lnkItemQuestionarioEdicao" ClientIDMode="Static" OnClick="lnkItemQuestionarioEdicao_Click" runat="server" CssClass="hidden-field " />

                <asp:LinkButton Text="link item quesitonário opção" ID="lnkItemQuestionarioOpcaoEdicao" ClientIDMode="Static" OnClick="lnkItemQuestionarioOpcaoEdicao_Click" runat="server" CssClass="hidden-field" />
            </ContentTemplate>
        </asp:UpdatePanel>

        <div class="form-group">
            <asp:Label ID="Label2" runat="server" Text="Tipo de questionário" AssociatedControlID="ddlTipoQuestionario" />
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip1" Chave="ddlTipoQuestionario" />
            <asp:DropDownList ID="ddlTipoQuestionario" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoQuestionario_SelectedIndexChanged"
                CssClass="form-control">
            </asp:DropDownList>
        </div>
        <asp:Panel ID="pnlOpcoesQuestionario" runat="server" Visible="false">
            <div class="panel-group" id="accordionGM">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <a data-toggle="collapse" href="#collapseDados" data-parent="#accordionGM">Dados Questionário</a>
                    </div>
                    <div id="collapseDados" class="panel-collapse collapse in edicao-questionario">
                        <div class="panel-body">
                            <div>
                                <div class="form-group" id="divCategoriaMoodle" runat="server">
                                    <asp:Label ID="lblCategoria" runat="server" Text="CATEGORIA" AssociatedControlID="ucCategorias1" />
                                    <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip2" Chave="ucCategorias1" />
                                    <uc2:ucCategorias ID="ucCategorias1" runat="server" />
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="Label1" runat="server" Text="Nome do Questionário" AssociatedControlID="txtNome" />
                                    <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip3" Chave="txtNome" />
                                    <asp:TextBox ID="txtNome" runat="server" CssClass="form-control" MaxLength="200" data-restaurar="Nome"></asp:TextBox>
                                </div>
                                <div class="form-group" id="divPrazoemMinutos" runat="server">
                                    <asp:Label ID="Label3" runat="server" Text="Prazo em Minutos" AssociatedControlID="txtPrazoemMinutos" />
                                    <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip4" Chave="txtPrazoemMinutos" />
                                    <asp:TextBox ID="txtPrazoemMinutos" runat="server" CssClass="form-control" MaxLength="3" onkeypress="return EhNumerico(event)" data-restaurar="PrazoMinutos"></asp:TextBox>
                                </div>
                                <div class="form-group" runat="server" id="divQtdQuestoesDaProva">
                                    <asp:Label ID="Label4" runat="server" Text="Qtd de Questões da Prova" AssociatedControlID="txtQtdQuestoesDaProva" />
                                    <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip5" Chave="dtQuestoesProva" />
                                    <asp:TextBox ID="txtQtdQuestoesDaProva" runat="server" CssClass="form-control" MaxLength="3"
                                        onkeypress="return EhNumerico(event)" data-restaurar="QtdQuestoesProva"></asp:TextBox>
                                </div>
                                <div class="form-group" runat="server" id="divNotaMinima" visible="false">
                                    <asp:Label ID="Label19" runat="server" Text="Nota mínima" AssociatedControlID="txtNotaMinima" />
                                    <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip22" Chave="txtNotaMinima" />
                                    <asp:TextBox ID="txtNotaMinima" runat="server" CssClass="form-control" MaxLength="3"
                                        onkeypress="return EhNumerico(event)" data-restaurar="NotaMinima"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="Label5" runat="server" Text="Enunciado" AssociatedControlID="txtTextoEnunciadoPre" />
                                    <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip6" Chave="txtTextoEnunciadoPre" />
                                    <CKEditor:CKEditorControl ID="txtTextoEnunciadoPre" BasePath="/js/ckeditor/" runat="server"></CKEditor:CKEditorControl>
                                    <script type="text/javascript">
                                        var Suggestions = [
                                            { id: 'NOME_ALUNO', label: '#NOME_ALUNO' },
                                            { id: 'SOLUCAO', label: '#SOLUCAO' },
                                            { id: 'OFERTA', label: '#OFERTA' },
                                            { id: 'TURMA', label: '#TURMA' },
                                            { id: 'DATAMATRICULA', label: '#DATAMATRICULA' },
                                            { id: 'DATATERMINO', label: '#DATATERMINO' },
                                            { id: 'TRILHA', label: '#TRILHA' },
                                            { id: 'TRILHANIVEL', label: '#TRILHANIVEL' },
                                            { id: 'DATAINICIOTRILHA', label: '#DATAINICIOTRILHA' },
                                            { id: 'DATAFIMTRILHA', label: '#DATAFIMTRILHA' },
                                            { id: 'DATADEHOJE', label: '#DATADEHOJE' }
                                        ];

                                        CKEDITOR.on('instanceReady', function (evt) {
                                            CKEDITOR.instances.ContentPlaceHolder1_ContentPlaceHolder1_txtTextoEnunciadoPre.execCommand('reloadSuggestionBox');
                                        });
                                    </script>
                                </div>
                                <div class="form-group clearfix" id="divFormaDeAquisicao" runat="server" visible="false">
                                    <label>Forma de Aquisição</label>
                                    <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip7" Chave="rblFormaDeAquisicao" />
                                    <asp:RadioButtonList ID="rblFormaDeAquisicao" runat="server" RepeatDirection="Vertical" RepeatLayout="UnorderedList" ClientIDMode="Static" data-restaurar="FormaAquisicaoId"></asp:RadioButtonList>
                                </div>
                                <div class="form-group clearfix" id="divNotificacao" runat="server" visible="false">
                                    <asp:Label ID="notificacao_label" runat="server" Text="Gerar Notificações aos usuários ao salvar?" AssociatedControlID="rblNotificacao" />
                                    <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip8" Chave="rblNotificacao" />
                                    <asp:RadioButtonList ID="rblNotificacao" runat="server" RepeatDirection="Horizontal" CssClass="form-control" />
                                </div>
                                <div class="form-group clearfix" id="divLinkQuestionario" runat="server" visible="false">
                                    <asp:Label ID="lblLinkQuestionario" runat="server" Text="Url de Acesso" AssociatedControlID="hpLinkQuestionario" />
                                    <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip9" Chave="hpLinkQuestionario" />
                                    <br />
                                    <asp:HyperLink ID="hpLinkQuestionario" Target="_blank" runat="server"></asp:HyperLink>
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="Label21" runat="server" Text="Ativo?" AssociatedControlID="rblAtivo" />
                                    <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip23" Chave="rblAtivo" />
                                    <asp:RadioButtonList ID="rblAtivo" runat="server" RepeatDirection="Horizontal" CssClass="form-control" data-restaurar="Ativo" />
                                </div>
                                <br />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel panel-default" id="divQuestionarioPermissoes" runat="server" visible="true">
                    <div class="panel-heading">
                        <a data-toggle="collapse" href="#collapsePermissoes" data-parent="#accordionGM">Permissões
                        </a>
                    </div>
                    <div id="collapsePermissoes" class="panel-collapse collapse">
                        <div class="panel-body edicao-questionario">
                            <uc1:ucPermissoes ID="ucPermissoes1" runat="server" DataAttribute="restaurar" />
                        </div>
                    </div>
                </div>
                <div class="panel panel-default">
                    <asp:HiddenField ID="hfOrdemItemQuestionario" ClientIDMode="Static" runat="server" />
                    <div class="panel-heading">
                        <a data-toggle="collapse" href="#collapseQuestoes" data-parent="#accordionGM">Questões Cadastradas</a>
                    </div>
                    <div id="collapseQuestoes" class="panel-collapse collapse">
                        <div class="panel-body">
                            <asp:GridView ID="dgvItensDoQuestionario" runat="server" OnRowCommand="dgvItensDoQuestionario_RowCommand" OnRowDataBound="dgvMatriculaOferta_RowDataBound"
                                CssClass="table col-sm-12" GridLines="None" AutoGenerateColumns="false" ClientIDMode="Static">
                                <Columns>
                                    <asp:TemplateField HeaderText="Tipo">
                                        <ItemTemplate>
                                            <%#Eval("TipoItemQuestionario.Nome")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item do Questionário">
                                        <ItemTemplate>
                                            <%#Eval("Questao")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Resposta Correta">
                                        <ItemTemplate>
                                            <asp:RadioButtonList ID="rdbValorDaResposta" runat="server" RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="rdbValorDaRespostaLista_OnSelectedIndexChanged" CssClass="table noborder mostrarload">
                                                <asp:ListItem Text="V" Value="V"></asp:ListItem>
                                                <asp:ListItem Text="F" Value="F"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Avalia Tutor">
                                        <ItemTemplate>
                                            <%#Eval("InAvaliaProfessorFormatado")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemStyle Width="90px" HorizontalAlign="center" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lkbEditar" runat="server" CausesValidation="False" CommandName="editar"
                                                CssClass="mostrarload" CommandArgument='<%# Container.DataItemIndex %>' ToolTip="Editar">
                                                    <span class="btn btn-default btn-xs">
								                        <span class="glyphicon glyphicon-pencil"></span>
							                        </span>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="lkbExcluir" runat="server" CausesValidation="False" CommandName="excluir" CommandArgument='<%# Container.DataItemIndex %>'
                                                ToolTip="Excluir" OnClientClick="return confirm('Deseja Realmente Excluir este Item deste Questionário ?');">
                                                    <span class="btn btn-default btn-xs">
								                        <span class="glyphicon glyphicon-remove"></span>
							                        </span>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="lkbDuplicar" runat="server" CausesValidation="False" CommandName="duplicar" CommandArgument='<%# Container.DataItemIndex %>' CssClass="mostrarload btn btn-default btn-xs" ToolTip="Duplicar">
							                    <span class="glyphicon glyphicon-floppy-saved"></span>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="lkbOrdenar" runat="server" Visible="false" CommandName="ordenar" ToolTip="Ordenar"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <itemtemplate>Nenhuma questão cadastrada</itemtemplate>
                                </EmptyDataTemplate>
                            </asp:GridView>
                            <asp:Button ID="btnAdicionarItemQuestionario" runat="server" OnClick="btnAdicionarItemQuestionario_Click"
                                UseSubmitBehavior="False" Text="Incluir questão" CssClass="btn btn-default mostrarload" />
                        </div>
                    </div>
                </div>
            </div>
            <div>
                <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click"
                    CssClass="btn btn-primary mostrarload" />
                <asp:Button ID="btnDuplicarQuestionario" runat="server" Text="Duplicar este Questionário" CssClass="btn btn-primary" OnClick="btnDuplicarQuestionario_Click" OnClientClick="return confirm('Deseja realmente duplicar este questionário?');" Visible="false" />
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click"
                    OnClientClick="return confirm('Deseja Realmente Cancelar o Cadastro deste Questionário ? Todos os dados não salvos serão perdidos');"
                    CssClass="btn btn-default" />
            </div>
            <asp:HiddenField ID="hdAcaoItemQuestionario" runat="server" />
            <asp:HiddenField ID="hdIndexOfItemQuestionario" runat="server" />
            <asp:HiddenField ID="hdAcaoItemQuestionarioOpcao" runat="server" />
            <asp:HiddenField ID="hdIndexOfItemQuestionarioOpcao" runat="server" />
            <!-- MODAL -->
            <asp:Panel ID="pnlModal" runat="server" Visible="false">
                <div aria-hidden="false" aria-labelledby="myModalLabel" role="dialog" tabindex="-1"
                    id="myModal" class="modal fade in" style="display: block;">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true" runat="server"
                                    onserverclick="OcultarModal_Click" id="btnFecharModal">
                                    &times;</button>
                                <h4 class="modal-title">Item do Questionário</h4>
                            </div>
                            <div class="modal-body">
                                <div class="form-group">
                                    <asp:Label ID="Label6" runat="server" Text="Tipo:" AssociatedControlID="ddlTipoItemQuestionario" />
                                    <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip10" Chave="ddlTipoItemQuestionario" />
                                    <asp:DropDownList ID="ddlTipoItemQuestionario" runat="server" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlTipoItemQuestionario_SelectedIndexChanged" CssClass="form-control mostrarload">
                                    </asp:DropDownList>
                                </div>
                                <asp:Panel ID="pnlItemQuestionario" runat="server" Visible="false">
                                    <div class="panel-group" id="accordionUC">
                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                                <a data-toggle="collapse" href="#collapseDadosDaQuestao" data-parent="#accordionUC">Dados da questão
                                                </a>
                                            </div>
                                            <div id="collapseDadosDaQuestao" clientidmode="Static" runat="server" class="panel-collapse collapse in pnl-itemQuestionario">
                                                <div class="panel-body">
                                                    <div class="form-group" runat="server" id="divQuestaoEnunciado">
                                                        <asp:Label ID="lblQuestionarionunciado" runat="server" Text="Bloco agrupador de questões" AssociatedControlID="ddlQuestionarioEnunciado" />
                                                        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip11" Chave="ddlQuestionarioEnunciado" />
                                                        <asp:DropDownList ID="ddlQuestionarioEnunciado" runat="server" AutoPostBack="true" CssClass="form-control mostrarload">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group">
                                                        <asp:Label ID="labelQuestaoNome" runat="server" Text="Enunciado / Pergunta" AssociatedControlID="txtQuestaoNome" />
                                                        <span class="pull-right" id="caracteres"></span>
                                                        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip12" Chave="txtQuestaoNome" />
                                                        <CKEditor:CKEditorControl ID="txtQuestaoNome" MaxLength="2000" BasePath="/js/ckeditor/" runat="server"></CKEditor:CKEditorControl>
                                                    </div>
                                                    <asp:UpdatePanel runat="server">
                                                        <ContentTemplate>
                                                            <div class="form-group" runat="server" id="divExibeFeedback">
                                                                <asp:Label ID="Label11" runat="server" Text="Exibir Feedback" AssociatedControlID="rdbExibeFeedback" />
                                                                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip16" Chave="rdbExibeFeedback" />
                                                                <asp:RadioButtonList ID="rdbExibeFeedback" runat="server" RepeatDirection="Horizontal" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="rdbExibeFeedback_OnSelectedIndexChanged" />
                                                            </div>

                                                            <div class="form-group" runat="server" id="divFeedback">
                                                                <asp:Label ID="Label8" runat="server" Text="Feedback" AssociatedControlID="txtFeedback" />
                                                                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip13" Chave="txtFeedback" />
                                                                <asp:TextBox ID="txtFeedback" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                    <div class="form-group" id="divValorQuestao" runat="server">
                                                        <asp:Label ID="Label9" runat="server" Text="Valor da Questão" AssociatedControlID="txtValorQuestao" />
                                                        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip14" Chave="txtValorQuestao" />
                                                        <asp:TextBox ID="txtValorQuestao" runat="server" MaxLength="3" CssClass="form-control"
                                                            onkeypress="return EhNumericoOuVirgula(event)"></asp:TextBox>
                                                    </div>
                                                    <div class="form-group" id="divGabarito" runat="server">
                                                        <asp:Label ID="Label10" runat="server" Text="Gabarito" AssociatedControlID="txtGabaritoQuestao" />
                                                        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip15" Chave="txtGabaritoQuestao" />
                                                        <asp:TextBox ID="txtGabaritoQuestao" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                    <asp:UpdatePanel runat="server">
                                                        <ContentTemplate>
                                                            <div class="form-group" runat="server" id="divEstilo">
                                                                <asp:Label ID="Label14" runat="server" Text="Estilo" AssociatedControlID="ddlEstiloItemQuestionario" />
                                                                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip17" Chave="ddlEstiloItemQuestionario" />
                                                                <asp:DropDownList ID="ddlEstiloItemQuestionario" AutoPostBack="true" runat="server" CssClass="form-control"></asp:DropDownList>
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                    <asp:UpdatePanel runat="server">
                                                        <ContentTemplate>
                                                            <div class="form-group" runat="server" id="divAvaliaTutor">
                                                                <asp:Label ID="Label15" runat="server" Text="Este Questionário Avalia o Tutor?" AssociatedControlID="ddlInAvaliaProfessor" />
                                                                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip18" Chave="ddlInAvaliaProfessor" />
                                                                <asp:DropDownList ID="ddlInAvaliaProfessor" AutoPostBack="true" runat="server" CssClass="form-control mostrarload">
                                                                    <asp:ListItem Value="0">Não</asp:ListItem>
                                                                    <asp:ListItem Value="1">Sim</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                    <div class="form-group" runat="server" id="divValorDaResposta" visible="False">
                                                        <asp:Label ID="Label16" runat="server" Text="Valor da resposta" AssociatedControlID="rdbValorDaResposta" />
                                                        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip21" Chave="rdbValorDaResposta" />
                                                        <asp:RadioButtonList ID="rdbValorDaResposta" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" AutoPostBack="True" OnSelectedIndexChanged="rdbValorDaResposta_OnSelectedIndexChanged" CssClass="mostrarload">
                                                            <asp:ListItem Text="V" Value="V"></asp:ListItem>
                                                            <asp:ListItem Text="F" Value="F"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </div>
                                                    <div class="form-group" runat="server" id="divRespostaObrigatoria" visible="true">
                                                        <asp:Label ID="Label7" runat="server" Text="Pergunta com resposta obrigatória?" AssociatedControlID="rdbRespostaObrigatoria" />
                                                        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip24" Chave="rdbRespostaObrigatoria" />
                                                        <asp:RadioButtonList ID="rdbRespostaObrigatoria" runat="server" RepeatDirection="Horizontal" CssClass="form-control" AutoPostBack="True" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <asp:Panel ID="pnlCamposItemQuestionarioOpcao" CssClass="panel panel-default" runat="server">
                                            <div class="panel-heading">
                                                <a data-toggle="collapse" href="#collapseOpcoesDeResposta" data-parent="#accordionUC">Resposta
                                                </a>
                                            </div>
                                            <div id="collapseOpcoesDeResposta"  clientidmode="Static" runat="server" class="panel-collapse collapse pnl-itemQuestionarioOpcao">
                                                <div class="panel-body">
                                                    <asp:GridView ID="dgvItemQuestionarioOpcoes" runat="server" AutoGenerateColumns="false" OnRowCommand="dgvItemQuestionarioOpcoes_RowCommand" OnRowDataBound="dgvItemQuestionarioOpcoes_OnRowDataBound"
                                                        CssClass="table col-sm-12" GridLines="None">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Resposta">
                                                                <ItemTemplate>
                                                                    <%#Eval("Nome")%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Resposta relacionada" Visible="False">
                                                                <ItemTemplate>
                                                                    <%#Eval("OpcaoVinculada") != null ? Eval("OpcaoVinculada.Nome") : ""%>
                                                                </ItemTemplate>
                                                                <ControlStyle CssClass="text-center"></ControlStyle>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Resposta Correta">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkRespostaCorreta" runat="server" OnCheckedChanged="chkRespostaCorreta_OnCheckedChanged" AutoPostBack="True" CssClass="mostrarload" />
                                                                </ItemTemplate>
                                                                <ControlStyle CssClass="text-center"></ControlStyle>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ShowHeader="False">
                                                                <ItemStyle Width="90px" HorizontalAlign="center" />
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lkbEditar" runat="server" CausesValidation="False" CommandName="editar" ToolTip="Editar"
                                                                        CssClass="mostrarload">
                                                                        <span class="btn btn-default btn-xs">
						                                                    <span class="glyphicon glyphicon-pencil"></span>
					                                                    </span>                        
                                                                    </asp:LinkButton>
                                                                    <asp:LinkButton ID="lkbExcluir" runat="server" CausesValidation="False"
                                                                        CommandName="excluir" OnClientClick="return confirm('Deseja realmente excluir esta resposta ?');" ToolTip="Excluir">
                                                                        <span class="btn btn-default btn-xs">
						                                                    <span class="glyphicon glyphicon-remove"></span>
						                                                </span>
                                                                    </asp:LinkButton>
                                                                    <asp:LinkButton ID="lkbDuplicar" runat="server" CausesValidation="False" CommandName="duplicar" CommandArgument='<%# Eval("ID")%>'
                                                                        CssClass="mostrarload btn btn-default btn-xs" ToolTip="Duplicar">
							                                            <span class="glyphicon glyphicon-floppy-saved"></span>
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <itemtemplate>Nenhuma opção cadastrada para esta questão</itemtemplate>
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>

                                                    <div class="form-group">
                                                        <asp:Button ID="btnAdicionarItemQuestionarioOpcao" runat="server" Text="Adicionar Opções" OnClick="btnAdicionarItemQuestionarioOpcao_Click" CssClass="btn btn-default mostrarload" CausesValidation="false" />
                                                    </div>

                                                    <div class="form-group" id="divCampoItemQuestionarioOpcao" runat="server">
                                                        <asp:Label ID="Label12" runat="server" Text="Resposta" AssociatedControlID="txtItemQuestionarioOpcao" />
                                                        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip19" Chave="txtItemQuestionarioOpcao" />
                                                        <asp:TextBox ID="txtItemQuestionarioOpcao" runat="server" MaxLength="2000" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="form-group" id="divRespostaCorreta" runat="server">
                                                        <asp:Label ID="Label13" runat="server" Text="Resposta Correta ?" AssociatedControlID="rblRespostaCorreta" />
                                                        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip20" Chave="rblRespostaCorreta" />
                                                        <asp:CheckBox ID="rblRespostaCorreta" runat="server" />
                                                    </div>
                                                    <div class="form-group" runat="server" id="divColunasRelacionadas">
                                                        <div class="row">
                                                            <div class="col-xs-12 col-md-6">
                                                                <asp:Label ID="Label17" runat="server" Text="Resposta" AssociatedControlID="txtResposta_coluna2" />
                                                                <asp:TextBox ID="txtResposta_coluna2" runat="server" CssClass="form-control" Rows="4"></asp:TextBox>
                                                            </div>
                                                            <div class="col-xs-12 col-md-6">
                                                                <asp:Label ID="Label18" runat="server" Text="Resposta Relacionada" AssociatedControlID="txtResposta_coluna1" />
                                                                <asp:TextBox ID="txtResposta_coluna1" runat="server" CssClass="form-control" Rows="4"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <asp:Button ID="btnEnviarItemQuestionarioOpcao" runat="server" Text="Enviar Opção" OnClick="btnEnviarItemQuestionarioOpcao_Click"
                                                            CssClass="btn btn-default mostrarload" CausesValidation="false" />
                                                        <asp:Button ID="btnCancelarItemQuestionarioOpcao" runat="server" Text="Cancelar" OnClick="btnCancelarItemQuestionarioOpcao_Click"
                                                            CssClass="btn btn-default  mostrarload" CausesValidation="false" />
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </asp:Panel>
                                <br />
                                <div class="form-group">
                                    <asp:Button ID="btnEnviarItemQuestionario" runat="server" Text="Enviar" CssClass="btn btn-primary mostrarload" OnClick="btnEnviarItemQuestionario_Click" />
                                    <asp:Button ID="btnCancelarItemQuestionario" runat="server" Text="Cancelar" CssClass="btn btn-default" OnClick="btnCancelarItemQuestionario_Click"
                                        OnClientClick="return confirm('Deseja realmente cancelar a edição?');" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <asp:Panel ID="pnlModalSessao" runat="server" Visible="false">
                <div aria-hidden="false" aria-labelledby="myModalLabel" role="dialog" tabindex="-1"
                    id="modalSessao" class="modal fade in" style="display: block;">
                    <div class="modal-dialog modal-aviso">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true" runat="server"
                                    onserverclick="OcultarModalSessao_Click" id="Button1">
                                    &times;</button>
                                <h2 class="modal-title">Registros Pendentes de Edição</h2>
                            </div>
                            <div class="modal-body">
                                <p>Existe uma edição de questionário na sessão, deseja restaurar a última criação/edição do questionário?</p>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="Button5" runat="server" Text="Restaurar" CssClass="btn btn-primary mostrarload" OnClick="btnRestaurarQuestionario_Click" />
                                <asp:Button ID="Button6" runat="server" Text="Cancelar" CssClass="btn btn-default" OnClick="btn_CancelarRestaurarQuestionario_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </asp:Panel>

        <script type="text/javascript">
            $('body').on('change', '.edicao-questionario .form-group input, .edicao-questionario .form-group textarea', function (e) {
                setTimeout(function () { $('#lnkQuestionarioEdicao').get(0).click() }, 1000)
            });

            $('body').on('change', '.pnl-itemQuestionario input, textarea, .pnl-itemQuestionario select', function (e) {
                setTimeout(function () { $('#lnkItemQuestionarioEdicao').get(0).click() }, 1000)
            });


            $('body').on('change', '.pnl-itemQuestionarioOpcao input, .pnl-itemQuestionarioOpcao textarea', function (e) {
                if (verficarOnChangeCampoOpcaoQuestionario()) {
                    return;
                }
                setTimeout(function () { $('#lnkItemQuestionarioOpcaoEdicao').get(0).click() }, 1000)
            });

            function verficarOnChangeCampoOpcaoQuestionario() {
                return $(e.target).attr('id').indexOf('ContentPlaceHolder1_ContentPlaceHolder1_txtItemQuestionarioOpcao') != -1;
            }
        </script>
    </fieldset>
</asp:Content>
