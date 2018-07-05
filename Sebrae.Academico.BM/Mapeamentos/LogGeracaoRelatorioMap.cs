using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    /// <summary>
    /// Mapeamento da classe referente a Log de Geração de Relatórios.
    /// <obs>Esta tabela não possui integridade referencial.</obs>
    /// </summary>
    public sealed class LogGeracaoRelatorioMap : ClassMap<LogGeracaoRelatorio>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public LogGeracaoRelatorioMap()
        {

            Table("LG_GeracaoRelatorio");
            LazyLoad();

            #region "Mapeamento de Chave composta"

            CompositeId().KeyReference(x => x.Usuario, "ID_Usuario")
                         .KeyReference(x => x.Relatorio, "ID_Relatorio")
                         .KeyProperty(x => x.DTGeracao, "DT_Geracao");

            #endregion

        }

    }

}