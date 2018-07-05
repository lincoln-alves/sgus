using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class ViewSistemaExternoPermissaoMap : ClassMap<ViewSistemaExternoPermissao>
    {
        public ViewSistemaExternoPermissaoMap()
        {
            Table("VW_SistemaExternoPermissao");
            Id(x => x.NumeroLinha, "NU_Linha").GeneratedBy.Assigned();
            References(x => x.SistemaExterno, "ID_SistemaExterno").Not.Nullable().Cascade.All();
            References(x => x.Usuario, "ID_Usuario").Not.Nullable().Cascade.All();
        }
    }
}
