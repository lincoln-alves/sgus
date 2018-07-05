using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{

    public sealed class MatriculaCapacitacaoMap : ClassMap<MatriculaCapacitacao>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public MatriculaCapacitacaoMap()
        {
            Table("TB_MATRICULACAPACITACAO");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_MATRICULACAPACITACAO");
            References(x => x.Capacitacao).Column("ID_CAPACITACAO").Not.Nullable();
            References(x => x.Usuario).Column("ID_USUARIO").Not.Nullable();
            References(x => x.UF, "ID_UF").Not.Nullable();
            References(x => x.NivelOcupacional, "ID_NivelOcupacional").Not.Nullable();
            Map(x => x.StatusMatricula, "ID_StatusMatricula").CustomType<enumStatusMatricula>();
            Map(x => x.DataAlteracao, "DT_UltimaAtualizacao").Nullable();
            Map(x => x.UsuarioAlteracao, "NM_UsuarioAtualizacao").Nullable();
            Map(x => x.DataGeracaoCertificado).Column("DT_GeracaoCertificado");
            Map(x => x.CDCertificado).Column("CD_Certificado");
            Map(x => x.CertificadoEmitidoPorGestor).Column("ID_CertificadoEmitidoPorGestor");
            Map(x => x.DataInicio).Column("DT_Inicio").Not.Nullable();
            Map(x => x.DataFim).Column("DT_Fim");

            //References(x => x.MatriculaTurmaCapacitacao).Column("ID_MATRICULACAPACITACAO").LazyLoad();
            //HasMany(x => x.ListaMatriculaTurmaCapacitacao).KeyColumn("ID_MatriculaCapacitacao");
            HasMany(x => x.ListaMatriculaTurmaCapacitacao).LazyLoad().KeyColumn("ID_MatriculaCapacitacao");
        }
    }
}
