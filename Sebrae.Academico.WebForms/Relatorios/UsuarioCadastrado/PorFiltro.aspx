<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="PorFiltro.aspx.cs" 
    Inherits="Sebrae.Academico.WebForms.Relatorios.UsuarioCadastrado.PorFiltro" MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/UserControls/ucSeletorListBox.ascx" TagName="ucSeletorListBox" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/ucLupaUsuario.ascx" TagName="ucLupaUsuario" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-group" id="gruporelatorio">
        <div class="panel panel-default">
            <div class="panel-heading">
                <a data-toggle="collapse" data-parent="#gruporelatorio" href="#Filtros">Filtros</a>
            </div>
            <asp:Panel ID="Filtros" runat="server" ClientIDMode="Static" CssClass="panel-collapse collapse in">
                <div class="panel-body">
                    <fieldset>
                        <div class="form-group">
                            <asp:Label ID="lblPerfil" runat="server" Text="Perfil" AssociatedControlID="ucMultiplosPerfil" />
                            <uc:ucSeletorListBox ID="ucMultiplosPerfil" runat="server" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label1" runat="server" Text="Nivel Ocupacional" AssociatedControlID="ucMultiplosNivelOcupacional" />
                            <uc:ucSeletorListBox ID="ucMultiplosNivelOcupacional" runat="server" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label2" runat="server" Text="UF" AssociatedControlID="ucMultiplosUF" />
                            <uc:ucSeletorListBox ID="ucMultiplosUF" runat="server" />
                        </div>
                        <div class="form-group">
                            <uc1:ucLupaUsuario ID="ucLupaUsuario" IsNacional="true" Chave="lupaUsuario" runat="server" Text="Usuário" />
                        </div>
                    </fieldset>
                </div>
            </asp:Panel>
        </div>
        <div class="panel panel-default">
            <div class="panel-heading">
                <a data-toggle="collapse" data-parent="#gruporelatorio" href="#Campos" title="As horas são agrupadas de acordo com os campos exibidos">Campos a Serem
                    Exibidos</a>
            </div>
            <div id="Campos" class="accordion-body collapse">
                <div class="accordion-inner">
                    <fieldset>
                        <div class="form-group">
                            <asp:CheckBoxList ID="chkListaCamposVisiveis" runat="server" RepeatDirection="Vertical"
                                RepeatLayout="UnorderedList" ClientIDMode="Static">
                                <asp:ListItem Selected="True" Value="UF">UF</asp:ListItem>
                                <asp:ListItem Selected="True" value="Nome">Nome do Usuário</asp:ListItem>
                                <asp:ListItem Selected="True" value="CPF">CPF</asp:ListItem>
                                <asp:ListItem Selected="True" value="Email">Email</asp:ListItem>
                                <asp:ListItem Selected="True" Value="NivelOcupacional">Nivel Ocupacional</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Perfil">Perfil</asp:ListItem>
                            </asp:CheckBoxList>
                        </div>
                    </fieldset>
                </div>
            </div>
        </div>
    </div>
    <hr />
    <br />
    <asp:Button ID="btnPesquisar" runat="server" Text="Consultar" CssClass="btn btn-primary mostrarload"
        OnClick="btnPesquisar_Click" />
    <hr />
    <div id="dvContador" runat="server" visible="false">
        <b>Total de Registros Encontrados: </b><asp:Literal ID="litContador" runat="server"></asp:Literal>
        <hr />
    </div>
    
    <asp:Panel ID="pnlPerfilUsuario" runat="server" Visible="false">
        <h4>Resultado da Busca</h4>

        <div class="table-responsive">
            <table class="table table-hover">
                <thead>
                    <tr>
                        <th ID="UF" runat="server" width="20%" visible="true">Uf</th>
                        <th ID="Nome" runat="server" width="20%" visible="true">Nome</th>
                        <th ID="CPF" runat="server" width="20%" visible="true">CPF</th>
                        <th ID="Email" runat="server" width="20%" visible="true">Email</th>
                        <th ID="NivelOcupacional" runat="server" width="20%" visible="true">Nível Ocupacional</th>
                        <th ID="Perfil" runat="server" width="20%">Perfil</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptPerfilUf" runat="server" OnItemDataBound="rptPerfilUf_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td ID="UF" runat="server"><%#Eval("Sigla")%></td>
                                <td ID="colspan" runat="server" colspan="5">
                                     <table class="table">
                                         <tbody>
                                            <asp:Repeater ID="rptUsuario" runat="server" OnItemDataBound="rptUsuario_ItemDataBound">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td ID="Nome" runat="server" width="20%"><%#Eval("Nome")%></td>
                                                        <td ID="CPF" runat="server" width="20%"><%#Eval("CPF")%></td>
                                                        <td ID="Email" runat="server" width="20%"><%#Eval("Email")%></td>
                                                        <td ID="NivelOcupacional" runat="server" width="20%"><%#Eval("NivelOcupacional")%></td>
                                                        <td ID="Perfil" runat="server" width="20%">
                                                            <table class="table">
                                                                <tbody>
                                                                    <asp:Repeater ID="rptPerfil" runat="server">
                                                                        <ItemTemplate>
                                                                             <tr>
                                                                                <td><%#Eval("Nome")%></td>
                                                                             </tr>
                                                                        </ItemTemplate>
                                                                    </asp:Repeater>
                                                                </tbody>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>

    </asp:Panel>

    <fieldset runat="server" id="componenteGeracaoRelatorio" clientidmode="Static" visible="false">
        <div class="form-group">
            <uc:SaidaRel ID="ucFormatoSaidaRelatorio" runat="server" />
        </div>
        <div class="form-group">
            <asp:Button ID="btnGerarRelatorio" runat="server" Text="Gerar Relatório" CssClass="btn btn-primary"
                OnClick="btnGerarRelatorio_Click" />
        </div>
    </fieldset>
</asp:Content>
