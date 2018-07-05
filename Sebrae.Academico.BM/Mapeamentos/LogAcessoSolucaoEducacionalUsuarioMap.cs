using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class LogAcessoSolucaoEducacionalUsuarioMap : ClassMap<LogAcessoSolucaoEducacionalUsuario>
    {
        public LogAcessoSolucaoEducacionalUsuarioMap()
        {
            Table("LG_AcessoSolucaoEducacionalUsuario");
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_AcessoSolucaoEducacionalUsuario");
            Map(x => x.ID_AcessoSolucaoEducacional).Column("ID_AcessoSolucaoEducacional");
            Map(x => x.ID_SolucaoEducacional).Column("ID_SolucaoEducacional");
            Map(x => x.ID_Oferta).Column("ID_Oferta");
            Map(x => x.ID_Turma).Column("ID_Turma");
            Map(x => x.ID_Usuario).Column("ID_Usuario");
            Map(x => x.DataAcesso).Column("DT_Acesso");
        }
    }
}
