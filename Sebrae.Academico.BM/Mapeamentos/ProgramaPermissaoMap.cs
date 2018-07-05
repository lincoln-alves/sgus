using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class ProgramaPermissaoMap : ClassMap<ProgramaPermissao>
    {
        public ProgramaPermissaoMap()
        {
            Table("TB_PROGRAMAPERMISSAO");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_ProgramaPermissao");
            References(x => x.Programa).Column("ID_Programa")//.Cascade.Delete() //.Cascade.SaveUpdate().Cascade.Delete();
                .ForeignKey("FK_Programa_ProgramaPermissao"); //.Cascade.Delete(); //.Cascade.SaveUpdate().Cascade.Delete();            
            
            References(x => x.NivelOcupacional).Column("ID_NivelOcupacional")//.Cascade.Delete()
                .ForeignKey("FK_NivelOcupacional_ProgramaPermissao"); //.Cascade.Delete(); //.Cascade.SaveUpdate().Cascade.Delete();            
            
            References(x => x.Uf).Column("ID_UF")//.Cascade.Delete() //.Cascade.SaveUpdate().Cascade.Delete();
                .ForeignKey("FK_UF_ProgramaPermissao"); //.Cascade.Delete(); //.Cascade.SaveUpdate().Cascade.Delete();            

            References(x => x.Perfil).Column("ID_PERFIL")//.Cascade.Delete() //.Cascade.SaveUpdate().Cascade.Delete();
                .ForeignKey("FK_Perfil_ProgramaPermissao"); //.Cascade.Delete(); //.Cascade.SaveUpdate().Cascade.Delete();            

            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
        }
    }
}
