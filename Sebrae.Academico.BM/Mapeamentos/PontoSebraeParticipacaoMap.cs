using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class PontoSebraeParticipacaoMap : ClassMap<PontoSebraeParticipacao>
    {
        public PontoSebraeParticipacaoMap()
        {
            Table("TB_PontoSebraeParticipacao");

            Id(x => x.ID).Column("ID_PontoSebraeParticipacao");
            Map(x => x.PrimeiraParticipacao).Column("DT_PrimeiraParticipacao");
            Map(x => x.UltimaParticipacao).Column("DT_UltimaParticipacao");

            References(x => x.PontoSebrae).Column("ID_PontoSebrae");
            References(x => x.UsuarioTrilha).Column("ID_UsuarioTrilha");
        }
    }
}