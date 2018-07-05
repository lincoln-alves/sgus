using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class RegiaoMap : ClassMap<Regiao>
    {
        public RegiaoMap()
        {
            Table("TB_Regiao");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Assigned().Column("ID_Regiao");
            Map(x => x.Nome).Column("NM_Regiao").Not.Nullable().Length(50);
            Map(x => x.SiglaRegiao).Column("SG_Regiao").Not.Nullable().Length(2);
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao").Length(100);
            HasMany(x => x.ListaUf).KeyColumn("ID_Regiao");
        }

    }

}