using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class SolucaoEducacionalPermissaoMap : ClassMap<SolucaoEducacionalPermissao>
    {
        public SolucaoEducacionalPermissaoMap()
        {
            Table("TB_SOLUCAOEDUCACIONALPERMISSAO");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_SolucaoEducacionalPermissao");
            References(x => x.SolucaoEducacional).Column("ID_SolucaoEducacional").LazyLoad().Not.Nullable();
            References(x => x.Uf).Column("ID_UF").LazyLoad();
            References(x => x.NivelOcupacional).Column("ID_NivelOcupacional").LazyLoad();
            References(x => x.Perfil).Column("ID_Perfil").LazyLoad();
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
            Map(x => x.QuantidadeVagasPorEstado).Column("QT_VagasPorUF");
        }
    }
}
