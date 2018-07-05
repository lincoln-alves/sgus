<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSeletorListBox.ascx.cs"
    Inherits="Sebrae.Academico.WebForms.UserControls.ucSeletorListBox" %>

<div style="display: table;" class="containerSeletorListBox">
    <div style="width: 500px; display: table-cell;">
        <div><asp:Label ID="lblDisponiveis" runat="server"></asp:Label></div>
        <asp:ListBox SelectionMode="multiple" ID="ddlItensDisponiveis" runat="server" CssClass="form-control list1" Rows="10">
        </asp:ListBox>
    </div>

    <div style="width: 50px; padding: 3px; display: table-cell; vertical-align: middle;">
        <input type="button" value="→" style="width: 100%; padding: 1px 5px; margin: 1px 0;" 
            onclick="adicionarItensListBox(this);" title="Mover para Selecionados" />
        <br />
        <input type="button" value="←" style="width: 100%; padding: 1px 5px; margin: 1px 0;" 
            onclick="removerItensListBox(this);" title="Retirar dos Selecionados" />
        <br />
        <input ID="inputAddTodos" type="button" value="←→" runat="server" style="width: 100%; padding: 1px 5px; margin: 1px 0;" 
            onclick="adicionarTodos(this);" title="Adicionar ou Remover Todos" />
    </div>
    
    <div style="width: 500px; display: table-cell;">
        <div><asp:Label ID="lblSelecionados" runat="server"></asp:Label></div>
        <asp:ListBox SelectionMode="multiple" ID="ddlItensSelecionados" runat="server" CssClass="form-control list2" Rows="10">
        </asp:ListBox>
    </div>

    <asp:HiddenField ID="hdnSelecionados" runat="server" />
</div>
