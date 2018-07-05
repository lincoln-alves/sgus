<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="Moodle.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.Sincronizador.Moodle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script type="text/javascript">
    AutocompleteCombobox("ddlSolucaoEducacional", true);
    AutocompleteCombobox("ddlOferta", true);
</script>
<h3>Sincronizar com o curso do Moodle</h3>
    <div>
        <fieldset>
            <div class="form-group">
                <asp:Label ID="Label2" runat="server" Text="ID do Curso" AssociatedControlID="txtIDCurso"></asp:Label>
                <asp:TextBox ID="txtIDCurso" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="Label1" runat="server" Text="Solução educacional" AssociatedControlID="ddlSolucaoEducacional"></asp:Label>
                <asp:DropDownList ID="ddlSolucaoEducacional" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlSolucaoEducacional_OnSelectedIndexChanged" ClientIDMode="Static">
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:Label ID="Label3" runat="server" Text="Oferta" AssociatedControlID="ddlOferta"></asp:Label>
                <asp:DropDownList ID="ddlOferta" runat="server" CssClass="form-control" ClientIDMode="Static">
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:Button ID="btnObterChaveExterna" runat="server" Text="Obter Chave Externa" OnClick="btnObterChaveExterna_Click"
                    CssClass="btn btn-primary mostrarload" />
            </div>
        </fieldset>
        <div id="dvResumo" runat="server" visible="false">
            <h4>Resumo</h4>
            <asp:Literal ID="litResumo" runat="server" />
        </div>
            <div class="form-group">
                <asp:Label ID="Label4" runat="server" Text="Link de sincronização" AssociatedControlID="ddlOferta"></asp:Label>
                <asp:HyperLink ID="hlSincronizacao" runat="server" Target="_blank" Text="http://ava.uc.sebrae.com.br/robo_sync/?&nome=admin&senha=sebrae" NavigateUrl="http://ava.uc.sebrae.com.br/robo_sync/?&nome=admin&senha=sebrae"></asp:HyperLink>
            </div>
    </div>
</asp:Content>
