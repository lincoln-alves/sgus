<%@ Page Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="AvaliarTurma.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.Matricula.AvaliarTurma" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .avaliar-default {
            -moz-min-width: 200px;
            -ms-min-width: 200px;
            -o-min-width: 200px;
            -webkit-min-width: 200px;
            min-width: 200px;
        }
    </style>
    <div class="table-responsive">
        <table class="table table-condensed">
            <thead>
                <tr>
                    <th colspan="2" style="padding: 5px; border-bottom: 2px solid #dddddd;" class="text-center">
                        <asp:Literal ID="ltrSolucaoEducacional" runat="server"></asp:Literal>
                    </th>
                    <th colspan="<%= QuantidadeQuestoes%>" style="padding: 5px; border: none; border-bottom: 2px solid #dddddd; border-left: 2px solid #dddddd;" class="text-center">Quesitos para avaliação do participante
                    </th>
                </tr>
                <tr>
                    <td width="200" style="padding: 5px; display: table-cell; vertical-align: bottom;">
                        <strong>
                            Nome
                        </strong>
                    </td>
                    <td style="padding: 5px; display: table-cell; vertical-align: bottom; border: none; border-right: 2px solid #dddddd;">
                        <strong>
                            UF
                        </strong>
                    </td>
                    <asp:Repeater ID="rptQuestoes" runat="server" OnItemDataBound="rptQuestoes_OnItemDataBound">
                        <ItemTemplate>
                            <td style="text-transform: none; font-size: 12px; border: none; border-left: 2px solid #dddddd;">
                                <p>
                                    <strong>
                                        <asp:Literal ID="ltrTitulo" runat="server"></asp:Literal>
                                    </strong>
                                </p>
                                <asp:Literal ID="ltrQuestao" runat="server"></asp:Literal>
                            </td>
                        </ItemTemplate>
                    </asp:Repeater>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rptMatriculas" runat="server" OnItemDataBound="rptMatriculas_OnItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td style="padding: 5px; display: inline-block; width: 300px; border: none;">
                                <asp:HiddenField ID="hdnIdMatriculaTurma" runat="server" />

                                <asp:Literal ID="ltrNome" runat="server"></asp:Literal>
                            </td>
                            <td style="padding: 5px; border: none;">
                                <asp:Literal ID="ltrUf" runat="server"></asp:Literal>
                            </td>
                            <asp:Repeater ID="rptQuestaoResposta" runat="server" OnItemDataBound="rptQuestaoResposta_OnItemDataBound">
                                <ItemTemplate>
                                    <td style="padding: 5px; border: none; border-left: 2px solid #dddddd;">
                                        <asp:HiddenField ID="hdnIdQuestaoResposta" runat="server" />
                                        <asp:HiddenField ID="hdnIdQuestao" runat="server" />

                                        <asp:Literal ID="ltrLabel" runat="server" Visible="False"></asp:Literal>
                                        <asp:DropDownList ID="ddlDominio" CssClass="form-control input-sm avaliar-default" Visible="False" runat="server"></asp:DropDownList>

                                        <asp:TextBox ID="txtDissertativo" runat="server" CssClass="form-control input-sm" Width="400px" TextMode="MultiLine" runat="server" Visible="False" Columns="40" Rows="3"></asp:TextBox>

                                        <asp:DropDownList ID="ddlStatus" CssClass="form-control input-sm avaliar-default" Visible="False" runat="server" ViewStateMode="Enabled" EnableViewState="True"></asp:DropDownList>
                                    </td>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </div>

    <hr />
    <div class="form-group">
        <asp:Button ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-primary mostrarload" OnClick="btnSalvar_OnClick"/>
        <asp:Button ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-primary" OnClientClick="return ConfirmarExclusao('Deseja mesmo sair desta tela? Dados não salvos poderão ser perdidos.');" OnClick="btnVoltar_OnClick" />
        <asp:Button ID="btnEnviarAvaliacao" runat="server" Text="Enviar avaliação ao Gestor" CssClass="btn btn-primary mostrarload" OnClick="btnEnviarAvaliacao_OnClick" />
        
        <asp:Button ID="btnAprovar" runat="server" Text="Aprovar" CssClass="btn btn-primary mostrarload" Visible="False" OnClick="btnAprovar_OnClick" />
        <asp:Button ID="btnReprovar" runat="server" Text="Não aprovar" CssClass="btn btn-primary mostrarload" Visible="False" OnClick="btnReprovar_OnClick" />
    </div>
    
    <script>

        (function ($){
            // Validação de cadastros com o plugin: "Are you sure?"
            $('form').areYouSure({ 'message': 'Existem dados que não foram salvos' });

        } (window.jQuery)); // Passa jQuery para definir que o $ é realmente do jQuery.
    </script>
</asp:Content>
