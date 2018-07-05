using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class ObjetivoMap : ClassMap<Objetivo>
    {
        public ObjetivoMap()
        {
            Table("TB_Objetivo");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_Objetivo");
            Map(x => x.Nome).Column("DE_Objetivo").Not.Nullable().Length(500);
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao").Length(100);
            Map(x => x.ChaveExterna).Column("ID_ChaveExterna").Length(50);            

            HasMany(x => x.ListaItemTrilha).KeyColumn("ID_Objetivo");
        }

    }

}