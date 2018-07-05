using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{

    public sealed class FormaAquisicaoMap : ClassMap<FormaAquisicao>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public FormaAquisicaoMap()
        {
            Table("TB_FORMAAQUISICAO");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_FORMAAQUISICAO");
            Map(x => x.Nome).Column("NM_FORMAAQUISICAO");
            Map(x => x.DataAlteracao).Column("DT_ULTIMAATUALIZACAO");
            Map(x => x.UsuarioAlteracao).Column("NM_USUARIOATUALIZACAO");
            HasMany(x => x.ListaSolucaoEducacional).KeyColumn("ID_FORMAAQUISICAO").AsBag().Inverse().Cascade.None();
            HasMany(x => x.ListaItemTrilha).KeyColumn("ID_FORMAAQUISICAO").AsBag().Inverse().Cascade.None();

            Map(x => x.Imagem).Column("OB_IMAGEM").Length(2147483647);
            Map(x => x.Descricao).Column("TX_Descricao");
            Map(x => x.TipoFormaDeAquisicao).Column("IN_TipoFormaDeAquisicao").CustomType<int>();
            Map(x => x.EnviarPortal).Column("IN_EnviarPortal");
            Map(x => x.Presencial).Column("IN_Presencial");
            Map(x => x.CargaHoraria).Column("QT_CargaHoraria").Nullable();
            References(x => x.Uf).Column("ID_UF").Cascade.SaveUpdate().Nullable();
            References(x => x.Responsavel).Column("ID_Responsavel");
            Map(x => x.PermiteAlterarCargaHoraria).Column("IN_PermiteAlterarCargaHoraria");

            //HasMany(x => x.ListaSolucaoEducacionalExtraCurricular).KeyColumn("ID_FormaAquisicao");
        }
    }
}
