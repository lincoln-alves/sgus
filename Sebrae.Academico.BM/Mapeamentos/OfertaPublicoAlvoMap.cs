using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class OfertaPublicoAlvoMap : ClassMap<OfertaPublicoAlvo>
    {
        public OfertaPublicoAlvoMap()
        {
            Table("TB_OfertaPublicoAlvo");
            LazyLoad();
            Id(x => x.ID, "ID_OfertaPublicoAlvo").GeneratedBy.Identity();
            Map(x => x.DataAlteracao, "DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao, "NM_UsuarioAtualizacao");
            References(x => x.PublicoAlvo).Column("ID_PublicoAlvo");
            References(x => x.Oferta).Column("ID_Oferta");
        }
    }
}
