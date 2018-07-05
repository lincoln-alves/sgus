<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucMatriculaOferta.ascx.cs"
    Inherits="Sebrae.Academico.WebForms.UserControls.ucMatriculaOferta" %>
<%@ Register Src="~/UserControls/ucLupaUsuario.ascx" TagName="LupaUsuario" TagPrefix="uc1" %>

<fieldset>
    <div class="form-group">
        <uc1:LupaUsuario ID="LupaUsuario" runat="server" />
    </div>
    <div class="form-group">
        Turma:
        <asp:DropDownList ID="ddlTurma" Enabled="false" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlTurma_OnSelectedIndexChanged"></asp:DropDownList>
    </div>
    <div class="form-group">
        Status:
        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control"></asp:DropDownList>
    </div>
    <div class="form-group" runat="server" id="divDataInscricao" visible="False">
        Data Matrícula:
        <asp:TextBox ID="txtDataInscricao" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
    </div>
    <div class="form-group" runat="server" id="divDataConclusao" visible="False">
        Data Conclusão:
        <asp:TextBox ID="txtDataConclusao" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
    </div>
    <div class="form-group">
        <asp:Button ID="btnEnviar" runat="server" Text="Enviar" CausesValidation="false" OnClick="btnEnviar_Click" CssClass="btn btn-default" />
    </div>

    <!-- Alert MATRICULA ADMIN/GESTOR-->
    <asp:Panel ID="pnlMatriculaAdminGestor" runat="server" Visible="false">

        <div class="alert alert-danger alert-dismissible fade in" role="alert">
            <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">×</span></button>
            <h4>Erro na matrícula do Usuário</h4>
            <p><asp:Label ID="lblRestricaoMatricula" runat="server"></asp:Label></p>
            <p>Deseja prosseguir com a matrícula do usuário ?</p>
            <p>
                <asp:Button Text="Não" runat="server" CssClass="btn btn-default" OnClick="OcultarModalMatriculaAdminGestorConfirmacao_Click" />
                <asp:Button Text="Sim" runat="server" CssClass="btn btn-danger" href="#collapseConfirmacao" data-toggle="collapse" />
            </p>
        </div>

        <div id="collapseConfirmacao" class="collapse">
            <div class="form-group">
                <label class="">Justificativa</label>
                <asp:TextBox ID="txtJustificativaMatricula" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <asp:Button Text="Efetivar Matrícula" runat="server" ID="btnEfetivarMatricula" OnClick="btnEfetivarMatricula_Click" CssClass="form-control btn btn-primary btn-block" />
        </div>
    </asp:Panel>

    <script type="text/javascript">
        (function ($) {
            $(document).ready(function () {
                var dtInscricao = $("#<%= txtDataInscricao.ClientID %>");
                dtInscricao.mask("99/99/9999", {
                    autoclear: false
                });
                var dtConclusao = $("#<%= txtDataConclusao.ClientID %>");
                dtConclusao.mask("99/99/9999", {
                    autoclear: false
                });
            });
        })(jQuery);
    </script>
</fieldset>

<!-- MODAL Confirmação Inscrição -->
<asp:Panel ID="pnlModalConfirmacaoInscricao" runat="server" Visible="false">
    <div class="modal fade in" id="ModalConfirmacaoInscricao" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true" style="display: block;">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button id="Button1" type="button" class="close" data-dismiss="modal" aria-hidden="true"
                        onserverclick="OcultarModalAvisoConfirmacao_Click" runat="server">
                        &times;</button>
                    <h4 class="modal-title">Confirmação de Inscrição</h4>
                </div>
                <div class="modal-body">
                    <div class="alert alert-danger" role="alert">
                        <asp:Label runat="server" ID="lblAvisoModalConfirmacaoInscricao"></asp:Label>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label30" runat="server" Text="Justificativa"></asp:Label><br />
                        <CKEditor:CKEditorControl ID="txtJustificativa" BasePath="/js/ckeditor/" runat="server"></CKEditor:CKEditorControl>
                    </div>
                    <div class="form-group">
                        <h4 class="modal-title">Deseja realmente realizar a inscrição?</h4>
                    </div>
                    <asp:Button runat="server" Text="Sim" CssClass="btn" OnClick="btnEnviarConfirmacao_OnClick" />
                    <asp:Button runat="server" Text="Não" CssClass="btn" OnClick="btnCancelarConfirmacao_OnClick" />
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
    <!-- /.modal -->
</asp:Panel>



