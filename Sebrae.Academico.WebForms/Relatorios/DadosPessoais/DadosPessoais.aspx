<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="DadosPessoais.aspx.cs" Inherits="Sebrae.Academico.WebForms.Relatorios.DadosPessoais.DadosPessoais"
    MaintainScrollPositionOnPostback="true" %>
<%@ Register Src="~/UserControls/ucSeletorListBox.ascx" TagName="ucSeletorListBox"TagPrefix="uc1" %>

    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <script type="text/javascript">
            jQuery(function ($) {
                $("#<%= txtDataInicio.ClientID %>").mask("99/99/9999");
                $("#<%= txtDataFinal.ClientID %>").mask("99/99/9999");
            });
        </script>
        <div class="table-responsive">
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
                                    <asp:Label ID="Label6" runat="server" Text="UF" AssociatedControlID="ListBoxesUF" />
                                    <uc1:ucSeletorListBox runat="server" ID="ListBoxesUF" />
                                </div>                        
                                <div class="form-group">
                                    <asp:Label ID="Label7" runat="server" Text="Nível Ocupacional" AssociatedControlID="ListBoxesNivelOcupacional" />
                                    <uc1:ucSeletorListBox runat="server" ID="ListBoxesNivelOcupacional" />
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="Label8" runat="server" Text="Perfil" AssociatedControlID="ListBoxesPerfil" />
                                    <uc1:ucSeletorListBox runat="server" ID="ListBoxesPerfil" />
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="Label4" runat="server" Text="Data Início" AssociatedControlID="txtDataInicio" />
                                    <asp:TextBox ID="txtDataInicio" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="Label5" runat="server" Text="Data Final" AssociatedControlID="txtDataFinal" />
                                    <asp:TextBox ID="txtDataFinal" runat="server" CssClass="form-control"></asp:TextBox>
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
                                        <asp:ListItem Selected="True">UF</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="NivelOcupacional">Nível Ocupacional</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="Perfil">Perfil</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="Matricula">Matrícula</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="Situacao">Situação</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="Email">E-mail</asp:ListItem>
                                        <asp:ListItem Selected="True">Unidade</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="Endereco">Endereco</asp:ListItem>
                                        <asp:ListItem Selected="True">Bairro</asp:ListItem>
                                        <asp:ListItem Selected="True">Cidade</asp:ListItem>
                                        <asp:ListItem Selected="True">Estado</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="TelResidencial">Tel. Residencial</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="TelCelular">Tel. Celular</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="DataAdmissaoForm">Data de Admissão</asp:ListItem>
                                        <asp:ListItem Selected="True">Sexo</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="EstadoCivil">Estado Civil</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="DataNascimentoForm">Data de Nascimento</asp:ListItem>
                                        <asp:ListItem Selected="True">Estado2</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="AreaConhecimentoForm">Area de Conhecimento</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="UltimoAcesso">Último Acesso</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="QtdAcessos">Qtd. de Acessos</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="DataAtualizacao">Data de Atualização</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="DT_Insercao">Data de Início</asp:ListItem>
                                    </asp:CheckBoxList>
                                </div>
                            </fieldset>
                        </div>
                    </div>
                </div>
            </div>
            <br />
            <asp:Button ID="btnPesquisar" runat="server" Text="Consultar" OnClick="btnPesquisar_Click"
                CssClass="btn btn-primary mostraload" />
            <hr />
            <asp:GridView ID="dgRelatorio" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                CssClass="table col-sm-12" GridLines="None" OnSorting="dgRelatorio_Sorting" AllowPaging="True"
                OnPageIndexChanging="dgRelatorio_PageIndexChanging" PageSize="100">
                <Columns>
                    <asp:BoundField DataField="Nome" HeaderText="Nome" SortExpression="Nome" />
                    <asp:BoundField DataField="CPF" HeaderText="CPF" SortExpression="CPF" />
                    <asp:BoundField DataField="UF" HeaderText="UF" SortExpression="UF" />
                    <asp:BoundField DataField="NivelOcupacional" HeaderText="Nível Ocupacional" SortExpression="NivelOcupacional" />
                    <asp:BoundField DataField="Perfil" HeaderText="Perfil" SortExpression="Perfil" />
                    <asp:BoundField DataField="Matricula" HeaderText="Matrícula" SortExpression="Matricula" />
                    <asp:BoundField DataField="Situacao" HeaderText="Situação" SortExpression="Situacao" />
                    <asp:BoundField DataField="Email" HeaderText="E-mail" SortExpression="Email" />
                    <asp:BoundField DataField="Unidade" HeaderText="Unidade" SortExpression="Unidade" />
                    <asp:BoundField DataField="Endereco" HeaderText="Endereço" SortExpression="Endereco" />
                    <asp:BoundField DataField="Bairro" HeaderText="Bairro" SortExpression="Bairro" />
                    <asp:BoundField DataField="Cidade" HeaderText="Cidade" SortExpression="Cidade" />
                    <asp:BoundField DataField="Estado" HeaderText="Estado" SortExpression="Estado" />
                    <asp:BoundField DataField="TelResidencial" HeaderText="Tel. Residencial" SortExpression="TelResidencial" />
                    <asp:BoundField DataField="TelCelular" HeaderText="Tel. Celular" SortExpression="TelCelular" />
                    <asp:BoundField DataField="DataAdmissaoForm" HeaderText="Admissão" SortExpression="DataAdmissao"
                        DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="Sexo" HeaderText="Sexo" SortExpression="Sexo" />
                    <asp:BoundField DataField="EstadoCivil" HeaderText="Estado Civil" SortExpression="EstadoCivil" />
                    <asp:BoundField DataField="DataNascimentoForm" HeaderText="DataNascimento" SortExpression="DataNascimento"
                        DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="Estado2" HeaderText="Estado2" SortExpression="Estado2" />
                    <asp:BoundField DataField="AreaConhecimentoForm" HeaderText="Área de Conhecimento" SortExpression="AreaConhecimento" />
                    <asp:BoundField DataField="UltimoAcessoForm" HeaderText="Último Acesso" SortExpression="UltimoAcesso" />
                    <asp:BoundField DataField="QtdAcessos" HeaderText="Qtd. Acessos" SortExpression="QtdAcessos" />
                    <asp:BoundField DataField="DataAtualizacaoForm" HeaderText="Data de Atualização" SortExpression="DataAtualizacao" />
                    <asp:BoundField DataField="DT_InsercaoForm" HeaderText="Data de Início" SortExpression="DT_Insercao" />
                </Columns>
                <EmptyDataTemplate>
                    <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
                </EmptyDataTemplate>
                <PagerSettings Position="TopAndBottom" />
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
        </div>
    </asp:Content>
