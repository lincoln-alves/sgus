using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class ViewOfertaPermissaoMap: ClassMap<ViewOfertaPermissao>
    {
        public ViewOfertaPermissaoMap()
        {
            Table("VW_OfertaPermissao");
            Id(x => x.NumeroLinha, "NU_Linha").GeneratedBy.Assigned();
            References(x => x.Oferta, "ID_Oferta").Not.Nullable().Cascade.All();
            References(x => x.Usuario, "ID_Usuario").Not.Nullable().Cascade.All();
        }
    }
}
