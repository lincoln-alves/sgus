<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EdicaoMatriculaTrilha.aspx.cs"
    MaintainScrollPositionOnPostback="true" MasterPageFile="~/Pagina.master" Inherits="Sebrae.Academico.WebForms.Cadastros.EdicaoMatriculaTrilha" %>

<%@ Register Src="~/UserControls/ucMatrilhaTrilha.ascx" TagName="ucMatrilhaTrilha"
    TagPrefix="uc1" %>

<%@ Register Src="~/UserControls/ucExibirQuestionarioResposta.ascx" TagName="ucExibirQuestionarioResposta"
    TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:ucMatrilhaTrilha ID="ucMatrilhaTrilha1" runat="server" />
    <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click"
        CssClass="btn btn-primary" />
    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click"
        CssClass="btn btn-default" />
    <%--&nbsp;<hr />--%>
    <asp:Panel ID="pnlProvasRealizadas" runat="server" Visible="false">
        <h4>Provas Realizadas</h4>
        <asp:GridView ID="dgvProvasRealizadas" CssClass="table col-sm-12" runat="server"
            OnRowDataBound="dgvProvasRealizadas_RowDataBound" AutoGenerateColumns="false"
            GridLines="None" OnRowCommand="dgvProvasRealizadas_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="Data da Geração">
                    <ItemTemplate>
                        <%#Eval("DataGeracao")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Data/Hora de Participação">
                    <ItemTemplate>
                        <%#Eval("DataParticipacao")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Nota da Prova">
                    <ItemTemplate>
                        <asp:Label ID="lblNotaProva" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tipo">
                    <ItemTemplate>
                        <asp:Label ID="lblTipoQuestionarioAssociacao" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <ItemStyle Width="90px" HorizontalAlign="center" />
                    <ItemTemplate>
                        <asp:LinkButton ID="lkbVerProva" runat="server" CausesValidation="False" CommandName="verprova"
                            CommandArgument='<%# Eval("ID")%>' Text="Ver Detalhes" ToolTip="Ver Informações detalhadas">
                        <span class="btn btn-default btn-xs">
						    <span class="glyphicon glyphicon-pencil"></span>
						</span>
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <itemtemplate>Nenhuma prova encontrada</itemtemplate>
            </EmptyDataTemplate>
        </asp:GridView>
        <asp:Button ID="btnHistoricoAtividade" runat="server" Text="Histórico de Atividades" OnClick="btnHistorico_Click" CssClass="btn btn-default" />

    </asp:Panel>

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
                        <h4 class="modal-title">Provas</h4>
                    </div>
                    <div class="modal-body">
                        <uc2:ucExibirQuestionarioResposta ID="ucExibirQuestionarioResposta" runat="server" />
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->
    </asp:Panel>

    <script type="text/javascript">
        validarForm();
    </script>
</asp:Content>
