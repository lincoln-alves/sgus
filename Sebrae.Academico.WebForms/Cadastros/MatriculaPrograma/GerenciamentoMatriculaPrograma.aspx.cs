using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.WebForms.UserControls;
using classes = Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Util.Classes;
using System.Web.UI.HtmlControls;

namespace Sebrae.Academico.WebForms.Cadastros.MatriculaPrograma
{
    public partial class GerenciamentoMatriculaPrograma : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Server.Transfer("~/Cadastros/Matricula/GerenciamentoMatricula.aspx");
            //try
            //{
            //    if (!Page.IsPostBack)
            //    {
            //        base.LogarAcessoFuncionalidade();
            //    }
            //}
            //catch (AcademicoException ex)
            //{
            //    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            //}
        }
    }
}
