<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="DadosProfessor.aspx.cs" Inherits="Sebrae.Academico.WebForms.Relatorios.DadosProfessor"
    MaintainScrollPositionOnPostback="true" %>

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
                            <asp:Label ID="lblNome" runat="server" Text="Nome" AssociatedControlID="txtNome" />
                            <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label1" runat="server" Text="CPF" AssociatedControlID="txtCPF" />
                            <asp:TextBox ID="txtCPF" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group" runat="server">
                            <asp:Label ID="Label2" runat="server" Text="UF" AssociatedControlID="cbxUF" />
                            <asp:DropDownList ID="cbxUF" runat="server" CssClass="form-control">
                            </asp:DropDownList>
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
                                <asp:ListItem Selected="True" Value="Email">E-mail</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Endereco">Endereco</asp:ListItem>
                                <asp:ListItem Selected="True">Cidade</asp:ListItem>
                                <asp:ListItem Selected="True">Estado</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Telefone">Telefone</asp:ListItem>
                                <asp:ListItem Selected="True" Value="TelefoneCelular">Telefone Celular</asp:ListItem>
                                <asp:ListItem Selected="True">CEP</asp:ListItem>
                            </asp:CheckBoxList>
                        </div>
                    </fieldset>
                </div>
            </div>
        </div>
    </div>
    <br />
    <asp:Button ID="btnPesquisar" runat="server" Text="Consultar" CssClass="btn btn-primary mostrarload"
        OnClick="btnPesquisar_Click" />
    <hr />
    <asp:GridView ID="dgRelatorio" runat="server" AutoGenerateColumns="False" AllowSorting="True"
        CssClass="table col-sm-12" GridLines="None" OnSorting="dgRelatorio_Sorting" AllowPaging="True"
        OnPageIndexChanging="dgRelatorio_PageIndexChanging" PageSize="100">
        <Columns>
            <asp:BoundField DataField="Nome" HeaderText="Nome" SortExpression="Nome" />
            <asp:BoundField DataField="CPF" HeaderText="CPF" SortExpression="CPF" />
            <asp:BoundField DataField="Telefone" HeaderText="Telefone" SortExpression="Telefone" />
            <asp:BoundField DataField="TelefoneCelular" HeaderText="Celular" SortExpression="TelefoneCelular" />
            <asp:BoundField DataField="Email" HeaderText="E-mail" SortExpression="Email" />
            <asp:BoundField DataField="Endereco" HeaderText="Endereço" SortExpression="Endereco" />
            <asp:BoundField DataField="Cidade" HeaderText="Cidade" SortExpression="Cidade" />
            <asp:BoundField DataField="Estado" HeaderText="Estado" SortExpression="Estado" />
            <asp:BoundField DataField="CEP" HeaderText="CEP" SortExpression="CEP" />
        </Columns>
        <EmptyDataTemplate>
            <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
        </EmptyDataTemplate>
        <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
    </asp:GridView>
    <fieldset runat="server" id="componenteGeracaoRelatorio" clientidmode="Static" visible="false">
        <hr/>
        <div class="form-group">
            <uc:SaidaRel ID="ucFormatoSaidaRelatorio" runat="server" />
        </div>
        <div class="form-group">
            <asp:Button ID="btnGerarRelatorio" runat="server" Text="Gerar Relatório" OnClick="btnGerarRelatorio_Click"
                CssClass="btn btn-primary" />
        </div>
    </fieldset>
</asp:Content>
