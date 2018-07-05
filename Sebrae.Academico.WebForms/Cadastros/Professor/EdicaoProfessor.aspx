<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="EdicaoProfessor.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.EdicaoProfessor" %>
<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <div class="form-group">
            <asp:Label ID="Label2" runat="server" Text="Nome*" AssociatedControlID="txtNome" ></asp:Label>
            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip1" Chave="nome" />
            <asp:TextBox ID="txtNome" runat="server" MaxLength="250" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label1" runat="server" Text="CPF" AssociatedControlID="txtCPF" ></asp:Label>
            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip2" Chave="cpf" />
            <asp:TextBox ID="txtCPF" runat="server" MaxLength="11" CssClass="form-control" onkeypress="return EhNumerico(event)"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label3" runat="server" Text="Data de Nascimento" AssociatedControlID="txtDtNascimento" ></asp:Label>
            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip3" Chave="dataNascimento" />
            <asp:TextBox ID="txtDtNascimento" runat="server" MaxLength="10" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label4" runat="server" Text="Data de Desativação" AssociatedControlID="txtDtDesativacao" ></asp:Label>
            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip4" Chave="dataDesativacao" />
            <asp:TextBox ID="txtDtDesativacao" runat="server" MaxLength="10" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label5" runat="server" Text="Ativo" AssociatedControlID="rblAtivo" ></asp:Label>
            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip5" Chave="statusAtivo" />
            <asp:RadioButtonList ID="rblAtivo" runat="server" RepeatDirection="Horizontal" CssClass="form-control"></asp:RadioButtonList>
        </div>
        <div class="form-group">
            <asp:Label ID="Label6" runat="server" Text="Currículo" AssociatedControlID="txtCurriculo" ></asp:Label>
            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip6" Chave="txtCurriculo" />
            <asp:TextBox ID="txtCurriculo" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label7" runat="server" Text="Observações" AssociatedControlID="txtObservacoes" ></asp:Label>
            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip7" Chave="txtObservacoes" />
            <asp:TextBox ID="txtObservacoes" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label8" runat="server" Text="Telefone" AssociatedControlID="txtTelefone" ></asp:Label>
            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip8" Chave="telefoneFixo" />
            <asp:TextBox ID="txtTelefone" runat="server" MaxLength="15" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label9" runat="server" Text="Celular" AssociatedControlID="txtTelefoneCelular" ></asp:Label>
            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip9" Chave="telefoneCelular" />
            <asp:TextBox ID="txtTelefoneCelular" runat="server" MaxLength="15" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label10" runat="server" Text="RG" AssociatedControlID="txtRG" ></asp:Label>
            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip10" Chave="RG" />
            <asp:TextBox ID="txtRG" runat="server" MaxLength="15" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label11" runat="server" Text="Tipo do RG" AssociatedControlID="txtTipoDocumento" ></asp:Label>
            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip11" Chave="RGTipo" />
            <asp:TextBox ID="txtTipoDocumento" runat="server" MaxLength="15" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label13" runat="server" Text="Data de Expedição RG" AssociatedControlID="txtDtExpedicao" ></asp:Label>
            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip12" Chave="RGDataExpedicao" />
            <asp:TextBox ID="txtDtExpedicao" runat="server" MaxLength="11" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label14" runat="server" Text="Naturalidade" AssociatedControlID="txtNaturalidade" ></asp:Label>
            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip13" Chave="naturalidade" />
            <asp:TextBox ID="txtNaturalidade" runat="server" MaxLength="25" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label15" runat="server" Text="Estado Civil" AssociatedControlID="ddlEstadoCivil" ></asp:Label>
            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip14" Chave="estadoCivil" />
            <asp:DropDownList ID="ddlEstadoCivil" runat="server" CssClass="form-control"></asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Label ID="Label16" runat="server" Text="Nome do Pai" AssociatedControlID="txtNomePai" ></asp:Label>
            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip15" Chave="nomePai" />
            <asp:TextBox ID="txtNomePai" runat="server" MaxLength="250" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label17" runat="server" Text="Nome da Mãe" AssociatedControlID="txtNomeMae" ></asp:Label>
            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip16" Chave="nomeMae" />
            <asp:TextBox ID="txtNomeMae" runat="server" MaxLength="250" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label18" runat="server" Text="E-mail" AssociatedControlID="txtEmail" ></asp:Label>
            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip17" Chave="email" />
            <asp:TextBox ID="txtEmail" runat="server" MaxLength="150" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label19" runat="server" Text="Endereço" AssociatedControlID="txtEndereco" ></asp:Label>
            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip18" Chave="enderecoFisico" />
            <asp:TextBox ID="txtEndereco" runat="server" MaxLength="150" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label20" runat="server" Text="Bairro" AssociatedControlID="txtBairro" ></asp:Label>
            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip19" Chave="bairro" />
            <asp:TextBox ID="txtBairro" runat="server" MaxLength="50" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label21" runat="server" Text="Cidade" AssociatedControlID="txtCidade" ></asp:Label>
            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip20" Chave="cidade" />
            <asp:TextBox ID="txtCidade" runat="server" MaxLength="50" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label22" runat="server" Text="UF" AssociatedControlID="ddlUF" ></asp:Label>
            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip21" Chave="UF" />
            <asp:DropDownList ID="ddlUF" runat="server" CssClass="form-control"></asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click" CssClass="btn btn-primary" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-default" />
        </div>
    </fieldset>

    <script type="text/javascript">
        jQuery(function ($) {
            $("#<%= txtDtNascimento.ClientID %>").mask("99/99/9999");
            $("#<%= txtDtDesativacao.ClientID %>").mask("99/99/9999");
            $("#<%= txtTelefone.ClientID %>").mask("(99)9999-9999");
            $("#<%= txtTelefoneCelular.ClientID %>").mask("(99)9999-9999?9");
            $("#<%= txtDtExpedicao.ClientID %>").mask("99/99/9999");

        });
    </script>
</asp:Content>
