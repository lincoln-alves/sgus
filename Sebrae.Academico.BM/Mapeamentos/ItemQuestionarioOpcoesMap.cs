using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class ItemQuestionarioOpcoesMap : ClassMap<ItemQuestionarioOpcoes>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public ItemQuestionarioOpcoesMap()
        {
            Table("TB_ItemQuestionarioOpcoes");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_ItemQuestionarioOpcoes");
            References(x => x.ItemQuestionario).Cascade.All().Column("ID_ItemQuestionario");
            Map(x => x.Nome).Column("NM_Opcao").Not.Nullable();
            Map(x => x.RespostaCorreta).Column("IN_RespostaCorreta").Not.Nullable();
            Map(x => x.DataAlteracao).Column("DT_ULTIMAATUALIZACAO");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
            Map(x => x.TipoDiagnostico, "IN_TipoDiagnostico").CustomType<enumTipoDiagnostico>();
            Map(x => x.OpcaoInt).Column("NM_OpcaoInt");

            References(x => x.OpcaoVinculada).Cascade.All().Column("ID_OpcaoVinculada");
        }
    }
}