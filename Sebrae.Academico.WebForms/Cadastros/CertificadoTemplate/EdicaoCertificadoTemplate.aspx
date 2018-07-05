<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeBehind="EdicaoCertificadoTemplate.aspx.cs"
    Inherits="Sebrae.Academico.WebForms.Cadastros.EdicaoCertificadoTemplate" %>

<%@ Register Src="~/UserControls/ucCategorias.ascx" TagName="ucCategorias" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucVisualizaCertificadoTemplate.ascx" TagName="ucVisualizaCertificadoTemplate" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <div class="form-group" id="divCategoriaMoodle" runat="server">
            <asp:Label ID="lblCategoria" runat="server" Text="CATEGORIA" AssociatedControlID="ucCategorias1" />
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip1" Chave="categorias" />
            <uc1:ucCategorias ID="ucCategorias1" runat="server" />
        </div>
        <asp:Panel ID="pnlConteudoGeral" runat="server">
            <div class="form-group">
                <asp:Label ID="Label6" runat="server" Text="Template para" AssociatedControlID="rblInProfessor"></asp:Label>
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip2" Chave="templatePara" />
                <asp:RadioButtonList ID="rblInProfessor" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="Aluno" Text="Aluno"></asp:ListItem>
                    <asp:ListItem Value="Professor" Text="Professor" Selected="True"></asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <div class="form-group">
                <asp:Label ID="Label2" runat="server" Text="Nome" AssociatedControlID="txtNome"></asp:Label>
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip3" Chave="nome" />
                <asp:TextBox ID="txtNome" runat="server" CssClass="form-control" MaxLength="250"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="Label1" runat="server" Text="Texto*" AssociatedControlID="txtTexto"></asp:Label>
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip4" Chave="templateCertificadoTexto" />
                <CKEditor:CKEditorControl ID="txtTexto" BasePath="/js/ckeditor/" runat="server"></CKEditor:CKEditorControl>

                <script type='text/javascript'>
                    var Suggestions = {};

                    Suggestions[0] = [
                        {"id":"ALUNO","label":"#ALUNO"},
                        {"id":"CPF","label":"#CPF"},
                        {"id":"NOMESE","label":"#NOMESE"},
                        {"id":"CARGAHORARIA","label":"#CARGAHORARIA"},
                        {"id":"CODIGOCERTIFICADO","label":"#CODIGOCERTIFICADO"},
                        {"id":"DATAGERACAOCERTIFICADO","label":"#DATAGERACAOCERTIFICADO"},
                        {"id":"DATAINICIOTURMA","label":"#DATAINICIOTURMA"},
                        {"id":"DATAFIMTURMA","label":"#DATAFIMTURMA"},
                        {"id":"PROFESSOR","label":"#PROFESSOR"},
                        {"id":"TURMA","label":"#TURMA"},
                        {"id":"OFERTA","label":"#OFERTA"},
                        {"id":"LOCAL","label":"#LOCAL"},
                        {"id":"TEXTOPORTAL","label":"#TEXTOPORTAL"},
                        {"id":"INFORMACOESADICIONAIS","label":"#INFORMACOESADICIONAIS"},
                        {"id":"AREATEMATICA","label":"#AREATEMATICA"},
                        {"id": "CARGAHORARIASOLUCAOSEBRAE", "label": "#CARGAHORARIASOLUCAOSEBRAE"}
                    ];

                    Suggestions[1] = [
                        {"id":"ALUNO","label":"#ALUNO"},
                        {"id":"CPF","label":"#CPF"},{"id":"NOMESE","label":"#NOMESE"},
                        {"id":"CARGAHORARIA","label":"#CARGAHORARIA"}, 
                        {"id":"CODIGOCERTIFICADO","label":"#CODIGOCERTIFICADO"}, 
                        {"id":"DATAGERACAOCERTIFICADO","label":"#DATAGERACAOCERTIFICADO"},
                        {"id":"DATAINICIOTURMA","label":"#DATAINICIOTURMA"},
                        {"id":"DATAFIMTURMA","label":"#DATAFIMTURMA"},
                        {"id":"TURMA","label":"#TURMA"},
                        {"id":"OFERTA","label":"#OFERTA"},
                        {"id":"TRILHANIVEL","label":"#TRILHANIVEL"},
                        {"id":"TRILHA","label":"#TRILHA"},
                        {"id":"DATAINICIOTRILHA","label":"#DATAINICIOTRILHA"},
                        {"id":"DATAFIMTRILHA","label":"#DATAFIMTRILHA"},
                        {"id":"MEDIAFINALTRILHA","label":"#MEDIAFINALTRILHA"},
                        {"id":"TEXTOPORTAL","label":"#TEXTOPORTAL"},
                        {"id":"INFORMACOESADICIONAIS","label":"#INFORMACOESADICIONAIS"},
                        {"id":"AREATEMATICA","label":"#AREATEMATICA"}, 
                        {"id": "CARGAHORARIASOLUCAOSEBRAE", "label": "#CARGAHORARIASOLUCAOSEBRAE"}
                    ];

                    CKEDITOR.on('instanceReady', function(evt) {
                        CKEDITOR.instances.<%=txtTexto.ClientID%>.execCommand('reloadSuggestionBox', '<%= chkCertificadoTrilhas.Checked %>' == 'True' ? 1 : 0 );
                    });
                </script>

            </div>

            <div id="divImagem1" class="form-group" runat="server">
                <asp:Label ID="Label5" runat="server" Text="Imagem Frente*" AssociatedControlID="fupldArquivoEnvio1"></asp:Label>
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip5" Chave="templateCertificadoImagem" />

                <div class="image-upload-control">
                    <img id="imgImagem1" runat="server" alt="Foto" class="img-thumbnail" />
                    <span class="btn btn-file">
                        <asp:FileUpload ID="fupldArquivoEnvio1" runat="server" EnableViewState="true" ViewStateMode="Enabled" /></span>
                    <asp:CheckBox ID="chkbExcluir1" runat="server" />
                </div>
            </div>
            <div class="form-group">
                <asp:CheckBox Text="&nbsp;Certificado de trilhas?" ID="chkCertificadoTrilhas" runat="server" Visible="true" AutoPostBack="True" OnCheckedChanged="chkCertificadoTrilhas_OnCheckedChanged" />
                <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip9" Chave="chkLiberarValidacao" />
            </div>
            <div class="form-group" runat="server" ID="divVerso">
                <asp:Label ID="Label3" runat="server" Text="Texto do Verso" AssociatedControlID="txtTextoCertificado2"></asp:Label>
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip6" Chave="templateCertificadoTexto2" />
                <CKEditor:CKEditorControl ID="txtTextoCertificado2" BasePath="/js/ckeditor/" runat="server"></CKEditor:CKEditorControl>
                
                <script type='text/javascript'>
                
                    CKEDITOR.on('instanceReady', function(evt) {
                        CKEDITOR.instances.<%=txtTextoCertificado2.ClientID%>.execCommand('reloadSuggestionBox', '<%= chkCertificadoTrilhas.Checked %>' == 'True' ? 1 : 0 );
                    });
                </script>
                
            </div>

            <div class="form-group">
                <asp:Label ID="Label4" runat="server" Text="Imagem Verso" AssociatedControlID="fupldArquivoEnvio2"></asp:Label>
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip7" Chave="templateCertificadoImagem2" />

                <div class="image-upload-control">
                    <img id="imgImagem2" runat="server" alt="Foto" class="img-thumbnail" />
                    <span class="btn btn-file">
                        <asp:FileUpload ID="fupldArquivoEnvio2" runat="server" EnableViewState="true" ViewStateMode="Enabled" /></span>
                    <asp:CheckBox ID="chkbExcluir2" runat="server" />
                </div>
            </div>

            <div class="form-group">
                <asp:Label Text="Ativo" runat="server" AssociatedControlID="rblAtivo"></asp:Label>
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip8" Chave="ativo" />
                <asp:RadioButtonList runat="server" ID="rblAtivo" RepeatDirection="Horizontal">
                </asp:RadioButtonList>
            </div>
        </asp:Panel>
        <div class="form-group">
            <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click" CssClass="btn btn-primary" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" CssClass="btn btn-default" />
            <asp:Button ID="btnVisualizar" runat="server" Text="Visualizar" OnClick="btnVisualizar_Click" ToolTip="As alterações só poderão ser visualizadas depois de salvas" CssClass="btn btn-default" />
        </div>
    </fieldset>
    <uc1:ucVisualizaCertificadoTemplate ID="ucMostraPreviaRel" runat="server" />
</asp:Content>
