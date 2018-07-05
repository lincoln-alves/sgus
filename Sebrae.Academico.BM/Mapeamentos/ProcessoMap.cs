using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class ProcessoMap : ClassMap<Processo>
    {
        public ProcessoMap()
        {
            Table("TB_Processo");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_Processo");

            Map(x => x.Ativo).Column("IN_Ativo");
            Map(x => x.Tipo).Column("IN_Tipo").CustomType<enumTipoProcesso>();
            Map(x => x.Mensal).Column("IN_Mensal");
            Map(x => x.DiaInicio).Column("DT_DiaInicio");
            Map(x => x.DiaFim).Column("DT_DiaFim");

            Map(x => x.Nome).Column("NM_Processo").Length(150);

            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
            
            References(x => x.Uf).Column("ID_UF");
            
            HasMany(x=>x.ListaProcessoResposta).KeyColumn("ID_Processo");
            HasMany(x => x.ListaEtapas).KeyColumn("ID_Processo").AsBag().Inverse().Cascade.All();//.Cascade.None();
        }
    }
}