using System.Collections.Generic;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.UserControls
{
    /// <summary>
    /// User Control com mensagens referentes ao Fale Conosco.
    /// </summary>
    public partial class ucFaleConosco : System.Web.UI.UserControl
    {

        /// <summary>
        /// Carrega as Mensagens do Fale Conosco
        /// </summary>
        public void CarregarMensagensDoUsuario()
        {
            try
            {

                if (!string.IsNullOrWhiteSpace(CPFUsuario))
                {
                    IList<LogFaleConosco> ListaMensagensDoFaleConosco = new ManterFaleConosco().ListarPorCPF(this.CPFUsuario);
                    WebFormHelper.PreencherGrid(ListaMensagensDoFaleConosco, dgvFaleConosco);
                }

            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        public string CPFUsuario
        {
            get
            {
                if (ViewState["ViewStateCpfUsuario"] != null)
                {
                    return (string)ViewState["ViewStateCpfUsuario"];
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                ViewState["ViewStateCpfUsuario"] = value;
            }

        }
    }
}