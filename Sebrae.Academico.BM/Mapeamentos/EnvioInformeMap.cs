using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class EnvioInformeMap : ClassMap<EnvioInforme>
    {
        public EnvioInformeMap()
        {
            Table("TB_EnvioInforme");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_EnvioInforme");
            Map(x => x.DataEnvio).Column("DT_Envio");

            References(x => x.Informe).Column("ID_Informe");
            References(x => x.Usuario).Column("ID_Usuario");

            HasManyToMany(a => a.Perfis)
                .Table("TB_EnvioInformePerfil")
                .ParentKeyColumn("ID_EnvioInforme")
                .ChildKeyColumn("ID_Perfil");

            HasManyToMany(a => a.NiveisOcupacionais)
                .Table("TB_EnvioInformeNivelOcupacional")
                .ParentKeyColumn("ID_EnvioInforme")
                .ChildKeyColumn("ID_NivelOcupacional");

            HasManyToMany(a => a.Ufs)
                .Table("TB_EnvioInformeUF")
                .ParentKeyColumn("ID_EnvioInforme")
                .ChildKeyColumn("ID_UF");
        }
    }
}
