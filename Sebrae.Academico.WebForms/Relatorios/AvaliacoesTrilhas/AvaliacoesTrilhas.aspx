<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="AvaliacoesTrilhas.aspx.cs" Inherits="Sebrae.Academico.WebForms.Relatorios.AvaliacoesTrilhas.AvaliacoesTrilhas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-group" id="gruporelatorio">
        <div class="panel panel-default">
            <div class="panel-heading">
                <a data-toggle="collapse" data-parent="#gruporelatorio" href="#Filtros">Filtros
                </a>
            </div>
            <asp:Panel ID="Filtros" runat="server" ClientIDMode="Static" CssClass="panel-collapse collapse in">
                <div class="panel-body">
                    <fieldset>
                        <div class="form-group">
                            <uc:LupaUsuario ID="luUsuario" runat="server" IsNacional="true" />
                            <asp:Button runat="server" Text="Limpar Usuário" ID="btnLimparUsuario" OnClick="btnLimparUsuario_Click" CssClass="btn btn-default" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label5" runat="server" Text="Trilhas" AssociatedControlID="drpTrilhas"></asp:Label>
                            <asp:DropDownList ID="drpTrilhas" AutoPostBack="true" runat="server" CssClass="form-control" OnSelectedIndexChanged="drpTrilhas_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label2" runat="server" Text="Nível da Trilha" AssociatedControlID="drpNivelTrilha"></asp:Label>
                            <asp:DropDownList ID="drpNivelTrilha" AutoPostBack="true" runat="server" CssClass="form-control" OnSelectedIndexChanged="drpNivelTrilha_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label1" runat="server" Text="Ponto Sebrae" AssociatedControlID="drpPontoSebrae"></asp:Label>
                            <asp:DropDownList ID="drpPontoSebrae" AutoPostBack="true" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </fieldset>
                </div>
            </asp:Panel>
        </div>
    </div>
    <br />
    <asp:Button ID="btnPesquisar" runat="server" Text="Consultar" OnClick="btnPesquisar_Click"
        CssClass="btn btn-primary mostrarload" />
    <hr />
    <asp:GridView ID="dgRelatorio" runat="server" AutoGenerateColumns="False" CssClass="table col-sm-12" GridLines="None">
        <Columns>
            <asp:BoundField DataField="Trilha" HeaderText="Trilha" />
            <asp:BoundField DataField="PontoSebrae" HeaderText="Ponto Sebrae" />
            <asp:BoundField DataField="SolucaoEducacional" HeaderText="Solução Avaliada" />
            <asp:BoundField DataField="Avaliacao" HeaderText="Síntese de Avaliação" />
            <asp:BoundField DataField="Resenha" HeaderText="Comentário" />
            <asp:BoundField DataField="CPF" HeaderText="CPF do Autor" />
            <asp:BoundField DataField="NomeUsuario" HeaderText="Nome do Autor" />
        </Columns>
        <EmptyDataTemplate>
            <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
        </EmptyDataTemplate>
        <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
    </asp:GridView>
</asp:Content>
