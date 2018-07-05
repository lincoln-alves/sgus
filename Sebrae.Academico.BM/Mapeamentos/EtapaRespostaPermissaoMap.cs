using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class EtapaRespostaPermissaoMap : ClassMap<EtapaRespostaPermissao>
    {
        public EtapaRespostaPermissaoMap()
        {
            Table("TB_EtapaRespostaPermissao");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_EtapaRespostaPermissao");
            References(x => x.EtapaResposta).Column("ID_EtapaResposta");
            References(x => x.EtapaPermissaoNucleo).Column("ID_EtapaPermissaoNucleo");
        }
    }
}
