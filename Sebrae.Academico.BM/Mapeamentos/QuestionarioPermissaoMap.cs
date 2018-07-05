using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class QuestionarioPermissaoMap : ClassMap<QuestionarioPermissao>
    {
        public QuestionarioPermissaoMap()
        {
            Table("TB_QuestionarioPermissao");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_QuestionarioPermissao");
            References(x => x.Questionario).Column("ID_Questionario"); 
            References(x => x.NivelOcupacional).Column("ID_NivelOcupacional");
            References(x => x.Uf).Column("ID_UF");
            References(x => x.Perfil).Column("ID_Perfil");
            References(x => x.FormaAquisicao).Column("ID_FormaAquisicao");
            Map(x => x.DataAlteracao, "DT_UltimaAtualizacao").Nullable();
            Map(x => x.UsuarioAlteracao, "NM_UsuarioAtualizacao").Nullable();
        }
    }
}
