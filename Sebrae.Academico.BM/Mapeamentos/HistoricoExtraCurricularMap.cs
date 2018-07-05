using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class HistoricoExtraCurricularMap: ClassMap<HistoricoExtraCurricular>
    {
        public HistoricoExtraCurricularMap()
        {
            Table("TB_HistoricoExtraCurricular");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_HistoricoExtraCurricular");
            References(x => x.Usuario).Column("ID_Usuario");
            References(x => x.FileServer).Column("ID_FileServer").Cascade.All();
            Map(x => x.SolucaoEducacionalExtraCurricular, "NM_SolucaoEducacionalExtraCurricular");
            Map(x => x.TextoAtividade).Column("TX_Atividade");
            Map(x => x.DataInicioAtividade).Column("DT_InicioAtividade");
            Map(x => x.DataFimAtividade).Column("DT_FimAtividade");
            Map(x => x.CargaHoraria).Column("QT_CargaHoraria");
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
            Map(x => x.Instituicao).Column("NM_Instituicao");
            References(x => x.FormaAquisicao).Column("ID_FormaAquisicao");
            References(x => x.Fornecedor).Column("ID_Fornecedor");
            
        }
    }
}
