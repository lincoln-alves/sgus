<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="ListarHierarquia.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.Hierarquia.ListarHierarquia" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <h2 class="text-center">
            Selecione a UF da árvore
        </h2>
        <hr/>

        <asp:Repeater ID="rptUfs" runat="server" OnItemDataBound="rptUfs_OnItemDataBound">
            <ItemTemplate>
                <div class="row">
                    <div class="col-lg-offset-4 col-lg-4 col-md-offset-3 col-md-6 col-xs-12">
                        <asp:Button ID="btnUf" runat="server" CssClass="btn btn-primary btn-lg btn-block mostrarload" OnClick="btnUf_OnClick" />
                    </div>
                </div>
                <br />
            </ItemTemplate>
        </asp:Repeater>
    </fieldset>
</asp:Content>
