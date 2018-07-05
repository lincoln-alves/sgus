using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class ValorSistemaMap : ClassMap<ValorSistema>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public ValorSistemaMap()
        {
            Table("TB_ValorSistema");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_ValorSistema");
            Map(x => x.Registro).Column("VL_Registro");
            Map(x => x.Descricao).Column("DE_Registro");
            Map(x => x.DataAlteracao).Column("DT_ULTIMAATUALIZACAO");
            Map(x => x.UsuarioAlteracao).Column("NM_USUARIOATUALIZACAO");
        }

    }
}