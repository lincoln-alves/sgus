using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{

    public sealed class ProgramaMap : ClassMap<Programa>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public ProgramaMap()
        {
            Table("TB_PROGRAMA");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_PROGRAMA");
            Map(x => x.Nome, "NM_Programa").Nullable();
            Map(x => x.DataAlteracao, "DT_UltimaAtualizacao").Nullable();
            Map(x => x.UsuarioAlteracao, "NM_UsuarioAtualizacao").Nullable();
            Map(x => x.Ativo, "In_Ativo").Not.Nullable();
            Map(x => x.Apresentacao, "TX_Apresentacao").CustomSqlType("nvarchar(MAX)").Length(int.MaxValue).Nullable();
            Map(x => x.Sequencia, "SQ_Sequencia").Nullable();

            HasMany(x => x.ListaSolucaoEducacional).KeyColumn("ID_PROGRAMA").AsBag().Inverse()
                .LazyLoad().Cascade.AllDeleteOrphan();

            HasMany(x => x.ListaMatriculaPrograma).KeyColumn("ID_PROGRAMA").AsBag().Inverse()
                .LazyLoad().Cascade.AllDeleteOrphan();

            HasMany(x => x.ListaCapacitacao).KeyColumn("ID_PROGRAMA").AsBag().Inverse()
                .LazyLoad().Cascade.AllDeleteOrphan();

            HasMany(x => x.ListaPermissao).KeyColumn("ID_PROGRAMA").AsBag().Inverse()
                .LazyLoad().Cascade.AllDeleteOrphan();

            HasMany(x => x.ListaTag).KeyColumn("ID_PROGRAMA").AsBag().Inverse()
                .LazyLoad().Cascade.AllDeleteOrphan();

            HasMany(x => x.ListaUsuariosPermitidos).KeyColumn("ID_PROGRAMA").AsBag()
                .Inverse().LazyLoad().Cascade.None();

            HasMany(x => x.ListaAreasTematicas).KeyColumn("ID_PROGRAMA").NotFound.Ignore().AsBag().Inverse().Cascade.AllDeleteOrphan();

            Map(x => x.IdNodePortal).Column("ID_Node_Portal");
        }
    }
}


