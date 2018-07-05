<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucUsuario.ascx.cs" Inherits="Sebrae.Academico.WebForms.UserControls.ucUsuario" %>
<%@ Register Src="~/UserControls/ucTags.ascx" TagName="ucTags" TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>

<fieldset>
    <div class="form-group">
        <asp:Label ID="Label1" runat="server" Text="Nome" AssociatedControlID="txtNome" />
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip1" Chave="txtNome" />
        <asp:TextBox ID="txtNome" MaxLength="250" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group">
        <asp:Label ID="Label2" runat="server" Text="UF" AssociatedControlID="ddlUF" />
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip2" Chave="ddlUF" />
        <asp:DropDownList ID="ddlUF" runat="server" CssClass="form-control">
        </asp:DropDownList>
    </div>
    <div class="form-group">
        <asp:Label ID="Label3" runat="server" Text="CPF" AssociatedControlID="txtCPF" />
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip3" Chave="txtCPF" />
        <asp:TextBox ID="txtCPF" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group">
        <asp:Label ID="tabelTxtSID" runat="server" Text="SID" AssociatedControlID="txtSID" />
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip4" Chave="txtSID" />
        <asp:TextBox ID="txtSID" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group">
        <asp:Label ID="Label4" runat="server" Text="Nível Ocupacional" AssociatedControlID="ddlNivelOcupacional" />
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip5" Chave="ddlNivelOcupacional" />
        <asp:DropDownList ID="ddlNivelOcupacional" CssClass="form-control" runat="server">
        </asp:DropDownList>
    </div>
    <div class="form-group">
        <asp:Label ID="Label5" runat="server" Text="Tipo de Documento" AssociatedControlID="txtTipoDocumento" />
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip6" Chave="txtTipoDocumento" />
        <asp:TextBox ID="txtTipoDocumento" MaxLength="50" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group">
        <asp:Label ID="Label6" runat="server" Text="Número da Identidade" AssociatedControlID="txtNumeroIdentidade" />
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip7" Chave="txtNumeroIdentidade" />
        <asp:TextBox ID="txtNumeroIdentidade" MaxLength="30" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group">
        <asp:Label ID="Label7" runat="server" Text=" Órgão Emissor" AssociatedControlID="txtOrgaoEmissor" />
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip8" Chave="txtOrgaoEmissor" />
        <asp:TextBox ID="txtOrgaoEmissor" MaxLength="20" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group">
        <asp:Label ID="Label8" runat="server" Text="Data de Expedição da Identidade" AssociatedControlID="txtDataExpedicaoIdentidade" />
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip9" Chave="txtDataExpedicaoIdentidade" />
        <asp:TextBox ID="txtDataExpedicaoIdentidade" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group">
        <asp:Label ID="Label9" runat="server" Text="Data de Nascimento" AssociatedControlID="txtDataNascimento" />
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip10" Chave="txtDataNascimento" />
        <asp:TextBox ID="txtDataNascimento" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group">
        <asp:Label ID="Label10" runat="server" Text="Sexo" AssociatedControlID="ddlSexo" />
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip11" Chave="ddlSexo" />
        <asp:DropDownList ID="ddlSexo" runat="server" CssClass="form-control">
            <asp:ListItem Value="0"> - Selecione -</asp:ListItem>
            <asp:ListItem>Masculino</asp:ListItem>
            <asp:ListItem>Feminino</asp:ListItem>
        </asp:DropDownList>
    </div>
    <div class="form-group">
        <asp:Label ID="Label11" runat="server" Text="Nacionalidade" AssociatedControlID="txtNacionalidade" />
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip12" Chave="txtNacionalidade" />
        <asp:TextBox ID="txtNacionalidade" MaxLength="30" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group">
        <asp:Label ID="Label12" runat="server" Text="Estado Civil" AssociatedControlID="txtEstadoCivil" />
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip13" Chave="txtEstadoCivil" />
        <asp:TextBox ID="txtEstadoCivil" MaxLength="15" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group">
        <asp:Label ID="Label13" runat="server" Text="Naturalidade" AssociatedControlID="txtNaturalidade" />
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip14" Chave="txtNaturalidade" />
        <asp:TextBox ID="txtNaturalidade" MaxLength="50" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group">
        <asp:Label ID="Label14" runat="server" Text="Nome do Pai" AssociatedControlID="txtNomePai" />
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip15" Chave="txtNomePai" />
        <asp:TextBox ID="txtNomePai" MaxLength="100" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group">
        <asp:Label ID="Label15" runat="server" Text="Nome da Mãe" AssociatedControlID="txtNomeMae" />
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip16" Chave="txtNomeMae" />
        <asp:TextBox ID="txtNomeMae" MaxLength="100" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group">
        <asp:Label ID="Label16" runat="server" Text="E-mail" AssociatedControlID="txtEmail" />
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip17" Chave="txtEmail" />
        <asp:TextBox ID="txtEmail" MaxLength="50" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group">
        <asp:Label ID="Label17" runat="server" Text="Matricula" AssociatedControlID="txtMatricula" />
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip18" Chave="txtMatricula" />
        <asp:TextBox ID="txtMatricula" MaxLength="20" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group">
        <asp:Label ID="Label18" runat="server" Text="Data de Admissão" AssociatedControlID="txtDataAdmissao" />
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip19" Chave="txtDataAdmissao" />
        <asp:TextBox ID="txtDataAdmissao" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group">
        <asp:Label ID="Label19" runat="server" Text="Status" AssociatedControlID="ddlStatus" />
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip20" Chave="ddlStatus" />
         <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
	        <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
	        <asp:ListItem Value="ativo" Text="Ativo"></asp:ListItem>
	        <asp:ListItem Value="inativo" Text="Inativo"></asp:ListItem>
        </asp:DropDownList>
    </div>
    <div class="form-group">
        <asp:Label ID="lblSituacao" Text="Situação" runat="server" AssociatedControlID="ddlSituacao"></asp:Label>
         <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip42" Chave="ddlSituacao" />
         <asp:DropDownList ID="ddlSituacao" runat="server" CssClass="form-control">
	        <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
	        <asp:ListItem Value="ativo" Text="Ativo"></asp:ListItem>
	        <asp:ListItem Value="inativo" Text="Inativo"></asp:ListItem>
	        <asp:ListItem Value="férias" Text="Férias"></asp:ListItem>
	        <asp:ListItem Value="licença mater." Text="Licença Maternidade"></asp:ListItem>
	        <asp:ListItem Value="licença remun." Text="Licença Remunerada"></asp:ListItem>
	        <asp:ListItem Value="licença s/venc" Text="Licença sem Vencimento"></asp:ListItem>
	        <asp:ListItem Value="admissão prox.mês" Text="Admissão prox.mês"></asp:ListItem>
	        <asp:ListItem Value="af.Ac.Trabalho" Text="Af.Ac.Trabalho"></asp:ListItem>
	        <asp:ListItem Value="af.previdência" Text="Af.previdência"></asp:ListItem>
	        <asp:ListItem Value="aguardando pagamento" Text="Aguardando Pagamento"></asp:ListItem>
	        <asp:ListItem Value="apos. invalidez" Text="Apos. Invalidez"></asp:ListItem>
	        <asp:ListItem Value="recesso remunerado de estagio" Text="Recesso Remunerado de Estágio"></asp:ListItem>
	        <asp:ListItem Value="demitido" Text="Demitido"></asp:ListItem>
             <asp:ListItem Value="licença mater. compl. 180 dias" Text="Licença mater. compl. 180 dias"></asp:ListItem>
        </asp:DropDownList>
    </div>
    <div class="form-group">
        <asp:Label ID="Label20" runat="server" Text="Endereço" AssociatedControlID="txtEndereco" />
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip21" Chave="txtEndereco" />
        <asp:TextBox ID="txtEndereco" MaxLength="200" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group">
        <asp:Label ID="Label21" runat="server" Text="Complemento" AssociatedControlID="txtComplemento" />
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip22" Chave="txtComplemento" />
        <asp:TextBox ID="txtComplemento" MaxLength="50" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group">
        <asp:Label ID="Label22" runat="server" Text="Bairro" AssociatedControlID="txtBairro" />
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip23" Chave="txtBairro" />
        <asp:TextBox ID="txtBairro" MaxLength="50" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group">
        <asp:Label ID="Label23" runat="server" Text="Cidade" AssociatedControlID="txtCidade" />
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip24" Chave="txtCidade" />
        <asp:TextBox ID="txtCidade" MaxLength="50" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group">
        <asp:Label ID="Label24" runat="server" Text="Estado" AssociatedControlID="txtEstado" />
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip25" Chave="txtEstado" />
        <asp:TextBox ID="txtEstado" MaxLength="2" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group">
        <asp:Label ID="Label25" runat="server" Text="País" AssociatedControlID="txtPais" />
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip26" Chave="txtPais" />
        <asp:TextBox ID="txtPais" MaxLength="2" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group">
        <asp:Label ID="Label26" runat="server" Text="CEP" AssociatedControlID="txtCEP" />
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip27" Chave="txtCEP" />
        <asp:TextBox ID="txtCEP" MaxLength="9" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group">
        <asp:Label ID="Label27" runat="server" Text="Telefone Residencial" AssociatedControlID="txtTelResidencial" />
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip28" Chave="txtTelResidencial" />
        <asp:TextBox ID="txtTelResidencial" MaxLength="20" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group">
        <asp:Label ID="Label28" runat="server" Text="Telefone Celular" AssociatedControlID="txtTelCelular" />
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip29" Chave="txtTelCelular" />
        <asp:TextBox ID="txtTelCelular" MaxLength="20" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group">
        <asp:Label ID="Label29" runat="server" Text="Escolaridade" AssociatedControlID="txtEscolaridade" />
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip30" Chave="txtEscolaridade" />
        <asp:TextBox ID="txtEscolaridade" MaxLength="20" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group">
        <asp:Label ID="Label30" runat="server" Text="Instituição" AssociatedControlID="txtInstituicao" />
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip31" Chave="txtInstituicao" />
        <asp:TextBox ID="txtInstituicao" MaxLength="100" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group">
        <asp:Label ID="Label31" runat="server" Text="Tipo de Instituição" AssociatedControlID="txtTipoInstituicao" />
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip32" Chave="txtTipoInstituicao" />
        <asp:TextBox ID="txtTipoInstituicao" MaxLength="50" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group">
        <asp:Label ID="Label32" runat="server" Text="Ano de Conclusão" AssociatedControlID="txtAnoConclusao" />
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip33" Chave="txtAnoConclusao" />
        <asp:TextBox ID="txtAnoConclusao" MaxLength="4" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group">
        <asp:Label ID="Label33" runat="server" Text="Unidade" AssociatedControlID="txtUnidade" />
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip34" Chave="txtUnidade" />
        <asp:TextBox ID="txtUnidade" MaxLength="60" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group">
        <asp:Label ID="Label34" runat="server" Text="Ramal de Exibição" AssociatedControlID="txtRamalExibicao" />
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip35" Chave="txtRamalExibicao" />
        <asp:TextBox ID="txtRamalExibicao" MaxLength="60" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group">
        <asp:Label ID="Label35" runat="server" Text="Tipo de Telefone de Exibição" AssociatedControlID="txtTipoTelefoneExibicao" />
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip36" Chave="txtTipoTelefoneExibicao" />
        <asp:TextBox ID="txtTipoTelefoneExibicao" MaxLength="60" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group">
        <asp:Label ID="Label36" runat="server" Text="Telefone de Exibição" AssociatedControlID="txtTelefoneExibicao" />
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip37" Chave="txtTelefoneExibicao" />
        <asp:TextBox ID="txtTelefoneExibicao" MaxLength="60" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group">
        <asp:Label ID="Label37" runat="server" Text="Nome de Exibição" AssociatedControlID="txtNomeExibicao" />
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip38" Chave="txtNomeExibicao" />
        <asp:TextBox ID="txtNomeExibicao" MaxLength="60" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group">
        <asp:Label ID="Label38" runat="server" Text="Minicurrículum" AssociatedControlID="txtMiniCurriculum" />
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip39" Chave="txtMiniCurriculum" />
        <asp:TextBox ID="txtMiniCurriculum" TextMode="MultiLine" MaxLength="60" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group">
        <asp:Label ID="Label41" runat="server" Text="FOTO" AssociatedControlID="fupldArquivoEnvio" />
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip41" Chave="fupldArquivoEnvio" />
        <br />
        <img id="imgImagem" runat="server" alt="Foto" class="img-thumbnail" />

        <span class="btn btn-file">
            <asp:FileUpload ID="fupldArquivoEnvio" runat="server" EnableViewState="true" ViewStateMode="Enabled" /></span>
    </div>
    <div class="form-group">
        <asp:Label ID="Label40" runat="server" Text="Perfis" AssociatedControlID="chkPerfil" />
        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip40" Chave="chkPerfil" />
        <asp:CheckBoxList ID="chkPerfil" runat="server" CssClass="form-control checkbox-inline-list" RepeatDirection="Vertical" RepeatLayout="UnorderedList">
        </asp:CheckBoxList>
    </div>
    <div class="form-group" runat="server" visible="False">
        <asp:Label ID="Label39" runat="server" Text="Tags" AssociatedControlID="ucTags1" />
        <div class="form-control tabled">
            <uc3:ucTags ID="ucTags1" runat="server" />
        </div>
    </div>
</fieldset>
<script type="text/javascript">

    jQuery(function ($) {
        $("#<%= txtCPF.ClientID %>").mask("999.999.999-99");
        $("#<%= txtDataNascimento.ClientID %>").mask("99/99/9999");
        $("#<%= txtDataExpedicaoIdentidade.ClientID %>").mask("99/99/9999");
        $("#<%= txtCEP.ClientID %>").mask("99999-999");
        $("#<%= txtDataAdmissao.ClientID %>").mask("99/99/9999");
        $("#<%= txtTelResidencial.ClientID %>").mask("(99) 9999-9999?9").ready(function (event) {
            var target, phone, element;
            target = (event.currentTarget) ? event.currentTarget : event.srcElement;
            phone = target.value.replace(/\D/g, '');
            element = $(target);
            element.unmask();
            if (phone.length > 10) {
                element.mask("(99) 99999-999?9");
            } else {
                element.mask("(99) 9999-9999?9");
            }
        });

        $("#<%= txtTelCelular.ClientID %>").mask("(99) 9999-9999?9").ready(function (event) {
            var target, phone, element;
            target = (event.currentTarget) ? event.currentTarget : event.srcElement;
            phone = target.value.replace(/\D/g, '');
            element = $(target);
            element.unmask();
            if (phone.length > 10) {
                element.mask("(99) 99999-999?9");
            } else {
                element.mask("(99) 9999-9999?9");
            }
        });
        $("#<%= txtAnoConclusao.ClientID %>").mask("9999");
        $("#<%= txtAnoConclusao.ClientID %>").mask("9999");
        $("#<%= txtAnoConclusao.ClientID %>").mask("9999");
        $("#<%= txtAnoConclusao.ClientID %>").mask("9999");
    });
</script>
