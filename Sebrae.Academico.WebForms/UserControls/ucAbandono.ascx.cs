using System.Collections.Generic;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System.Web.UI.WebControls;
using Sebrae.Academico.BM.Classes;
using System.Linq;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.WebForms.UserControls
{
    public partial class ucAbandono : System.Web.UI.UserControl
    {
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


        public void PrepararTelaParaExibirAbandonosDoUsuario(int IdUsuario)
        {
            if (IdUsuario > 0)
            {
                this.IdUsuario = IdUsuario;
                this.PreencherInformacoesDosAbandonos(IdUsuario);
            }
        }

        private void PreencherInformacoesDosAbandonos(int IdUsuario)
        {
            //ManterAbandono manterAbandono = new ManterAbandono();
            //IList<UsuarioAbandono> ListaAbandonos = manterAbandono.ObterPorUsuario(IdUsuario);
            //WebFormHelper.PreencherGrid(ListaAbandonos, this.dgvAbandonos);

            ManterMatriculaOferta manterMatriculaOferta = new ManterMatriculaOferta();
            var ListaMatriculaOferta = manterMatriculaOferta.ObterPorUsuario(IdUsuario).Where(x => x.StatusMatricula == enumStatusMatricula.Abandono).ToList();
            WebFormHelper.PreencherGrid(ListaMatriculaOferta, this.dgvAbandonos);
        }

        #region "Eventos do Grid"

        protected void dgvAbandonos_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {

            //if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            //{

            //    UsuarioAbandono usuarioAbandono = (UsuarioAbandono)e.Row.DataItem;

            //    if (usuarioAbandono != null && usuarioAbandono.Usuario.ID > 0)
            //    {
            //        CheckBox chkDesconsiderado = (CheckBox)e.Row.FindControl("chkDesconsiderado");

            //        if (chkDesconsiderado != null)
            //        {
            //            chkDesconsiderado.Checked = usuarioAbandono.Desconsiderado;

            //            //Regra -> Uma vez desconsiderado, desabilita o checkbox
            //            if (usuarioAbandono.Desconsiderado)
            //            {
            //                chkDesconsiderado.Enabled = false;
            //            }
            //            else
            //            {
            //                chkDesconsiderado.Enabled = true;
            //            }
            //        }

            //        HiddenField hdfIdUsuarioAbandono = (HiddenField)e.Row.FindControl("hdfIdUsuarioAbandono");

            //        if (hdfIdUsuarioAbandono != null)
            //        {
            //            hdfIdUsuarioAbandono.Value = usuarioAbandono.Usuario.ID.ToString();
            //        }
            //    }

            //}

        }



        #endregion

        /// <summary>
        /// Método criado para manipular, dinamicamente, o evento CheckedChanged do ckeckbox (existente na grid)
        /// </summary>
        /// <param name="sender">objeto que disparou o evento</param>
        /// <param name="e">Argumentos do evento</param>
        protected void chkDesconsiderado_CheckedChanged(object sender, System.EventArgs e)
        {
            CheckBox chkDesconsiderado = (CheckBox)sender;

            if (chkDesconsiderado != null)
            {
                this.AlterarInformacoesSobreOAbandono(chkDesconsiderado);
            }
        }

        private void AlterarInformacoesSobreOAbandono(CheckBox chkDesconsiderado)
        {
            GridViewRow linhadaGrid = (GridViewRow)chkDesconsiderado.NamingContainer;

            if (linhadaGrid != null)
            {

                HiddenField hdfIdUsuarioAbandono = (HiddenField)linhadaGrid.FindControl("hdfIdUsuarioAbandono");

                ManterAbandono manterAbandono = new ManterAbandono();
                UsuarioAbandono usuarioAbandono = manterAbandono.ObterPorID(int.Parse(hdfIdUsuarioAbandono.Value));

                if (usuarioAbandono != null)
                {
                    usuarioAbandono.Desconsiderado = chkDesconsiderado.Checked;
                    manterAbandono.AtualizarUsuarioAbandono(usuarioAbandono);

                    //Atualiza a grid com as informações da matrícula Oferta
                    this.PreencherGridComInformacoesDoUsuarioAbandonoAtualizados(usuarioAbandono);

                    //WebFormHelper.ExibirMensagem(string.Format("O usuário '{0}' foi matriculado na turma '{1}'",
                    //    usuarioAbandono.Usuario.Nome, usuarioAbandono.Turma.Nome));
                }

            }

        }

        private void PreencherGridComInformacoesDoUsuarioAbandonoAtualizados(UsuarioAbandono usuarioAbandono)
        {
            this.PrepararTelaParaExibirAbandonosDoUsuario(usuarioAbandono.ID);
        }

        protected void chkDesconsiderado_PreRender(object sender, System.EventArgs e)
        {
            CheckBox startCheckBox = (CheckBox)sender;
            const string ReturnConfirm = "if (!confirm('Deseja Realmente Desconsiderar este Registro ?')) return false;";
            startCheckBox.Attributes.Add("OnClick", ReturnConfirm);
        }


    }
}