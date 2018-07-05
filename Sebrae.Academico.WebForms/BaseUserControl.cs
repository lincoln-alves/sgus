using System.Collections.Generic;
using System.Web.UI;
using Sebrae.Academico.Dominio.Enumeracao;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace Sebrae.Academico.WebForms
{
    public abstract class BaseUserControl: UserControl
    {

        /// <summary>
        /// Lista de Permissões necessárias para acessar uma funcionalidade
        /// </summary>
        protected abstract IList<enumPerfil> PerfisNecessariosParaAcessarAFuncionalidade { get; }


        protected void AlterarStatusTab(LinkButton link, HtmlGenericControl controle)
        {
            if (string.IsNullOrWhiteSpace(link.Attributes["class"]))
            {
                //Esconder
                EsconderTab(link, controle);
            }
            else
            {
                //Exibir
                ExibirTab(link, controle);
            }
        }

        protected void ExibirTab(LinkButton link, HtmlGenericControl controle)
        {
            link.Attributes.Remove("class");
            controle.Attributes.Remove("class");
            controle.Attributes.Add("class", "panel-collapse in");
        }

        protected void EsconderTab(LinkButton link, HtmlGenericControl controle)
        {
            link.Attributes.Add("class", "collapsed");
            controle.Attributes.Remove("class");
            controle.Attributes.Add("class", "panel-collapse collapse");
        }


    }
}