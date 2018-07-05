using FluentNHibernate.Mapping;
using FluentNHibernate.Testing.Values;
using NHibernate;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class TermoAceiteMap : ClassMap<TermoAceite>
    {
        public TermoAceiteMap()
        {
            Table("TB_TermoAceite");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_TermoAceite");
            Map(x => x.Nome).Column("NM_TermoAceite");
            Map(x => x.Texto).Column("TX_TermoAceite").CustomSqlType("nvarchar(max)").Length(int.MaxValue);
            Map(x => x.PoliticaConsequencia).Column("TX_PoliticaConsequencia").CustomSqlType("nvarchar(max)").Length(int.MaxValue);
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");

            References(x => x.Usuario).Column("ID_Usuario");
            References(x => x.Uf).Column("ID_UF");

            HasMany(x => x.ListaCategoriaConteudo).KeyColumn("ID_TermoAceite").AsBag().Inverse()
                .Cascade.AllDeleteOrphan();
        }
    }
}
