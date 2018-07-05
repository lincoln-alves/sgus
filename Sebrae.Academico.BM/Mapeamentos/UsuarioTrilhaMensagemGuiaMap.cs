using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class UsuarioTrilhaMensagemGuiaMap : ClassMap<UsuarioTrilhaMensagemGuia>
    {
        public UsuarioTrilhaMensagemGuiaMap()
        {
            Table("TB_UsuarioTrilhaMensagemGuia");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_UsuarioTrilhaMensagemGuia");
            Map(x => x.Visualizacao, "DT_Visualizacao");
            References(x => x.UsuarioTrilha).Column("ID_UsuarioTrilha");
            References(x => x.MensagemGuia).Column("ID_MensagemGuia");
            References(x => x.LogLider).Column("ID_LogLider");
            References(x => x.ItemTrilha).Column("ID_ItemTrilha");
            References(x => x.Missao).Column("ID_Missao");
        }
    }
}