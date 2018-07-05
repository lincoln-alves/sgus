using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    /// <summary>
    /// Mapeamento da classe referente a Log de Acessos.
    /// <obs>Esta tabela não possui integridade referencial.</obs>
    /// </summary>
    public sealed class LogBuscaSiteMap : ClassMap<LogBuscaSite>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public LogBuscaSiteMap()
        {
            Table("LG_BuscaSite");
            LazyLoad();
            CompositeId().KeyProperty(x => x.IDUsuario, "ID_Usuario")
                         .KeyProperty(x => x.DataEvento, "DT_Evento");
            Map(x => x.Busca).Column("TX_Busca");
        }

    }

}