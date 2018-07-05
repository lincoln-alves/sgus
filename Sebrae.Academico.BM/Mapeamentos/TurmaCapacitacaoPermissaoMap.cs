using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class TurmaCapacitacaoPermissaoMap : ClassMap<TurmaCapacitacaoPermissao>
    {
        public TurmaCapacitacaoPermissaoMap()
        {
            Table("TB_TurmaCapacitacaoPermissao");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_TurmaCapacitacaoPermissao");
            References(x => x.TurmaCapacitacao).Column("ID_TurmaCapacitacao").LazyLoad();
            References(x => x.Uf).Column("ID_UF").LazyLoad();
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
            Map(x => x.QuantidadeVagasPorEstado).Column("QT_VagasPorUF");
        }
    }
}
