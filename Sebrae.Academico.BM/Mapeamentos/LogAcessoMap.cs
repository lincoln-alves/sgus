using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    /// <summary>
    /// Mapeamento da classe referente a Log de Acessos.
    /// <obs>Esta tabela não possui integridade referencial.</obs>
    /// </summary>
    public sealed class LogAcessoMap : ClassMap<LogAcesso>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public LogAcessoMap()
        {
            Table("LG_Acesso");
            LazyLoad();
            CompositeId().KeyProperty(x => x.IDUsuario, "ID_Usuario")
                         .KeyProperty(x => x.DataAcesso, "DT_Acesso");
            Map(x => x.INSGUS).Column("IN_SGUS");
            Map(x => x.IP).Column("TX_IP");
        }

    }

}