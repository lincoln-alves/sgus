using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class TemplateMap : ClassMap<Template>
    {
        public TemplateMap()
        {
            Table("TB_Template");
            LazyLoad();
            Id(x => x.ID, "ID_Template").GeneratedBy.Assigned();
            Map(x => x.TextoTemplate).Column("TX_Template").Length(2147483647);
            Map(x => x.Assunto).Column("TX_Assunto");
            Map(x => x.DescricaoTemplate).Column("DE_Template");
            Map(x => x.HashTag).Column("TX_HashTag");

            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
           
        }
    }
}