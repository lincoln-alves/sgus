using System;
using Sebrae.Academico.BM.Classes;
using System.Collections.Generic;
using Sebrae.Academico.Dominio.Classes;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.Cadastros.Protocolo
{
    public partial class ListarProtocolo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            // TODO: Alterar camada para usar apenas o BP
            var bm = new BMProtocolo();
            var protocolos = new List<Sebrae.Academico.Dominio.Classes.Protocolo>();
            var numero = -1;
            if (string.IsNullOrEmpty(txtNumero.Text))
                protocolos = bm.ObterTodos().ToList();
            else if (!int.TryParse(txtNumero.Text,out numero)) {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Número de protocolo inválido. Informe somente números para o protocolo.");}
            else{
                var protocolo = bm.ObterPorNumero(numero);
                if(protocolo != null) protocolos.Add(protocolo);
            }

            grdProtocolos.DataSource = protocolos;
            grdProtocolos.DataBind();
        }

        protected void grdProtocolos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "editar")
            {
                int id;
                if (int.TryParse(e.CommandArgument.ToString(), out id))
                    Response.Redirect("EdicaoProtocolo.aspx?Id=" + id.ToString(), false);
                else
                    throw new AcademicoException("Não foi possível visualizar o protocolo.");
            }
        }
    }
}