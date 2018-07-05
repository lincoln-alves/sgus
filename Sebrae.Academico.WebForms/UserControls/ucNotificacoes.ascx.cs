using System.Collections.Generic;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;
using System;
using System.Web.UI.WebControls;

namespace Sebrae.Academico.WebForms.UserControls
{
    public partial class ucNotificacoes : BaseUserControl
    {
        protected override IList<enumPerfil> PerfisNecessariosParaAcessarAFuncionalidade
        {
            get
            {
                IList<enumPerfil> perfisNecessariosParaAcessarAFuncionalidade = new List<enumPerfil>();
                perfisNecessariosParaAcessarAFuncionalidade.Add(enumPerfil.Administrador);
                perfisNecessariosParaAcessarAFuncionalidade.Add(enumPerfil.GestorUC);
                return perfisNecessariosParaAcessarAFuncionalidade;
            }
        }

       

        /// <summary>
        /// ID do Usuário. O ID do usuário é persistido (armazenado) no viewstate da página.
        /// </summary>
        public int? IdUsuario
        {
            get
            {
                if (ViewState["ViewStateIdUsuario"] != null)
                {
                    return (int)ViewState["ViewStateIdUsuario"];
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                ViewState["ViewStateIdUsuario"] = value;
            }

        }


        public void PrepararTelaParaExibirNotificacoesDoUsuario(int IdUsuario)
        {
            if (IdUsuario > 0)
            {
                this.IdUsuario = IdUsuario;
                this.PreencherInformacoesDasNotificacoes(IdUsuario);
            }
        }

        private void PreencherInformacoesDasNotificacoes(int IdUsuario)
        {
            ManterNotificacao manterNotificacao = new ManterNotificacao();
            IList<Notificacao> ListaNotificacoes = manterNotificacao.ObterUltimasNotificacoesDoUsuario(IdUsuario);
            WebFormHelper.PreencherGrid(ListaNotificacoes, this.dgvNotificacoes);
        }

        public void CarregarInformacoesSobreNotificacoes()
        {
            try
            {
                IList<DTONotificacao> ListaNotificacoesDoUsuario = new ManterDashBoard().ListaNotificacoesMaisAcessadasPorUsuario();

                if (ListaNotificacoesDoUsuario != null && ListaNotificacoesDoUsuario.Count > 0)
                {
                    WebFormHelper.PreencherGrid(ListaNotificacoesDoUsuario, dgvNotificacoes);
                }
                if (dgvNotificacoes.Rows.Count > 0)
                {
                    dgvNotificacoes.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

    }
}