using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.BM.Mapeamentos.Portal
{
    public class LogPortalMap : ClassMap<LogAcoesPortal>
    {
        public LogPortalMap()
        {
            Table("access_log");
            LazyLoad();
            Id(x => x.ID).Column("id_log");
            Map(x => x.ID_Usuario).Column("id_usuario");
            Map(x => x.Url).Column("url");
            Map(x => x.Acao).Column("acao");
            Map(x => x.IP).Column("ip");
            Map(x => x.Datacesso).Column("created").CustomSqlType("timestamp");
        }
    }
}
