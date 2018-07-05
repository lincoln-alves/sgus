<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSolucaoEducacional.ascx.cs"
    Inherits="Sebrae.Academico.WebForms.UserControls.ucSolucaoEducacional" %>
<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>
<fieldset>
    <div class="form-group">
        <div class="form-group">
            
            <asp:Label ID="lblTopicoTematico" runat="server" Text="Solução Educacional" AssociatedControlID="ddlSolucaoEducacional"></asp:Label>
            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip6" />
            <asp:DropDownList ID="ddlSolucaoEducacional" runat="server" ClientIDMode="Static"
                CssClass="form-control">
            </asp:DropDownList>
        </div>
        <asp:Button ID="btnAdicionar" CssClass="btn btn-primary mostrarload" runat="server"
            OnClick="btnAdicionar_Click" ClientIDMode="Static" Text="Incluir" />
        
    </div>
    <div class="form-group">
        <asp:ListBox ID="lbSolucoesEscolhidas" runat="server" SelectionMode="Multiple" CssClass="form-control">
        </asp:ListBox>
    </div>
    <div class="form-group">
        <asp:Button ID="btnRemoverSelecionados" runat="server" OnClick="btnRemoverSelecionados_Click"
            Text="Remover Selecionados" CssClass="btn btn-default" OnClientClick="return confirm('Deseja Realmente Remover esta Solução Educacional ?');" />
        <asp:Button ID="btnRemoverTodos" runat="server" OnClick="btnRemoverTodos_Click" Text="Remover Todos"
            CssClass="btn btn-default" OnClientClick="return confirm('Deseja Realmente Remover Todas as Soluções Educacionais da Lista ?');" />
    </div>
</fieldset>
