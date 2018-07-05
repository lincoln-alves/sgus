using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class CapacitacaoMap : ClassMap<Capacitacao>
    {
        public CapacitacaoMap()
        {
            Table("TB_Capacitacao");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_Capacitacao");
            References(x => x.Programa).Column("ID_Programa");
            Map(x => x.Nome).Column("NM_Capacitacao");
            Map(x => x.Descricao).Column("TX_Descricao");
            Map(x => x.DataInicio).Column("DT_Inicio");
            Map(x => x.DataFim).Column("DT_Fim");
            Map(x => x.DataInicioInscricao).Column("DT_InicioInscricao");
            Map(x => x.DataFimInscricao).Column("DT_FimInscricao");
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            References(x => x.Certificado).Column("ID_Certificado");
            Map(x => x.PermiteCancelarMatricula).Column("IN_PermiteCancelarMatricula");
            Map(x => x.PermiteAlterarSituacao).Column("IN_PermiteAlterarSituacao");
            Map(x => x.PermiteMatriculaPeloGestor).Column("IN_PermiteMatriculaPeloGestor");
            HasMany(x => x.ListaModulos).KeyColumn("ID_Capacitacao").AsBag().Inverse().Cascade.None();
            HasMany(x => x.ListaTurmas).KeyColumn("ID_Capacitacao").AsBag().Inverse().Cascade.None();
            Map(x => x.IdNodePortal).Column("ID_Node_Portal");
        }
    }
}
