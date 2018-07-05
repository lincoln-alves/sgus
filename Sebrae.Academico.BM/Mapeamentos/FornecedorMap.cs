using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BM.Mapeamentos
{

    public sealed class FornecedorMap : ClassMap<Fornecedor>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public FornecedorMap()
        {
            Table("TB_FORNECEDOR");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_FORNECEDOR");
            Map(x => x.Nome).Column("NM_FORNECEDOR");
            Map(x => x.Login).Column("NM_LOGIN");
            Map(x => x.Senha).Column("NM_SENHA");
            Map(x => x.DataUltimoAcesso).Column("DT_ULTIMOACESSO");
            Map(x => x.QuantidadeAcessos).Column("QT_ACESSOS");
            Map(x => x.DataAlteracao).Column("DT_ULTIMAATUALIZACAO");
            Map(x => x.UsuarioAlteracao).Column("NM_USUARIOATUALIZACAO");
            Map(x => x.TextoCriptografia).Column("TX_CRIPTOGRAFIA");
            Map(x => x.NomeApresentacao).Column("NM_ApresentacaoFornecedor");
            HasMany(x => x.ListaSolucaoEducacional).KeyColumn("ID_FORNECEDOR").AsBag().Inverse().Cascade.None();
            HasMany(x => x.ListaLogAcesoFornecedor).KeyColumn("ID_FORNECEDOR").AsBag().Inverse().LazyLoad().Cascade.None();
            HasMany(x => x.ListaFornecedorUF).KeyColumn("ID_Fornecedor").Cascade.AllDeleteOrphan();
            Map(x => x.LinkAcesso).Column("LK_Acesso");
            Map(x => x.PermiteGestaoSGUS).Column("IN_PermiteGestaoSGUS");
            Map(x => x.PermiteCriarOferta).Column("IN_PermiteCriarOferta");
            Map(x => x.PermiteCriarTurma).Column("IN_PermiteCriarTurma");
            Map(x => x.ApresentarComoFornecedorNoPortal).Column("IN_ApresentarComoFornecedorNoPortal");
            Map(x => x.FornecedorSistema).Column("ID_FornecedorSistema").CustomType(typeof(enumFornecedor)); ;
        }
    }
}
