<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucItemTrilha.ascx.cs"
    Inherits="Sebrae.Academico.WebForms.UserControls.ucItemTrilha" %>
<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>

<h3 runat="server" id="h3Acao" visible="False">
    <span id="spanAcao" runat="server" />
</h3>
<fieldset>
    <div class="form-group">
        <asp:Label ID="Label1" runat="server" Text="Trilha" AssociatedControlID="ddlTrilha"></asp:Label>
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip1" Chave="ddlTrilha" />
        <asp:DropDownList ID="ddlTrilha" runat="server" CssClass="form-control mostrarload" AutoPostBack="True" OnSelectedIndexChanged="ddlTrilha_SelectedIndexChanged">
        </asp:DropDownList>
    </div>
    <div class="form-group">
        <asp:Label ID="Label4" runat="server" Text="Nível" AssociatedControlID="ddlTrilhaNivel"></asp:Label>
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip2" Chave="ddlTrilhaNivel" />
        <asp:DropDownList ID="ddlTrilhaNivel" runat="server" CssClass="form-control mostrarload" AutoPostBack="True" OnSelectedIndexChanged="ddlTrilhaNivel_OnSelectedIndexChanged">
        </asp:DropDownList>
    </div>
    <div class="form-group">
        <asp:Label ID="Label19" runat="server" Text="Ponto Sebrae" AssociatedControlID="ddlPontoSebrae"></asp:Label>
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip5" Chave="ddlPontoSebrae" />
        <asp:DropDownList ID="ddlPontoSebrae" runat="server" CssClass="form-control mostrarload" AutoPostBack="True" OnSelectedIndexChanged="ddlPontoSebrae_OnSelectedIndexChanged">
        </asp:DropDownList>
    </div>
    <div class="form-group">
        <asp:Label ID="Label20" runat="server" Text="Missão" AssociatedControlID="ddlMissao"></asp:Label>
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip8" Chave="ddlMissao" />
        <asp:DropDownList ID="ddlMissao" runat="server" CssClass="form-control">
        </asp:DropDownList>
    </div>
    <div class="form-group">
        <asp:Label ID="Label2" runat="server" Text="Nome" AssociatedControlID="txtNome"></asp:Label>
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip3" Chave="txtNome" />
        <asp:TextBox ID="txtNome" runat="server" MaxLength="250" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group">
        <asp:Label ID="Label9" runat="server" Text="Carga Horaria" AssociatedControlID="txtCargaHoraria" />
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip21" Chave="cargaHoraria" />
        <asp:TextBox ID="txtCargaHoraria" runat="server" PlaceHolder="Total em minutos" CssClass="form-control formatHora"></asp:TextBox>
    </div>
    <div class="form-group">
        <asp:Label ID="Label18" runat="server" Text="Quantidade de moedas" AssociatedControlID="txtMoedas"></asp:Label>
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip18" Chave="txtMoedas" />
        <asp:TextBox ID="txtMoedas" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group">
        <asp:Label ID="Label3" runat="server" Text="Ativo" AssociatedControlID="ddlStatus"></asp:Label>
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip4" Chave="ddlStatus" />
        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
        </asp:DropDownList>
    </div>
    <div class="form-group">
        <asp:Label ID="Label14" runat="server" Text="Tipo" AssociatedControlID="ddlTipo"></asp:Label>
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip15" Chave="ddlTipo" />
        <asp:DropDownList ID="ddlTipo" runat="server" CssClass="form-control mostrarload" OnSelectedIndexChanged="ddlTipo_OnSelectedIndexChanged" AutoPostBack="True">
        </asp:DropDownList>
    </div>
    <div class="form-group" runat="server" id="divFormaAquisicao" visible="False">
        <asp:Label ID="Label6" runat="server" Text="Forma de Aquisição" AssociatedControlID="ddlFormaAquisicao"></asp:Label>
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip6" Chave="ddlFormaAquisicao" />
        <asp:DropDownList ID="ddlFormaAquisicao" runat="server" CssClass="form-control">
        </asp:DropDownList>
    </div>

    <asp:Panel ID="pnlArtigoOnline" runat="server" visible="False">
        <div class="form-group">
            <asp:Label ID="LabelPermiteReenvio" runat="server" Text="Permite reenvio de arquivo quando não aprovado?" AssociatedControlID="ddlPermiteReenvio"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip19" Chave="ddlPermiteReenvio" />
            <asp:DropDownList ID="ddlPermiteReenvio" runat="server" CssClass="form-control">
            </asp:DropDownList>
        </div>
    </asp:Panel>

    <div class="form-group" runat="server" id="divSolucao" visible="False">
        <asp:Label ID="Label16" runat="server" Text="Solução Educacional" AssociatedControlID="ddlSolucao"></asp:Label>
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip16" Chave="ddlSolucao" />
        <asp:DropDownList ID="ddlSolucao" runat="server" CssClass="form-control mostrarload" AutoPostBack="True" OnSelectedIndexChanged="ddlSolucao_OnSelectedIndexChanged">
        </asp:DropDownList>
    </div>
    <div class="form-group" runat="server" id="divFaseJogo" visible="False">
        <asp:Label ID="Label7" runat="server" Text="Fase do Jogo" AssociatedControlID="ddlFase"></asp:Label>
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip7" Chave="ddlFase" />
        <asp:DropDownList ID="ddlFase" runat="server" CssClass="form-control">
        </asp:DropDownList>
    </div>
    <div class="form-group" runat="server" id="divQuestionario" visible="False">
        <asp:Label ID="Label17" runat="server" Text="Questionário" AssociatedControlID="ddlQuestionario"></asp:Label>
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip17" Chave="ddlQuestionario" />
        <asp:DropDownList ID="ddlQuestionario" runat="server" CssClass="form-control">
        </asp:DropDownList>
    </div>
    <div class="form-group" runat="server" id="divOrientacoes" visible="False">
        <asp:Label ID="Label10" runat="server" Text="Orientações para participação" AssociatedControlID="txtOrientacoes"></asp:Label>
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip11" Chave="txtLocal" />
        <CKEditor:CKEditorControl MaxLength="2000" ID="txtOrientacoes" BasePath="/js/ckeditor/" runat="server"></CKEditor:CKEditorControl>
    </div>
    <div class="form-group" runat="server" id="divLinkAcesso" visible="False">
        <asp:Label ID="Label12" runat="server" Text="Link de acesso" AssociatedControlID="txtLinkAcesso"></asp:Label>
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip12" Chave="txtLinkAcesso" />
        <asp:TextBox ID="txtLinkAcesso" runat="server" MaxLength="250" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group" runat="server" id="divReferenciaBibliografica" visible="False">
        <asp:Label ID="Label11" runat="server" Text="Referência Bibliográfica" AssociatedControlID="txtReferenciaBibliografica"></asp:Label>
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip13" Chave="txtReferenciaBibliografica" />
        <asp:TextBox ID="txtReferenciaBibliografica" runat="server" MaxLength="1000" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group" runat="server" id="divNomeArquivo">
        <asp:Label ID="Label15" runat="server" Text="Nome do Arquivo" AssociatedControlID="txtNomeArquivo"></asp:Label>
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip20" Chave="txtNomeArquivoSolucaoSebrae" />
        <asp:TextBox ID="txtNomeArquivo" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group" runat="server" id="divArquivoEnvio" visible="False">
        <asp:Label ID="Label13" runat="server" Text="Arquivo de Envio" AssociatedControlID="fupldArquivoEnvio"></asp:Label>
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip14" Chave="fupldArquivoEnvio" />
        <asp:FileUpload ID="fupldArquivoEnvio" runat="server" class="form-control" />
    </div>
    <div class="form-group" runat="server" id="divDownloadArquivo" visible="False">
        <asp:LinkButton ID="lkbArquivo" runat="server" OnClick="lkbArquivo_Click" />
    </div>

    <asp:Panel ID="pnlConheciGame" runat="server" Visible="false">
        <div class="form-group">
            <asp:Label ID="LabelConteudo" runat="server" Text="Conteúdo" AssociatedControlID="ddlConteudo"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltipConteudo" Chave="ddlConteudoConheciGame" />
            <asp:DropDownList ID="ddlConteudo" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlConteudo_SelectedIndexChanged"></asp:DropDownList>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlTemaConheciGame" runat="server" Visible="false">
        <div class="form-group">
            <asp:Label ID="Label5" runat="server" Text="Tema" AssociatedControlID="ddlTema"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip9" Chave="ddlTemaConheciGame" />
            <asp:DropDownList ID="ddlTema" runat="server" CssClass="form-control"></asp:DropDownList>
        </div>

        <div class="form-group">
            <asp:Label ID="Label8" runat="server" Text="Quantidade de acertos no tema" AssociatedControlID="txtAcertosTema"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip10" Chave="txtAcertosTema" />
            <asp:TextBox ID="txtAcertosTema" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
    </asp:Panel>
</fieldset>
