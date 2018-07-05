using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class ViewConfiguracaoPagamentoPublicoAlvoMap : ClassMap<ViewConfiguracaoPagamentoPublicoAlvo>
    {
        public ViewConfiguracaoPagamentoPublicoAlvoMap()
        {
            Table("VW_ConfiguracaoPagamentoPublicoAlvo");
            Id(x => x.NumeroLinha, "NU_Linha").GeneratedBy.Assigned();
            References(x => x.ConfiguracaoPagamento, "ID_ConfiguracaoPagamento").Not.Nullable();
            References(x => x.Usuario, "ID_Usuario").Not.Nullable();
        }
    }
}
