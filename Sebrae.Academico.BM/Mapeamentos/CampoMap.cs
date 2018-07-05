using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class CampoMap : ClassMap<Campo>
    {
        public CampoMap()
        {
            Table("TB_Campo");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_Campo");
            References(d => d.Etapa).Column("ID_Etapa");
            References(d => d.Questionario).Column("ID_Questionario");
            //References(x => x.Etapa).Column("ID_Etapa");
            //References(x => x.Resposta).Column("ID_Campo");
            //Map(x => x.Label).Column("NM_Label");
            Map(x => x.Nome).Column("NM_Campo");
            Map(x => x.Tamanho).Column("VL_Tamanho");
            Map(x => x.Ordem).Column("VL_Ordem");
            Map(x => x.TipoDado).Column("VL_TipoDado");
            Map(x => x.TipoCampo).Column("VL_Tipo");
            Map(x => x.PermiteNulo).Column("IN_PermitirNulo");
            Map(x => x.SomenteLetra).Column("IN_SomenteLetras");
            Map(x => x.SomenteNumero).Column("IN_SomenteNumero");
            Map(x => x.Largura).Column("VL_Largura");
            Map(x => x.ExibirImpressao).Column("IN_ExibirImpressao");
            Map(x => x.ExibirAjudaImpressao).Column("IN_ExibirAjudaImpressao");
            Map(x => x.CampoDivisor).Column("IN_CampoBaseDivisao");

            // Usado como ajuda e como tipo de campo conteúdo HTML, por isso é um text no banco
            Map(x => x.Ajuda).Column("TX_Ajuda");

            References(x => x.OrcamentoReembolso).Column("ID_OrcamentoReembolso");

            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
            HasMany(x => x.ListaAlternativas).KeyColumn("ID_Campo").AsBag().Inverse().Cascade.All();

            HasMany(x => x.ListaMetaValues).KeyColumn("ID_Campo").AsBag().Inverse().Cascade.All();
            HasManyToMany(x => x.ListaCamposVinculados).Table("TB_CampoSomatorio").ParentKeyColumn("ID_Campo").ChildKeyColumn("ID_CampoParaSomatorio");
            HasMany(x => x.ListaCampoPorcentagem).KeyColumn("ID_Campo").NotFound.Ignore().AsBag().Inverse().Cascade.AllDeleteOrphan();
        }
    }
}
