using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class ViewSolucaoEducacionalPermissaoMap : ClassMap<ViewSolucaoEducacionalPermissao>
    {
        public ViewSolucaoEducacionalPermissaoMap()
        {
            Table("VW_SolucaoEducacionalPermissao");
            Id(x => x.NumeroLinha, "NU_Linha").GeneratedBy.Assigned();
            References(x => x.SolucaoEducacional, "ID_SolucaoEducacional").Not.Nullable().Cascade.All();
            References(x => x.Usuario, "ID_Usuario").Not.Nullable().Cascade.All();
        }
    }
}
