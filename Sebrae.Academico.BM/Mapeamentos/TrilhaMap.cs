using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{

    public sealed class TrilhaMap : ClassMap<Trilha>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public TrilhaMap()
        {
            Table("TB_TRILHA");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_TRILHA");
            References(x => x.CategoriaConteudo).Column("ID_CATEGORIACONTEUDO");
            Map(x => x.Nome).Column("NM_TRILHA");
            Map(x => x.NomeEstendido).Column("NM_TrilhaEstendido");
            Map(x => x.Descricao).Column("TX_DESCRICAO");
            Map(x => x.IdNode).Column("ID_Node");
            Map(x => x.DataAlteracao).Column("DT_ULTIMAATUALIZACAO");
            Map(x => x.UsuarioAlteracao).Column("NM_USUARIOATUALIZACAO");
            Map(x => x.EmailTutor).Column("NM_EmailTutor");
            Map(x => x.ID_CodigoMoodle).Column("ID_CodigoMoodle");
            Map(x => x.IdNodePortal).Column("ID_Node_Portal");
            Map(x => x.Credito).Column("TX_Credito").CustomType("StringClob").CustomSqlType("nvarchar(max)");
            HasMany(x => x.ListaTrilhaNivel).KeyColumn("ID_TRILHA").AsBag().Inverse()
               .Cascade.AllDeleteOrphan();

            HasMany(x => x.ListaPermissao).KeyColumn("ID_Trilha").AsBag().Inverse()
               .Cascade.AllDeleteOrphan();

            HasMany(x => x.ListaTag).KeyColumn("ID_TRILHA").AsBag().Inverse() //.BatchSize(250)
               .Cascade.AllDeleteOrphan();

            HasMany(x => x.ListaCategoriaConteudo).KeyColumn("ID_TRILHA").AsBag().Inverse()
                .Cascade.AllDeleteOrphan();

            HasMany(x => x.ListaAreasTematicas).KeyColumn("ID_Trilha").NotFound.Ignore().AsBag().Inverse().Cascade.AllDeleteOrphan();
        }

    }
}
