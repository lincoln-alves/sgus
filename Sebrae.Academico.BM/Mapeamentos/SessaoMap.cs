using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class SessaoMap : ClassMap<Sessao>
    {
        public SessaoMap()
        {
            Table("TP_Sessao");
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_Sessao");
            Map(x => x.Valor).Column("VL_Valor").CustomSqlType("nvarchar(MAX)").Length(int.MaxValue);
            Map(x => x.Hash).Column("VL_Hash");
        }
    }
}
