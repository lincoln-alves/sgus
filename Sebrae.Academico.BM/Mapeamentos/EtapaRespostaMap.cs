using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class EtapaRespostaMap : ClassMap<EtapaResposta>
    {
        public EtapaRespostaMap()
        {
            Table("TB_EtapaResposta");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_EtapaResposta");
            References(x => x.Etapa).Column("ID_Etapa");
            References(x => x.ProcessoResposta).Column("ID_ProcessoResposta");
            References(x => x.Analista).Column("ID_Analista");
            References(x => x.Assessor).Column("ID_Assessor");
            References(x => x.CargoAnalista).Column("ID_Cargo");
            Map(x => x.Status).Column("VL_Status");
            Map(x => x.Ativo).Column("IN_Ativo");
            Map(x => x.DataPreenchimento).Column("DT_DataPreenchimento");
            Map(x => x.PrazoEncaminhamento).Column("DT_PrazoEncaminhamento");
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
            //HasMany(x => x.ListaEtapas).KeyColumn("ID_Processo").AsBag().Inverse().Cascade.None();

            HasMany(x => x.PermissoesNucleoEtapaResposta).KeyColumn("ID_EtapaResposta").AsBag().Cascade.All();
            HasMany(x => x.ListaEtapaEncaminhamentoUsuario).KeyColumn("ID_EtapaResposta").AsBag().Cascade.All();
            HasMany(x => x.ListaCampoResposta).KeyColumn("ID_EtapaResposta");
        }
    }
}
