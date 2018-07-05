using System;
using System.Web.UI;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.WebForms.UserControls
{
    public partial class ucNacionalizarRelatorio : UserControl
    {
        public ucNacionalizarRelatorio()
        {
            AutoPostBack = false;
        }

        public delegate void OnNacionalizarRelatorio(object sender, NacionalizarRelatorioEventArgs e);
        public event OnNacionalizarRelatorio NacionalizouRelatorio;


        public bool IsNacionalizado
        {
            get
            {
                return ckbNacionalizacao.Checked;
            }
            set
            {
                ckbNacionalizacao.Checked = value;
            }
        }


        public bool AutoPostBack { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (NacionalizouRelatorio != null)
                AutoPostBack = true;

            ckbNacionalizacao.AutoPostBack = AutoPostBack;

            ckbNacionalizacao.InputAttributes.Clear();

            if (AutoPostBack)
            {
                ckbNacionalizacao.InputAttributes.Add("class", "mostrarload");
            }
            

            if (!Page.IsPostBack && new ManterUsuario().PerfilAdministrador())
            {
                divNacionalizarRelatorio.Visible = false;
            }
        }

        protected void ckbNacionalizacao_OnCheckedChanged(object sender, EventArgs e)
        {
            if (NacionalizouRelatorio == null) return;

            var ufUsuario = new ManterUsuario().ObterUsuarioLogado().UF;

            NacionalizouRelatorio(this, new NacionalizarRelatorioEventArgs(ufUsuario, IsNacionalizado));
        }
    }

    public class NacionalizarRelatorioEventArgs : EventArgs
    {
        public Uf UfSelecionada { get; set; }

        public bool IsNacionalizado { get; set; }

        public NacionalizarRelatorioEventArgs(Uf ufSelecionada, bool isNacionalizado)
        {
            UfSelecionada = ufSelecionada;
            IsNacionalizado = isNacionalizado;
        }
    }
}