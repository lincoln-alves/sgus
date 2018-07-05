//using FluentNHibernate.Mapping;
//using Sebrae.Academico.Dominio.Classes;

//namespace Sebrae.Academico.BM.Mapeamentos
//{
//    public class IndiceImportanciaMap : ClassMap<IndiceImportancia>
//    {
//        public IndiceImportanciaMap()
//        {
//            Table("TB_IndiceImportancia");
//            LazyLoad();
//            Id(x => x.ID).GeneratedBy.Identity().Column("ID_IndiceImportancia");
//            References(x => x.Usuario, "ID_Usuario");
//            References(x => x.TrilhaNivel, "ID_TrilhaNivel");
//            References(x => x.TrilhaTopicoTematico, "ID_TrilhaTopicoTematico");
//            References(x => x.Objetivo, "ID_Objetivo");
//            Map(x => x.Pre).Column("IN_Pre");
//            Map(x => x.ValorImportancia).Column("VL_Importancia");
//            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
//            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
//        }
//    }
//}
