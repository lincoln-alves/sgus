<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="UsuarioPagante.aspx.cs" Inherits="Sebrae.Academico.WebForms.Relatorios.UsuarioPagante.UsuarioPagante"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/UserControls/ucTotalizadorRelatorio.ascx" TagPrefix="uc" TagName="ucTotalizadorRelatorio" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                                    <asp:Label ID="lblNome" runat="server" Text="Nome" AssociatedControlID="txtNome" />
                                    <asp:TextBox ID="txtNome" runat="server" CssClass="form-control" MaxLength="250"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="Label1" runat="server" Text="CPF" AssociatedControlID="txtCPF" />
                                    <asp:TextBox ID="txtCPF" runat="server" CssClass="form-control" MaxLength="15"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="Label2" runat="server" Text="UF" AssociatedControlID="cbxUF" />
                                    <asp:DropDownList ID="cbxUF" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="Label3" runat="server" Text="Nível Ocupacional" AssociatedControlID="cbxNivelOcupacional" />
                                    <asp:DropDownList ID="cbxNivelOcupacional" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="Label4" runat="server" Text="Pagantes" AssociatedControlID="rblPagante" />
                                    <div class="form-control" style="height: auto;">
                                        <asp:RadioButtonList runat="server" ID="rblPagante" RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="rblPagante_SelectedIndexChanged">
                                            <asp:ListItem Text="Sim" Value="1" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Não" Value="2"></asp:ListItem>                                    
                                            <asp:ListItem Text="Todos" Value="0"></asp:ListItem>
                                        </asp:RadioButtonList>
                                        São considerados pagantes os usuários que possuírem "Pago" ou "Pag. Informado pelo Usuário" ou "Pag. Confirmado" como "Sim".
                                    </div>
                                </div>

                            </fieldset>
                        </div>
                    </asp:Panel>
                </div>
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <a data-toggle="collapse" data-parent="#gruporelatorio" href="#Campos">Campos a Serem
                            Exibidos </a>
                    </div>
                    <div id="Campos" class="accordion-body collapse">
                        <div class="accordion-inner">
                            <fieldset>
                                <div class="form-group">
                                    <asp:CheckBoxList ID="chkListaCamposVisiveis" runat="server" RepeatDirection="Vertical"
                                        RepeatLayout="UnorderedList" ClientIDMode="Static">
                                        <asp:ListItem Selected="True">Nome</asp:ListItem>
                                        <asp:ListItem Selected="True">CPF</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="Matricula">Matrícula</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="Situacao">Situação</asp:ListItem>
                                        <asp:ListItem Selected="True">Unidade</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="NivelOcupacional">Nível Oculpacional</asp:ListItem>
                                        <asp:ListItem Selected="True">UF</asp:ListItem>

                                        <asp:ListItem Selected="True" Value="DescricaoFormaPagamento">Forma Pagamento</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="DescricaoPago">Pago</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="DescricaoPagamentoInformado">Pag. Informado Pelo Usuário</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="DescricaoPagamentoConfirmado">Pag. Confirmado</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="DataUltimaAtualizacao">Data Atualização</asp:ListItem>
                                    </asp:CheckBoxList>
                                </div>
                            </fieldset>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <asp:Button ID="btnPesquisar" runat="server" Text="Consultar" OnClick="btnPesquisar_Click"
        CssClass="btn btn-primary mostraload" />
    <hr />
    
    <uc:ucTotalizadorRelatorio runat="server" ID="ucTotalizadorRelatorio" />

    <asp:GridView ID="dgRelatorio" runat="server" AutoGenerateColumns="False" AllowSorting="True"
        CssClass="table col-sm-12" GridLines="None" OnSorting="dgRelatorio_Sorting" AllowPaging="True"
        OnPageIndexChanging="dgRelatorio_PageIndexChanging" PageSize="100">
        <Columns>
            <asp:BoundField DataField="Nome" HeaderText="Nome" SortExpression="Nome" />
            <asp:BoundField DataField="CPF" HeaderText="CPF" SortExpression="CPF" />
            <asp:BoundField DataField="Matricula" HeaderText="Matrícula" SortExpression="Matricula" />
            <asp:BoundField DataField="Situacao" HeaderText="Situação" SortExpression="Situacao" />
            <asp:BoundField DataField="Unidade" HeaderText="Unidade" SortExpression="Unidade" />
            <asp:BoundField DataField="NivelOcupacional" HeaderText="Nivel Ocupacional" SortExpression="NivelOcupacional" />
            <asp:BoundField DataField="UF" HeaderText="UF" SortExpression="UF" />
            <asp:BoundField DataField="DescricaoFormaPagamento" HeaderText="Forma Pagamento" SortExpression="FormaPagamento" />
            <asp:BoundField DataField="DescricaoPago" HeaderText="Pago" SortExpression="DescricaoPago" />
            <asp:BoundField DataField="DescricaoPagamentoInformado" HeaderText="Pag. Informado pelo Usuário" SortExpression="DescricaoPagamentoInformado" />
            <asp:BoundField DataField="DescricaoPagamentoConfirmado" HeaderText="Pag. Confirmado" SortExpression="DescricaoPagamentoConfirmado" />
            <asp:BoundField DataField="DataUltimaAtualizacao" HeaderText="Data Atualização" SortExpression="DataUltimaAtualizacao"
                DataFormatString="{0:dd/MM/yyyy HH:mm}" />
        </Columns>
        <EmptyDataTemplate>
            <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
        </EmptyDataTemplate>
        <PagerSettings Position="TopAndBottom" />
    </asp:GridView>
    <fieldset runat="server" id="componenteGeracaoRelatorio" clientidmode="Static" visible="false">
        <hr />
        <div class="form-group">
            <uc:SaidaRel ID="ucFormatoSaidaRelatorio" runat="server" />
        </div>
        <div class="form-group">
            <asp:Button ID="btnGerarRelatorio" runat="server" Text="Gerar Relatório" OnClick="btnGerarRelatorio_Click"
                CssClass="btn btn-primary" />
        </div>
    </fieldset>
</asp:Content>
