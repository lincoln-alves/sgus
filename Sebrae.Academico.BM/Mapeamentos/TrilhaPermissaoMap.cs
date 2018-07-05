using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{

    public sealed class TrilhaPermissaoMap : ClassMap<TrilhaPermissao>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public TrilhaPermissaoMap()
        {
            Table("TB_TRILHAPERMISSAO");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_TRILHAPERMISSAO");
            References(x => x.Trilha).Column("ID_TRILHA"); //.ForeignKey("FK_Programa_ProgramaPermissao"); 
            References(x => x.Perfil).Column("ID_PERFIL");
            References(x => x.NivelOcupacional).Column("ID_NIVELOCUPACIONAL");
            References(x => x.Uf).Column("ID_UF");
            Map(x => x.DataAlteracao).Column("DT_ULTIMAATUALIZACAO");
            Map(x => x.UsuarioAlteracao).Column("NM_USUARIOATUALIZACAO");
        }

    }
}
