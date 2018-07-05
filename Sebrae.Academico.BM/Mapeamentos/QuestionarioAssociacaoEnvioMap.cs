using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class QuestionarioAssociacaoEnvioMap : ClassMap<QuestionarioAssociacaoEnvio>
    {
        public QuestionarioAssociacaoEnvioMap()
        {

            Table("TB_QuestionarioAssociacaoEnvio");
            //LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_QuestionarioAssociacaoEnvio");
            References(x => x.QuestionarioAssociacao).Column("ID_QuestionarioAssociacao").Cascade.None(); //.Not.LazyLoad();
            References(x => x.Usuario).Column("ID_Usuario").Cascade.None();
            Map(x => x.DataEnvio).Column("DT_Envio").CustomSqlType("DATETIME");
            Map(x => x.Ativo).Column("IN_Ativo");
        }

    }

}