using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class MissaoMedalhaMap : ClassMap<MissaoMedalha>
    {
        public MissaoMedalhaMap()
        {
            Table("TB_MissaoMedalha");
            LazyLoad();            
            Id(x => x.ID, "ID_MissaoMedalha").GeneratedBy.Identity();
            Map(x => x.Medalhas, "QT_Medalhas");
            References(x => x.UsuarioTrilha, "ID_UsuarioTrilha");
            References(x => x.Missao, "ID_Missao");
            Map(x => x.DataRegistro, "DT_Registro");
            
        }
    }
}
