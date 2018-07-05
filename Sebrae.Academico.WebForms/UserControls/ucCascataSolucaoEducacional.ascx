<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCascataSolucaoEducacional.ascx.cs"
    Inherits="Sebrae.Academico.WebForms.UserControls.ucCascataSolucaoEducacional" %>

    <script type="text/javascript">
        //AutocompleteCombobox("ddlSolucao", true);
    </script>
<div>
    <fieldset>
        <div class="form-group">
            <h3>
                <span id="spanAcao" runat="server" />
            </h3>
        </div>
        <div class="form-group" runat="server" id="divCategoria">
            <asp:Label ID="LblCategoria" runat="server" AssociatedControlID="ddlCategoria" Text="Categoria solução educacional"  data-help="<%$ Resources:Resource, cadastroCategoriaSolucaoEducacional%>"></asp:Label>
            <asp:DropDownList ID="ddlCategoria" runat="server" AutoPostBack="true" CssClass="form-control mostrarloadselect"
                OnSelectedIndexChanged="ddlCategoria_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Label ID="LblSolucao" runat="server" AssociatedControlID="ddlSolucao" Text="Solução educacional" data-help="<%$ Resources:Resource, cadastroSolucaoEducacional%>"></asp:Label>
            <asp:DropDownList ID="ddlSolucao" runat="server" CssClass="form-control" ClientIDMode="Static" OnSelectedIndexChanged="ddlSolucao_SelectedIndexChanged" AutoPostBack="True">
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Label ID="LblOferta" runat="server" Text="Oferta" AssociatedControlID="ddlOferta" data-help="<%$ Resources:Resource, cadastroOferta%>"></asp:Label>
            <asp:DropDownList ID="ddlOferta" runat="server" CssClass="form-control">
            </asp:DropDownList>
        </div>
        <div class="form-group">
        </div>
        </span>
    </fieldset>
</div>
