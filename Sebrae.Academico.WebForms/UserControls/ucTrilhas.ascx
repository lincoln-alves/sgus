<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucTrilhas.ascx.cs" Inherits="Sebrae.Academico.WebForms.UserControls.ucTrilhas" %>
<%@ Register Src="~/UserControls/ucPublicacaoSaber.ascx" TagName="ucPublicacaoSaber"
    TagPrefix="uc2" %>
<asp:Panel ID="Panel1" ScrollBars="Auto" runat="server" Direction="LeftToRight">
    <asp:GridView ID="dgvTrilhas" runat="server" AutoGenerateColumns="false" OnRowDataBound="dgvTrilhas_RowDataBound"
        CssClass="table col-sm-12" GridLines="None" OnRowCommand="dgvTrilhas_RowCommand">
        <Columns>
            <asp:TemplateField HeaderText="Nível">
                <ItemTemplate>
                    <%#Eval("TrilhaNivel.Nome")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Trilha">
                <ItemTemplate>
                    <%#Eval("TrilhaNivel.Trilha.Nome")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Data Inscrição">
                <ItemTemplate>
                    <%#Eval("DataInicio")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Data Limite">
                <ItemTemplate>
                    <%#Eval("DataLimite")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Status">
                <ItemTemplate>
                    <%#Eval("StatusMatriculaFormatado")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:LinkButton ID="lkbFazerInscricao" runat="server" CausesValidation="False" CommandArgument='<%# Eval("ID")%>'
                        CommandName="fazerinscricao" Text="Inscrever-se"></asp:LinkButton>
                </ItemTemplate>
                <ItemTemplate>
                    <asp:LinkButton ID="lkbEditar" runat="server" CausesValidation="False" CommandArgument='<%# Eval("ID")%>'
                        CommandName="editar" Text="Editar"></asp:LinkButton>
                </ItemTemplate>
                <ItemTemplate>
                    <asp:LinkButton ID="lkbEmitirCertificado" runat="server" CausesValidation="False"
                        OnClick="lkbEmitirCertificado_Click" CommandArgument='<%# Eval("ID")%>' Visible="false"
                        CommandName="editar" Text="Emitir Certificado"></asp:LinkButton>
                </ItemTemplate>
                <ItemTemplate>
                    <asp:Button ID="btnCertificado" runat="server" CausesValidation="False" OnClick="btnCertificado_Click"
                        CommandArgument='<%# Eval("ID")%>' Visible="false" CommandName="editar" Text="Certificado" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
        </EmptyDataTemplate>
    </asp:GridView>
</asp:Panel>
<!-- Modal -->
<div class="modal fade" id="ModalPublicacoesDoSaber" tabindex="-1" role="dialog"
    aria-labelledby="myModalLabel" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                    &times;</button>
                <h4 class="modal-title">
                    Publicações do Saber</h4>
            </div>
            <div class="modal-body">
                <uc2:ucPublicacaoSaber ID="ucPublicacaoSaber1" runat="server" />
                <br />
                <asp:Panel ID="Panel3" ScrollBars="Auto" runat="server" Direction="LeftToRight">
                </asp:Panel>
            </div>
        </div>
        <!-- /.modal-content -->
    </div>
    <!-- /.modal-dialog -->
</div>
<!-- /.modal -->
