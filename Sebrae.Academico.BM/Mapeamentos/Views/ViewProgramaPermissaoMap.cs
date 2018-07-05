using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class ViewProgramaPermissaoMap : ClassMap<ViewProgramaPermissao>
    {
        public ViewProgramaPermissaoMap()
        {
            Table("VW_ProgramaPermissao");
            Id(x => x.NumeroLinha, "NU_Linha").GeneratedBy.Assigned();
            References(x => x.Programa, "ID_Programa").Not.Nullable().Cascade.All();
            References(x => x.Usuario,"ID_Usuario").Not.Nullable().Cascade.All();
        }
    }
}
