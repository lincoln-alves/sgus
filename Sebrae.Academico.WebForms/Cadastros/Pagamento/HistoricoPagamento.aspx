<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="HistoricoPagamento.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.Pagamento.HistoricoPagamento" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <div>
        <table>
            <tr>
                <td colspan="2">
                    <h3>
                        Histórico de Pagamentos
                    </h3>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
          
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" />
                </td>
            </tr>
        </table>
    </div>
    <br />
    <asp:Panel ID="pnlInformacoesDeHistoricoDePagamento" runat="server" Visible="false">
        <div>
            <p>
                <b>Resultado da Busca</b></p>
            <asp:Literal ID="litTable" runat="server" />
           
        </div>
    </asp:Panel>
</asp:Content>
