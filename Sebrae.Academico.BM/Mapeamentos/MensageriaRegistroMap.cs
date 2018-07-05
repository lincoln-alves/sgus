using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class MensageriaRegistroMap : ClassMap<MensageriaRegistro>
    {
        public MensageriaRegistroMap()
        {
            Table("LG_MensageriaRegistro");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_MensageriaRegistro");
            References(x => x.MensageriaParametro, "ID_MensageriaParametro").Not.Nullable().Cascade.None();
            References(x => x.MatriculaTurma, "ID_MatriculaTurma").Cascade.None();
            References(x => x.UsuarioTrilha, "ID_UsuarioTrilha").Cascade.None().NotFound.Ignore();
            Map(x => x.DataEnvio).Column("DT_Envio").Not.Nullable();
            Map(x => x.TextoEnviado).Column("TX_Envio").Not.Nullable();
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
        }
    }
}
