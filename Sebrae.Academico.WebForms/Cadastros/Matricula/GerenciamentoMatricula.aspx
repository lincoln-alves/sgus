<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" EnableViewState="true" EnableEventValidation="false"
    ValidateRequest="false" CodeBehind="GerenciamentoMatricula.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.Matricula.GerenciamentoMatricula" %>

<%@ Register Src="~/UserControls/ucMatriculaOferta.ascx" TagName="ucMatriculaOferta" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/ucTurma.ascx" TagName="ucTurma" TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/ucOferta.ascx" TagName="ucOferta" TagPrefix="uc4" %>
<%@ Register Src="~/UserControls/ucMatriculaTurma.ascx" TagName="ucMatriculaTurma" TagPrefix="uc5" %>
<%@ Register Src="~/UserControls/ucExibirQuestionarioResposta.ascx" TagName="ucExibirQuestionarioResposta" TagPrefix="uc6" %>

<%@ Register TagName="ucCategorias" TagPrefix="uc7" Src="~/UserControls/ucCategorias.ascx" %>
<%@ Register Src="~/UserControls/ucQuestionario.ascx" TagPrefix="uc2" TagName="ucQuestionario" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ScriptManager ID="scriptManager" runat="server"></asp:ScriptManager>
    <link href="/css/print.css" rel="stylesheet" media="print" />
    <script type="text/javascript">
        var _preloadedListSE = <%= ViewState["_SE"] ?? "''" %>;
        AutoCompleteDefine(_preloadedListSE, '#txtSolucaoEducacional', true, false, true);
        
        var _preloadedListOferta = <%= ViewState["_Oferta"] ?? "[]" %>;
        AutoCompleteDefine(_preloadedListOferta, '#txtOferta', true, false, true);
        
        var _preloadedListTurma = <%= ViewState["_Turma"] ?? "[]" %>;
        AutoCompleteDefine(_preloadedListTurma, '#txtTurma', true, false, true);
    </script>

    <fieldset>
        <div class="form-group">
            <asp:Label ID="Label5" runat="server" Text="Gerenciar Matrículas Abertas" AssociatedControlID="rblGerenciarMatriculasAbertas" />
            <asp:RadioButtonList ID="rblGerenciarMatriculasAbertas" runat="server" CssClass="form-control"
                RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblGerenciarMatriculasAbertas_OnSelectedIndexChanged">
            </asp:RadioButtonList>
        </div>
        <%--<div id="divLupaUsuario" runat="server" class="form-group">
            <uc:LupaUsuario ID="LupaUsuario" runat="server" OnUserSelected="UserSelectedHandler" />
        </div>--%>
        <div id="DvAreaPrograma" runat="server" visible="false">
            <div class="form-group">
                <asp:Label ID="Label2" runat="server" Text="Programa" AssociatedControlID="txtPrograma" />
                <asp:TextBox ID="txtPrograma" ClientIDMode="Static" runat="server" OnTextChanged="txtPrograma_OnTextChanged"></asp:TextBox>
                <script>
                    var _preloadedListPrograma = <%= ViewState["_Programa"] ?? "''" %>;
                    AutoCompleteDefine(_preloadedListPrograma, '#txtPrograma', true, false, true);
                </script>
            </div>
            <div class="form-group">
                <asp:Label ID="Label3" runat="server" Text="Oferta de programa" AssociatedControlID="txtCapacitacao" />
                <asp:TextBox ID="txtCapacitacao" ClientIDMode="Static" runat="server" OnTextChanged="txtCapacitacao_OnTextChanged" data-mensagemVazia="Selecione um Programa"></asp:TextBox>
                <script>
                    var _preloadedListCpacitacao = <%= ViewState["_Capacitacao"] ?? "''" %>;
                    AutoCompleteDefine(_preloadedListCpacitacao, '#txtCapacitacao', true, false, true);
                </script>
            </div>
            <div class="form-group">
                <asp:Label ID="Label4" runat="server" Text="Módulo" AssociatedControlID="txtModulo" />
                <asp:TextBox ID="txtModulo" ClientIDMode="Static" runat="server" OnTextChanged="txtModulo_OnTextChanged" data-mensagemVazia="Selecione uma Capacitação"></asp:TextBox>
                <script>
                    var _preloadedListModulo = <%= ViewState["_Modulo"] ?? "''" %>;
                    AutoCompleteDefine(_preloadedListModulo, '#txtModulo', true, false, true);
                </script>
            </div>
        </div>
        <!--<div id="DvAreaCategoria" runat="server" class="form-group">
            <asp:Label ID="Label10" runat="server" Text="CATEGORIAS" AssociatedControlID="ucCategorias1" />
            <uc7:ucCategorias ID="ucCategorias1" runat="server" />
        </div>-->
        <div class="form-group">
            <asp:Label ID="Label1" runat="server" Text="Solução Educacional" AssociatedControlID="txtSolucaoEducacional" />
            <asp:TextBox ID="txtSolucaoEducacional" ClientIDMode="Static" runat="server" OnTextChanged="txtSolucaoEducacional_OnTextChanged" data-mensagemVazia="Nenhum item a ser exibido"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="lblOferta" runat="server" Text="Oferta" AssociatedControlID="txtOferta" />
            <asp:TextBox ID="txtOferta" ClientIDMode="Static" runat="server" OnTextChanged="txtOferta_OnTextChanged" data-mensagemVazia="Selecione uma Solução Educacional"></asp:TextBox>
        </div>
       <%-- <div class="form-group">
            <asp:Label ID="Label99" runat="server" Text="Turma" AssociatedControlID="txtTurma" />
            <asp:TextBox ID="txtTurma" ClientIDMode="Static" runat="server" OnTextChanged="txtTurma_OnTextChanged" data-mensagemVazia="Selecione uma Oferta"></asp:TextBox>
        </div>--%>

        <div class="form-group">
            <%--<asp:Button runat="server" CssClass="btn btn-default" ID="btnFiltrarMatriculas" OnClick="btnFiltrarMatriculas_Click" Text="Pesquisar" />--%>
            <asp:Button ID="btnVerDetalhes" runat="server" Text="Ver Detalhes" Visible="false"
                CssClass="btn btn-primary mostrarload" OnClick="btnVerDetalhes_OnClick" />
            <asp:Button ID="btnVerTermoAceite" runat="server" Text="Ver Termo de Aceite" Visible="false"
                CssClass="btn btn-primary mostrarload" OnClick="btnVerTermoAceite_OnClick" />
            <asp:Button ID="btnVerPoliticaConsequencia" runat="server" Text="Ver Política de Consequência"
                Visible="false" CssClass="btn btn-primary mostrarload" OnClick="btnVerPoliticaConsequencia_OnClick" />
            <asp:Button ID="btnModoAvaliacao" runat="server" Visible="false" CssClass="btn btn-primary mostrarload" OnClick="btnModoAvaliacao_OnClick" />
        </div>
        <div class="form-group" style="display: none;">
            <asp:Button ID="btnAdicionarOferta" runat="server" Text="Adicionar Oferta" Visible="false"
                OnClick="btnAdicionarOferta_Click" CssClass="btn btn-primary mostrarload" />
            <asp:Button ID="btnEditarOferta" OnClick="btnEditarOferta_Click" runat="server" Text="Editar Oferta"
                Visible="false" CssClass="btn btn-default mostrarload" />
        </div>
    </fieldset>
    <asp:Panel ID="pnlMatricula" runat="server" Visible="false">
        <div class="panel-group" id="accordionGM">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <asp:LinkButton runat="server" ClientIDMode="Static" ID="lkbAlunosMatriculados" OnClick="lkbAlunosMatriculados_Click"
                        Text="Alunos Matriculados"></asp:LinkButton>
                </div>
                <div id="collapseMatriculados" class="panel-collapse collapse" runat="server" clientidmode="Static">
                    <div class="panel-body">
                        <div>
                            <asp:GridView ID="dgvMatriculaOferta" runat="server" AutoGenerateColumns="false"
                                OnRowCommand="dgvMatriculaOferta_RowCommand" OnRowDataBound="dgvMatriculaOferta_RowDataBound"
                                CssClass="table col-sm-12" GridLines="None">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkAlterarStatusLote" runat="server" Visible="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Aluno">
                                        <ItemTemplate>
                                            <asp:Label ID="Label6" runat="server" Text='<%# Bind("Usuario.Nome") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Uf">
                                        <ItemTemplate>
                                            <asp:Label ID="Label66" runat="server" Text='<%# Bind("Usuario.UF.Sigla") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Data Conclusão">
                                        <ItemTemplate>
                                            <asp:Label ID="Label662" runat="server" Text='<%# Bind("DataConclusao") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Nota Final">
                                        <ItemTemplate>
                                            <asp:Label ID="Label661" runat="server" Text='<%# Bind("NotaFinal") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status Matrícula">
                                        <ItemTemplate>
                                            <asp:DropDownList runat="server" ID="ddlStatusOferta" ClientIDMode="Static" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlStatusOferta_SelectedIndexChanged" ViewStateMode="Enabled"
                                                CssClass="mostrarload" EnableViewState="true" Width="200">
                                            </asp:DropDownList>
                                            <asp:Label ID="statusOferta" runat="server" Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Turma">
                                        <ItemTemplate>
                                            <asp:HiddenField runat="server" ID="hdfIdMatriculaOferta" ClientIDMode="Static" />
                                            <asp:DropDownList runat="server" ID="ddlTurma" ClientIDMode="Static" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlTurma_SelectedIndexChanged" CausesValidation="true"
                                                CssClass="mostrarload" ViewStateMode="Enabled" EnableViewState="true" Width="250">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemStyle Width="90px" HorizontalAlign="center" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lkbEditarMatriculaTurma" runat="server" CausesValidation="False"
                                                CssClass="mostrarload" Visible="false" CommandName="editarMatTurma" Text="Editar Mat. Turma"
                                                ToolTip="Edita as informações da matrícula de uma turma">
                                                    <span class="btn btn-default btn-xs">
								                        <span class="glyphicon glyphicon-pencil"></span>
							                        </span>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="lkbEnviarMatricula" runat="server" CausesValidation="False" CssClass="mostrarload"
                                                Visible="false" CommandName="enviarMatricula" Text="Export Mat. Turma" ToolTip="Enviar matrícula para o fornecedor">
                                                    <span class="btn btn-default btn-xs">
								                        <span class="glyphicon glyphicon-export"></span>
							                        </span>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="lkbEnviarMatriculaEmailPendente" runat="server" CausesValidation="False"
                                                CssClass="mostrarload" Visible="false" CommandName="enviarEmailPendente" Text="Enviar email para o usuário"
                                                ToolTip="Enviar o email sobre a pendência para o usuário">
                                                    <span class="btn btn-default btn-xs">
								                        <span class="glyphicon glyphicon-send"></span>
							                        </span>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="lbEmitirCertificado" runat="server" CausesValidation="False"
                                                Visible="false" CommandName="emitirCertificado" Text="Emitir Certificado"
                                                ToolTip="Emitir Certificado">
                                                    <span class="btn btn-default btn-xs">
								                        <span class="glyphicon glyphicon-save"></span>
							                        </span>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
                                </EmptyDataTemplate>
                            </asp:GridView>

                            <div class="col-sm-12 manualPagination">
                                <ul class="list-unstyled list-inline">
                                    <asp:Repeater ID="rptMatriculaOfertaPager" runat="server">
                                        <ItemTemplate>
                                            <li>
                                                <asp:LinkButton ID="lnkPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
                                                    CssClass='<%# Convert.ToBoolean(Eval("Enabled")) ? "current" : "mostrarload" %>'
                                                    OnClick="lnkPage_Click"></asp:LinkButton>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                            </div>

                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <asp:LinkButton ID="lkbAlterarStatusEmLote" ClientIDMode="Static" OnClick="lkbAlterarStatusEmLote_OnClick" runat="server" Text="Alterar informações em lote"></asp:LinkButton>
                                </div>
                                <div class="panel-collapse collapse" id="collapseStatusEmLote" runat="server">
                                    <div class="panel-body">
                                        <div class="form-group">
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <asp:Label ID="Label11" runat="server" Text="Status de matrícula" AssociatedControlID="ddlStatusEmLoteOferta" />
                                                        <asp:DropDownList runat="server" ID="ddlStatusEmLoteOferta" ClientIDMode="Static"
                                                            ViewStateMode="Enabled"
                                                            EnableViewState="true" CssClass="form-control">
                                                        </asp:DropDownList>

                                                    </div>
                                                    <div class="form-group">
                                                        <asp:CheckBox Text="Selecionar todos" ID="chkTodosAlteracaoEmLote" OnCheckedChanged="chkTodosAlteracaoEmLote_CheckedChanged" AutoPostBack="true" runat="server" />
                                                    </div>
                                                </div>

                                                <div runat="server" id="divDataNotaLote">
                                                    <div class="col-md-4">
                                                        <asp:Label ID="Label16" runat="server" Text="Data de conclusão do lote" AssociatedControlID="txtDtConclusaoLote" />
                                                        <asp:TextBox ID="txtDtConclusaoLote" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="Label12" runat="server" Text="Nota Final do lote" AssociatedControlID="txtNotaFinalLote" />
                                                        <asp:TextBox ID="txtNotaFinalLote" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <asp:Button ID="btnAtualizarStatus" Text="Atualizar Status" runat="server" CssClass="btn btn-default mostrarload" OnClick="btnAtualizarStatus_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="panel panel-default" id="dvGerenciadorVagas" runat="server" visible="false">
                <div class="panel-heading">
                    <asp:LinkButton runat="server" ClientIDMode="Static" ID="lbGerenciadorVagas" OnClick="lbGerenciadorVagas_Click"
                        Text="Quantidade de Vagas"></asp:LinkButton>
                </div>
                <div id="collapseGerenciadorVagas" class="panel-collapse collapse" runat="server" clientidmode="Static">
                    <div class="panel-body">
                        <div class="form-group">
                            <asp:Label ID="Label9" runat="server" Text="Quantidade de Vagas Disponíveis" AssociatedControlID="lblQtdeVagasDisponiveis" />
                            <asp:Label ID="lblQtdeVagasDisponiveis" runat="server" Text="" Font-Bold="true" CssClass="form-control"></asp:Label>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label7" runat="server" Text="Recusar Vagas" AssociatedControlID="ddlQtdeVagas" />
                            <asp:DropDownList ID="ddlQtdeVagas" runat="server" AutoPostBack="True" ClientIDMode="Static"
                                CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label8" runat="server" Text="Observação" AssociatedControlID="txtObservacaoQtdeVagas" />
                            <asp:TextBox ID="txtObservacaoQtdeVagas" runat="server" ClientIDMode="Static" CssClass="form-control"
                                TextMode="MultiLine"></asp:TextBox>
                        </div>
                        <br />
                        <asp:Button ID="btnGerenciarVagas" runat="server" Text="Enviar" CssClass="btn btn-default"
                            OnClick="btnGerenciarVagas_OnClick" OnClientClick="return ConfirmarExclusao('Enviar esta observação?');" />
                    </div>
                </div>
            </div>
            <div class="panel panel-default" id="dvMatricularAluno" runat="server">
                <div class="panel-heading">
                    <asp:LinkButton ID="lkbMatricularAluno" runat="server" OnClick="lkbMatricularAluno_OnClick"
                        ClientIDMode="Static" Text="Matricular Aluno"></asp:LinkButton>
                </div>
                <div id="collapseMatriculaOferta" class="panel-collapse collapse" runat="server"
                    clientidmode="Static">
                    <div class="panel-body">
                        <uc2:ucMatriculaOferta ID="ucMatriculaOferta1" runat="server" OnMatriculouAlunoEmUmaOferta="MatriculaOferta_MatriculouAlunoEmUmaOferta" />
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <asp:LinkButton ID="lkbMatricularAlunosEmLote" ClientIDMode="Static" OnClick="lkbMatricularAlunosEmLote_OnClick" runat="server" Text="Matricular Alunos em Lote"></asp:LinkButton>
                            </div>
                            <div class="panel-collapse collapse" id="collapseAlunosEmLote" runat="server">
                                <div class="panel-body">
                                    <div class="form-group">
                                        <asp:Label runat="server">Planilha de exemplo:</asp:Label>
                                        <a href="/arquivos/planilha_modelo_matricula_em_lote.xlsx" target="_blank"><span class="glyphicon glyphicon-download-alt" style="font-size: 30px" runat="server" id="planilhaDownload"></span></a>
                                    </div>
                                    <div class="form-group">
                                        <input type="file" id="fileUpload" runat="server" class="fomr-control" />
                                    </div>
                                    <div class="form-group">
                                        <asp:Button ID="btnEnviarArquivo" Text="Enviar Arquivo" runat="server" CssClass="btn btn-default" OnClick="btnEnviarArquivo_Click" />
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" Visible="False" ID="lblMatriculaTodos">Alterar status matricula para todos</asp:Label>
                                        <asp:DropDownList runat="server" ID="ddlStatusTodosEmLote" OnSelectedIndexChanged="ddlStatusTodosEmLote_OnSelectedIndexChanged" Visible="False" AutoPostBack="True" />

                                        <asp:Label runat="server" Visible="False" ID="lblTurmaTodos">Alterar turma para todos</asp:Label>
                                        <asp:DropDownList runat="server" ID="ddlTurmaTodosEmLote" OnSelectedIndexChanged="ddlTurmaTodosEmLote_OnSelectedIndexChanged" Visible="False" AutoPostBack="True" />
                                    </div>

                                    <asp:GridView runat="server" ID="grdMatriculaEmLote" AutoGenerateColumns="false" CssClass="table form-group" OnRowDataBound="grdMatriculaEmLote_OnRowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="F1" HeaderText="Nome" />
                                            <asp:BoundField DataField="F2" HeaderText="E-mail" />
                                            <asp:BoundField DataField="F3" HeaderText="CPF" />
                                            <asp:TemplateField HeaderText="Status Matrícula">
                                                <ItemTemplate>
                                                    <asp:DropDownList runat="server" ID="ddlStatusOfertaMatriculaEmLote" AutoPostBack="true" CssClass="mostrarload" Width="200">
                                                    </asp:DropDownList>
                                                    <asp:Label ID="statusOferta" runat="server" Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Turma">
                                                <ItemTemplate>
                                                    <asp:HiddenField runat="server" ID="hdfIdMatriculaOferta" ClientIDMode="Static" />
                                                    <asp:DropDownList runat="server" ID="ddlTurmaEmLote" AutoPostBack="true" CausesValidation="true"
                                                        CssClass="mostrarload" ViewStateMode="Enabled" EnableViewState="true" Width="250" ClientIDMode="Static">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <div>
                                        <asp:Button Text="Matricular Alunos" runat="server" CssClass="btn btn-primary" ID="btnMatricularEmLote" OnClick="btnMatricularEmLote_OnClick" Visible="False" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="panel panel-default" id="dvTurmasCadastradas" runat="server">
                <div class="panel-heading">
                    <asp:LinkButton ID="lkbTurmasCadastradas" runat="server" OnClick="lkbTurmasCadastradas_Click"
                        ClientIDMode="Static" Text="Turmas Cadastradas"></asp:LinkButton>
                </div>
                <div id="collapseTurmas" class="panel-collapse collapse" runat="server" clientidmode="Static">
                    <div class="panel-body">
                        <asp:GridView ID="dgvTurmasDaOferta" runat="server" AutoGenerateColumns="false" GridLines="None"
                            CssClass="table col-sm-12" OnRowCommand="dgvTurmasDaOferta_RowCommand" OnRowDataBound="dgvTurmasDaOferta_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Turma">
                                    <ItemTemplate>
                                        <%#Eval("Nome")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Data de Início">
                                    <ItemTemplate>
                                        <%#Eval("DataInicio", "{0:dd/MM/yyyy}")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Data Final">
                                    <ItemTemplate>
                                        <%#Eval("DataFinal", "{0:dd/MM/yyyy}")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Matriculados">
                                    <ItemTemplate>
                                        <%#Eval("QuantidadeAlunosMatriculadosNaTurma")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemStyle Width="90px" HorizontalAlign="center" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lkbEditar" runat="server" CausesValidation="False" CommandName="editarTurma"
                                            ToolTip="Edita as informações de uma turma" CommandArgument='<%# Eval("ID")%>'
                                            CssClass="mostrarload">
                                            <span class="btn btn-default btn-xs">
								                <span class="glyphicon glyphicon-pencil"></span>
							                </span>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="lkbExcluir" runat="server" CausesValidation="False" CommandName="excluirTurma"
                                            ToolTip="Exclui as informações de uma turma" CommandArgument='<%# Eval("ID")%>'
                                            OnClientClick="return ConfirmarExclusao();">
                                            <span class="btn btn-default btn-xs">
								                <span class="glyphicon glyphicon-remove"></span>
							                </span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
                            </EmptyDataTemplate>
                        </asp:GridView>
                        <br />
                        <asp:Button ID="btnAdicionarTurma" runat="server" Text="Adicionar Turma" Enabled="false"
                            OnClick="btnAdicionarTurma_Click" UseSubmitBehavior="False" CssClass="btn btn-primary mostrarload" />
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <!-- MODAL Data Termino #3623 -->
    <asp:Panel ID="pnlModalDataConclusao" runat="server" Visible="false">
        <div class="modal fade in" id="ModalDataTermino" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: block;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button id="Button9" type="button" class="close" data-dismiss="modal" aria-hidden="true" onserverclick="OcultarModalDataConclusao_Click" runat="server">&times;</button>
                        <h4 class="modal-title">Alteração de Status da Matrícula</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            Data Conclusão
                            <asp:TextBox ID="txtModalDataConclusao" runat="server" class="form-control"></asp:TextBox>
                        </div>
                        <asp:HiddenField runat="server" ID="hdfModalDataConclusaoIdMatriculaOferta" ClientIDMode="Static" />
                        <asp:HiddenField runat="server" ID="hdfModalDataConclusaoIdStatusMatriculaOferta" ClientIDMode="Static" />
                        <asp:HiddenField runat="server" ID="hdfModalDataConclusaoIdMatriculaTurma" ClientIDMode="Static" />
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnSalvarModalDataConclusao" runat="server" Text="Salvar" CssClass="btn btn-primary" OnClick="btnSalvarModalDataConclusao_OnClick" />
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->
    </asp:Panel>
    <!-- MODAL Data Termino #3623 -->
    <asp:Panel ID="pnlModalConfirmacaoStatusInscrito" runat="server" Visible="false">
        <div class="modal fade in" id="ModalDataTermino" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: block;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button id="Button10" type="button" class="close" data-dismiss="modal" aria-hidden="true" onserverclick="OcultarModalConfirmacaoStatusInscrito_Click" runat="server">&times;</button>
                        <h4 class="modal-title">Alteração de Status da Matrícula</h4>
                    </div>
                    <div class="modal-body">
                        <div class="alert alert-warning" role="alert">
                            <b>Atenção!</b> Esta alteração fará com que a data de conclusão do aluno fique vazia.<br />
                            <br />
                            <b>Deseja realmente continuar com esta alteração?</b>
                            <asp:HiddenField runat="server" ID="hdfStatusMatricula" ClientIDMode="Static" />
                            <asp:HiddenField runat="server" ID="hdfModalConfirmacaoStatusInscritoIdMatriculaOferta" ClientIDMode="Static" />
                            <asp:HiddenField runat="server" ID="hdfModalConfirmacaoStatusInscritoIdMatriculaTurma" ClientIDMode="Static" />
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnModalConfirmacaoSim" runat="server" Text="Sim" CssClass="btn btn-primary" OnClick="btnSimModalConfirmacaoStatusInscrito_OnClick" />
                        <asp:Button ID="btnModalConfirmacaoNão" runat="server" Text="Não" CssClass="btn btn-default" OnClick="OcultarModalConfirmacaoStatusInscrito_Click" />
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->
    </asp:Panel>
    <!-- MODAL MATRICULA TURMA-->
    <asp:Panel ID="pnlModalMatriculaTurma" runat="server" Visible="false">
        <div class="modal fade in" id="ModalMatriculaTurma" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
            aria-hidden="true" style="display: block;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button id="Button1" type="button" class="close" data-dismiss="modal" aria-hidden="true"
                            onserverclick="OcultarMatriculaTurma_Click" runat="server">
                            &times;</button>
                        <h4 class="modal-title">Edição Matrícula</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            Nome
                            <asp:TextBox ID="txtNomeModalMatriculaTurma" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            CPF
                            <asp:TextBox ID="txtCpfModalMatriculaTurma" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            UF
                            <asp:TextBox ID="txtUfModalMatriculaTurma" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            E-mail
                            <asp:TextBox ID="txtEmailModalMatriculaTurma" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                        <uc5:ucMatriculaTurma ID="ucMatriculaTurma1" runat="server" OnMatriculouAlunoEmUmaTurma="MatricularAlunoNaTurma_MatriculouAlunoEmUmaTurma"
                            OnSelecionouUmaProva="SelecionarProvaDeUmAluno_SelecionouProvaDeUmAluno" />
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->
    </asp:Panel>
    <asp:Panel ID="pnlModalVisualizacaoRapida" runat="server" Visible="false">
        <div class="modal fade in" id="ModalVisualizacaoRapida" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
            aria-hidden="true" style="display: block;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button id="Button11" type="button" class="close" data-dismiss="modal" aria-hidden="true"
                            onserverclick="OcultarVisualizacaoRapida_Click" runat="server">
                            &times;</button>
                        <h4 class="modal-title">Visualização Rápida</h4>
                    </div>
                    <div class="modal-body">

                        <style>
                            .avaliar-default {
                                -moz-min-width: 200px;
                                -ms-min-width: 200px;
                                -o-min-width: 200px;
                                -webkit-min-width: 200px;
                                min-width: 200px;
                            }
                        </style>
                        <div class="table-responsive">
                            <table class="table table-condensed">
                                <thead>
                                    <tr>
                                        <th colspan="2" style="padding: 5px; border-bottom: 2px solid #dddddd;" class="text-center">
                                            <asp:Literal ID="ltrSolucaoEducacional" runat="server"></asp:Literal>
                                        </th>
                                        <th colspan="<%= QuantidadeQuestoes %>" style="padding: 5px; border: none; border-bottom: 2px solid #dddddd; border-left: 2px solid #dddddd;" class="text-center">Quesitos para avaliação do participante
                                        </th>
                                    </tr>
                                    <tr>
                                        <td width="200" style="padding: 5px; display: table-cell; vertical-align: bottom;">
                                            <strong>Nome
                                            </strong>
                                        </td>
                                        <td style="padding: 5px; display: table-cell; vertical-align: bottom; border: none; border-right: 2px solid #dddddd;">
                                            <strong>UF
                                            </strong>
                                        </td>
                                        <asp:Repeater ID="rptQuestoes" runat="server" OnItemDataBound="rptQuestoes_OnItemDataBound">
                                            <ItemTemplate>
                                                <td style="text-transform: none; font-size: 12px; border: none; border-left: 2px solid #dddddd;">
                                                    <p>
                                                        <strong>
                                                            <asp:Literal ID="ltrTitulo" runat="server"></asp:Literal>
                                                        </strong>
                                                    </p>
                                                    <asp:Literal ID="ltrQuestao" runat="server"></asp:Literal>
                                                </td>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater ID="rptMatriculas" runat="server" OnItemDataBound="rptMatriculas_OnItemDataBound">
                                        <ItemTemplate>
                                            <tr>
                                                <td style="padding: 5px; display: inline-block; width: 300px; border: none;">
                                                    <asp:HiddenField ID="hdnIdMatriculaTurma" runat="server" />

                                                    <asp:Literal ID="ltrNome" runat="server"></asp:Literal>
                                                </td>
                                                <td style="padding: 5px; border: none;">
                                                    <asp:Literal ID="ltrUf" runat="server"></asp:Literal>
                                                </td>
                                                <asp:Repeater ID="rptQuestaoResposta" runat="server" OnItemDataBound="rptQuestaoResposta_OnItemDataBound">
                                                    <ItemTemplate>
                                                        <td class="avaliar-default" style="padding: 5px; border: none; border-left: 2px solid #dddddd;">
                                                            <asp:Literal ID="ltrLabel" runat="server" Visible="False"></asp:Literal>
                                                            <asp:Literal ID="ltrValor" runat="server"></asp:Literal>
                                                        </td>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->
    </asp:Panel>
    <!-- MODAL OFERTA-->
    <asp:Panel ID="pnlModalOferta" runat="server" Visible="false">
        <div class="modal fade in" id="ModalOferta" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
            aria-hidden="true" style="display: block;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button id="Button2" type="button" class="close" data-dismiss="modal" aria-hidden="true"
                            onserverclick="OcultarModalOferta_Click" runat="server">
                            &times;</button>
                        <h4 class="modal-title">Criar Oferta</h4>
                    </div>
                    <div class="modal-body">
                        <uc4:ucOferta ID="ucOferta1" runat="server" />
                        <asp:Button ID="btnSalvarOferta" runat="server" Text="Enviar" OnClick="btnSalvarOferta_Click"
                            CssClass="btn btn-default mostrarload" />
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <!-- MODAL TURMA-->
    <asp:Panel ID="pnlModalTurma" runat="server" Visible="false">
        <div class="modal fade in" id="ModalTurma" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
            aria-hidden="true" style="display: block;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button id="Button3" type="button" class="close" data-dismiss="modal" aria-hidden="true"
                            onserverclick="OcultarTurma_Click" runat="server">
                            &times;</button>
                        <h4 class="modal-title">Criar Turma</h4>
                    </div>
                    <div class="modal-body">
                        <uc3:ucTurma ID="ucTurma1" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnSalvarTurma" runat="server" Text="Salvar" CssClass="btn btn-default" OnClick="btnSalvarTurma_OnClick" />
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlUcInformacoesDetalhadasProvasRealizadas" runat="server">
        <div class="modal fade in" id="ModalInformacoesDetalhadasDaProvaRealizada" tabindex="-1"
            role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: block;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button id="Button4" type="button" class="close" data-dismiss="modal" aria-hidden="true"
                            onserverclick="OcultarInformacoesDetalhadasDaProvaRealizada_Click" runat="server">
                            &times;</button>
                        <h4 class="modal-title">Provas</h4>
                    </div>
                    <div class="modal-body">
                        <uc6:ucExibirQuestionarioResposta ID="ucExibirQuestionarioResposta" runat="server"
                            OnExibiuResposta="ExibirResposta_ExibiuResposta" />
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlLupaDetalhes" runat="server" Visible="false">
        <div aria-hidden="false" aria-labelledby="myModalLabel" role="dialog" tabindex="-1"
            id="ModalLupa" class="modal fade in" style="display: block;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button id="Button5" type="button" class="close" data-dismiss="modal" aria-hidden="true"
                            onserverclick="OcultarModal_Click" runat="server">
                            &times;</button>
                        <h4 class="modal-title">
                            <asp:Literal ID="liTituloGrande" runat="server"></asp:Literal></h4>
                    </div>
                    <div class="modal-body">
                        <p>
                            <b><asp:Literal ID="liTituloPequeno" runat="server"></asp:Literal></b>
                            <asp:Literal ID="liTextoPortal" runat="server"></asp:Literal>
                        </p>
                    </div>
                </div>
            </div>

        </div>
    </asp:Panel>
    <asp:Panel ID="pnlTermosAceite" runat="server" Visible="false">
        <div aria-hidden="false" aria-labelledby="myModalLabel" role="dialog" tabindex="-1"
            id="Div1" class="modal fade in" style="display: block;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button id="Button6" type="button" class="close" data-dismiss="modal" aria-hidden="true"
                            onserverclick="OcultarModal_Click" runat="server">
                            &times;</button>
                        <h4 class="modal-title">Termo de Aceite</h4>
                        <div class="modal-body">
                            <p>
                                <b><asp:Literal ID="lblTermoAceite" runat="server"></asp:Literal></b>
                            </p>
                            <p>
                                <asp:Literal ID="lblTermoAceiteTexto" runat="server"></asp:Literal>
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlPoliticaConsequencia" runat="server" Visible="false">
        <div aria-hidden="false" aria-labelledby="myModalLabel" role="dialog" tabindex="-1"
            id="Div2" class="modal fade in" style="display: block;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button id="Button7" type="button" class="close" data-dismiss="modal" aria-hidden="true"
                            onserverclick="OcultarModal_Click" runat="server">
                            &times;</button>
                        <h4 class="modal-title">Política de Consequência</h4>
                        <div class="modal-body">
                            <p>
                                <b>
                                    <asp:Literal ID="lblPoliticaConsequencia" runat="server"></asp:Literal>
                                </b>
                            </p>
                            <p>
                                <asp:Literal ID="lblPoliticaConsequenciaTexto" runat="server"></asp:Literal>
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlQuestionarioCancelamento" runat="server" Visible="false">
        <div aria-hidden="false" aria-labelledby="myModalLabel" role="dialog" tabindex="-1"
            id="ModalQuestionarioCancelamento" class="modal fade in" style="display: block;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button id="Button8" type="button" class="close" data-dismiss="modal" aria-hidden="true"
                            onserverclick="OcultarModal_Click" runat="server">
                            &times;</button>
                        <h4 class="modal-title">Questionário de cancelamento</h4>
                    </div>
                    <div class="modal-body">
                        <uc2:ucQuestionario runat="server" ID="ucQuestionario" />
                    </div>
                    <div class="modal-footer">
                        <div class="form-group">
                            <asp:Button ID="btnEnviarQuestionario" CssClass="btn btn-primary mostrarload" runat="server" Text="Enviar questionário" OnClick="btnEnviarQuestionario_OnClick" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>

    <script type="text/javascript">
        (function($) {
            $(document).ready(function() {
                var dtModalConclusao = $("#<%= txtModalDataConclusao.ClientID %>");
                dtModalConclusao.mask("99/99/9999", {
                    autoclear: false
                });

                $("#<%= txtDtConclusaoLote.ClientID %>").mask("99/99/9999 99:99:99", {
                    autoclear: false
                }).on('blur', function () {
                    var valor = $("#<%= txtDtConclusaoLote.ClientID %>").val();
                    var dados = valor.split(' ');
                    var data = validDate(dados[0]);
                    if (!data.result) {
                        $("#<%= txtDtConclusaoLote.ClientID %>").val('');
                    } else {
                        dados[1] = dados[1].replace(/_/g, "9");
                        dados[1] = validHour(dados[1]);
                        $("#<%= txtDtConclusaoLote.ClientID %>").val(dados.join(' '));
                    }
                });
            });
        })(jQuery);

        function matriculasEmLote() {
            $('#<%= collapseAlunosEmLote.ClientID %>').collapse();
        }

        function pad2(number) {
            return (number < 10 ? '0' : '') + number;
        }

        function validHour(value) {
            var comp = value.split(':');
            var h = parseInt(comp[0], 10);
            var m = parseInt(comp[1], 10);
            var s = parseInt(comp[2], 10);
            if (h > 23) h = 23;
            if (m > 59) m = 59;
            if (s > 59) s = 59;
            return pad2(h) + ':' + pad2(m) + ':' + pad2(s);
        }

        function validDate(value) {
            var comp = value.split('/');
            var d = parseInt(comp[0], 10);
            var m = parseInt(comp[1], 10);
            var y = parseInt(comp[2], 10);
            var date = new Date(y, m - 1, d);
            return {
                result: (date.getFullYear() === y && date.getMonth() + 1 === m && date.getDate() === d),
                value: value
            }
        }
    </script>
</asp:Content>
