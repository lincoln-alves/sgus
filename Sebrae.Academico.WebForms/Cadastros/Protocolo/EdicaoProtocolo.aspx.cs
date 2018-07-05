using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System;
using System.Collections.Generic;

namespace Sebrae.Academico.WebForms.Cadastros.Protocolo
{
    public partial class EdicaoProtocolo : PageBase
    {
        protected override enumFuncionalidade Funcionalidade
        {
            get
            {
                return enumFuncionalidade.Protocolo;
            }
        }

        protected override IList<enumPerfil> PerfisNecessariosParaAcessarAFuncionalidade
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    base.LogarAcessoFuncionalidade();

                    int id;
                    if (int.TryParse(Request["Id"], out id))
                    {
                        PreencherCampos(id);
                    }
                }
                catch (Exception ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
            }
        }

        private void PreencherCampos(int id)
        {
            var protocolo = new BMProtocolo().ObterPorId(id);

            txtNumero.Text = protocolo.Numero.ToString();
            txtRemetente.Text = protocolo.Remetente.Nome;
            txtDestinatario.Text = protocolo.Destinatario.Nome;
            txtDataDeEnvio.Text = protocolo.DataEnvio.Date.ToShortDateString();
            txtDataDeRecebimento.Text = protocolo.DataRecebimento != null ? protocolo.DataRecebimento.Value.Date.ToShortDateString() : "";
            txtAssinadoPor.Text = protocolo.UsuarioAssinatura != null ? protocolo.UsuarioAssinatura.Nome : "";
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListarProtocolo.aspx", false);
        }
    }
}