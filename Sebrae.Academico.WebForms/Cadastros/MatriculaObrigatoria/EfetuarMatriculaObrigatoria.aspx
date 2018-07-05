<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="EfetuarMatriculaObrigatoria.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.MatriculaObrigatoria.EfetuarMatriculaObrigatoria" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <div class="form-group">
            <asp:Label ID="Label20" runat="server" Text="Matricular Por" AssociatedControlID="rblEfetuar" />
            <asp:RadioButtonList ID="rblEfetuar" runat="server" RepeatDirection="Horizontal" CssClass="form-control" OnSelectedIndexChanged="rblEfetuar_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem Value="SolucaoEducacional" Text="Solução Educacional"></asp:ListItem>
                <asp:ListItem Value="NivelOcupacional" Text="Nivel Ocupacional"></asp:ListItem>
            </asp:RadioButtonList>
        </div>
        <div id="divSolucaoEducacional" runat="server" visible="false">
            <div class="form-group">
                <asp:Label ID="Label1" runat="server" Text="Solução Educacional" AssociatedControlID="ddlSolucaoEducacional" />
                <asp:DropDownList ID="ddlSolucaoEducacional" runat="server" CssClass="form-control mostrarload" AutoPostBack="true" OnSelectedIndexChanged="ddlSolucaoEducacional_OnSelectedIndexChanged" />
            </div>
            <div class="form-group clearfix">
                <label>Nível Ocupacional</label>
                -
                <asp:Label ID="btnNive" runat="server" Text="Selecionar" AssociatedControlID="cblNivelOcupacional" CssClass="marcar-todos"></asp:Label>
                <asp:CheckBoxList ID="cblNivelOcupacional" runat="server" RepeatDirection="Vertical" RepeatLayout="UnorderedList" Width="100%" CssClass="large-li" />
            </div>
        </div>
        <div id="divNivelOcupacional" runat="server" visible="false">
            <div class="form-group">
                <asp:Label ID="Label2" runat="server" Text="Nivel Ocupacional" AssociatedControlID="ddlNivelOcupacional" />
                <asp:DropDownList ID="ddlNivelOcupacional" runat="server" CssClass="form-control mostrarload" AutoPostBack="true" OnSelectedIndexChanged="ddlNivelOcupacional_OnSelectedIndexChanged" />
            </div>
            <div class="form-group clearfix">
                <label>Solução Educacional</label>
                -
                <asp:Label ID="Label3" runat="server" Text="Selecionar" AssociatedControlID="cblSolucaoEducacional" CssClass="marcar-todos"></asp:Label>
                <asp:CheckBoxList ID="cblSolucaoEducacional" runat="server" RepeatDirection="Vertical" RepeatLayout="UnorderedList" Width="100%" CssClass="large-li" />
            </div>
        </div>
        <div class="form-group">
            <asp:Button ID="btnMatricular" runat="server" Text="Efetuar Matrícula no Filtro Efetuado" OnClick="btnMatricular_Click" CssClass="btn btn-primary mostrarload" Visible="False" />
        </div>
    </fieldset>

    <script type="text/javascript">
        $(function () {
            $(".marcar-todos").each(function () {
                var btn = $(this);
                var target = btn.attr("for");
                var cks = btn.parent().find("#" + target).find("input[type=checkbox]");
                $(this).unbind('click').click(function () {
                    ckStatus = !cks.prop('checked');
                    cks.prop('checked', ckStatus);
                    btn.html((ckStatus) ? "Desmarcar Todos" : "Marcar Todos");
                });
                cks.unbind('change').change(function () {
                    ckStatus = cks.prop('checked');
                    btn.html((ckStatus) ? "Desmarcar Todos" : "Marcar Todos");
                });
            });
        });
    </script>

</asp:Content>
