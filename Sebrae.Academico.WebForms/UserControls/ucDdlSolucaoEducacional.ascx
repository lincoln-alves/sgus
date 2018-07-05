<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDdlSolucaoEducacional.ascx.cs" Inherits="Sebrae.Academico.WebForms.UserControls.ucDdlSolucaoEducacional" %>

<script type="text/javascript">
    AutocompleteCombobox("ddlSolucaoEducacional", true);    
</script>

<div>
    <asp:Label ID="Label1" runat="server" Text="Solução Educacional" AssociatedControlID="ddlSolucaoEducacional" />
    <asp:DropDownList ID="ddlSolucaoEducacional" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSolucaoEducacional_SelectedIndexChanged"
        ClientIDMode="Static" CssClass="form-control mostrarload">
    </asp:DropDownList>     
</div>