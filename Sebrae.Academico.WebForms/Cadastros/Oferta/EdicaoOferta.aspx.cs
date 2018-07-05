using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.WebForms.UserControls;

namespace Sebrae.Academico.WebForms.Cadastros
{
    public partial class EdicaoOferta : Page
    {

        private Dominio.Classes.Oferta ofertaEdicao = null;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request["Id"] == null)
            {
                ucOferta1.IdOferta = 0;
            }
            else
            {
                ucOferta1.IdOferta = int.Parse(Request["Id"]);
                this.ucOferta1.AcaoDaTela = (int)ucOferta.enumAcaoDaTela.EdicaoDeUmaOferta;
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            string mensagemRetorno;

            try
            {
                var manterOferta = new ManterOferta();

                if (Request["Id"] == null)
                {
                    Dominio.Classes.Oferta oferta = ucOferta1.ObterObjetoOferta();
                    oferta.Sequencia = manterOferta.ObterProximoCodigoSequencial(oferta.SolucaoEducacional);
                    manterOferta.IncluirOferta(oferta);

                    ofertaEdicao = oferta;

                    ucOferta1.ValidarSubAreasSelecionadas(ofertaEdicao);

                    mensagemRetorno = "Oferta cadastrada com sucesso.";

                }
                else if (!string.IsNullOrEmpty(Request["Republicar"]))
                {
                    var oferta = new Dominio.Classes.Oferta();
                    oferta = ucOferta1.ObterObjetoOferta(true);
                    oferta.Sequencia = manterOferta.ObterProximoCodigoSequencial(oferta.SolucaoEducacional);

                    ucOferta1.ValidarSubAreasSelecionadas(ofertaEdicao);

                    manterOferta.IncluirOferta(oferta);
                    ofertaEdicao = oferta;

                    mensagemRetorno = "Oferta atualizada com sucesso.";
                }
                else
                {
                    ofertaEdicao = ucOferta1.ObterObjetoOferta();

                    if (ofertaEdicao.ID != 0 && manterOferta.AlterouSolucaoEducacional(ofertaEdicao.ID, ofertaEdicao.SolucaoEducacional))
                        ofertaEdicao.Sequencia =
                            manterOferta.ObterProximoCodigoSequencial(ofertaEdicao.SolucaoEducacional);

                    ucOferta1.ValidarSubAreasSelecionadas(ofertaEdicao);

                    manterOferta.AlterarOferta(ofertaEdicao);

                    mensagemRetorno = "Oferta atualizada com sucesso.";
                }

                // Sincronizar Oferta e SE.
                if (ofertaEdicao.SolucaoEducacional.Fornecedor.ID == (int)enumFornecedor.MoodleSebrae)
                    if (manterOferta.SincronizarOfertaComMoodle(ofertaEdicao, ofertaEdicao.SolucaoEducacional))
                        mensagemRetorno = string.Format("Oferta {0} e sincronizada com sucesso.",
                            Request["Id"] == null ? "cadastrada" : "alterada");
            }
            catch (Exception ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                return;
            }

            WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, mensagemRetorno, "ListarOferta.aspx");
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Session.Remove("OfertaEdit");
            Response.Redirect("ListarOferta.aspx");
        }
    }
}