<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucVisualizaCertificadoTemplate.ascx.cs" Inherits="Sebrae.Academico.WebForms.UserControls.ucVisualizaCertificadoTemplate" %>
<asp:Panel ID="pnlModal" runat="server" Visible="false">
    <div aria-hidden="false" aria-labelledby="myModalLabel" role="dialog" tabindex="-1"
        id="myModal" class="modal fade in" style="display: block;">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true" runat="server"
                        onserverclick="OcultarModal_Click" id="btnFecharModal">
                        &times;</button>
                    <h4 class="modal-title">Visualização</h4>
                </div>
                <div class="modal-body">
                    <asp:HtmlIframe id="ifrmMostraRelat" width="100%" runat="server"></asp:HtmlIframe>
                </div>
            </div>
        </div>
    </div>
</asp:Panel>
