using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class MissaoMap : ClassMap<Missao>
    {
        public MissaoMap()
        {
            Table("TB_Missao");

            Id(x => x.ID).Column("ID_Missao");
            Map(x => x.Nome).Column("DE_Objetivo");

            References(x => x.PontoSebrae).Column("ID_PontoSebrae");
            HasMany(x => x.ListaItemTrilha).KeyColumn("ID_Missao");
        }
    }
}