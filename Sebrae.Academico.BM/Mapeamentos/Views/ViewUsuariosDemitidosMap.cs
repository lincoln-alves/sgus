using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes.Views;

namespace Sebrae.Academico.BM.Mapeamentos.Views
{
    public class ViewUsuariosDemitidosMap : ClassMap<ViewUsuariosDemitidos>
    {
        public ViewUsuariosDemitidosMap()
        {
            Table("VW_UsuariosDemitidos");
            Id(x => x.Id).Column("NU_Linha");
            Map(x => x.Nome).Column("Nome");
            Map(x => x.CPF).Column("CPF");
            Map(x => x.Matricula).Column("Matricula");
            Map(x => x.DataDemissao).Column("DataDemissao");
        }
    }
}
