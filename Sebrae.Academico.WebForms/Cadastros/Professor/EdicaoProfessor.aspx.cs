using System;
using System.Web.UI;
using Sebrae.Academico.BP;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using classes = Sebrae.Academico.Dominio.Classes;
using System.Collections.Generic;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.WebForms.Cadastros
{
    public partial class EdicaoProfessor : System.Web.UI.Page
    {

        private classes.Professor professor = null;
        private ManterProfessor manterProfessor = new ManterProfessor();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.PreencherCombos();

                if (Request["Id"] != null)
                {
                    int idProfessor = int.Parse(Request["Id"].ToString());
                    professor = manterProfessor.ObterProfessorPorId(idProfessor);
                    PreencherCampos(professor);
                }
            }
        }

        private void PreencherCampos(classes.Professor professor)
        {
            //Nome
            if (!string.IsNullOrWhiteSpace(professor.Nome)) this.txtNome.Text = professor.Nome;

            //CPF
            if (!string.IsNullOrWhiteSpace(professor.Cpf)) this.txtCPF.Text = professor.Cpf;

            //Data de Nascimento
            if (professor.DataNascimento.HasValue) this.txtDtNascimento.Text = professor.DataNascimento.Value.ToShortDateString();

            //Data de Desativação
            if (professor.DataDesativacao.HasValue) this.txtDtDesativacao.Text = professor.DataDesativacao.Value.ToShortDateString();

            //Ativo ?
            if (rblAtivo.SelectedItem != null && !string.IsNullOrWhiteSpace(rblAtivo.SelectedItem.Value))
            {
                string valorInformadoParaAtivo = rblAtivo.SelectedItem.Value;

                if (valorInformadoParaAtivo.ToString().ToUpper().Equals("S"))
                    professor.Ativo = true;
                else if (valorInformadoParaAtivo.ToString().ToUpper().Equals("N"))
                    professor.Ativo = false;
            }

            //Currículo
            if (!string.IsNullOrWhiteSpace(professor.Curriculo))
                this.txtCurriculo.Text = professor.Curriculo;

            //Observações
            if (!string.IsNullOrWhiteSpace(professor.Observacoes))
                this.txtObservacoes.Text = professor.Observacoes;

            //Nº do Telefone
            if (!string.IsNullOrWhiteSpace(professor.Telefone))
                this.txtTelefone.Text = professor.Telefone;

            //Nº do Telefone Celular
            if (!string.IsNullOrWhiteSpace(professor.TelefoneCelular))
                this.txtTelefoneCelular.Text = professor.TelefoneCelular;

            //RG
            if (!string.IsNullOrWhiteSpace(professor.RG))
                this.txtRG.Text = professor.RG;

            //Tipo do Documento
            if (!string.IsNullOrWhiteSpace(professor.TipoDocumentoRG))
                this.txtTipoDocumento.Text = professor.TipoDocumentoRG;

            //Data de Expedição
            if (professor.DataExpedicao.HasValue)
                this.txtDtExpedicao.Text = professor.DataExpedicao.Value.ToString();

            //Naturalidade
            if (!string.IsNullOrWhiteSpace(professor.Naturalidade))
                this.txtNaturalidade.Text = professor.Naturalidade;

            //Estado Civil
            WebFormHelper.SetarValorNaCombo(professor.EstadoCivil, ddlEstadoCivil);

            //Nome do Pai
            if (!string.IsNullOrWhiteSpace(professor.NomePai))
                this.txtNomePai.Text = professor.NomePai;

            //Nome da Mãe
            if (!string.IsNullOrWhiteSpace(professor.NomeMae))
                this.txtNomeMae.Text = professor.NomeMae;

            //E-mail
            if (!string.IsNullOrWhiteSpace(professor.Email))
                this.txtEmail.Text = professor.Email;

            //Endereço
            if (!string.IsNullOrWhiteSpace(professor.Endereco))
                this.txtEndereco.Text = professor.Endereco;

            //Bairro
            if (!string.IsNullOrWhiteSpace(professor.Bairro))
                this.txtBairro.Text = professor.Bairro;

            //Cidade
            if (!string.IsNullOrWhiteSpace(professor.Cidade))
                this.txtCidade.Text = professor.Cidade;

            //UF
            WebFormHelper.SetarValorNaCombo(professor.Estado, ddlUF);

            //Ativo
            WebFormHelper.SetarValorNoRadioButtonList(professor.Ativo, rblAtivo);
        }

        private void PreencherCombos()
        {
            WebFormHelper.PreencherComponenteComOpcoesSimNao(this.rblAtivo);
            PreencherComboEstadoCivil();
            PreencherComboDeUfs();
        }

        private void PreencherComboEstadoCivil()
        {
            WebFormHelper.PreencherComponenteComEstadoCivil(this.ddlEstadoCivil, true);
        }

        private void PreencherComboDeUfs()
        {
            try
            {
                IList<Uf> ListaUF = new ManterUf().ObterTodosUf();
                WebFormHelper.PreencherLista(ListaUF, ddlUF, false, true, true);
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }


        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {

                if (Request["Id"] == null)
                {
                    manterProfessor = new ManterProfessor();
                    professor = ObterObjetoProfessor();
                    manterProfessor.IncluirProfessor(professor);
                }
                else
                {
                    professor = ObterObjetoProfessor();
                    manterProfessor.AlterarProfessor(professor);
                }

                Session.Remove("ProfessorEdit");

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados Gravados com Sucesso !", "ListarProfessor.aspx");

            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }


        }

        private classes.Professor ObterObjetoProfessor()
        {
            if (Request["Id"] == null)
            {
                manterProfessor = new ManterProfessor();
                professor = new classes.Professor();
                professor.DataCadastro = DateTime.Now;
            }
            else
            {
                int idProfessor = int.Parse(Request["Id"].ToString());
                professor = new ManterProfessor().ObterProfessorPorId(idProfessor);
            }

            //Nome
            professor.Nome = this.txtNome.Text.Trim();

            //CPF
            professor.Cpf = this.txtCPF.Text.Trim();

            //Data de Nascimento
            if (!string.IsNullOrWhiteSpace(this.txtDtNascimento.Text))
                professor.DataNascimento = CommonHelper.TratarData(this.txtDtNascimento.Text, "Data de Nascimento").Value;
            else
                professor.DataNascimento = null;

            //Data de Desativação
            if (!string.IsNullOrWhiteSpace(this.txtDtDesativacao.Text))
                professor.DataDesativacao = CommonHelper.TratarData(this.txtDtDesativacao.Text, "Data de Desativação").Value;
            else
                professor.DataDesativacao = null;

            //Ativo ?
            if (rblAtivo.SelectedItem != null && !string.IsNullOrWhiteSpace(rblAtivo.SelectedItem.Value))
            {
                string valorInformadoParaAtivo = rblAtivo.SelectedItem.Value;

                if (valorInformadoParaAtivo.ToString().ToUpper().Equals("S"))
                    professor.Ativo = true;
                else if (valorInformadoParaAtivo.ToString().ToUpper().Equals("N"))
                    professor.Ativo = false;
            }
            else
            {
                professor.Ativo = false;
            }

            //Currículo
            professor.Curriculo = this.txtCurriculo.Text.Trim();

            //Observações
            professor.Observacoes = this.txtObservacoes.Text.Trim();

            //Nº do Telefone
            professor.Telefone = this.txtTelefone.Text.Trim();

            //Nº do Telefone Celular
            professor.TelefoneCelular = this.txtTelefoneCelular.Text.Trim();

            //RG
            professor.RG = this.txtRG.Text.Trim();

            //Tipo do Documento
            professor.TipoDocumentoRG = this.txtTipoDocumento.Text.Trim();

            //Data de Expedição
            if (!string.IsNullOrWhiteSpace(this.txtDtExpedicao.Text))
                professor.DataExpedicao = CommonHelper.TratarData(this.txtDtExpedicao.Text, "Data de Expedição").Value;
            else
                professor.DataExpedicao = null;

            //Naturalidade
            professor.Naturalidade = this.txtNaturalidade.Text.Trim();

            //Estado Civil
            if (this.ddlEstadoCivil.SelectedItem != null && !string.IsNullOrWhiteSpace(this.ddlEstadoCivil.SelectedItem.Value))
                professor.EstadoCivil = this.ddlEstadoCivil.SelectedItem.Value;
            else
                professor.EstadoCivil = null;

            //Nome do Pai
            professor.NomePai = this.txtNomePai.Text.Trim();

            //Nome da Mãe
            professor.NomeMae = this.txtNomeMae.Text.Trim();

            //E-mail
            professor.Email = this.txtEmail.Text.Trim();

            //Endereço
            professor.Endereco = this.txtEndereco.Text.Trim();

            //Bairro
            professor.Bairro = this.txtBairro.Text.Trim();

            //Cidade
            professor.Cidade = this.txtCidade.Text.Trim();

            //UF
            if (this.ddlUF.SelectedItem != null && !string.IsNullOrWhiteSpace(this.ddlUF.SelectedItem.Value))
                professor.Estado = this.ddlUF.SelectedItem.Value;
            else
                professor.Estado = null;



            return professor;
        }


    }
}
