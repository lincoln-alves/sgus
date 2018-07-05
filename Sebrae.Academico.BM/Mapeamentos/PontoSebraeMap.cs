using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class PontoSebraeMap : ClassMap<PontoSebrae>
    {
        public PontoSebraeMap()
        {
            Table("TB_PontoSebrae");

            Id(x => x.ID).Column("ID_PontoSebrae");
            Map(x => x.Nome).Column("NM_Nome");
            Map(x => x.NomeExibicao).Column("NM_Exibicao");
            Map(x => x.QtMinimaPontos).Column("QT_MinimoPontos");
            Map(x => x.Ativo).Column("IN_Ativo");

            References(x => x.TrilhaNivel).Column("ID_TrilhaNivel");
            HasMany(x => x.ListaMissoes).KeyColumn("ID_PontoSebrae");
            HasMany(x => x.ListaPontoSebraeParticipacao).KeyColumn("ID_PontoSebrae");
        }
    }
}