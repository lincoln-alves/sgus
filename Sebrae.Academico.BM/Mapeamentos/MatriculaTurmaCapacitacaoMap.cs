using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Dominio.Classes;
namespace Sebrae.Academico.BM.Mapeamentos
{
    public class MatriculaTurmaCapacitacaoMap : ClassMap<MatriculaTurmaCapacitacao>
    {
        public MatriculaTurmaCapacitacaoMap()
        {
            Table("TB_MatriculaTurmaCapacitacao");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_MatriculaTurmaCapacitacao");
            References(x => x.TurmaCapacitacao).Column("ID_TurmaCapacitacao").Not.Nullable(); 
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.DataMatricula).Column("DT_Matricula").Not.Nullable();
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");

            References(x => x.MatriculaCapacitacao).Column("ID_MatriculaCapacitacao");            
        }
    }
}
