using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.BP;
using Sebrae.Academico.Util.Classes;
using classes = Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.WebForms.Cadastros.HierarquiaAuxiliar
{
    public partial class EdicaoHierarquiaAuxiliar : System.Web.UI.Page
    {
        private ManterHierarquiaAuxiliar manterHierarquia = new ManterHierarquiaAuxiliar();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                WebFormHelper.PreencherLista(new BMHierarquia().ObterDiretorias().OrderBy(x => x.Unidade).Select(x => new {Nome = x.Unidade, ID = x.CodUnidade}).ToList(), ddlCodUnidade, false, true);                
                if (Request["Id"] != null)
                {
                    var idHierarquia = int.Parse(Request["Id"]);
                    var hierarquiaAux = manterHierarquia.ObterPorId(idHierarquia);
                    PreencherCampos(hierarquiaAux);
                }
            }
        }

        private void PreencherCampos(classes.HierarquiaAuxiliar hierarquiaAux)
        {
            if (hierarquiaAux != null)
            {

                if (hierarquiaAux.CodUnidade != null) {
                    WebFormHelper.SetarValorNaCombo(hierarquiaAux.CodUnidade.ToString(), ddlCodUnidade);
                }

                if (hierarquiaAux.Usuario != null) { 
                    ucLupaUsuario.SelectedUser = hierarquiaAux.Usuario;
                }

            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                classes.HierarquiaAuxiliar hierarquiaAuxiliar = ObterObjetoHierarquiaAuxiliar();
                manterHierarquia.AlterarHierarquiaAuxiliar(hierarquiaAuxiliar);
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados Gravados com Sucesso !", "ListarHierarquiaAuxiliar.aspx");
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                return;
            }
        }

        private classes.HierarquiaAuxiliar ObterObjetoHierarquiaAuxiliar()
        {
            var retorno = new classes.HierarquiaAuxiliar();

            if (Request["Id"] != null)
            {
                int idCapacitacao = int.Parse(Request["Id"].ToString());
                retorno = manterHierarquia.ObterPorId(idCapacitacao);
            }

            if (ddlCodUnidade.SelectedIndex > 0)
                retorno.CodUnidade = ddlCodUnidade.SelectedValue;
            else
                throw new AcademicoException("Você deve informar o programa da Oferta");

            if (ucLupaUsuario.SelectedUser != null)
            {
                retorno.Usuario = ucLupaUsuario.SelectedUser;
            }
            

            return retorno;
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListarHierarquiaAuxiliar.aspx");
        }
    }
}