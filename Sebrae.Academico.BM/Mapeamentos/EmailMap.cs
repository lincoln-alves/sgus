using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{

    public sealed class EmailMap : ClassMap<Email>
    {
        public EmailMap()
        {
            Table("TB_Email");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_Email");
            References(x => x.Usuario).Column("ID_Usuario").UniqueKey("UQ_Email").Not.Nullable();
            References(x => x.EmailEnvio).Column("ID_EmailEnvio").UniqueKey("UQ_Email").Not.Nullable();
            Map(x => x.Assunto).Column("TX_Assunto").Not.Nullable();
            Map(x => x.TextoEmail).Column("TX_Email").Not.Nullable();
            Map(x => x.Enviado).Column("IN_Enviado").Not.Nullable();
            Map(x => x.DataGeracao).Column("DT_Geracao").Not.Nullable();
            Map(x => x.DataEnvio).Column("DT_Envio");
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");

        }
    }
}
