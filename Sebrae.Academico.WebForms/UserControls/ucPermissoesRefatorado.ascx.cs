using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.UserControls
{
    public partial class ucPermissoesRefatorado : UserControl
    {
        internal void LimparCampos()
        {
            foreach (ListItem registro in ckblstPerfil.Items)
            {
                registro.Selected = false;
            }

            foreach (ListItem registro in ckblstNivelOcupacional.Items)
            {
                registro.Selected = false;
            }
        }

        internal void PreencherListas()
        {
            PreencherUfs();
            PreencherPerfis();
            PreencherNiveisOcupacionais();
        }

        private void PreencherUfs(bool exibirVagasPorUf = false)
        {
            try
            {
                var ufs = new ManterUf().ObterTodosUf();

                rptUFs.DataSource = ufs;
                rptUFs.DataBind();

                for (int i = 0; i < ufs.Count(); i++)
                {
                    var ckUf = (CheckBox)rptUFs.Items[i].FindControl("ckUF");
                    var lblUf = (Label)rptUFs.Items[i].FindControl("lblUF");
                    var txtVagas = (TextBox)rptUFs.Items[i].FindControl("txtVagas");

                    ckUf.ToolTip = ufs[i].Nome;
                    ckUf.Attributes.Add("ID_UF", ufs[i].ID.ToString());

                    if (exibirVagasPorUf)
                    {
                        lblUf.Text = ufs[i].Sigla + " | Vagas: ";
                    }
                    else
                    {
                        lblUf.Text = ufs[i].Nome;
                        liVagaParaTodos.Visible =
                        txtVagas.Visible =
                        txtVagasTodos.Visible = false;
                    }
                }
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private void PreencherPerfis()
        {
            try
            {
                var perfis = new ManterPerfil().ObterTodosPerfis();
                WebFormHelper.PreencherLista(perfis, ckblstPerfil);
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private void PreencherNiveisOcupacionais()
        {
            try
            {
                var niveis = new ManterNivelOcupacional().ObterTodosNivelOcupacional();
                WebFormHelper.PreencherLista(niveis, ckblstNivelOcupacional);
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        public void SelecionarPerfis(IList<Perfil> perfis)
        {
            SelecionarPerfis(perfis.Select(x => x.ID).ToList());
        }

        public void SelecionarPerfis(IList<int> perfis)
        {
            foreach (ListItem ckbPerfil in ckblstPerfil.Items)
            {
                int id;

                if (int.TryParse(ckbPerfil.Value, out id) && perfis.Contains(id))
                    ckbPerfil.Selected = true;
            }
        }

        public void SelecionarNiveisOcupacionais(IList<NivelOcupacional> niveis)
        {
            SelecionarNiveisOcupacionais(niveis.Select(x => x.ID).ToList());
        }

        public void SelecionarNiveisOcupacionais(IList<int> niveis)
        {
            foreach (ListItem ckbNivel in ckblstNivelOcupacional.Items)
            {
                int id;

                if (int.TryParse(ckbNivel.Value, out id) && niveis.Contains(id))
                    ckbNivel.Selected = true;
            }
        }

        public void SelecionarUfs(IList<Uf> listaUfs)
        {
            SelecionarUfs(listaUfs.Select(x => x.ID).ToList());
        }

        public void SelecionarUfs(IList<int> listaUfs)
        {
            for (var i = 0; i < rptUFs.Controls.Count; i++)
            {
                var ckUf = (CheckBox)rptUFs.Items[i].FindControl("ckUF");

                int ufId;

                if (int.TryParse(ckUf.Attributes["ID_UF"], out ufId) && listaUfs.Contains(ufId))
                {
                    ckUf.Checked = true;
                }
            }
        }

        public List<Perfil> ObterPerfisSelecionados()
        {
            return
                ckblstPerfil.Items.Cast<ListItem>()
                    .Where(x => x.Selected)
                    .Select(x => new Perfil { ID = int.Parse(x.Value) })
                    .ToList();
        }

        public List<NivelOcupacional> ObterNiveisOcupacionaisSelecionados()
        {
            return
                ckblstNivelOcupacional.Items.Cast<ListItem>()
                    .Where(x => x.Selected)
                    .Select(x => new NivelOcupacional { ID = int.Parse(x.Value) })
                    .ToList();
        }

        public List<Uf> ObterUfsSelecionadas()
        {
            var ufs = new List<Uf>();

            for (var i = 0; i < rptUFs.Controls.Count; i++)
            {
                var ckUf = (CheckBox)rptUFs.Items[i].FindControl("ckUF");

                int ufId;

                if (ckUf.Checked && int.TryParse(ckUf.Attributes["ID_UF"], out ufId))
                    ufs.Add(new Uf {ID = ufId});
            }

            return ufs;
        }
    }
}