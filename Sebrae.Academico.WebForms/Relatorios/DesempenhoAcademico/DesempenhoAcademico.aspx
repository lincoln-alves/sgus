<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="DesempenhoAcademico.aspx.cs" Inherits="Sebrae.Academico.WebForms.Relatorios.DesempenhoAcademico.DesempenhoAcademico"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/UserControls/ucCategorias.ascx" TagName="ucCategorias" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucSeletorListBox.ascx" TagName="ucSeletorListBox"TagPrefix="uc2" %>
<%@ Register TagPrefix="uc" TagName="ucNacionalizarRelatorio" Src="~/UserControls/ucNacionalizarRelatorio.ascx" %>
<%@ Register Src="~/UserControls/ucTotalizadorRelatorio.ascx" TagPrefix="uc" TagName="ucTotalizadorRelatorio" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        input.form-control {
            width: 100% !important;
        }
    </style>
    <script type="text/javascript">
        var _preloadedListSE = <%= ViewState["_SE"] ?? "''" %>;
        AutoCompleteDefine(_preloadedListSE, '#txtSolucaoEducacional', true, true, false);
        
        var _preloadedListOferta = <%= ViewState["_Oferta"] ?? "''" %>;
        AutoCompleteDefine(_preloadedListOferta, '#txtOferta', true, false, true);

        var _preloadedListTurma = <%= ViewState["_Turma"] ?? "''" %>;
        AutoCompleteDefine(_preloadedListTurma, '#txtTurma', false, false, true);

    </script>
    <div class="panel-group" id="gruporelatorio">
        <div class="panel panel-default">
            <div class="panel-heading">
                <a data-toggle="collapse" data-parent="#gruporelatorio" href="#Filtros">Filtros
                </a>
            </div>
            <asp:Panel ID="Filtros" runat="server" ClientIDMode="Static" CssClass="panel-collapse collapse in">
                <div class="panel-body">
                    <fieldset>
                        <uc:ucNacionalizarRelatorio runat="server" ID="ucNacionalizarRelatorio" OnNacionalizouRelatorio="ucNacionalizarRelatorio_OnNacionalizouRelatorio" />
                        <div class="form-group">
                            <asp:Label ID="lblNome" runat="server" Text="Nome" AssociatedControlID="txtNome" />
                            <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label1" runat="server" Text="CPF" AssociatedControlID="txtCPF" />
                            <asp:TextBox ID="txtCPF" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                       
                        <div class="form-group">
                            <asp:Label ID="Label2" runat="server" Text="Nível Ocupacional" AssociatedControlID="ListBoxesNivelOcupacional" />
                            <uc2:ucSeletorListBox runat="server" ID="ListBoxesNivelOcupacional" />
                        </div>
                        <div class="form-group" id="dvUF" runat="server">
                            <asp:Label ID="Label3" runat="server" Text="UF" AssociatedControlID="ListBoxesUF" />
                            <uc2:ucSeletorListBox runat="server" ID="ListBoxesUF" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label6" runat="server" Text="Status" AssociatedControlID="ListBoxesStatus" />
                            <uc2:ucSeletorListBox runat="server" ID="ListBoxesStatus" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label13" runat="server" Text="Público-Alvo" AssociatedControlID="ListBoxesStatus" />
                            <uc2:ucSeletorListBox runat="server" ID="ListBoxesPublicoAlvo" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label4" runat="server" Text="Data Início de Matrícula" AssociatedControlID="txtDataInicio" />
                            <asp:TextBox ID="txtDataInicio" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label5" runat="server" Text="Data Final de Matrícula" AssociatedControlID="txtDataFinal" />
                            <asp:TextBox ID="txtDataFinal" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label15" runat="server" Text="Data Início da Turma" AssociatedControlID="txtDataInicioTurma"></asp:Label>
                            <asp:TextBox ID="txtDataInicioTurma" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label16" runat="server" Text="Data Fim da turma" AssociatedControlID="txtDataFinalTurma"></asp:Label>
                            <asp:TextBox ID="txtDataFinalTurma" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label11" runat="server" Text="Data Inicial de Conclusão do Aluno" AssociatedControlID="txtDataTerminoInicio" />
                            <asp:TextBox ID="txtDataTerminoInicio" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label12" runat="server" Text="Data Final de Conclusão do Aluno" AssociatedControlID="txtDataTerminoFim" />
                            <asp:TextBox ID="txtDataTerminoFim" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label10" runat="server" Text="CATEGORIAS" AssociatedControlID="ucCategorias1" />
                            <uc1:ucCategorias ID="ucCategorias1" runat="server" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label7" runat="server" Text="Solução Educacional" AssociatedControlID="txtSolucaoEducacional" />
                            <asp:TextBox ID="txtSolucaoEducacional" runat="server" ClientIDMode="Static" OnTextChanged="txtSolucaoEducacional_OnTextChanged" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label8" runat="server" Text="Oferta" AssociatedControlID="txtOferta" />
                            <asp:TextBox ID="txtOferta" runat="server" ClientIDMode="Static" OnTextChanged="txtOferta_OnTextChanged" data-mensagemVazia="Selecione uma Solução Educacional" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label9" runat="server" Text="Turma" AssociatedControlID="txtTurma" />
                            <asp:TextBox ID="txtTurma" runat="server" ClientIDMode="Static" data-mensagemVazia="Selecione uma Oferta" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label14" runat="server" Text="Forma de aquisição" AssociatedControlID="ListBoxesFormaDeAquisicao" />
                            <uc2:ucSeletorListBox runat="server" ID="ListBoxesFormaDeAquisicao" />
                        </div>
                        <div class="form-group" id="divUfResposanvel" runat="server">
                            <asp:Label ID="lblUfResposanvel" runat="server" Text="UF responsável" AssociatedControlID="ListBoxesUFResponsavel" />
                            <uc2:ucSeletorListBox runat="server" ID="ListBoxesUFResponsavel" />
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
                            <label id="titleCampos" runat="server" clientidmode="Static" style="margin-left: 20px;">
                                Campos -
                            </label>
                            <asp:CheckBoxList ID="chkListaCamposVisiveis" runat="server" RepeatDirection="Vertical"
                                RepeatLayout="UnorderedList" ClientIDMode="Static">
                                <asp:ListItem Selected="True" Value="Nome">Nome</asp:ListItem>
                                <asp:ListItem Selected="True" Value="CPF">CPF</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Email">Email</asp:ListItem>
                                <asp:ListItem Selected="True" Value="NivelOcupacional">Nível Ocupacional</asp:ListItem>
                                <asp:ListItem Selected="True" Value="UF">UF</asp:ListItem>
                                <asp:ListItem Selected="True" Value="CategoriaPai">Categoria</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Categoria2">2° nível de categoria</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Categoria3">3° nível de categoria</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Categoria">Categoria Vinculada</asp:ListItem>
                                <asp:ListItem Selected="True" Value="SolucaoEducacional">Solução Educacional</asp:ListItem>
                                <asp:ListItem Selected="True" Value="FormaAquisicao">Forma de Aquisição</asp:ListItem>
                                <asp:ListItem Selected="True" Value="CargaHoraria">Carga Horaria</asp:ListItem>
                                <asp:ListItem Selected="True" Value="TipoTutoria">Tipo de Tutoria</asp:ListItem>
                                <asp:ListItem Selected="True" Value="TemMaterial">Tem Material?</asp:ListItem>
                                <asp:ListItem Selected="True" Value="StatusMatricula">Status Matrícula</asp:ListItem>
                                <asp:ListItem Selected="True" Value="MediaFinal">Nota Final</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Oferta">Oferta</asp:ListItem>
                                <asp:ListItem Selected="True" Value="DataInicioInscricoesOferta">Data de início das inscrições</asp:ListItem>
                                <asp:ListItem Selected="True" Value="DataFimInscricoesOferta">Data fim das inscrições</asp:ListItem>
                                <asp:ListItem Selected="True" Value="DataMatricula">Data Matrícula</asp:ListItem>
                                <asp:ListItem Selected="True" Value="DataTermino">Data Conclusão do Aluno</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Turma">Turma</asp:ListItem>
                                <asp:ListItem Selected="True" Value="DataInicioTurma">Data Inicio Turma</asp:ListItem>
                                <asp:ListItem Selected="True" Value="DataFimTurma">Data Fim Turma</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Fornecedor">Fornecedor</asp:ListItem>
                                <asp:ListItem Selected="True" Value="DataGeracaoCertificadoString">Data de Geração Certificado</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Observacao">Observação</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Cidade">Cidade</asp:ListItem>
                                <asp:ListItem Selected="True" Value="DataAdmissao">Data de Admissão</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Sexo">Sexo</asp:ListItem>
                                <asp:ListItem Selected="True" Value="DataNascimento">Data de Nascimento</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Feedback">Feedback</asp:ListItem>
                                <asp:ListItem Selected="True" Value="UFResponsavel">UF responsável</asp:ListItem>
                                <asp:ListItem Selected="True" Value="ResponsavelPelaTurma">Responsável pela Turma</asp:ListItem>
                                <asp:ListItem Selected="True" Value="DataInicioTurma">Data início da Turma</asp:ListItem>
                                <asp:ListItem Selected="True" Value="DataFimTurma">Data fim da Turma</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Cargos">Unidade Demandante</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Unidade">Unidade</asp:ListItem>
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

    <uc:ucTotalizadorRelatorio runat="server" ID="ucTotalizadorRelatorio" />

    <asp:GridView ID="dgRelatorio" runat="server" AutoGenerateColumns="False" AllowSorting="True"
        CssClass="table col-sm-12" GridLines="None" OnSorting="dgRelatorio_Sorting" AllowPaging="True"
        OnPageIndexChanging="dgRelatorio_PageIndexChanging" PageSize="100">
        <Columns>
            <asp:BoundField DataField="Nome" HeaderText="Nome" SortExpression="Nome" />
            <asp:BoundField DataField="CPF" HeaderText="CPF" SortExpression="CPF" />
            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
            <asp:BoundField DataField="NivelOcupacional" HeaderText="Nível Ocupacional" SortExpression="NivelOcupacional" />
            <asp:BoundField DataField="UF" HeaderText="UF" SortExpression="UF" />
            <asp:BoundField DataField="CategoriaPai" HeaderText="Categoria" SortExpression="CategoriaPai" />
            <asp:BoundField DataField="Categoria2" HeaderText="2° nível de categoria" SortExpression="Categoria2" />
            <asp:BoundField DataField="Categoria3" HeaderText="3° nível de categoria" SortExpression="Categoria3" />
            <asp:BoundField DataField="Categoria" HeaderText="Categoria vinculada" SortExpression="Categoria" />
            <asp:BoundField DataField="SolucaoEducacional" HeaderText="Solução Educacional" SortExpression="SolucaoEducacional" />
            <asp:BoundField DataField="FormaAquisicao" HeaderText="Forma Aquisição" SortExpression="FormaAquisicao" />
            <asp:BoundField DataField="CargaHoraria" HeaderText="Carga Horária" SortExpression="CargaHoraria" />
            <asp:BoundField DataField="TipoTutoria" HeaderText="Tipo Tutoria" SortExpression="TipoTutoria" />
            <asp:BoundField DataField="Oferta" HeaderText="Oferta" SortExpression="Oferta" />
            <asp:BoundField DataField="DataInicioInscricoesOferta" HeaderText="Data de início das inscrições" SortExpression="DataInicioInscricoesOferta" />
            <asp:BoundField DataField="DataFimInscricoesOferta" HeaderText="Data fim das inscrições" SortExpression="DataFimInscricoesOferta" />
            <asp:BoundField DataField="Turma" HeaderText="Turma" SortExpression="Turma" />
            <asp:BoundField DataField="DataInicioTurma" HeaderText="Data Inicio Turma" SortExpression="DataInicioTurma" />
            <asp:BoundField DataField="DataFimTurma" HeaderText="Data Fim Turma" SortExpression="DataFimTurma" />
            <asp:BoundField DataField="TemMaterial" HeaderText="Tem Material?" SortExpression="TemMaterial" />
            <asp:BoundField DataField="StatusMatricula" HeaderText="StatusMatricula" SortExpression="StatusMatricula" />
            <asp:BoundField DataField="MediaFinal" HeaderText="Nota Final" SortExpression="MediaFinal" />
            <asp:BoundField DataField="DataMatricula" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Data Matrícula" SortExpression="DataMatricula" />
            <asp:BoundField DataField="DataTermino" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Data de Conclusão do Aluno" SortExpression="DataTermino" />
            <asp:BoundField DataField="Fornecedor" HeaderText="Fornecedor" SortExpression="Fornecedor" />
            <asp:BoundField DataField="DataGeracaoCertificadoString" HeaderText="Data de Geracão Certificado" SortExpression="DataGeracaoCertificado" />
            <asp:BoundField DataField="Observacao" HeaderText="Observação" SortExpression="Observacao" />
            <asp:BoundField DataField="Unidade" HeaderText="Unidade" SortExpression="Unidade" />
            <asp:BoundField DataField="Cidade" HeaderText="Cidade" SortExpression="Cidade" />
            <asp:BoundField DataField="DataAdmissao" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Data da Admissao" SortExpression="DataAdmissao" />
            <asp:BoundField DataField="Sexo" HeaderText="Sexo" SortExpression="Sexo" />
            <asp:BoundField DataField="DataNascimento" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Data de Nascimento" SortExpression="DataNascimento" />
            <asp:BoundField DataField="Feedback" HeaderText="Feedback" SortExpression="Feedback" />
            <asp:BoundField DataField="UFResponsavel" HeaderText="UF Responsável" SortExpression="UFResponsavel" />
            <asp:BoundField DataField="ResponsavelPelaTurma" HeaderText="Responsável pela Turma" SortExpression="ResponsavelPelaTurma" />
            <asp:BoundField DataField="DataInicioTurma" HeaderText="Data início da Turma" SortExpression="DataInicioTurma" />
            <asp:BoundField DataField="DataFimTurma" HeaderText="Data fim da Turma" SortExpression="DataFimTurma" />
            <asp:BoundField DataField="Cargos" HeaderText="Unidade Demandante" SortExpression="Cargos" />
        </Columns>
        <EmptyDataTemplate>
            <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
        </EmptyDataTemplate>
        <PagerSettings Mode="NumericFirstLast" />
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
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="ConsultaDesempenhoAcademico"
        TypeName="Sebrae.Academico.BP.Relatorios.RelatorioDesempenhoAcademico">
        <SelectParameters>
            <asp:Parameter Name="pNome" Type="String" />
            <asp:Parameter Name="pCPF" Type="String" />
            <asp:Parameter Name="pNivelOcupacional" Type="Int32" />
            <asp:Parameter Name="pUf" Type="Int32" />
            <asp:Parameter Name="pDataInicialMatricula" Type="DateTime" />
            <asp:Parameter Name="pDataFinalMatricula" Type="DateTime" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <script type="text/javascript">
        jQuery(function ($) {
            $("#<%= txtDataInicio.ClientID %>").mask("99/99/9999");
            $("#<%= txtDataFinal.ClientID %>").mask("99/99/9999");

            $("#<%= txtDataTerminoInicio.ClientID %>").mask("99/99/9999");
            $("#<%= txtDataTerminoFim.ClientID %>").mask("99/99/9999");

             $("#<%= txtDataInicioTurma.ClientID %>").mask("99/99/9999");
            $("#<%= txtDataFinalTurma.ClientID %>").mask("99/99/9999");
        });

        $(document).ready(function() {
            // Botões de marcar e desmarcar.
            $.markAll('titleCampos', '<%= chkListaCamposVisiveis.ClientID %>', 'Marcar todos', 'Desmarcar todos');
        });
    </script>

</asp:Content>
