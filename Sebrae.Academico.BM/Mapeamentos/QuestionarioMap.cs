using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class QuestionarioMap : ClassMap<Questionario>
    {
        public QuestionarioMap()
        {
            Table("TB_Questionario");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_Questionario");
            References(x => x.TipoQuestionario).LazyLoad().Column("ID_TipoQuestionario");
            References(x => x.Uf).Column("ID_UF");
            Map(x => x.Nome).Column("NM_Questionario");
            Map(x => x.PrazoMinutos).Column("QT_PrazoMinutos");
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
            Map(x => x.QtdQuestoesProva).Column("QT_QuestoesProva");
            Map(x => x.TextoEnunciado).Column("TX_ENUNCIADOPRE");
            Map(x => x.NotaMinima).Column("VL_NotaMinima");
            Map(x => x.Ativo).Column("IN_Ativo");

            HasMany(x => x.ListaItemQuestionario).KeyColumn("ID_Questionario").AsBag().Inverse()
                .Cascade.AllDeleteOrphan();

            HasMany(x => x.ListaQuestionarioAssociacao).KeyColumn("ID_Questionario").AsBag().Inverse()
               .Cascade.AllDeleteOrphan().Not.LazyLoad();

            HasMany(x => x.ListaQuestionarioParticipacao).KeyColumn("ID_Questionario").AsBag().Inverse()
                .Cascade.All();

            HasMany(x => x.ListaQuestionarioPermissao).KeyColumn("ID_Questionario").AsBag().Inverse()
                .Cascade.AllDeleteOrphan();

            HasMany(x => x.ListaCampos).KeyColumn("ID_Questionario").AsBag().Inverse();

            HasManyToMany(a => a.ListaCategoriaConteudo)
                .Table("TB_CategoriaConteudoQuestionario")
                .ParentKeyColumn("ID_Questionario")
                .ChildKeyColumn("ID_CategoriaConteudo");

            HasMany(x => x.ListaItemTrilha).KeyColumn("ID_ItemTrilha").AsBag().Inverse();
        }
    }
}