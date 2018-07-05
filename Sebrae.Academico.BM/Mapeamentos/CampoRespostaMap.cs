using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class CampoRespostaMap : ClassMap<CampoResposta>
    {
        public CampoRespostaMap()
        {
            Table("TB_CampoResposta");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_CampoResposta");
            References(x => x.Campo).Column("ID_Campo");
            References(x => x.EtapaResposta).Column("ID_EtapaResposta");
            Map(x => x.Resposta).Column("VL_Resposta").CustomSqlType("nvarchar(MAX)").Length(int.MaxValue);

            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
        }
    }
}
