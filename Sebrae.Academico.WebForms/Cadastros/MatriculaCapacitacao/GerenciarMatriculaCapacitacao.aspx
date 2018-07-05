<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="GerenciarMatriculaCapacitacao.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.MatriculaCapacitacao.GerenciarMatriculaCapacitacao" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%@ Register Src="~/UserControls/ucMatriculaCapacitacao.ascx" TagName="ucMatriculaCapacitacao" TagPrefix="uc2" %>
    <fieldset>
        <div class="form-group">
            <asp:Label ID="Label3" runat="server" Text="Gerenciar Matrículas Abertas" AssociatedControlID="rblGerenciarMatriculasAbertas" />
            <asp:RadioButtonList ID="rblGerenciarMatriculasAbertas" runat="server" CssClass="form-control" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblGerenciarMatriculasAbertas_OnSelectedIndexChanged"></asp:RadioButtonList>
        </div>
        <div class="form-group">
            <asp:Label ID="Label1" runat="server" Text="Programa" AssociatedControlID="ddlProgama" />
            <asp:DropDownList ID="ddlProgama" runat="server" AutoPostBack="True" ClientIDMode="Static" CssClass="form-control mostrarload" OnSelectedIndexChanged="ddlProgama_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Label ID="Label2" runat="server" Text="Oferta" AssociatedControlID="ddlCapacitacao" />
            <asp:DropDownList ID="ddlCapacitacao" runat="server" AutoPostBack="True" ClientIDMode="Static" CssClass="form-control mostrarload" Enabled="false" OnSelectedIndexChanged="ddlCapacitacao_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
    </fieldset>
    <asp:Panel ID="pnlMatricula" runat="server" Visible="false">
        <div class="panel-group">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <asp:LinkButton runat="server" ClientIDMode="Static" ID="lkbAlunosMatriculados"
                        Text="Alunos Matriculados"></asp:LinkButton>
                </div>
                <div id="collapseMatriculados" class="panel-collapse" runat="server" clientidmode="Static">
                    <div class="panel-body">
                        <div>
                            <asp:GridView ID="dgvMatriculaCapacitacao" runat="server" AutoGenerateColumns="false"
                                OnRowCommand="dgvMatriculaCapacitacao_RowCommand" OnRowDataBound="dgvMatriculaCapacitacao_RowDataBound"
                                CssClass="table col-sm-12" GridLines="None">
                                <Columns>
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
                                    <asp:TemplateField HeaderText="Nível Ocupacional">
                                        <ItemTemplate>
                                            <asp:Label ID="Label661" runat="server" Text='<%# Bind("Usuario.NivelOcupacional.Nome") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status Matrícula">
                                        <ItemTemplate>
                                            <asp:DropDownList runat="server" ID="ddlStatusOferta" ClientIDMode="Static" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlStatusOferta_SelectedIndexChanged" ViewStateMode="Enabled"
                                                CssClass="mostrarload" EnableViewState="true" Width="300">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Turma">
                                        <ItemTemplate>
                                            <asp:HiddenField runat="server" ID="hdfIdMatriculaOferta" ClientIDMode="Static" />
                                            <asp:DropDownList runat="server" ID="ddlTurma" ClientIDMode="Static" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlTurma_SelectedIndexChanged" CausesValidation="true"
                                                CssClass="mostrarload" ViewStateMode="Enabled" EnableViewState="true" Width="300">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False" Visible="false">
                                        <ItemStyle Width="90px" HorizontalAlign="center" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lkbEditarMatriculaTurma" runat="server" CausesValidation="False"
                                                CssClass="mostrarload" Visible="false" CommandName="editarMatTurma" Text="Editar Mat. Turma"
                                                ToolTip="Edita as informações da matrícula de uma turma">
                                                <span class="btn btn-default btn-xs">
								                    <span class="glyphicon glyphicon-pencil"></span>
							                    </span>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="lkbEnviarMatricula" runat="server" CausesValidation="False"
                                                CssClass="mostrarload" Visible="false" CommandName="enviarMatricula" Text="Export Mat. Turma"
                                                ToolTip="Enviar matrícula para o fornecedor">
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
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                        <div class="form-group">
                            <%--<asp:Button ID="btnEnviarEmail" runat="server" Text="Enviar E-mail" OnClick="btnEnviarEmail_Click"
                                ToolTip="Envia e-mail para os alunos matriculados na Oferta" CssClass="btn btn-default mostrarload"  OnClientClick="return confirm('Todos os alunos receberão o email de alerta padrão, deseja continua?');" />--%>

                            <%--<asp:Button ID="btnEnviarEmailInscrito" runat="server" Text="Enviar E-mail para Inscritos" OnClick="btnEnviarEmail_Click"
                                ToolTip="Envia e-mail para os alunos matriculados na Oferta" CssClass="btn btn-default mostrarload"  OnClientClick="return confirm('Todos os alunos inscritos receberão o email de alerta padrão, deseja continua?');" />

                                <asp:Button ID="btnEnviarEmailPendente" runat="server" Text="Enviar E-mail para Pendentes de Confirmação" OnClick="btnEnviarEmail_Click"
                                ToolTip="Envia e-mail para os alunos matriculados na Oferta" CssClass="btn btn-default mostrarload"  OnClientClick="return confirm('Todos os alunos pendentes receberão o email de alerta padrão, deseja continua?');" />--%>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <div class="panel panel-default" id="dvMatricularAluno" runat="server">
        <div class="panel-heading">
            <asp:LinkButton ID="lkbMatricularAluno" runat="server" OnClick="lkbMatricularAluno_Click"
                ClientIDMode="Static" Text=" Matricular Aluno"></asp:LinkButton>
        </div>
        <div id="collapseMatriculaOferta" class="panel-collapse collapse" runat="server"
            clientidmode="Static">
            <div class="panel-body">
                <uc2:ucMatriculaCapacitacao ID="ucMatriculaOferta1" runat="server" OnMatriculouAlunoEmUmaOferta="MatriculaOferta_MatriculouAlunoEmUmaOferta" />
            </div>
        </div>
    </div>
    <div class="panel panel-default" id="dvCriarTurma" runat="server">
        <div class="panel-heading">
            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="lkbCriarTurma"
                ClientIDMode="Static" Text=" Criar Turma"></asp:LinkButton>
        </div>
        <div id="dvFormularioTurma" visible="false" class="panel-collapse" runat="server" clientidmode="Static">
            <asp:Panel ID="Panel1" runat="server">
                <div class="panel-group">
                    <div class="panel panel-default">
                        <div id="Div1" class="panel-collapse" clientidmode="Static">
                            <div class="panel-body">
                                <div>
                                    <asp:GridView ID="gvTurmas" runat="server" AutoGenerateColumns="false" CssClass="table col-sm-12" GridLines="None">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Aluno">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label6" runat="server" Text='<%# Bind("Nome") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <div class="panel-body">
                <asp:Label ID="LblNomeTurma" runat="server" Text="Nome" AssociatedControlID="txtNomeTurma"></asp:Label>
                <asp:TextBox ID="txtNomeTurma" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="panel-body">
                <asp:Label ID="Label4" runat="server" Text="Quantidade de Vagas" AssociatedControlID="txtQuantidadeVagasTurma"></asp:Label>
                <asp:TextBox ID="txtQuantidadeVagasTurma" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="panel-body">
                <asp:Button ID="btnCriarTurma" runat="server" Text="Criar Turma" OnClick="btnCriarTurma_OnClick" CssClass="btn btn-default" />
            </div>
        </div>
    </div>
</asp:Content>
