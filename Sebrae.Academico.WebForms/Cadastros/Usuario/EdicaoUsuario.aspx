<%@ Page Title="" Language="C#" MasterPageFile="~/Academico20.Master" AutoEventWireup="true"
    CodeBehind="EdicaoUsuario.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.EdicaoUsuario" %>

<%@ Register Src="../../UserControls/ucUsuario.ascx" TagName="ucUsuario" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="overflow: scroll;">
        <table>
            <tr>
                <td colspan="2">
                    <h3>
                        <asp:Label ID="Label2" runat="server" Text="Edição de Usuário"></asp:Label></h3>
                </td>
            </tr>
        </table>
        <%-- <div id="divEdicaoUsuario">
        <table>
            <tr>
            
                <td colspan="2">
                    <h4>
                        <asp:Label ID="Label3" runat="server" Text="Dados de Usuário"></asp:Label></h4>
                </td>
            </tr>
       
            <tr>
                <td>
                    Nome:
                </td>
                <td>
                    <strong><asp:Label ID="lblNome" runat="server" Text="Label"></asp:Label></strong>
                </td>
            </tr>

            <tr>
                <td>
                    UF:
                </td>
                <td>
                    <strong><asp:Label ID="lblUF" runat="server" Text="Label"></asp:Label></strong>
                </td>
            </tr>

            <tr>
                <td>
                    CPF:
                </td>
                <td>
                    <strong><asp:Label ID="lblCPF" runat="server" Text="Label"></asp:Label></strong>
                </td>
            </tr>
       
            <tr>
                <td>
                    Nível Ocupacional:
                </td>
                <td>
                    <strong><asp:Label ID="lblNivelOcupacional" runat="server" Text="Label"></asp:Label></strong>
                </td>
            </tr>

            <tr>
                <td>
                    Tipo de Documento:
                </td>
                <td>
                    <strong><asp:Label ID="lblTipoDocumento" runat="server" Text="Label"></asp:Label></strong>
                </td>
            </tr>

            <tr>
                <td>
                    Número da Identidade:
                </td>
                <td>
                    <strong><asp:Label ID="lblNumeroIdentidade" runat="server" Text="Label"></asp:Label></strong>
                </td>
            </tr>

            <tr>
                <td>
                    Órgão Emissor:
                </td>
                <td>
                    <strong><asp:Label ID="lblOrgaoEmissor" runat="server" Text="Label"></asp:Label></strong>
                </td>
            </tr>

            <tr>
                <td>
                    Data de Expedição da Identidade:
                </td>
                <td>
                    <strong><asp:Label ID="lblDataExpedicaoIdentidade" runat="server" Text="Label"></asp:Label></strong>
                </td>
            </tr>

            <tr>
                <td>
                    Data de Nascimento:
                </td>
                <td>
                    <strong><asp:Label ID="lblDataNascimento" runat="server" Text="Label"></asp:Label></strong>
                </td>
            </tr>

            <tr>
                <td>
                    Sexo:
                </td>
                <td>
                    <strong><asp:Label ID="lblSexo" runat="server" Text="Label"></asp:Label></strong>
                </td>
            </tr>

            <tr>
                <td>
                    Nacionalidade:
                </td>
                <td>
                    <strong><asp:Label ID="lblNacionalidade" runat="server" Text="Label"></asp:Label></strong>
                </td>
            </tr>

            <tr>
                <td>
                    Estado Civil:
                </td>
                <td>
                    <strong><asp:Label ID="lblEstadoCivil" runat="server" Text="Label"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td>
                    Naturalidade:
                </td>
                <td>
                    <strong><asp:Label ID="lblNaturalidade" runat="server" Text="Label"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td>
                    Nome do Pai:
                </td>
                <td>
                    <strong><asp:Label ID="lblNomePai" runat="server" Text="Label"></asp:Label></strong>
                </td>
            </tr>

            <tr>
                <td>
                    Nome da Mãe:
                </td>
                <td>
                    <strong><asp:Label ID="lblNomeMae" runat="server" Text="Label"></asp:Label></strong>
                </td>
            </tr>

            <tr>
                <td>
                    E-mail:
                </td>
                <td>
                    <strong><asp:Label ID="lblEmail" runat="server" Text="Label"></asp:Label></strong>
                </td>
            </tr>

            <tr>
                <td>
                    Matricula:
                </td>
                <td>
                    <strong><asp:Label ID="lblMatricula" runat="server" Text="Label"></asp:Label></strong>
                </td>
            </tr>

            <tr>
                <td>
                    Data de Admissão:
                </td>
                <td>
                    <strong><asp:Label ID="lblDataAdmissao" runat="server" Text="Label"></asp:Label></strong>
                </td>
            </tr>

            <tr>
                <td>
                    Situacao:
                </td>
                <td>
                    <strong><asp:Label ID="lblSituacao" runat="server" Text="Label"></asp:Label></strong>
                </td>
            </tr>

            <tr>
                <td>
                    Endereço:
                </td>
                <td>
                    <strong><asp:Label ID="lblEndereco" runat="server" Text="Label"></asp:Label></strong>
                </td>
            </tr>

                <tr>
                <td>
                    Complemento:
                </td>
                <td>
                    <strong><asp:Label ID="lblComplemento" runat="server" Text="Label"></asp:Label></strong>
                </td>
            </tr>

            <tr>
                <td>
                    Bairro:
                </td>
                <td>
                    <strong><asp:Label ID="lblBairro" runat="server" Text="Label"></asp:Label></strong>
                </td>
            </tr>

                <tr>
                <td>
                    Cidade:
                </td>
                <td>
                    <strong><asp:Label ID="lblCidade" runat="server" Text="Label"></asp:Label></strong>
                </td>
            </tr>

                <tr>
                <td>
                    Estado:
                </td>
                <td>
                    <strong><asp:Label ID="lblEstado" runat="server" Text="Label"></asp:Label></strong>
                </td>
            </tr>

                <tr>
                <td>
                    País:
                </td>
                <td>
                    <strong><asp:Label ID="lblPaís" runat="server" Text="Label"></asp:Label></strong>
                </td>
            </tr>

            <tr>
                <td>
                    CEP:
                </td>
                <td>
                    <strong><asp:Label ID="lblCEP" runat="server" Text="Label"></asp:Label></strong>
                </td>
            </tr>

            <tr>
                <td>
                    Endereço 2:
                </td>
                <td>
                    <strong><asp:Label ID="lblEndereco2" runat="server" Text="Label"></asp:Label></strong>
                </td>
            </tr>

                <tr>
                <td>
                    Complemento 2:
                </td>
                <td>
                    <strong><asp:Label ID="lblComplemento2" runat="server" Text="Label"></asp:Label></strong>
                </td>
            </tr>

            <tr>
                <td>
                    Bairro 2:
                </td>
                <td>
                    <strong><asp:Label ID="lblBairro2" runat="server" Text="Label"></asp:Label></strong>
                </td>
            </tr>

                <tr>
                <td>
                    Cidade 2:
                </td>
                <td>
                    <strong><asp:Label ID="lblCidade2" runat="server" Text="Label"></asp:Label></strong>
                </td>
            </tr>

                <tr>
                <td>
                    Estado 2:
                </td>
                <td>
                    <strong><asp:Label ID="lblEstado2" runat="server" Text="Label"></asp:Label></strong>
                </td>
            </tr>

                <tr>
                <td>
                    País 2:
                </td>
                <td>
                    <strong><asp:Label ID="lblPais2" runat="server" Text="Label"></asp:Label></strong>
                </td>
            </tr>

            <tr>
                <td>
                    CEP 2:
                </td>
                <td>
                    <strong><asp:Label ID="lblCEP2" runat="server" Text="Label"></asp:Label></strong>
                </td>
            </tr>

            <tr>
                <td>
                    Telefone Residencial:
                </td>
                <td>
                    <strong><asp:Label ID="lblTelResidencial" runat="server" Text="Label"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td>
                    Telefone Celular:
                </td>
                <td>
                    <strong><asp:Label ID="lblTelCelular" runat="server" Text="Label"></asp:Label></strong>
                </td>
            </tr>

            <tr>
                <td>
                    Material Didático:
                </td>
                <td>
                    <strong><asp:Label ID="lblMaterialDidatico" runat="server" Text="Label"></asp:Label></strong>
                </td>
            </tr>

            <tr>
                <td>
                    Escolaridade:
                </td>
                <td>
                    <strong><asp:Label ID="lblEscolaridade" runat="server" Text="Label"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td>
                    Instituição:
                </td>
                <td>
                    <strong><asp:Label ID="lblInstituição" runat="server" Text="Label"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td>
                    Tipo de Instituição:
                </td>
                <td>
                    <strong><asp:Label ID="lblTipoInstituicao" runat="server" Text="Label"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td>
                    Ano de Conclusão:
                </td>
                <td>
                    <strong><asp:Label ID="lblAnoConclusao" runat="server" Text="Label"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td>
                    Unidade:
                </td>
                <td>
                    <strong><asp:Label ID="lblUnidade" runat="server" Text="Label"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td>
                    Minicurrículum:
                </td>
                <td>
                    <strong><asp:Label ID="lblMinicurriculum" runat="server" Text="Label"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td>
                    Foto:
                </td>
            
                <td>

                    <img id="imgImagem" runat="server" alt="Foto" />
                            
                </td>
            </tr>

        </table>
        <p />
    </div>  --%>
        <uc1:ucUsuario ID="ucUsuario1" runat="server" />
        <div id="divAlterarSenha">
            <table>
                <tr>
                    <td colspan="2">
                        <h4>
                            <asp:Label ID="Label4" runat="server" Text="Alterar Senha do Usuário"></asp:Label></h4>
                    </td>
                </tr>
                <tr>
                    <td>
                        Nova Senha:
                    </td>
                    <td>
                        <asp:TextBox ID="txtNovaSenha" TextMode="Password" MaxLength="80" runat="server"
                            ValidationGroup="vlSenha"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="vdlNovaSenha" runat="server" ControlToValidate="txtNovaSenha"
                            CssClass="alert-danger" ErrorMessage="Campo Obrigatório" ValidationGroup="vlSenha"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        Confirmar Senha:
                    </td>
                    <td>
                        <asp:TextBox ID="txtConfirmarSenha" MaxLength="80" TextMode="Password" runat="server"
                            ValidationGroup="vlSenha"></asp:TextBox>
                        <asp:CompareValidator ID="vldSenha" runat="server" ControlToCompare="txtConfirmarSenha"
                            ControlToValidate="txtNovaSenha" ErrorMessage="O texto informado não confere"
                            CssClass="alert-danger" ValidationGroup="vlSenha"></asp:CompareValidator>
                    </td>
                </tr>
            </table>
            <br />
            <table>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btnSalvarAlteracaoSenha" runat="server" Text="Salvar" OnClick="btnSalvarAlteracaoSenha_Click"
                            ValidationGroup="vlSenha" />
                        <asp:Button ID="btnCancelarAlteracaoSenha" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <p />
        <div id="divAlterarPerfil">
            <h4>
                <asp:Label ID="Label1" runat="server" Text="Alterar Perfil"></asp:Label>
            </h4>
            <br />
            <asp:CheckBoxList ID="chkPerfil" runat="server" RepeatDirection="Vertical" RepeatColumns="3"
                ClientIDMode="Static">
            </asp:CheckBoxList>
            <br />
            <table>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btnSalvarPerfil" runat="server" Text="Salvar" OnClick="btnSalvarPerfil_Click" />
                        <asp:Button ID="btnCancelarPerfil" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="divAlterarTags">
            <h4>
                <asp:Label ID="Label5" runat="server" Text="Alterar Tags"></asp:Label>
            </h4>
            <br />
            <asp:CheckBoxList ID="chkTags" runat="server" RepeatDirection="Vertical" RepeatColumns="3"
                ClientIDMode="Static">
            </asp:CheckBoxList>
            <br />
            <table>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btnSalvarAlteracaoTag" runat="server" Text="Salvar" OnClick="btnSalvarAlteracaoTag_Click" />
                        <asp:Button ID="btnCancelarAlteracaoTag" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
