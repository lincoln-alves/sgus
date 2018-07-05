using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class QuestionarioAssociacaoMap : ClassMap<QuestionarioAssociacao>
    {
        public QuestionarioAssociacaoMap()
        {

            Table("TB_QuestionarioAssociacao");
            //LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_QuestionarioAssociacao");
            References(x => x.Questionario).Column("ID_Questionario").Cascade.None(); //.Not.LazyLoad();
            References(x => x.TrilhaNivel).Column("ID_TrilhaNivel").Cascade.None();
            References(x => x.Turma).Column("ID_Turma").Cascade.None();
            References(x => x.TurmaCapacitacao).Column("ID_TurmaCapacitacao").Cascade.None();
            References(x => x.TipoQuestionarioAssociacao).Column("ID_TipoQuestionarioAssociacao").Cascade.None(); //.LazyLoad();
            Map(x => x.Obrigatorio).Column("IN_Obrigatorio").Not.Nullable();
            Map(x => x.Evolutivo).Column("IN_EVOLUTIVO").UniqueKey("UK_QuestionaAssociacao").Not.Nullable();
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
            Map(x => x.DataDisparoLinkPesquisa).Column("DT_DisparoLinkPesquisa").CustomSqlType("DATETIME");
            Map(x => x.DataDisparoLinkEficacia).Column("DT_DisparoLinkEficacia").CustomSqlType("DATETIME");

        }

    }

}