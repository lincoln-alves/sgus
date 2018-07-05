<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="EdicaoConfiguracaoPagamento.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.EdicaoConfiguracaoPagamento"
    MaintainScrollPositionOnPostback="true" %>
<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <div class="form-group">
            <asp:Label ID="Label1" runat="server" Text="Nome" AssociatedControlID="txtNomeConfiguracaoPagamento"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip1" Chave="nome" />
            <asp:TextBox ID="txtNomeConfiguracaoPagamento" runat="server" CssClass="form-control"
                MaxLength="255"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label2" runat="server" Text="Tipo de Pagamento" AssociatedControlID="cbxTipoPagamento"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip2" Chave="pagamentoTipo" />
            <asp:DropDownList ID="cbxTipoPagamento" runat="server" CssClass="form-control">
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Label ID="Label3" runat="server" Text="Inicio da Competência" AssociatedControlID="txtInicioCompetencia"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip3" Chave="pagamentoInicioCompetencia" />
            <asp:TextBox ID="txtInicioCompetencia" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label4" runat="server" Text="Fim da Competência" AssociatedControlID="txtFimCompetencia"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip4" Chave="pagamentoFimCompetencia" />
            <asp:TextBox ID="txtFimCompetencia" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label5" runat="server" Text="Valor a Pagar" AssociatedControlID="txtValorAPagar"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip5" Chave="pagamentoValorAPagar" />
            <asp:TextBox ID="txtValorAPagar" runat="server" CssClass="form-control" MaxLength="5"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label7" runat="server" Text="Situação" AssociatedControlID="rgpSituacao"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip6" Chave="pagamentoSituacao" />
            <asp:RadioButtonList ID="rgpSituacao" runat="server" RepeatDirection="Horizontal"
                CssClass="form-control">
                <asp:ListItem Value="1" Selected="True">Ativo</asp:ListItem>
                <asp:ListItem Value="0">Inativo</asp:ListItem>
            </asp:RadioButtonList>
        </div>
        <div class="form-group">
            <asp:Label ID="Label8" runat="server" Text="Pagamento" AssociatedControlID="chkBloqueiaAcesso"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip7" Chave="pagamentoBloqueioAcesso" />
            <asp:CheckBox ID="chkBloqueiaAcesso" runat="server" Text="Bloqueia Acesso" CssClass="form-control" />
        </div>
        <div class="form-group">
            <asp:Label ID="Label9" runat="server" Text="Cobrança" AssociatedControlID="chkRecursiva"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip8" Chave="pagamentoRecursiva" />
            <asp:CheckBox ID="chkRecursiva" runat="server" Text="Recursiva" CssClass="form-control" />
        </div>
        <div class="form-group">
            <asp:Label ID="Label10" runat="server" Text="Qtd. Dias de Validade" AssociatedControlID="txtQtdDiasValidade"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip9" Chave="pagamentoDiasValidade" />
            <asp:TextBox ID="txtQtdDiasValidade" runat="server" CssClass="form-control" MaxLength="4"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label11" runat="server" Text="Qtd. Dias para Renovação" AssociatedControlID="txtQtdDiasRenovacao"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip10" Chave="pagamentoDiasRenovacao" />
            <asp:TextBox ID="txtQtdDiasRenovacao" runat="server" CssClass="form-control" MaxLength="4"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label12" runat="server" Text="Qtd. Dias de Inadimplência" AssociatedControlID="txtQtdDiasInadimplencia"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip11" Chave="pagamentoDiasInadimplencia" />
            <asp:TextBox ID="txtQtdDiasInadimplencia" runat="server" CssClass="form-control"
                MaxLength="4"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label13" runat="server" Text="Qtd. Dias Para Pagamento" AssociatedControlID="txtQtdDiasPagamento"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip12" Chave="pagamentoDiasPagamento" />
            <asp:TextBox ID="txtQtdDiasPagamento" runat="server" CssClass="form-control" MaxLength="4"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label6" runat="server" Text="Termo de Adesão" AssociatedControlID="txtTermoAdesao"/>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip13" Chave="pagamentoTermoAdesao" />
            <CKEditor:CKEditorControl ID="txtTermoAdesao" BasePath="/js/ckeditor/" runat="server"></CKEditor:CKEditorControl>
        </div>
        <div class="form-group">
            <uc:Permissoes ID="ucPermissoes" runat="server" />
        </div>
        <div class="form-group">
            <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click"
                CssClass="btn btn-primary mostrarload" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click"
                CssClass="btn btn-default mostrarload" />
        </div>
    </fieldset>
    <script type="text/javascript">
        jQuery(function ($) {
            $("#<%= txtInicioCompetencia.ClientID %>").mask("99/99/9999");
            $("#<%= txtFimCompetencia.ClientID %>").mask("99/99/9999");
        });
    </script>
</asp:Content>
