using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{

    public sealed class CertificadoTemplateMap : ClassMap<CertificadoTemplate>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public CertificadoTemplateMap()
        {
            Table("TB_CERTIFICADOTEMPLATE");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_CERTIFICADOTEMPLATE");
            Map(x => x.Nome).Column("NM_CERTIFICADOTEMPLATE");
            Map(x => x.TextoDoCertificado).Column("TX_CERTIFICADO").Length(2147483647);
            Map(x => x.Imagem).Column("OB_IMAGEM").Length(2147483647);
            Map(x => x.DataAlteracao).Column("DT_ULTIMAATUALIZACAO");
            Map(x => x.UsuarioAlteracao).Column("NM_USUARIOATUALIZACAO");

            Map(x => x.Imagem2).Column("OB_Image2").Length(2147483647);
            Map(x => x.TextoCertificado2).Column("TX_Certificado2").Length(2147483647);
            Map(x => x.Professor).Column("IN_Professor");
            Map(x => x.Ativo).Column("IN_Ativo");
            Map(x => x.CertificadoTrilhas).Column("IN_CertificadoTrilhas");

            References<Uf>(x => x.UF).Column("ID_UF");

            //HasMany(x => x.ListaSolucaoEducacional).KeyColumn("ID_CERTIFICADOTEMPLATE").AsBag().Inverse().Cascade.None();
            HasMany(x => x.ListaTrilhaNivel).KeyColumn("ID_CERTIFICADOTEMPLATE").AsBag().Inverse().Cascade.None().Fetch.Select().LazyLoad();
            HasMany(x => x.ListaOferta).KeyColumn("ID_CERTIFICADOTEMPLATE").AsBag().Inverse().Cascade.None().Fetch.Select().LazyLoad();
            HasMany(x => x.ListaOfertaProfessor).KeyColumn("ID_CERTIFICADOTEMPLATEPROFESSOR").AsBag().Inverse().Cascade.None().Fetch.Select().LazyLoad();
            
            HasManyToMany(a => a.ListaCategoriaConteudo)
                .Table("TB_CategoriaConteudoCertificadoTemplate")
                .ParentKeyColumn("ID_CertificadoTemplate")
                .ChildKeyColumn("ID_CategoriaConteudo");
        }
    }
}


