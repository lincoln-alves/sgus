<%@ Page Title="Relatório de Envio" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="Detalhes.aspx.cs" Inherits="Sebrae.Academico.WebForms.Relatorios.Notificacoes.Detalhes"
    ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
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
                                    <asp:Label Text="Nome" runat="server" />
                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtNome"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <asp:Label Text="CPF" runat="server" />
                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtCPF"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <asp:Label Text="E-mail" runat="server" />
                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtEmail"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <asp:Label Text="Data de Envio" runat="server" />
                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtDataDeEnvio"></asp:TextBox>
                                </div>
                            </fieldset>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <asp:Button ID="btnPesquisar" runat="server" Text="Filtrar" CssClass="btn btn-primary mostraload" OnClick="btnPesquisar_Click" />
    <asp:Button ID="btnVoltar" runat="server" Text="Voltar" OnClick="btnVoltar_Click" CssClass="btn btn-default" />
    <hr />

    <asp:GridView runat="server" ID="dgvNotificacoes" CssClass="table col-sm-12" AutoGenerateColumns="false">
        <Columns>
            <asp:TemplateField HeaderText="Nome">
                <ItemTemplate>
                    <%#Eval("Usuario.Nome")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="CPF">
                <ItemTemplate>
                    <%#Eval("Usuario.CPF")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="E-mail">
                <ItemTemplate>
                    <%#Eval("Usuario.Email")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Data de Envio">
                <ItemTemplate>
                    <%#Eval("DataEnvio")%>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>

        <EmptyDataTemplate>
            <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
        </EmptyDataTemplate>
    </asp:GridView>

    <fieldset runat="server" id="componenteGeracaoRelatorio" clientidmode="Static" visible="false">
        <asp:Label ID="lblNome" runat="server" Text="Formato de Saída" AssociatedControlID="rblTipoSaida" />
        <asp:RadioButtonList ID="rblTipoSaida" runat="server" RepeatLayout="UnorderedList" CssClass="form-control file-types unorderedList">
            <asp:ListItem Value="EXCEL" Selected="True"><i class="icon icon-ms-excel" title="EXCEL"></i></asp:ListItem>
        </asp:RadioButtonList>
        <hr />
        <div class="form-group">
            <asp:Button ID="btnGerarRelatorio" runat="server" Text="Gerar Relatório" OnClick="btnGerarRelatorio_Click"
                CssClass="btn btn-primary" />
        </div>
    </fieldset>
</asp:Content>
