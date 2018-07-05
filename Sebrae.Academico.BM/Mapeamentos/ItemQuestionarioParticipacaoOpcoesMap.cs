using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class ItemQuestionarioParticipacaoOpcoesMap: ClassMap<ItemQuestionarioParticipacaoOpcoes>
    {
        public ItemQuestionarioParticipacaoOpcoesMap()
        {
            Table("TB_ItemQuestionarioParticipacaoOpcoes");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_ItemQuestionarioParticipacaoOpcoes");
            References(x => x.ItemQuestionarioParticipacao).Column("ID_ItemQuestionarioParticipacao").Cascade.All();
            Map(x => x.Nome).Column("NM_Opcao");
            Map(x => x.RespostaSelecionada).Column("IN_RespostaSelecionada");
            Map(x => x.RespostaCorreta).Column("IN_RespostaCorreta");
            Map(x => x.TipoDiagnostico, "IN_TipoDiagnostico").CustomType<enumTipoDiagnostico>();
            Map(x => x.OpcaoInt).Column("NM_OpcaoInt");

            References(x => x.OpcaoVinculada).Column("ID_OpcaoVinculada");
            References(x => x.OpcaoSelecionada).Column("ID_OpcaoSelecionada");
        }
    }
}
