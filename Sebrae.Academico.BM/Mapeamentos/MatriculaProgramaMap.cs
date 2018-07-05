using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{

    public sealed class MatriculaProgramaMap : ClassMap<MatriculaPrograma>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public MatriculaProgramaMap()
        {
            Table("TB_MATRICULAPROGRAMA");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_MATRICULAPROGRAMA");
            References(x => x.Programa).Column("ID_PROGRAMA").Cascade.None().Not.Nullable(); 
            References(x => x.Usuario).Column("ID_USUARIO").Cascade.None().Not.Nullable(); 
            References(x => x.UF, "ID_UF").Cascade.None().Not.Nullable();
            References(x => x.NivelOcupacional, "ID_NivelOcupacional").Cascade.None().Not.Nullable();
            Map(x => x.StatusMatricula, "ID_StatusMatricula").CustomType<enumStatusMatricula>().Not.Nullable();
            Map(x => x.DataAlteracao, "DT_UltimaAtualizacao").Nullable();
            Map(x => x.UsuarioAlteracao, "NM_UsuarioAtualizacao").Nullable();

            Map(x => x.DataInicio).Column("DT_Inicio").Not.Nullable();
            Map(x => x.DataFim).Column("DT_Fim");
        }
    }
}
