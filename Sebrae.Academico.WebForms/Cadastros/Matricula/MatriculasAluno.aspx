<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" EnableViewState="true" EnableEventValidation="false"
    ValidateRequest="false" CodeBehind="MatriculasAluno.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.Matricula.MatriculasAluno" %>

<%@ Register Src="~/UserControls/ucLupaUsuario.ascx" TagName="ucLupaUsuario" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/ucMatriculaTurma.ascx" TagName="ucMatriculaTurma" TagPrefix="uc5" %>
<%@ Register Src="~/UserControls/ucExibirQuestionarioResposta.ascx" TagName="ucExibirQuestionarioResposta" TagPrefix="uc6" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <asp:ScriptManager ID="scriptManager" runat="server"></asp:ScriptManager>

    <fieldset>
        <div class="form-group">
            <asp:Label ID="Label5" runat="server" Text="Gerenciar Matrículas Abertas" AssociatedControlID="rblGerenciarMatriculasAbertas" />
            <asp:RadioButtonList ID="rblGerenciarMatriculasAbertas" runat="server" OnSelectedIndexChanged="rblGerenciarMatriculasAbertas_SelectedIndexChanged" 
                CssClass="form-control" RepeatDirection="Horizontal" AutoPostBack="true">
            </asp:RadioButtonList>
        </div>
        <div id="divLupaUsuario" runat="server" class="form-group">
            <uc:LupaUsuario ID="ucLupaUsuario" runat="server" />
        </div>
        <div class="form-group">
            <asp:Button runat="server" CssClass="btn btn-default" ID="btnFiltrarMatriculas" OnClick="btnPesquisar_Click" Text="Pesquisar" />
        </div>
    </fieldset>
    <asp:Panel ID="pnlMatricula" runat="server" Visible="false">
        <div class="panel-group" id="accordionGM">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <asp:LinkButton runat="server" ClientIDMode="Static" ID="lkbAlunosMatriculados" OnClick="lkbAlunosMatriculados_Click"
                        Text="Alunos Matriculados"></asp:LinkButton>
                </div>
                <div id="collapseMatriculados" class="panel-collapse collapse in" runat="server" clientidmode="Static">
                    <div class="panel-body">
                        <div class="table-responsive">
                            <asp:GridView ID="dgvMatriculaOferta" runat="server" AutoGenerateColumns="false"
                                OnRowCommand="dgvMatriculaOferta_RowCommand" OnRowDataBound="dgvMatriculaOferta_RowDataBound"
                                CssClass="table col-sm-12" GridLines="None" OnPreRender="dgvMatriculaOferta_PreRender">
                                <Columns>
                                    <asp:TemplateField HeaderText="Solução Educacional">
                                        <ItemTemplate>
                                            <asp:Label ID="Label7" runat="server" Text='<%# Bind("Oferta.SolucaoEducacional.Nome") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Oferta">
                                        <ItemTemplate>
                                            <asp:Label ID="Label8" runat="server" Text='<%# Bind("Oferta.Nome") %>'></asp:Label>
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
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlUcInformacoesDetalhadasProvasRealizadas" runat="server" Visible="false">
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
                            <asp:HiddenField runat="server" ID="hdfModalConfirmacaoStatusInscritoIdMatriculaOferta" ClientIDMode="Static" />
                            <asp:HiddenField runat="server" ID="hdfModalConfirmacaoStatusInscritoIdMatriculaTurma" ClientIDMode="Static" />
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnModalConfirmacaoSim" runat="server" Text="Sim" CssClass="btn btn-default" OnClick="btnSimModalConfirmacaoStatusInscrito_OnClick" />
                        <asp:Button ID="btnModalConfirmacaoNão" runat="server" Text="Não" CssClass="btn btn-default" OnClick="OcultarModalConfirmacaoStatusInscrito_Click" />
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->
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
                            <asp:HiddenField runat="server" ID="hdfModalDataConclusaoIdMatriculaOferta" ClientIDMode="Static" />
                            <asp:HiddenField runat="server" ID="hdfModalDataConclusaoIdStatusMatriculaOferta" ClientIDMode="Static" />
                            <asp:HiddenField runat="server" ID="hdfModalDataConclusaoIdMatriculaTurma" ClientIDMode="Static" />
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnSalvarModalDataConclusao" runat="server" Text="Salvar" CssClass="btn btn-default" OnClick="btnSalvarModalDataConclusao_OnClick" />
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->
    </asp:Panel>
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
     
</asp:Content>