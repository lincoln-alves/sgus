using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class ItemQuestionarioParticipacaoMap : ClassMap<ItemQuestionarioParticipacao>
    {

        /// <summary>
        /// Construtor.
        /// </summary>
        public ItemQuestionarioParticipacaoMap()
        {
            Table("TB_ItemQuestionarioParticipacao");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_ItemQuestionarioParticipacao");
            References(x => x.QuestionarioParticipacao).Column("ID_QuestionarioParticipacao"); //.Cascade.All(); //.Not.Nullable();
            References(x => x.TipoItemQuestionario).Column("ID_TipoItemQuestionario");
            References(x => x.EstiloItemQuestionario).Column("ID_EstiloItemQuestionario");
            Map(x => x.Questao).Column("NM_Questao");
            Map(x => x.ValorQuestao).Column("VL_Questao");
            Map(x => x.Gabarito).Column("NM_Gabarito");
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.Resposta, "NM_Resposta");
            Map(x => x.ValorAtribuido, "VL_Atribuido");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
            HasMany(x => x.ListaOpcoesParticipacao)
                .KeyColumn("ID_ItemQuestionarioParticipacao").AsBag()
                   .Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            Map(x => x.InAvaliaProfessor).Column("IN_AvaliaProfessor");
            Map(x => x.Comentario).Column("TX_Comentario");
            Map(x => x.Feedback).Column("TX_Feedback");
            Map(x => x.Ordem).Column("VL_Ordem");
            Map(x => x.ExibeFeedback).Column("IN_ExibeFeedback");
            Map(x => x.RespostaObrigatoria).Column("IN_RespostaObrigatoria");
        }
    }
}