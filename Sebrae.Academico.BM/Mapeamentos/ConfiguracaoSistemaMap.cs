using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class ConfiguracaoSistemaMap : ClassMap<ConfiguracaoSistema>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public ConfiguracaoSistemaMap()
        {
            Table("TB_ConfiguracaoSistema");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Assigned().Column("ID_ConfiguracaoSistema");
            Map(x => x.Registro).Column("VL_Registro");
            Map(x => x.Descricao).Column("DE_Registro");
            Map(x => x.DataAlteracao).Column("DT_ULTIMAATUALIZACAO");
            Map(x => x.UsuarioAlteracao).Column("NM_USUARIOATUALIZACAO");
        }

    }
}