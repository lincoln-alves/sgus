using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BM.Mapeamentos
{

    public sealed class UsuarioTrilhaMoedasMap : ClassMap<UsuarioTrilhaMoedas>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public UsuarioTrilhaMoedasMap()
        {
            Table("TB_UsuarioTrilhaMoedas");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_UsuarioTrilhaMoedas");

            References(x => x.UsuarioTrilha).Column("ID_UsuarioTrilha").Cascade.None();
            References(x => x.Curtida).Column("ID_ItemTrilhaCurtida").Cascade.None();
            References(x => x.ItemTrilha).Column("ID_ItemTrilha").Cascade.None();
            
            Map(x => x.MoedasDeOuro).Column("QT_MoedasOuro");
            Map(x => x.MoedasDePrata).Column("QT_MoedasPratas");

            Map(x => x.TipoCurtida).Column("IN_Acao").CustomType<enumTipoCurtida>();

            Map(x => x.DataCriacao).Column("DT_Criacao");
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
        }
    }
}
