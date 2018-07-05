using System;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.BM.Classes;
using System.Collections.Generic;

namespace Sebrae.Academico.Util.Classes
{
    /// <summary>
    /// Classe de Fachada para Obter informações sobre Templates do sistema.
    /// </summary>
    public class TemplateUtil
    {
        /// <summary>
        /// Obtém informações de um template, de acordo com a enumeração informada.
        /// </summary>
        /// <param name="pTemplate">enumeração referente a um template do sistema, sobre o qual se deseja obter informações</param>
        /// <returns>Objeto da classe Template, com informações sobre templates do Sistema</returns>
        public static Template ObterInformacoes(enumTemplate pTemplate)
        {
            try
            {
                return new BMTemplate().ObterPorID((int)pTemplate);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static IList<Template> ObterTodasAsInformacoesDoTemplate()
        {
            try
            {
                return new BMTemplate().ObterTodos();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


    }
}
