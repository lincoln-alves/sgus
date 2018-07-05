using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.WebForms.Cadastros.Pagamento
{
    public partial class AdministrarPagamento : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            BMUsuario bmUsuario = new BMUsuario();
            IList<Usuario> listaUsuario = bmUsuario.ObterPorFiltros(new Usuario { Nome = txtNome.Text, CPF = txtCPF.Text });
            dvDdlUsuario.Visible = true;
            WebFormHelper.PreencherLista(listaUsuario, ddlUsuario, false, true);
        }

        protected void PreencherGrid(int idUsuario)
        {
            BMUsuarioPagamento bmUsuarioPagamento = new BMUsuarioPagamento();
            IList<UsuarioPagamento> listaUsuarioPagamento = bmUsuarioPagamento.ObterInformacoesDePagamentoDoUsuario(idUsuario);
            if (listaUsuarioPagamento != null && listaUsuarioPagamento.Count() > 0)
            {
                pnlInformacoesDePagamento.Visible = true;
                WebFormHelper.PreencherGrid(listaUsuarioPagamento, dgvDados);
            }
            else
            {
                dvPagamento.Visible = true;
            }
        }

        protected void ddlUsuario_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            PreencherGrid(int.Parse(ddlUsuario.SelectedValue));
        }

        protected void btnGerarDadosPagamento_Click(object sender, EventArgs e)
        {
            BMUsuarioPagamento bmUsuarioPagamento = new BMUsuarioPagamento();

            UsuarioPagamento novoUsuarioPagamento= new UsuarioPagamento();

            novoUsuarioPagamento.Usuario = new BMUsuario().ObterPorId(int.Parse(ddlUsuario.SelectedValue));
            novoUsuarioPagamento.ConfiguracaoPagamento = new BMConfiguracaoPagamento().ObterPorID(1);
            novoUsuarioPagamento.DataInicioVigencia = DateTime.Now;
            novoUsuarioPagamento.DataFimVigencia = DateTime.Now.AddYears(2);
            novoUsuarioPagamento.ValorPagamento = 15;
            novoUsuarioPagamento.DataInicioRenovacao = DateTime.Now.AddYears(2);
            novoUsuarioPagamento.DataMaxInadimplencia = DateTime.Now.AddMonths(2);
            novoUsuarioPagamento.PagamentoConfirmado = false;
            novoUsuarioPagamento.PagamentoEfetuado = false;
            novoUsuarioPagamento.FormaPagamento = enumFormaPagamento.Boleto;
            novoUsuarioPagamento.DataAceiteTermoAdesao = DateTime.Now;
            novoUsuarioPagamento.DataVencimento = DateTime.Now.AddDays(2);
            novoUsuarioPagamento.PagamentoEnviadoBanco = false;


            //CADASTRA O NOVO REGISTRO
            bmUsuarioPagamento.Salvar(novoUsuarioPagamento);

            //RECUPERA O NOSSO NUMERO E SALVA O REGISTRO
            novoUsuarioPagamento.NossoNumero = new BMUsuarioPagamento().GerarNossoNumero(novoUsuarioPagamento);
            bmUsuarioPagamento.Salvar(novoUsuarioPagamento);

            PreencherGrid(novoUsuarioPagamento.Usuario.ID);
        }

        protected void dgvDados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idPagamento = int.Parse(e.CommandArgument.ToString());

            BMUsuarioPagamento bmUsuarioPagamento = new BMUsuarioPagamento();

            UsuarioPagamento pagamento = bmUsuarioPagamento.ObterInformacoesDePagamentoPorID(idPagamento);

            if (pagamento.PagamentoConfirmado.HasValue && !pagamento.PagamentoConfirmado.Value)
            {
                pagamento.DataPagamento = DateTime.Now;
                pagamento.PagamentoEfetuado = true;
                pagamento.PagamentoConfirmado = true;
                pagamento.DataPagamentoInformado = DateTime.Now;

                bmUsuarioPagamento.Salvar(pagamento);

                PreencherGrid(pagamento.Usuario.ID);
            }
        }
    }
}