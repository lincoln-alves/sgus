using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class ItemQuestionarioMap : ClassMap<ItemQuestionario>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public ItemQuestionarioMap()
        {
            Table("TB_ItemQuestionario");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_ItemQuestionario");
            References(x => x.Questionario).Column("ID_Questionario").Cascade.None().LazyLoad();
            References(x => x.TipoItemQuestionario).Cascade.None().Column("ID_TipoItemQuestionario").LazyLoad();
            References(x => x.EstiloItemQuestionario).Cascade.None().Column("ID_EstiloItemQuestionario").Nullable().LazyLoad();
            References(x => x.QuestionarioEnunciado).Cascade.None().Column("ID_QUESTIONARIOENUNCIADO").Nullable().LazyLoad();
            Map(x => x.Questao).Column("NM_Questao").Not.Nullable();
            Map(x => x.ValorQuestao).Column("VL_Questao");
            Map(x => x.NomeGabarito).Column("NM_Gabarito");
            Map(x => x.DataAlteracao).Column("DT_ULTIMAATUALIZACAO");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
            //HasMany(x => x.ListaItemQuestionarioOpcoes).AsBag().Inverse().Cascade.AllDeleteOrphan().KeyColumn("ID_ItemQuestionario").LazyLoad();
            HasMany(x => x.ListaItemQuestionarioOpcoes).KeyColumn("ID_ItemQuestionario").AsBag().Inverse()
                .Not.LazyLoad().Cascade.AllDeleteOrphan();
            Map(x => x.InAvaliaProfessor).Column("IN_AvaliaProfessor");
            Map(x => x.Feedback).Column("TX_Feedback");
            Map(x => x.Comentario).Column("TX_COMENTARIO");
            Map(x => x.Ordem).Column("VL_Ordem");
            Map(x => x.ExibeFeedback).Column("IN_ExibeFeedback");
            Map(x => x.RespostaObrigatoria).Column("IN_RespostaObrigatoria");
        }
    }
}