using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class EtapaMap : ClassMap<Etapa>
    {
        public EtapaMap()
        {
            Table("TB_Etapa");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_Etapa");

            References(x => x.Processo, "ID_Processo");
            References(x => x.EtapaRetorno, "ID_EtapaRetorno");
            References(x => x.UsuarioAssinatura, "ID_UsuarioAssinatura");
            References(x => x.FileServer).Column("ID_FileServer").Cascade.All();
            References(x => x.NomeFinalizacaoEtapa, "ID_NomeFinalizacaoEtapa");
            References(x => x.NomeReprovacaoEtapa, "ID_NomeReprovacaoEtapa");

            Map(x => x.Nome).Column("NM_Etapa");
            Map(x => x.RequerAprovacao).Column("IN_RequerAprovacao");
            Map(x => x.Ordem).Column("VL_Ordem");
            Map(x => x.PrimeiraEtapa).Column("IN_PrimeiraEtapa");
            Map(x => x.NomeBotaoFinalizacao).Column("TX_NomeBotaoFinalizacao");
            Map(x => x.NomeBotaoReprovacao).Column("TX_NomeBotaoReprovacao");
            Map(x => x.NomeBotaoAjuste).Column("TX_NomeBotaoAjuste");
            //Map(x => x.PodeSerAprovadoAssessor).Column("IN_PodeSerAprovadoAssessor");
            Map(x => x.NotificaDiretorAnalise).Column("IN_NotificarDiretorAnalise");

            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
            Map(x => x.VisivelImpressao).Column("IN_VisivelImpressao");
            Map(x => x.PodeSerAprovadoChefeGabinete).Column("IN_PodeSerAprovadoChefeGabinete");

            Map(x => x.PrazoEncaminhamento).Column("VL_PrazoEncaminhamento");
            Map(x => x.NotificarNucleo).Column("IN_NotificarNucleo");

            Map(x => x.PodeSerReprovada).Column("IN_PodeSerReprovada");

            HasMany(x => x.ListaEtapaResposta).KeyColumn("ID_Etapa");
            HasMany(x => x.ListaCampos).KeyColumn("ID_Etapa").AsBag().Inverse().Cascade.All();

            HasMany(x => x.Permissoes).KeyColumn("ID_Etapa").AsBag()
                .Not.KeyNullable()
                .Cascade.Delete()
                .Cascade.SaveUpdate();

            HasMany(x => x.PermissoesNucleo).KeyColumn("ID_Etapa").AsBag().Cascade.Delete();
        }
    }
}
