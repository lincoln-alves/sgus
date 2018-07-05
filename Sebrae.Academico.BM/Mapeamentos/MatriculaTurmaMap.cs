using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class MatriculaTurmaMap: ClassMap<MatriculaTurma>
    {
        public MatriculaTurmaMap()
        {
            Table("TB_MatriculaTurma");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_MatriculaTurma");
            References(x => x.Turma).Column("ID_Turma").Cascade.None().Not.Nullable(); 
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.DataMatricula).Column("DT_Matricula").Not.Nullable();
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
            Map(x => x.DataLimite, "DT_Limite");
            Map(x => x.Nota1).Column("VL_Nota1");
            Map(x => x.Nota2).Column("VL_Nota2");
            Map(x => x.PercentualPresenca).Column("PC_Presenca");
            Map(x => x.DataTermino).Column("DT_Termino");
            Map(x => x.MediaFinal).Column("VL_MediaFinal"); //.CustomSqlType("decimal(3,2)"); //.Precision(3).Scale(2);
            Map(x => x.ValorNotaProvaOnline).Column("VL_NotaProvaOnline");
            Map(x => x.Observacao).Column("TX_Observacao").CustomType("StringClob").CustomSqlType("nvarchar(max)");
            Map(x => x.Feedback).Column("TX_Feedback").CustomType("StringClob").CustomSqlType("nvarchar(max)");
            Map(x => x.Presencas).Column("IN_Presencas");
            Map(x => x.TotalPresencas).Column("IN_TotalPresencas");
            References(x => x.MatriculaOferta).Column("ID_MatriculaOferta").NotFound.Ignore().Not.Nullable();
            HasMany(x => x.Questionarios).KeyColumn("ID_MatriculaTurma").AsBag().Inverse().Not.LazyLoad().Cascade.AllDeleteOrphan(); ;
        }
    }
}
