<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="Administracao.aspx.cs" Inherits="Sebrae.Academico.WebForms.Administracao" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h3>
        Administrador</h3>
    <div class="form-group">        
        <fieldset>
            <div class="form-group">
                <asp:Label ID="Label5" runat="server" Text="CPF:" AssociatedControlID="txtCPF" />
                <asp:TextBox ID="txtCPF" runat="server" MaxLength="11"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Button ID="btnEnviar" runat="server" Text="Enviar" OnClick="btnEnviar_Click" CssClass="btn btn-default mostrarload" />
            </div>
            <div class="form-group">
                <asp:Literal ID="liRetorno" runat="server"></asp:Literal>
            </div>
        </fieldset>
        <h4>Cache</h4>
        <fieldset>
            <div class="form-group">
                <asp:Button ID="cleanCache" runat="server" Text="Limpar Cache de Dados" OnClick="btnCleanCache_Click" CssClass="btn btn-default mostrarload" />
            </div>            
        </fieldset>
    </div>
    
    <hr />
    
    <div class="form-group">
        <asp:Button ID="btnAtualizarChavesExternas" runat="server" Text="Atualizar chaves externas duplicadas" CssClass="btn btn-primary" OnClick="btnAtualizarChavesExternas_OnClick" />
    </div>
    <div class="form-group">
        <h5>
            Ofertas sincronizadas
        </h5>
        <asp:Label ID="labelSync" runat="server" Text=""></asp:Label>
    </div>
    <div class="form-group">
        <h5>
            Ofertas NÃO sincronizadas
        </h5>
        <asp:Label ID="labelNaoSync" runat="server" Text=""></asp:Label>
    </div>
</asp:Content>
