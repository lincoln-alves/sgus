using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    /// <summary>
    /// Mapeamento da classe referente a Log de Acessos a uma trilha.
    /// <obs>Esta tabela não possui integridade referencial.</obs>
    /// </summary>
    public sealed class LogAcessoTrilhaMap : ClassMap<LogAcessoTrilha>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public LogAcessoTrilhaMap()
        {
            Table("LG_ACESSOTRILHA");
            LazyLoad();
            CompositeId().KeyProperty(x => x.IDUsuarioTrilha, "ID_UsuarioTrilha")
                         .KeyProperty(x => x.DataAcesso, "DT_Acesso");

        }

    }

}