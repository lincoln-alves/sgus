using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{

    public sealed class ProgramaSolucaoEducacionalMap : ClassMap<ProgramaSolucaoEducacional>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public ProgramaSolucaoEducacionalMap()
        {
            Table("TB_PROGRAMASOLUCAOEDUCACIONAL");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_PROGRAMASOLUCAOEDUCACIONAL");
            References(x => x.Programa).Column("ID_PROGRAMA");
            References(x => x.SolucaoEducacional).Column("ID_SOLUCAOEDUCACIONAL");
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
        }

    }
}

