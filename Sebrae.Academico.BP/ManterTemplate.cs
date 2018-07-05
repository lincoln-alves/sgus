using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System.Collections.Generic;
using System.Linq;

namespace Sebrae.Academico.BP
{
    public class ManterTemplate : BusinessProcessBase
    {

        #region "Atributos Privados"

        private BMTemplate bmTemplate = null;

        #endregion

        #region "Construtor"

        public ManterTemplate()
            : base()
        {
            bmTemplate = new BMTemplate();
        }

        #endregion

        #region "Métodos Públicos"

        public void IncluirTemplate(Template pTemplate)
        {
            bmTemplate.Salvar(pTemplate);
        }

        public void IncluirTemplate(IList<Template> pTemplate)
        {
            pTemplate.ToList().ForEach(x => PreencherInformacoesDeAuditoria(x));
            bmTemplate.Salvar(pTemplate);
        }

        public Template ObterTemplatePorID(int pId)
        {
            return bmTemplate.ObterPorID(pId);
        }

        public IList<Template> ObterTodosTemplates()
        {
            try
            {
                return bmTemplate.ObterTodos();
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
