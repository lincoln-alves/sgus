using System;
using System.Web.UI;
using Sebrae.Academico.BP;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.WebForms.Cadastros
{
    public partial class EditarOrcamentoReembolso : Page
    {
        private Dominio.Classes.OrcamentoReembolso _orcamento = null;

        private readonly ManterOrcamentoReembolso _manterOrcamentoReembolso = new ManterOrcamentoReembolso();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            if (Request["Id"] == null) return;

            var id = Convert.ToInt32(Request["ID"]);

            _orcamento = _manterOrcamentoReembolso.ObterPorId(id);

            if (_orcamento == null)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Registro não encontrado.", "/Cadastros/OrcamentoReembolso/ListarOrcamentoReembolso.aspx");
                return;
            }

            PreencherCampos();
        }

        void PreencherCampos()
        {
            if (Request["Id"] != null)
            {
                txtAno.Text = _orcamento.Ano.ToString();
                txtAno.Enabled = false;
            }

            txtOrcamento.Text = _orcamento.Orcamento.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
        }

        Dominio.Classes.OrcamentoReembolso ObterObjetoOrcamentoReembolso()
        {
            var novo = Request["Id"] == null;
            var obj = novo
                ? new Dominio.Classes.OrcamentoReembolso()
                : _manterOrcamentoReembolso.ObterPorId(Convert.ToInt32(Request["id"]));

            if (Request["Id"] == null)
            {
                obj.Ano = int.Parse(txtAno.Text);
            }

            obj.Orcamento = decimal.Parse(txtOrcamento.Text.Replace(",", "").Replace(".", ","));

            return obj;
        }

        public bool ValidarOrcamento()
        {
            if(string.IsNullOrWhiteSpace(txtAno.Text))
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Ano é obrigatório");
                return false;
            }

            int ano;
            if(int.TryParse(txtAno.Text, out ano) == false)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Ano inválido");
                return false;
            }

            if(string.IsNullOrWhiteSpace(txtOrcamento.Text))
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Valor do Orçamento é obrigatório");
                return false;
            }

            decimal p;
            if (decimal.TryParse(txtOrcamento.Text.Replace(",", "").Replace(".", ","), out p) == false)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Valor do Orçamento inválido");
                return false;
            }

            if (Request["Id"] == null && new ManterOrcamentoReembolso().AnoExiste(ano))
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, $"O ano {ano} já está cadastrado");
                return false;
            }

            return true;
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if(ValidarOrcamento())
            {
                _manterOrcamentoReembolso.Salvar(ObterObjetoOrcamentoReembolso());

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados Gravados com Sucesso!", "/Cadastros/OrcamentoReembolso/ListarOrcamentoReembolso.aspx");
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListarOrcamentoReembolso.aspx");
        }
    }
}
