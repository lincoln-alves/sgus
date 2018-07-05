using System;
using System.Collections.Generic;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.BM.Classes;

namespace Sebrae.Academico.Util.Classes
{
    /// <summary>
    /// Classe de Fachada para Obter informações sobre configurações do sistema.
    /// </summary>
    public class ConfiguracaoSistemaUtil
    {
        /// <summary>
        /// Obtém informações de uma configuração do Sistema de acordo com a enumeração informada.
        /// </summary>
        /// <param name="pConfiguracaoSistema">enumeração referente a configuração do sistema, sobre o qual se deseja obter informações</param>
        /// <param name="bmConfiguracaoSistema">Caso queira passar um objeto BMConfiguracaoSistema existente.</param>
        /// <returns>Objeto da classe ConfiguracaoSistema, com informações sobre configurações do Sistema</returns>
        public static ConfiguracaoSistema ObterInformacoes(enumConfiguracaoSistema pConfiguracaoSistema, BMConfiguracaoSistema bmConfiguracaoSistema = null)
        {
            try
            {
                return (bmConfiguracaoSistema ?? new BMConfiguracaoSistema()).ObterPorID((int) pConfiguracaoSistema);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static IList<ConfiguracaoSistema> ObterTodasAsInformacoesDoTemplate()
        {
            try
            {
                return new BMConfiguracaoSistema().ObterTodos();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }

}
